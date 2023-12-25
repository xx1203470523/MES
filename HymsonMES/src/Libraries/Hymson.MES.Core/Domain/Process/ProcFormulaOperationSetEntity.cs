using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据实体（配方操作设置）   
    /// proc_formula_operation_set
    /// @author hjy
    /// @date 2023-12-15 02:11:07
    /// </summary>
    public class ProcFormulaOperationSetEntity : BaseEntity
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Serial { get; set; }

       /// <summary>
        /// 配方操作 proc_formula_operation Id
        /// </summary>
        public long FormulaOperationId { get; set; }

       /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 设定值
        /// </summary>
        public int Value { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
