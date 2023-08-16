using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// IPQC检验项目 查询参数
    /// </summary>
    public class QualIpqcInspectionQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public IPQCTypeEnum Type { get; set; }

        /// <summary>
        /// 全检参数idqual_inspection_parameter_group 的id
        /// </summary>
        public long InspectionParameterGroupId { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
    }
}
