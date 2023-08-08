/*
 *creator: Karl
 *
 *describe: 降级规则    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-08-07 02:00:57
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 降级规则，数据实体对象   
    /// manu_downgrading_rule
    /// @author Karl
    /// @date 2023-08-07 02:00:57
    /// </summary>
    public class ManuDowngradingRuleEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 序号
        /// </summary>
        public int SerialNumber { get; set; }

       /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       
    }
}
