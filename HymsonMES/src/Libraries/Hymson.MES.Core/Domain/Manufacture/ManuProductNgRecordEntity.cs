using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（产品NG记录表）   
    /// manu_product_ng_record
    /// @author Czhipu
    /// @date 2023-10-27 05:32:31
    /// </summary>
    public class ManuProductNgRecordEntity : BaseEntity
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 不良记录Id
        /// </summary>
        public long BadRecordId { get; set; }

       /// <summary>
        /// 标记缺陷Id
        /// </summary>
        public long UnqualifiedId { get; set; }

       /// <summary>
        /// 不合格代码
        /// </summary>
        public string NGCode { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       
    }
}
