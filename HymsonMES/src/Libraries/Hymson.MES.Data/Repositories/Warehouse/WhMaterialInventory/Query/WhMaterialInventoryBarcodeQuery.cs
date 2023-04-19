using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query
{
    public class WhMaterialInventoryBarcodeQuery
    {
        /// <summary>
        /// 条码集合
        /// </summary>
        public IEnumerable<string> BarCodes { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }
    }
}
