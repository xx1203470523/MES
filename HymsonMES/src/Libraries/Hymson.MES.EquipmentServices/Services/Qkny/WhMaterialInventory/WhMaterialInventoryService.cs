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
        /// 供应商仓储
        /// </summary>
        private readonly IWhSupplierRepository _whSupplierRepository;

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
        /// <param name="whSupplierRepository"></param>
        /// <param name="whMaterialInventoryCoreService"></param>
        /// <param name="equEquipmentService"></param>
        public WhMaterialInventoryService(IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IProcMaterialRepository procMaterialRepository,
            IWhSupplierRepository whSupplierRepository,
            IWhMaterialInventoryCoreService whMaterialInventoryCoreService,
            IEquEquipmentService equEquipmentService)
        {
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _procMaterialRepository = procMaterialRepository;
            _whSupplierRepository = whSupplierRepository;
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
                string barCodeStr = string.Join(";",query.BarCodes);
                throw new CustomerValidationException(nameof(ErrorCode.MES45081))
                    .WithData("barCodeStr", barCodeStr);
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

            //原材料条码解析（物料代码(6)，供应商代码(5)，生产批号/日期(8)，班次P01(3)，最小包装数量,最小包装流水号(P001B001)）
            //原材料条码解析（物料代码（久期8位数字码）,物料名称（6位BOM物料代码）,物料批次（8位入库日期+3位流水码）,供应商批次,生产日期,总托数,本托数量,本托编号,最小包装数量,最小包装流水号） 2024-07-11
            //var barcodeSupplierDic = new Dictionary<string, string>();
            foreach (var item in dto.BarCodeList)
            {
                if (item == null) continue;
                if (!item.IsRawMaterial) continue;

                var arr = item.BarCode.Trim().Split(',');
                if (arr.Length < 7)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES45231)).WithData("BarCode", item.BarCode);
                }

                if (!decimal.TryParse(arr[6], out decimal qty) || qty <= 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES45232)).WithData("BarCode", item.BarCode);
                }

                item.MaterialCode = arr[1];
                item.Qty = qty;
                //barcodeSupplierDic.Add(item.BarCode, arr[1]);
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
            //查询供应商信息
            //var supplierCodes = await _whSupplierRepository.GetByCodesAsync(new WhSuppliersByCodeQuery
            //{
            //    SiteId = equResModel.SiteId,
            //    Codes = barcodeSupplierDic.Values.Distinct()
            //});

            await _whMaterialInventoryCoreService.MaterialInventoryAsync(new CoreServices.Bos.Manufacture.MaterialInventoryBo
            {
                SiteId = equResModel.SiteId,
                UserName = equResModel.EquipmentCode,
                IsCheckSupplier = false,
                BarCodeList = dto.BarCodeList.Select(x => new CoreServices.Bos.Manufacture.MaterialInventorySfcInfoBo
                {
                    Source = Core.Enums.MaterialInventorySourceEnum.WMS,
                    MaterialId = materialEntities.First(z => z.MaterialCode == x.MaterialCode).Id,
                    MaterialBarCode = x.BarCode,
                    QuantityResidue = x.Qty,
                    SupplierId = 0, //supplierCodes.FirstOrDefault(s => s.Code == barcodeSupplierDic[x.BarCode])?.Id ?? 0,
                    Type = Core.Enums.WhMaterialInventoryTypeEnum.MaterialReceiving
                })
            });
        }
    }
}
