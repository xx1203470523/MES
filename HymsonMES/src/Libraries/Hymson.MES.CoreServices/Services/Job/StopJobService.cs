using Dapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 中止
    /// </summary>
    [Job("中止", JobTypeEnum.Standard)]
    public class StopJobService : IJobService
    {
        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 服务接口（多语言）
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="masterDataService"></param>
        /// <param name="localizationService"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        public StopJobService(IMasterDataService masterDataService,
            ILocalizationService localizationService,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcStepRepository manuSfcStepRepository)
        {
            _masterDataService = masterDataService;
            _localizationService = localizationService;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<StopRequestBo>();
            if (bo == null) return;

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, bo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any()) return;

            await bo.Proxy.GetValueAsync(_masterDataService.GetProduceBusinessEntitiesBySFCsAsync, bo);

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcStatusEnum.Activity, _localizationService)
                              .VerifyProcedure(bo.ProcedureId)
                              .VerifyResource(bo.ResourceId);
        }

        /// <summary>
        /// 执行前节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return await Task.FromResult<IEnumerable<JobBo>?>(default);
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<StopRequestBo>();
            if (bo == null) return default;

            // 待执行的命令
            StopResponseBo responseBo = new();

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy!.GetValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, bo);
            responseBo.SFCProduceEntities = sfcProduceEntities.AsList();
            if (responseBo.SFCProduceEntities == null || !responseBo.SFCProduceEntities.Any()) return default;

            // 更新时间
            var updatedBy = bo.UserName;
            var updatedOn = HymsonClock.Now();
            responseBo.SFCProduceEntities.ForEach(sfcProduceEntity =>
            {
                // 条码状态（当前状态）
                var currentStatus = sfcProduceEntity.Status;

                // 更改状态，将条码由"活动"改为"排队"
                sfcProduceEntity.Status = SfcStatusEnum.lineUp;
                sfcProduceEntity.UpdatedBy = updatedBy;
                sfcProduceEntity.UpdatedOn = updatedOn;
                sfcProduceEntity.RepeatedCount--;

                // 初始化步骤
                responseBo.SFCStepEntities.Add(new ManuSfcStepEntity
                {
                    // 插入 manu_sfc_step 状态为 停止
                    Operatetype = ManuSfcStepTypeEnum.Stop,
                    CurrentStatus = currentStatus,
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfcProduceEntity.SFC,
                    ProductId = sfcProduceEntity.ProductId,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    WorkCenterId = sfcProduceEntity.WorkCenterId,
                    ProductBOMId = sfcProduceEntity.ProductBOMId,
                    ProcedureId = bo.ProcedureId,
                    ResourceId = bo.ResourceId,
                    EquipmentId = bo.EquipmentId,
                    Qty = sfcProduceEntity.Qty,
                    SiteId = bo.SiteId,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                });
                responseBo.InStationManuSfcByIdCommands.Add(new InStationManuSfcByIdCommand
                {
                    Id = sfcProduceEntity.SFCId,
                    Status = SfcStatusEnum.Activity,
                    IsUsed = YesOrNoEnum.Yes,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                });
            });
            return responseBo;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo?> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not StopResponseBo data) return responseBo;

            // 更新数据
            List<Task<int>> tasks = new();
            //更新条码表 状态为排队
            var multiUpdateSfcIsUsedTask = _manuSfcRepository.InStationManuSfcByIdAsync(data.InStationManuSfcByIdCommands);
            tasks.Add(multiUpdateSfcIsUsedTask);

            var insertRangeTask = _manuSfcStepRepository.InsertRangeAsync(data.SFCStepEntities);
            tasks.Add(insertRangeTask);

            var updateRangeTask = _manuSfcProduceRepository.UpdateRangeAsync(data.SFCProduceEntities);
            tasks.Add(updateRangeTask);

            // 等待所有任务完成
            var rowArray = await Task.WhenAll(tasks);
            responseBo.Rows += rowArray.Sum();

            // 面板需要的数据
            List<PanelModuleEnum> panelModules = new();
            responseBo.Content = new Dictionary<string, string> { { "PanelModules", panelModules.ToSerialize() } };
            responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES16340), string.Join(",", data.SFCProduceEntities.Select(s => s.SFC)));
            return responseBo;
        }

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return await Task.FromResult<IEnumerable<JobBo>?>(default);
        }

    }
}
