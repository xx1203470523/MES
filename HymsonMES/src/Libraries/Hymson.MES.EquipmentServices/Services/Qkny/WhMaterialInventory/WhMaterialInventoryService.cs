using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.CoreServices.Services.Manufacture.WhMaterialInventory;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.EquipmentServices.Services.Qkny.EquEquipment;
using Microsoft.IdentityModel.Tokens;

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
        /// 物料仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 库存接收CoreService
        /// </summary>
        private readonly IWhMaterialInventoryCoreService _whMaterialInventoryCoreService;

        /// <summary>
        /// 设备服务
        /// </summary>
        private readonly IEquEquipmentService _equEquipmentService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="whMaterialInventoryCoreService"></param>
        /// <param name="equEquipmentService"></param>
        public WhMaterialInventoryService(IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IProcMaterialRepository procMaterialRepository,
            IWhMaterialInventoryCoreService whMaterialInventoryCoreService,
            IEquEquipmentService equEquipmentService)
        {
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _procMaterialRepository = procMaterialRepository;
            _whMaterialInventoryCoreService = whMaterialInventoryCoreService;
            _equEquipmentService = equEquipmentService;
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

        /// <summary>
        /// 库存接收
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task MaterialInventoryAsync(MaterialInventoryDto dto)
        {
            //获取设备基础信息
            var equResModel = await _equEquipmentService.GetEquResAsync(dto);

            //原材料条码解析
            foreach (var item in dto.BarCodeList)
            {
                if (item == null) continue;
                if (!item.IsRawMaterial) continue;

                var arr = item.BarCode.Trim().Split(',');
                if (arr.Length != 5)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES45231)).WithData("BarCode", item.BarCode);
                }

                if (!decimal.TryParse(arr[3], out decimal qty) || qty <= 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES45232)).WithData("BarCode", item.BarCode);
                }

                item.MaterialCode = arr[0];
                item.Qty = qty;
            }

            //查询物料信息
            var materialCodes = dto.BarCodeList.Select(x => x.MaterialCode).Distinct();
            var materialEntities = await _procMaterialRepository.GetByCodesAsync(new ProcMaterialsByCodeQuery
            {
                SiteId = equResModel.SiteId,
                MaterialCodes = materialCodes
            });
            var noExistMaterialCodes = materialCodes.Except(materialEntities.Select(x => x.MaterialCode).Distinct());
            if (noExistMaterialCodes.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45233)).WithData("MaterialCode", string.Join(',', noExistMaterialCodes));
            }

            await _whMaterialInventoryCoreService.MaterialInventoryAsync(new CoreServices.Bos.Manufacture.MaterialInventoryBo
            {
                SiteId = equResModel.SiteId,
                UserName = equResModel.EquipmentCode,
                IsCheckSupplier = false,
                BarCodeList = dto.BarCodeList.Select(x => new CoreServices.Bos.Manufacture.MaterialInventorySfcInfoBo
                {
                    Source = Core.Enums.MaterialInventorySourceEnum.Equipment,
                    MaterialId = materialEntities.First(x => x.MaterialCode == x.MaterialCode).Id,
                    MaterialBarCode = x.BarCode,
                    QuantityResidue = x.Qty,
                    //Batch = "",
                    SupplierId = 0,
                    Type = Core.Enums.WhMaterialInventoryTypeEnum.MaterialReceiving
                })
            });
        }
    }
}
