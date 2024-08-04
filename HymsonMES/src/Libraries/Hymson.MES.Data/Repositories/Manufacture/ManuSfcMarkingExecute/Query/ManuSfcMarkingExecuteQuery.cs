namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// Marking执行表 查询参数
    /// </summary>
    public class ManuSfcMarkingExecuteQuery
    {
        /// <summary>
        /// 排序(默认为 CreatedOn DESC)
        /// </summary>
        public string Sorting { get; set; } = "CreatedOn DESC";

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// SFC
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// SFC列表
        /// </summary>
        public IEnumerable<string>? SFCs { get; set; }

        /// <summary>
        /// 发现不良工序
        /// </summary>
        public long? FoundBadProcedureId { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public long? UnqualifiedCodeId { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public IEnumerable<long>? UnqualifiedCodeIds { get; set; }
    }
}
