﻿using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Dtos.Parameter
{
    /// <summary>
    /// 参数采集实体类
    /// </summary>
    public record EquipmentParameterDto : BaseEntityDto
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 参数Id
        /// </summary>
        public long ParameterId { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParameterValue { get; set; } = "";

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectionTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; } = "";

        public DateTime Date { get; set; }
    }
}
