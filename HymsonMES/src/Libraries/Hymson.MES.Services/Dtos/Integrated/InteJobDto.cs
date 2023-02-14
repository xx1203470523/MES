/*
 *creator: Karl
 *
 *describe: 作业表    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 04:32:34
 */

using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 作业表Dto
    /// </summary>
    public record InteJobDto : BaseEntityDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 作业编号
        /// </summary>
        public string JobCode { get; set; }

       /// <summary>
        /// 作业名称
        /// </summary>
        public string JobName { get; set; }

       /// <summary>
        /// 类程序
        /// </summary>
        public string ClassProgram { get; set; }

       /// <summary>
        /// 参数说明
        /// </summary>
        public string ParameterDescribe { get; set; }

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
    /// 作业表新增Dto
    /// </summary>
    public record InteJobCreateDto : BaseEntityDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 作业编号
        /// </summary>
        public string JobCode { get; set; }

       /// <summary>
        /// 作业名称
        /// </summary>
        public string JobName { get; set; }

       /// <summary>
        /// 类程序
        /// </summary>
        public string ClassProgram { get; set; }

       /// <summary>
        /// 参数说明
        /// </summary>
        public string ParameterDescribe { get; set; }

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
    /// 作业表更新Dto
    /// </summary>
    public record InteJobModifyDto : BaseEntityDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 作业编号
        /// </summary>
        public string JobCode { get; set; }

       /// <summary>
        /// 作业名称
        /// </summary>
        public string JobName { get; set; }

       /// <summary>
        /// 类程序
        /// </summary>
        public string ClassProgram { get; set; }

       /// <summary>
        /// 参数说明
        /// </summary>
        public string ParameterDescribe { get; set; }

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
    /// 作业表分页Dto
    /// </summary>
    public class InteJobPagedQueryDto : PagerInfo
    {
        ///// <summary>
        ///// 描述 :站点编码 
        ///// 空值 : false  
        ///// </summary>
        //public string SiteCode { get; set; }
    }
}
