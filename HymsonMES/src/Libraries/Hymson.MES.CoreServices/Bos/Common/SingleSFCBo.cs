﻿namespace Hymson.MES.CoreServices.Bos.Common
{
    /// <summary>
    /// 单条码
    /// </summary>
    //[Obsolete("建议使用 MultiSFCBo")]
    public class SingleSFCBo
    {
        /// <summary>
        /// 工厂Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = "";

    }
}