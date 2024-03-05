using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 获取配方
    /// </summary>
    public record FormulaListGetDto : QknyBaseDto
    {
        /// <summary>
        /// 产品型号;为空则返回所有的，不为空则返回对应产品的
        /// </summary>
        public string ProductCode { get; set; } = "";
    }
}
