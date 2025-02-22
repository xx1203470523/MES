﻿using Hymson.MES.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process.LoadPoint.View
{
    /// <summary>
    /// 上料点或设备数据
    /// </summary>
    public class PointOrEquipmentView
    {
        /// <summary>
        /// 数据类型 1-上料点 2-设备
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 设备或者上料点ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResId { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResName { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
    }

    /// <summary>
    /// 上料点物料
    /// </summary>
    public class ProcLoadPointMaterialView : ProcLoadPointEntity
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialID { get; set; }
    }
}
