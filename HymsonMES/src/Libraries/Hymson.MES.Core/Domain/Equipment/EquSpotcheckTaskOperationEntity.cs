using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（设备点检任务操作）   
    /// equ_spotcheck_task_operation
    /// @author JAM
    /// @date 2024-05-15 02:00:11
    /// </summary>
    public class EquSpotcheckTaskOperationEntity : BaseEntity
    {
        /// <summary>
        /// 点检任务ID;equ_spotcheck_task表的Id
        /// </summary>
        public long SpotCheckTaskId { get; set; }

        /// <summary>
        /// 操作类型;1:待处理、2:处理中、3:待审核、4:已关闭
        /// </summary>
        public EquSpotcheckOperationTypeEnum OperationType { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperationBy { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationOn { get; set; }

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
