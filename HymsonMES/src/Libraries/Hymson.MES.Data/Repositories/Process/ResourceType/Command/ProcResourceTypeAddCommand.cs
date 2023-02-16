using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process.ResourceType
{
    public class ProcResourceTypeAddCommand : BaseEntity
    {
        /// <summary>
        /// 站点
        /// </summary>
        public string SiteCode { get; set; }

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
        public IEnumerable<string> ResourceIds { get; set; }
    }
}
