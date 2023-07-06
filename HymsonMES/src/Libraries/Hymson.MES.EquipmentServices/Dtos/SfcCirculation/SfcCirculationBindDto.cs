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
    public record SfcCirculationBindDto : BaseDto
    {
        /// <summary>
        /// 模组/Pack条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;
        /// <summary>
        /// 模组绑电芯条码/Pack绑模组条码
        /// </summary>
        public CirculationBindDto[] BindSFCs { get; set; }

        /// <summary>
        /// 是否为模组虚拟条码参数
        /// 为兼容永泰虚拟条码场景
        /// </summary>
        public bool IsVirtualSFC { get; set; } = false;
    }

    public class CirculationBindDto
    {
        /// <summary>
        /// 绑定位置
        /// </summary>
        public int Location { get; set; } = 0;
        /// <summary>
        /// 绑定的条码
        /// </summary>
        public string SFC { get; set; }
    }
}
