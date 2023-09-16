using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Integrated.InteSFCBox.Query
{
    public class InteSFCBoxQueryRep : PagerInfo
    {
        /// <summary>
        /// 电芯码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 箱码
        /// </summary>
        public string? BoxCode { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; }
    }
}
