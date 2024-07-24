using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据实体（子步骤）   
    /// proc_procedure_substep
    /// @author zhaoqing
    /// @date 2024-07-02 04:28:03
    /// </summary>
    public class ProcProcedureSubstepEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型 1、正常，2、可选
        /// </summary>
        public ProcedureSubstepTypeEnum? Type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        
    }
}
