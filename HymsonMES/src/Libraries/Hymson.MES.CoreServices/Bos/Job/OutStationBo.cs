using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcSummary.Command;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 出站job实体
    /// </summary>
    public class OutStationRequestBo : JobBaseBo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";
        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 载具条码
        /// </summary>
        public string? VehicleCode { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public bool? IsQualified { get; set; }

        /// <summary>
        /// 消耗
        /// </summary>
        public IEnumerable<OutStationConsumeBo>? ConsumeList { get; set; }

        /// <summary>
        /// 出站不良信息
        /// </summary>
        public IEnumerable<OutStationUnqualifiedBo>? OutStationUnqualifiedList { get; set; }
    }

    /// <summary>
    /// 消耗
    /// </summary>
    public class OutStationConsumeBo
    {
        /// <summary>
        /// 消耗条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 消耗数量
        /// </summary>
        public decimal? ConsumeQty { get; set; }
    }

    public class OutStationUnqualifiedBo
    {
        /// <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OutStationResponseBo
    {
        /// <summary>
        /// 是否完工
        /// </summary>
        public bool IsCompleted { get; set; } = true;

        /// <summary>
        /// 条码（首个）
        /// </summary>
        public string FirstSFC { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<UpdateQtyByIdCommand> UpdateQtyByIdCommands { get; set; } = new List<UpdateQtyByIdCommand>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcCirculationEntity> ManuSfcCirculationEntities { get; set; } = new List<ManuSfcCirculationEntity>();

        /// <summary>
        /// 
        /// </summary>
        public List<ManuSfcStepEntity> SFCStepEntities { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public List<WhMaterialInventoryEntity> WhMaterialInventoryEntities { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public List<WhMaterialStandingbookEntity> WhMaterialStandingbookEntities { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public DeletePhysicalByProduceIdsCommand DeletePhysicalByProduceIdsCommand { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public UpdateQtyCommand UpdateQtyCommand { get; set; } = new();

        /// <summary>
        /// 条码信息
        /// </summary>
        public List<ManuSfcEntity> SFCEntities { get; set; } = new();

        /// <summary>
        /// 在制品信息
        /// </summary>
        public List<ManuSfcProduceEntity> SFCProduceEntities { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public DeleteSfcProduceBusinesssBySfcInfoIdsCommand DeleteSfcProduceBusinesssBySfcInfoIdsCommand { get; set; } = new();
        
        /// <summary>
        /// 
        /// </summary>
        public ProcessRouteTypeEnum ProcessRouteType { get; set; }

        /// <summary>
        /// 下一工序编码
        /// </summary>
        public string NextProcedureCode { get; set; } = "";

        /// <summary>
        /// 降级品录入对象
        /// </summary>
        public DegradedProductExtendBo DegradedProductExtendBo { get; set; }

        /// <summary>
        /// 降级品录入对象
        /// </summary>
        public IEnumerable<ManuDowngradingEntity> DowngradingEntities { get; set; }

        /// <summary>
        /// 不良品录入对象
        /// </summary>
        public List<ManuProductBadRecordEntity> ProductBadRecordEntities { get; set; } = new();

        /// <summary>
        /// 汇总表更新对象
        /// </summary>
        public IEnumerable<MultiUpdateSummaryOutStationCommand> MultiUpdateSummaryOutStationCommands { get; set; }
    }

    /// <summary>
    /// 出站消耗
    /// </summary>
    public class MaterialConsumptionBo
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<UpdateQtyByIdCommand> UpdateQtyByIdCommands { get; set; } = new List<UpdateQtyByIdCommand>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcCirculationEntity> ManuSfcCirculationEntities { get; set; } = new List<ManuSfcCirculationEntity>();

    }

}
