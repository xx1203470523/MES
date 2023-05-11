using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCreateBarcodeDto;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuGenerateBarcodeDto;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.GenerateBarcode;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Linq;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCreateBarcode
{
    /// <summary>
    /// 创建条码
    /// @author wangkeming
    /// @date 2023-03-30
    /// </summary>
    public class ManuCreateBarcodeService : IManuCreateBarcodeService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        private readonly IManuCommonService _manuCommonService;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly ILocalizationService _localizationService;

        public ManuCreateBarcodeService(ICurrentUser currentUser,
             ICurrentSite currentSite,
             IManuCommonService manuCommonService,
             IProcMaterialRepository procMaterialRepository,
             IInteCodeRulesRepository inteCodeRulesRepository,
             IManuGenerateBarcodeService manuGenerateBarcodeService,
             IManuSfcRepository manuSfcRepository,
             IManuSfcInfoRepository manuSfcInfoRepository,
             IManuSfcProduceRepository manuSfcProduceRepository,
             IManuSfcStepRepository manuSfcStepRepository,
             IPlanWorkOrderRepository planWorkOrderRepository,
                ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonService = manuCommonService;
            _procMaterialRepository = procMaterialRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 工单下达条码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task CreateBarcodeByWorkOrderId(CreateBarcodeByWorkOrderDto param)
        {
            var planWorkOrderEntity = await _manuCommonService.GetProduceWorkOrderByIdAsync(param.WorkOrderId);

            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(planWorkOrderEntity.ProductId);
            var inteCodeRulesEntity = await _inteCodeRulesRepository.GetInteCodeRulesByProductIdAsync(planWorkOrderEntity.ProductId);
            if (inteCodeRulesEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16501)).WithData("product", procMaterialEntity.MaterialCode);
            }
            if (procMaterialEntity.Batch == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16502)).WithData("product", procMaterialEntity.MaterialCode);
            }
            var discuss = (int)Math.Ceiling(param.Qty / procMaterialEntity.Batch);
            var barcodeList = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new GenerateBarcodeDto
            {
                CodeRuleId = inteCodeRulesEntity.Id,
                Count = discuss
            });
            var processRouteFirstProcedure = await _manuCommonService.GetFirstProcedureAsync(planWorkOrderEntity.ProcessRouteId);
            List<ManuSfcEntity> manuSfcList = new List<ManuSfcEntity>();
            List<ManuSfcInfoEntity> manuSfcInfoList = new List<ManuSfcInfoEntity>();
            List<ManuSfcProduceEntity> manuSfcProduceList = new List<ManuSfcProduceEntity>();
            List<ManuSfcStepEntity> manuSfcStepList = new List<ManuSfcStepEntity>();
            var issQty = param.Qty;
            foreach (var item in barcodeList)
            {
                var qty = issQty > procMaterialEntity.Batch ? procMaterialEntity.Batch : issQty;
                issQty -= procMaterialEntity.Batch;

                var manuSfcEntity = new ManuSfcEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = item,
                    Qty = qty,
                    IsUsed = YesOrNoEnum.No,
                    Status = SfcStatusEnum.InProcess,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                };
                manuSfcList.Add(manuSfcEntity);

                manuSfcInfoList.Add(new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SfcId = manuSfcEntity.Id,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductId = planWorkOrderEntity.ProductId,
                    IsUsed = true,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                });

                manuSfcProduceList.Add(new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = item,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    BarCodeInfoId = manuSfcEntity.Id,
                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    Qty = qty,
                    ProcedureId = processRouteFirstProcedure.ProcedureId,
                    Status = SfcProduceStatusEnum.lineUp,
                    RepeatedCount = 0,
                    IsScrap = TrueOrFalseEnum.No,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                });

                manuSfcStepList.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = item,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    Qty = qty,
                    ProcedureId = processRouteFirstProcedure.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.Create,
                    CurrentStatus = SfcProduceStatusEnum.lineUp,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                });
            }
            using var ts = TransactionHelper.GetTransactionScope();
            var row = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderId(new UpdatePassDownQuantityCommand
            {
                WorkOrderId = planWorkOrderEntity.Id,
                PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100),
                PassDownQuantity = param.Qty,
                UserName = _currentUser.UserName,
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
        /// 设备请求条码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task CreateBarcodeByResourceId(CreateBarcodeByResourceDto param)
        {

        }

        /// <summary>
        /// 根据外部条码下达条码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task CreateBarcodeByExternalSFC(CreateBarcodeByExternalSFCDto param)
        {
            var planWorkOrderEntity = await _manuCommonService.GetWorkOrderByIdAsync(param.WorkOrderId);
            var sfclist = await _manuSfcRepository.GetBySFCsAsync(param.ExternalSFCs.Select(x => x.SFC));

            var processRouteFirstProcedure = await _manuCommonService.GetFirstProcedureAsync(planWorkOrderEntity.ProcessRouteId);

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
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = item.SFC,
                    Qty = item.Qty,
                    Status = SfcStatusEnum.InProcess,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                };
                manuSfcList.Add(manuSfcEntity);

                manuSfcInfoList.Add(new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SfcId = manuSfcEntity.Id,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductId = planWorkOrderEntity.ProductId,
                    IsUsed = true,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                });

                manuSfcProduceList.Add(new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = item.SFC,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    BarCodeInfoId = manuSfcEntity.Id,
                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    Qty = item.Qty,
                    ProcedureId = processRouteFirstProcedure.ProcedureId,
                    Status = SfcProduceStatusEnum.lineUp,
                    RepeatedCount = 0,
                    IsScrap = TrueOrFalseEnum.No,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                });

                manuSfcStepList.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = item.SFC,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    Qty = item.Qty,
                    ProcedureId = processRouteFirstProcedure.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.Create,
                    CurrentStatus = SfcProduceStatusEnum.lineUp,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                });
            }
            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }
            using var ts = TransactionHelper.GetTransactionScope();
            var row = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderId(new UpdatePassDownQuantityCommand
            {
                WorkOrderId = planWorkOrderEntity.Id,
                PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100),
                PassDownQuantity = param.ExternalSFCs.Sum(x => x.Qty),
                UserName = _currentUser.UserName,
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
        public async Task CreateBarcodeByOldMESSFC(CreateBarcodeByOldMesSFCDto param)
        {
            var planWorkOrderEntity = await _manuCommonService.GetWorkOrderByIdAsync(param.WorkOrderId);
            var sfclist = await _manuSfcRepository.GetBySFCsAsync(param.OldSFCs.Select(x => x.SFC));
            var sfcInfoList = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfclist.Select(x => x.Id));
            var processRouteFirstProcedure = await _manuCommonService.GetFirstProcedureAsync(planWorkOrderEntity.ProcessRouteId);

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
                if (!(sfcEntity.Status == SfcStatusEnum.Complete || sfcEntity.Status == SfcStatusEnum.Received))
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
                sfcEntity.Status = SfcStatusEnum.InProcess;
                sfcEntity.UpdatedBy = _currentUser.UserName;
                sfcEntity.UpdatedOn = HymsonClock.Now();
                manuSfcList.Add(sfcEntity);
                var sfcInfo = sfcInfoList.FirstOrDefault(x => x.SfcId == sfcEntity.Id);
                if (sfcInfo != null)
                {
                    sfcInfo.IsUsed = false;
                    sfcInfo.UpdatedBy = _currentUser.UserName;
                    sfcInfo.UpdatedOn = HymsonClock.Now();
                    updateManuSfcInfoList.Add(sfcInfo);
                }
                manuSfcInfoList.Add(new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SfcId = sfcEntity.Id,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductId = planWorkOrderEntity.ProductId,
                    IsUsed = true,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                });

                manuSfcProduceList.Add(new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = item.SFC,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    BarCodeInfoId = sfcEntity.Id,
                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    Qty = item.Qty,
                    ProcedureId = processRouteFirstProcedure.ProcedureId,
                    Status = SfcProduceStatusEnum.lineUp,
                    RepeatedCount = 0,
                    IsScrap = TrueOrFalseEnum.No,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                });

                manuSfcStepList.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = item.SFC,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    Qty = item.Qty,
                    ProcedureId = processRouteFirstProcedure.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.Create,
                    CurrentStatus = SfcProduceStatusEnum.lineUp,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                });
            }
            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }
            using var ts = TransactionHelper.GetTransactionScope();
            var row = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderId(new UpdatePassDownQuantityCommand
            {
                WorkOrderId = planWorkOrderEntity.Id,
                PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100),
                PassDownQuantity = param.OldSFCs.Sum(x => x.Qty),
                UserName = _currentUser.UserName,
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
    }
}
