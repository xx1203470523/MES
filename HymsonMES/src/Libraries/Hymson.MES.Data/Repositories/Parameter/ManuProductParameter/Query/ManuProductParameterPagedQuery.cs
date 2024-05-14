using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Parameter
{
    public class ManuProductParameterPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public long? ParameterId { get; set; }

        /// <summary>
        /// 上报时间  时间范围  数组
        /// </summary>
        public DateTime[]? CollectionTimeRange { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string? Sfc { get; set; }

        /// <summary>
        /// 产品条码列表
        /// </summary>
        public IEnumerable<string>? Sfcs { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        //public long? WorkCenterId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        //public string? OrderCode { get; set; }

        ///// <summary>
        ///// 产品
        ///// </summary>
        //public long? ProductId { get; set; }
    }
}
