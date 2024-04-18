/*
 *creator: Karl
 *
 *describe: 上料点关联资源表    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-18 09:36:09
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
    /// 上料点关联资源表Dto
    /// </summary>
    public record ProcLoadPointLinkResourceDto : BaseEntityDto
    {
       /// <summary>
        /// 所属资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 所属资源ID
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }
               
    }


    /// <summary>
    /// 上料点关联资源表新增Dto
    /// </summary>
    public record ProcLoadPointLinkResourceCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 序号( 程序生成)
        /// </summary>
        public long SerialNo { get; set; }

       /// <summary>
        /// 所属上料点ID
        /// </summary>
        public long LoadPointId { get; set; }

       /// <summary>
        /// 所属资源ID
        /// </summary>
        public long ResourceId { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

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
        public string? UpdatedBy { get; set; }

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
    /// 上料点关联资源表更新Dto
    /// </summary>
    public record ProcLoadPointLinkResourceModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 序号( 程序生成)
        /// </summary>
        public long SerialNo { get; set; }

       /// <summary>
        /// 所属上料点ID
        /// </summary>
        public long LoadPointId { get; set; }

       /// <summary>
        /// 所属资源ID
        /// </summary>
        public long ResourceId { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

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
        public string? UpdatedBy { get; set; }

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
    /// 上料点关联资源表分页Dto
    /// </summary>
    public class ProcLoadPointLinkResourcePagedQueryDto : PagerInfo
    {
        
    }

    /// <summary>
    /// 上料点关联资源表Dto
    /// </summary>
    public record ProcLoadPointLinkResourceViewDto : BaseEntityDto
    {
        /// <summary>
        /// 描述 :所属资源ID 
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 描述 :资源代码 
        /// 空值 : false  
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 描述 :资源名称 
        /// 空值 : false  
        /// </summary>
        public string ResName { get; set; }
    }

}
