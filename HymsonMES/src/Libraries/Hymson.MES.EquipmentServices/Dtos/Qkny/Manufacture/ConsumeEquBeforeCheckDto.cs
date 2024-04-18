using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 设备投料前校验(制胶匀浆)
    /// </summary>
    public record ConsumeEquBeforeCheckDto : QknyBaseDto
    {
        /// <summary>
        /// 投料至设备
        /// </summary>
        public string ConsumeEquipmentCode { get; set; } = "";

        /// <summary>
        /// 投料至资源
        /// </summary>
        public string ConsumeResourceCode { get; set; } = "";

        /// <summary>
        /// 待投料条码集合
        /// </summary>
        public List<string> SfcList { get; set; } = new List<string>();
    }
}
