using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 工具类型和设备关联关系表新增/更新Dto
    /// </summary>
    public record EquToolingTypeGroupEquipmentGroupRelationSaveDto : BaseEntityDto
    {
        
       /// <summary>
        /// 工具类型 的id
        /// </summary>
        public long? ToolTypeId { get; set; }

        /// <summary>
        /// 设备组 的id
        /// </summary>
        public long? EquipmentGroupId { get; set; }

    }

    /// <summary>
    ///工具类型和设备关联关系表分页Dto
    /// </summary>
    public class EquToolingTypeEquipmentGroupRelationPagedQueryDto : PagerInfo { }

}
