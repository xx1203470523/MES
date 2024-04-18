using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 工序复投
    /// </summary>
    public record ProcedureRejudgeBo
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
        /// 编码（工序）
        /// </summary>
        public string ProcedureCode { get; set; } = "";

        /// <summary>
        /// 是否有设置不合格工艺路线
        /// </summary>
        public bool IsHasUnQualifiedProcessRoute { get; set; } = false;

        /// <summary>
        /// ID（下工序）（当值是0时，大多情况是因为未找到不合格工艺路线）
        /// </summary>
        public long NextProcedureId { get; set; }

        /// <summary>
        /// 编码（下工序）
        /// </summary>
        public string NextProcedureCode { get; set; } = "";

        /// <summary>
        /// 循环次数 
        /// </summary>
        public int Cycle { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public ProcedureTypeEnum Type { get; set; }

        /// <summary>
        /// 是否复判
        /// </summary>
        public TrueOrFalseEnum IsRejudge { get; set; }

        /// <summary>
        /// 是否校验NG信息
        /// </summary>
        public TrueOrFalseEnum IsValidNGCode { get; set; }

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

        /// <summary>
        /// 集合（不合格代码）
        /// </summary>
        public IEnumerable<QualUnqualifiedCodeEntity> UnqualifiedCodeEntities { get; set; }

    }

}
