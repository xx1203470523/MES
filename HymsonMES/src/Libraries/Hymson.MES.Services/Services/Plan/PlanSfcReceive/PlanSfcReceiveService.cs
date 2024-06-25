using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Dtos.Manufacture;
using Hymson.MES.CoreServices.Services.Manufacture.ManuCreateBarcode;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
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
        private readonly IManuCommonOldService _manuCommonOldService;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuContainerPackRepository _manuContainerPackRepository;
        private readonly AbstractValidator<PlanSfcReceiveCreateDto> _validationCreateRules;
        private readonly AbstractValidator<PlanSfcReceiveScanCodeDto> _validationModifyRules;
        private readonly AbstractValidator<PlanSfcReceiveScanListDto> _validationScanListRules;

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
        /// <param name="manuCommonOldService"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuContainerPackRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="validationScanListRules"></param>
        public PlanSfcReceiveService(ICurrentUser currentUser, ICurrentSite currentSite,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuSfcRepository manuSfcRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            ILocalizationService localizationService,
            IManuCreateBarcodeService manuCreateBarcodeService,
            IProcMaterialRepository procMaterialRepository,
            IManuCommonOldService manuCommonOldService,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuContainerPackRepository manuContainerPackRepository,
            AbstractValidator<PlanSfcReceiveCreateDto> validationCreateRules,
            AbstractValidator<PlanSfcReceiveScanCodeDto> validationModifyRules,
            AbstractValidator<PlanSfcReceiveScanListDto> validationScanListRules)
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
            _manuCommonOldService = manuCommonOldService;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuContainerPackRepository = manuContainerPackRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _validationScanListRules = validationScanListRules;
        }

        /// <summary>
        /// 条码接收
        /// </summary>
        /// <param name="planSfcInfoCreateDto"></param>
        /// <returns></returns>
        public async Task CreatePlanSfcInfoAsync(PlanSfcReceiveCreateDto planSfcInfoCreateDto)
        {
            //#region 验证与数据组装
            await _validationCreateRules.ValidateAndThrowAsync(planSfcInfoCreateDto);
            var planWorkOrderEntity = await _manuCommonOldService.GetWorkOrderByIdAsync(planSfcInfoCreateDto.WorkOrderId);
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(planWorkOrderEntity.ProductId);
            if (planSfcInfoCreateDto.ReceiveType == PlanSFCReceiveTypeEnum.SupplierSfc && procMaterialEntity.Batch == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16502)).WithData("product", procMaterialEntity.MaterialCode);
            }

            var validationFailures = new List<ValidationFailure>();
            var barcodeList = new List<BarcodeDto>();
            IEnumerable<ManuContainerPackEntity> ManuContainerPackList = new List<ManuContainerPackEntity>();
            if (planSfcInfoCreateDto.ReceiveType == PlanSFCReceiveTypeEnum.MaterialSfc)
            {
                ManuContainerPackList = await _manuContainerPackRepository.GetByLadeBarCodesAsync(new ManuContainerPackQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    LadeBarCodes = planSfcInfoCreateDto.SFCs,
                });
            }
            var whMaterialInventoryList = await _whMaterialInventoryRepository.GetByBarCodesOfHasQtyAsync(new WhMaterialInventoryBarCodesQuery
            {
                BarCodes = planSfcInfoCreateDto.SFCs,
                SiteId = _currentSite.SiteId ?? 0
            });
            var manuSfcList = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = _currentSite.SiteId,
                SFCs = planSfcInfoCreateDto.SFCs,
                Type = SfcTypeEnum.Produce
            });
            //TODO  考虑库存中是否存放工单字段 王克明
            IEnumerable<ManuSfcInfoEntity> manuSfcInfoList = new List<ManuSfcInfoEntity>();
            var sfcids = manuSfcList.Select(x => x.Id).ToList();
            if (sfcids != null && sfcids.Any())
            {
                manuSfcInfoList = await _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(sfcids);
            }

            foreach (var sfc in planSfcInfoCreateDto.SFCs)
            {
                decimal qty = 0;
                var manuSfcEntity = manuSfcList.FirstOrDefault(x => x.SFC == sfc);
                if (planSfcInfoCreateDto.ReceiveType == PlanSFCReceiveTypeEnum.MaterialSfc)
                {
                    //if (!manuSfcInfoList.Any(x => x.SfcId == manuSfcEntity?.Id && x.WorkOrderId == planSfcInfoCreateDto.RelevanceWorkOrderId))
                    //{
                    //    var validationFailure = new ValidationFailure();
                    //    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    //    {
                    //        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                    //        { "CollectionIndex", sfc}
                    //    };
                    //    }
                    //    else
                    //    {
                    //        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    //    }
                    //    validationFailure.ErrorCode = nameof(ErrorCode.MES16127);
                    //    validationFailures.Add(validationFailure);
                    //}
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
                        //if (whMaterialInventoryEntity.MaterialId != procMaterialEntity.Id)
                        //{
                        //    var validationFailure = new ValidationFailure();
                        //    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        //    {
                        //        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                        //    { "CollectionIndex", sfc}
                        //           };
                        //    }
                        //    else
                        //    {
                        //        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                        //    }
                        //    validationFailure.ErrorCode = nameof(ErrorCode.MES16129);
                        //    validationFailures.Add(validationFailure);
                        //}

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
                    if (manuSfcEntity != null && ManuSfcStatus.SfcStatusInProcess.Contains(manuSfcEntity.Status))
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
                    if (!await _manuCommonOldService.CheckBarCodeByMaskCodeRuleAsync(sfc, procMaterialEntity.Id))
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

                    qty = procMaterialEntity.Batch ?? 0;
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
            if (planSfcInfoCreateDto.ReceiveType == PlanSFCReceiveTypeEnum.MaterialSfc)
            {
                await _manuCreateBarcodeService.CreateBarcodeByOldMESSFCAsync(new CreateBarcodeByOldMesSFCBo
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    UserName = _currentUser.UserName,
                    WorkOrderId = planSfcInfoCreateDto.WorkOrderId,
                    OldSFCs = barcodeList
                }, _localizationService);
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
                await _manuCreateBarcodeService.CreateBarcodeByExternalSFCAsync(new CreateBarcodeByExternalSFCBo
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    UserName = _currentUser.UserName,
                    WorkOrderId = planSfcInfoCreateDto.WorkOrderId,
                    ExternalSFCs = barcodeList
                }, _localizationService);
            }
            ts.Complete();
        }

        /// <summary>
        /// 条码接收扫码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PlanSfcReceiveSfcDto> PlanSfcReceiveScanCodeAsync(PlanSfcReceiveScanCodeDto param)
        {
            await _validationModifyRules.ValidateAndThrowAsync(param);
            var planWorkOrderEntity = await _manuCommonOldService.GetWorkOrderByIdAsync(param.WorkOrderId);
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(planWorkOrderEntity.ProductId);
            if (param.ReceiveType == PlanSFCReceiveTypeEnum.SupplierSfc && procMaterialEntity.Batch == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16502)).WithData("product", procMaterialEntity.MaterialCode);
            }
            var manuSfcEntity = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
            {
                SFC = param.SFC,
                SiteId = _currentSite.SiteId ?? 0,
                Type = SfcTypeEnum.Produce
            });

            decimal qty = 0;
            if (param.ReceiveType == PlanSFCReceiveTypeEnum.MaterialSfc)
            {
                if (manuSfcEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16128)).WithData("sfc", param.SFC);

                }
                if (ManuSfcStatus.SfcStatusInProcess.Contains(manuSfcEntity.Status))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16123)).WithData("sfc", param.SFC);
                }
                var manuSfcInfoEntity = await _manuSfcInfoRepository.GetBySFCIdWithIsUseAsync(manuSfcEntity.Id);

                //if (manuSfcInfoEntity != null && manuSfcInfoEntity.WorkOrderId != param.RelevanceWorkOrderId)
                //{
                //    throw new CustomerValidationException(nameof(ErrorCode.MES16127));
                //}

                var manuContainerPackEntity = await _manuContainerPackRepository.GetByLadeBarCodeAsync(new ManuContainerPackQuery { LadeBarCode = param.SFC, SiteId = _currentSite.SiteId ?? 0 });

                if (manuContainerPackEntity != null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16132)).WithData("sfc", param.SFC);
                }

                var whMaterialInventoryEntity = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery { SiteId = _currentSite.SiteId, BarCode = param.SFC });
                if (whMaterialInventoryEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16120));
                }
                //if (procMaterialEntity.Id != whMaterialInventoryEntity.MaterialId)
                //{
                //    throw new CustomerValidationException(nameof(ErrorCode.MES16129));
                //}
                qty = whMaterialInventoryEntity.QuantityResidue;
                if (qty <= 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18021));
                }
                procMaterialEntity = await _procMaterialRepository.GetByIdAsync(whMaterialInventoryEntity.MaterialId);
            }
            else
            {
                if (!await _manuCommonOldService.CheckBarCodeByMaskCodeRuleAsync(param.SFC, procMaterialEntity.Id))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16121)).WithData("product", procMaterialEntity.MaterialCode);
                }
                if (manuSfcEntity != null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16122)).WithData("sfc", param.SFC);
                }
                qty = procMaterialEntity.Batch ?? 0;
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
            return new PlanSfcReceiveSfcDto()
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

        /// <summary>
        /// 条码接收扫码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<PlanSfcReceiveSfcDto>> PlanSfcReceiveScanListAsync(PlanSfcReceiveScanListDto param)
        {
            await _validationScanListRules.ValidateAndThrowAsync(param);

            var rspList = new List<PlanSfcReceiveSfcDto>();

            var planWorkOrderEntity = await _manuCommonOldService.GetWorkOrderByIdAsync(param.WorkOrderId);
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(planWorkOrderEntity.ProductId);
            if (param.ReceiveType == PlanSFCReceiveTypeEnum.SupplierSfc && procMaterialEntity.Batch == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16502)).WithData("product", procMaterialEntity.MaterialCode);
            }
            var manuSfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFCs = param.SFCs,
                Type = SfcTypeEnum.Produce
            });

            var sfcMaterialEntityDic = new Dictionary<string, (decimal, ProcMaterialEntity)>();

            decimal qty = 0;
            if (param.ReceiveType == PlanSFCReceiveTypeEnum.MaterialSfc)
            {
                //校验条码是否存在
                if (manuSfcEntities == null || !manuSfcEntities.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16128)).WithData("sfc", string.Join(',', param.SFCs));
                }
                var noExistSfcs = param.SFCs.Except(manuSfcEntities.Select(x => x.SFC));
                if (noExistSfcs != null && noExistSfcs.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16128)).WithData("sfc", string.Join(',', noExistSfcs));
                }
                //校验是否已是在制状态
                var inProcessSfcs = manuSfcEntities.Where(x => ManuSfcStatus.SfcStatusInProcess.Contains(x.Status)).Select(x => x.SFC);
                if (inProcessSfcs != null && inProcessSfcs.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16123)).WithData("sfc", string.Join(',', inProcessSfcs));
                }
                //校验是否已装箱
                var manuContainerPackEntities = await _manuContainerPackRepository.GetByLadeBarCodesAsync(new ManuContainerPackQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    LadeBarCodes = param.SFCs
                });
                if (manuContainerPackEntities != null && manuContainerPackEntities.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16132)).WithData("sfc", string.Join(',', manuContainerPackEntities.Select(x => x.LadeBarCode)));
                }
                //校验库存数量
                var whMaterialInventoryEntities = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery { SiteId = _currentSite.SiteId ?? 0, BarCodes = param.SFCs });
                if (whMaterialInventoryEntities == null || !whMaterialInventoryEntities.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16139)).WithData("sfc", string.Join(',', param.SFCs));
                }
                var noExistInventorySfcs = param.SFCs.Except(whMaterialInventoryEntities.Select(x => x.MaterialBarCode));
                if (noExistInventorySfcs != null && noExistInventorySfcs.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16139)).WithData("sfc", string.Join(',', noExistInventorySfcs));
                }
                var usderstockSfcs = whMaterialInventoryEntities.Where(x => x.QuantityResidue <= 0).Select(x => x.MaterialBarCode);
                if (usderstockSfcs != null && usderstockSfcs.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16140)).WithData("sfc", string.Join(',', usderstockSfcs));
                }
                //查询条码对应物料信息
                var materialEntities = await _procMaterialRepository.GetByIdsAsync(whMaterialInventoryEntities.Select(x => x.MaterialId).Distinct());
                foreach (var item in whMaterialInventoryEntities)
                {
                    sfcMaterialEntityDic.Add(item.MaterialBarCode, (item.QuantityResidue, materialEntities.First(x => x.Id == item.MaterialId)));
                }
            }
            else
            {
                //校验掩码规则
                foreach (var item in param.SFCs)
                {
                    if (!await _manuCommonOldService.CheckBarCodeByMaskCodeRuleAsync(item, procMaterialEntity.Id))
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16121)).WithData("product", procMaterialEntity.MaterialCode);
                    }
                }
                //校验条码是否存在
                if (manuSfcEntities != null && manuSfcEntities.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16122)).WithData("sfc", string.Join(',', manuSfcEntities.Select(x => x.SFC)));
                }
                qty = procMaterialEntity.Batch ?? 0;
                if (qty <= 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16610)).WithData("sfc", param.SFCs[0]);
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

            rspList = param.SFCs.Select(item =>
            {
                var materialEntity = procMaterialEntity;
                var tmpQty = qty;
                if (param.ReceiveType == PlanSFCReceiveTypeEnum.MaterialSfc)
                {
                    (tmpQty, materialEntity) = sfcMaterialEntityDic[item];
                }
                var dto = new PlanSfcReceiveSfcDto
                {
                    OrderCode = planWorkOrderEntity.OrderCode,
                    Type = planWorkOrderEntity.Type,
                    OrderCodeQty = planWorkOrderEntity.Qty,
                    BarCode = item,
                    MaterialCode = materialEntity.MaterialCode,
                    MaterialName = materialEntity.MaterialName,
                    MaterialVersion = materialEntity.Version ?? "",
                    Qty = tmpQty,
                    RelevanceOrderCode = relevanceOrderCode
                };
                return dto;
            }).ToList();

            return rspList;
        }

    }
}
