/*
 *creator: Karl
 *
 *describe: 马威FQC检验    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2024-07-24 03:09:40
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.QualFqcInspectionMaval
{
    /// <summary>
    /// 马威FQC检验，数据实体对象   
    /// qual_fqc_inspection_maval
    /// @author pengxin
    /// @date 2024-07-24 03:09:40
    /// </summary>
    public class QualFqcInspectionMavalEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

         /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 判定结果1.合格2.不合格
        /// </summary>
        public FqcJudgmentResultsEnum JudgmentResults { get; set; }

        /// <summary>
        /// 不合格原因
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 检验类型
        /// </summary>
        public string? TestType { get; set; }
    }
}
