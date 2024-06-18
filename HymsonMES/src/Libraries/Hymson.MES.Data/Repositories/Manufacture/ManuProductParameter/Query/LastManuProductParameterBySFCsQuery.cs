using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuProductParameter.Query
{
    /// <summary>
    /// 获取
    /// </summary>
    public class LastManuProductParameterBySFCsQuery
    {

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public IEnumerable<string> SFCs { get; set; }
    }
}
