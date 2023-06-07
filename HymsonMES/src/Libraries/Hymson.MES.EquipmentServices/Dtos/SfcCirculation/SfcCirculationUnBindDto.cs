using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.SfcCirculation
{
    /// <summary>
    /// 绑定条码流转表
    /// </summary>
    public record SfcCirculationUnBindDto : BaseDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        /// 解绑的条码列表
        /// </summary>
        public string[] UnBindSFCs { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 是否为模组虚拟条码参数
        /// 为兼容永泰虚拟条码场景
        /// </summary>
        public bool? IsVirtualSFC { get; set; } = false;
    }
}
