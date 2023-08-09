using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Parameter.ManuProductParameter.Query
{
    public  class ManuProductParameterBySfcQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码属性
        /// </summary>
        public  IEnumerable<string> SFCs { get; set; }
    }
}
