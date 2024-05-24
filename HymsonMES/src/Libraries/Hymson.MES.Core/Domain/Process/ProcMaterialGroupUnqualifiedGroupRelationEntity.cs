using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据实体（物料组与不合格代码组关系）   
    /// proc_material_group_unqualified_group_relation
    /// @author zhaoqing
    /// @date 2024-05-15 11:53:03
    /// </summary>
    public class ProcMaterialGroupUnqualifiedGroupRelationEntity : BaseEntity
    {
        /// <summary>
        /// 物料组ID;proc_material_group的Id
        /// </summary>
        public long MaterialGroupId { get; set; }

        /// <summary>
        /// 不合格代码组ID;qual_unqualified_group的Id
        /// </summary>
        public long UnqualifiedGroupId { get; set; }

        
    }
}
