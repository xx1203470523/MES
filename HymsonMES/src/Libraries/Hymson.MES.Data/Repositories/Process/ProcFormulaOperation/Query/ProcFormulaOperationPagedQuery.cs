using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Process.Query
{
    /// <summary>
    /// 配方操作 分页参数
    /// </summary>
    public class ProcFormulaOperationPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 操作编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 操作状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public FormulaOperationTypeEnum? Type { get; set; }
    }

    /// <summary>
    /// 配方操作 分页参数
    /// </summary>
    public class GetByIdsPagedQuery : PagerInfo
    {
        /// <summary>
        /// 配方操作 Ids
        /// </summary>
        public long[] Ids { get; set; }

    }
}
