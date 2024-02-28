using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.WhWareHouse
{
    /// <summary>
    /// 仓库新增Dto
    /// </summary>
    public record WhWarehouseSaveDto : BaseEntityDto
    {
        ///// <summary>
        ///// 
        ///// </summary>
        //public long? Id { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string? Name { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

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
        ///// 是否逻辑删除
        ///// </summary>
        //public long? IsDeleted { get; set; }

        ///// <summary>
        ///// 站点Id
        ///// </summary>
        //public long? SiteId { get; set; }
    }

    /// <summary>
    /// 仓库Dto
    /// </summary>
    public record WhWarehouseDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 仓库编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }


        /// <summary>
        /// 仓库名称
        /// </summary>
        public string Name { get; set; }
       
       /// <summary>
        /// 状态
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
    }

    /// <summary>
    /// 仓库分页Dto
    /// </summary>
    public class WhWarehousePagedQueryDto : PagerInfo {
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }
    }

    /// <summary>
    /// 更新Dto
    /// </summary>
    public record WhWarehouseModifyDto : BaseEntityDto {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }
    }

}
