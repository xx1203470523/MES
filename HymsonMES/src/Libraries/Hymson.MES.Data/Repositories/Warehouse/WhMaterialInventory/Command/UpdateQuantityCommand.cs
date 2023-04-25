using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command
{
    public class UpdateQuantityCommand : UpdateCommand
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal QuantityResidue { get; set; }
    }
}
