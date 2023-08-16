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

        /// <summary>
        /// 工序编码（工序）
        /// </summary>
        public string ProcedureCode { get; set; } = "";
    }
}
