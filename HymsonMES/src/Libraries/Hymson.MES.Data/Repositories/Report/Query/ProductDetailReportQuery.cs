using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Report
{
    public class ProductDetailReportQuery : PagerInfo
    {
        /// <summary>
        /// 产线Id
        /// </summary>
        public string? WorkCenterId { get; set; }

        /// <summary>
        /// 产线编码
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public long? OrderId { get; set; }

        /// <summary>
        /// 查询日期类型（日月年）
        /// </summary>
        public int? Type { get; set; } = 13;

        /// <summary>
        /// 查询日期类型（日月年）
        /// </summary>
        public string? SearchType { get; set; }

        /// <summary>
        /// 查询起始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 查询截至日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 查询截至日期
        /// </summary>
        public DateTime[]? Date { get; set; }
    }
}
