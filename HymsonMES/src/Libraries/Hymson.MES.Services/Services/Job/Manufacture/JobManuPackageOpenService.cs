using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Manufacture;
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
        private readonly IManuCommonOldService _manuCommonOldService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IManuContainerBarcodeRepository _manuContainerBarcodeRepository;

        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuCommonOldService"></param>
        /// <param name="manuContainerBarcodeRepository"></param>
        public JobManuPackageOpenService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonOldService manuCommonOldService, IManuContainerBarcodeRepository manuContainerBarcodeRepository,ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonOldService = manuCommonOldService;
            _manuContainerBarcodeRepository = manuContainerBarcodeRepository;
            _localizationService = localizationService;
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
            int status = 1;//1打开，2关闭
            //当前状态不等于修改状态
            if (manuContainerBarcodeEntity.Status != status)
            {
                //修改容器状态
                manuContainerBarcodeEntity.Status = status;
                manuContainerBarcodeEntity.UpdatedBy = _currentUser.UserName;
                manuContainerBarcodeEntity.UpdatedOn = HymsonClock.Now();
                await _manuContainerBarcodeRepository.UpdateStatusAsync(manuContainerBarcodeEntity);
                //defaultDto.Message = $"打开成功！";
                defaultDto.Message = _localizationService.GetResource(nameof(ErrorCode.MES16346));
            }
            else
            {
                success = "false";
                //defaultDto.Message = $"该容器已经打开！";
                defaultDto.Message = _localizationService.GetResource(nameof(ErrorCode.MES16347));
            }

            defaultDto.Content?.Add("Operation", ManuContainerPackagJobReturnTypeEnum.JobManuPackageOpenService.ParseToInt().ToString());
            defaultDto.Content?.Add("Status", $"{status}".ToString());
            defaultDto.Content?.Add("Success", success);
            return defaultDto;
        }

    }
}
