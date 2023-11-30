using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 包装（关闭）
    /// </summary>
    [Job("包装（关闭）", JobTypeEnum.Standard)]
    public class PackageCloseJobService : IJobService
    {
        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IManuContainerBarcodeRepository _manuContainerBarcodeRepository;

        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<PackageCloseRequestBo> _validationRepairJob;
        /// <summary>
        /// 构造函数 
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        public PackageCloseJobService(
            AbstractValidator<PackageCloseRequestBo> validationRepairJob,
            IManuContainerBarcodeRepository manuContainerBarcodeRepository, ILocalizationService localizationService)
        {
            _validationRepairJob = validationRepairJob;
            _manuContainerBarcodeRepository = manuContainerBarcodeRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <param name="proxy"></param> 
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<PackageCloseRequestBo>() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10103));

            // 验证DTO
            await _validationRepairJob.ValidateAndThrowAsync(bo);
        }


        /// <summary>
        /// 执行前节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<PackageCloseRequestBo>() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10103));

            var defaultDto = new PackageCloseResponseBo { };

            string success = "true";
            var manuContainerBarcodeEntity = await param.Proxy!.GetValueAsync(_manuContainerBarcodeRepository.GetByIdAsync, bo.ContainerId);
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
                manuContainerBarcodeEntity.Status = status;
                manuContainerBarcodeEntity.UpdatedBy = bo.UserName;
                manuContainerBarcodeEntity.UpdatedOn = HymsonClock.Now();
                defaultDto.ManuContainerBarcode = manuContainerBarcodeEntity;

                defaultDto.Message = _localizationService.GetResource(nameof(ErrorCode.MES16344));
            }
            else
            {
                success = "false";

                defaultDto.Message = _localizationService.GetResource(nameof(ErrorCode.MES16345));
            }
            defaultDto.Content?.Add("Success", success);

            return defaultDto;
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not PackageCloseResponseBo data) return responseBo;

            responseBo.Rows += await _manuContainerBarcodeRepository.UpdateStatusAsync(data.ManuContainerBarcode);

            return new JobResponseBo { Content = data.Content!, Message = data.Message, Rows = responseBo.Rows, Time = data.Time };
        }


        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }
    }
}