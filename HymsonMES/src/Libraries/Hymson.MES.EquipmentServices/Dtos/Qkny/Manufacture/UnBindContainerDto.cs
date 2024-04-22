using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 托盘解绑
    /// </summary>
    public record UnBindContainerDto : QknyBaseDto
    {
        /// <summary>
        /// 托盘码
        /// </summary>
        public string ContainCode { get; set; } = "";

        /// <summary>
        /// 电芯条码
        /// </summary>
        public List<string> SfcList { get; set; } = new List<string>();
    }
}
