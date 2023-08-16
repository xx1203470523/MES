using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Dtos.Parameter
{
    /// <summary>
    /// 按照工序查询查询
    /// </summary>
    public  class QueryParameterByProcedureDto
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 条码集合
        /// </summary>
        public IEnumerable<string>  SFCs { get; set;}
    }
}
