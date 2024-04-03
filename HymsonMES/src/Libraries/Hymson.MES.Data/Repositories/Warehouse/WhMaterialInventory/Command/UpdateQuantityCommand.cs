using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command
{
    /// <summary>
    /// 
    /// </summary>
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

        /// <summary>
        /// 数量（原始）
        /// </summary>
        public decimal QuantityOriginal { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UpdateQuantityRangeCommand : UpdateQuantityCommand
    {
        /// <summary>
        /// 条码状态
        /// </summary>
        public WhMaterialInventoryStatusEnum Status { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class UpdateQuantityResidueBySfcsCommand : UpdateCommand
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal QuantityResidue { get; set; }

        /// <summary>
        /// 条码集合
        /// </summary>
        public IEnumerable<string> Sfcs { get; set; }

    }
}
