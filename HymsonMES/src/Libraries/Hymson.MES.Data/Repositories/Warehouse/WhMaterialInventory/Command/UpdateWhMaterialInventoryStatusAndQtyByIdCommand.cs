using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateWhMaterialInventoryStatusAndQtyByIdCommand : UpdateCommand
    {
        /// <summary>
        /// 条码
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// ID集合（备件）
        /// </summary>
        public WhMaterialInventoryStatusEnum Status { get; set; }
    }
}
