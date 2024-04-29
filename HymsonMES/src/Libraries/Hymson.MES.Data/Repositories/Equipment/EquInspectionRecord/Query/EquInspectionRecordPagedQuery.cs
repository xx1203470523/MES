using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 点检记录表 分页参数
    /// </summary>
    public class EquInspectionRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? EquipmentName { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 点检类型 1、日点检 2、周点检
        /// </summary>
        public EquInspectionTypeEnum? InspectionType { get; set; }

        /// <summary>
        /// 类别 1、白班2、晚班3、巡检
        /// </summary>
        public EquInspectionTaskTypeEnum? Type { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartExecuTime { get; set; }

        /// <summary>
        /// 1、待检验2、检验中3、已完成
        /// </summary>
        public EquInspectionRecordStatusEnum? Status { get; set; }
    }
}
