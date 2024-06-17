using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 工具类型新增/更新Dto
    /// </summary>
    public record EquToolingTypeSaveDto : BaseEntityDto
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
    /// 工具类型Dto
    /// </summary>
    public record EquToolingTypeDto : BaseEntityDto
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
        /// 是否需要校准
        /// </summary>
        public YesOrNoEnum Calibration { get; set; }

        /// <summary>
        /// 额定使用寿命
        /// </summary>
        public int LifeSpan { get; set; }

        /// <summary>
        /// 校准周期
        /// </summary>
        public string Cycle { get; set; }

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
    /// 工具类型分页Dto
    /// </summary>
    public class EquToolingTypeQueryDto : PagerInfo {

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
