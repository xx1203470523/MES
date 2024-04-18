using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{

    /// <summary>
    /// 工装寿命上报
    /// </summary>
    public record ToolLifeDto : QknyBaseDto
    {
        /// <summary>
        /// 工装编码
        /// </summary>
        public string ToolCode { get; set; } = "";

        /// <summary>
        /// 已使用寿命
        /// </summary>
        public decimal UsedLife { get; set; }
    }
}
