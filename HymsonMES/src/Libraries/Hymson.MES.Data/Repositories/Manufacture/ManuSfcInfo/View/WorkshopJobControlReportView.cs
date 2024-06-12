using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    public class WorkshopJobControlReportView:BaseEntity
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum SFCStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SfcStatusEnum SFCProduceStatus { get; set; }

        /// <summary>
        /// 物料编码/版本
        /// </summary>
        public string MaterialCodeVersion { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum OrderType { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// Bom编码/版本
        /// </summary>
        public string BomCodeVersion { get; set; }

        /// <summary>
        /// bom名称
        /// </summary>
        public string BomName { get; set; }

        /// <summary>
        /// 条码数量
        /// </summary>
        public decimal Qty { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkshopJobControlReportOptimizeView : BaseEntity
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum SFCStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SfcStatusEnum SFCProduceStatus { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// bomID
        /// </summary>
        public long ProductBOMId { get; set; }

        /// <summary>
        /// 工艺路线Id
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 条码数量
        /// </summary>
        public decimal Qty { get; set; }
    }

    /// <summary>
    /// 条码信息和条码
    /// </summary>
    public class ManuSfcInfoSfcView : ManuSfcInfoEntity
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }
    }


}
