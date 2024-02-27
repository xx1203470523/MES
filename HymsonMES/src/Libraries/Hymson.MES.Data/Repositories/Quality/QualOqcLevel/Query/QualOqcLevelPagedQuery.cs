using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// OQC检验水平 分页参数
    /// </summary>
    public class QualOqcLevelPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public IEnumerable<long>? MaterialIds { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public IEnumerable<long>? CustomIds { get; set; }

        /// <summary>
        /// 设置类型 1、通用 2、物料
        /// </summary>
        public QCMaterialTypeEnum? Type { get; set; }

        /// <summary>
        /// 状态 0、已禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

    }
}
