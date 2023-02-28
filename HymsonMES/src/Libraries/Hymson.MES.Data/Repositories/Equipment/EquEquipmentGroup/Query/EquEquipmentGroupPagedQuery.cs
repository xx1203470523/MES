using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup.Query
{
    /// <summary>
    /// 设备组 分页参数
    /// </summary>
    public class EquEquipmentGroupPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 编码（设备组）
        /// </summary>
        public string EquipmentGroupCode { get; set; } = "";

        /// <summary>
        /// 名称（设备组）
        /// </summary>
        public string EquipmentGroupName { get; set; } = "";
    }
}
