using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command
{
    public class UpdateStatusByIdCommand:UpdateCommand
    {
        /// <summary>
        /// ID集合（备件）
        /// </summary>
        public WhMaterialInventoryStatusEnum Status { get; set; }

        /// <summary>
        /// Id列表
        /// </summary>
        public IEnumerable<long> Ids { get; set; }
    }
}
