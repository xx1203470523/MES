using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.AgvTaskRecord
{
    /// <summary>
    /// 数据实体（AGV任务记录表）   
    /// agv_task_record
    /// @author User
    /// @date 2024-03-11 11:37:12
    /// </summary>
    public class AgvTaskRecordEntity : BaseEntity
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public string TaskType { get; set; }

        /// <summary>
        /// 发送内容
        /// </summary>
        public string SendContent { get; set; }

        /// <summary>
        /// 接收内容
        /// </summary>
        public string ReceiveContent { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
