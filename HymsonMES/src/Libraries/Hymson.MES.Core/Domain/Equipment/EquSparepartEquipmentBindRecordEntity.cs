using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（工具绑定设备操作记录表）   
    /// equ_sparepart_equipment_bind_record
    /// @author zhaoqing
    /// @date 2024-06-12 04:12:19
    /// </summary>
    public class EquSparepartEquipmentBindRecordEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 备件id equ_sparepart的id
        /// </summary>
        public long SparepartId { get; set; }

        /// <summary>
        /// 备件记录表id equ_sparepart_record 的Id
        /// </summary>
        public long SparepartRecordId { get; set; }

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
        public string Remark { get; set; } = "";

        
    }
}
