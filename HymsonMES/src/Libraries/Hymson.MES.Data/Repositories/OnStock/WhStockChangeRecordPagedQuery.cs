using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.OnStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.OnStock
{
    public class WhStockChangeRecordPagedQuery:PagerInfo
    {
        /// <summary>
        /// 描述 :站点编码 
        /// 空值 : false  
        /// </summary>
        public string? SiteCode { get; set; }

        /// <summary>
        /// 描述 :变更类型(0-库存异动 1-储位转换 2-形态转换) 
        /// 空值 : false  
        /// </summary>
        public ChangeTypeEnum? ChangeType { get; set; }

        /// <summary>
        /// 描述 :来源单号 
        /// 空值 : false  
        /// </summary>
        public string? SourceNo { get; set; }
    }
}
