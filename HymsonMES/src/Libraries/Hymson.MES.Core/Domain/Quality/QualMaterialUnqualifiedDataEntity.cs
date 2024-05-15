using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（车间物料不良记录）   
    /// qual_material_unqualified_data
    /// @author zhaoqing
    /// @date 2024-05-15 11:53:12
    /// </summary>
    public class QualMaterialUnqualifiedDataEntity : BaseEntity
    {
        /// <summary>
        /// 物料库存Id;wh_material_inventory的Id
        /// </summary>
        public long MaterialInventoryId { get; set; }

        /// <summary>
        /// 不良状态;1、打开 2、关闭
        /// </summary>
        public bool UnqualifiedStatus { get; set; }

        /// <summary>
        /// 不良备注
        /// </summary>
        public string UnqualifiedRemark { get; set; }

        /// <summary>
        /// 处置结果;1、放行 2、退料
        /// </summary>
        public bool? DisposalResult { get; set; }

        /// <summary>
        /// 处置时间
        /// </summary>
        public DateTime? DisposalTime { get; set; }

        /// <summary>
        /// 处置备注
        /// </summary>
        public string DisposalRemark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
