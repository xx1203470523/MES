using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 条码产出上报
    /// </summary>
    [Job("条码产出上报", JobTypeEnum.Standard)]
    public class IOutputModifyService : IJobService
    {
        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        public IOutputModifyService(IMasterDataService masterDataService, IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository, IManuSfcStepRepository manuSfcStepRepository)
        {
            _masterDataService = masterDataService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
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
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return default;

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.OutStationRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
     
            if (sfcProduceEntities == null || !sfcProduceEntities.Any()) return default;

            List<ManuSfcStepEntity> manuSfcStepEnties = new();
            List<UpdateSfcProcedureQtyByIdCommand> updateSfcProcedureQtyCommands = new();
            List<UpdateManuSfcQtyByIdCommand> updateManuSfcQtyByIdCommands = new();
            foreach (var item in commonBo.OutStationRequestBos)
            {
                var sfcProduceEntity = sfcProduceEntities.FirstOrDefault(x => x.SFC == item.SFC);
                if (sfcProduceEntity == null)
                {
                    continue;
                }
                sfcProduceEntity.Qty = item.QualifiedQty + item.UnQualifiedQty;
                sfcProduceEntity.UpdatedBy = commonBo.UserName;
                sfcProduceEntity.UpdatedOn = HymsonClock.Now();

                updateSfcProcedureQtyCommands.Add(new UpdateSfcProcedureQtyByIdCommand
                {
                    Id = sfcProduceEntity.Id,
                    Qty = item.QualifiedQty + item.UnQualifiedQty,
                    UpdatedBy = commonBo.UserName,
                    UpdatedOn = HymsonClock.Now()
                });

                updateManuSfcQtyByIdCommands.Add(new UpdateManuSfcQtyByIdCommand
                {
                    Id = sfcProduceEntity.SFCId,
                    Qty = item.QualifiedQty+ item.UnQualifiedQty,
                    UpdatedBy = commonBo.UserName,
                    UpdatedOn = HymsonClock.Now()
                });

                manuSfcStepEnties.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfcProduceEntity.SFC,
                    ProductId = sfcProduceEntity.ProductId,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    WorkCenterId = sfcProduceEntity.WorkCenterId,
                    ProductBOMId = sfcProduceEntity.ProductBOMId,
                    ProcedureId = commonBo.ProcedureId,
                    Qty = sfcProduceEntity.Qty,
                    EquipmentId = commonBo.EquipmentId,
                    ResourceId = commonBo.ResourceId,
                    CurrentStatus = sfcProduceEntity.Status,
                    Operatetype = ManuSfcStepTypeEnum.OutputReport,
                    SiteId = commonBo.SiteId,
                    CreatedBy = commonBo.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = commonBo.UserName,
                    UpdatedOn = HymsonClock.Now()
                });
            }

            return new OutputModifyResponseBo
            {
                ManuSfcProduceEntities = sfcProduceEntities,
                ManuSfcStepEntities = manuSfcStepEnties,
                UpdateSfcProcedureQtyByIdCommands = updateSfcProcedureQtyCommands,
                UpdateManuSfcQtyByIdCommands = updateManuSfcQtyByIdCommands
            };
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo?> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not OutputModifyResponseBo data) return responseBo;

            if (data.ManuSfcStepEntities != null && data.ManuSfcStepEntities.Any())
            {
                await _manuSfcStepRepository.InsertRangeAsync(data.ManuSfcStepEntities);
            }

            if (data.UpdateSfcProcedureQtyByIdCommands != null && data.UpdateSfcProcedureQtyByIdCommands.Any())
            {
                await _manuSfcProduceRepository.UpdateQtyRangeAsync(data.UpdateSfcProcedureQtyByIdCommands);
            }

            if (data.UpdateManuSfcQtyByIdCommands != null && data.UpdateManuSfcQtyByIdCommands.Any())
            {
                await _manuSfcRepository.UpdateManuSfcQtyByIdRangeAsync(data.UpdateManuSfcQtyByIdCommands);
            }
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
