using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据实体（配方维护）   
    /// proc_formula
    /// @author Karl
    /// @date 2023-12-20 09:58:49
    /// </summary>
    public class ProcFormulaEntity : BaseEntity
    {
        /// <summary>
        /// 配方编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 配方名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 类型 0、新建 1、启用 2、保留 3、废除
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// proc_material 的id
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// proc_procedure 的id
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// proc_process_equipment_group的id
        /// </summary>
        public long EquipmentGroupId { get; set; }

       /// <summary>
        /// proc_formula_operation_group 的id
        /// </summary>
        public long FormulaOperationGroupId { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
