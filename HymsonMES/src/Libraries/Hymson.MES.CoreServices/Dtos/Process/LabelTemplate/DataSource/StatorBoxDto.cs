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
    /// 定子装箱数据源
    /// </summary>
    [Description("定子装箱打印数据模板")]
    public record StatorBoxDto : BasePrintData
    {
        /// <summary>
        /// 箱体码
        /// </summary>
        [Description("箱体码")]
        public string BoxCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        [Description("料号")]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        [Description("产品名称")]
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料型号
        /// </summary>
        [Description("规格型号")]
        public string MaterialModel { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Description("数量")]
        public int Num { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [Description("单位")]
        public string MaterialUnit { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        [Description("生产日期")]
        public string ProductionDate { get; set; }
    }
}
