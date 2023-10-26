using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 工序复投
    /// </summary>
    public class ProcedureRejudgeBo
    {
        /// <summary>
        /// 工厂Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// ID（工序）
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 循环次数 
        /// </summary>
        public int Cycle { get; set; }

        /// <summary>
        /// 是否复判
        /// </summary>
        public TrueOrFalseEnum? IsRejudge { get; set; }

        /// <summary>
        /// 是否校验NG信息
        /// </summary>
        public TrueOrFalseEnum? IsValidNGCode { get; set; }

        /// <summary>
        /// 标记缺陷编码
        /// </summary>
        public long? MarkUnqualifiedId { get; set; }

        /// <summary>
        /// 最终缺陷编码
        /// </summary>
        public QualUnqualifiedCodeEntity? LastUnqualified { get; set; }

        /// <summary>
        /// 阻断缺陷编码
        /// </summary>
        public IEnumerable<long>? BlockUnqualifiedIds { get; set; }

    }

}
