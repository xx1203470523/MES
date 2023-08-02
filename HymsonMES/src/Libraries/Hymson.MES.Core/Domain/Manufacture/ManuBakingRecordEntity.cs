/*
 *creator: Karl
 *
 *describe: 烘烤执行表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  wxk
 *build datetime: 2023-08-02 07:32:33
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 烘烤执行表，数据实体对象   
    /// manu_baking_record
    /// @author wxk
    /// @date 2023-08-02 07:32:33
    /// </summary>
    public class ManuBakingRecordEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 烘烤Id
        /// </summary>
        public long BakingId { get; set; }

       /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }

       /// <summary>
        /// 烘烤开始时间
        /// </summary>
        public DateTime BakingStart { get; set; }

       /// <summary>
        /// 烘烤结束时间
        /// </summary>
        public DateTime? BakingEnd { get; set; }

       /// <summary>
        /// 烘烤计划时长
        /// </summary>
        public int? BakingPlan { get; set; }

       /// <summary>
        /// 烘烤时长
        /// </summary>
        public int? BakingExecution { get; set; }

       /// <summary>
        /// 烘烤状态：0 烘烤中，1 正常结束，2终止
        /// </summary>
        public bool Status { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       
    }
}
