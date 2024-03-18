using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// OQC检验类型新增/更新Dto
    /// </summary>
    public record QualOqcOrderTypeSaveDto : BaseEntityDto
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
        /// OQC检验单Id
        /// </summary>
        public long OQCOrderId { get; set; }

        /// <summary>
        /// 检验类型
        /// </summary>
        public bool InspectionType { get; set; }

        /// <summary>
        /// 验证水准
        /// </summary>
        public bool VerificationLevel { get; set; }

        /// <summary>
        /// 接收标准
        /// </summary>
        public int AcceptanceLevel { get; set; }

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty { get; set; }

        /// <summary>
        /// 已检数量
        /// </summary>
        public int CheckedQty { get; set; }

        /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public bool? IsQualified { get; set; }

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
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }


    }

    /// <summary>
    /// OQC检验类型Dto
    /// </summary>
    public record QualOqcOrderTypeDto : BaseEntityDto
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
        /// OQC检验单Id
        /// </summary>
        public long OQCOrderId { get; set; }

        /// <summary>
        /// 检验类型
        /// </summary>
        public bool InspectionType { get; set; }

        /// <summary>
        /// 验证水准
        /// </summary>
        public bool VerificationLevel { get; set; }

        /// <summary>
        /// 接收标准
        /// </summary>
        public int AcceptanceLevel { get; set; }

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty { get; set; }

        /// <summary>
        /// 已检数量
        /// </summary>
        public int CheckedQty { get; set; }

        /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public bool? IsQualified { get; set; }

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
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }


    }

    /// <summary>
    /// OQC检验类型分页Dto
    /// </summary>
    public class QualOqcOrderTypePagedQueryDto : PagerInfo { }

    /// <summary>
    /// 获取应检数量和已检数量
    /// </summary>
    public record SampleQtyAndCheckedQtyQueryDto {
        /// <summary>
        /// OQC检验单Id
        /// </summary>
        public long OQCOrderId { get; set; }

        /// <summary>
        /// 检验类型
        /// </summary>
        public OQCInspectionTypeEnum InspectionType { get; set; }
    }

    /// <summary>
    /// 获取应检数量和已检数量OutDto
    /// </summary>
    public record SampleQtyAndCheckedQtyQueryOutDto: BaseEntityDto
    {
        /// <summary>
        /// 已检数量
        /// </summary>
        public int CheckedQty { get; set; }

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty { get; set; }
    }
}
