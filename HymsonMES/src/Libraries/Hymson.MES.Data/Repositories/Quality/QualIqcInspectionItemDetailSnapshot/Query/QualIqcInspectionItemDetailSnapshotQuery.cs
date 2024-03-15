using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// IQC检验项目详情快照表 查询参数
    /// </summary>
    public class QualIqcInspectionItemDetailSnapshotQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 快照表ID
        /// </summary>
        public long? IqcInspectionItemSnapshotId { get; set; }

        /// <summary>
        /// 检验类型;1、常规检验2、外观检验3、包装检验4、特殊性检验5、破坏性检验
        /// </summary>
        public IQCInspectionTypeEnum? InspectionType { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public string? ParameterCode { get; set; }

    }
}
