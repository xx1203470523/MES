/*
 *creator: Karl
 *
 *describe: 降级品录入记录    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-08-10 10:15:49
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 降级品录入记录，数据实体对象   
    /// manu_downgrading_record
    /// @author Karl
    /// @date 2023-08-10 10:15:49
    /// </summary>
    public class ManuDowngradingRecordEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 品级
        /// </summary>
        public string Grade { get; set; }

       /// <summary>
        /// 是否取消;0 否 1是
        /// </summary>
        public ManuDowngradingRecordTypeEnum? IsCancellation { get; set; }

    }
}
