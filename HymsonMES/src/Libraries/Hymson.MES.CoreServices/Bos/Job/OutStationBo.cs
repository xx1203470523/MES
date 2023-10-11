using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcSummary.Command;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 出站job实体
    /// </summary>
    public class OutStationRequestBo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

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

    /// <summary>
    /// 
    /// </summary>
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
        /// 是否完工（ 如果没有尾工序，就表示已完工）
        /// </summary>
        public bool IsCompleted { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public ProcessRouteTypeEnum ProcessRouteType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ManuSfcEntity SFCEntity { get; set; }

        /// <summary>
        /// 在制品信息
        /// </summary>
        public ManuSfcProduceEntity SFCProduceEntitiy { get; set; }

        /// <summary>
        /// 步骤
        /// </summary>
        public ManuSfcStepEntity SFCStepEntity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WhMaterialInventoryEntity MaterialInventoryEntity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WhMaterialStandingbookEntity MaterialStandingbookEntity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MultiUpdateSummaryOutStationCommand UpdateSummaryOutStationCommand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<UpdateQtyByIdCommand> UpdateQtyByIdCommands { get; set; } = new List<UpdateQtyByIdCommand>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcCirculationEntity> ManuSfcCirculationEntities { get; set; } = new List<ManuSfcCirculationEntity>();

        /// <summary>
        /// 降级品录入对象
        /// </summary>
        public IEnumerable<ManuDowngradingEntity> DowngradingEntities { get; set; }

        /// <summary>
        /// 降级品录入记录对象
        /// </summary>
        public IEnumerable<ManuDowngradingRecordEntity> DowngradingRecordEntities { get; set; }

        /// <summary>
        /// 不良品录入对象
        /// </summary>
        public IEnumerable<ManuProductBadRecordEntity> ProductBadRecordEntities { get; set; }


        // 额外给面板用来显示的参数
        /// <summary>
        /// 下一工序编码
        /// </summary>
        public string NextProcedureCode { get; set; } = "";
    }

    /// <summary>
    /// 
    /// </summary>
    public class OutStationResponseSummaryBo
    {
        /// <summary>
        /// 条码信息
        /// </summary>
        public IEnumerable<ManuSfcEntity>? SFCEntities { get; set; }

        /// <summary>
        /// 在制品信息
        /// </summary>
        public IEnumerable<ManuSfcProduceEntity>? SFCProduceEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcStepEntity>? SFCStepEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<WhMaterialInventoryEntity>? WhMaterialInventoryEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<WhMaterialStandingbookEntity>? WhMaterialStandingbookEntities { get; set; }

        /// <summary>
        /// 汇总表更新对象
        /// </summary>
        public IEnumerable<MultiUpdateSummaryOutStationCommand>? MultiUpdateSummaryOutStationCommands { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<UpdateQtyByIdCommand>? UpdateQtyByIdCommands { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcCirculationEntity>? ManuSfcCirculationEntities { get; set; }

        /// <summary>
        /// 降级品录入对象
        /// </summary>
        public IEnumerable<ManuDowngradingEntity>? DowngradingEntities { get; set; }

        /// <summary>
        /// 降级品录入记录对象
        /// </summary>
        public IEnumerable<ManuDowngradingRecordEntity>? DowngradingRecordEntities { get; set; }

        /// <summary>
        /// 不良品录入对象
        /// </summary>
        public IEnumerable<ManuProductBadRecordEntity>? ProductBadRecordEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DeleteSfcProduceBusinesssBySfcInfoIdsCommand DeleteSfcProduceBusinesssBySfcInfoIdsCommand { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public DeletePhysicalByProduceIdsCommand DeletePhysicalByProduceIdsCommand { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public List<UpdateQtyCommand> UpdateQtyCommands { get; set; } = new();


        // 额外给面板用来显示的参数
        /// <summary>
        /// 是否尾工序
        /// </summary>
        public bool IsCompleted { get; set; } = true;
        /// <summary>
        /// 下一工序编码
        /// </summary>
        public string NextProcedureCode { get; set; } = "";
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
