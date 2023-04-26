using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command
{
    public class UpdateWhMaterialInventoryEmptyCommand
    {
        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<string> BarCodeList { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public  DateTime UpdateTime { get; set; }
    }
}
