using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Process.Query
{
    /// <summary>
    /// 配方操作设置 分页参数
    /// </summary>
    public class ProcFormulaOperationSetPagedQuery : PagerInfo
    {
        public long ? FormulaOperationId { get; set; }
    }

}
