using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Manufacture.WhMaterialInventoryScrap;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Services.Dtos.Manufacture.ManuSFCScrap;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Minio.DataModel;

namespace Hymson.MES.Services.Services.Manufacture.ManuSfcScrapservice
{
    /// <summary>
    /// 部分报废实现
    /// </summary>
    public class ManuMaterialScrapService : IManuMaterialScrapService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 条码信息表 仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 条码信息表  仓储
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 条码生产信息（物理删除） 仓储
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 物料库存 仓储
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 条码步骤表仓储 仓储
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 报废表仓储
        /// </summary>
        private readonly IManuSfcScrapRepository _manuSfcScrapRepository;

        /// <summary>
        /// 工单表仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 物料表仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;
        private readonly IWhMaterialInventoryScrapRepository _whMaterialInventoryScrapRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="localizationService"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcScrapRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        /// <param name="whMaterialInventoryScrapRepository"></param>
        public ManuMaterialScrapService(ICurrentUser currentUser, ICurrentSite currentSite, IManuSfcRepository manuSfcRepository, IManuSfcInfoRepository manuSfcInfoRepository, IManuSfcProduceRepository manuSfcProduceRepository, ILocalizationService localizationService, IWhMaterialInventoryRepository whMaterialInventoryRepository, IManuSfcStepRepository manuSfcStepRepository, IManuSfcScrapRepository manuSfcScrapRepository, IPlanWorkOrderRepository planWorkOrderRepository, IProcMaterialRepository procMaterialRepository, IWhMaterialStandingbookRepository whMaterialStandingbookRepository, IWhMaterialInventoryScrapRepository whMaterialInventoryScrapRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _localizationService = localizationService;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcScrapRepository = manuSfcScrapRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procMaterialRepository = procMaterialRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _whMaterialInventoryScrapRepository = whMaterialInventoryScrapRepository;
        }

