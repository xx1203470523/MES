﻿using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateStatusByBarCodeCommand : UpdateCommand
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal QuantityResidue { get; set; }

        /// <summary>
        /// ID集合（备件）
        /// </summary>
        public WhMaterialInventoryStatusEnum Status { get; set; }
    }
}
