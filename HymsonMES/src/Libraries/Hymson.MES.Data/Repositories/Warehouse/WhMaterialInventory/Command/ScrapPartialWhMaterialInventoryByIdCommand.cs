﻿using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command
{
    /// <summary>
    /// 报废部分数量
    /// </summary>
    public class ScrapPartialWhMaterialInventoryByIdCommand : UpdateCommand
    {
        /// <summary>
        /// 条码数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        ///报废数量
        /// </summary>
        public decimal ScrapQty { get; set; }


        /// <summary>
        ///合格数量
        /// </summary>
        public decimal CurrentQuantityResidue { get; set; }

        /// <summary>
        /// 条码Id
        /// </summary>
        public long Id { get; set; }

    }
}