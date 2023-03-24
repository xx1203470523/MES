/*
 *creator: Karl
 *
 *describe: 资源配置表    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-10 10:21:26
 */

using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 资源配置表Dto
    /// </summary>
    public record ProcResourceConfigResDto : BaseEntityDto
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
        /// 设置类型(字典配置)
        /// </summary>
        public int SetType { get; set; }

       /// <summary>
        /// 设置值
        /// </summary>
        public string Value { get; set; }

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

    /// <summary>
    /// 资源配置表新增Dto
    /// </summary>
    public record ProcResourceConfigResCreateDto : BaseEntityDto
    {
       /// <summary>
        /// 设置类型(字典配置)
        /// </summary>
        public int SetType { get; set; }

       /// <summary>
        /// 设置值
        /// </summary>
        public int? Value { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }
    }

    /// <summary>
    /// 资源配置表更新Dto
    /// </summary>
    public record ProcResourceConfigResModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 设置类型
        /// </summary>
        public int SetType { get; set; }

        /// <summary>
        /// 设置值
        /// </summary>
        public int? Value { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        //[Required(ErrorMessage = "操作类型不可为空")]
        //[Range(1, 3)]
       // public int OperationType { get; set; }
    }

    /// <summary>
    /// 资源配置表分页Dto
    /// </summary>
    public class ProcResourceConfigResPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }
    }
}
