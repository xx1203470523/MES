using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.Requests.Rotor
{
    /// <summary>
    /// 转子线LMS物料数据
    /// </summary>
    public class RotorMaterialSync
    {
        /// <summary>
        /// 物料编号
        /// </summary>
        public string MaterialCode { get; set; } = string.Empty;

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; } = string.Empty;

        /// <summary>
        /// 物料规格
        /// </summary>
        public string MatSpecification { get; set; } = string.Empty;

        /// <summary>
        /// 物料类型
        /// </summary>
        public string MaterialType { get; set; } = string.Empty;

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; } = string.Empty;

        /// <summary>
        /// 计量单位
        /// </summary>
        public string MaterialUnit { get; set; } = string.Empty;

        /// <summary>
        /// 条码类型
        /// </summary>
        public string BarcodeType { get; set; } = string.Empty;

        /// <summary>
        /// 物料正则
        /// </summary>
        public string Regular { get; set; } = string.Empty;

        /// <summary>
        /// 批次验证规则
        /// </summary>
        public string BatchCodeRegular { get; set; } = string.Empty;

        /// <summary>
        /// 有效天数
        /// </summary>
        public int? EffectiveDays { get; set; }

        /// <summary>
        /// 预警数量
        /// </summary>
        public decimal? WarnAmount { get; set; }

        /// <summary>
        /// 是否启用
        /// 默认值为1，表示启用
        /// </summary>
        public bool Enable { get; set; } = true;
    }
}
