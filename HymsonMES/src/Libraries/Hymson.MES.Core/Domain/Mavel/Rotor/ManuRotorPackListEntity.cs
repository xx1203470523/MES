using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Mavel.Rotor
{
    /// <summary>
    /// 转子包装列表实体
    /// </summary>
    public class ManuRotorPackListEntity : BaseEntity
    {
        /// <summary>  
        /// 箱码  
        /// </summary>  
        public string BoxCode { get; set; }

        /// <summary>  
        /// 产品代码  
        /// </summary>  
        public string ProductCode { get; set; }

        /// <summary>  
        /// 产品编号  
        /// </summary>  
        public string ProductNo { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
    }
}
