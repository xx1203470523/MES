using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// 车间物料不良记录 查询参数
    /// </summary>
    public class QualMaterialUnqualifiedDataQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料库存Id
        /// </summary>
        public long? MaterialInventoryId { get; set; }

        /// <summary>
        /// 不良状态;1、打开 2、关闭
        /// </summary>
        public QualMaterialUnqualifiedStatusEnum? UnqualifiedStatus { get; set; }
    }
}
