using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny.WhMaterialInventory
{
    /// <summary>
    /// 物料库存
    /// </summary>
    public interface IWhMaterialInventoryService
    {
        /// <summary>
        /// 根据物料条码获取数据
        /// 不管数量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<WhMaterialInventoryEntity>> GetByBarCodesAsync(WhMaterialInventoryBarCodesQuery query);

        /// <summary>
        /// 根据物料条码获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<WhMaterialInventoryEntity> GetByBarCodeAsync(WhMaterialInventoryBarCodeQuery query);

        /// <summary>
        /// 库存接收
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task MaterialInventoryAsync(MaterialInventoryDto dto);
    }
}
