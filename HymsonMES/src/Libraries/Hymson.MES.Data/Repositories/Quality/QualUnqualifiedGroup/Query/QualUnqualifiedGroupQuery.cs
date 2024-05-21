namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    ///不合代码组查询参数
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class QualUnqualifiedGroupQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 物料组IdId
        /// </summary>
        public long? MaterialGroupId { get; set; }
    }
}
