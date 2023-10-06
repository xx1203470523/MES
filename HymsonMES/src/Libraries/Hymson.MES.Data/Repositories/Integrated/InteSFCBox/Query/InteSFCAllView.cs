using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Integrated.InteSFCBox.Query
{
    public class InteSFCAllView
    {
        /// <summary>
        /// 批次码
        /// </summary>
        public string BatchNo { get; set; }
        public string Grade { get; set; }
        /// <summary>
        /// OCVB最小值
        /// </summary>
        public decimal OCVBDiff { get; set; }
        /// <summary>
        /// IMPB最大值
        /// </summary>
        public decimal MaxIMPB { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        //public long? SiteId { get; set; }
    }
}
