using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 卷绕极组产出
    /// </summary>
    public record CollingPolarDto : QknyBaseDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; } = string.Empty;

        /// <summary>
        /// 0：不合格； 1：合格
        /// </summary>
        public int Passed { get; set; }

        /// <summary>
        /// 不良原因
        /// </summary>
        public List<string> NgList { get; set; } = new List<string>();
    }
}
