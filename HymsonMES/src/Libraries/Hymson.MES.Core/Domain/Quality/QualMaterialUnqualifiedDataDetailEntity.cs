using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（车间物料不良记录明细）   
    /// qual_material_unqualified_data_detail
    /// @author zhaoqing
    /// @date 2024-05-15 11:53:21
    /// </summary>
    public class QualMaterialUnqualifiedDataDetailEntity : BaseEntity
    {
        /// <summary>
        /// 车间物料不良ID;qual_material_unqualified_data的Id
        /// </summary>
        public long MaterialUnqualifiedDataId { get; set; }

        /// <summary>
        /// 不合格代码组ID;qual_unqualified_group的Id
        /// </summary>
        public long UnqualifiedGroupId { get; set; }

        /// <summary>
        /// 不合格代码ID;qual_unqualified_code的Id
        /// </summary>
        public long UnqualifiedCodeId { get; set; }

        
    }
}
