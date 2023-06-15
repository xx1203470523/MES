using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuFacePlateRepair.Query
{
    public class ManuFacePlateRepairQuery
    {
    }

    public class ManuSfcRepairDetailByProductBadIdQuery
    {
        /// <summary>
        /// 不合格录入ID 
        /// </summary>
        public long[] ProductBadId { get; set; }
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }

    public class GetManuSfcRepairBySFCQuery 
    {
        /// <summary>
        /// SFC 
        /// </summary>
        public string SFC { get; set; }
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }
    
}
