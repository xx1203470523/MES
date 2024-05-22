using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Core.Domain.Equipment.EquSpotcheck
{
    /// <summary>
    /// 数据实体（设备点检任务结果处理）   
    /// equ_spotcheck_task_processed
    /// @author JAM
    /// @date 2024-05-15 02:00:21
    /// </summary>
    public class EquSpotcheckTaskProcessedEntity : BaseEntity
    {
        /// <summary>
        /// 点检任务ID;equ_spotcheck_task表的Id
        /// </summary>
        public long SpotCheckTaskId { get; set; }

        /// <summary>
        /// 不合格处理方式;1-通过；2-不通过
        /// </summary>
        public EquSpotcheckTaskProcessedEnum? HandMethod { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public string ProcessedBy { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? ProcessedOn { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }


    }
}
