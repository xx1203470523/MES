using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// 车间物料不良记录 分页参数
    /// </summary>
    public class QualMaterialUnqualifiedDataPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string? MaterialBarCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 不合格组
        /// </summary>
        public string? UnqualifiedGroup { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string? UnqualifiedCode { get; set; }

        /// <summary>
        /// 创建时间数组 ：时间范围 
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }

        /// <summary>
        /// 处置时间数组 ：时间范围 
        /// </summary>
        public DateTime[]? DisposalTime { get; set; }

        /// <summary>
        /// 不良状态;1、打开 2、关闭
        /// </summary>
        public QualMaterialUnqualifiedStatusEnum? UnqualifiedStatus { get; set; }

        /// <summary>
        /// 处置结果;1、放行 2、退料
        /// </summary>
        public QualMaterialDisposalResultEnum? DisposalResult { get; set; }
    }
}
