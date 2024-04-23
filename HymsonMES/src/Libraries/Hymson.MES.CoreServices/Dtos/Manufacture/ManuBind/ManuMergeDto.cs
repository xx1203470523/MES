using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Dtos.Manufacture.ManuBind
{
    
    /// <summary>
    /// 条码合并
    /// </summary>
    public class ManuMergeRequestDto
    {
        /// <summary>
        /// 要合并的条码集合
        /// </summary>
        public IEnumerable<string> Barcodes { get; set; }
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
       
    }
}
