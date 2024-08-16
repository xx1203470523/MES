using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 废成品入库记录新增/更新Dto
    /// </summary>
    public record ManuWasteProductsReceiptRecordSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 入库工单号
        /// </summary>
        public string WarehouseOrderCode { get; set; }

       /// <summary>
        /// 工单id
        /// </summary>
        public long? WorkOrderId { get; set; }

       /// <summary>
        /// 状态0:审批中，1：审批失败，2：审批成功3.已退料
        /// </summary>
        public bool Status { get; set; }

       /// <summary>
        /// 物料描述
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 完工入库单
        /// </summary>
        public string CompletionOrderCode { get; set; }

       
    }

    /// <summary>
    /// 废成品入库记录Dto
    /// </summary>
    public record ManuWasteProductsReceiptRecordDto : BaseEntityDto
    {
        /// <summary>
        /// 入库工单号
        /// </summary>
        public string WarehouseOrderCode { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 状态0:审批中，1：审批失败，2：审批成功3.已退料
        /// </summary>
        public ProductReceiptStatusEnum Status { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 计量单位(字典定义)
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 完工入库单
        /// </summary>
        public string CompletionOrderCode { get; set; }
    }

    /// <summary>
    /// 废成品入库记录分页Dto
    /// </summary>
    public class ManuWasteProductsReceiptRecordPagedQueryDto : PagerInfo { }

}
