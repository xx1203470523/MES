using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Plan;

namespace Hymson.MES.Data.Repositories.NIO.Query
{
    /// <summary>
    /// 物料及其关键下级件信息表 分页参数
    /// </summary>
    public class NioPushKeySubordinatePagedQuery : PagerInfo
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

        /// <summary>
        /// 时间
        /// </summary>
        public string? Date { get; set; }

        /// <summary>
        /// 关键下级件物料编码
        /// </summary>
        public string? SubordinateCode { get; set; }

        /// <summary>
        /// 关键下级件物料名称
        /// </summary>
        public string? SubordinateName { get; set; }
    }
}
