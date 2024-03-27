using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.Data.Repositories.Manufacture;

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
        public bool? IsQualified { get; set; } = true;

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
    /// 
    /// </summary>
    public class OutStationResponseBo
    {
        /// <summary>
        /// 是否尾工序（ 如果已经是尾工序，就表示已完工）
        /// </summary>
        public bool IsLastProcedure { get; set; } = false;

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
        public UpdateOutputQtySummaryCommand UpdateOutputQtySummaryCommand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<UpdateFeedingQtyByIdCommand> UpdateFeedingQtyByIdCommands { get; set; } = new List<UpdateFeedingQtyByIdCommand>();

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

        /// <summary>
        /// 产品NG记录对象
        /// </summary>
        public IEnumerable<ManuProductNgRecordEntity> ProductNgRecordEntities { get; set; }

        /// <summary>
        /// 在制维修业务
        /// </summary>
        public ManuSfcProduceBusinessEntity SFCProduceBusinessEntity { get; set; }

        // 额外给面板用来显示的参数
        /// <summary>
        /// 下一工序编码
        /// </summary>
        public string NextProcedureCode { get; set; } = "";
    }

    /// <summary>
    /// 
    /// </summary>
    public class OutStationResponseSummaryBo : CommonResponseBo
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
        /// 
        /// </summary>
        public IEnumerable<UpdateFeedingQtyByIdCommand>? UpdateFeedingQtyByIdCommands { get; set; }

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
        /// 产品NG记录对象
        /// </summary>
        public IEnumerable<ManuProductNgRecordEntity> ProductNgRecordEntities { get; set; }

        /// <summary>
        /// 在制品业务
        /// </summary>
        public IEnumerable<ManuSfcProduceBusinessEntity> SFCProduceBusinessEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DeleteSFCProduceBusinesssByIdsCommand DeleteSfcProduceBusinesssBySfcInfoIdsCommand { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public PhysicalDeleteSFCProduceByIdsCommand DeletePhysicalByProduceIdsCommand { get; set; } = new();

    }

    /// <summary>
    /// 出站消耗
    /// </summary>
    public class MaterialConsumptionBo
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<UpdateFeedingQtyByIdCommand> UpdateFeedingQtyByIdCommands { get; set; } = new List<UpdateFeedingQtyByIdCommand>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcCirculationEntity> ManuSfcCirculationEntities { get; set; } = new List<ManuSfcCirculationEntity>();

    }

}