        /// <summary>
        /// 报废
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task ScrapAsync(ManuMaterialScrapDto param)
        {
            //条码表
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SFCs = param.BarcodeScrapList.Select(x => x.SFC),
                SiteId = _currentSite.SiteId ?? 0,
                Type = SfcTypeEnum.NoProduce
            });
            //条码信息表
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcEntities.Select(x => x.Id));
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = param.BarcodeScrapList.Select(x => x.SFC), SiteId = _currentSite.SiteId ?? 00 };
            ////在制品信息
            //var manuSfcProduces = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);

            //修改库存信息
            var whMaterialInventoryList = await _whMaterialInventoryRepository.GetByBarCodesAsync(new Data.Repositories.Warehouse.WhMaterialInventory.Query.WhMaterialInventoryBarCodesQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                BarCodes = param.BarcodeScrapList.Select(x => x.SFC)
            });
            var materialList = await _procMaterialRepository.GetByIdsAsync(whMaterialInventoryList.Select(x => x.MaterialId));
            var validationFailures = new List<ValidationFailure>();
            List<WhMaterialInventoryScrapEntity> manuSfcScrapEntities = new();
            List<ManuSFCPartialScrapByIdCommand> manuSFCPartialScrapByIdCommandList = new();
           
            List<WhMaterialStandingbookEntity> whMaterialStandingbookEntities = new();
            List<ScrapPartialWhMaterialInventoryByIdCommand> scrapPartialWhMaterialInventoryEmptyByIdCommandList = new();
            List<WhMaterialInventoryScrapNgRelationEntity> whMaterialInventoryScrapNgRelationEntitiesList = new List<WhMaterialInventoryScrapNgRelationEntity>();
            foreach (var barcodeItem in param.BarcodeScrapList)
            {
                var sfcEntity = sfcEntities.FirstOrDefault(x => x.SFC == barcodeItem.SFC);

                if (sfcEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", barcodeItem.SFC}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", barcodeItem.SFC);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15415);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                if (sfcEntity.Qty < barcodeItem.Qty)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", barcodeItem.SFC}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", barcodeItem.SFC);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15448);
                    validationFailure.FormattedMessagePlaceholderValues.Add("Qty", sfcEntity.Qty);
                    validationFailure.FormattedMessagePlaceholderValues.Add("ScrapQty", barcodeItem.Qty);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                if (barcodeItem.Qty == 0)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", barcodeItem.SFC}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", barcodeItem.SFC);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15448);
                    validationFailure.FormattedMessagePlaceholderValues.Add("Qty", sfcEntity.Qty);
                    validationFailure.FormattedMessagePlaceholderValues.Add("ScrapQty", barcodeItem.Qty);
                    validationFailures.Add(validationFailure);
                    continue;
                }


                var manuSFCPartialScrapByIdCommand = new ManuSFCPartialScrapByIdCommand
                {
                    Id = sfcEntity.Id,
                    ScrapQty = (sfcEntity.ScrapQty ?? 0) + barcodeItem.Qty,
                    Qty = sfcEntity.Qty - barcodeItem.Qty,
                    UpdatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                    CurrentQty = sfcEntity.Qty,
                    CurrentStatus = sfcEntity.Status,
                };
                if (sfcEntity.Qty == barcodeItem.Qty)
                {
                    manuSFCPartialScrapByIdCommand.Status = SfcStatusEnum.Scrapping;
                }
                else
                {
                    manuSFCPartialScrapByIdCommand.Status = sfcEntity.Status;
                }

                manuSFCPartialScrapByIdCommandList.Add(manuSFCPartialScrapByIdCommand);

                var sfcInfoEntity = sfcInfoEntities.FirstOrDefault(x => x.SfcId == sfcEntity.Id);

                if (sfcInfoEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", barcodeItem.SFC}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", barcodeItem.SFC);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES16701);

                    validationFailures.Add(validationFailure);
                    continue;

                }

                var whMaterialInventoryEntity = whMaterialInventoryList.FirstOrDefault(x => x.MaterialBarCode == barcodeItem.SFC);
                if (whMaterialInventoryEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", barcodeItem.SFC}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", barcodeItem.SFC);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES16120);
                    validationFailures.Add(validationFailure);
                    continue;

                }


                scrapPartialWhMaterialInventoryEmptyByIdCommandList.Add(new ScrapPartialWhMaterialInventoryByIdCommand
                {
                    Id = whMaterialInventoryEntity.Id,
                    ScrapQty = (whMaterialInventoryEntity.ScrapQty ?? 0) + barcodeItem.Qty,
                    Qty = whMaterialInventoryEntity.QuantityResidue - barcodeItem.Qty,
                    CurrentQuantityResidue = whMaterialInventoryEntity.QuantityResidue,
                    UpdatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName
                });
                
                var materialEntity = materialList.FirstOrDefault(x => x.Id == whMaterialInventoryEntity.MaterialId);

                // 新增 wh_material_standingbook
                WhMaterialStandingbookEntity whMaterialStandingbookEntity = new WhMaterialStandingbookEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    MaterialCode = materialEntity.MaterialCode ?? "",
                    MaterialName = materialEntity.MaterialName,
                    MaterialVersion = materialEntity.Version,
                    MaterialBarCode = barcodeItem.SFC ?? "",
                    Batch = materialEntity.Batch,
                    Quantity = barcodeItem.Qty,
                    //ScrapQty=item.ScrapQuantity,
                    Unit = materialEntity.Unit ?? "",
                    Type = WhMaterialInventoryTypeEnum.MaterialScrapping,
                    Source = MaterialInventorySourceEnum.ManualEntry,
                    SiteId = _currentSite.SiteId ?? 123456,
                    CreatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = DateTime.UtcNow,
                };

                whMaterialStandingbookEntities.Add(whMaterialStandingbookEntity);
                //记录报废信息
                // 新增 wh_material_inventory_scrap
                WhMaterialInventoryScrapEntity whMaterialInventoryScrap = new WhMaterialInventoryScrapEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SupplierId = whMaterialInventoryEntity.SupplierId,
                    MaterialId = materialEntity.Id,
                    MaterialBarCode = whMaterialInventoryEntity.MaterialBarCode ?? "",
                    Batch = whMaterialInventoryEntity.Batch,
                    ScrapQty = barcodeItem.Qty,
                    MaterialStandingbookId = whMaterialStandingbookEntity.Id,
                    ScrapType = barcodeItem.ScrapType,
                    IsCancellation = TrueOrFalseEnum.No,
                    ProcedureId = barcodeItem.ProcedureId,
                    SiteId = _currentSite.SiteId ?? 123456,
                    WorkOrderId = barcodeItem.WorkOrderId,// whMaterialInventoryEntity.WorkOrderId??0,
                    Remark = barcodeItem.Remark ?? "",
                    CreatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = DateTime.UtcNow,
                    UnqualifiedId = barcodeItem.UnqualifiedId
                };
                manuSfcScrapEntities.Add(whMaterialInventoryScrap);
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }

            //入库
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //1、修改条码表可用数量、报废数量
                if (manuSFCPartialScrapByIdCommandList != null && manuSFCPartialScrapByIdCommandList.Any())
                {
                    var row = await _manuSfcRepository.PartialScrapmanuSFCByIdAsync(manuSFCPartialScrapByIdCommandList);
                    if (row != manuSFCPartialScrapByIdCommandList.Count())
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES15449));
                    }
                }

                //4.插入台账记录
                if (whMaterialStandingbookEntities != null && whMaterialStandingbookEntities.Any())
                {
                    await _whMaterialStandingbookRepository.InsertsAsync(whMaterialStandingbookEntities);
                }

                //5.修改库存数据
                if (scrapPartialWhMaterialInventoryEmptyByIdCommandList != null && scrapPartialWhMaterialInventoryEmptyByIdCommandList.Any())
                {
                    var row = await _whMaterialInventoryRepository.ScrapPartialWhMaterialInventoryByIdAsync(scrapPartialWhMaterialInventoryEmptyByIdCommandList);
                    if (row != scrapPartialWhMaterialInventoryEmptyByIdCommandList.Count())
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES15449));
                    }
                }

                //6.插入报废表
                if (manuSfcScrapEntities != null && manuSfcScrapEntities.Any())
                {
                    await _whMaterialInventoryScrapRepository.InsertAsync(manuSfcScrapEntities);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 扫码校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<MaterialScrapBarCodeDto> BarcodeScanningAsync(MaterialScrapScanningDto param)
        {
            //条码表
            var manuSfcEntity = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
            {
                SFC = param.BarCode,
                SiteId = _currentSite.SiteId ?? 0,
                Type = SfcTypeEnum.NoProduce //原材料条码
            });

            if (manuSfcEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15415));
            }

            if (ManuSfcStatus.ForbidScrapSfcStatuss.Contains(manuSfcEntity.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15450)).WithData("Status", _localizationService.GetResource($"Hymson.MES.Core.Enums.manu.SfcStatusEnum.{SfcStatusEnum.GetName(manuSfcEntity.Status)}"));
            }

            var manuSfcInfoEntity = await _manuSfcInfoRepository.GetBySFCIdAsync(manuSfcEntity.Id);
           // var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(manuSfcInfoEntity.WorkOrderId ?? 0);
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(manuSfcInfoEntity.ProductId);
            MaterialScrapBarCodeDto partialScrapBarCodeDto = new MaterialScrapBarCodeDto()
            {
                BarCode = manuSfcEntity.SFC,
                // WorkOrderCode = planWorkOrderEntity.OrderCode,
                materialCode = procMaterialEntity.MaterialCode,
                materialName = procMaterialEntity.MaterialName,
                Qty = manuSfcEntity.Qty
            };
            return partialScrapBarCodeDto;
        }
    }
}

