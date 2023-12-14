using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.WhWarehouseRegion
{
    /// <summary>
    /// 库区新增Dto
    /// </summary>
    public record WhWarehouseRegionSaveDto : BaseEntityDto
    {
        ///// <summary>
        ///// 
        ///// </summary>
        //public long? Id { get; set; }

        /// <summary>
        /// 库区编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 库区名称
        /// </summary>
        public string Name { get; set; }

        ///// <summary>
        ///// 仓库id
        ///// </summary>
        //public long? WarehouseId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string? WarehouseCode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

       /// <summary>
        /// 状态;1、启用  2未启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

       // /// <summary>
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
    /// 库区Dto
    /// </summary>
    public record WhWarehouseRegionDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 库区编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 库区编码
        /// </summary>
        public string WarehouseRegionCode { get; set; }

        /// <summary>
        /// 库区名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 仓库id
        /// </summary>
        public long WarehouseId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string? WarehouseCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string? WarehouseName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态;1、启用  2未启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

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

       
    }

    /// <summary>
    /// 库区分页Dto
    /// </summary>
    public class WhWarehouseRegionPagedQueryDto : PagerInfo {

        /// <summary>
        /// 库区编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 库区名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string? WareHouseCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string? WareHouseName { get; set; }

        /// <summary>
        /// 状态;1、启用  2未启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }
    }

    /// <summary>
    /// 修改Dto
    /// </summary>
    public record WhWarehouseRegionModifyDto : BaseEntityDto {

        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 库区名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 状态;1、启用  2未启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }
    }
}
