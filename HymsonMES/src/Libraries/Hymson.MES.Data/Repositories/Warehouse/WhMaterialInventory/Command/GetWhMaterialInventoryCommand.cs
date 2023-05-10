using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command
{
    public class WhSupplierByMaterialCommand
    {
        /// <summary>
        /// 物料ID 
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; } = 0;

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }
    }
}
