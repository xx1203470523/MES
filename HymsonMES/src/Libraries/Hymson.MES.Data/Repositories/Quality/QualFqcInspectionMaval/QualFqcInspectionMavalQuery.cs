/*
 *creator: Karl
 *
 *describe: 马威FQC检验 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-07-24 03:09:40
 */

namespace Hymson.MES.Data.Repositories.QualFqcInspectionMaval
{
    /// <summary>
    /// 马威FQC检验 查询参数
    /// </summary>
    public class QualFqcInspectionMavalQuery
    {
        /// <summary>
        /// 工厂Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码组
        /// </summary>
        public IEnumerable<string> SFCs { get; set; }
    }
}
