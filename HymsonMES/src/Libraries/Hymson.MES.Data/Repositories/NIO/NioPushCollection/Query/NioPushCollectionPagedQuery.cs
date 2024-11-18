using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Plan;

namespace Hymson.MES.Data.Repositories.NioPushCollection.Query
{
    /// <summary>
    /// NIO推送参数 分页参数
    /// </summary>
    public class NioPushCollectionPagedQuery : PagerInfo
    {
        /// <summary>
        /// 合作伙伴总成序列号
        /// </summary>
        public string? VendorProductSn { get; set; }

        /// <summary>
        /// 合作伙伴总成临时序列号
        /// </summary>
        public string? VendorProductTempSn { get; set; }

        /// <summary>
        /// 工位唯一标识
        /// </summary>
        public string? StationId { get; set; }

        /// <summary>
        /// 对应控制项主数据中的字段
        /// </summary>
        public string? VendorFieldCode { get; set; }

        /// <summary>
        /// 推送状态;0：无需推送；1：待推送；2：已推送；3：推送失败；
        /// </summary>
        public PushStatusEnum? Status { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public TrueOrFalseEnum? IsOk { get; set; }

        /// <summary>
        /// 合作伙伴产品名称
        /// </summary>
        public String? VendorProductName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String? ParameterName { get; set; }

    /// <summary>
    /// 操作时间
    /// </summary>
    public DateTime[]? CreatedOn { get; set; }
    }
}
