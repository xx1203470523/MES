using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// OQC检验参数组明细快照 查询参数
    /// </summary>
    public class QualOqcParameterGroupDetailSnapshootQuery
    {
        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// SiteId
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string? ParameterCode { get; set; }

        /// <summary>
        /// OQC检验参数组Id
        /// </summary>
        public long? ParameterGroupId { get; set; }

        /// <summary>
        /// 检验类型
        /// </summary>
        public OQCInspectionTypeEnum? InspectionType { get; set; }
    }
}
