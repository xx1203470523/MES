using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码表 分页参数
    /// </summary>
    public class ManuSfcPassDownPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string? WorkOrderCode { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public PlanSFCUsedStatusEnum? IsUsed { get; set; }
    }
}
