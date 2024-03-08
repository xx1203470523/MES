﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query
{
    /// <summary>
    /// 根据设备编码+资源编码查询 设备，资源，资源类型，工序，线体，车间 基础信息
    /// </summary>
    public class EquResAllQuery
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResCode { get; set; }
    }
}
