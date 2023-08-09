using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.SfcCirculation
{
    /// <summary>
    /// 解绑CCS
    /// </summary>
    public record SfcCirculationCCSUnBindDto : BaseDto
    {
        /// <summary>
        /// 模组条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        /// 解绑的条码列表
        /// 为空解绑所有
        /// </summary>
        public string[] UnBindSFCs { get; set; } = Array.Empty<string>();
    }
}
