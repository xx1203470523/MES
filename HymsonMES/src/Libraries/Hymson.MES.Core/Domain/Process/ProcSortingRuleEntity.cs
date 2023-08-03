/*
 *creator: Karl
 *
 *describe: 分选规则    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:24:54
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 分选规则，数据实体对象   
    /// proc_sorting_rule
    /// @author zhaoqing
    /// @date 2023-07-25 03:24:54
    /// </summary>
    public class ProcSortingRuleEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 规则编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 规则名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
       
    }
}
