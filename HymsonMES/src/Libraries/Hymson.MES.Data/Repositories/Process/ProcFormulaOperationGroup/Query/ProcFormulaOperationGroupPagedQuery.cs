using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Process.Query
{
    /// <summary>
    /// 配方操作组 分页参数
    /// </summary>
    public class ProcFormulaOperationGroupPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 配方操作组编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 配方操作组名称
        /// </summary>
        public string? Name { get; set; }
    }
}
