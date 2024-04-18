using Hymson.Infrastructure;
using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Manufacture.ManuSFCScrap
{
    /// <summary>
    /// 部分报废实体
    /// </summary>
    public class ManuSFCPartialScrapDto
    {
        /// <summary>
        /// 流出工序
        /// </summary>
        public IEnumerable<BarcodeScrap> BarcodeScrapList { get; set; }
    }

    /// <summary>
    /// 报废条码信息
    /// </summary>
    public class BarcodeScrap
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 发现不合格代码
        /// </summary>
        public long FindProcedureId { get; set; }


        /// <summary>
        /// 流出工序
        /// </summary>
        public long OutFlowProcedureId { get; set; }

        /// <summary>
        /// 报废条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public long UnqualifiedId { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty { get; set; }
    }

    /// <summary>
    /// 部分报废导入模板模型
    /// </summary>
    public record ManuSFCPartialScrapExcelDto : BaseExcelDto
    {
        /// <summary>
        /// 发现不合格工序
        /// </summary>
        [EpplusTableColumn(Header = "发现不合格工序(必填)", Order = 1)]
        public string FindProcedureCode { get; set; }

        /// <summary>
        /// 流出不合格工序
        /// </summary>
        [EpplusTableColumn(Header = "流出不合格工序(必填)", Order = 2)]
        public string OutFlowProcedureCode { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        [EpplusTableColumn(Header = "不合格代码(必填)", Order = 3)]
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 产品序列码
        /// </summary>
        [EpplusTableColumn(Header = "产品序列码(必填)", Order = 4)]
        public string BarCode { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        [EpplusTableColumn(Header = "报废数量(必填)", Order = 5)]
        public decimal ScrapQty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [EpplusTableColumn(Header = "备注", Order = 6)]
        public string? Remark { get; set; }
    }
}
