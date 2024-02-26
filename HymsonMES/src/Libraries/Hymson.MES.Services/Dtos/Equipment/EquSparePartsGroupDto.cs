using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 备件类型新增/更新Dto
    /// </summary>
    public record EquSparePartsGroupSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 备件编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 备件名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 状态;0 禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 关联设备组
        /// </summary>
        public IEnumerable<long>? EquipmentGroupIds { get; set; }

        /// <summary>
        /// 关联备件
        /// </summary>
        public IEnumerable<long>? SparePartIds { get; set; }

    }

    /// <summary>
    /// 备件类型Dto
    /// </summary>
    public record EquSparePartsGroupDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 备件编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 备件名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 状态;0 禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; } = "";

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

       
    }

    /// <summary>
    /// 备件类型分页Dto
    /// </summary>
    public class EquSparePartsGroupPagedQueryDto : PagerInfo {

        /// <summary>
        /// 备件编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态;0 禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

    }

}
