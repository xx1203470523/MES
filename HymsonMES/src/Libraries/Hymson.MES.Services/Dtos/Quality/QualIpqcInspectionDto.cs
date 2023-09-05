using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// IPQC检验项目新增/更新Dto
    /// </summary>
    public record QualIpqcInspectionSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 检验类型;1、首检 2、尾检 3、巡检
        /// </summary>
        public IPQCTypeEnum Type { get; set; }

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty { get; set; }

        /// <summary>
        /// 全检参数idqual_inspection_parameter_group 的id
        /// </summary>
        public long InspectionParameterGroupId { get; set; }

        /// <summary>
        /// 参数集编码
        /// </summary>
        public string ParameterGroupCode { get; set; }

        /// <summary>
        /// 生成条件
        /// </summary>
        public int GenerateCondition { get; set; }

        /// <summary>
        /// 生成条件单位;1、小时 2、班次 3、批次 4、罐 5、卷
        /// </summary>
        public GenerateConditionUnitEnum GenerateConditionUnit { get; set; }

        /// <summary>
        /// 管控时间
        /// </summary>
        public int? ControlTime { get; set; }

        /// <summary>
        /// 管控时间单位;1、时 2、分
        /// </summary>
        public ControlTimeUnitEnum? ControlTimeUnit { get; set; }

        /// <summary>
        /// 物料id proc_material 的 id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 工序id  proc_procedure的id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        ///// <summary>
        ///// 状态
        ///// </summary>
        //public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 检验项目明细
        /// </summary>
        public IEnumerable<QualIpqcInspectionParameterSaveDto>? Details { get; set; }

        /// <summary>
        /// 检验规则
        /// </summary>
        public IEnumerable<QualIpqcInspectionRuleSaveDto>? Rules { get; set; }
    }

    /// <summary>
    /// IPQC检验项目Dto
    /// </summary>
    public record QualIpqcInspectionDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 检验类型;1、首检 2、尾检 3、巡检
        /// </summary>
        public IPQCTypeEnum Type { get; set; }

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty { get; set; }

        /// <summary>
        /// 全检参数idqual_inspection_parameter_group 的id
        /// </summary>
        public long InspectionParameterGroupId { get; set; }

        /// <summary>
        /// 参数集编码
        /// </summary>
        public string ParameterGroupCode { get; set; }

        /// <summary>
        /// 参数集名称
        /// </summary>
        public string ParameterGroupName { get; set; }

        /// <summary>
        /// 参数集版本
        /// </summary>
        public string ParameterGroupVersion { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 产品名称
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

        /// <summary>
        /// 生成条件
        /// </summary>
        public int GenerateCondition { get; set; }

        /// <summary>
        /// 生成条件单位;1、小时 2、班次 3、批次 4、罐 5、卷
        /// </summary>
        public GenerateConditionUnitEnum GenerateConditionUnit { get; set; }

        /// <summary>
        /// 管控时间
        /// </summary>
        public int? ControlTime { get; set; }

        /// <summary>
        /// 管控时间单位;1、时 2、分
        /// </summary>
        public ControlTimeUnitEnum? ControlTimeUnit { get; set; }

        /// <summary>
        /// 物料id proc_material 的 id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 工序id  proc_procedure的id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 备注
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
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }


    }

    /// <summary>
    /// IPQC检验项目分页Dto
    /// </summary>
    public class QualIpqcInspectionPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 参数集编码
        /// </summary>
        public string? ParameterGroupCode { get; set; }

        /// <summary>
        /// 参数集名称
        /// </summary>
        public string? ParameterGroupName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string? ProcedureName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 检验类型;1、首检 2、尾检 3、巡检
        /// </summary>
        public IPQCTypeEnum? Type { get; set; }
    }

    /// <summary>
    /// IPQC检验项目Dto
    /// </summary>
    public record QualIpqcInspectionViewDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 检验类型;1、首检 2、尾检 3、巡检
        /// </summary>
        public IPQCTypeEnum Type { get; set; }

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty { get; set; }

        /// <summary>
        /// 全检参数idqual_inspection_parameter_group 的id
        /// </summary>
        public long InspectionParameterGroupId { get; set; }

        /// <summary>
        /// 参数集编码
        /// </summary>
        public string ParameterGroupCode { get; set; }

        /// <summary>
        /// 参数集名称
        /// </summary>
        public string ParameterGroupName { get; set; }

        /// <summary>
        /// 参数集版本
        /// </summary>
        public string ParameterGroupVersion { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 产品名称
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

        /// <summary>
        /// 生成条件
        /// </summary>
        public int GenerateCondition { get; set; }

        /// <summary>
        /// 生成条件单位;1、小时 2、班次 3、批次 4、罐 5、卷
        /// </summary>
        public GenerateConditionUnitEnum GenerateConditionUnit { get; set; }

        /// <summary>
        /// 管控时间
        /// </summary>
        public int? ControlTime { get; set; }

        /// <summary>
        /// 管控时间单位;1、时 2、分
        /// </summary>
        public ControlTimeUnitEnum? ControlTimeUnit { get; set; }

        /// <summary>
        /// 物料id proc_material 的 id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 工序id  proc_procedure的id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 备注
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
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }
    }
}
