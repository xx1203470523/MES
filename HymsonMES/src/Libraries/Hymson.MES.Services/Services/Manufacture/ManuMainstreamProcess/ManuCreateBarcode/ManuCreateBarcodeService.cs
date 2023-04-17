using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
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
             IPlanWorkOrderRepository planWorkOrderRepository)
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
                throw new BusinessException(nameof(ErrorCode.MES16501)).WithData("product", procMaterialEntity.MaterialCode);
            }
            if (procMaterialEntity.Batch == 0)
            {
                throw new BusinessException(nameof(ErrorCode.MES16502)).WithData("product", procMaterialEntity.MaterialCode);
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

            foreach (var item in barcodeList)
            {
                var qty = param.Qty > procMaterialEntity.Batch ? procMaterialEntity.Batch : param.Qty;
                param.Qty -= procMaterialEntity.Batch;

                var manuSfcEntity = new ManuSfcEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = item,
                    Qty = qty,
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
                PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale/100),
                PassDownQuantity = param.Qty,
                UserName = _currentUser.UserName,
                UpdateDate = HymsonClock.Now()
            });

            if (row == 0)
            {
                throw new BusinessException(nameof(ErrorCode.MES16503)).WithData("workorder", planWorkOrderEntity.OrderCode);
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
        /// 条码复用
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task CreateBarcodeByResourceId(CreateBarcodeByOldSFCDto param)
        {

        }
    }
}
