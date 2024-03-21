using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.EquToolLifeRecord.Query
{
    /// <summary>
    /// 设备夹具寿命 分页参数
    /// </summary>
    public class EquToolLifeRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
