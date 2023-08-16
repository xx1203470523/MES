using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 检验规则（首检才有）新增/更新Dto
    /// </summary>
    public record QualIpqcInspectionRuleSaveDto : BaseEntityDto
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
        /// 检验方式;1、停机 2、不停机
        /// </summary>
        public IPQCRuleWayEnum Way { get; set; }

        /// <summary>
        /// 指定规则;1、固定 2、随机 3、顺序
        /// </summary>
        public IPQCSpecifyRuleEnum SpecifyRule { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 单位;1、台、2、%
        /// </summary>
        public IPQCRuleUnitEnum Unit { get; set; }

        /// <summary>
        /// 关联资源
        /// </summary>
        public IEnumerable<QualIpqcInspectionRuleResourceRelationSaveDto>? Resources { get; set; }
    }

    /// <summary>
    /// 检验规则（首检才有）Dto
    /// </summary>
    public record QualIpqcInspectionRuleDto : BaseEntityDto
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
        /// 检验方式;1、停机 2、不停机
        /// </summary>
        public IPQCRuleWayEnum Way { get; set; }

        /// <summary>
        /// 指定规则;1、固定 2、随机 3、顺序
        /// </summary>
        public IPQCSpecifyRuleEnum SpecifyRule { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 单位;1、台、2、%
        /// </summary>
        public IPQCRuleUnitEnum Unit { get; set; }

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
    /// 检验规则（首检才有）分页Dto
    /// </summary>
    public class QualIpqcInspectionRulePagedQueryDto : PagerInfo { }

}
