using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Microsoft.IdentityModel.Tokens;
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
    public class WhMaterialInventoryService : IWhMaterialInventoryService
    {
        /// <summary>
        /// 物料库存仓储
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WhMaterialInventoryService(IWhMaterialInventoryRepository whMaterialInventoryRepository)
        {
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
        }

        /// <summary>
        /// 根据物料条码获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<WhMaterialInventoryEntity>> GetByBarCodesAsync(WhMaterialInventoryBarCodesQuery query)
        {
            var dbList = await _whMaterialInventoryRepository.GetByBarCodesNoQtyAsync(query);
            if (dbList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45081));
            }
            return dbList.ToList();
        }

        /// <summary>
        /// 根据物料条码获取数据
        /// 不抛异常
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<WhMaterialInventoryEntity> GetByBarCodeAsync(WhMaterialInventoryBarCodeQuery query)
        {
            WhMaterialInventoryBarCodesQuery listQuery = new WhMaterialInventoryBarCodesQuery();
            listQuery.SiteId = (long)query.SiteId;
            listQuery.BarCodes = new List<string>() { query.BarCode };
            var dbList = await _whMaterialInventoryRepository.GetByBarCodesNoQtyAsync(listQuery);
            if (dbList.IsNullOrEmpty() == true)
            {
                return null;
            }
            return dbList.First();
        }
    }
}
