using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（设备维保权限）   
    /// equ_operation_permissions
    /// @author User
    /// @date 2024-04-03 04:49:48
    /// </summary>
    public class EquOperationPermissionsEntity : BaseEntity
    {
        /// <summary>
        /// 设备Id;equ_equipment表的Id
        /// </summary>
        public long Equipmentid { get; set; }

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
        public string ExecutorIds { get; set; }

        /// <summary>
        /// 点检负责人;
        /// </summary>
        public string LeaderIds { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }
}
