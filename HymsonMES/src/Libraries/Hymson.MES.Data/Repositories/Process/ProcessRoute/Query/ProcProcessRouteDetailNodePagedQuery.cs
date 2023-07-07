using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process.ProcessRoute.Query
{
    public class ProcProcessRouteDetailNodePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 工序ID（前工序ID）
        /// </summary>
        public long? ProcedureId { get; set; }
    }
}
