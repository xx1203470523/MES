using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process.ResourceType
{
    /// <summary>
    /// 资源类型维护表查询对象
    /// </summary>
    public class ProcResourceTypePagedQuery : PagerInfo
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

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;
    }
}
