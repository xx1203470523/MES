/*
 *creator: Karl
 *
 *describe: 设备保养模板 分页查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:06:41
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.EquMaintenanceTemplate
{
    /// <summary>
    /// 设备保养模板 分页参数
    /// </summary>
    public class EquMaintenanceTemplatePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 保养模板编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 保养模板名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 设备组编码
        /// </summary>
        public string? EquipmentGroupCode { get; set; }

        /// <summary>
        /// 设备组名称
        /// </summary>
        public string? EquipmentGroupName { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 模板IDs
        /// </summary>
        public IEnumerable<long>? MaintenanceTemplateIds { get; set; }
    }
}
