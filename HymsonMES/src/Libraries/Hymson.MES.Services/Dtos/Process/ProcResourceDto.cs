using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
    public record ProcResourceDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

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

        /// <summary>
        /// 描述 :状态 
        /// 空值 : false  
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 描述 :所属资源类型ID 
        /// 空值 : false  
        /// </summary>
        public long ResTypeId { get; set; }

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; } = "";

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }

    /// <summary>
    /// 资源维护表查询对象
    /// </summary>
    public class ProcResourcePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 描述 :资源代码 
        /// 空值 : false  
        /// </summary>
        public string? ResCode { get; set; }

        /// <summary>
        /// 描述 :资源名称 
        /// 空值 : false  
        /// </summary>
        public string? ResName { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public string? ResType { get; set; }

        /// <summary>
        /// 资源类型id
        /// </summary>
        public long? ResTypeId { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : false  
        /// </summary>
        public string? Status { get; set; }

        //站点
        public string? SiteCode { get; set; }
    }

    public record ProcResourceViewDto: ProcResourceDto
    {
        /// <summary>
        /// 资源
        /// </summary>
        public string ResType { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public string ResTypeName { get; set; }
    }
}
