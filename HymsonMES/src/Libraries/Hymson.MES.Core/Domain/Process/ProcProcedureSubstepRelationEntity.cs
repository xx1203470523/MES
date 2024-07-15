using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据实体（工序配置子步骤表）   
    /// proc_procedure_substep_relation
    /// @author zhaoqing
    /// @date 2024-07-05 11:54:41
    /// </summary>
    public class ProcProcedureSubstepRelationEntity : BaseEntity
    {
        /// <summary>
        /// 所属工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 子步骤ID
        /// </summary>
        public long ProcedureSubstepId { get; set; }

        
    }
}
