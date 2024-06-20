using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据实体（工序资质认证（物理删除））   
    /// proc_procedure_qualification_authentication_relation
    /// @author zhaoqing
    /// @date 2024-06-18 06:00:17
    /// </summary>
    public class ProcProcedureQualificationAuthenticationRelationEntity : BaseEntity
    {
        /// <summary>
        /// 序Id proc_procedure_ 的Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资质Id inte_qualification_authentication的Id
        /// </summary>
        public long QualificationAuthenticationId { get; set; }

        /// <summary>
        /// 是否启用  0 未启用 1启用
        /// </summary>
        public decimal IsEnable { get; set; }

        
    }
}
