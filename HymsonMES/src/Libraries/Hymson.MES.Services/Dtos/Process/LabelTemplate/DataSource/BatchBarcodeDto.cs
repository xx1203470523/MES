using Hymson.Print.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process.LabelTemplate.DataSource
{
    /// <summary>
    ///批次条码数据源
    /// </summary>
    [Description("批次条码打印数据模板")]
    public record BatchBarcodeDto : BasePrintData
    {
        /// <summary>
        /// 条码
        /// </summary>
        [Description("条码")]
        public string SFC { get; set; }

        /// <summary>
        /// 物料代码
        /// </summary>
        [Description("物料代码")]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        [Description("物料名称")]
        public string MaterialName { get; set; }

        /// <summary>
        /// 生产批次
        /// </summary>
        [Description("生产批次")]
        public string ProductionBatch { get; set; }

        /// <summary>
        /// 生产班次
        /// </summary>
        [Description("生产班次")]
        public string ProductionShift { get; set; }

        /// <summary>
        /// 收容数量
        /// </summary>
        [Description("收容数量")]
        public decimal Qty { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        [Description("流水号")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// 托/箱号
        /// </summary>
        [Description("托/箱号")]
        public string VehicleCode { get; set; }
    }
}
