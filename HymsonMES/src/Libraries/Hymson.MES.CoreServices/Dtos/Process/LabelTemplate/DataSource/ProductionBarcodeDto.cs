using Hymson.Print.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.DataSource
{
    /// <summary>
    ///生产条码数据源
    /// </summary>
    [Description("生产条码打印数据模板")]
    public record ProductionBarcodeDto : BasePrintData
    {
        /// <summary>
        /// 条码
        /// </summary>
        [Description("条码")]
        public string SFC { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        [Description("工单号")]
        public string WorkOrderCode { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        [Description("产品编码")]
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Description("产品名称")]
        public string ProductName { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Description("条码数量")]
        public decimal Qty { get; set; }

        /// <summary>
        /// 合格数量
        /// </summary>
        [Description("合格数量")]
        public decimal QualifiedQty { get; set; }

        /// <summary>
        /// 不良数量
        /// </summary>
        [Description("不良数量")]
        public decimal UnQualifiedQty { get; set; }

        /// <summary>
        /// 所在线体编码
        /// </summary>
        [Description("线体编码")]
        public string? WorkCenterLineCode { get; set; }

        /// <summary>
        /// 所在线体名称
        /// </summary>
        [Description("线体名称")]
        public string? WorkCenterLineName { get; set; }

        /// <summary>
        /// 所在工序编码
        /// </summary>
        [Description("产出工序编码")]
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 所在工序名称
        /// </summary>
        [Description("产出工序名称")]
        public string? ProcedureName { get; set; }

        /// <summary>
        /// 所在资源编码
        /// </summary>
        [Description("产出资源编码")]
        public string? ResourceCode { get; set; }

        /// <summary>
        /// 所在资源名称
        /// </summary>
        [Description("产出资源名称")]
        public string? ResourceName { get; set; }

        /// <summary>
        /// 所在设备编码
        /// </summary>
        [Description("产出设备编码")]
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 所在设备名称
        /// </summary>
        [Description("产出设备名称")]
        public string? EquipmentName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        public string Status { get; set; }

        /// <summary>
        ///生产日期
        /// </summary>
        [Description("生产日期")]
        public string? LasttOutputTime { get; set; }

        /// <summary>
        /// 打印时间
        /// </summary>
        [Description("打印时间")]
        public string? PrintTime { get; set; }

        /// <summary>
        /// 有效时间
        /// </summary>
        [Description("有效时间")]
        public string? ExpirationDate { get; set; }


        /// <summary>
        /// 物料批次 取产品条码生成时的日期
        /// </summary>
        [Description("物料批次")]
        public string? ProductionBatch { get; set; }
    }
}
