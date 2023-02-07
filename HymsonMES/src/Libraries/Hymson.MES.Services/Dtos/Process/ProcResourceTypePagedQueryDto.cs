using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
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

        /// <summary>
        /// 站点
        /// </summary>
        public string? SiteCode { get; set; }
    }
}
