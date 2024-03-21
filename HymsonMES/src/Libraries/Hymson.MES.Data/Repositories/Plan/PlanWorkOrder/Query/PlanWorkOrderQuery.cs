using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 工单信息表 查询参数
    /// </summary>
    public class PlanWorkOrderQuery
    {
        // <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public List<long>? ProductIds { get; set; }

        /// <summary>
        /// 工单状态;1：未开始；2：下达；3：生产中；4：完成；5：锁定；6：暂停中；
        /// </summary>
        public List<PlanWorkOrderStatusEnum>? StatusList { get; set; }
    }

    /// <summary>
    /// 工单信息表 查询参数
    /// </summary>
    public class PlanWorkOrderNewQuery
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; }

        // <summary>
        /// 工单号
        /// </summary>
        public string? OrderCode { get; set; }


    }
}
