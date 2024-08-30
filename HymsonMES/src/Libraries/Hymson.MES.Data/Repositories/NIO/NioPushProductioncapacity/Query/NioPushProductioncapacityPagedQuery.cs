using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.NIO.Query
{
    /// <summary>
    /// 合作伙伴精益与生产能力 分页参数
    /// </summary>
    public class NioPushProductioncapacityPagedQuery : PagerInfo
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
        /// 日期（格式为yyyy-MM-dd HH:mm:ss）
        /// </summary>
        public string? Date { get; set; }
    }
}
