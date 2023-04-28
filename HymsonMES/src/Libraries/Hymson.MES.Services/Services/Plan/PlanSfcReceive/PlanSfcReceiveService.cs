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
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
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
using System.Security.Policy;
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
        private readonly AbstractValidator<PlanSfcReceiveCreateDto> _validationCreateRules;
        private readonly AbstractValidator<PlanSfcReceiveModifyDto> _validationModifyRules;

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
        AbstractValidator<PlanSfcReceiveCreateDto> validationCreateRules, AbstractValidator<PlanSfcReceiveModifyDto> validationModifyRules)
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
            var planWorkOrderEntity = await _manuCommonService.GetProduceWorkOrderByIdAsync(param.WorkOrderId);
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

            foreach (var sfc in param.SFCs)
            {
                var validationFailure = new ValidationFailure();
                decimal qty = 0;
                if (param.ReceiveType == PlanSFCReceiveTypeEnum.MaterialSfc)
                {
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
                    var whMaterialInventoryEntity = whMaterialInventoryList.FirstOrDefault(x => x.MaterialBarCode == sfc);
                    if (whMaterialInventoryEntity == null || whMaterialInventoryEntity.QuantityResidue == 0)
                    {
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16120);
                        validationFailures.Add(validationFailure);
                    }
                    else
                    {
                        qty = whMaterialInventoryEntity.QuantityResidue;
                    }
                }
                else
                {

                    if (!await _manuCommonService.CheckBarCodeByMaskCodeRule(sfc, procMaterialEntity.Id))
                    {
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
                        validationFailure.FormattedMessagePlaceholderValues.Add("Product", procMaterialEntity.MaterialCode);
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16121);
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
            var planWorkOrderEntity = await _manuCommonService.GetProduceWorkOrderByIdAsync(param.WorkOrderId);
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(planWorkOrderEntity.ProductId);
            if (param.ReceiveType == PlanSFCReceiveTypeEnum.SupplierSfc && procMaterialEntity.Batch == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16502)).WithData("product", procMaterialEntity.MaterialCode);
            }
            decimal qty = 0;
            if (param.ReceiveType == PlanSFCReceiveTypeEnum.MaterialSfc)
            {
                var whMaterialInventoryEntity = await _whMaterialInventoryRepository.GetByBarCode1Async(param.SFC);
                if (whMaterialInventoryEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16120));
                }
                qty = whMaterialInventoryEntity.QuantityResidue;
                procMaterialEntity = await _procMaterialRepository.GetByIdAsync(whMaterialInventoryEntity.MaterialId);
            }
            else
            {
                if (!await _manuCommonService.CheckBarCodeByMaskCodeRule(param.SFC, procMaterialEntity.Id))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16121)).WithData("Product", procMaterialEntity.MaterialCode); ;
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
