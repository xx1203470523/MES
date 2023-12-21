using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 配方维护新增/更新Dto
    /// </summary>
    public record ProcFormulaSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 配方编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 配方名称
        /// </summary>
        public string Name { get; set; }

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


        public IEnumerable<ProcFormulaDetailsDto>? ProcFormulaDetailsDtos { get; set; }
    }

    /// <summary>
    /// 配方维护Dto
    /// </summary>
    public record ProcFormulaDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

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
        public string Remark { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       
    }

    public record ProcFormulaViewDto : ProcFormulaDto 
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }
    }

    public record ProcFormulaDetailViewDto : ProcFormulaDto 
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 工艺设备组编码
        /// </summary>
        public string EquipmentGroupCode { get; set; }

        /// <summary>
        /// 工艺设备组名称
        /// </summary>
        public string EquipmentGroupName { get; set; }

        /// <summary>
        /// 操作组编码
        /// </summary>
        public string FormulaOperationGroupCode {  get; set; }

        /// <summary>
        /// 操作组名称
        /// </summary>
        public string FormulaOperationGroupName { get;set; }
    }
    /// <summary>
    /// 配方维护分页Dto
    /// </summary>
    public class ProcFormulaPagedQueryDto : PagerInfo 
    {
        /// <summary>
        /// 配方编码
        /// </summary>
        public string? Code {  get; set; }
        
        /// <summary>
        /// 配方名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string? ProcedureName { get; set; }
    }

    /// <summary>
    /// 配方维护详情Dto
    /// </summary>
    public record ProcFormulaDetailsDto : BaseEntityDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Serial { get; set; }

        /// <summary>
        /// proc_formula_operation id 配方操作id
        /// </summary>
        public long FormulaOperationId { get; set; }

        /// <summary>
        /// proc_material 的id
        /// </summary>
        public long? MateriaId { get; set; }

        /// <summary>
        /// proc_material_group 的id
        /// </summary>
        public long? MateriaGroupId { get; set; }

        /// <summary>
        /// proc_process_equipment_group的id
        /// </summary>
        public long? EquipmentGroupId { get; set; }

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
    }

    public record ProcFormulaDetailsViewDto : ProcFormulaDetailsDto { }
}
