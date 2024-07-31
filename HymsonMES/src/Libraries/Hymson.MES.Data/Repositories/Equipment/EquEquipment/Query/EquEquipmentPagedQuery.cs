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
        public long[]? UseDepartments { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string? WorkCenterShopCode { get; set; }

        /// <summary>
        /// 存放位置
        /// </summary>
        public string? Location { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EquEquipmentSpotcheckRelationPagedQuery : PagerInfo
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
        /// 名称（设备）
        /// </summary>
        public string? EquipmentName { get; set; }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 存放位置
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// 设备组
        /// </summary>
        public string? EquipmentGroupCode { get; set; }

        /// <summary>
        /// 设备组
        /// </summary>
        public int? EopType { get; set; } = 0;

    }
}
