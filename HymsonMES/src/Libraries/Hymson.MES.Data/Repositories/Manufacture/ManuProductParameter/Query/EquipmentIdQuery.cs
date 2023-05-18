﻿namespace Hymson.MES.Data.Repositories.Manufacture.ManuProductParameter.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class EquipmentIdQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

    }
}
