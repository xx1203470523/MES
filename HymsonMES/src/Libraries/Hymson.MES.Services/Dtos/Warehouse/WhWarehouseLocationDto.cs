using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.WhWarehouseLocation
{
    /// <summary>
    /// 库位新增
    /// </summary>
    public record WhWarehouseLocationSaveDto : BaseEntityDto
    {
        ///// <summary>
        ///// 
        ///// </summary>
        //public long Id { get; set; }

        ///// <summary>
        // /// 货架id
        // /// </summary>
        // public long? WarehouseShelfId { get; set; }

        /// <summary>
        /// 货架编码
        /// </summary>
        public string? WarehouseShelfCode { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string? WarehouseCode { get; set; }

        /// <summary>
        /// 库区编码
        /// </summary>
        public string? WarehouseRegionCode { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 方式;1、自动生成  2、自定义 3、指定
        /// </summary>
        public WhWarehouseLocationTypeEnum? Type { get; set; }

        /// <summary>
        /// 状态;1、启用  2、未启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 行
        /// </summary>
        public int? Row { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public int? Column { get; set; }

        ///// <summary>
        // /// 创建人
        // /// </summary>
        // public string CreatedBy { get; set; }

        ///// <summary>
        // /// 创建时间
        // /// </summary>
        // public DateTime CreatedOn { get; set; }

        ///// <summary>
        // /// 最后修改人
        // /// </summary>
        // public string UpdatedBy { get; set; }

        ///// <summary>
        // /// 修改时间
        // /// </summary>
        // public DateTime UpdatedOn { get; set; }

        ///// <summary>
        // /// 是否逻辑删除
        // /// </summary>
        // public long IsDeleted { get; set; }

        ///// <summary>
        // /// 站点Id
        // /// </summary>
        // public long SiteId { get; set; }

    }

    /// <summary>
    /// 库位Dto
    /// </summary>
    public record WhWarehouseLocationDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 货架id
        /// </summary>
        public long? WarehouseShelfId { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 方式;1、自动生成  2、自定义 3、指定
        /// </summary>
        public WhWarehouseLocationTypeEnum Type { get; set; }

        /// <summary>
        /// 状态;1、启用  2、未启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string? WarehouseCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string? WarehouseName { get; set; }

        /// <summary>
        /// 库区编码
        /// </summary>
        public string? WarehouseRegionCode { get; set; }

        /// <summary>
        /// 库区名称
        /// </summary>
        public string? WarehouseRegionName { get; set; }

        /// <summary>
        /// 货架编码
        /// </summary>
        public string? WarehouseShelfCode { get; set; }

        /// <summary>
        /// 货架名称
        /// </summary>
        public string? WarehouseShelfName { get; set; }
    }

    /// <summary>
    /// 库位分页Dto
    /// </summary>
    public class WhWarehouseLocationPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string? WarehouseCode { get; set; }

        /// <summary>
        /// 库区编码
        /// </summary>
        public string? WarehouseRegionCode { get; set; }

        /// <summary>
        /// 货架编码
        /// </summary>
        public string? WarehouseShelfCode { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 状态;1、启用  2、未启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }
    }

    /// <summary>
    /// 库区查询Dto
    /// </summary>
    public record WhWarehouseLocationQueryDto : BaseEntityDto
    {
        /// <summary>
        /// 货架Id
        /// </summary>
        public long? WarehouseShelfId { get; set; }
    }

    /// <summary>
    /// 更新Dto
    /// </summary>
    public record WhWarehouseLocationModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 状态;1、启用  2、未启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }
    }
}
