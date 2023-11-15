using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Utils;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 维修开始
    /// </summary>
    [Job("维修开始", JobTypeEnum.Standard)]
    public class RepairStartJobService : IJobService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<RepairStartJobService> _logger;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（在制）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 仓储接口（工艺路线工序节点）
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;

        /// <summary>
        /// 仓储接口（工艺路线工序连线）
        /// </summary>
        private readonly IProcProcessRouteDetailLinkRepository _procProcessRouteDetailLinkRepository;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<RepairStartRequestBo> _validationRepairJob;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="validationRepairJob"></param>
        /// <param name="masterDataService"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        public RepairStartJobService(ILogger<RepairStartJobService> logger,
            IProcProcedureRepository procProcedureRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            AbstractValidator<RepairStartRequestBo> validationRepairJob,
            IMasterDataService masterDataService,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository)
        {
            _logger = logger;
            _procProcedureRepository = procProcedureRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _validationRepairJob = validationRepairJob;
            _masterDataService = masterDataService;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <param name="proxy"></param> 
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<RepairStartRequestBo>() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10103));

            // 验证DTO
            await _validationRepairJob.ValidateAndThrowAsync(bo);
            await Task.CompletedTask;
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
            var bo = param.ToBo<RepairStartRequestBo>() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10103));
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
            var sfcProduceEntity = sfcProduceEntitys.FirstOrDefault();
            // 如果工序对应不上
            if (sfcProduceEntity?.ProcedureId != bo.ProcedureId)
            {
                var processRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetProcessRouteDetailLinksByProcessRouteIdAsync(sfcProduceEntity!.ProcessRouteId)
                 ?? throw new CustomerValidationException(nameof(ErrorCode.MES18213));

                var processRouteDetailNodes = await _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(sfcProduceEntity.ProcessRouteId)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES18208));

                // 判断上一个工序是否是随机工序
                var IsRandomPreProcedure = await bo.Proxy.GetValueAsync(_masterDataService.IsRandomPreProcedureAsync, new ManuRouteProcedureWithInfoBo
                {
                    ProcessRouteDetailLinks = processRouteDetailLinks,
                    ProcessRouteDetailNodes = processRouteDetailNodes,
                    ProcessRouteId = sfcProduceEntity.ProcessRouteId,
                    ProcedureId = bo.ProcedureId
                });
                if (!IsRandomPreProcedure) throw new CustomerValidationException(nameof(ErrorCode.MES16308));

                // 将SFC对应的工序改为当前工序
                sfcProduceEntity.ProcessRouteId = bo.ProcedureId;
            }

            // 获取当前工序信息
            var procedureEntity = await _procProcedureRepository.GetByIdAsync(bo.ProcedureId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16352));

            // 读取工序关联的资源
            var resourceIds = await param.Proxy!.GetValueAsync(_masterDataService.GetProcResourceIdByProcedureIdAsync, bo.ProcedureId);
            if (resourceIds == null || !resourceIds.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16355)).WithData("ProcedureCode", procedureEntity.Code);
            }

            // 校验工序和资源是否对应
            if (!resourceIds.Any(a => a == bo.ResourceId))
            {
                _logger.LogWarning($"工序{bo.ProcedureId}和资源{bo.ResourceId}不对应");
                throw new CustomerValidationException(nameof(ErrorCode.MES16317));
            }

            // 当前工序是否是排队状态
            if (sfcProduceEntity.Status == SfcStatusEnum.Activity)
            {
                // 如果状态已经为活动中，就直接返回成功
                return new RepairStartResponseBo();
            }

            // 获取生产工单（附带工单状态校验）
            _ = await bo.Proxy!.GetValueAsync(_masterDataService.GetProduceWorkOrderByIdAsync, new WorkOrderIdBo { WorkOrderId = sfcProduceEntity.WorkOrderId });

            // 检查是否测试工序
            if (procedureEntity?.Type == ProcedureTypeEnum.Test)
            {
                // 超过复投次数，标识为NG
                if (sfcProduceEntity.RepeatedCount > procedureEntity.Cycle) throw new CustomerValidationException(nameof(ErrorCode.MES16036));
                sfcProduceEntity.RepeatedCount++;
            }
            //更新工序资源
            var updateProcedureAndStatus = new UpdateProcedureAndResourceCommand()
            {
                ProcedureId = sfcProduceEntity.ProcedureId,
                ResourceId = bo.ResourceId,
                Sfcs = bo.SFCs.ToArray(),
                SiteId = bo.SiteId,
                UserId = bo.UserName,
                UpdatedOn = HymsonClock.Now()
            };
            //返回
            return new RepairStartResponseBo() { UpdateResourceCommand = updateProcedureAndStatus };
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not RepairStartResponseBo data) return responseBo;

            if (data == null || data.UpdateResourceCommand == null)
            {
                return responseBo;
            }

            //事务入库

            responseBo.Rows += await _manuSfcProduceRepository.UpdateProcedureAndResourceRangeAsync(data.UpdateResourceCommand);
            return responseBo;
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
