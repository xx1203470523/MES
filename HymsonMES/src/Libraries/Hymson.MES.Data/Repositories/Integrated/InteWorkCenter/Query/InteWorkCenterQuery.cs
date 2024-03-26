using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.Query
{
    public class InteWorkCenterQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工作中心编码列表
        /// </summary>
        public string[]? Codes { get; set; }
    }
}
