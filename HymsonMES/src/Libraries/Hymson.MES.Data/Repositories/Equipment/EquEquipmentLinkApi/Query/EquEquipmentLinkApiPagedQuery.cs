﻿using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class EquEquipmentLinkApiPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public string? SiteCode { get; set; }

    }
}
