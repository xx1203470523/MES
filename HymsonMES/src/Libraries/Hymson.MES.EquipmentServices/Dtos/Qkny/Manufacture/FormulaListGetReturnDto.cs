using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{

    /// <summary>
    /// 配方列表获取
    /// </summary>
    public record FormulaListGetReturnDto
    {
        /// <summary>
        /// 配方编码
        /// </summary>
        public string FormulaCode { get; set; } = "";

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = "";

        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductCode { get; set; } = "";

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateOnTime { get; set; }
    }
}
