using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Context;
using Hymson.MES.CoreServices.Services.Manufacture.ManuCreateBarcode;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Runtime.CompilerServices;

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
        private readonly IManuCommonService _manuCommonService;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

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
           IPlanWorkOrderBindRepository planWorkOrderBindRepository,
           IManuCommonService manuCommonService,
           IManuSfcStepRepository manuSfcStepRepository)
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
            _planWorkOrderBindRepository = planWorkOrderBindRepository;
            _manuCommonService = manuCommonService;
            _manuSfcStepRepository = manuSfcStepRepository;
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

            IEnumerable<ManuSfcProduceEntity>? sfcProduceEntities = await new List<ManuSfcProduceEntity>().GetProxy<MultiSFCBo, ManuSfcProduceEntity>(
             bo.Proxy, _masterDataService.GetProduceEntitiesBySFCsAsync, new MultiSFCBo { SiteId = param.SiteId, SFCs = param.SFCs }
            );

            IEnumerable<ManuSfcProduceEntity>? sfcProduceEntities1 = await new List<ManuSfcProduceEntity>().GetProxy<MultiSFCBo, ManuSfcProduceEntity>(
           bo.Proxy, _masterDataService.GetProduceEntitiesBySFCsAsync, new MultiSFCBo { SiteId = param.SiteId, SFCs = param.SFCs }
          );

            // 获取生产条码信息
            //if (sfcProducProxy.HasKey)
            //{
            //    sfcProduceEntities = sfcProducProxy;
            //    if (sfcProduceEntities.Any())
            //    {

            //    }
            //}
            //else
            //{
            //    sfcProduceEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, new MultiSFCBo { SiteId = param.SiteId, SFCs = param.SFCs });
            //}



            //获取绑定工单
            //var planWorkOrderBindEntity = await _planWorkOrderBindRepository.GetByResourceIDAsync(new PlanWorkOrderBindByResourceIdQuery
            //{
            //    SiteId = bo.SiteId,
            //    ResourceId = bo.ResourceId
            //});

            //if (planWorkOrderBindEntity == null)
            //{
            //    throw new BusinessException(nameof(ErrorCode.MES16306));
            //}
            //var planWorkOrderEntity = await _masterDataService.GetWorkOrderByIdAsync(planWorkOrderBindEntity.WorkOrderId);

            ////获取首工序
            //var firstProcedure = await _masterDataService.GetFirstProcedureAsync(planWorkOrderEntity.ProcessRouteId);

            ////获取bom TODO BOM逻辑比较牵强
            //var boms = await _masterDataService.GetProcMaterialEntitiesByBomIdAndProcedureIdAsync(planWorkOrderEntity.ProductBOMId, planWorkOrderEntity.ProcessRouteId);
            //var bomMaterials = GetBomMaterials(boms);

            //// 获取库存数据
            //var whMaterialInventorys = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
            //{
            //    SiteId = bo.SiteId,
            //    BarCodes = bo.SFCs
            //});
            var validationFailures = new List<ValidationFailure>();
            List<ManuSfcEntity> manuSfcList = new List<ManuSfcEntity>();
            List<ManuSfcInfoEntity> manuSfcInfoList = new List<ManuSfcInfoEntity>();
            List<ManuSfcInfoEntity> updateManuSfcInfoList = new List<ManuSfcInfoEntity>();
            List<ManuSfcProduceEntity> manuSfcProduceList = new List<ManuSfcProduceEntity>();
            List<ManuSfcStepEntity> manuSfcStepList = new List<ManuSfcStepEntity>();
            manuSfcProduceList.Add(new ManuSfcProduceEntity() { Id= 13072651102011393, SFC = "1234" });
            manuSfcProduceList.Add(new ManuSfcProduceEntity() { SFC = "1456" });
            //foreach (var sfc in bo.SFCs)
            //{
            //    if (sfcProduceEntities != null && sfcProduceEntities.Any(x => x.SFC == sfc)) continue;

            //    var whMaterialInventory = whMaterialInventorys.FirstOrDefault(x => x.MaterialBarCode == sfc);

            //    BomMaterial? material = null;
            //    decimal qty = 0;
            //    //不存在库存中 则使用bom清单试探
            //    if (whMaterialInventory == null)
            //    {
            //        // 循环匹配掩码规则来确认接收物料
            //        foreach (var bom in bomMaterials.Where(x => x.DataCollectionWay == MaterialSerialNumberEnum.Outside))
            //        {
            //            if (await _manuCommonService.CheckBarCodeByMaskCodeRuleAsync(sfc, bom.MaterialId))
            //            {
            //                material = bom;
            //                break;
            //            }
            //        }
            //        if (material != null)
            //        {
            //            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(material.MaterialId);
            //            qty = procMaterialEntity.Batch;
            //        }
            //    }
            //    else
            //    {
            //        var whMaterial = bomMaterials.FirstOrDefault(x => x.MaterialId == whMaterialInventory.MaterialId);
            //        if (whMaterial != null)
            //        {
            //            if (whMaterial.DataCollectionWay == MaterialSerialNumberEnum.Inside)
            //            {
            //                // 报错 内部条码  bom属性为外部
            //                continue;
            //            }
            //            qty = whMaterialInventory.QuantityResidue;
            //        }
            //    }

            //    if (material == null)
            //    {
            //        //报错  不在bom中

            //        continue;
            //    }

            //    if (qty == 0)
            //    {
            //        continue;
            //    }

            //    //准备接收数据
            //    var manuSfcEntity = new ManuSfcEntity
            //    {
            //        Id = IdGenProvider.Instance.CreateId(),
            //        SiteId = bo.SiteId,
            //        SFC = sfc,
            //        Qty = qty,
            //        IsUsed = YesOrNoEnum.Yes,
            //        Status = SfcStatusEnum.InProcess,
            //        CreatedBy = bo.UserName,
            //        UpdatedBy = bo.UserName
            //    };
            //    manuSfcList.Add(manuSfcEntity);

            //    manuSfcInfoList.Add(new ManuSfcInfoEntity
            //    {
            //        Id = IdGenProvider.Instance.CreateId(),
            //        SiteId = bo.SiteId,
            //        SfcId = manuSfcEntity.Id,
            //        WorkOrderId = planWorkOrderEntity.Id,
            //        ProductId = 00,
            //        IsUsed = true,
            //        CreatedBy = bo.UserName,
            //        UpdatedBy = bo.UserName
            //    });

            //    manuSfcProduceList.Add(new ManuSfcProduceEntity
            //    {
            //        Id = IdGenProvider.Instance.CreateId(),
            //        SiteId = bo.SiteId,
            //        SFC = sfc,
            //        SFCId = manuSfcEntity.Id,
            //        ProductId = planWorkOrderEntity.ProductId,
            //        WorkOrderId = planWorkOrderEntity.Id,
            //        BarCodeInfoId = manuSfcEntity.Id,
            //        ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
            //        WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
            //        ProductBOMId = planWorkOrderEntity.ProductBOMId,
            //        Qty = qty,
            //        ProcedureId = firstProcedure.ProcedureId,
            //        Status = SfcProduceStatusEnum.lineUp,
            //        RepeatedCount = 0,
            //        IsScrap = TrueOrFalseEnum.No,
            //        CreatedBy = bo.UserName,
            //        UpdatedBy = bo.UserName
            //    });

            //    manuSfcStepList.Add(new ManuSfcStepEntity
            //    {
            //        Id = IdGenProvider.Instance.CreateId(),
            //        SiteId = bo.SiteId,
            //        SFC = sfc,
            //        ProductId = planWorkOrderEntity.ProductId,
            //        WorkOrderId = planWorkOrderEntity.Id,
            //        ProductBOMId = planWorkOrderEntity.ProductBOMId,
            //        WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
            //        Qty = qty,
            //        ProcedureId = firstProcedure.ProcedureId,
            //        Operatetype = ManuSfcStepTypeEnum.Receive,
            //        CurrentStatus = SfcProduceStatusEnum.lineUp,
            //        CreatedBy = bo.UserName,
            //        UpdatedBy = bo.UserName
            //    });
            //}

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }

            return new BarcodeSfcReceiveResponseBo
            {
                //WorkOrderId = planWorkOrderEntity.Id,
                //PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100),
                PassDownQuantity = manuSfcProduceList.Sum(x => x.Qty),
                UserName = bo.UserName,
                ManuSfcList = manuSfcList,
                ManuSfcInfoList = manuSfcInfoList,
                UpdateManuSfcInfoList = updateManuSfcInfoList,
                ManuSfcProduceList = manuSfcProduceList,
                ManuSfcStepList = manuSfcStepList,
            };
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            //JobResponseBo responseBo = new();
            //if (obj is not BarcodeSfcReceiveResponseBo data) return responseBo;

            //var row = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderId(new UpdatePassDownQuantityCommand
            //{
            //    WorkOrderId = data.WorkOrderId,
            //    PlanQuantity = data.PlanQuantity,
            //    PassDownQuantity = data.PlanQuantity,
            //    UserName = data.UserName,
            //    UpdateDate = HymsonClock.Now()
            //});

            //if (row == 0)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES16503)).WithData("workorder", data.OrderCode);
            //}
            //if (data.ManuSfcList != null && data.ManuSfcList.Any())
            //{
            //    await _manuSfcRepository.InsertRangeAsync(data.ManuSfcList);
            //}
            //if (data.ManuSfcInfoList != null && data.ManuSfcInfoList.Any())
            //{
            //    await _manuSfcInfoRepository.InsertsAsync(data.ManuSfcInfoList);
            //}
            //if (data.ManuSfcProduceList != null && data.ManuSfcProduceList.Any())
            //{
            //    await _manuSfcProduceRepository.InsertRangeAsync(data.ManuSfcProduceList);
            //}
            //if (data.ManuSfcStepList != null && data.ManuSfcStepList.Any())
            //{
            //    await _manuSfcStepRepository.InsertRangeAsync(data.ManuSfcStepList);
            //}
            return await Task.FromResult(new JobResponseBo { });
        }

        /// <summary>
        /// 解析boms
        /// </summary>
        /// <param name="boms"></param>
        /// <returns></returns>
        private IEnumerable<BomMaterial> GetBomMaterials(IEnumerable<BomMaterialBo> boms)
        {
            List<BomMaterial> list = new List<BomMaterial>();
            foreach (var bom in boms)
            {
                list.Add(new BomMaterial
                {
                    MaterialId = bom.MaterialId,
                    DataCollectionWay = bom.DataCollectionWay,
                });
                foreach (var bomMaterial in bom.BomMaterials)
                {
                    list.Add(new BomMaterial
                    {
                        MaterialId = bomMaterial.MaterialId,
                        DataCollectionWay = bomMaterial.DataCollectionWay,
                    });
                }
                foreach (var procMaterial in bom.ProcMaterials)
                {
                    list.Add(new BomMaterial
                    {
                        MaterialId = procMaterial.MaterialId,
                        DataCollectionWay = procMaterial.DataCollectionWay,
                    });
                }
            }
            return list;
        }
    }
}
