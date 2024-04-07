using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（点检任务详情）   
    /// equ_inspection_task_details
    /// @author User
    /// @date 2024-04-03 04:51:43
    /// </summary>
    public class EquInspectionTaskDetailsEntity : BaseEntity
    {
        /// <summary>
        /// 点检任务Id
        /// </summary>
        public long? InspectionTaskId { get; set; }

        /// <summary>
        /// 点检项目Id
        /// </summary>
        public long? InspectionItemId { get; set; }

        /// <summary>
        /// 基准值
        /// </summary>
        public decimal? BaseValue { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

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
