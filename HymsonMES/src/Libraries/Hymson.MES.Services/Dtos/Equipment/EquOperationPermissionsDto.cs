using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 设备维保权限新增/更新Dto
    /// </summary>
    public record EquOperationPermissionsSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 设备Id;equ_equipment表的Id
        /// </summary>
        public long Equipmentid { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 类型;点检/保养/维修
        /// </summary>
        public EquipmentOperationPermissionsTypeEnum Type { get; set; }

        /// <summary>
        /// 点检执行人;
        /// </summary>
        public string[]? Executorids { get; set; }

        /// <summary>
        /// 点检负责人;
        /// </summary>
        public string[]? Leaderids { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }

    /// <summary>
    /// 设备维保权限Dto
    /// </summary>
    public record EquOperationPermissionsDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备Id;equ_equipment表的Id
        /// </summary>
        public long Equipmentid { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 类型;点检/保养/维修
        /// </summary>
        public EquipmentOperationPermissionsTypeEnum Type { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? EquipmentName { get; set; }
        /// <summary>
        /// 设备组编码
        /// </summary>
        public string? EquipmentGroupCode { get; set; }

        /// <summary>
        /// 设备组名称
        /// </summary>
        public string? EquipmentGroupName { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public string? Location { get; set; }
        /// <summary>
        /// 执行人
        /// </summary>
        public string? ExecutorIds { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string? LeaderIds { get; set; } 
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
        public string? UpdatedBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }

    /// <summary>
    /// 设备维保权限分页Dto
    /// </summary>
    public class EquOperationPermissionsQueryDto : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? EquipmentName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public EquipmentOperationPermissionsTypeEnum? Type { get; set; }
        /// <summary>
        /// 设备组编码
        /// </summary>
        public string? EquipmentGroupCode { get; set; }

        /// <summary>
        /// 设备组名称
        /// </summary>
        public string? EquipmentGroupName { get; set; }
        /// <summary>
        /// 执行人
        /// </summary>
        public string? ExecutorIds { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string? LeaderIds { get; set; }
    }

}
