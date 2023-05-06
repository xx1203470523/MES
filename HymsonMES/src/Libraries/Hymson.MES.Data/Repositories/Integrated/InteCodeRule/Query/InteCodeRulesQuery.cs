/*
 *creator: Karl
 *
 *describe: 编码规则 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:26
 */

using Hymson.MES.Core.Enums.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 编码规则 查询参数
    /// </summary>
    public class InteCodeRulesQuery
    {
        /// <summary>
        /// 产品id  物料
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 编码类型;1：过程控制序列码；2：包装序列码；
        /// </summary>
        public CodeRuleCodeTypeEnum? CodeType { get; set; }

    }
}
