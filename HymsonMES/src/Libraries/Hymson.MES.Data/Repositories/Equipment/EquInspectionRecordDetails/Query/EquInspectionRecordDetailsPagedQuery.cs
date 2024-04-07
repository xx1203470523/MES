using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 点检记录详情表 分页参数
    /// </summary>
    public class EquInspectionRecordDetailsPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
