using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteCodeRule.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.ProductSet.Query;
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
        private readonly IProcBomRepository _procBomRepository;

        private readonly IProcMaterialRepository _procMaterialRepository;

        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;

        private readonly IInteCodeRulesMakeRepository _inteCodeRulesMakeRepository;

        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;

        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        ///  仓储（物料加载）
        /// </summary>
        private readonly IManuFeedingRepository _manuFeedingRepository;

        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 仓储接口（生产配置）
        /// </summary>
        private readonly IProcProductSetRepository _procProductSetRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（资源维护）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        private readonly IProcProcessRouteRepository _procProcessRouteRepository;

        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;

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
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuFeedingRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="masterDataService"></param>
        /// <param name="procProductSetRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procResourceRepository"></param>
        public ManuCreateBarcodeService(IProcMaterialRepository procMaterialRepository,
                 IInteCodeRulesRepository inteCodeRulesRepository,
                 IInteCodeRulesMakeRepository inteCodeRulesMakeRepository,
                 IManuGenerateBarcodeService manuGenerateBarcodeService,
                 IManuSfcRepository manuSfcRepository,
                 IManuSfcInfoRepository manuSfcInfoRepository,
                 IManuSfcProduceRepository manuSfcProduceRepository,
                 IManuSfcStepRepository manuSfcStepRepository,
                 IManuFeedingRepository manuFeedingRepository,
                 IPlanWorkOrderRepository planWorkOrderRepository,
                 IMasterDataService masterDataService,
                 IProcProductSetRepository procProductSetRepository,
                 IProcProcedureRepository procProcedureRepository,
                 IProcResourceRepository procResourceRepository,
                 IInteWorkCenterRepository inteWorkCenterRepository,
                 IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
                 IProcProcessRouteRepository procProcessRouteRepository,
                 IProcBomRepository procBomRepository)
        {
            _procMaterialRepository = procMaterialRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuFeedingRepository = manuFeedingRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _masterDataService = masterDataService;
            _procProductSetRepository = procProductSetRepository;
            _procProcedureRepository = procProcedureRepository;
            _procResourceRepository = procResourceRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _procBomRepository = procBomRepository;
        }

        /// <summary>
        /// 工单下达条码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<CreateBarcodeByWorkOrderOutputBo>> CreateBarcodeByWorkOrderIdAsync(CreateBarcodeByWorkOrderBo param, ILocalizationService localizationService)
        {
            var planWorkOrderEntity = await _masterDataService.GetProduceWorkOrderByIdAsync(new WorkOrderIdBo
            {
                WorkOrderId = param.WorkOrderId,
                IsVerifyActivation = false
            });

            var inteWorkCenterEntity = await _inteWorkCenterRepository.GetByResourceIdAsync(param.ResourceId);
            if (inteWorkCenterEntity == null)
            {
                var procResourceEntity = await _procResourceRepository.GetByIdAsync(param.ResourceId);
                throw new CustomerValidationException(nameof(ErrorCode.MES16511)).WithData("Code", procResourceEntity.ResCode);
            }

            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(param.ProcedureId);
            if (procProcedureEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16510));
            }

            var procBomEntity = await _procBomRepository.GetByIdAsync(planWorkOrderEntity.ProductBOMId);

            // 获取产出设置的产品ID
            var productIdOfSet = await _masterDataService.GetProductSetIdAsync(new Bos.Common.MasterData.ProductSetBo
            {
                SiteId = param.SiteId,
                ProductId = planWorkOrderEntity.ProductId,
                ProcedureId = param.ProcedureId,
                ResourceId = param.ResourceId,
            });

            // 产品ID
            var productId = productIdOfSet ?? planWorkOrderEntity.ProductId;

            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(productId);
            var inteCodeRulesEntity = await _inteCodeRulesRepository.GetInteCodeRulesByProductIdAsync(new InteCodeRulesByProductQuery
            {
                ProductId = productId,
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

            var processRouteDetailNodeEntities = await _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(planWorkOrderEntity.ProcessRouteId);
            var processRouteDetailNodeEntity = processRouteDetailNodeEntities.FirstOrDefault(x => x.ProcedureId == param.ProcedureId);
            if (processRouteDetailNodeEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16509));
            }

            var processRouteEntity = await _procProcessRouteRepository.GetByIdAsync(processRouteDetailNodeEntity.ProcessRouteId);

            // 读取基础数据
            var codeRulesMakeList = await _inteCodeRulesMakeRepository.GetInteCodeRulesMakeEntitiesAsync(new InteCodeRulesMakeQuery
            {
                SiteId = param.SiteId,
                CodeRulesId = inteCodeRulesEntity.Id
            });

            if (codeRulesMakeList == null || !codeRulesMakeList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16501));
            }

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
                SiteId = param.SiteId,
            });

            List<CreateBarcodeByWorkOrderOutputBo> result = new();

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

                    var manuSfcInfoEntity = new ManuSfcInfoEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = param.SiteId,
                        SfcId = manuSfcEntity.Id,
                        WorkOrderId = planWorkOrderEntity.Id,
                        ProductId = productId,
                        IsUsed = true,
                        CreatedBy = param.UserName!,
                        UpdatedBy = param.UserName
                    };
                    manuSfcInfoList.Add(manuSfcInfoEntity);

                    var manuSfcProduceEntity = new ManuSfcProduceEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = param.SiteId,
                        SFC = sfc,
                        SFCId = manuSfcEntity.Id,
                        ProductId = productId,
                        WorkOrderId = planWorkOrderEntity.Id,
                        BarCodeInfoId = manuSfcInfoEntity.Id,
                        ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                        WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                        ProductBOMId = planWorkOrderEntity.ProductBOMId,
                        Qty = qty,
                        ProcedureId = procProcedureEntity.Id,
                        Status = SfcStatusEnum.lineUp,
                        RepeatedCount = 0,
                        IsScrap = TrueOrFalseEnum.No,
                        CreatedBy = param.UserName!,
                        UpdatedBy = param.UserName
                    };
                    manuSfcProduceList.Add(manuSfcProduceEntity);

                    var manuSfcStepEntity = new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = param.SiteId,
                        SFC = sfc,
                        ProductId = productId,
                        WorkOrderId = planWorkOrderEntity.Id,
                        ProductBOMId = planWorkOrderEntity.ProductBOMId,
                        WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                        Qty = qty,
                        ProcedureId = procProcedureEntity.Id,
                        Operatetype = ManuSfcStepTypeEnum.Create,
                        CurrentStatus = SfcStatusEnum.lineUp,
                        CreatedBy = param.UserName!,
                        UpdatedBy = param.UserName
                    };
                    manuSfcStepList.Add(manuSfcStepEntity);

                    var resultItem = new CreateBarcodeByWorkOrderOutputBo
                    {
                        ManuSFCId = manuSfcEntity.Id,
                        SFC = manuSfcEntity.SFC,
                        BarcodeStatus = manuSfcEntity.Status,
                        ProcedureId = procProcedureEntity.Id,
                        ProcedureCode = procProcedureEntity.Code,
                        ProcedureVersion = procProcedureEntity.Version,
                        MaterialId = procMaterialEntity.Id,
                        MaterialCode = procMaterialEntity.MaterialCode,
                        MaterialVersion = procMaterialEntity.Version,
                        ProcessRouteId = processRouteEntity.Id,
                        ProcessRouteCode = processRouteEntity.Code,
                        ProcessRouteVersion = processRouteEntity.Version,
                        BomId = procBomEntity?.Id,
                        BomCode = procBomEntity?.BomCode,
                        BomVersion = procBomEntity?.Version
                    };
                    result.Add(resultItem);
                }
            }

            // 开启事务
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);

            if (productId == planWorkOrderEntity.ProductId)
            {
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
            }

            await _manuSfcRepository.InsertRangeAsync(manuSfcList);
            await _manuSfcInfoRepository.InsertsAsync(manuSfcInfoList);
            await _manuSfcProduceRepository.InsertRangeAsync(manuSfcProduceList);
            await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepList);

            trans.Complete();

            return result;
        }

        /// <summary>
        /// 根据外部条码接收
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task CreateBarcodeByExternalSFCAsync(CreateBarcodeByExternalSFCBo param, ILocalizationService localizationService)
        {
            var planWorkOrderEntity = await _masterDataService.GetProduceWorkOrderByIdAsync(new WorkOrderIdBo
            {
                WorkOrderId = param.WorkOrderId,
                IsVerifyActivation = false
            });
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

                var manuSfcInfoEntity = new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SfcId = manuSfcEntity.Id,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductId = planWorkOrderEntity.ProductId,
                    IsUsed = true,
                    CreatedBy = param.UserName!,
                    UpdatedBy = param.UserName
                };
                manuSfcInfoList.Add(manuSfcInfoEntity);

                manuSfcProduceList.Add(new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SFC = item.SFC,
                    SFCId = manuSfcEntity.Id,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    BarCodeInfoId = manuSfcInfoEntity.Id,
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
            await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepList);

            ts.Complete();

        }

        /// <summary>
        /// 内部条码复用
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task CreateBarcodeByOldMESSFCAsync(CreateBarcodeByOldMesSFCBo param, ILocalizationService localizationService)
        {
            var planWorkOrderEntity = await _masterDataService.GetProduceWorkOrderByIdAsync(new WorkOrderIdBo
            {
                WorkOrderId = param.WorkOrderId,
                IsVerifyActivation = false
            });
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

                var manuSfcInfoEntity = new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SfcId = sfcEntity.Id,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductId = planWorkOrderEntity.ProductId,
                    IsUsed = true,
                    CreatedBy = param.UserName!,
                    UpdatedBy = param.UserName
                };
                manuSfcInfoList.Add(manuSfcInfoEntity);

                manuSfcProduceList.Add(new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SFC = item.SFC,
                    SFCId = sfcEntity.Id,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    BarCodeInfoId = manuSfcInfoEntity.Id,
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
            await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepList);

            ts.Complete();
        }

        /// <summary>
        /// 半成品条码生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<ManuSfcEntity>> CreateBarcodeBySemiProductIdAsync(CreateBarcodeBySemiProductId param)
        {
            // 查询资源
            var resourceEntity = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = param.SiteId,
                Code = param.ResourceCode,
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19603)).WithData("Code", param.ResourceCode);

            // 半成品生成条码 通过上料记录来查询资源绑定的工单
            var feedingEntities = await _manuFeedingRepository.GetByResourceIdAndMaterialIdsWithOutZeroAsync(new GetByResourceIdAndMaterialIdsQuery
            {
                ResourceId = resourceEntity.Id,
                MaterialIds = null
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19607)).WithData("Code", param.ResourceCode);

            var feedingEntity = feedingEntities.FirstOrDefault(f => f.WorkOrderId != null && f.WorkOrderId != 0)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES19608)).WithData("Code", param.ResourceCode);

            // 工单检查
            var workOrderId = feedingEntity.WorkOrderId ?? 0;
            if (workOrderId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES16016)).WithData("WorkOrder", workOrderId);

            var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(workOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16016)).WithData("WorkOrder", workOrderId);

            var procedureEntity = await _procProcedureRepository.GetProcProcedureByResourceIdAsync(new ProcProdureByResourceIdQuery
            {
                SiteId = param.SiteId,
                ResourceId = resourceEntity.Id,
            });

            // 产出设置
            var productSetEntity = await _procProductSetRepository.GetByProcedureIdAndProductIdAsync(new GetByProcedureIdAndProductIdQuery
            {
                ProductId = workOrderEntity.ProductId,
                SetPointId = resourceEntity.Id,
                SiteId = param.SiteId,
            });
            productSetEntity ??= await _procProductSetRepository.GetByProcedureIdAndProductIdAsync(new GetByProcedureIdAndProductIdQuery
            {
                ProductId = workOrderEntity.ProductId,
                SetPointId = procedureEntity.Id,
                SiteId = param.SiteId,
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19605));


            // 下面的代码等有时间了再整理。
            var mo = await _procMaterialRepository.GetByIdAsync(productSetEntity.SemiProductId);

            var planWorkOrderEntity = await _masterDataService.GetProduceWorkOrderByIdAsync(new WorkOrderIdBo
            {
                WorkOrderId = workOrderEntity.Id,
                IsVerifyActivation = false
            });


            var inteCodeRulesEntity = await _inteCodeRulesRepository.GetInteCodeRulesByProductIdAsync(new InteCodeRulesByProductQuery
            {
                ProductId = productSetEntity.SemiProductId,
                CodeType = CodeRuleCodeTypeEnum.ProcessControlSeqCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16501)).WithData("product", mo.MaterialCode);

            //这些取的是半成品的物料批次信息
            if (mo.Batch <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16508));
            }

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
                Count = 1,
                Base = inteCodeRulesEntity.Base,
                Increment = inteCodeRulesEntity.Increment,
                IgnoreChar = inteCodeRulesEntity.IgnoreChar,
                OrderLength = inteCodeRulesEntity.OrderLength,
                ResetType = inteCodeRulesEntity.ResetType,
                StartNumber = inteCodeRulesEntity.StartNumber,
                CodeMode = inteCodeRulesEntity.CodeMode,
                SiteId = param.SiteId,
            });

            // 开启事务
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);

            List<ManuSfcEntity> manuSfcList = new();
            List<ManuSfcInfoEntity> manuSfcInfoList = new();
            List<ManuSfcProduceEntity> manuSfcProduceList = new();
            List<ManuSfcStepEntity> manuSfcStepList = new();

            foreach (var item in barcodeList)
            {
                foreach (var sfc in item.BarCodes)
                {
                    var sfcId = IdGenProvider.Instance.CreateId();
                    var sfcInfoId = IdGenProvider.Instance.CreateId();

                    manuSfcList.Add(new ManuSfcEntity
                    {
                        Id = sfcId,
                        SiteId = param.SiteId,
                        SFC = sfc,
                        Qty = mo.Batch,
                        IsUsed = YesOrNoEnum.No,
                        Status = SfcStatusEnum.lineUp,
                        CreatedBy = param.UserName,
                        UpdatedBy = param.UserName
                    });

                    manuSfcInfoList.Add(new ManuSfcInfoEntity
                    {
                        Id = sfcInfoId,
                        SiteId = param.SiteId,
                        SfcId = sfcId,
                        WorkOrderId = planWorkOrderEntity.Id,
                        ProductId = productSetEntity.SemiProductId,
                        IsUsed = true,
                        CreatedBy = param.UserName,
                        UpdatedBy = param.UserName
                    });

                    manuSfcProduceList.Add(new ManuSfcProduceEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = param.SiteId,
                        SFC = sfc,
                        SFCId = sfcId,
                        ProductId = productSetEntity.SemiProductId,
                        WorkOrderId = planWorkOrderEntity.Id,
                        BarCodeInfoId = sfcInfoId,
                        ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                        WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                        ProductBOMId = planWorkOrderEntity.ProductBOMId,
                        Qty = mo.Batch,
                        ProcedureId = procedureEntity.Id,
                        Status = SfcStatusEnum.lineUp,
                        RepeatedCount = 0,
                        IsScrap = TrueOrFalseEnum.No,
                        CreatedBy = param.UserName,
                        UpdatedBy = param.UserName
                    });

                    manuSfcStepList.Add(new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = param.SiteId,
                        SFC = sfc,
                        ProductId = productSetEntity.SemiProductId,
                        WorkOrderId = planWorkOrderEntity.Id,
                        ProductBOMId = planWorkOrderEntity.ProductBOMId,
                        WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                        Qty = mo.Batch,
                        ProcedureId = procedureEntity.Id,
                        Operatetype = ManuSfcStepTypeEnum.Create,
                        CurrentStatus = SfcStatusEnum.lineUp,
                        CreatedBy = param.UserName,
                        UpdatedBy = param.UserName
                    });
                }
            }

            await _manuSfcRepository.InsertRangeAsync(manuSfcList);
            await _manuSfcInfoRepository.InsertsAsync(manuSfcInfoList);
            await _manuSfcProduceRepository.InsertRangeAsync(manuSfcProduceList);
            await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepList);

            trans.Complete();

            return manuSfcList;
        }

    }
}
