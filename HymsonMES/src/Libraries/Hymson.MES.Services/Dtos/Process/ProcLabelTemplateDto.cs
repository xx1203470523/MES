/*
 *creator: Karl
 *
 *describe: 仓库标签模板    Dto | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-03-09 02:51:26
 */

using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 仓库标签模板Dto
    /// </summary>
    public record ProcLabelTemplateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标签物理路径
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// 模板内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

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


    }
    public record PreviewImageDataDto : BaseEntityDto
    {
        public string base64Str { get; set; }
        public bool result { get; set; }
    }


    /// <summary>
    /// 仓库标签模板新增Dto
    /// </summary>
    public record ProcLabelTemplateCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标签物理路径
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 模板内容
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public string? printDataModel { get; set; }

        /// <summary>
        /// 模板打印配置信息
        /// </summary>
        public ProcLabelTemplateRelationCreateDto ProcLabelTemplateRelationCreateDto { get; set; }
    }

    /// <summary>
    /// 仓库标签模板更新Dto
    /// </summary>
    public record ProcLabelTemplateModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标签物理路径
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 模板内容
        /// </summary>
        public string? Content { get; set; }

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
        /// 数据源
        /// </summary>
        public string? printDataModel { get; set; }


        /// <summary>
        /// 模板打印配置信息
        /// </summary>
        public ProcLabelTemplateRelationCreateDto ProcLabelTemplateRelationCreateDto { get; set; }
    }

    /// <summary>
    /// 仓库标签模板分页Dto
    /// </summary>
    public class ProcLabelTemplatePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public string? Name { get; set; }
    }

    public record PrintDataResultDto
    {
        /// <summary>
        /// 打印模板打印设计信息
        /// </summary>
        public ProcLabelTemplateRelationDto ProcLabelTemplateRelationDto { get; set; }

        /// <summary>
        /// 打印模型的数据 对应的数据
        /// </summary>
        public string PrintBodies { get; set; }

    }
}
