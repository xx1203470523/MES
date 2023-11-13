using Hymson.Excel.Abstractions.Attributes;
using Hymson.Infrastructure;
using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report.Excel
{
    /// <summary>
    /// 产品参数导出
    /// </summary>
    [SheetDescriptionAttribute("产能报表")]
    public record ProductDetailExportDto : BaseExcelDto
    {
        /// <summary>
        /// 工单号
        /// </summary>
        [EpplusTableColumn(Header = "工单号", Order = 1)]
        public string? OrderCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        [EpplusTableColumn(Header = "物料编码", Order = 2)]
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        [EpplusTableColumn(Header = "物料名称", Order = 3)]
        public string? MaterialName { get; set; }

        /// <summary>
        /// 查询类型
        /// </summary>
        [EpplusTableColumn(Header = "查询类型", Order = 4)]
        public string? Type { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [EpplusTableColumn(Header = "开始时间", Order = 5)]
        public string? StartDate { get; set; }

        /// <summary>
        /// 截至时间
        /// </summary>
        [EpplusTableColumn(Header = "截止时间", Order = 6)]
        public string? EndDate { get; set; }

        /// <summary>
        /// 投入
        /// </summary>
        [EpplusTableColumn(Header = "投入数量", Order = 7)]
        public decimal? FeedingQty { get; set; }

        /// <summary>
        /// 产出
        /// </summary>
        [EpplusTableColumn(Header = "产出数", Order = 8)]
        public decimal? OutputQty { get; set; }
    }
}
