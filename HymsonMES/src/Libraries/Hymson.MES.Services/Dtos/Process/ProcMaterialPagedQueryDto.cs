using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.OnStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
    public class ProcMaterialPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 描述 :站点编码 
        /// 空值 : false  
        /// </summary>
        public string? SiteCode { get; set; }
    }
}
