using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{

    /// <summary>
    /// 配方版本校验
    /// </summary>
    public record FormulaVersionExamineDto : QknyBaseDto
    {
        /// <summary>
        /// 配方
        /// </summary>
        public string FormulaCode { get; set; } = "";

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = "";
    }
}
