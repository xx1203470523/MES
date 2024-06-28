using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（工具绑定设备操作记录表）   
    /// equ_tools_equipment__bind_record
    /// @author zhaoqing
    /// @date 2024-06-12 04:16:13
    /// </summary>
    public class EquToolsEquipmentBindRecordEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工具id
        /// </summary>
        public long ToolId { get; set; }

        /// <summary>
        /// 工具记录表id equ_tools_record 的Id
        /// </summary>
        public long? ToolsRecordId { get; set; }

        /// <summary>
        /// 设备id equ_equipment的 id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 设备记录id equ_equipment_record
        /// </summary>
        public long EquipmentRecordId { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 操作类型1、绑定 2、卸载
        /// </summary>
        public BindOperationTypeEnum OperationType { get; set; }

        /// <summary>
        /// 卸载原因 1、正常2、异常
        /// </summary>
        public EquUninstallReasonEnum? UninstallReason { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 卸载人
        /// </summary>
        public string UninstallBy { get; set; }

        /// <summary>
        /// 卸载时间
        /// </summary>
        public DateTime? UninstallOn { get; set; }

        /// <summary>
        /// 本次使用寿命
        /// </summary>
        public decimal? CurrentUsedLife { get; set; }
    }
}
