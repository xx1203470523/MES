using Hymson.MES.Core.Enums.Common;

namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 工装条码绑定 查询参数
    /// </summary>
    public class ManuToolingBindQuery
    {
        /// <summary>
        /// 排序(默认为 CreatedOn DESC)
        /// </summary>
        public string Sorting { get; set; } = "CreatedOn DESC";

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工装编码
        /// </summary>
        public string ToolingCode { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public IEnumerable<string> Barcodes { get; set; }

        /// <summary>
        /// 0-解绑 1-绑定
        /// </summary>
        public BindStatusEnum? Status { get; set; }
    }
}
