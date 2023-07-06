using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Manufacture.ManuCreateBarcode;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 条码接收
    /// </summary>
    public class BarcodeReceiveService : IJobService
    {
        /// <summary>
        /// 条码接收 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IManuCreateBarcodeService _manuCreateBarcodeService;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IMasterDataService _masterDataService;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuContainerPackRepository _manuContainerPackRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IPlanWorkOrderBindRepository _planWorkOrderBindRepository;

        public BarcodeReceiveService(
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuSfcRepository manuSfcRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            ILocalizationService localizationService,
            IManuCreateBarcodeService manuCreateBarcodeService,
           IProcMaterialRepository procMaterialRepository,
           IMasterDataService masterDataService,
           IManuSfcInfoRepository manuSfcInfoRepository,
           IManuContainerPackRepository manuContainerPackRepository,
           IManuSfcProduceRepository manuSfcProduceRepository,
           IPlanWorkOrderBindRepository planWorkOrderBindRepository)
        {
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcRepository = manuSfcRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _localizationService = localizationService;
            _manuCreateBarcodeService = manuCreateBarcodeService;
            _procMaterialRepository = procMaterialRepository;
            _masterDataService = masterDataService;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuContainerPackRepository = manuContainerPackRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _planWorkOrderBindRepository= planWorkOrderBindRepository;
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>00
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<BarcodeSfcReceiveBo>();
            if (bo == null) return default;
            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, new MultiSFCBo {SiteId= param.SiteId,SFCs= param .SFCs});
            if (sfcProduceEntities == null || sfcProduceEntities.Any() == false)
            {

            }
            else
            {
                //获取绑定工单
                var planWorkOrderBindEntity = await _planWorkOrderBindRepository.GetByResourceIDAsync(new PlanWorkOrderBindByResourceIdQuery
                {
                    SiteId = bo.SiteId,
                    ResourceId = bo.ResourceId
                });

                if (planWorkOrderBindEntity == null)
                {
                    throw new  BusinessException(nameof(ErrorCode.MES16306));
                }
                var manuSfcProduceEntity =  await _masterDataService.GetWorkOrderByIdAsync(planWorkOrderBindEntity.WorkOrderId);

                //获取首工序
                var firstProcedure = _masterDataService.GetFirstProcedureAsync(manuSfcProduceEntity.ProcessRouteId);

                //获取bom TODO BOM逻辑比较牵强
                var bomMaterials = await _masterDataService.GetProcMaterialEntitiesByBomIdAndProcedureIdAsync(manuSfcProduceEntity.ProductBOMId, manuSfcProduceEntity.ProcessRouteId);

                // 获取库存数据
               var whMaterialInventorys= await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
                {
                    SiteId = bo.SiteId,
                    BarCodes = bo.SFCs
                });

                foreach (var sfc in bo.SFCs)
                { 
                  // 试探条码物料
                  
                }
            }
            return null;
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            return await Task.FromResult(new JobResponseBo { });
        }
    }
}
