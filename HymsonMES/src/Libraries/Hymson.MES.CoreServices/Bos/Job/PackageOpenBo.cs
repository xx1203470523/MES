﻿using Hymson.MES.Core.Domain.Manufacture;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 
    /// </summary>
    public class PackageCloseRequestBo : JobBaseBo
    {

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public long ContainerId { get; set; }

    }

    /// <summary>
    ///  
    /// </summary>
    public class PackageCloseResponseBo
    {
        /// <summary>
        /// 内容
        /// </summary>
        public Dictionary<string, string>? Content { get; set; } = new();

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; } = "";

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; } = HymsonClock.Now();

        /// <summary>
        /// 容器条码信息
        /// </summary>
        public ManuContainerBarcodeEntity ManuContainerBarcode { get; set; } = new ManuContainerBarcodeEntity();
    }

}
