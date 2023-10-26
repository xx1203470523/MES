using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据实体（工序复投设置）   
    /// proc_procedure_rejudge
    /// @author Czhipu
    /// @date 2023-10-26 02:57:44
    /// </summary>
    public class ProcProcedureRejudgeEntity : BaseEntity
    {
        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long UnqualifiedCodeId { get; set; }

        /// <summary>
        /// 不合格代码类型;1:标记编码;2:最终缺陷编码;3:阻断缺陷编码;
        /// </summary>
        public RejudgeUnqualifiedCodeEnum Type { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
