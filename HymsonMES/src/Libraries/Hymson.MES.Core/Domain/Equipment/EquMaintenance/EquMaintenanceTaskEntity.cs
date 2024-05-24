using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment.EquMaintenance;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（设备保养任务）   
    /// equ_maintenance_task
    /// @author JAM
    /// @date 2024-05-23 03:20:49
    /// </summary>
    public class EquMaintenanceTaskEntity : BaseEntity
    {
        /// <summary>
        /// 任务编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 开始时间（实际）
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 结束时间（实际）
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 状态;1:待处理、2:处理中、3:待审核、4:已关闭
        /// </summary>
        public EquMaintenanceTaskStautusEnum Status { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

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
