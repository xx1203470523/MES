using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 产品检验参数表新增/更新Dto
    /// </summary>
    public record ProcProductParameterGroupSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 参数集编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 参数集名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 是否默认版本
        /// </summary>
        public bool? IsDefaultVersion { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 项目集合
        /// </summary>
        public IEnumerable<ProcProductParameterGroupDetailSaveDto> Details { get; set; }

    }

    /// <summary>
    /// 产品检验参数表Dto
    /// </summary>
    public record ProcProductParameterGroupDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 参数集编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 参数集名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 是否默认版本
        /// </summary>
        public bool? IsDefaultVersion { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

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

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

    }

    /// <summary>
    /// 产品检验参数表Dto
    /// </summary>
    public record ProcProductParameterGroupInfoDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 参数集编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 参数集名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 是否默认版本
        /// </summary>
        public bool? IsDefaultVersion { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

    }

    /// <summary>
    /// 产品检验参数表分页Dto
    /// </summary>
    public class ProcProductParameterGroupPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 参数集编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 参数集名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
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
    }

    public class ProcProductParameterGroupBySfcAndProcedureIdQueryDto 
    { 
        /// <summary>
        /// 产品序列码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }
    }

    public class ProcProductParameterGroupToParameterCollectionQueryDto
    {
        /// <summary>
        /// 条码类型
        /// </summary>
        public ManuFacePlateBarcodeTypeEnum BarcodeType { get; set; }

        /// <summary>
        /// 产品序列码
        /// </summary>
        public string[] Sfcs { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }
    }
}
