/*
 *creator: Karl
 *
 *describe: 马威FQC检验 分页查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-07-24 03:09:40
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.QualFqcInspectionMaval
{
    /// <summary>
    /// 马威FQC检验 分页参数
    /// </summary>
    public class QualFqcInspectionMavalPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }

    }
}
