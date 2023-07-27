using Dapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 中止
    /// </summary>
    [Job("中止", JobTypeEnum.Standard)]
    public class StopJobService : IJobService
    {
        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="masterDataService"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="localizationService"></param>
        public StopJobService(IManuCommonService manuCommonService,
            IMasterDataService masterDataService,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            ILocalizationService localizationService)
        {
            _manuCommonService = manuCommonService;
            _masterDataService = masterDataService;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _localizationService = localizationService;
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
            var sfcProduceEntities = await bo.Proxy.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, bo);
            if (sfcProduceEntities == null || sfcProduceEntities.Any() == false) return;

            var sfcProduceBusinessEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceBusinessEntitiesBySFCsAsync, bo);

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcProduceStatusEnum.Activity, _localizationService.GetResource($"{typeof(SfcProduceStatusEnum).FullName}.{nameof(SfcProduceStatusEnum.Activity)}"))
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
            await Task.CompletedTask;
            return null;
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
            var sfcProduceEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, bo);
            responseBo.SFCProduceEntities = sfcProduceEntities.AsList();
            if (responseBo.SFCProduceEntities == null || responseBo.SFCProduceEntities.Any() == false) return default;

            responseBo.FirstSFCProduceEntity = responseBo.SFCProduceEntities.FirstOrDefault();
            if (responseBo.FirstSFCProduceEntity == null) return default;

            // 更新时间
            var updatedBy = bo.UserName;
            var updatedOn = HymsonClock.Now();

            List<ManuSfcStepEntity> sfcStepEntities = new();
            responseBo.SFCProduceEntities.ForEach(sfcProduceEntity =>
            {
                // 更改状态，将条码由"活动"改为"排队"
                sfcProduceEntity.Status = SfcProduceStatusEnum.lineUp;
                sfcProduceEntity.UpdatedBy = updatedBy;
                sfcProduceEntity.UpdatedOn = updatedOn;

                // 初始化步骤
                responseBo.SFCStepEntities.Add(new ManuSfcStepEntity
                {
                    // 插入 manu_sfc_step 状态为 停止
                    Operatetype = ManuSfcStepTypeEnum.Stop,
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfcProduceEntity.SFC,
                    ProductId = sfcProduceEntity.ProductId,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    WorkCenterId = sfcProduceEntity.WorkCenterId,
                    ProductBOMId = sfcProduceEntity.ProductBOMId,
                    ProcedureId = bo.ProcedureId,
                    Qty = sfcProduceEntity.Qty,
                    EquipmentId = sfcProduceEntity.EquipmentId,
                    ResourceId = bo.ResourceId,
                    SiteId = bo.SiteId,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
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
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not StopResponseBo data) return responseBo;

            // 更新数据
            List<Task<int>> tasks = new();

            var insertRangeTask = _manuSfcStepRepository.InsertRangeAsync(data.SFCStepEntities);
            tasks.Add(insertRangeTask);

            var updateRangeTask = _manuSfcProduceRepository.UpdateRangeAsync(data.SFCProduceEntities);
            tasks.Add(updateRangeTask);

            // 等待所有任务完成
            var rowArray = await Task.WhenAll(tasks);
            responseBo.Rows += rowArray.Sum();

            // 面板需要的数据
            responseBo.Content = new Dictionary<string, string> {
                { "PackageCom", "False" },
                { "BadEntryCom", "False" },
            };

            responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES16340), data.FirstSFCProduceEntity.SFC);
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
