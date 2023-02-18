/*
 *creator: Karl
 *
 *describe: 上料点表    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-17 08:57:53
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
    /// 上料点表Dto
    /// </summary>
    public record ProcLoadPointDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 上料点
        /// </summary>
        public string LoadPoint { get; set; }

       /// <summary>
        /// 上料点名称
        /// </summary>
        public string LoadPointName { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

       /// <summary>
        /// 说明
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
    /// 上料点表新增Dto
    /// </summary>
    public record ProcLoadPointCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 上料点
        /// </summary>
        public string LoadPoint { get; set; }

       /// <summary>
        /// 上料点名称
        /// </summary>
        public string LoadPointName { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

       /// <summary>
        /// 说明
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
    /// 上料点表更新Dto
    /// </summary>
    public record ProcLoadPointModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 上料点
        /// </summary>
        public string LoadPoint { get; set; }

       /// <summary>
        /// 上料点名称
        /// </summary>
        public string LoadPointName { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

       /// <summary>
        /// 说明
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
    /// 上料点表分页Dto
    /// </summary>
    public class ProcLoadPointPagedQueryDto : PagerInfo
    {
        ///// <summary>
        ///// 描述 :站点编码 
        ///// 空值 : false  
        ///// </summary>
        //public string SiteCode { get; set; }
    }
}
