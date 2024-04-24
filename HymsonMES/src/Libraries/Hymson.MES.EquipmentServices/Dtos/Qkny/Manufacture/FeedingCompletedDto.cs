using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 上料完成(制胶匀浆)
    /// </summary>
    public record FeedingCompletedDto : QknyBaseDto
    {
        /// <summary>
        /// 上料前重量
        /// </summary>
        public decimal BeforeFeedingQty { get; set; }

        /// <summary>
        /// 上料后重量
        /// </summary>
        public decimal AfterFeedingQty { get; set; }
    }
}
