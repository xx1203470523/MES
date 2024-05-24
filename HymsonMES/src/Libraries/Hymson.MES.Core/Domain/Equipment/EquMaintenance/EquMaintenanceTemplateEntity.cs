using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment.EquMaintenance
{
    /// <summary>
    /// 设备点检模板，数据实体对象   
    /// equ_maintenance_template
    /// @author pengxin
    /// @date 2024-05-23 03:06:41
    /// </summary>
    public class EquMaintenanceTemplateEntity : BaseEntity 
    {
        /// <summary>
        /// 点检模板编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 点检模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


    }
}
