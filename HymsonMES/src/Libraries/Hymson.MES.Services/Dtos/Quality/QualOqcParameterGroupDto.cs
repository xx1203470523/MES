using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Qual;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// OQC检验参数组新增/更新Dto
    /// </summary>
    public record QualOqcParameterGroupSaveDto : BaseEntityDto
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
        /// 客户Id
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty { get; set; }

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
    }

    /// <summary>
    /// OQC检验参数组Dto
    /// </summary>
    public record QualOqcParameterGroupDto : BaseEntityDto
    {

        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 参数集编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 参数集名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public long? CustomerId { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 参数组
        /// </summary>
        public IEnumerable<QualOqcParameterGroupDetailDto>? qualOqcParameterGroupDetailDtos { get; set; }


    }

    /// <summary>
    /// OQC检验参数组分页Dto
    /// </summary>
    public class QualOqcParameterGroupPagedQueryDto : PagerInfo {
        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string? CustomerName { get; set; }

        /// <summary>
        /// 状态 0、已禁用 2、启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }
    
    }








    public class QualOqcParameterGroupQueryDto : QueryDtoAbstraction
    {
        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }
    }

    /// <summary>
    /// 输出返回DTO
    /// </summary>
    public record QualOqcParameterGroupOutputDto: QualOqcParameterGroupUpdateDto
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
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }

        /// <summary>
        /// OQC检验项目版本
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


    public record QualOqcParameterGroupUpdateDto : QualOqcParameterGroupDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 参数组
        /// </summary>
        new public IEnumerable<QualOqcParameterGroupDetailOutputDto>? qualOqcParameterGroupDetailDtos { get; set; }
    }

    public record QualOqcParameterGroupDeleteDto : QualOqcParameterGroupDto
    {
        /// <summary>
        /// 要删除的组
        /// </summary>
        public IEnumerable<long> Ids { get; set; }
    }

}
