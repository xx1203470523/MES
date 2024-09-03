using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Plan;

namespace Hymson.MES.Data.Repositories.NIO.Query
{
    /// <summary>
    /// 物料发货信息表 分页参数
    /// </summary>
    public class NioPushActualDeliveryPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 推送状态;0：无需推送；1：待推送；2：已推送；3：推送失败；
        /// </summary>
        public PushStatusEnum? Status { get; set; }
    }
}
