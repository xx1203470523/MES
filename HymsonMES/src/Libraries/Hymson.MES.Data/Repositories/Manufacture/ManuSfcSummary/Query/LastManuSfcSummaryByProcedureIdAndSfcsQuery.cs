using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcSummary.Query
{
    /// <summary>
    /// 查询工序条码最后数据
    /// </summary>
    public  class LastManuSfcSummaryByProcedureIdAndSfcsQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        //public long SiteId { get; set; }

        /// <summary>
        /// 条码集合
        /// </summary>
        public  IEnumerable<string>  Sfcs { get; set; }
    }
}
