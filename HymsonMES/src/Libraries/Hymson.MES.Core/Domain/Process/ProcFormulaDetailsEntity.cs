using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据实体（配方维护详情）   
    /// proc_formula_details
    /// @author Karl
    /// @date 2023-12-21 11:30:02
    /// </summary>
    public class ProcFormulaDetailsEntity : BaseEntity
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Serial { get; set; }

       /// <summary>
        /// proc_formula 的id
        /// </summary>
        public long FormulaId { get; set; }

       /// <summary>
        /// proc_formula_operation id 配方操作id
        /// </summary>
        public long FormulaOperationId { get; set; }

       /// <summary>
        /// proc_material 的id
        /// </summary>
        public long? MaterialId { get; set; }

       /// <summary>
        /// proc_material_group 的id
        /// </summary>
        public long? MaterialGroupId { get; set; }

        /// <summary>
        /// proc_parameter的id
        /// </summary>
        public long? ParameterId { get; set; }

       /// <summary>
        /// 功能代码
        /// </summary>
        public string? FunctionCode { get; set; }

       /// <summary>
        /// 设定值
        /// </summary>
        public string Setvalue { get; set; }

       /// <summary>
        /// 上限
        /// </summary>
        public decimal? UpperLimit { get; set; }

       /// <summary>
        /// 下限
        /// </summary>
        public decimal? LowLimit { get; set; }

       /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 顺序（正整数，最大值10000）
        /// </summary>
        public int Sort { get; set; }
    }
}
