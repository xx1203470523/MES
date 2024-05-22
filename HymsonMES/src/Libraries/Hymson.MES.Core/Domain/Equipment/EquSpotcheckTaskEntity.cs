using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（点检任务）   
    /// equ_spotcheck_task
    /// @author User
    /// @date 2024-05-14 09:00:47
    /// </summary>
    public class EquSpotcheckTaskEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 点检任务编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 点检任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 开始时间（实际）
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 结束时间（实际）
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 状态;1:待处理、2:处理中、3:待审核、4:已关闭
        /// </summary>
        public EquSpotcheckTaskStautusEnum Status { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }


    }


    public class EquSpotcheckTaskUnionPlanEntity : BaseEntity
    {
        /// <summary>
        /// 点检任务编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 点检任务名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 开始时间（实际）
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 结束时间（实际）
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 状态;1:待处理、2:处理中、3:待审核、4:已关闭
        /// </summary>
        public EquSpotcheckTaskStautusEnum? Status { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }


        // <summary>
        /// 点检计划编码
        /// </summary>
        public string? PlanCode { get; set; }

        /// <summary>
        /// 点检计划名称
        /// </summary>
        public string? PlanName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 设备ID;equ_equipment表的Id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 点检执行人;用户中心UserId集合
        /// </summary>
        public string? ExecutorIds { get; set; }

        /// <summary>
        /// 点检负责人;用户中心UserId集合
        /// </summary>
        public string? LeaderIds { get; set; }

        /// <summary>
        /// 点检类型;周/日
        /// </summary>
        public bool? PlanType { get; set; }

        /// <summary>
        /// 开始时间（计划）
        /// </summary>
        public DateTime? PlanBeginTime { get; set; }

        /// <summary>
        /// 结束时间（计划）
        /// </summary>
        public DateTime? PlanEndTime { get; set; }
    }
}
