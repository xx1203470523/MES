using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 包装（打开）
    /// </summary>
    [Job("包装", JobTypeEnum.Standard)]
    public class PackageOpenJobService : IJobService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IManuContainerBarcodeRepository _manuContainerBarcodeRepository;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<PackageOpenRequestBo> _validationRepairJob;
        /// <summary>
        /// 构造函数 
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        public PackageOpenJobService(AbstractValidator<PackageOpenRequestBo> validationRepairJob,
            IManuContainerBarcodeRepository manuContainerBarcodeRepository)
        {
            _validationRepairJob = validationRepairJob;
            _manuContainerBarcodeRepository = manuContainerBarcodeRepository;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <param name="proxy"></param> 
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<PackageOpenRequestBo>() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10103));
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
            var bo = param.ToBo<PackageOpenRequestBo>() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10103));
            var defaultDto = new PackageOpenResponseBo();
            string success = "true";
            var manuContainerBarcodeEntity = await param.Proxy!.GetValueAsync(_manuContainerBarcodeRepository.GetByIdAsync, bo.ContainerId);
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
                manuContainerBarcodeEntity.UpdatedBy = bo.UserName;
                manuContainerBarcodeEntity.UpdatedOn = HymsonClock.Now();
                defaultDto.Message = $"打开成功！";
            }
            else
            {
                success = "false";
                defaultDto.Message = $"该容器已经打开！";
            }

            defaultDto.Content?.Add("Operation", ManuContainerPackagJobReturnTypeEnum.JobManuPackageOpenService.ParseToInt().ToString());
            defaultDto.Content?.Add("Status", $"{status}".ToString());
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

            if (obj is not PackageOpenResponseBo data) return responseBo;

            var rows = await _manuContainerBarcodeRepository.UpdateStatusAsync(data.ManuContainerBarcode);

            return new JobResponseBo { Content = data.Content!, Message = data.Message, Rows = rows, Time = data.Time };
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
