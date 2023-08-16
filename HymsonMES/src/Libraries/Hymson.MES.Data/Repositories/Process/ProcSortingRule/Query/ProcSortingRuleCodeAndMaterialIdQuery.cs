using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process.ProcSortingRule.Query
{
    /// <summary>
    /// 根据编码和物料获取分选规则
    /// </summary>
    public class ProcSortingRuleCodeAndMaterialIdQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

    }
}
