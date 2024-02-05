﻿using Hymson.Infrastructure;

namespace Hymson.MES.EquipmentServices
{
    /// <summary>
    /// 
    /// </summary>
    public record BaseDto : BaseEntityDto
    {
        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; } = "";

        /// <summary>
        /// 设备调用本地时间
        /// </summary>
        public DateTime LocalTime { get; set; }
    }
}
