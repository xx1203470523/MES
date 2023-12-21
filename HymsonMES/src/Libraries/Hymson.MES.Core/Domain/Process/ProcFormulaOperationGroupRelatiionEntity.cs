using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据实体（配方操作组维护）   
    /// proc_formula_operation_group_relatiion
    /// @author hjy
    /// @date 2023-12-15 02:10:15
    /// </summary>
    public class ProcFormulaOperationGroupRelatiionEntity : BaseEntity
    {
        /// <summary>
        /// 配方操作Id
        /// </summary>
        public long FormulaOperationId { get; set; }

       /// <summary>
        /// 配方操作组Id
        /// </summary>
        public long FormulaOperationGroupId { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
