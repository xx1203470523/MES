using Hymson.MES.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process.ProcSortingRule.View
{
    /// <summary>
    /// 分选规则详情
    /// 给设备接口用
    /// </summary>
    public class ProcSortRuleDetailEquView : ProcSortingRuleDetailEntity
    {
        /// <summary>
        /// 分选规则编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 分选规则名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }
    }
}
