/*
 *creator: Karl
 *
 *describe: 烘烤执行表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  wxk
 *build datetime: 2023-07-28 05:42:41
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 烘烤执行表，数据实体对象   
    /// manu_baking_record
    /// @author wxk
    /// @date 2023-07-28 05:42:41
    /// </summary>
    public class ManuBakingRecordEntity : BaseEntity
    {
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

       
    }
}
