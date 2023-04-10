﻿namespace Hymson.MES.Data.Repositories.Common.Command
{
    /// <summary>
    /// 实体（更新）
    /// </summary>
    public class UpdateCommand
    {
        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; } = "";

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }
}