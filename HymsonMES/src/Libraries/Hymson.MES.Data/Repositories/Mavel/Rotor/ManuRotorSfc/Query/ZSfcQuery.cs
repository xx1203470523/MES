using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Mavel.Rotor.ManuRotorSfc.Query
{
    /// <summary>
    /// 轴码查询
    /// </summary>
    public class ZSfcQuery
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public List<string> SfcList { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
    }
}
