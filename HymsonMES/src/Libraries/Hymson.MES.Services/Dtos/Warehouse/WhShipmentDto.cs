using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.WhShipmentMaterial;

namespace Hymson.MES.Services.Dtos.WhShipment
{
    /// <summary>
    /// 出货单新增/更新Dto
    /// </summary>
    public record WhShipmentSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        //public long Id { get; set; }

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 出货单号
        /// </summary>
        public string ShipmentNum { get; set; }

       /// <summary>
        /// 客户id
        /// </summary>
        public long CustomerId { get; set; }

       /// <summary>
        /// 计划出现时间
        /// </summary>
        public string PlanShipmentTime { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public IList<WhShipmentMaterialDto> Details { get; set; }


    }

    /// <summary>
    /// 出货单Dto
    /// </summary>
    public record WhShipmentDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 出货单号
        /// </summary>
        public string ShipmentNum { get; set; }

       /// <summary>
        /// 客户id
        /// </summary>
        public long CustomerId { get; set; }

       /// <summary>
        /// 计划出现时间
        /// </summary>
        public DateTime? PlanShipmentTime { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

    }

    /// <summary>
    /// 出货单分页Dto
    /// </summary>
    public class WhShipmentPagedQueryDto : PagerInfo {

        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 出货单号
        /// </summary>
        public string? ShipmentNum { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime[]? TimeStamp { get; set; }

        /// <summary>
        /// 创建时间开始日期
        /// </summary>
        public DateTime? PlanShipmentTimeStart { get; set; }

        /// <summary>
        /// 创建时间结束日期
        /// </summary>
        public DateTime? PlanShipmentTimeEnd { get; set; }

    }

    /// <summary>
    /// 包含子项
    /// </summary>
    public record WhShipmentOutDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 出货单号
        /// </summary>
        public string ShipmentNum { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        public long CustomId { get; set; }

        /// <summary>
        /// 计划出现时间
        /// </summary>
        public string PlanShipmentTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public IList<WhShipmentMaterialDto> WhShipmentMaterials { get; set; }

    }

    /// <summary>
    /// 出货单，客户，物料信息View
    /// </summary>
    public class WhShipmentSupplierMaterialViewDto
    {
        /// <summary>
        /// 出货单详情Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 客户编码
        /// </summary>
        public string? CustomCode { get; set; }

        /// <summary>
        /// 出货数量
        /// </summary>
        public decimal? Qty { get; set; }
}

    /// <summary>
    /// 出货单查询Dto
    /// </summary>
    public class WhShipmentQueryDto
    { 
        /// <summary>
        /// 出货单Id
        /// </summary>
        public long? Id { get; set; }
    }
}
