using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（设备点检任务项目）   
    /// equ_spotcheck_task_item
    /// @author JAM
    /// @date 2024-05-21 02:06:49
    /// </summary>
    public class EquSpotcheckTaskItemEntity : BaseEntity
    {
        /// <summary>
        /// 点检任务ID;equ_spotcheck_task表的Id
        /// </summary>
        public long? SpotCheckTaskId { get; set; }

        /// <summary>
        /// 点检项目快照ID;equ_spotcheck_item_snapshot表的Id
        /// </summary>
        public long SpotCheckItemSnapshotId { get; set; }

        /// <summary>
        /// 检验值
        /// </summary>
        public string? InspectionValue { get; set; }

        /// <summary>
        /// 是否合格;(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
