﻿namespace Hymson.MES.Data.Repositories.Common.Query
{
    /// <summary>
    /// code查询实体
    /// </summary>
    public  class EntityByCodeQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long? Site { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
    }
}
