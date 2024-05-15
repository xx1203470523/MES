using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment.Qkny.ManuEuqipmentNewestInfo.View
{
    /// <summary>
    /// 最新状态信息
    /// </summary>
    public class ManuEquipmentNewestInfoView
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 心跳最后更新时间
        /// </summary>
        public DateTime HeartUpdatedOn { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 状态更新时间
        /// </summary>
        public string StatusUpdatedOn { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentCode { get; set; }
    }
}
