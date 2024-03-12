using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（IQC检验水平详情）   
    /// qual_iqc_level_detail
    /// @author Czhipu
    /// @date 2024-02-02 02:04:17
    /// </summary>
    public class QualIqcLevelDetailEntity : BaseEntity
    {
        /// <summary>
        /// qual_iqc_level id
        /// </summary>
        public long IqcLevelId { get; set; }

        /// <summary>
        /// 检验类型
        /// </summary>
        public IQCInspectionTypeEnum Type { get; set; }

        /// <summary>
        /// 检验水准
        /// </summary>
        public VerificationLevelEnum VerificationLevel { get; set; }

        /// <summary>
        /// 接收水准
        /// </summary>
        public int AcceptanceLevel { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
