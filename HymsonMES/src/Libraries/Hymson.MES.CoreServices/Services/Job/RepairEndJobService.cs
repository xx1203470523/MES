using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 维修结束
    /// </summary>
    [Job("维修结束", JobTypeEnum.Standard)]
    public class RepairEndJobService : IJobService
    {
        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<RepairEndRequestBo> _validationRepairJob;
        /// <summary>
        /// 构造函数 
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        public RepairEndJobService(AbstractValidator<RepairEndRequestBo> validationRepairJob,
            IMasterDataService masterDataService)
        {
            _validationRepairJob = validationRepairJob;
            _masterDataService = masterDataService;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <param name="proxy"></param> 
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<RepairEndRequestBo>() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10103));

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
            var bo = param.ToBo<RepairEndRequestBo>() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10103));

            // 获取生产条码信息
            var sfcProduceEntitys = await param.Proxy!.GetValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, new MultiSFCBo { SFCs = bo.SFCs, SiteId = bo.SiteId });
            if (sfcProduceEntitys == null || !sfcProduceEntitys.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16306));
            }
            if (sfcProduceEntitys.GroupBy(it => it.ProcedureId).Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16330));
            }
            if (sfcProduceEntitys.GroupBy(it => it.ProcedureId).Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16331));
            }
            var sfcProduceActivitys = sfcProduceEntitys.Where(it => it.Status != SfcStatusEnum.Activity);
            if (sfcProduceActivitys.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16336)).WithData("SFC", string.Join(",", sfcProduceActivitys.Select(it => it.SFC).ToArray()));
            }
            //返回
            return await Task.FromResult(new JobResponseBo { });
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            return await Task.FromResult(new JobResponseBo { });
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
