using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Process.Query
{
    /// <summary>
    /// 工艺设备组 分页参数
    /// </summary>
    public class ProcProcessEquipmentGroupPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 编码（设备组）
        /// </summary>
        public string Code { get; set; } = "";

    }

    /// <summary>
    ///更具编码查询参数
    /// </summary>
    public class ProcProcessEquipmentGroupByCodeQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 编码集合
        /// </summary>
        public IEnumerable<string> Codes { get; set; }
    }
}
