using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（工具类型和物料关系表）   
    /// equ_tools_type_material_relation
    /// @author zhaoqing
    /// @date 2024-07-09 05:01:24
    /// </summary>
    public class EquToolsTypeMaterialRelationEntity : BaseEntity
    {
        /// <summary>
        /// 类型id equ_tools_type的id
        /// </summary>
        public long ToolTypeId { get; set; }

        /// <summary>
        /// 物料id proc_material 的id
        /// </summary>
        public long MaterialId { get; set; }

        
    }
}
