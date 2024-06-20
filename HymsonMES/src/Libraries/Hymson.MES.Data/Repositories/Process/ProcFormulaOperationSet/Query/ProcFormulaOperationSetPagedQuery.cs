using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Process.Query
{
    /// <summary>
    /// 配方操作设置 分页参数
    /// </summary>
    public class ProcFormulaOperationSetPagedQuery : PagerInfo
    {
        public long ? FormulaOperationId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
    }

}
