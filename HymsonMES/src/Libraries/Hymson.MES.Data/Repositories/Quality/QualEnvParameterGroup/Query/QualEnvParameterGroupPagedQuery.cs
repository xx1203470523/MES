using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// 环境检验参数表 分页参数
    /// </summary>
    public class QualEnvParameterGroupPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 参数集编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 参数集名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 工作中心编码（车间或者线体）
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心名称（车间或者线体）
        /// </summary>
        public string? WorkCenterName { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string? ProcedureName { get; set; }

    }
}
