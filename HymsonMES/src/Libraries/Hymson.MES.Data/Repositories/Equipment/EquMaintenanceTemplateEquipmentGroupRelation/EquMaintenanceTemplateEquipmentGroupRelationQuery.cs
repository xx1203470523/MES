/*
 *creator: Karl
 *
 *describe: 设备点检模板与设备组关系 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:22:22
 */

namespace Hymson.MES.Data.Repositories.EquMaintenanceTemplateEquipmentGroupRelation
{
    /// <summary>
    /// 设备点检模板与设备组关系 查询参数
    /// </summary>
    public class EquMaintenanceTemplateEquipmentGroupRelationQuery
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 模板IDs
        /// </summary>
        public IEnumerable<long>? MaintenanceTemplateIds { get; set; }
    }


    /// <summary>
    /// 设备点检模板与设备组关系 查询参数
    /// </summary>
    public class GetByTemplateIdAndGroupIdQuery
    {
        /// <summary>
        /// 模板IDs
        /// </summary>
        public long? MaintenanceTemplateId { get; set; }

        /// <summary>
        /// 模板IDs
        /// </summary>
        public IEnumerable<long>? EquipmentGroupIds { get; set; }
    }
}
