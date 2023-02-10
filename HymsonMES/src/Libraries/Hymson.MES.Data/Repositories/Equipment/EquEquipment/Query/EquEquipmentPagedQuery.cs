﻿using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class EquEquipmentPagedQuery : PagerInfo
    {
        /// <summary>
        /// 编码（设备）
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 名称（设备）
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 类型（设备）
        /// </summary>
        public string EquipmentType { get; set; }

        /// <summary>
        /// 使用状态（设备）
        /// </summary>
        public string UseStatus { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkCenterShopName { get; set; }

        /// <summary>
        /// 使用部门
        /// </summary>
        public string UseDepartment { get; set; }

        /// <summary>
        /// 存放位置
        /// </summary>
        public string Location { get; set; }
    }
}
