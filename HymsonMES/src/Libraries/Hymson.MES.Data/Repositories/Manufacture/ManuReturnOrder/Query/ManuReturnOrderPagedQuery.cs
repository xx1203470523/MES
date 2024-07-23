using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 生产退料单 分页参数
    /// </summary>
    public class ManuReturnOrderPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 退料单号
        /// </summary>
        public string? ReturnOrderCode { get; set; }

        /// <summary>
        /// ID集合（待退料的工单）
        /// </summary>
        public IEnumerable<long>? SourceWorkOrderIds { get; set; }

        /// <summary>
        /// 退料单类型0:工单退料 1:工单借料
        /// </summary>
        public ManuReturnTypeEnum? Type { get; set; }

    }
}
