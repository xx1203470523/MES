using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 点检任务 分页参数
    /// </summary>
    public class EquInspectionTaskPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 点检任务编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 点检类型 1、日点检 2、周点检
        /// </summary>
        public EquInspectionTypeEnum? InspectionType { get; set; }

        /// <summary>
        /// 点检类别 1、白班2、晚班3、巡检
        /// </summary>
        public EquInspectionTaskTypeEnum? Type { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }
    }
}
