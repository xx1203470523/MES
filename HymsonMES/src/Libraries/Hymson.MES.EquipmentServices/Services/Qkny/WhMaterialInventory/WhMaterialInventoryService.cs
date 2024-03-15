﻿using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
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
        public async Task<WhMaterialInventoryEntity> GetByBarCodeAsync(WhMaterialInventoryBarCodeQuery query)
        {
            var dbModel = await _whMaterialInventoryRepository.GetByBarCodeAsync(query);
            if(dbModel == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45081));
            }
            return dbModel!;
        }
    }
}
