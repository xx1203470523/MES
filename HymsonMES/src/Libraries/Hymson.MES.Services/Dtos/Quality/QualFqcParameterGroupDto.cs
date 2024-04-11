using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Services.Dtos.Qual;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// FQC检验参数组新增/更新Dto
    /// </summary>
    public record QualFqcParameterGroupSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 参数集编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 参数集名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty { get; set; }

        /// <summary>
        /// 检验容量
        /// </summary>
        public int Inspectcapacity { get; set; }

        /// <summary>
        /// 批次数量
        /// </summary>
        public int LotSize { get; set; }

        /// <summary>
        /// 批次单位
        /// </summary>
        public FQCLotUnitEnum LotUnit { get; set; }

        /// <summary>
        /// 是否同工单
        /// </summary>
        public TrueOrFalseEnum IsSameWorkOrder { get; set; }

        /// <summary>
        /// 是否同产线
        /// </summary>
        public TrueOrFalseEnum IsSameWorkCenter { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 状态(0-新建 1-启用 2-保留 3-废除)
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// FQC检验参数组Dto
    /// </summary>
    public record QualFqcParameterGroupDto : BaseEntityDto
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty { get; set; }

        /// <summary>
        /// 批次数量
        /// </summary>
        public int LotSize { get; set; }

        /// <summary>
        /// 批次单位
        /// </summary>
        public FQCLotUnitEnum LotUnit { get; set; }

        /// <summary>
        /// 是否同工单
        /// </summary>
        public TrueOrFalseEnum IsSameWorkOrder { get; set; }

        /// <summary>
        /// 是否同产线
        /// </summary>
        public TrueOrFalseEnum IsSameWorkCenter { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 状态(0-新建 1-启用 2-保留 3-废除)
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 参数组
        /// </summary>
        public IEnumerable<QualFqcParameterGroupDetailDto>? qualFqcParameterGroupDetailDtos { get; set; }
    }

    /// <summary>
    /// FQC检验参数组分页Dto
    /// </summary>
    public class QualFqcParameterGroupPagedQueryDto : PagerInfo 
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 状态(0-新建 1-启用 2-保留 3-废除)
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

    }

    public class QualFqcParameterGroupQueryDto : QueryDtoAbstraction
    {
        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }
    }

    /// <summary>
    /// 输出返回DTO
    /// </summary>
    public record QualFqcParameterGroupOutputDto: QualFqcParameterGroupUpdateDto
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 物料单位
        /// </summary>
        public string? MaterialUnit { get; set; }

        /// <summary>
        /// 检验数量
        /// </summary>
        public string? InspectCount { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }

        /// <summary>
        /// FQC检验项目版本
        /// </summary>
        //public string? Version { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string? CustomerCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string? CustomerName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string? UpdatedBy { get; set; }
    }


    public record QualFqcParameterGroupUpdateDto : QualFqcParameterGroupDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 参数组
        /// </summary>
        new public IEnumerable<QualFqcParameterGroupDetailOutputDto>? qualFqcParameterGroupDetailDtos { get; set; }
    }

    public record QualFqcParameterGroupDeleteDto : QualFqcParameterGroupDto
    {
        /// <summary>
        /// 要删除的组
        /// </summary>
        public long[] Ids { get; set; }
    }

}
