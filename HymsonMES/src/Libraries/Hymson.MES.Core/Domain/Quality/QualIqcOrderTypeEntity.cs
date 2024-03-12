using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（IQC检验类型）   
    /// qual_iqc_order_type
    /// @author Czhipu
    /// @date 2024-03-06 02:26:54
    /// </summary>
    public class QualIqcOrderTypeEntity : BaseEntity
    {
        /// <summary>
        /// IQC检验单Id
        /// </summary>
        public long IQCOrderId { get; set; }

        /// <summary>
        /// 检验类型;1、常规检验2、外观检验3、包装检验4、特殊性检验5、破坏性检验
        /// </summary>
        public IQCInspectionTypeEnum Type { get; set; }

        /// <summary>
        /// 验证水准 R,I, II, III, IV, V, VI, VII，T
        /// </summary>
        public VerificationLevelEnum VerificationLevel { get; set; }

        /// <summary>
        /// 接收水准
        /// </summary>
        public int AcceptanceLevel { get; set; }

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty { get; set; }

        /// <summary>
        /// 检验数量
        /// </summary>
        public int CheckedQty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }


    }
}
