using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// OQC检验水平新增/更新Dto
    /// </summary>
    public record QualOqcLevelSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// proc_material id  物料Id
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// inte_custom id 客户id
        /// </summary>
        public long? CustomId { get; set; }

        /// <summary>
        /// 设置类型 1、通用 2、物料
        /// </summary>
        public QCMaterialTypeEnum Type { get; set; }

        /// <summary>
        /// 检验水平 I, II, III, IV, V, VI, VII
        /// </summary>
        public InspectionLevelEnum Level { get; set; }

        /// <summary>
        /// 状态 0、已禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 整体接收标准
        /// </summary>
        public int AcceptanceLevel { get; set; }

        /// <summary>
        /// 项目集合
        /// </summary>
        public IEnumerable<QualOqcLevelDetailDto> Details { get; set; }

    }

    /// <summary>
    /// OQC检验水平明细新增/更新Dto
    /// </summary>
    public record QualOqcLevelDetailDto : BaseEntityDto
    {
        /// <summary>
        /// 检验类型
        /// </summary>
        public InspectionTypeEnum? Type { get; set; }

        /// <summary>
        /// 检验水准
        /// </summary>
        public VerificationLevelEnum? VerificationLevel { get; set; }

        /// <summary>
        /// 接收水准
        /// </summary>
        public int? AcceptanceLevel { get; set; }

    }

    /// <summary>
    /// OQC检验水平Dto
    /// </summary>
    public record QualOqcLevelDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// proc_material id  物料Id
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 编码（物料）
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 名称（物料）
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 版本（物料）
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// inte_custom id 客户id
        /// </summary>
        public long? CustomId { get; set; }

        /// <summary>
        /// 编码（客户）
        /// </summary>
        public string CustomCode { get; set; }

        /// <summary>
        /// 名称（客户）
        /// </summary>
        public string CustomName { get; set; }

        /// <summary>
        /// 设置类型 1、通用 2、物料
        /// </summary>
        public QCMaterialTypeEnum Type { get; set; }

        /// <summary>
        /// 检验水平 I, II, III, IV, V, VI, VII
        /// </summary>
        public InspectionLevelEnum Level { get; set; }

        /// <summary>
        /// 状态 0、已禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 整体接收水准
        /// </summary>
        public int AcceptanceLevel { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 项目集合
        /// </summary>
        public IEnumerable<QualOqcLevelDetailDto> Details { get; set; }

    }

    /// <summary>
    /// IQC检验水平分页Dto
    /// </summary>
    public class QualOqcLevelPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 编码（物料）
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 名称（物料）
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 编码（客户）
        /// </summary>
        public string? CustomCode { get; set; }

        /// <summary>
        /// 设置类型 1、通用 2、物料
        /// </summary>
        public QCMaterialTypeEnum? Type { get; set; }

        /// <summary>
        /// 状态 0、已禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

    }

}
