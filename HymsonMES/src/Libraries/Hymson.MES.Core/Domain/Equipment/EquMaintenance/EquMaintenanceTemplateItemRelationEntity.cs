using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment.EquMaintenance
{
    /// <summary>
    /// 设备点检模板与项目关系，数据实体对象   
    /// equ_maintenance_template_item_relation
    /// @author pengxin
    /// @date 2024-05-23 03:22:39
    /// </summary>
    public class EquMaintenanceTemplateItemRelationEntity : BaseEntity 
    {
        /// <summary>
        /// 点检模板ID;equ_maintenance_template的Id
        /// </summary>
        public long MaintenanceTemplateId { get; set; }

        /// <summary>
        /// 点检项目ID;equ_spotcheck_item的Id
        /// </summary>
        public long MaintenanceItemId { get; set; } 

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal? LowerLimit { get; set; }

        /// <summary>
        /// 规格值（规格中心）
        /// </summary>
        public decimal? Center { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal? UpperLimit { get; set; }


    }
}
