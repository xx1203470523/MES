using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{

    /// <summary>
    /// 配方明细获取
    /// </summary>
    public record FormulaDetailGetDto : QknyBaseDto
    {
        /// <summary>
        /// 配方编码
        /// </summary>
        public string FormulaCode { get; set; } = "";
    }
}
