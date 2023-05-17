using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.FeedingConsumption
{
    /// <summary>
    /// 上报物料消耗
    /// </summary>
    public record FeedingConsumptionDto : BaseDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        ///数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 是否是上料点上料
        /// </summary>
        public bool IsFeedingPoint { get; set; }

    }
}
