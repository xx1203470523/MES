/*
 *creator: Karl
 *
 *describe: 资源作业配置表    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-10 05:26:36
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 资源作业配置表Dto
    /// </summary>
    public record ProcResourceConfigJobDto : BaseEntityDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 所属资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 关联点(字典key值)
        /// </summary>
        public string LinkPoint { get; set; }

        /// <summary>
        /// 作业ID
        /// </summary>
        public long JobId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsUse { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string Parameter { get; set; }

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
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool? IsDeleted { get; set; }


    }

    public record ProcResourceConfigJobViewDto : ProcResourceConfigJobDto
    {
        /// <summary>
        /// 描述 :作业编号 
        /// 空值 : false  
        /// </summary>
        public string JobCode { get; set; }

        /// <summary>
        /// 描述 :作业名称 
        /// 空值 : false  
        /// </summary>
        public string JobName { get; set; }
    }

    /// <summary>
    /// 资源作业配置表新增Dto
    /// </summary>
    public record ProcResourceConfigJobCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 关联点
        /// </summary>
        public ResourceJobLinkPointEnum? LinkPoint { get; set; }

        /// <summary>
        /// 作业ID
        /// </summary>
       // [Required(ErrorMessage = "作业ID不能为空")]
        public long JobId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse { get; set; } = false;

        /// <summary>
        /// 参数
        /// </summary>
       // [MaxLength(length: 100, ErrorMessage = "参数超长")]
        public string Parameter { get; set; } = "";

        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }
    }

    /// <summary>
    /// 资源作业配置表更新Dto
    /// </summary>
    public record ProcResourceConfigJobModifyDto : ProcResourceConfigJobCreateDto
    {

    }

    /// <summary>
    /// 资源作业配置表分页Dto
    /// </summary>
    public class ProcResourceConfigJobPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }
    }
}
