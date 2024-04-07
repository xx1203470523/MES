using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（点检任务）   
    /// equ_inspection_task
    /// @author User
    /// @date 2024-04-03 04:51:32
    /// </summary>
    public class EquInspectionTaskEntity : BaseEntity
    {
        /// <summary>
        /// 点检类型 1、日点检 2、周点检
        /// </summary>
        public bool InspectionType { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public long WorkCenterId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 执行月1-12
        /// </summary>
        public bool? Month { get; set; }

        /// <summary>
        /// 执行日 1、周一，2、周二，3、周三，4、周四，5、周五，6、周六，7、周日
        /// </summary>
        public bool? Day { get; set; }

        /// <summary>
        /// 执行开始时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 完成时长（分钟）
        /// </summary>
        public bool? CompleteTime { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 状态 1、新建2、启用3、保留4、废除
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// 类别 1、白班2、晚班3、巡检
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
