using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Process;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using MySqlX.XDevAPI.Common;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 维修开始
    /// </summary>
    [Job("维修开始", JobTypeEnum.Standard)]
    public class RepairStartJobService : IJobService
    {

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（在制）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

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
        /// <param name="manuCommonService"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        public RepairStartJobService(IManuCommonService manuCommonService,
            IProcProcedureRepository procProcedureRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            AbstractValidator<RepairStartRequestBo> validationRepairJob,
            IMasterDataService masterDataService,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository)
        {
            _manuCommonService = manuCommonService;
            _procProcedureRepository = procProcedureRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
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
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<RepairStartRequestBo>() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10103));
            // 获取生产条码信息
            var sfcProduceEntitys = await param.Proxy.GetValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, new MultiSFCBo { SFCs = bo.SFCs, SiteId = bo.SiteId });
            if (sfcProduceEntitys == null || !sfcProduceEntitys.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16306));
            }
            if (sfcProduceEntitys.GroupBy(it => it.ProcedureId).Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16330));
            };
            if (sfcProduceEntitys.GroupBy(it => it.ProcedureId).Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16331));
            };
            var sfcProduceEntity = sfcProduceEntitys.FirstOrDefault();
            // 如果工序对应不上
            if (sfcProduceEntity?.ProcedureId != bo.ProcedureId)
            {
                var processRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetProcessRouteDetailLinksByProcessRouteIdAsync(sfcProduceEntity.ProcessRouteId)
                 ?? throw new CustomerValidationException(nameof(ErrorCode.MES18213));

                var processRouteDetailNodes = await _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(sfcProduceEntity.ProcessRouteId)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES18208));

                // 判断上一个工序是否是随机工序
                var IsRandomPreProcedure = await param.Proxy.GetValueAsync(async parameters =>
                {
                    var processRouteDetailLinks = (IEnumerable<ProcProcessRouteDetailLinkEntity>)parameters[0];
                    var processRouteDetailNodes = (IEnumerable<ProcProcessRouteDetailNodeEntity>)parameters[1];
                    var processRouteId = (long)parameters[2];
                    var procedureId = (long)parameters[3];
                    return await _masterDataService.IsRandomPreProcedureAsync(processRouteDetailLinks, processRouteDetailNodes, processRouteId, procedureId);
                }, new object[] { processRouteDetailLinks, processRouteDetailNodes, sfcProduceEntity.ProcessRouteId, bo.ProcedureId });
                if (IsRandomPreProcedure == false) throw new CustomerValidationException(nameof(ErrorCode.MES16308));

                // 将SFC对应的工序改为当前工序
                sfcProduceEntity.ProcessRouteId = bo.ProcedureId;
            }

            // 校验工序和资源是否对应
            var resourceIds = await param.Proxy.GetValueAsync(_masterDataService.GetProcResourceIdByProcedureIdAsync, bo.ProcedureId);
            if (resourceIds == null || resourceIds.Any(a => a == bo.ResourceId) == false) throw new CustomerValidationException(nameof(ErrorCode.MES16317));



            // 当前工序是否是排队状态
            if (sfcProduceEntity.Status == SfcProduceStatusEnum.Activity)
            {
                // 如果状态已经为活动中，就直接返回成功
                return new RepairStartResponseBo();
            }

            // 获取生产工单（附带工单状态校验）
            _ = await param.Proxy.GetValueAsync(async parameters =>
            {
                long workOrderId = (long)parameters[0];
                bool isVerifyActivation = parameters.Length <= 1 || (bool)parameters[1];
                return await _masterDataService.GetProduceWorkOrderByIdAsync(workOrderId, isVerifyActivation);
            }, new object[] { sfcProduceEntity.WorkOrderId, true });

            // 获取当前工序信息
            var procedureEntity = await param.Proxy.GetValueAsync(_procProcedureRepository.GetByIdAsync, sfcProduceEntity.ProcedureId);

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
        public async Task<int> ExecuteAsync(object obj)
        {
            int rows = 0;
            if (obj is not RepairStartResponseBo data) return rows;
            if (data == null || data.UpdateResourceCommand == null)
            {
                return rows;
            }

            //事务入库
            //using var trans = TransactionHelper.GetTransactionScope();

            rows += await _manuSfcProduceRepository.UpdateProcedureAndResourceRangeAsync(data.UpdateResourceCommand);

            //trans.Complete();
            return rows;
        }

    }
}
