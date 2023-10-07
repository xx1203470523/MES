using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;
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
        /// 批次号
        /// </summary>
        public string? BatchNo { get; set; }

        /// <summary>
        /// 档位
        /// </summary>
        public string? Grade { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SFCBoxEnum? Status { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; }
    }

    public class InteSFCBoxEntityQuery
    {
        /// <summary>
        /// 电芯码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 电芯集合
        /// </summary>
        public string[]? SFCs { get; set; }

        /// <summary>
        /// 箱码
        /// </summary>
        public string? BoxCode { get; set; }

    }
}
