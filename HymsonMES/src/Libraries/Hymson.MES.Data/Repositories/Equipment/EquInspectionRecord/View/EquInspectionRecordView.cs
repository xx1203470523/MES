using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Equipment
{
    public class EquInspectionRecordView : BaseEntity
    {
        public long Id { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartExecuTime { get; set; }

        /// <summary>
        /// 点检单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 点检任务Id
        /// </summary>
        public long InspectionTaskSnapshootId { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long WorkCenterId { get; set; }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 点检类型 1、日点检 2、周点检
        /// </summary>
        public EquInspectionTypeEnum InspectionType { get; set; }

        /// <summary>
        /// 类别 1、白班2、晚班3、巡检
        /// </summary>
        public EquInspectionTaskTypeEnum? Type { get; set; }

        /// <summary>
        /// 完成时长（分钟）
        /// </summary>
        public int? CompleteTime { get; set; }
    }
}
