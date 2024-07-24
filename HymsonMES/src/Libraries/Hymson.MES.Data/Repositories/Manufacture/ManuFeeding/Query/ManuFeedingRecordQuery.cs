using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Query
{
    public  class ManuFeedingRecordQuery
    {
        /// <summary>
        /// 工程 
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
       /// 物料台账id
       /// </summary>
        public long? MaterialStandingbookId { get; set;}
    }
}
