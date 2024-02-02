namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 解除锁定
    /// </summary>
    public  class UnLockedProcedureCommand
    {
        public long SiteId { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public IEnumerable<string> Sfcs { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }
}
