using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.EquEquipmentLoginRecord.Query
{
    /// <summary>
    /// 操作员登录记录 分页参数
    /// </summary>
    public class EquEquipmentLoginRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
