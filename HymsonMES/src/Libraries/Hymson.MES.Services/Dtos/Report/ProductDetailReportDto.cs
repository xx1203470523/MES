using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report
{
    public record ProductDetailReportDto : BaseEntityDto
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 查询类型
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string? StartDate { get; set; }

        /// <summary>
        /// 截至时间
        /// </summary>
        public string? EndDate { get; set; }

        /// <summary>
        /// 投入
        /// </summary>
        public decimal? FeedingQty { get; set; }

        /// <summary>
        /// 产出
        /// </summary>
        public decimal? OutputQty { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }

    /// <summary>
    /// 查询输出对象
    /// </summary>
    public record ProductDetailReportOutputDto : ProductDetailReportDto
    {

    }


    public class ProductDetailReportQueryDto : PagerInfo
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

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
        public int? Type { get; set; }

        /// <summary>
        /// 查询起始日期
        /// </summary>
        public string? StartDate { get; set; }

        /// <summary>
        /// 查询截至日期
        /// </summary>
        public string? EndDate { get; set; }

        /// <summary>
        /// 查询时间
        /// </summary>
        public DateTime[]? Date { get; set; }
    }
}
