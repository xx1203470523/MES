using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 获取条码信息
    /// </summary>
    public record GetSfcInfoDto : QknyBaseDto
    {
        /// <summary>
        /// 条码泪飙
        /// </summary>
        public List<string> SfcList { get; set; } = new List<string>();
    }
}
