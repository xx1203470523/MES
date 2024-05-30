using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.Data.Repositories.Manufacture;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 出站业务对象（请求）
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
        /// 合格数量
        /// </summary>
        public decimal QualifiedQty { get; set; }

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal UnQualifiedQty { get; set; }

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
    /// 出站业务对象（响应）
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

        /*
        /// <summary>
        /// 流转记录
        /// </summary>
        public IEnumerable<ManuSfcCirculationEntity> ManuSfcCirculationEntities { get; set; } = new List<ManuSfcCirculationEntity>();
        */

        /// <summary>
        /// 条码关系
        /// </summary>
        public IEnumerable<ManuBarCodeRelationEntity> ManuBarCodeRelationEntities { get; set; } = new List<ManuBarCodeRelationEntity>();

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
    /// 出站业务对象（请求）
    /// </summary>
    public class OutStationCoreRequestBo
    {
        /// <summary>
        /// 作业基础参数
        /// </summary>
        public JobRequestBo CommonBo { get; set; }

        /// <summary>
        /// 出站参数
        /// </summary>
        public OutStationRequestBo RequestBo { get; set; }

        /// <summary>
        /// 复投相关参数
        /// </summary>
        public ProcedureRejudgeBo ProcedureRejudgeBo { get; set; }

        /// <summary>
        /// 条码实体
        /// </summary>
        public ManuSfcEntity SFCEntity { get; set; }

        /// <summary>
        /// 在制条码实体
        /// </summary>
        public ManuSfcProduceEntity SFCProduceEntity { get; set; }

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

        /*
        /// <summary>
        /// 流转记录
        /// </summary>
        public IEnumerable<ManuSfcCirculationEntity>? ManuSfcCirculationEntities { get; set; }
        */

        /// <summary>
        /// 条码关系
        /// </summary>
        public IEnumerable<ManuBarCodeRelationEntity> ManuBarCodeRelationEntities { get; set; } = new List<ManuBarCodeRelationEntity>();

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
    /// 出站消耗业务对象（请求）
    /// </summary>
    public class MaterialConsumptionRequestBo
    {
        /// <summary>
        /// 作业基础参数
        /// </summary>
        public ManufactureIdsBo IdsBo { get; set; }

        /// <summary>
        /// 产品序列码
        /// </summary>
        public ManuSfcProduceEntity SFCProduceEntity { get; set; }

        /// <summary>
        /// 可以消耗的物料清单
        /// </summary>
        public MaterialDeductResponseSummaryBo InitialMaterialSummaryBo { get; set; }

        /// <summary>
        /// 消耗
        /// </summary>
        public IEnumerable<OutStationConsumeBo>? ConsumeList { get; set; }

        /// <summary>
        /// 步骤ID
        /// </summary>
        public long SFCStepId { get; set; }

    }

    /// <summary>
    /// 出站消耗业务对象（响应）
    /// </summary>
    public class MaterialConsumptionResponseBo
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<UpdateFeedingQtyByIdCommand> UpdateFeedingQtyByIdCommands { get; set; } = new List<UpdateFeedingQtyByIdCommand>();

        /// <summary>
        /// 条码关系
        /// </summary>
        public IEnumerable<ManuBarCodeRelationEntity> ManuBarCodeRelationEntities { get; set; } = new List<ManuBarCodeRelationEntity>();

    }


}
