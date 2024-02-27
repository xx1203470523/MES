using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Plan;

namespace Hymson.MES.Core.Domain.Plan
{
    /// <summary>
    /// 数据实体（班制详细）   
    /// plan_shift_detail
    /// @author Jam
    /// @date 2024-01-30 02:16:55
    /// </summary>
    public class PlanShiftDetailEntity : BaseEntity
    {
        /// <summary>
        /// 主表Id
        /// </summary>
        public long ShfitId { get; set; }

       /// <summary>
        /// 班次类型;1、早班 2、中班 3、晚班
        /// </summary>
        public InteShiftTypeEnum ShiftType { get; set; }

       /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

       /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

       /// <summary>
        /// 是否跨天;0、否  1、 是
        /// </summary>
        public bool? IsDaySpan { get; set; }

       /// <summary>
        /// 是否加班;0、否  1、 是
        /// </summary>
        public bool? IsOverTime { get; set; }

        /// <summary>
        /// 物料组描述
        /// </summary>
        public string Remark { get; set; } = "";

       
    }
}
