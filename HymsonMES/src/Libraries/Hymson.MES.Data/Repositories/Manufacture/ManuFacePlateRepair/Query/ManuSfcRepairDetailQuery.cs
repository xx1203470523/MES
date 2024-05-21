using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuFacePlateRepair.Query
{
    public class ManuSfcRepairDetailQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 维修主表id
        /// </summary>
        public IEnumerable<long>? SfcRepairIds { get; set; }
    }
}
