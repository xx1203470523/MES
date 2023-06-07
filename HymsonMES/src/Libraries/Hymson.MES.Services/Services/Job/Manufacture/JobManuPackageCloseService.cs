using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Integrated.InteContainer;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 容器包装开启操作
    /// </summary>
    public class JobManuPackageCloseService : IJobManufactureService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;
        private readonly IManuContainerBarcodeRepository _manuContainerBarcodeRepository;
        private readonly IManuContainerPackRepository _manuContainerPackRepository;
        private readonly IInteContainerRepository _inteContainerRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuContainerBarcodeRepository"></param>
        /// <param name="manuContainerPackRepository"></param>
        /// <param name="inteContainerRepository"></param>
        public JobManuPackageCloseService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonService manuCommonService, IManuContainerBarcodeRepository manuContainerBarcodeRepository, IManuContainerPackRepository manuContainerPackRepository, IInteContainerRepository inteContainerRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonService = manuCommonService;
            _manuContainerBarcodeRepository = manuContainerBarcodeRepository;
            _manuContainerPackRepository = manuContainerPackRepository;
            _inteContainerRepository = inteContainerRepository;
        }


        /// <summary>
        /// 验证参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync(Dictionary<string, string>? param)
        {
            if (param == null || param.ContainsKey("ContainerId") == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16312));
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 执行（关闭）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<JobResponseDto> ExecuteAsync(Dictionary<string, string>? param)
        {
            var defaultDto = new JobResponseDto { };

            var bo = new ManufactureContainerBo
            {
                ContainerId = param["ContainerId"].ParseToLong()
            };
            string success = "true";
            var manuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetByIdAsync(bo.ContainerId);
            if (manuContainerBarcodeEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16702));
            }
            int status = 2;//1打开，2关闭
            defaultDto.Content?.Add("Operation", ManuContainerPackagJobReturnTypeEnum.JobManuPackageCloseService.ParseToInt().ToString());
            defaultDto.Content?.Add("Status", $"{status}".ToString());
            //当前状态不等于修改状态
            if (manuContainerBarcodeEntity.Status != status)
            {

                //关闭操作必须要装箱数量达到最小包装数
                var container = await _inteContainerRepository.GetByIdAsync(manuContainerBarcodeEntity.ContainerId);
                //查询已包装数
                var containerPacks = await _manuContainerPackRepository.GetByContainerBarCodeIdAsync(manuContainerBarcodeEntity.Id, _currentSite.SiteId.Value);
                if (containerPacks.Count() < container.Minimum)
                {
                    success = "false";
                    defaultDto.Message = ErrorCode.MES16723;
                    defaultDto.Content?.Add("Success", success);
                    return defaultDto;
                }
                //修改容器状态
                manuContainerBarcodeEntity.Status = status;
                manuContainerBarcodeEntity.UpdatedBy = _currentUser.UserName;
                manuContainerBarcodeEntity.UpdatedOn = HymsonClock.Now();
                await _manuContainerBarcodeRepository.UpdateStatusAsync(manuContainerBarcodeEntity);
                defaultDto.Message = $"关闭成功！";
            }
            else
            {
                success = "false";
                defaultDto.Message = $"该容器已经关闭！";
            }
            defaultDto.Content?.Add("Success", success);

            return defaultDto;
        }

    }
}
