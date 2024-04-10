/*
 *creator: Karl
 *
 *describe: 降级规则    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-07 02:00:57
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 降级规则Dto
    /// </summary>
    public record ManuDowngradingRuleDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 序号
        /// </summary>
        public int SerialNumber { get; set; }

       /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

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
    /// 降级规则新增Dto
    /// </summary>
    public record ManuDowngradingRuleCreateDto : BaseEntityDto
    {
       /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

    }

    /// <summary>
    /// 降级规则更新Dto
    /// </summary>
    public record ManuDowngradingRuleModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

    }

    /// <summary>
    /// 降级规则分页Dto
    /// </summary>
    public class ManuDowngradingRulePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 或编码
        /// </summary>
        public string? OrCode {  get; set; }

        /// <summary>
        /// 或名称
        /// </summary>
        public string? OrName {  get; set; }
    }

    public class ManuDowngradingRuleChangeSerialNumberDto 
    {
        public long Id { set; get; }

        /// <summary>
        /// 序号
        /// </summary>
        public int SerialNumber { get; set; }
    }
}
