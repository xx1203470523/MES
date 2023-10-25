/*
 *creator: Karl
 *
 *describe: 标准模板打印配置信息    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-10-09 09:13:47
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 标准模板打印配置信息Dto
    /// </summary>
    public record ProcLabelTemplateRelationDto : BaseEntityDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 标签模板Id
        /// </summary>
        public long LabelTemplateId { get; set; }

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
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 打印配置信息
        /// </summary>
        public string PrintConfig { get; set; }

       /// <summary>
        /// 打印模板地址
        /// </summary>
        public string PrintTemplatePath { get; set; }

    }


    /// <summary>
    /// 标准模板打印配置信息新增Dto
    /// </summary>
    public record ProcLabelTemplateRelationCreateDto : BaseEntityDto
    {
       /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

       /// <summary>
        /// 打印配置信息
        /// </summary>
        public string PrintConfig { get; set; }

       /// <summary>
        /// 打印模板地址
        /// </summary>
        public string PrintTemplatePath { get; set; }

       
    }

    /// <summary>
    /// 标准模板打印配置信息更新Dto
    /// </summary>
    public record ProcLabelTemplateRelationModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 标签模板Id
        /// </summary>
        public long LabelTemplateId { get; set; }

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
        /// 是否删除;删除时赋值为主键
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 打印配置信息
        /// </summary>
        public string PrintConfig { get; set; }

       /// <summary>
        /// 打印模板地址
        /// </summary>
        public string PrintTemplatePath { get; set; }

       

    }

    /// <summary>
    /// 标准模板打印配置信息分页Dto
    /// </summary>
    public class ProcLabelTemplateRelationPagedQueryDto : PagerInfo
    {
    }
}
