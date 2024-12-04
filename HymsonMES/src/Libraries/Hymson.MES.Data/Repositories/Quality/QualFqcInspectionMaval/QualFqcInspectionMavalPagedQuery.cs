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

        /// <summary>
        /// 不合格原因
        /// </summary>
        public string? Remark { get; set; }
        
        /// <summary>
        /// 工序Code
        /// </summary>
        public long? ProcedureId {  get; set; }
        
        /// <summary>
        /// 资源Code
        /// </summary>
        public long? ResourceId {  get; set; }

    }
}
