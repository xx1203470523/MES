using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command
{
    /// <summary>
    /// 这么这个类起这么名字，跟基础状态操作类 UpdateStatusCommand 重名了，先加个New吧
    /// </summary>
    public class UpdateStatusNewCommand
    {
        public long Id { get; set; }

        /// <summary>
        /// 工单状态;1：未开始；2：下达；3：生产中；4：完成；5：锁定；6：暂停中；
        /// </summary>
        public PlanWorkOrderStatusEnum Status { get; set; }


        /// <summary>
        /// 工单状态;1：未开始；2：下达；3：生产中；4：完成；5：锁定；6：暂停中；
        /// </summary>
        public PlanWorkOrderStatusEnum BeforeStatus { get; set; }

        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }
}
