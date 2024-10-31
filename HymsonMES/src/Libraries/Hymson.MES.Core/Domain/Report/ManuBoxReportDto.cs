using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Warehouse;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 成品
    /// </summary>
    public record ReportBoxResultDto : BaseEntityDto
    {
        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime InWmsData { get; set; }

        /// <summary>
        /// 生产工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 完工单号
        /// </summary>
        public string CompletionOrderCode { get; set; }

        /// <summary>
        /// 成品码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 箱体码
        /// </summary>
        public string ContaineCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specifications { get; set; }

        /// <summary>
        /// 主计量单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 工单数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        public int InWmsQty { get; set; }

        /// <summary>
        /// 来源订单号
        /// </summary>
        public string WorkPlanCode { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 入库状态
        /// </summary>
        public ProductReceiptStatusEnum Status { get; set; }
    }

    /// <summary>
    /// 成品查询
    /// </summary>
    public class ReportBoxQueryDto : PagerInfo 
    {
        /// <summary>
        /// 仓库
        /// </summary>
        public string? WarehouseCode { get; set; }        // t2.Warehouse  

        /// <summary>
        /// 生产工单号
        /// </summary>
        public string? OrderCode { get; set; }        // t3.OrderCode  

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }     // t5.MaterialCode  

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }     // t5.MaterialName  

        /// <summary>
        /// 领料日期
        /// </summary>
        public DateTime[] ? InWmsData { get; set; }        // t4.CreatedOn  

        /// <summary>
        /// 领料状态
        /// </summary>
        public ProductReceiptStatusEnum? Status { get; set; }           // t2.Status  

        /// <summary>
        /// 箱体码
        /// </summary>
        public string? ContaineCode { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public string ?CreatedBy { get; set; }        // t1.CreatedBy  
    }

}
