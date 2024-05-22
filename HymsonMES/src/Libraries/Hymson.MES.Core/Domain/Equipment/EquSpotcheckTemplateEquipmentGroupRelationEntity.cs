/*
 *creator: Karl
 *
 *describe: 设备点检模板与设备组关系    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2024-05-13 03:22:22
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.EquSpotcheckTemplateEquipmentGroupRelation
{
    /// <summary>
    /// 设备点检模板与设备组关系，数据实体对象   
    /// equ_spotcheck_template_equipment_group_relation
    /// @author pengxin
    /// @date 2024-05-13 03:22:22
    /// </summary>
    public class EquSpotcheckTemplateEquipmentGroupRelationEntity : BaseEntity
    {
        /// <summary>
        /// 点检模板ID;equ_spotcheck_template的Id
        /// </summary>
        public long SpotCheckTemplateId { get; set; }

       /// <summary>
        /// 设备组ID;equ_equipment_group的Id
        /// </summary>
        public long EquipmentGroupId { get; set; }

       
    }
}
