using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
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

        private readonly IPlanWorkOrderBindRepository _planWorkOrderBindRepository;

        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;

        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

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
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteRepository"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="planWorkOrderBindRepository"></param>
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
                 IProcBomRepository procBomRepository,
                 IPlanWorkOrderBindRepository planWorkOrderBindRepository,
                 IPlanWorkOrderActivationRepository planWorkOrderActivationRepository,
                 IManuSfcCirculationRepository manuSfcCirculationRepository)
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
            _planWorkOrderBindRepository = planWorkOrderBindRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
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
            var productIdOfSet = await _masterDataService.GetProductSetIdAsync(new ProductSetBo
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

            var discuss = (int)Math.Ceiling(param.Qty / (procMaterialEntity.Batch ?? 1));

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
                throw new CustomerValidationException(nameof(ErrorCode.MES16501)).WithData("product", procMaterialEntity.MaterialCode);
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
                ProductId = procMaterialEntity.Id,
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
                InteWorkCenterId = inteWorkCenterEntity.Id
            });

            List<CreateBarcodeByWorkOrderOutputBo> result = new();

            List<ManuSfcEntity> manuSfcList = new();
            List<ManuSfcInfoEntity> manuSfcInfoList = new();
            List<ManuSfcProduceEntity> manuSfcProduceList = new();
            List<ManuSfcStepEntity> manuSfcStepList = new();

            var issQty = param.Qty;

            foreach (var barCodeInfoBarCodes in barcodeList.Select(barCodeInfo => barCodeInfo.BarCodes))
            {
                var batch = procMaterialEntity.Batch ?? 0;
                var qty = issQty > batch * barCodeInfoBarCodes.Count() ? batch : issQty / barCodeInfoBarCodes.Count();

                foreach (var sfc in barCodeInfoBarCodes)
                {
                    issQty -= batch;

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
                        ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                        ProductBOMId = planWorkOrderEntity.ProductBOMId,
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
                var row = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderIdAsync(new UpdatePassDownQuantityCommand
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
            var sfclist = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = param.SiteId,
                SFCs = param.ExternalSFCs.Select(x => x.SFC),
                Type = SfcTypeEnum.Produce
            });
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
                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
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
            var row = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderIdAsync(new UpdatePassDownQuantityCommand
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
            var sfclist = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = param.SiteId,
                SFCs = param.OldSFCs.Select(x => x.SFC),
                Type = SfcTypeEnum.Produce
            });
            var sfcInfoList = await _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(sfclist.Select(x => x.Id));
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
                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
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
            var row = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderIdAsync(new UpdatePassDownQuantityCommand
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
        /// 生产中生成条码
        /// </summary>
        /// <param name="param"></param>
        /// <param name="localizationService"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CreateBarcodeByWorkOrderOutputBo>> CreateBarcodeInProductionAsync(CreateBarcodeInProductionBo param)
        {
            // 查询资源
            var resourceEntity = await _procResourceRepository.GetByIdAsync(param.ResourceId) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19603)).WithData("Code", param.ResourceId);
            var inteWorkCenterEntity = await _inteWorkCenterRepository.GetByResourceIdAsync(resourceEntity.Id);
            if (inteWorkCenterEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19606)).WithData("Code", resourceEntity.ResCode);
            }

            long workOrderId = 0;
            if (inteWorkCenterEntity.IsMixLine ?? false)
            {
                var planWorkOrderActivationList = await _planWorkOrderActivationRepository.GetByWorkCenterIdAsync(inteWorkCenterEntity.Id);
                if (planWorkOrderActivationList == null || !planWorkOrderActivationList.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19912)).WithData("ResCode", resourceEntity.ResCode);
                }

                workOrderId = planWorkOrderActivationList?.FirstOrDefault()?.Id ?? 0;
            }
            else
            {
                var workOrderBindEntity = await _planWorkOrderBindRepository.GetByResourceIDAsync(new PlanWorkOrderBindByResourceIdQuery
                {
                    ResourceId = resourceEntity.Id,
                    SiteId = param.SiteId,
                }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19928)).WithData("ResCode", resourceEntity.ResCode);

                workOrderId = workOrderBindEntity?.Id ?? 0;
            }

            var planWorkOrderEntity = await _masterDataService.GetProduceWorkOrderByIdAsync(new WorkOrderIdBo
            {
                WorkOrderId = workOrderId,
                IsVerifyActivation = true
            });
            var procProcedureEntity = await _procProcedureRepository.GetProcProcedureByResourceIdAsync(new ProcProdureByResourceIdQuery
            {
                SiteId = param.SiteId,
                ResourceId = param.ResourceId,
            });

            if (procProcedureEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16510));
            }

            var procBomEntity = await _procBomRepository.GetByIdAsync(planWorkOrderEntity.ProductBOMId);

            // 获取产出设置的产品ID
            var productIdOfSet = await _masterDataService.GetProductSetIdAsync(new ProductSetBo
            {
                SiteId = param.SiteId,
                ProductId = planWorkOrderEntity.ProductId,
                ProcedureId = procProcedureEntity.Id,
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

            var processRouteDetailNodeEntities = await _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(planWorkOrderEntity.ProcessRouteId);
            var processRouteDetailNodeEntity = processRouteDetailNodeEntities.FirstOrDefault(x => x.ProcedureId == procProcedureEntity.Id);
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
                throw new CustomerValidationException(nameof(ErrorCode.MES16501)).WithData("product", procMaterialEntity.MaterialCode);
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
                Count = param.Count,
                Base = inteCodeRulesEntity.Base,
                Increment = inteCodeRulesEntity.Increment,
                IgnoreChar = inteCodeRulesEntity.IgnoreChar ?? "",
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

            decimal qty = 0;
            var status = param.IsInActive ? SfcStatusEnum.Activity : SfcStatusEnum.lineUp;
            foreach (var barCodeInfoBarCodes in barcodeList.Select(barCodeInfo => barCodeInfo.BarCodes))
            {
                foreach (var sfc in barCodeInfoBarCodes)
                {
                    qty = qty + (procMaterialEntity.Batch ?? 0);
                    var manuSfcEntity = new ManuSfcEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = param.SiteId,
                        SFC = sfc,
                        Qty = procMaterialEntity.Batch ?? 0,
                        IsUsed = YesOrNoEnum.No,
                        Status = status,
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
                        ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                        ProductBOMId = planWorkOrderEntity.ProductBOMId,
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
                        Qty = procMaterialEntity.Batch ?? 0,
                        ProcedureId = procProcedureEntity.Id,
                        Status = status,
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
                        Qty = procMaterialEntity.Batch ?? 0,
                        ProcedureId = procProcedureEntity.Id,
                        Operatetype = ManuSfcStepTypeEnum.Create,
                        CurrentStatus = SfcStatusEnum.lineUp,
                        CreatedBy = param.UserName!,
                        UpdatedBy = param.UserName
                    };
                    manuSfcStepList.Add(manuSfcStepEntity);

                    if (param.IsInActive)
                    {
                        manuSfcStepList.Add(
                            new ManuSfcStepEntity
                            {
                                Id = IdGenProvider.Instance.CreateId(),
                                SiteId = param.SiteId,
                                SFC = sfc,
                                ProductId = productId,
                                WorkOrderId = planWorkOrderEntity.Id,
                                ProductBOMId = planWorkOrderEntity.ProductBOMId,
                                WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                                Qty = procMaterialEntity.Batch ?? 0,
                                ProcedureId = procProcedureEntity.Id,
                                Operatetype = ManuSfcStepTypeEnum.InStock,
                                CurrentStatus = SfcStatusEnum.lineUp,
                                CreatedBy = param.UserName!,
                                UpdatedBy = param.UserName
                            }
                            );
                    }
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
                var row = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderIdAsync(new UpdatePassDownQuantityCommand
                {
                    WorkOrderId = planWorkOrderEntity.Id,
                    PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100),
                    PassDownQuantity = qty,
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
        /// 条码生成（半成品）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<ManuSfcEntity>> CreateBarcodeBySemiProductIdAsync(CreateBarcodeByResourceCode param)
        {
            // 查询资源
            var resourceEntity = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = param.SiteId,
                Code = param.ResourceCode,
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19603)).WithData("Code", param.ResourceCode);
            var inteWorkCenterEntity = await _inteWorkCenterRepository.GetByResourceIdAsync(resourceEntity.Id);
            if (inteWorkCenterEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19606)).WithData("Code", param.ResourceCode);
            }
            ////半成品生成条码 通过上料记录来查询资源绑定的工单
            //var fr = await _manuFeedingRepository.GetByResourceIdAndMaterialIdsWithOutZeroAsync(new Data.Repositories.Manufacture.ManuFeeding.Query.GetByResourceIdAndMaterialIdsQuery()
            //{
            //    ResourceId = resourceEntity.Id,
            //    MaterialIds = null
            //}) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19604)).WithData("ResourceCode", param.ResourceCode);

            var wob = await _planWorkOrderBindRepository.GetByResourceIDAsync(new PlanWorkOrderBindByResourceIdQuery
            {
                ResourceId = resourceEntity.Id,

                SiteId = param.SiteId,
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19928)).WithData("ResCode", resourceEntity.ResCode);
            //throw new CustomerValidationException(nameof(ErrorCode.MES19136)).WithData("ResourceCode", param.ResourceCode); ;
            //var foo = wob.FirstOrDefault(f => f.WorkOrderId != null && f.WorkOrderId != 0) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19604)).WithData("ResourceCode", param.ResourceCode);

            var wo = await _planWorkOrderRepository.GetByIdAsync(wob.WorkOrderId);
            //?? throw new CustomerValidationException(nameof(ErrorCode.MES19929));//.WithData("ResourceCode", param.ResourceCode); ; ;
            var proc = await _procProcedureRepository.GetProcProduresByResourceIdAsync(new ProcProdureByResourceIdQuery
            {
                SiteId = param.SiteId,
                ResourceId = resourceEntity.Id,
            });

            // 资源绑定多个工序时，取第一个
            var procedureId = proc.FirstOrDefault()!.Id;

            // 获取产出设置的产品ID
            var productIdOfSet = await _masterDataService.GetProductSetIdAsync(new ProductSetBo
            {
                SiteId = param.SiteId,
                ProductId = wo.ProductId,
                ProcedureId = procedureId,
                ResourceId = resourceEntity.Id,
            });

            // 产品ID
            var productId = productIdOfSet ?? wo.ProductId;
            var mo = await _procMaterialRepository.GetByIdAsync(productId);

            var planWorkOrderEntity = await _masterDataService.GetProduceWorkOrderByIdAsync(new WorkOrderIdBo
            {
                WorkOrderId = wo.Id,
                IsVerifyActivation = false
            });
            // var processRouteFirstProcedure = await _masterDataService.GetFirstProcedureAsync(planWorkOrderEntity.ProcessRouteId);


            var inteCodeRulesEntity = await _inteCodeRulesRepository.GetInteCodeRulesByProductIdAsync(new InteCodeRulesByProductQuery
            {
                ProductId = productId,
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
                InteWorkCenterId = inteWorkCenterEntity.Id
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
                        Qty = mo.Batch ?? 0,
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
                        ProductId = productId,
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
                        ProductId = productId,
                        WorkOrderId = planWorkOrderEntity.Id,
                        BarCodeInfoId = sfcInfoId,
                        ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                        WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                        ProductBOMId = planWorkOrderEntity.ProductBOMId,
                        Qty = mo.Batch ?? 0,
                        ProcedureId = procedureId,
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
                        ProductId = productId,
                        WorkOrderId = planWorkOrderEntity.Id,
                        ProductBOMId = planWorkOrderEntity.ProductBOMId,
                        WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                        Qty = mo.Batch ?? 0,
                        ProcedureId = procedureId,
                        Operatetype = ManuSfcStepTypeEnum.Create,
                        CurrentStatus = SfcStatusEnum.lineUp,
                        CreatedBy = param.UserName,
                        UpdatedBy = param.UserName
                    });
                }
            }
            if (productId == planWorkOrderEntity.ProductId)
            {
                var row = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderIdAsync(new UpdatePassDownQuantityCommand
                {
                    WorkOrderId = planWorkOrderEntity.Id,
                    PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100),
                    PassDownQuantity = manuSfcList.Sum(x => x.Qty),
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

            return manuSfcList;
        }

        /// <summary>
        /// 条码生成（电芯）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<ManuSfcEntity>> CreateCellBarCodeAsync(CreateBarcodeByResourceCode param)
        {
            // TODO
            return await Task.FromResult(new List<ManuSfcEntity> { });
        }

        /// <summary>
        /// 根据极组码生成电芯码
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<string> CreateCellBarCodeBySfcAsync(CreateCellBarcodeBo bo)
        {
            //查询极组在制信息
            var manuSfcProduceEntities = await _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery
            {
                Sfcs = bo.Barcodes,
                SiteId = bo.SiteId
            });
            if (manuSfcProduceEntities == null || !manuSfcProduceEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', bo.Barcodes));
            }
            // 是否有不属于在制品表的条码
            var notIncludeSFCs = bo.Barcodes.Except(manuSfcProduceEntities.Select(s => s.SFC));
            if (notIncludeSFCs != null && notIncludeSFCs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(",", notIncludeSFCs));
            }
            //校验极组当前所在工序
            if (manuSfcProduceEntities.Any(x => x.ProcedureId != bo.ProcedureId))
            {
                var polarSfcs = manuSfcProduceEntities.Where(x => x.ProcedureId != bo.ProcedureId).Select(x => x.SFC);
                throw new CustomerValidationException(nameof(ErrorCode.MES16513)).WithData("SFC", string.Join(',', polarSfcs));
            }

            //任意取一个极组信息
            var polarSfcProduceEntity = manuSfcProduceEntities.First();
            var workorderId = polarSfcProduceEntity.WorkOrderId;
            var productId = polarSfcProduceEntity.ProductId;
            var workCenterId = polarSfcProduceEntity.WorkCenterId;

            //按编码规则生成电芯码
            var materialCode = (await _procMaterialRepository.GetByIdAsync(productId))?.MaterialCode ?? "";
            var inteCodeRule = await _inteCodeRulesRepository.GetInteCodeRulesByProductIdAsync(new InteCodeRulesByProductQuery
            {
                CodeType = CodeRuleCodeTypeEnum.ProcessControlSeqCode,
                ProductId = productId,
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16501)).WithData("product", materialCode);

            var barcodes = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new Bos.Manufacture.ManuGenerateBarcode.GenerateBarcodeBo
            {
                SiteId = bo.SiteId,
                CodeRuleId = inteCodeRule.Id,
                Count = 1,
                Sfcs = bo.Barcodes,
                ProductId = productId,
                WorkOrderId = workorderId,
                InteWorkCenterId = workCenterId,
                UserName = bo.UserName
            });
            if (barcodes == null || !barcodes.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16200));
            }

            var cellSFC = barcodes.First();

            //校验电芯码长度
            if (cellSFC.Length != 24)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16512)).WithData("SFC", cellSFC);
            }

            #region 组装数据

            var sfcEntity = new ManuSfcEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = bo.SiteId,
                SFC = cellSFC,
                Qty = 1,
                IsUsed = YesOrNoEnum.Yes,
                Status = bo.IsInStock ? SfcStatusEnum.Activity : SfcStatusEnum.lineUp,
                Type = SfcTypeEnum.Produce,
                CreatedBy = bo.UserName,
                UpdatedBy = bo.UserName
            };
            var sfcInfoEntity = new ManuSfcInfoEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = bo.SiteId,
                SfcId = sfcEntity.Id,
                WorkOrderId = workorderId,
                ProductId = productId,
                ProductBOMId = polarSfcProduceEntity.ProductBOMId,
                ProcessRouteId = polarSfcProduceEntity.ProcessRouteId,
                IsUsed = true,
                CreatedBy = bo.UserName,
                UpdatedBy = bo.UserName
            };
            var sfcProduceEntity = new ManuSfcProduceEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = bo.SiteId,
                SFC = cellSFC,
                SFCId = sfcEntity.Id,
                ProductId = productId,
                WorkOrderId = workorderId,
                BarCodeInfoId = sfcInfoEntity.Id,
                ResourceId = bo.ResourceId,
                EquipmentId = bo.EquipmentId,
                ProcessRouteId = polarSfcProduceEntity.ProcessRouteId,
                WorkCenterId = polarSfcProduceEntity.WorkCenterId,
                ProductBOMId = polarSfcProduceEntity.ProductBOMId,
                Qty = 1,
                ProcedureId = bo.ProcedureId,
                Status = bo.IsInStock ? SfcStatusEnum.Activity : SfcStatusEnum.lineUp,
                RepeatedCount = 0,
                IsScrap = TrueOrFalseEnum.No,
                CreatedBy = bo.UserName,
                UpdatedBy = bo.UserName
            };
            //步骤表数据
            var sfcStepEntities = new List<ManuSfcStepEntity>
            {
                //电芯码下达步骤
                new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = bo.SiteId,
                    SFC = cellSFC,
                    ProductId = polarSfcProduceEntity.ProductId,
                    WorkOrderId = polarSfcProduceEntity.WorkOrderId,
                    ProductBOMId = polarSfcProduceEntity.ProductBOMId,
                    WorkCenterId = polarSfcProduceEntity.WorkCenterId,
                    EquipmentId = bo.EquipmentId,
                    ResourceId = bo.ResourceId,
                    Qty = 1,
                    ProcedureId = bo.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.Create,
                    CurrentStatus = SfcStatusEnum.lineUp,
                    CreatedBy = bo.UserName,
                    UpdatedBy = bo.UserName
                }
            };
            if (bo.IsInStock)
            {
                //电芯码进站步骤
                sfcStepEntities.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = bo.SiteId,
                    SFC = cellSFC,
                    ProductId = polarSfcProduceEntity.ProductId,
                    WorkOrderId = polarSfcProduceEntity.WorkOrderId,
                    ProductBOMId = polarSfcProduceEntity.ProductBOMId,
                    WorkCenterId = polarSfcProduceEntity.WorkCenterId,
                    EquipmentId = bo.EquipmentId,
                    ResourceId = bo.ResourceId,
                    Qty = 1,
                    ProcedureId = bo.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.InStock,
                    CurrentStatus = SfcStatusEnum.lineUp,
                    AfterOperationStatus = SfcStatusEnum.Activity,
                    CreatedBy = bo.UserName,
                    UpdatedBy = bo.UserName
                });
            }
            //极组码完成步骤
            foreach (var item in manuSfcProduceEntities)
            {
                sfcStepEntities.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = item.SiteId,
                    SFC = item.SFC,
                    ProductId = item.ProductId,
                    WorkOrderId = item.WorkOrderId,
                    ProductBOMId = item.ProductBOMId,
                    WorkCenterId = item.WorkCenterId,
                    EquipmentId = bo.EquipmentId,
                    ResourceId = bo.ResourceId,
                    Qty = item.Qty,
                    ProcedureId = item.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.OutStock,
                    CurrentStatus = item.Status,
                    AfterOperationStatus = SfcStatusEnum.Complete,
                    CreatedBy = bo.UserName,
                    UpdatedBy = bo.UserName
                });
            }

            //流转记录
            var manuSfcCirculationEntitys = new List<ManuSfcCirculationEntity>();
            foreach (var item in bo.Barcodes)
            {
                manuSfcCirculationEntitys.Add(new ManuSfcCirculationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = bo.SiteId,
                    ProcedureId = bo.ProcedureId,
                    ResourceId = bo.ResourceId,
                    SFC = item,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    ProductId = sfcProduceEntity.ProductId,
                    CirculationBarCode = cellSFC,
                    CirculationProductId = sfcProduceEntity.ProductId,
                    CirculationMainProductId = sfcProduceEntity.ProductId,
                    CirculationQty = sfcProduceEntity.Qty,
                    CirculationType = SfcCirculationTypeEnum.Merge,
                    CreatedBy = bo.UserName,
                    UpdatedBy = bo.UserName
                });
            }

            #endregion

            //数据库操作
            using var trans = TransactionHelper.GetTransactionScope();
            await _manuSfcRepository.InsertAsync(sfcEntity);
            await _manuSfcInfoRepository.InsertAsync(sfcInfoEntity);
            await _manuSfcProduceRepository.InsertAsync(sfcProduceEntity);
            await _manuSfcStepRepository.InsertRangeAsync(sfcStepEntities);
            if (manuSfcCirculationEntitys.Any())
            {
                await _manuSfcCirculationRepository.InsertRangeAsync(manuSfcCirculationEntitys);
            }
            //更新极组条码为完成状态
            await _manuSfcRepository.UpdateStatusAsync(new ManuSfcUpdateCommand
            {
                SiteId = bo.SiteId,
                Sfcs = bo.Barcodes.ToArray(),
                Status = Core.Enums.SfcStatusEnum.Complete,
                UpdatedOn = HymsonClock.Now(),
                UserId = bo.UserName
            });
            trans.Complete();

            return cellSFC;
        }

    }
}
