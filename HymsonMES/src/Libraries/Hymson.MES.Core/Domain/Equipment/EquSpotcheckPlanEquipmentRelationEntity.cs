/*
 *creator: Karl
 *
 *describe: 设备点检计划与设备关系    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2024-05-20 03:08:03
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.EquSpotcheckPlanEquipmentRelation
{
    /// <summary>
    /// 设备点检计划与设备关系，数据实体对象   
    /// equ_spotcheck_plan_equipment_relation
    /// @author pengxin
    /// @date 2024-05-20 03:08:03
    /// </summary>
    public class EquSpotcheckPlanEquipmentRelationEntity : BaseEntity
    {
        /// <summary>
        /// 点检计划ID;equ_spotcheck_plan表的Id
        /// </summary>
        public long SpotCheckPlanId { get; set; }

       /// <summary>
        /// 点检模板ID;equ_spotcheck_template表的Id
        /// </summary>
        public long SpotCheckTemplateId { get; set; }

       /// <summary>
        /// 设备ID;equ_equipment的Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 点检执行人;用户中心UserId集合
        /// </summary>
        public string? ExecutorIds { get; set; }

        /// <summary>
        /// 点检负责人;用户中心UserId集合
        /// </summary>
        public string? LeaderIds { get; set; }
    }
}
