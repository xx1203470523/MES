using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Warehouse;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 领料明细
    /// </summary>
    public record ManuRequistionOrderDetailDto
    {
        /// <summary>
        /// 领料单据号
        /// </summary>
        public string ReqOrderCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 物料批次
        /// </summary>
        public string Batch { get; set; }

        /// <summary>
        /// 应领数量
        /// </summary>
        public decimal NeedQty { get; set; } = 0;

        /// <summary>
        /// 领料数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public WhMaterialPickingStatusEnum? Status { get; set; }

        /// <summary>
        /// 领料时间
        /// </summary>
        public DateTime PickTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; } = "";

    }

    /// <summary>
    /// 领料导出
    /// </summary>
    public record ManuRequistionOrderExportDto : BaseExcelDto
    {

        /// <summary>
        /// 领料日期
        /// </summary>
        [EpplusTableColumn(Header = "领料日期", Order = 1)]
        public DateTime ReqDate { get; set; }        // t4.CreatedOn  

        /// <summary>
        /// 出库日期
        /// </summary>
        [EpplusTableColumn(Header = "出库日期", Order = 2)]
        public DateTime OutWmsDate { get; set; }     // t4.UpdatedOn  

        /// <summary>
        /// 生产工单号
        /// </summary>
        [EpplusTableColumn(Header = "生产工单号", Order = 3)]
        public string OrderCode { get; set; }        // t3.OrderCode  

        /// <summary>
        /// 物料编码
        /// </summary>
        [EpplusTableColumn(Header = "物料编码", Order = 4)]
        public string MaterialCode { get; set; }     // t5.MaterialCode  

        /// <summary>
        /// 物料名称
        /// </summary>
        [EpplusTableColumn(Header = "物料名称", Order = 5)]
        public string MaterialName { get; set; }     // t5.MaterialName  

        /// <summary>
        /// 规格型号
        /// </summary>
        [EpplusTableColumn(Header = "规格型号", Order = 6)]
        public string Specifications { get; set; }   // t5.Specifications  

        /// <summary>
        /// 单位
        /// </summary>
        [EpplusTableColumn(Header = "单位", Order = 7)]
        public string Unit { get; set; }             // t6.Name  

        /// <summary>
        /// 工单数量
        /// </summary>
        [EpplusTableColumn(Header = "工单数量", Order = 8)]
        public int OrderQty { get; set; }            // t3.Qty
                                                     // 
        /// <summary>
        /// 领料单号
        /// </summary>
        [EpplusTableColumn(Header = "领料单号", Order = 9)]
        public string reqOrderCode { get; set; }            // t3.Qty

        /// <summary>
        /// 领料数量
        /// </summary>
        [EpplusTableColumn(Header = "领料数量", Order = 10)]
        public int ReqQty { get; set; }              // t1.Qty  

        /// <summary>
        /// 来源订单号
        /// </summary>
        [EpplusTableColumn(Header = "来源订单号", Order = 11)]
        public string WorkPlanCode { get; set; }     // t4.WorkPlanCode  

        /// <summary>
        /// 仓库
        /// </summary>
        [EpplusTableColumn(Header = "仓库", Order = 12)]
        public string Warehouse { get; set; }        // t2.Warehouse  

        /// <summary>
        /// 申请人
        /// </summary>
        [EpplusTableColumn(Header = "申请人", Order = 13)]
        public string CreatedBy { get; set; }        // t1.CreatedBy  

        /// <summary>
        /// 领料状态
        /// </summary>
        [EpplusTableColumn(Header = "领料状态", Order = 14)]
        public WhMaterialPickingStatusEnum Status { get; set; }           // t2.Status  

    }

    /// <summary>
    /// 退料导出
    /// </summary>
    public record ManuReturnOrderExportDto : BaseExcelDto
    {

        /// <summary>
        /// 退料日期
        /// </summary>
        [EpplusTableColumn(Header = "退料日期", Order = 1)]
        public DateTime ReturnDate { get; set; }        // t4.CreatedOn  

       
        /// <summary>
        /// 生产工单号
        /// </summary>
        [EpplusTableColumn(Header = "生产工单号", Order = 2)]
        public string OrderCode { get; set; }        // t3.OrderCode  
       
        /// <summary>
        /// 物料编码
        /// </summary>
        [EpplusTableColumn(Header = "物料编码", Order = 3)]
        public string MaterialCode { get; set; }     // t5.MaterialCode  

        /// <summary>
        /// 物料名称
        /// </summary>
        [EpplusTableColumn(Header = "物料名称", Order = 4)]
        public string MaterialName { get; set; }     // t5.MaterialName  

        /// <summary>
        /// 规格型号
        /// </summary>
        [EpplusTableColumn(Header = "规格型号", Order = 5)]
        public string Specifications { get; set; }   // t5.Specifications  

        /// <summary>
        /// 单位
        /// </summary>
        [EpplusTableColumn(Header = "单位", Order = 6)]
        public string Unit { get; set; }             // t6.Name  

        
       /// <summary>
       /// 工单数量
       /// </summary>
       [EpplusTableColumn(Header = "工单数量", Order = 7)]
       public int OrderQty { get; set; }            // t3.Qty  

        /// <summary>
        /// 退料数量
        /// </summary>
        [EpplusTableColumn(Header = "退料数量", Order = 8)]
       public int ReqQty { get; set; }              // t1.Qty  

        /// <summary>
        /// 退料单号
        /// </summary>
        [EpplusTableColumn(Header = "退料单号", Order = 9)]
       public string ReturnOrderCode { get; set; }            // t3.Qty


       /// <summary>
       /// 来源订单号
       /// </summary>
       [EpplusTableColumn(Header = "来源订单号", Order = 10)]
       public string WorkPlanCode { get; set; }     // t4.WorkPlanCode  

       /// <summary>
       /// 仓库
       /// </summary>
       [EpplusTableColumn(Header = "仓库", Order = 11)]
       public string Warehouse { get; set; }        // t2.Warehouse  

       /// <summary>
       /// 申请人
       /// </summary>
       [EpplusTableColumn(Header = "申请人", Order = 12)]
       public string CreatedBy { get; set; }        // t1.CreatedBy  

       /// <summary>
       /// 领料状态
       /// </summary>
       [EpplusTableColumn(Header = "领料状态", Order = 13)]
       public WhWarehouseMaterialReturnStatusEnum Status { get; set; }           // t2.Status  

    }
    
    /// <summary>
    /// 成品入库
    /// </summary>
    public record ManuBoxReportExportDto : BaseExcelDto
    {
        /// <summary>
        /// 入库日期
        /// </summary>
        [EpplusTableColumn(Header = "入库日期", Order = 1)]
        public DateTime InWmsData { get; set; }

        /// <summary>
        /// 生产工单号
        /// </summary>
        [EpplusTableColumn(Header = "生产工单号", Order = 2)]
        public string OrderCode { get; set; }

        /// <summary>
        /// 完工单号
        /// </summary>
        [EpplusTableColumn(Header = "完工单号", Order = 3)]
        public string CompletionOrderCode { get; set; }

        /// <summary>
        /// 成品码
        /// </summary>
        [EpplusTableColumn(Header = "成品码", Order = 4)]
        public string Sfc { get; set; }

        /// <summary>
        /// 箱体码
        /// </summary>
        [EpplusTableColumn(Header = "箱体码", Order = 5)]
        public string ContaineCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        [EpplusTableColumn(Header = "物料名称", Order = 6)]
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        [EpplusTableColumn(Header = "物料编码", Order = 7)]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        [EpplusTableColumn(Header = "规格型号", Order = 8)]
        public string Specifications { get; set; }

        /// <summary>
        /// 主计量单位
        /// </summary>
        [EpplusTableColumn(Header = "主计量单位", Order = 9)]
        public string Unit { get; set; }

        /// <summary>
        /// 工单数量
        /// </summary>
        [EpplusTableColumn(Header = "工单数量", Order = 10)]
        public int Qty { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        [EpplusTableColumn(Header = "入库数量", Order = 11)]
        public int InWmsQty { get; set; }

        /// <summary>
        /// 来源订单号
        /// </summary>
        [EpplusTableColumn(Header = "来源订单号", Order = 12)]
        public string WorkPlanCode { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        [EpplusTableColumn(Header = "仓库", Order = 13)]
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [EpplusTableColumn(Header = "创建人", Order = 14)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 入库状态
        /// </summary>
        [EpplusTableColumn(Header = "入库状态", Order = 15)]
        public ProductReceiptStatusEnum Status { get; set; }

    }
}
