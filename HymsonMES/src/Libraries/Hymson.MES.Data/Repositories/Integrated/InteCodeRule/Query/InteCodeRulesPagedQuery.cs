/*
 *creator: Karl
 *
 *describe: 编码规则 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 编码规则 分页参数
    /// </summary>
    public class InteCodeRulesPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }

        /// <summary>
        /// 编码规则描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 编码类型;1：过程控制序列码；2：包装序列码；
        /// </summary>
        public CodeRuleCodeTypeEnum? CodeType { get; set; }
    }
}
