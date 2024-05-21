using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuSFCScrap;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Http;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Manufacture.ManuSfcScrapservice
{
    /// <summary>
    /// 部分报废实现
    /// </summary>
    public class ManuSFCPartialScrapService : IManuSFCPartialScrapService
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

        /// <summary>
        /// 工序
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 不合格
        /// </summary>
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;

        /// <summary>
        /// 服务接口（Excel）
        /// </summary>
        private readonly IExcelService _excelService;

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
        /// <param name="procProcedureRepository"></param>
        /// <param name="qualUnqualifiedCodeRepository"></param>
        /// <param name="excelService"></param>
        public ManuSFCPartialScrapService(ICurrentUser currentUser, ICurrentSite currentSite, IManuSfcRepository manuSfcRepository, IManuSfcInfoRepository manuSfcInfoRepository, IManuSfcProduceRepository manuSfcProduceRepository, ILocalizationService localizationService, IWhMaterialInventoryRepository whMaterialInventoryRepository, IManuSfcStepRepository manuSfcStepRepository, IManuSfcScrapRepository manuSfcScrapRepository, IPlanWorkOrderRepository planWorkOrderRepository, IProcMaterialRepository procMaterialRepository, IProcProcedureRepository procProcedureRepository, IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository, IExcelService excelService)
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
            _procProcedureRepository = procProcedureRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _excelService = excelService;
        }

        #region 报废
        /// <summary>
        /// 报废
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task PartialScrapAsync(ManuSFCPartialScrapDto param)
        {
            //条码表
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SFCs = param.BarcodeScrapList.Select(x => x.SFC),
                SiteId = _currentSite.SiteId ?? 0,
                Type = SfcTypeEnum.Produce
            });
            //条码信息表
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcEntities.Select(x => x.Id));
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = param.BarcodeScrapList.Select(x => x.SFC), SiteId = _currentSite.SiteId ?? 00 };
            //在制品信息
            var manuSfcProduces = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);

            //修改库存信息
            var whMaterialInventoryList = await _whMaterialInventoryRepository.GetByBarCodesAsync(new Data.Repositories.Warehouse.WhMaterialInventory.Query.WhMaterialInventoryBarCodesQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                BarCodes = param.BarcodeScrapList.Select(x => x.SFC)
            });

            var validationFailures = new List<ValidationFailure>();
            List<ManuSfcScrapEntity> manuSfcScrapEntities = new();
            List<ManuSFCPartialScrapByIdCommand> manuSFCPartialScrapByIdCommandList = new();
            List<ManuSfcProducePartialScrapByIdCommand> manuSfcProducePartialScrapByIdCommandList = new();
            List<ManuSfcStepEntity> manuSfcStepEntities = new();
            List<ScrapPartialWhMaterialInventoryByIdCommand> scrapPartialWhMaterialInventoryEmptyByIdCommandList = new();
            List<long> deleteSFCProduceIds = new();
            var group = param.BarcodeScrapList.GroupBy(x => x.SFC);
            foreach (var groupItem in group)
            {
                var sfc = groupItem.Key;
                var ScrapQty = groupItem.Sum(x => x.ScrapQty);
                var sfcEntity = sfcEntities.FirstOrDefault(x => x.SFC == sfc);

                if (sfcEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15415);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                if (sfcEntity.Qty < ScrapQty)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex",sfc}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15448);
                    validationFailure.FormattedMessagePlaceholderValues.Add("Qty", sfcEntity.Qty);
                    validationFailure.FormattedMessagePlaceholderValues.Add("ScrapQty", ScrapQty);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                if (ManuSfcStatus.ForbidScrapSfcStatuss.Contains(sfcEntity.Status))
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }
                    validationFailure.FormattedMessagePlaceholderValues.Add("Status", _localizationService.GetResource($"Hymson.MES.Core.Enums.manu.SfcStatusEnum.{SfcStatusEnum.GetName(sfcEntity.Status)}"));
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15450);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                if (sfcEntity.Status == SfcStatusEnum.Locked)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15416);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                var manuSFCPartialScrapByIdCommand = new ManuSFCPartialScrapByIdCommand
                {
                    Id = sfcEntity.Id,
                    ScrapQty = (sfcEntity.ScrapQty ?? 0) + ScrapQty,
                    Qty = sfcEntity.Qty - ScrapQty,
                    UpdatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                    CurrentQty = sfcEntity.Qty,
                    CurrentStatus = sfcEntity.Status,
                };
                if (sfcEntity.Qty == ScrapQty)
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
                    throw new CustomerValidationException(nameof(ErrorCode.MES15401));
                }
                ManuSfcProduceEntity? manuSfcProduceInfoEntity = new();
                if (sfcEntity.Status == SfcStatusEnum.Complete)
                {
                    var whMaterialInventoryEntity = whMaterialInventoryList.FirstOrDefault(x => x.MaterialBarCode == sfc);
                    if (whMaterialInventoryEntity != null)
                    {
                        scrapPartialWhMaterialInventoryEmptyByIdCommandList.Add(new ScrapPartialWhMaterialInventoryByIdCommand
                        {
                            Id = whMaterialInventoryEntity.Id,
                            ScrapQty = (whMaterialInventoryEntity.ScrapQty ?? 0) + ScrapQty,
                            Qty = whMaterialInventoryEntity.QuantityResidue - ScrapQty,
                            CurrentQuantityResidue = whMaterialInventoryEntity.QuantityResidue,
                            UpdatedOn = HymsonClock.Now(),
                            UpdatedBy = _currentUser.UserName
                        });
                    }
                }
                else
                {
                    manuSfcProduceInfoEntity = manuSfcProduces.FirstOrDefault(x => x.SFC == sfc);

                    if (manuSfcProduceInfoEntity != null)
                    {
                        if (manuSfcProduceInfoEntity.Qty == ScrapQty)
                        {
                            // 部分不做取消报废 故物理删除在制品
                            deleteSFCProduceIds.Add(manuSfcProduceInfoEntity.Id);
                        }
                        else
                        {
                            manuSfcProducePartialScrapByIdCommandList.Add(new ManuSfcProducePartialScrapByIdCommand
                            {
                                Id = manuSfcProduceInfoEntity.Id,
                                ScrapQty = (manuSfcProduceInfoEntity.ScrapQty ?? 0) + ScrapQty,
                                Qty = manuSfcProduceInfoEntity.Qty - ScrapQty,
                                UpdatedOn = HymsonClock.Now(),
                                UpdatedBy = _currentUser.UserName
                            });
                        }
                    }
                }
                foreach (var item in groupItem)
                {
                    //记录步骤表
                    var stepEntity = new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SFC = sfc,
                        ProductId = sfcInfoEntity.ProductId,
                        WorkOrderId = sfcInfoEntity.WorkOrderId ?? 0,
                        WorkCenterId = manuSfcProduceInfoEntity?.WorkCenterId,
                        ProductBOMId = manuSfcProduceInfoEntity?.ProductBOMId,
                        ProcessRouteId = manuSfcProduceInfoEntity?.ProcessRouteId,
                        Qty = sfcEntity.Qty,
                        ScrapQty = sfcEntity.ScrapQty,
                        EquipmentId = manuSfcProduceInfoEntity?.EquipmentId,
                        ResourceId = manuSfcProduceInfoEntity?.ResourceId,
                        ProcedureId = manuSfcProduceInfoEntity?.ProcedureId,
                        Operatetype = ManuSfcStepTypeEnum.PartialDiscard,
                        CurrentStatus = sfcEntity.Status,
                        Remark = item.Remark ?? "",
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedOn = HymsonClock.Now(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentUser.UserName
                    };
                    manuSfcStepEntities.Add(stepEntity);

                    //记录报废信息
                    var manuSfcScrapEntity = new ManuSfcScrapEntity()
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SFC = sfc,
                        SfcinfoId = sfcInfoEntity?.Id ?? 0,
                        SfcStepId = stepEntity.Id,
                        ProcedureId = item.FindProcedureId,
                        UnqualifiedId = item.UnqualifiedId,
                        OutFlowProcedureId = item.OutFlowProcedureId,
                        ScrapQty = item.ScrapQty,
                        IsCancel = false,
                        Remark = item.Remark,
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedOn = HymsonClock.Now(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentUser.UserName
                    };
                    manuSfcScrapEntities.Add(manuSfcScrapEntity);
                }
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

                //2、修改在制品表可用数量、报废数量
                if (manuSfcProducePartialScrapByIdCommandList != null && manuSfcProducePartialScrapByIdCommandList.Any())
                {
                    await _manuSfcProduceRepository.PartialScrapManuSfcProduceByIdAsync(manuSfcProducePartialScrapByIdCommandList);
                }

                //3、删除全报废的在制品数据
                if (deleteSFCProduceIds != null && deleteSFCProduceIds.Any())
                {
                    var physicalDeleteSFCProduceByIdsCommand = new PhysicalDeleteSFCProduceByIdsCommand
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        Ids = deleteSFCProduceIds
                    };
                    await _manuSfcProduceRepository.DeletePhysicalRangeByIdsAsync(physicalDeleteSFCProduceByIdsCommand);
                }

                //4.插入数据操作类型为报废
                if (manuSfcStepEntities != null && manuSfcStepEntities.Any())
                {
                    await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepEntities);
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
                    await _manuSfcScrapRepository.InsertRangeAsync(manuSfcScrapEntities);
                }

                trans.Complete();
            }
        }
        #endregion

        #region 下载导入模板
        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<string> DownloadImportTemplateAsync(Stream stream)
        {
            var worksheetName = "报废";
            await _excelService.ExportAsync(Array.Empty<ManuSFCPartialScrapExcelDto>(), stream, worksheetName);
            return worksheetName;
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        public async Task ImportAsync(IFormFile formFile)
        {
            using MemoryStream memoryStream = new();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var dtos = _excelService.Import<ManuSFCPartialScrapExcelDto>(memoryStream);
            if (dtos == null || !dtos.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10133));
            var outFlowProcedureCodes = dtos.Select(x => x.OutFlowProcedureCode).Distinct().ToList();
            var findProcedureCodes = dtos.Select(x => x.FindProcedureCode).Distinct().ToList();
            var codes = new List<string>();
            codes.AddRange(outFlowProcedureCodes);
            codes.AddRange(findProcedureCodes);
            var procedureEntities = await _procProcedureRepository.GetProcProcedureEntitiesAsync(new ProcProcedureQuery
            {
                Codes = codes,
                SiteId = _currentSite.SiteId ?? 0
            });

            var unqualifiedEntities = await _qualUnqualifiedCodeRepository.GetByCodesAsync(new QualUnqualifiedCodeByCodesQuery
            {
                Codes = dtos.Select(x => x.UnqualifiedCode).Distinct(),
                SiteId = _currentSite.SiteId ?? 0
            });

            var barcodeScrapList = new List<BarcodeScrap>();
            var validationFailures = new List<ValidationFailure>();
            var index = 0;
            foreach (var item in dtos)
            {
                index++;
                var findProcedureEntity = procedureEntities.FirstOrDefault(x => x.Code == item.FindProcedureCode);
                if (findProcedureEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", index}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", index);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15460);
                    validationFailures.Add(validationFailure);
                }
                var outFlowProcedureEntity = procedureEntities.FirstOrDefault(x => x.Code == item.OutFlowProcedureCode);
                if (outFlowProcedureEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", index}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", index);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15461);
                    validationFailures.Add(validationFailure);
                }
                var unqualifiedEntity = unqualifiedEntities.FirstOrDefault(x => x.UnqualifiedCode == item.UnqualifiedCode);
                if (unqualifiedEntity==null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", index}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", index);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15462);
                    validationFailures.Add(validationFailure);
                }
                if (findProcedureEntity == null || outFlowProcedureEntity == null || unqualifiedEntity == null)
                {
                    continue;
                }
                barcodeScrapList.Add(new BarcodeScrap
                {
                    Remark = item.Remark,
                    FindProcedureId = findProcedureEntity.Id,
                    OutFlowProcedureId = outFlowProcedureEntity.Id,
                    UnqualifiedId = unqualifiedEntity.Id,
                    SFC = item.BarCode,
                    ScrapQty = item.ScrapQty
                });
            }
            if (validationFailures!=null&&validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("ExcelRowError"), validationFailures);
            }

            await PartialScrapAsync(new ManuSFCPartialScrapDto()
            {
                BarcodeScrapList = barcodeScrapList
            });
        }
        #endregion

        #region 扫码校验
        /// <summary>
        /// 扫码校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<PartialScrapBarCodeDto> BarcodeScanningAsync(PartialScrapScanningDto param)
        {
            //条码表
            var manuSfcEntity = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
            {
                SFC = param.BarCode,
                SiteId = _currentSite.SiteId ?? 0,
                Type = SfcTypeEnum.Produce
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
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(manuSfcInfoEntity.WorkOrderId ?? 0);
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(manuSfcInfoEntity.ProductId);
            PartialScrapBarCodeDto partialScrapBarCodeDto = new PartialScrapBarCodeDto()
            {
                BarCode = manuSfcEntity.SFC,
                WorkOrderCode = planWorkOrderEntity.OrderCode,
                ProductCode = procMaterialEntity.MaterialCode,
                ProductName = procMaterialEntity.MaterialName,
                Qty = manuSfcEntity.Qty
            };
            return partialScrapBarCodeDto;
        }
        #endregion
    }
}

