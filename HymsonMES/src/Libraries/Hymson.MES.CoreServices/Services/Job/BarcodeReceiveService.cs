using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
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
        private readonly IProcMaterialRepository _procMaterialRepository;
        /// <summary>
        /// 仓储接口（资源维护）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IMasterDataService _masterDataService;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IPlanWorkOrderBindRepository _planWorkOrderBindRepository;
        private readonly IManuCommonService _manuCommonService;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="masterDataService"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="planWorkOrderBindRepository"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuSfcStepRepository"></param>
        public BarcodeReceiveService(IPlanWorkOrderRepository planWorkOrderRepository,
           IManuSfcRepository manuSfcRepository,
           IWhMaterialInventoryRepository whMaterialInventoryRepository,
           IProcMaterialRepository procMaterialRepository,
           IProcResourceRepository procResourceRepository,
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
            _procMaterialRepository = procMaterialRepository;
            _procResourceRepository = procResourceRepository;
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
            if (param is not JobRequestBo commonBo) return default;
            if (commonBo == null) return default;

            return await _masterDataService.GetJobRelationJobByProcedureIdOrResourceIdAsync(new Bos.Common.MasterData.JobRelationBo
            {
                ProcedureId = commonBo.ProcedureId,
                ResourceId = commonBo.ResourceId,
                LinkPoint = ResourceJobLinkPointEnum.BeforeStart
            });
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>00
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return default;
            if (commonBo == null) return default;
            if (commonBo.InStationRequestBos == null || commonBo.InStationRequestBos.Any() == false) return default;

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.InStationRequestBos.Select(s => s.SFC) };

            // 条码在制表
            var sfcProduceEntities = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery
            {
                SiteId = multiSFCBo.SiteId,
                Sfcs = multiSFCBo.SFCs
            });
            //var sfcProduceEntities = await commonBo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, multiSFCBo);

            var sfcEntitys = await commonBo.Proxy!.GetDataBaseValueAsync(_manuSfcRepository.GetManuSfcEntitiesAsync, new ManuSfcQuery { SiteId = multiSFCBo.SiteId, SFCs = multiSFCBo.SFCs });

            var resourceEntity = await _procResourceRepository.GetByIdAsync(commonBo.ResourceId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16337));

            // 获取绑定工单
            var planWorkOrderBindEntity = await _planWorkOrderBindRepository.GetByResourceIDAsync(new PlanWorkOrderBindByResourceIdQuery
            {
                SiteId = commonBo.SiteId,
                ResourceId = commonBo.ResourceId
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19928)).WithData("ResCode", resourceEntity.ResCode);

            var planWorkOrderEntity = await _masterDataService.GetProduceWorkOrderByIdAsync(new WorkOrderIdBo
            {
                WorkOrderId = planWorkOrderBindEntity.WorkOrderId,
                IsVerifyActivation = false
            });

            //获取首工序
            var productId = await _masterDataService.GetProductSetIdAsync(new Bos.Common.MasterData.ProductSetBo { SiteId = commonBo.SiteId, ProductId = planWorkOrderEntity.ProductId, ProcedureId = commonBo.ProcedureId, ResourceId = commonBo.ResourceId }) ?? planWorkOrderEntity.ProductId;

            var boms = await _masterDataService.GetProcMaterialEntitiesByBomIdAndProcedureIdAsync(planWorkOrderEntity.ProductBOMId, commonBo.ProcedureId);
            var bomMaterials = GetBomMaterials(boms);

            // 获取库存数据
            var whMaterialInventorys = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
            {
                SiteId = multiSFCBo.SiteId,
                BarCodes = multiSFCBo.SFCs
            });
            var validationFailures = new List<ValidationFailure>();
            List<ManuSfcEntity> manuSfcList = new List<ManuSfcEntity>();
            List<ManuSfcEntity> updateManuSfcList = new List<ManuSfcEntity>();
            List<ManuSfcInfoEntity> manuSfcInfoList = new List<ManuSfcInfoEntity>();
            List<ManuSfcProduceEntity> manuSfcProduceList = new List<ManuSfcProduceEntity>();
            List<ManuSfcStepEntity> manuSfcStepList = new List<ManuSfcStepEntity>();
            ManuSfcInfoUpdateIsUsedBo manuSfcInfoUpdateIsUsedBo = new ManuSfcInfoUpdateIsUsedBo()
            {
                UserId = commonBo.UserName,
                UpdatedOn = commonBo.Time,
                SfcIds = new List<long>()
            };
            foreach (var sfc in multiSFCBo.SFCs)
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
                        SiteId = commonBo.SiteId,
                        SFC = sfc,
                        Qty = qty,
                        IsUsed = YesOrNoEnum.No,
                        Status = SfcStatusEnum.lineUp,
                        CreatedBy = commonBo.UserName,
                        UpdatedBy = commonBo.UserName
                    };
                    manuSfcList.Add(manuSfcEntity);
                }
                else
                {
                    manuSfcEntity.IsUsed = YesOrNoEnum.No;
                    manuSfcEntity.Status = SfcStatusEnum.lineUp;
                    manuSfcEntity.UpdatedBy = commonBo.UserName;
                    manuSfcEntity.UpdatedOn = HymsonClock.Now();
                    updateManuSfcList.Add(manuSfcEntity);
                    manuSfcInfoUpdateIsUsedBo.SfcIds.Add(manuSfcEntity.Id);
                }

                manuSfcInfoList.Add(new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = commonBo.SiteId,
                    SfcId = manuSfcEntity.Id,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductId = productId,
                    IsUsed = true,
                    CreatedBy = commonBo.UserName,
                    UpdatedBy = commonBo.UserName
                });

                manuSfcProduceList.Add(new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = commonBo.SiteId,
                    SFC = sfc,
                    SFCId = manuSfcEntity.Id,
                    ProductId = productId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    BarCodeInfoId = manuSfcEntity.Id,
                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    Qty = qty,
                    ProcedureId = commonBo.ProcedureId,
                    Status = SfcStatusEnum.lineUp,
                    RepeatedCount = 0,
                    IsScrap = TrueOrFalseEnum.No,
                    CreatedBy = commonBo.UserName,
                    UpdatedBy = commonBo.UserName
                });

                manuSfcStepList.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = commonBo.SiteId,
                    SFC = sfc,
                    ProductId = productId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    Qty = qty,
                    ProcedureId = commonBo.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.Receive,
                    CurrentStatus = SfcStatusEnum.lineUp,
                    CreatedBy = commonBo.UserName,
                    UpdatedBy = commonBo.UserName
                });
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(commonBo.LocalizationService.GetResource("SFCError"), validationFailures);
            }

            /*
            // 手动写进缓存
            var func = _masterDataService.GetProduceEntitiesBySFCsWithCheckAsync;
            var paramString = multiSFCBo.ToSerialize();
            var cacheKey = (uint)$"{func.Method.DeclaringType?.FullName}.{func.Method.Name}{paramString}".GetHashCode();
            commonBo.Proxy.Set(cacheKey, manuSfcProduceList);
            */

            return new BarcodeSfcReceiveResponseBo
            {
                WorkOrderId = planWorkOrderEntity.Id,
                PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100),
                PassDownQuantity = manuSfcProduceList.Sum(x => x.Qty),
                UserName = commonBo.UserName,
                ManuSfcInfoUpdateIsUsed = manuSfcInfoUpdateIsUsedBo,
                ManuSfcList = manuSfcList,
                UpdateManuSfcList = updateManuSfcList,
                ManuSfcInfoList = manuSfcInfoList,
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
                    UpdatedOn = data.ManuSfcInfoUpdateIsUsed.UpdatedOn,
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
            if (param is not JobRequestBo commonBo) return default;
            if (commonBo == null) return default;

            return await _masterDataService.GetJobRelationJobByProcedureIdOrResourceIdAsync(new Bos.Common.MasterData.JobRelationBo
            {
                ProcedureId = commonBo.ProcedureId,
                ResourceId = commonBo.ResourceId,
                LinkPoint = ResourceJobLinkPointEnum.AfterStart
            });
        }
    }
}
