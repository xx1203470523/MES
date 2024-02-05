using Hymson.MES.Core.Domain.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipment
{
    /// <summary>
    /// 设备注册 View
    /// </summary>
    public class EquEquipmentPageView: EquEquipmentEntity
    {
        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkCenterShopCode { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkCenterShopName { get; set; }

    }
}
