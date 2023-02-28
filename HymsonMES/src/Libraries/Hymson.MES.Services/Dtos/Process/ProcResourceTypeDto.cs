using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 异常类型
    /// </summary>
    public record ProcResourceTypeDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 描述 :资源类型 
        /// 空值 : false  
        /// </summary>
        public string ResType { get; set; }

        /// <summary>
        /// 描述 :资源类型名称 
        /// 空值 : false  
        /// </summary>
        public string ResTypeName { get; set; }

        /// <summary>
        /// 描述 :描述 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }

        public long[] ResourceIds { get; set; }

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

    public record ProcResourceTypeViewDto : ProcResourceTypeDto
    {
        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResName { get; set; }
    }

    /// <summary>
    /// 查询实体
    /// </summary>
    public class ProcResourceTypePagedQueryDto : PagerInfo
    {
        /// 描述 :资源类型代码 
        /// 空值 : false  
        /// </summary>
        public string? ResType { get; set; }

        /// <summary>
        /// 描述 :资源类型名称 
        /// 空值 : false  
        /// </summary>
        public string? ResTypeName { get; set; }

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
    }

    /// <summary>
    /// 新增实体
    /// </summary>
    public record ProcResourceTypeAddDto : BaseEntityDto
    {
        /// <summary>
        /// 描述 :资源类型 
        /// 空值 : false  
        /// </summary>
        public string ResType { get; set; }

        /// <summary>
        /// 描述 :资源类型名称 
        /// 空值 : false  
        /// </summary>
        public string ResTypeName { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 关联的资源Id
        /// </summary>
        public IEnumerable<long> ResourceIds { get; set; }
    }

    /// <summary>
    /// 修改实体
    /// </summary>
    public record ProcResourceTypeUpdateDto : BaseEntityDto
    {
        /// <summary>
        /// 唯一Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 描述 :资源类型名称 
        /// 空值 : false  
        /// </summary>
        public string ResTypeName { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 关联的资源Id
        /// </summary>
        public IEnumerable<long> ResourceIds { get; set; }
    }
}
