using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class QualInspectionParameterGroupDetailPagedQuery : PagerInfo
    {
        /// <summary>
        /// 全检检验参数组id
        /// </summary>
        public long ParameterGroupId { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string? ParameterCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string? ParameterName { get; set; }
    }
}
