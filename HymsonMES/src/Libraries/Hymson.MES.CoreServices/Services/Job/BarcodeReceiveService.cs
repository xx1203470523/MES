using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Common.MasterData;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.Snowflake;
using Hymson.Utils;

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
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IMasterDataService _masterDataService;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IPlanWorkOrderBindRepository _planWorkOrderBindRepository;
        private readonly IManuCommonService _manuCommonService;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        public BarcodeReceiveService(
           IPlanWorkOrderRepository planWorkOrderRepository,
           IManuSfcRepository manuSfcRepository,
           IWhMaterialInventoryRepository whMaterialInventoryRepository,
           ILocalizationService localizationService,
           IProcMaterialRepository procMaterialRepository,
           IMasterDataService masterDataService,
           IManuSfcInfoRepository manuSfcInfoRepository,
           IManuSfcProduceRepository manuSfcProduceRepository,
           IPlanWorkOrderBindRepository planWorkOrderBindRepository,
           IManuCommonService manuCommonService,
           IManuSfcStepRepository manuSfcStepRepository)
        {
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcRepository = manuSfcRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _localizationService = localizationService;
            _procMaterialRepository = procMaterialRepository;
            _masterDataService = masterDataService;
            _manuSfcInfoRepository = manuSfcInfoRepository;
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
        /// 执行前节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<InStationRequestBo>();
            if (bo == null) return null;
            return await _masterDataService.GetJobRalationJobByProcedureIdOrResourceIdAsync(new Bos.Common.MasterData.JobRelationBo
            {
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
            });
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

            var sfcProduceEntities = await bo.Proxy!.GetDataBaseValueAsync(
               _masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, new MultiSFCBo { SiteId = param.SiteId, SFCs = param.SFCs }
                );

            var sfcEntitys = await bo.Proxy.GetDataBaseValueAsync(
              _manuSfcRepository.GetManuSfcEntitiesAsync, new ManuSfcQuery { SiteId = param.SiteId, SFCs = param.SFCs }
               );

            //获取绑定工单
            var planWorkOrderBindEntity = await _planWorkOrderBindRepository.GetByResourceIDAsync(new PlanWorkOrderBindByResourceIdQuery
            {
                SiteId = bo.SiteId,
                ResourceId = bo.ResourceId
            });

            if (planWorkOrderBindEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16306));
            }

            var planWorkOrderEntity = await _masterDataService.GetWorkOrderByIdAsync(planWorkOrderBindEntity.WorkOrderId);

            //获取首工序
            var productId = await _masterDataService.GetProductSetIdAsync(new ProductSetBo { SiteId = bo.SiteId, ProductId = planWorkOrderEntity.ProductId, ProcedureId = bo.ProcedureId, ResourceId = bo.ResourceId }) ?? planWorkOrderEntity.ProductId;

            var boms = await _masterDataService.GetProcMaterialEntitiesByBomIdAndProcedureIdAsync(planWorkOrderEntity.ProductBOMId, bo.ProcedureId);
            var bomMaterials = GetBomMaterials(boms);

            // 获取库存数据
            var whMaterialInventorys = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
            {
                SiteId = bo.SiteId,
                BarCodes = bo.SFCs
            });
            var validationFailures = new List<ValidationFailure>();
            List<ManuSfcEntity> manuSfcList = new List<ManuSfcEntity>();
            List<ManuSfcEntity> updateManuSfcList = new List<ManuSfcEntity>();
            List<ManuSfcInfoEntity> manuSfcInfoList = new List<ManuSfcInfoEntity>();
            List<ManuSfcProduceEntity> manuSfcProduceList = new List<ManuSfcProduceEntity>();
            List<ManuSfcStepEntity> manuSfcStepList = new List<ManuSfcStepEntity>();
            ManuSfcInfoUpdateIsUsedBo manuSfcInfoUpdateIsUsedBo = new ManuSfcInfoUpdateIsUsedBo()
            {
                UserId = bo.UserName,
                UpdatedOn = HymsonClock.Now(),
                SfcIds=new List<long>()
            };
            foreach (var sfc in bo.SFCs)
            {
                if (sfcProduceEntities != null && sfcProduceEntities.Any(x => x.SFC == sfc)) continue;

                var whMaterialInventory = whMaterialInventorys.FirstOrDefault(x => x.MaterialBarCode == sfc);

                BomMaterial? material = null;
                decimal qty = 0;
                //不存在库存中 则使用bom清单试探
                if (whMaterialInventory == null)
                {
                    // 循环匹配掩码规则来确认接收物料
                    foreach (var bom in bomMaterials.Where(x => x.DataCollectionWay == MaterialSerialNumberEnum.Outside))
                    {
                        if (await _manuCommonService.CheckBarCodeByMaskCodeRuleAsync(sfc, bom.MaterialId))
                        {
                            material = bom;
                            break;
                        }
                    }
                    if (material != null)
                    {
                        var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(material.MaterialId);
                        qty = procMaterialEntity.Batch;
                        if (qty == 0)
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
                                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", planWorkOrderEntity.OrderCode);
                            }
                            validationFailure.FormattedMessagePlaceholderValues.Add("MaterialCode", procMaterialEntity.MaterialCode);
                            validationFailure.ErrorCode = nameof(ErrorCode.MES16137);
                            validationFailures.Add(validationFailure);
                            continue;
                        }
                    }
                }
                else
                {
                    var whMaterial = bomMaterials.FirstOrDefault(x => x.MaterialId == whMaterialInventory.MaterialId);
                    if (whMaterial != null)
                    {
                        if (whMaterial.DataCollectionWay == MaterialSerialNumberEnum.Inside)
                        {
                            // 报错 内部条码  bom属性为外部

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
                            validationFailure.ErrorCode = nameof(ErrorCode.MES16134);
                            validationFailures.Add(validationFailure);
                            continue;
                        }
                        qty = whMaterialInventory.QuantityResidue;

                        if (qty == 0)
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
                                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", planWorkOrderEntity.OrderCode);
                            }
                            validationFailure.ErrorCode = nameof(ErrorCode.MES16136);
                            validationFailures.Add(validationFailure);
                            continue;
                        }
                    }
                }

                if (material == null)
                {
                    //报错  不在bom中
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                            };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", planWorkOrderEntity.OrderCode);
                    }
                    validationFailure.FormattedMessagePlaceholderValues.Add("WorkOrder", sfc);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES16135);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                //准备接收数据
                var manuSfcEntity = sfcEntitys?.FirstOrDefault(x => x.SFC == sfc);
                if (manuSfcEntity == null)
                {
                    manuSfcEntity = new ManuSfcEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = bo.SiteId,
                        SFC = sfc,
                        Qty = qty,
                        IsUsed = YesOrNoEnum.No,
                        Status = SfcStatusEnum.InProcess,
                        CreatedBy = bo.UserName,
                        UpdatedBy = bo.UserName
                    };
                    manuSfcList.Add(manuSfcEntity);
                }
                else
                {
                    manuSfcEntity.IsUsed = YesOrNoEnum.No;
                    manuSfcEntity.Status = SfcStatusEnum.InProcess;
                    manuSfcEntity.UpdatedBy = bo.UserName;
                    manuSfcEntity.UpdatedOn = HymsonClock.Now();
                    updateManuSfcList.Add(manuSfcEntity);
                    manuSfcInfoUpdateIsUsedBo.SfcIds.Add(manuSfcEntity.Id);
                }

                manuSfcInfoList.Add(new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = bo.SiteId,
                    SfcId = manuSfcEntity.Id,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductId = productId,
                    IsUsed = true,
                    CreatedBy = bo.UserName,
                    UpdatedBy = bo.UserName
                });

                manuSfcProduceList.Add(new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = bo.SiteId,
                    SFC = sfc,
                    SFCId = manuSfcEntity.Id,
                    ProductId = productId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    BarCodeInfoId = manuSfcEntity.Id,
                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    Qty = qty,
                    ProcedureId = bo.ProcedureId,
                    Status = SfcProduceStatusEnum.lineUp,
                    RepeatedCount = 0,
                    IsScrap = TrueOrFalseEnum.No,
                    CreatedBy = bo.UserName,
                    UpdatedBy = bo.UserName
                });

                manuSfcStepList.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = bo.SiteId,
                    SFC = sfc,
                    ProductId = productId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    Qty = qty,
                    ProcedureId = bo.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.Receive,
                    CurrentStatus = SfcProduceStatusEnum.lineUp,
                    CreatedBy = bo.UserName,
                    UpdatedBy = bo.UserName
                });
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }

            return new BarcodeSfcReceiveResponseBo
            {
                WorkOrderId = planWorkOrderEntity.Id,
                PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100),
                PassDownQuantity = manuSfcProduceList.Sum(x => x.Qty),
                UserName = bo.UserName,
                ManuSfcInfoUpdateIsUsed= manuSfcInfoUpdateIsUsedBo,
                ManuSfcList = manuSfcList,
                UpdateManuSfcList= updateManuSfcList,
                ManuSfcInfoList = manuSfcInfoList,
                ManuSfcProduceList= manuSfcProduceList,
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
            JobResponseBo responseBo = new();
            if (obj is not BarcodeSfcReceiveResponseBo data) return responseBo;

            var row = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderId(new UpdatePassDownQuantityCommand
            {
                WorkOrderId = data.WorkOrderId,
                PlanQuantity = data.PlanQuantity,
                PassDownQuantity = data.PlanQuantity,
                UserName = data.UserName,
                UpdateDate = HymsonClock.Now()
            });

            if (row == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16503)).WithData("workorder", data.OrderCode);
            }
            if (data.ManuSfcList != null && data.ManuSfcList.Any())
            {
                await _manuSfcRepository.InsertRangeAsync(data.ManuSfcList);
            }
            if (data.UpdateManuSfcList != null && data.UpdateManuSfcList.Any())
            {
                await _manuSfcRepository.UpdateRangeAsync(data.UpdateManuSfcList);
            }
            if (data.ManuSfcInfoUpdateIsUsed.SfcIds != null && data.ManuSfcInfoUpdateIsUsed.SfcIds.Any())
            {
                await _manuSfcInfoRepository.UpdatesIsUsedAsync(new ManuSfcInfoUpdateIsUsedCommand()
                {
                    UpdatedOn= data.ManuSfcInfoUpdateIsUsed.UpdatedOn,
                    SfcIds = data.ManuSfcInfoUpdateIsUsed.SfcIds,
                    UserId = data.ManuSfcInfoUpdateIsUsed.UserId,
                });
            }
            if (data.ManuSfcInfoList != null && data.ManuSfcInfoList.Any())
            {
                await _manuSfcInfoRepository.InsertsAsync(data.ManuSfcInfoList);
            }
            if (data.ManuSfcProduceList != null && data.ManuSfcProduceList.Any())
            {
                await _manuSfcProduceRepository.InsertRangeAsync(data.ManuSfcProduceList);
            }
            if (data.ManuSfcStepList != null && data.ManuSfcStepList.Any())
            {
                await _manuSfcStepRepository.InsertRangeAsync(data.ManuSfcStepList);
            }
       
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

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }
    }
}
