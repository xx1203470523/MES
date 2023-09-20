using FluentValidation;
using FluentValidation.Results;
using Hymson.EventBus.Abstractions;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Events.ManufactureEvents.ManuSfcStepEvents;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteCodeRule.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuCreateBarcode
{
    /// <summary>
    /// 条码下达与生成
    /// </summary>
    public class ManuCreateBarcodeService : IManuCreateBarcodeService
    {
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        private readonly IInteCodeRulesMakeRepository _inteCodeRulesMakeRepository;
        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 事件总线
        /// </summary>
        private readonly IEventBus<EventBusInstance1> _eventBus;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="procMaterialRepository"></param>
        /// <param name="inteCodeRulesRepository"></param>
        /// <param name="inteCodeRulesMakeRepository"></param>
        /// <param name="manuGenerateBarcodeService"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="masterDataService"></param>
        /// <param name="eventBus"></param>
        public ManuCreateBarcodeService(IProcMaterialRepository procMaterialRepository,
                 IInteCodeRulesRepository inteCodeRulesRepository,
                 IInteCodeRulesMakeRepository inteCodeRulesMakeRepository,
                 IManuGenerateBarcodeService manuGenerateBarcodeService,
                 IManuSfcRepository manuSfcRepository,
                 IManuSfcInfoRepository manuSfcInfoRepository,
                 IManuSfcProduceRepository manuSfcProduceRepository,
                 IPlanWorkOrderRepository planWorkOrderRepository,
                 IMasterDataService masterDataService,
                 IEventBus<EventBusInstance1> eventBus)
        {
            _procMaterialRepository = procMaterialRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _masterDataService = masterDataService;
            _eventBus = eventBus;
        }

        /// <summary>
        /// 工单下达条码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<ManuSfcEntity>> CreateBarcodeByWorkOrderIdAsync(CreateBarcodeByWorkOrderBo param, ILocalizationService localizationService)
        {
            var planWorkOrderEntity = await _masterDataService.GetProduceWorkOrderByIdAsync(new WorkOrderIdBo
            {
                WorkOrderId = param.WorkOrderId,
                IsVerifyActivation = false
            });

            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(planWorkOrderEntity.ProductId);
            var inteCodeRulesEntity = await _inteCodeRulesRepository.GetInteCodeRulesByProductIdAsync(new InteCodeRulesByProductQuery
            {
                ProductId = planWorkOrderEntity.ProductId,
                CodeType = CodeRuleCodeTypeEnum.ProcessControlSeqCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16501)).WithData("product", procMaterialEntity.MaterialCode);

            if (procMaterialEntity.Batch == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16502)).WithData("product", procMaterialEntity.MaterialCode);
            }
            if (param.Qty <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16508));
            }
            var discuss = (int)Math.Ceiling(param.Qty / procMaterialEntity.Batch);
            var processRouteFirstProcedure = await _masterDataService.GetFirstProcedureAsync(planWorkOrderEntity.ProcessRouteId);

            // 读取基础数据
            var codeRulesMakeList = await _inteCodeRulesMakeRepository.GetInteCodeRulesMakeEntitiesAsync(new InteCodeRulesMakeQuery
            {
                SiteId = param.SiteId,
                CodeRulesId = inteCodeRulesEntity.Id
            });

            var barcodeList = await _manuGenerateBarcodeService.GenerateBarCodeSerialNumberReturnBarCodeInfosAsync(new BarCodeSerialNumberBo
            {
                IsTest = false,
                IsSimulation = false,
                CodeRulesMakeBos = codeRulesMakeList.Select(s => new CodeRulesMakeBo
                {
                    Seq = s.Seq,
                    ValueTakingType = s.ValueTakingType,
                    SegmentedValue = s.SegmentedValue,
                    CustomValue = s.CustomValue,
                }),

                CodeRuleKey = $"{inteCodeRulesEntity.Id}",
                Count = discuss,
                Base = inteCodeRulesEntity.Base,
                Increment = inteCodeRulesEntity.Increment,
                IgnoreChar = inteCodeRulesEntity.IgnoreChar,
                OrderLength = inteCodeRulesEntity.OrderLength,
                ResetType = inteCodeRulesEntity.ResetType,
                StartNumber = inteCodeRulesEntity.StartNumber,
                CodeMode = inteCodeRulesEntity.CodeMode,
            });

            List<ManuSfcEntity> manuSfcList = new();
            List<ManuSfcInfoEntity> manuSfcInfoList = new();
            List<ManuSfcProduceEntity> manuSfcProduceList = new();
            List<ManuSfcStepEntity> manuSfcStepList = new();
            var issQty = param.Qty;
            foreach (var barCodeInfoBarCodes in barcodeList.Select(barCodeInfo => barCodeInfo.BarCodes))
            {
                var qty = issQty > procMaterialEntity.Batch * barCodeInfoBarCodes.Count() ? procMaterialEntity.Batch : issQty / barCodeInfoBarCodes.Count();
                foreach (var sfc in barCodeInfoBarCodes)
                {

                    issQty -= procMaterialEntity.Batch;

                    var manuSfcEntity = new ManuSfcEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = param.SiteId,
                        SFC = sfc,
                        Qty = qty,
                        IsUsed = YesOrNoEnum.No,
                        Status = SfcStatusEnum.lineUp,
                        CreatedBy = param.UserName!,
                        UpdatedBy = param.UserName
                    };
                    manuSfcList.Add(manuSfcEntity);

                    manuSfcInfoList.Add(new ManuSfcInfoEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = param.SiteId,
                        SfcId = manuSfcEntity.Id,
                        WorkOrderId = planWorkOrderEntity.Id,
                        ProductId = planWorkOrderEntity.ProductId,
                        IsUsed = true,
                        CreatedBy = param.UserName!,
                        UpdatedBy = param.UserName
                    });

                    manuSfcProduceList.Add(new ManuSfcProduceEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = param.SiteId,
                        SFC = sfc,
                        SFCId = manuSfcEntity.Id,
                        ProductId = planWorkOrderEntity.ProductId,
                        WorkOrderId = planWorkOrderEntity.Id,
                        BarCodeInfoId = manuSfcEntity.Id,
                        ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                        WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                        ProductBOMId = planWorkOrderEntity.ProductBOMId,
                        Qty = qty,
                        ProcedureId = processRouteFirstProcedure.ProcedureId,
                        Status = SfcStatusEnum.lineUp,
                        RepeatedCount = 0,
                        IsScrap = TrueOrFalseEnum.No,
                        CreatedBy = param.UserName!,
                        UpdatedBy = param.UserName
                    });

                    manuSfcStepList.Add(new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = param.SiteId,
                        SFC = sfc,
                        ProductId = planWorkOrderEntity.ProductId,
                        WorkOrderId = planWorkOrderEntity.Id,
                        ProductBOMId = planWorkOrderEntity.ProductBOMId,
                        WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                        Qty = qty,
                        ProcedureId = processRouteFirstProcedure.ProcedureId,
                        Operatetype = ManuSfcStepTypeEnum.Create,
                        CurrentStatus = SfcStatusEnum.lineUp,
                        CreatedBy = param.UserName!,
                        UpdatedBy = param.UserName
                    });
                }
            }

            // 开启事务
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            var row = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderId(new UpdatePassDownQuantityCommand
            {
                WorkOrderId = planWorkOrderEntity.Id,
                PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100),
                PassDownQuantity = param.Qty,
                UserName = param.UserName!,
                UpdateDate = HymsonClock.Now()
            });

            if (row == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16503)).WithData("workorder", planWorkOrderEntity.OrderCode);
            }

            await _manuSfcRepository.InsertRangeAsync(manuSfcList);
            await _manuSfcInfoRepository.InsertsAsync(manuSfcInfoList);
            await _manuSfcProduceRepository.InsertRangeAsync(manuSfcProduceList);
            ManuSfcStepsEvent @event = new ManuSfcStepsEvent()
            {
                manuSfcStepEntities = manuSfcStepList
            };
            _eventBus.Publish<ManuSfcStepsEvent>(@event);
            trans.Complete();
            return manuSfcList;
        }

        /// <summary>
        /// 根据外部条码接收
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task CreateBarcodeByExternalSFCAsync(CreateBarcodeByExternalSFCBo param, ILocalizationService localizationService)
        {
            var planWorkOrderEntity = await _masterDataService.GetWorkOrderByIdAsync(param.WorkOrderId);
            var sfclist = await _manuSfcRepository.GetBySFCsAsync(param.ExternalSFCs.Select(x => x.SFC));

            var processRouteFirstProcedure = await _masterDataService.GetFirstProcedureAsync(planWorkOrderEntity.ProcessRouteId);

            List<ManuSfcEntity> manuSfcList = new List<ManuSfcEntity>();
            List<ManuSfcInfoEntity> manuSfcInfoList = new List<ManuSfcInfoEntity>();
            List<ManuSfcProduceEntity> manuSfcProduceList = new List<ManuSfcProduceEntity>();
            List<ManuSfcStepEntity> manuSfcStepList = new List<ManuSfcStepEntity>();
            var validationFailures = new List<ValidationFailure>();
            foreach (var item in param.ExternalSFCs)
            {
                var sfcEntity = sfclist.FirstOrDefault(x => x.SFC == item.SFC);
                if (sfcEntity != null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", item.SFC}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.SFC);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES16504);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                var manuSfcEntity = new ManuSfcEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SFC = item.SFC,
                    Qty = item.Qty,
                    IsUsed = YesOrNoEnum.Yes,
                    Status = SfcStatusEnum.lineUp,
                    CreatedBy = param.UserName!,
                    UpdatedBy = param.UserName
                };
                manuSfcList.Add(manuSfcEntity);

                manuSfcInfoList.Add(new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SfcId = manuSfcEntity.Id,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductId = planWorkOrderEntity.ProductId,
                    IsUsed = true,
                    CreatedBy = param.UserName!,
                    UpdatedBy = param.UserName
                });

                manuSfcProduceList.Add(new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SFC = item.SFC,
                    SFCId = manuSfcEntity.Id,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    BarCodeInfoId = manuSfcEntity.Id,
                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    Qty = item.Qty,
                    ProcedureId = processRouteFirstProcedure.ProcedureId,
                    Status = SfcStatusEnum.lineUp,
                    RepeatedCount = 0,
                    IsScrap = TrueOrFalseEnum.No,
                    CreatedBy = param.UserName!,
                    UpdatedBy = param.UserName
                });

                manuSfcStepList.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SFC = item.SFC,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    Qty = item.Qty,
                    ProcedureId = processRouteFirstProcedure.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.Receive,
                    CurrentStatus = SfcStatusEnum.lineUp,
                    CreatedBy = param.UserName!,
                    UpdatedBy = param.UserName
                });
            }
            if (validationFailures.Any())
            {
                throw new ValidationException(localizationService.GetResource("SFCError"), validationFailures);
            }
            using var ts = TransactionHelper.GetTransactionScope();
            var row = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderId(new UpdatePassDownQuantityCommand
            {
                WorkOrderId = planWorkOrderEntity.Id,
                PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100),
                PassDownQuantity = param.ExternalSFCs.Sum(x => x.Qty),
                UserName = param.UserName!,
                UpdateDate = HymsonClock.Now()
            });

            if (row == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16503)).WithData("workorder", planWorkOrderEntity.OrderCode);
            }
            await _manuSfcRepository.InsertRangeAsync(manuSfcList);
            await _manuSfcInfoRepository.InsertsAsync(manuSfcInfoList);
            await _manuSfcProduceRepository.InsertRangeAsync(manuSfcProduceList);
            //await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepList);
            _eventBus.Publish(new ManuSfcStepsEvent { manuSfcStepEntities = manuSfcStepList });

            ts.Complete();

        }

        /// <summary>
        /// 内部条码复用
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task CreateBarcodeByOldMESSFCAsync(CreateBarcodeByOldMesSFCBo param, ILocalizationService localizationService)
        {
            var planWorkOrderEntity = await _masterDataService.GetWorkOrderByIdAsync(param.WorkOrderId);
            var sfclist = await _manuSfcRepository.GetBySFCsAsync(param.OldSFCs.Select(x => x.SFC));
            var sfcInfoList = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfclist.Select(x => x.Id));
            var processRouteFirstProcedure = await _masterDataService.GetFirstProcedureAsync(planWorkOrderEntity.ProcessRouteId);

            List<ManuSfcEntity> manuSfcList = new List<ManuSfcEntity>();
            List<ManuSfcInfoEntity> manuSfcInfoList = new List<ManuSfcInfoEntity>();
            List<ManuSfcInfoEntity> updateManuSfcInfoList = new List<ManuSfcInfoEntity>();
            List<ManuSfcProduceEntity> manuSfcProduceList = new List<ManuSfcProduceEntity>();
            List<ManuSfcStepEntity> manuSfcStepList = new List<ManuSfcStepEntity>();
            var validationFailures = new List<ValidationFailure>();
            foreach (var item in param.OldSFCs)
            {
                var sfcEntity = sfclist.FirstOrDefault(x => x.SFC == item.SFC);
                if (sfcEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", item.SFC}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.SFC);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES16505);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                if (sfcEntity.Status != SfcStatusEnum.Complete)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", item.SFC}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.SFC);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES16506);
                    validationFailures.Add(validationFailure);
                }

                sfcEntity.Qty = item.Qty;
                sfcEntity.IsUsed = YesOrNoEnum.Yes;
                sfcEntity.Status = SfcStatusEnum.lineUp;
                sfcEntity.UpdatedBy = param.UserName;
                sfcEntity.UpdatedOn = HymsonClock.Now();
                manuSfcList.Add(sfcEntity);
                var sfcInfo = sfcInfoList.FirstOrDefault(x => x.SfcId == sfcEntity.Id);
                if (sfcInfo != null)
                {
                    sfcInfo.IsUsed = false;
                    sfcInfo.UpdatedBy = param.UserName;
                    sfcInfo.UpdatedOn = HymsonClock.Now();
                    updateManuSfcInfoList.Add(sfcInfo);
                }
                manuSfcInfoList.Add(new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SfcId = sfcEntity.Id,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductId = planWorkOrderEntity.ProductId,
                    IsUsed = true,
                    CreatedBy = param.UserName!,
                    UpdatedBy = param.UserName
                });

                manuSfcProduceList.Add(new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SFC = item.SFC,
                    SFCId = sfcEntity.Id,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    BarCodeInfoId = sfcEntity.Id,
                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    Qty = item.Qty,
                    ProcedureId = processRouteFirstProcedure.ProcedureId,
                    Status = SfcStatusEnum.lineUp,
                    RepeatedCount = 0,
                    IsScrap = TrueOrFalseEnum.No,
                    CreatedBy = param.UserName!,
                    UpdatedBy = param.UserName
                });

                manuSfcStepList.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SFC = item.SFC,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    Qty = item.Qty,
                    ProcedureId = processRouteFirstProcedure.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.Receive,
                    CurrentStatus = SfcStatusEnum.lineUp,
                    CreatedBy = param.UserName!,
                    UpdatedBy = param.UserName
                });
            }
            if (validationFailures.Any())
            {
                throw new ValidationException(localizationService.GetResource("SFCError"), validationFailures);
            }
            using var ts = TransactionHelper.GetTransactionScope();
            var row = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderId(new UpdatePassDownQuantityCommand
            {
                WorkOrderId = planWorkOrderEntity.Id,
                PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100),
                PassDownQuantity = param.OldSFCs.Sum(x => x.Qty),
                UserName = param.UserName!,
                UpdateDate = HymsonClock.Now()
            });

            if (row == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16503)).WithData("workorder", planWorkOrderEntity.OrderCode);
            }
            await _manuSfcInfoRepository.UpdatesAsync(updateManuSfcInfoList);
            await _manuSfcRepository.UpdateRangeAsync(manuSfcList);
            await _manuSfcInfoRepository.InsertsAsync(manuSfcInfoList);
            await _manuSfcProduceRepository.InsertRangeAsync(manuSfcProduceList);
            ManuSfcStepsEvent @event = new ManuSfcStepsEvent()
            {
                manuSfcStepEntities = manuSfcStepList
            };
            _eventBus.Publish<ManuSfcStepsEvent>(@event);
            ts.Complete();
        }
    }
}
