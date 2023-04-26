using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 容器包装关闭操作
    /// </summary>
    public class JobManuPackageOpenService : IJobManufactureService
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


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuContainerBarcodeRepository"></param>
        public JobManuPackageOpenService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonService manuCommonService, IManuContainerBarcodeRepository manuContainerBarcodeRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonService = manuCommonService;
            _manuContainerBarcodeRepository = manuContainerBarcodeRepository;
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
            int status = 1;//1打开，2关闭
            //当前状态不等于修改状态
            if (manuContainerBarcodeEntity.Status != status)
            {
                //修改容器状态
                manuContainerBarcodeEntity.Status = status;
                manuContainerBarcodeEntity.UpdatedBy = _currentUser.UserName;
                manuContainerBarcodeEntity.UpdatedOn = HymsonClock.Now();
                await _manuContainerBarcodeRepository.UpdateStatusAsync(manuContainerBarcodeEntity);
                defaultDto.Message = $"打开成功！";
            }
            else
            {
                success = "false";
                defaultDto.Message = $"该容器已经打开！";
            }

            defaultDto.Content?.Add("Operation", ManuContainerPackagJobReturnTypeEnum.JobManuPackageCloseService.ParseToInt().ToString());
            defaultDto.Content?.Add("Status", $"{status}".ToString());
            defaultDto.Content?.Add("Success", success);
            return defaultDto;
        }

    }
}
