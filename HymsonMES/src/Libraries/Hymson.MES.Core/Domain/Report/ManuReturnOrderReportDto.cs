using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Warehouse;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 领料记录详情Dto
    /// </summary>
    public record ReportReturnOrderResultDto : BaseEntityDto
    {
        /// <summary>
        /// 领料日期
        /// </summary>
        public DateTime ReturnDate { get; set; }        // t4.CreatedOn  

        /// <summary>
        /// 生产工单号
        /// </summary>
        public string OrderCode { get; set; }        // t3.OrderCode  

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }     // t5.MaterialCode  

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }     // t5.MaterialName  

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specifications { get; set; }   // t5.Specifications  

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }             // t6.Name  

        /// <summary>
        /// 工单数量
        /// </summary>
        public int OrderQty { get; set; }            // t3.Qty  

        /// <summary>
        /// 领料数量
        /// </summary>
        public int ReqQty { get; set; }              // t1.Qty  

        /// <summary>
        /// 来源订单号
        /// </summary>
        public string WorkPlanCode { get; set; }     // t4.WorkPlanCode  

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse { get; set; }        // t2.Warehouse  

        /// <summary>
        /// 申请人
        /// </summary>
        public string CreatedBy { get; set; }        // t1.CreatedBy  

        /// <summary>
        /// 领料状态
        /// </summary>
        public WhMaterialPickingStatusEnum Status { get; set; }           // t2.Status  

        /// <summary>
        /// 领料单号
        /// </summary>
        public string ReturnOrderCode { get; set; }     // t2.ReqOrderCode  

         /// <summary>
         /// 类型
         /// </summary>
        public ManuRequistionTypeEnum Type { get; set; }             // t2.Type (注意：在C#中，通常不会使用`作为标识符的一部分，这里假设`Type`是列名，并且它不是C#的关键字或保留字)  



    }

    /// <summary>
    /// 领料记录详情分页Dto
    /// </summary>
    public class ReportReturnOrderQueryDto : PagerInfo 
    {
        /// <summary>
        /// 仓库
        /// </summary>
        public string? Warehouse { get; set; }        // t2.Warehouse  

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
        public DateTime[] ?ReqDate { get; set; }        // t4.CreatedOn  

        /// <summary>
        /// 领料状态
        /// </summary>
        public WhWarehouseMaterialReturnStatusEnum? Status { get; set; }           // t2.Status  

        /// <summary>
        /// 申请人
        /// </summary>
        public string ?CreatedBy { get; set; }        // t1.CreatedBy  
    }


    /// <summary>
    /// 导出领料
    /// </summary>
    public record ManuRerurnsOrderDto : BaseEntityDto
    {
        /// <summary>
        /// 领料日期
        /// </summary>
        public DateTime ReqDate { get; set; }        // t4.CreatedOn  

        /// <summary>
        /// 退料日期
        /// </summary>
        public DateTime ReturnDate { get; set; }        // t4.CreatedOn  

        /// <summary>
        /// 出库日期
        /// </summary>
        public DateTime OutWmsDate { get; set; }     // t4.UpdatedOn  

        /// <summary>
        /// 生产工单号
        /// </summary>
        public string OrderCode { get; set; }        // t3.OrderCode  

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }     // t5.MaterialCode  

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }     // t5.MaterialName  

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specifications { get; set; }   // t5.Specifications  

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }             // t6.Name  

        /// <summary>
        /// 工单数量
        /// </summary>
        public int OrderQty { get; set; }            // t3.Qty  

        /// <summary>
        /// 领料数量
        /// </summary>
        public int ReqQty { get; set; }              // t1.Qty  

        /// <summary>
        /// 来源订单号
        /// </summary>
        public string WorkPlanCode { get; set; }     // t4.WorkPlanCode  

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse { get; set; }        // t2.Warehouse  

        /// <summary>
        /// 申请人
        /// </summary>
        public string CreatedBy { get; set; }        // t1.CreatedBy  

        /// <summary>
        /// 领料状态
        /// </summary>
        public WhMaterialPickingStatusEnum Status { get; set; }           // t2.Status  

        /// <summary>
        /// 领料单号
        /// </summary>
        public string ReqOrderCode { get; set; }     // t2.ReqOrderCode  

        /// <summary>
        /// 类型
        /// </summary>
        public ManuRequistionTypeEnum Type { get; set; }             // t2.Type (注意：在C#中，通常不会使用`作为标识符的一部分，这里假设`Type`是列名，并且它不是C#的关键字或保留字)  


    }

}
