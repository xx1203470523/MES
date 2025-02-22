﻿using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Parameter
{
    /// <summary>
    /// 产品工序参数
    /// </summary>
    public class ManuProductParameterEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

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
        ///步骤表id
        /// </summary>
        public long? SfcstepId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; } = 0;

        /// <summary>
        /// 产品参数组id
        /// </summary>
        public long ParameterGroupId { get; set; } = 0;
    }
}
