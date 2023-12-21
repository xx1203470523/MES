using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据实体（配方操作组）   
    /// proc_formula_operation_group
    /// @author hjy
    /// @date 2023-12-15 02:09:27
    /// </summary>
    public class ProcFormulaOperationGroupEntity : BaseEntity
    {
        /// <summary>
        /// 配方操作编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 配方操作名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
