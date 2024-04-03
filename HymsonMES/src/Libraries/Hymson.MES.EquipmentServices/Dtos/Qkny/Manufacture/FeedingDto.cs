using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 原材料上料
    /// </summary>
    public record FeedingDto : QknyBaseDto
    {
        /// <summary>
        /// 上料条码
        /// </summary>
        public string Sfc { get; set; } = "";

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 是否上料点
        /// </summary>
        public bool IsFeedingPoint { get; set; } = false;
    }
}
