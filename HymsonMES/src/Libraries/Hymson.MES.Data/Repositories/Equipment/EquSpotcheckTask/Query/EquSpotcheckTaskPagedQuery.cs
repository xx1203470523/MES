using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 点检任务 分页参数
    /// </summary>
    public class EquSpotcheckTaskPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 点检任务编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 点检任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string LeaderIds { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 状态;1:待处理、2:处理中、3:待审核、4:已关闭
        /// </summary>
        public EquSpotcheckTaskStautusEnum? Status { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 处理结果 不合格处理方式;1-通过；2-不通过
        /// </summary>
        public EquSpotcheckTaskProcessedEnum? HandMethod { get; set; }

        /// <summary>
        /// 计划开始时间  时间范围  数组
        /// </summary>
        public DateTime[]? PlanStartTime { get; set; }

        /// <summary>
        /// 任务Ids
        /// </summary>
        public IEnumerable<long>? TaskIds { get; set; }

    }
}
