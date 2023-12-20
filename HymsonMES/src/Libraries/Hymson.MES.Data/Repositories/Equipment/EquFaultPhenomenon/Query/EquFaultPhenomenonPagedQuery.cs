using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 分页参数（设备故障现象）
    /// </summary>
    public class EquFaultPhenomenonPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 编号（设备故障现象）
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 名称（设备故障现象）
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 设备组名称
        /// </summary>
        public string EquipmentGroupName { get; set; } = "";

        /// <summary>
        /// 使用状态 0-禁用 1-启用（设备故障现象）
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";
    }
}
