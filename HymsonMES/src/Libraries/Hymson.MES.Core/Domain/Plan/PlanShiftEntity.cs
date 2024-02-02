using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Plan
{
    /// <summary>
    /// 数据实体（班制）   
    /// plan_shift
    /// @author Jam
    /// @date 2024-01-24 11:57:04
    /// </summary>
    public class PlanShiftEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 物料组描述
        /// </summary>
        public string Remark { get; set; }

       
    }
}
