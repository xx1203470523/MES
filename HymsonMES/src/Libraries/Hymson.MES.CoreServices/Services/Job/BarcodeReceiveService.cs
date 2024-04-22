using Dapper;
using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.Snowflake;
using Hymson.Utils;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 条码接收
    /// </summary>
    [Job("条码接收", JobTypeEnum.Standard)]
    public class BarcodeReceiveService : IJobService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<BarcodeReceiveService> _logger;

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
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
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
        public BarcodeReceiveService(ILogger<BarcodeReceiveService> logger,
            IPlanWorkOrderRepository planWorkOrderRepository,
           IManuSfcRepository manuSfcRepository,
           IWhMaterialInventoryRepository whMaterialInventoryRepository,
           IProcMaterialRepository procMaterialRepository,
           IProcResourceRepository procResourceRepository,
           IMasterDataService masterDataService,
           IManuSfcInfoRepository manuSfcInfoRepository,
           IManuSfcProduceRepository manuSfcProduceRepository,
           IPlanWorkOrderBindRepository planWorkOrderBindRepository,
           IManuCommonService manuCommonService,
           IManuSfcStepRepository manuSfcStepRepository,
           IPlanWorkOrderActivationRepository planWorkOrderActivationRepository)
        {
            _logger = logger;
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
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
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
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return default;
            if (commonBo == null) return default;
            if (commonBo.InStationRequestBos == null || !commonBo.InStationRequestBos.Any()) return default;

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.InStationRequestBos.Select(s => s.SFC) };

            // 条码在制表
            var sfcProduceEntities = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery
            {
                SiteId = multiSFCBo.SiteId,
                Sfcs = multiSFCBo.SFCs
            });

            var sfcEntitys = await commonBo.Proxy!.GetDataBaseValueAsync(_manuSfcRepository.GetListAsync, new ManuSfcQuery
            {
                SiteId = multiSFCBo.SiteId,
                SFCs = multiSFCBo.SFCs,
                Type = SfcTypeEnum.Produce
            });

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

            var planWorkOrderActivationEntity = await _planWorkOrderActivationRepository.GetByWorkOrderIdAsync(planWorkOrderBindEntity.WorkOrderId);
            if (planWorkOrderActivationEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19937)).WithData("WorkOrderCode", planWorkOrderEntity.OrderCode);
            }

            // 获取产出设置的产品ID
            var productIdOfSet = await _masterDataService.GetProductSetIdAsync(new ProductSetBo
            {
                SiteId = commonBo.SiteId,
                ProductId = planWorkOrderEntity.ProductId,
                ProcedureId = commonBo.ProcedureId,
                ResourceId = commonBo.ResourceId
            });

            // 产品ID
            var productId = productIdOfSet ?? planWorkOrderEntity.ProductId;

            // 组合物料数据（放缓存）
            var initialMaterials = await commonBo.Proxy.GetValueAsync(_masterDataService.GetInitialMaterialsAsync, new MaterialDeductRequestBo
            {
                SiteId = commonBo.SiteId,
                ProcedureId = commonBo.ProcedureId,
                ProductBOMId = planWorkOrderEntity.ProductBOMId
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16133));

            // 平铺物料数据
            var bomMaterials = initialMaterials.Select(s => new MaterialDeductItemBo { MaterialId = s.MaterialId, DataCollectionWay = s.DataCollectionWay }).AsList();
            bomMaterials.AddRange(initialMaterials.SelectMany(s => s.ReplaceMaterials).Select(s => new MaterialDeductItemBo { MaterialId = s.MaterialId, DataCollectionWay = s.DataCollectionWay }));

            // 获取库存数据
            var whMaterialInventorys = await _whMaterialInventoryRepository.GetByBarCodesOfHasQtyAsync(new WhMaterialInventoryBarCodesQuery
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
            List<UpdateWhMaterialInventoryEmptyByIdCommand> updateWhMaterialInventoryEmptyByIdCommands = new List<UpdateWhMaterialInventoryEmptyByIdCommand>();
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

                MaterialDeductItemBo? material = null;
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
                                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", sfc } };
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
                        if (whMaterial.DataCollectionWay == MaterialSerialNumberEnum.Outside)
                        {
                            // 报错 内部条码  bom属性为外部
                            var validationFailure = new ValidationFailure();
                            if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                            {
                                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", sfc } };
                            }
                            else
                            {
                                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                            }
                            validationFailure.ErrorCode = nameof(ErrorCode.MES16134);
                            validationFailures.Add(validationFailure);
                            continue;
                        }
                        material = whMaterial;
                        qty = whMaterialInventory.QuantityResidue;

                        if (qty == 0)
                        {
                            var validationFailure = new ValidationFailure();
                            if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                            {
                                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", sfc } };
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
                    updateWhMaterialInventoryEmptyByIdCommands.Add(new UpdateWhMaterialInventoryEmptyByIdCommand
                    {
                        Id = whMaterialInventory.Id,
                        UpdatedBy = commonBo.UserName,
                        UpdatedOn = commonBo.Time
                    });
                }

                if (material == null)
                {
                    //报错  不在bom中
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", sfc } };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", planWorkOrderEntity.OrderCode);
                    }
                    validationFailure.FormattedMessagePlaceholderValues.Add("WorkOrder", planWorkOrderEntity.OrderCode);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES16135);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                // 准备接收数据
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
                    manuSfcEntity.Type = SfcTypeEnum.Produce;
                    manuSfcEntity.IsUsed = YesOrNoEnum.No;
                    manuSfcEntity.Status = SfcStatusEnum.lineUp;
                    manuSfcEntity.UpdatedBy = commonBo.UserName;
                    manuSfcEntity.UpdatedOn = commonBo.Time;
                    updateManuSfcList.Add(manuSfcEntity);
                    manuSfcInfoUpdateIsUsedBo.SfcIds.Add(manuSfcEntity.Id);
                }

                var sfcInfoId = IdGenProvider.Instance.CreateId();

                manuSfcInfoList.Add(new ManuSfcInfoEntity
                {
                    Id = sfcInfoId,
                    SiteId = commonBo.SiteId,
                    SfcId = manuSfcEntity.Id,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    ProductId = productId,
                    IsUsed = true,
                    CreatedBy = commonBo.UserName,
                    CreatedOn = commonBo.Time,
                    UpdatedBy = commonBo.UserName,
                    UpdatedOn = commonBo.Time
                });

                manuSfcProduceList.Add(new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = commonBo.SiteId,
                    SFC = sfc,
                    SFCId = manuSfcEntity.Id,
                    ProductId = productId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    BarCodeInfoId = sfcInfoId,
                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    Qty = qty,
                    ResourceId = commonBo.ResourceId,
                    EquipmentId = commonBo.EquipmentId,
                    ProcedureId = commonBo.ProcedureId,
                    Status = SfcStatusEnum.lineUp,
                    RepeatedCount = 0,
                    IsScrap = TrueOrFalseEnum.No,
                    CreatedBy = commonBo.UserName,
                    CreatedOn = commonBo.Time,
                    UpdatedBy = commonBo.UserName,
                    UpdatedOn = commonBo.Time
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
                    CreatedOn = commonBo.Time,
                    UpdatedBy = commonBo.UserName,
                    UpdatedOn = commonBo.Time
                });
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(commonBo.LocalizationService.GetResource("SFCError"), validationFailures);
            }

            return new BarcodeSfcReceiveResponseBo
            {
                WorkOrderId = planWorkOrderEntity.Id,
                PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100),
                PassDownQuantity = manuSfcProduceList.Sum(x => x.Qty),
                UserName = commonBo.UserName,
                IsProductSame = productId == planWorkOrderEntity.ProductId,
                ManuSfcInfoUpdateIsUsed = manuSfcInfoUpdateIsUsedBo,
                ManuSfcList = manuSfcList,
                UpdateManuSfcList = updateManuSfcList,
                ManuSfcInfoList = manuSfcInfoList,
                ManuSfcProduceList = manuSfcProduceList,
                ManuSfcStepList = manuSfcStepList,
                updateWhMaterialInventoryEmptyByIdCommands = updateWhMaterialInventoryEmptyByIdCommands
            };
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo?> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not BarcodeSfcReceiveResponseBo data) return responseBo;
            if (data.ManuSfcInfoUpdateIsUsed.SfcIds != null && data.ManuSfcInfoUpdateIsUsed.SfcIds.Any())
            {
                responseBo.Rows += await _manuSfcInfoRepository.UpdatesIsUsedAsync(new ManuSfcInfoUpdateIsUsedCommand()
                {
                    UpdatedOn = data.ManuSfcInfoUpdateIsUsed.UpdatedOn,
                    SfcIds = data.ManuSfcInfoUpdateIsUsed.SfcIds,
                    UserId = data.ManuSfcInfoUpdateIsUsed.UserId,
                });
            }
            // 当产出设置的产品和工单对应的产品一致时，才更新工单的下达数量
            if (data.IsProductSame)
            {
                responseBo.Rows += await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderIdAsync(new UpdatePassDownQuantityCommand
                {
                    WorkOrderId = data.WorkOrderId,
                    PlanQuantity = data.PlanQuantity,
                    PassDownQuantity = data.PassDownQuantity,
                    UserName = data.UserName,
                    UpdateDate = HymsonClock.Now()
                });

                if (responseBo.Rows == 0) throw new CustomerValidationException(nameof(ErrorCode.MES16503)).WithData("workorder", data.OrderCode);
            }

            // 更新数据
            List<Task<int>> tasks = new()
            {
                _manuSfcRepository.InsertRangeAsync(data.ManuSfcList),
                _manuSfcRepository.UpdateRangeAsync(data.UpdateManuSfcList),
                _manuSfcInfoRepository.InsertsAsync(data.ManuSfcInfoList),
                _manuSfcProduceRepository.InsertRangeAsync(data.ManuSfcProduceList),
                _manuSfcStepRepository.InsertRangeAsync(data.ManuSfcStepList),
                _whMaterialInventoryRepository.UpdateWhMaterialInventoryEmptyByIdRangeAync(data.updateWhMaterialInventoryEmptyByIdCommands)
            };

            // 等待所有任务完成
            var rowArray = await Task.WhenAll(tasks);
            responseBo.Rows += rowArray.Sum();

            return responseBo;
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
