using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 工单完工入库新增/更新Dto
    /// </summary>
    public record ManuProductReceiptOrderSaveDto : BaseEntityDto
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
        public string WorkOrderCode { get; set; }

        /// <summary>
        /// 箱号编码
        /// </summary>
        public string ContaineCode { get; set; }

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


    }

    /// <summary>
    /// 工单完工入库Dto
    /// </summary>
    public record ManuProductReceiptOrderDto : BaseEntityDto
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
        public string WorkOrderCode { get; set; }

        /// <summary>
        /// 箱号编码
        /// </summary>
        public string ContaineCode { get; set; }

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

    }

    /// <summary>
    /// 入库明细单
    /// </summary>
    public record ManuProductReceiptOrderDetailDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 箱号编码
        /// </summary>
        public string ContaineCode { get; set; }

        /// <summary>
        /// 批次码
        /// </summary>
        public string Batch { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 计量单位(字典定义)
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 品检状态
        /// </summary>
        public ProductReceiptQualifiedStatusEnum? Status { get; set; }

        /// <summary>
        /// 入库状态
        /// </summary>
        public ProductReceiptStatusEnum StorageStatus { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 完工单号
        /// </summary>
        public string CompletionOrderCode { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; } = "";

    }

    /// <summary>
    /// 工单完工入库分页Dto
    /// </summary>
    public class ManuProductReceiptOrderPagedQueryDto : PagerInfo { }

}
