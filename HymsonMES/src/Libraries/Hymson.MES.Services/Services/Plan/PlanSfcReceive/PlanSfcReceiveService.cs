using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCommonDto;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCreateBarcodeDto;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCreateBarcode;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Collections.Generic;
using System.Transactions;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 条码接收 服务
    /// </summary>
    public class PlanSfcReceiveService : IPlanSfcReceiveService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 条码接收 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IManuCreateBarcodeService _manuCreateBarcodeService;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IManuCommonService _manuCommonService;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuContainerPackRepository _manuContainerPackRepository;
        private readonly AbstractValidator<PlanSfcReceiveCreateDto> _validationCreateRules;
        private readonly AbstractValidator<PlanSfcReceiveScanCodeDto> _validationModifyRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="localizationService"></param>
        /// <param name="manuCreateBarcodeService"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuContainerPackRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        public PlanSfcReceiveService(ICurrentUser currentUser, ICurrentSite currentSite,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuSfcRepository manuSfcRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            ILocalizationService localizationService,
            IManuCreateBarcodeService manuCreateBarcodeService,
            IProcMaterialRepository procMaterialRepository,
            IManuCommonService manuCommonService,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuContainerPackRepository manuContainerPackRepository,
        AbstractValidator<PlanSfcReceiveCreateDto> validationCreateRules,
        AbstractValidator<PlanSfcReceiveScanCodeDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcRepository = manuSfcRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _localizationService = localizationService;
            _manuCreateBarcodeService = manuCreateBarcodeService;
            _manuCreateBarcodeService = manuCreateBarcodeService;
            _procMaterialRepository = procMaterialRepository;
            _manuCommonService = manuCommonService;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuContainerPackRepository = manuContainerPackRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 条码接收
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task CreatePlanSfcInfoAsync(PlanSfcReceiveCreateDto param)
        {
            //#region 验证与数据组装
            await _validationCreateRules.ValidateAndThrowAsync(param);
            var planWorkOrderEntity = await _manuCommonService.GetWorkOrderByIdAsync(param.WorkOrderId);
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(planWorkOrderEntity.ProductId);
            if (param.ReceiveType == PlanSFCReceiveTypeEnum.SupplierSfc && procMaterialEntity.Batch == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16502)).WithData("product", procMaterialEntity.MaterialCode);
            }

            var validationFailures = new List<ValidationFailure>();
            var barcodeList = new List<BarcodeDto>();
            IEnumerable<ManuContainerPackEntity> ManuContainerPackList = new List<ManuContainerPackEntity>();
            if (param.ReceiveType == PlanSFCReceiveTypeEnum.MaterialSfc)
            {
                ManuContainerPackList = await _manuContainerPackRepository.GetByLadeBarCodesAsync(new ManuContainerPackQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    LadeBarCodes = param.SFCs,
                });
            }
            var whMaterialInventoryList = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
            {
                BarCodes = param.SFCs,
                SiteId = _currentSite.SiteId ?? 0
            });
            var manuSfcList = await _manuSfcRepository.GetBySFCsAsync(param.SFCs);
            //TODO  考虑库存中是否存放工单字段 王克明
            var manuSfcInfoList = await _manuSfcInfoRepository.GetBySFCIdsAsync(manuSfcList.Select(x => x.Id).ToList());

            foreach (var sfc in param.SFCs)
            {
                decimal qty = 0;
                var manuSfcEntity = manuSfcList.FirstOrDefault(x => x.SFC == sfc);
                if (param.ReceiveType == PlanSFCReceiveTypeEnum.MaterialSfc)
                {
                    if (!manuSfcInfoList.Any(x => x.SfcId == manuSfcEntity?.Id && x.WorkOrderId == param.RelevanceWorkOrderId))
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
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16127);
                        validationFailures.Add(validationFailure);
                    }
                    var whMaterialInventoryEntity = whMaterialInventoryList.FirstOrDefault(x => x.MaterialBarCode == sfc);
                    if (whMaterialInventoryEntity == null || whMaterialInventoryEntity.QuantityResidue == 0)
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
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16120);
                        validationFailures.Add(validationFailure);
                    }
                    else
                    {
                        if (whMaterialInventoryEntity.MaterialId != procMaterialEntity.Id)
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
                            validationFailure.ErrorCode = nameof(ErrorCode.MES16129);
                            validationFailures.Add(validationFailure);
                        }

                        if (whMaterialInventoryEntity.QuantityResidue <= 0)
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
                            validationFailure.ErrorCode = nameof(ErrorCode.MES18021);
                            validationFailures.Add(validationFailure);
                        }
                        qty = whMaterialInventoryEntity.QuantityResidue;
                    }
                    if (manuSfcEntity != null && manuSfcEntity.Status == SfcStatusEnum.InProcess)
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
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16124);
                        validationFailures.Add(validationFailure);
                    }

                    if (ManuContainerPackList.Any(x => x.LadeBarCode == sfc))
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
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16133);
                        validationFailures.Add(validationFailure);
                    }
                }
                else
                {
                    if (!await _manuCommonService.CheckBarCodeByMaskCodeRuleAsync(sfc, procMaterialEntity.Id))
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
                        validationFailure.FormattedMessagePlaceholderValues.Add("product", procMaterialEntity.MaterialCode);
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16121);
                        validationFailures.Add(validationFailure);
                    }

                    if (manuSfcEntity != null)
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
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16125);
                        validationFailures.Add(validationFailure);
                    }

                    qty = procMaterialEntity.Batch;
                    if (qty <= 0)
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
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16131);
                        validationFailures.Add(validationFailure);
                    }
                }
                barcodeList.Add(new BarcodeDto
                {
                    SFC = sfc,
                    Qty = qty
                });
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }

            using var ts = TransactionHelper.GetTransactionScope(TransactionScopeOption.Suppress);
            if (param.ReceiveType == PlanSFCReceiveTypeEnum.MaterialSfc)
            {
                await _manuCreateBarcodeService.CreateBarcodeByOldMESSFCAsync(new CreateBarcodeByOldMesSFCDto
                {
                    WorkOrderId = param.WorkOrderId,
                    OldSFCs = barcodeList
                });
                await _whMaterialInventoryRepository.UpdateWhMaterialInventoryEmptyByBarCodeAync(new UpdateWhMaterialInventoryEmptyCommand
                {
                    BarCodeList = whMaterialInventoryList.Select(x => x.MaterialBarCode).ToList(),
                    SiteId = _currentSite.SiteId ?? 0,
                    UserName = _currentUser.UserName,
                    UpdateTime = HymsonClock.Now()
                });
            }
            else
            {
                await _manuCreateBarcodeService.CreateBarcodeByExternalSFCAsync(new CreateBarcodeByExternalSFCDto
                {
                    WorkOrderId = param.WorkOrderId,
                    ExternalSFCs = barcodeList
                });
            }
            ts.Complete();
        }

        /// <summary>
        /// 条码接收扫码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PlanSfcReceiveSFCDto> PlanSfcReceiveScanCodeAsync(PlanSfcReceiveScanCodeDto param)
        {
            await _validationModifyRules.ValidateAndThrowAsync(param);
            var planWorkOrderEntity = await _manuCommonService.GetWorkOrderByIdAsync(param.WorkOrderId);
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(planWorkOrderEntity.ProductId);
            if (param.ReceiveType == PlanSFCReceiveTypeEnum.SupplierSfc && procMaterialEntity.Batch == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16502)).WithData("product", procMaterialEntity.MaterialCode);
            }
            var manuSfcEntity = await _manuSfcRepository.GetBySFCAsync(new GetBySFCQuery { SFC = param.SFC, SiteId = _currentSite.SiteId });

            decimal qty = 0;
            if (param.ReceiveType == PlanSFCReceiveTypeEnum.MaterialSfc)
            {
                if (manuSfcEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16128)).WithData("sfc", param.SFC);

                }
                if (manuSfcEntity.Status == SfcStatusEnum.InProcess)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16123)).WithData("sfc", param.SFC);
                }
                var manuSfcInfoEntity = await _manuSfcInfoRepository.GetBySFCAsync(manuSfcEntity.Id);

                if (manuSfcInfoEntity != null && manuSfcInfoEntity.WorkOrderId != param.RelevanceWorkOrderId)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16127));
                }

                var manuContainerPackEntity = await _manuContainerPackRepository.GetByLadeBarCodeAsync(new ManuContainerPackQuery {LadeBarCode= param.SFC ,SiteId=_currentSite.SiteId??0 });

                if (manuContainerPackEntity != null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16132)).WithData("sfc", param.SFC);
                }

                var whMaterialInventoryEntity = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery { SiteId = _currentSite.SiteId, BarCode = param.SFC });
                if (whMaterialInventoryEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16120));
                }
                if (procMaterialEntity.Id != whMaterialInventoryEntity.MaterialId)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16129));
                }
                qty = whMaterialInventoryEntity.QuantityResidue;
                if (qty <= 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18021));
                }
                procMaterialEntity = await _procMaterialRepository.GetByIdAsync(whMaterialInventoryEntity.MaterialId);
            }
            else
            {
                if (!await _manuCommonService.CheckBarCodeByMaskCodeRuleAsync(param.SFC, procMaterialEntity.Id))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16121)).WithData("product", procMaterialEntity.MaterialCode);
                }
                if (manuSfcEntity != null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16122)).WithData("sfc", param.SFC);
                }
                qty = procMaterialEntity.Batch;
                if (qty <= 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16610)).WithData("sfc", param.SFC);
                }
            }
            var relevanceOrderCode = string.Empty;
            if (param.RelevanceWorkOrderId.HasValue)
            {
                var relevancePlanWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(param.RelevanceWorkOrderId ?? 0);
                if (relevancePlanWorkOrderEntity != null)
                {
                    relevanceOrderCode = relevancePlanWorkOrderEntity.OrderCode;
                }
            }
            return new PlanSfcReceiveSFCDto()
            {
                OrderCode = planWorkOrderEntity.OrderCode,
                Type = planWorkOrderEntity.Type,
                OrderCodeQty = planWorkOrderEntity.Qty,
                BarCode = param.SFC,
                MaterialCode = procMaterialEntity.MaterialCode,
                MaterialName = procMaterialEntity.MaterialName,
                MaterialVersion = procMaterialEntity.Version ?? "",
                Qty = qty,
                RelevanceOrderCode = relevanceOrderCode
            };
        }

    }
}
