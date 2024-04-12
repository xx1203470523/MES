using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// FQC检验参数组明细快照 查询参数
    /// </summary>
    public class QualFqcParameterGroupDetailSnapshootQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 快照表ID
        /// </summary>
        public long? ParameterGroupId { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public string? ParameterCode { get; set; }
    }
}
