using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process.Parameter.Query
{
    /// <summary>
    ///更具编码查询参数
    /// </summary>
    public  class ProcParametersByCodeQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 编码集合
        /// </summary>
        public IEnumerable<string > Codes { get; set; }
    }
}
