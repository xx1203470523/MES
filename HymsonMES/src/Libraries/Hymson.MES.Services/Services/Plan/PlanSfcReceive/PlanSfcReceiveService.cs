/*
 *creator: Karl
 *
 *describe: 条码接收    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */
using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
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
using System.Transactions;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 条码接收 服务
    /// </summary>
    public class PlanSfcReceiveService : IPlanSfcReceiveService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        /// <summary>
        /// 条码接收 仓储
        /// </summary>
        private readonly IPlanSfcReceiveRepository _planSfcInfoRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        private readonly IInteCodeRulesMakeRepository _inteCodeRulesMakeRepository;
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IManuCreateBarcodeService _manuCreateBarcodeService;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IManuCommonService _manuCommonService;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly AbstractValidator<PlanSfcReceiveCreateDto> _validationCreateRules;
        private readonly AbstractValidator<PlanSfcReceiveScanCodeDto> _validationModifyRules;

        public PlanSfcReceiveService(ICurrentUser currentUser, ICurrentSite currentSite,
            IPlanSfcReceiveRepository planSfcInfoRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuSfcRepository manuSfcRepository,
            IInteCodeRulesRepository inteCodeRulesRepository,
            IInteCodeRulesMakeRepository inteCodeRulesMakeRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            ILocalizationService localizationService,
            IManuCreateBarcodeService manuCreateBarcodeService,
            IProcMaterialRepository procMaterialRepository,
            IManuCommonService manuCommonService,
            IManuSfcInfoRepository manuSfcInfoRepository,
        AbstractValidator<PlanSfcReceiveCreateDto> validationCreateRules, AbstractValidator<PlanSfcReceiveScanCodeDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _planSfcInfoRepository = planSfcInfoRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcRepository = manuSfcRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _localizationService = localizationService;
            _manuCreateBarcodeService = manuCreateBarcodeService;
            _manuCreateBarcodeService = manuCreateBarcodeService;
            _procMaterialRepository = procMaterialRepository;
            _manuCommonService = manuCommonService;
            _manuSfcInfoRepository = manuSfcInfoRepository;
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
                    if (!manuSfcInfoList.Any(x => x.SfcId == manuSfcEntity?.Id && x.WorkOrderId == param.WorkOrderId))
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
                await _manuCreateBarcodeService.CreateBarcodeByOldMESSFC(new CreateBarcodeByOldMesSFCDto
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
                await _manuCreateBarcodeService.CreateBarcodeByExternalSFC(new CreateBarcodeByExternalSFCDto
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
            var manuSfcEntity = await _manuSfcRepository.GetBySFCAsync(param.SFC);

            decimal qty = 0;
            if (param.ReceiveType == PlanSFCReceiveTypeEnum.MaterialSfc)
            {
                if (manuSfcEntity != null && manuSfcEntity.Status == SfcStatusEnum.InProcess)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16123)).WithData("sfc", param.SFC);
                }
                var manuSfcInfoEntity = await _manuSfcInfoRepository.GetBySFCAsync(param.SFC);

                if (manuSfcInfoEntity != null && manuSfcInfoEntity.WorkOrderId != param.WorkOrderId)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16127));
                }
                var whMaterialInventoryEntity = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery { SiteId = _currentSite.SiteId, BarCode = param.SFC });
                if (whMaterialInventoryEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16120));
                }
                qty = whMaterialInventoryEntity.QuantityResidue;
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
