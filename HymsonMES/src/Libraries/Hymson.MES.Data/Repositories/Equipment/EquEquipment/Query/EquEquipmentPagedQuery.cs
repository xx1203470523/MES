using Hymson.Infrastructure;
using Hymson.Infrastructure.Constants;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class EquEquipmentPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 编码（设备）
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string[]? EquipmentCodes { get; set; }

        /// <summary>
        /// 名称（设备）
        /// </summary>
        public string? EquipmentName { get; set; }

        /// <summary>
        /// 类型（设备）
        /// </summary>
        public int? EquipmentType { get; set; }

        /// <summary>
        /// 使用状态（设备）
        /// </summary>
        public int? UseStatus { get; set; }

        /// <summary>
        /// 使用部门
        /// </summary>
        public int? UseDepartment { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string? WorkCenterShopName { get; set; }

        /// <summary>
        /// 存放位置
        /// </summary>
        public string? Location { get; set; }
    }
}
