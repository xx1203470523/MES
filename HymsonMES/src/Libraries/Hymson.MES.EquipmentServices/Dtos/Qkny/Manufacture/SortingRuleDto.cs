using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 分选规则
    /// </summary>
    public record SortingRuleDto : QknyBaseDto
    {
        /// <summary>
        /// 电芯条码
        /// </summary>
        public string Sfc { get; set; } = string.Empty;

        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductCode { get; set;} = string.Empty;
    }
}
