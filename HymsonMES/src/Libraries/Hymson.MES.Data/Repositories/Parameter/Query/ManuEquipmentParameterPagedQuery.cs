﻿using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Parameter.Query
{
    /// <summary>
    /// 设备参数分页查询 按设备Id分表
    /// </summary>
    public class ManuEquipmentParameterPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 参数Id
        /// </summary>
        public long? ParameterId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime[] CreatedOn { get; set; }

    }
}
