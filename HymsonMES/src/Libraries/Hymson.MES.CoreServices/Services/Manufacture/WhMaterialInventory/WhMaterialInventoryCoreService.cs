using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.CoreServices.Services.Manufacture.WhMaterialInventory
{
    /// <summary>
    /// 车间库存接收Service
    /// </summary>
    public class WhMaterialInventoryCoreService : IWhMaterialInventoryCoreService
    {
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        public WhMaterialInventoryCoreService(IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
            IProcMaterialRepository procMaterialRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository)
        {
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _procMaterialRepository = procMaterialRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
        }

        /// <summary>
        /// 车间库存接收
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<int> MaterialInventoryAsync(MaterialInventoryBo bo)
        {
            if (bo == null || bo.BarCodeList == null || !bo.BarCodeList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10111));
            }

            List<WhMaterialInventoryEntity> materialInventories = new();
            List<WhMaterialStandingbookEntity> materialStandingBooks = new();
            List<ManuSfcEntity> manuSfcEntities = new();
            List<ManuSfcInfoEntity> manuSfcInfoEntities = new();

            // 检查条码Id是否在物料里面
            var procMaterials = await _procMaterialRepository.GetByIdsAsync(bo.BarCodeList.Select(it => it.MaterialId).Distinct());
            if (procMaterials == null || !procMaterials.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15101));
            var procMaterialDict = procMaterials.ToDictionary(node => node.Id);

            var validationFailures = new List<ValidationFailure>();
            foreach (var item in bo.BarCodeList)
            {
                var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.MaterialBarCode);
                validationFailure.FormattedMessagePlaceholderValues.Add("MaterialCode", item.MaterialBarCode);

                // 校验是否有重复条码
                item.MaterialBarCode = item.MaterialBarCode.Trim();
                var isMaterialBarCodeList = bo.BarCodeList.Where(it => it.MaterialBarCode.Trim() == item.MaterialBarCode);
                if (isMaterialBarCodeList.Count() > 1)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15107);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                // 是否未设置供应商
                if (bo.IsCheckSupplier && item.SupplierId <= 0)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15108);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                // 是否数量不足
                if (item.QuantityResidue <= 0)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15103);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                // 是否存在该物料
                if (!procMaterialDict.ContainsKey(item.MaterialId))
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15101);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                var materialEntity = procMaterialDict[item.MaterialId];

                // 查询是否已存在物料条码
                await CheckMaterialBarCodeAnyAsync(item.MaterialBarCode, bo.SiteId);

                #region 数据组装

                var manuSfcEntity = new ManuSfcEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = bo.SiteId,
                    SFC = item.MaterialBarCode,
                    Qty = item.QuantityResidue,
                    Type = SfcTypeEnum.NoProduce,
                    IsUsed = YesOrNoEnum.No,
                    Status = SfcStatusEnum.Complete,
                    CreatedBy = bo.UserName,
                    UpdatedBy = bo.UserName,
                };

                manuSfcEntities.Add(manuSfcEntity);

                manuSfcInfoEntities.Add(new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = bo.SiteId,
                    SfcId = manuSfcEntity.Id,
                    ProductId = materialEntity.Id,
                    IsUsed = true,
                    CreatedBy = bo.UserName,
                    UpdatedBy = bo.UserName,
                });

                // 物料库存
                materialInventories.Add(new WhMaterialInventoryEntity
                {
                    SupplierId = item.SupplierId,
                    MaterialId = materialEntity.Id,
                    MaterialBarCode = item.MaterialBarCode,
                    Batch = item.Batch,
                    MaterialType = MaterialInventoryMaterialTypeEnum.PurchaseParts,
                    QuantityResidue = item.QuantityResidue,
                    Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                    DueDate = item.DueDate,
                    Source = item.Source,

                    SiteId = bo.SiteId,
                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = bo.UserName,
                    UpdatedBy = bo.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now()
                });

                string version = materialEntity.Version ?? "";
                if (!string.IsNullOrWhiteSpace(item.Version)) version = item.Version;

                // 台账数据
                materialStandingBooks.Add(new WhMaterialStandingbookEntity
                {
                    MaterialCode = materialEntity.MaterialCode,
                    MaterialName = materialEntity.MaterialName,
                    MaterialBarCode = item.MaterialBarCode,
                    Batch = item.Batch,
                    Quantity = item.QuantityResidue,
                    Unit = materialEntity.Unit ?? "",
                    Type = item.Type,
                    Source = item.Source,
                    SupplierId = item.SupplierId,
                    MaterialVersion = version,

                    SiteId = bo.SiteId,
                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = bo.UserName,
                    UpdatedBy = bo.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now()
                });

                #endregion
            }

            // 是否存在错误
            if (validationFailures.Any())
            {
                throw new ValidationException("", validationFailures);
            }

            // 保存实体
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _whMaterialInventoryRepository.InsertsAsync(materialInventories);
            rows += await _whMaterialStandingbookRepository.InsertsAsync(materialStandingBooks);
            rows += await _manuSfcRepository.InsertRangeAsync(manuSfcEntities);
            rows += await _manuSfcInfoRepository.InsertsAsync(manuSfcInfoEntities);
            trans.Complete();

            return rows;
        }

        /// <summary>
        /// 查询是否已存在物料条码
        /// </summary>
        /// <param name="materialBarCode"></param>
        /// <returns></returns>
        public async Task<bool> CheckMaterialBarCodeAnyAsync(string materialBarCode, long siteId)
        {
            var pagedInfo = await _whMaterialInventoryRepository.GetWhMaterialInventoryEntitiesAsync(new WhMaterialInventoryQuery
            {
                MaterialBarCode = materialBarCode,
                SiteId = siteId
            });

            if (pagedInfo != null && pagedInfo.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15104)).WithData("MaterialCode", materialBarCode);
            }

            var sfcEntit = await _manuSfcRepository.GetBySFCAsync(new EntityBySFCQuery { SFC = materialBarCode, SiteId = siteId });
            if (sfcEntit != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES152016)).WithData("MaterialCode", materialBarCode);
            }
            return false;
        }
    }
}
