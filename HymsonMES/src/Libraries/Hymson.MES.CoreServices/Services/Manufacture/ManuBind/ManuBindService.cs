using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Manufacture;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.CoreServices.Services.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class ManuBindService : IManuBindService
    {
        public readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        public readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IMasterDataService _masterDataService;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcBomRepository _procBomRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="masterDataService"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="manuCommonService"></param>
        public ManuBindService(IManuSfcProduceRepository manuSfcProduceRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IMasterDataService masterDataService, IProcProcedureRepository procProcedureRepository, IProcBomRepository procBomRepository,
            IProcMaterialRepository procMaterialRepository, IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuSfcRepository manuSfcRepository, IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcStepRepository manuSfcStepRepository, IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IManuCommonService manuCommonService)
        {
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _masterDataService = masterDataService;
            _procProcedureRepository = procProcedureRepository;
            _procBomRepository = procBomRepository;
            _procMaterialRepository = procMaterialRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _manuCommonService = manuCommonService;
        }

        /// <summary>
        /// <summary>
        /// 条码绑定-活动
        /// </summary>
        /// <returns></returns>
        public async Task BindByActive(ManuBindDto param, ILocalizationService localizationService)
        {
            //验证绑定条码
            var manuSfcProduceList = await _manuSfcProduceRepository.GetListBySfcsAsync(
                    new ManuSfcProduceBySfcsQuery
                    {
                        Sfcs = param.BindSFCs.Select(x => x.SFC),
                        SiteId = param.SiteId
                    });

            if (manuSfcProduceList == null || !manuSfcProduceList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17401)).WithData("SFCs", string.Join(",", param.BindSFCs.Select(x => x.SFC)));
            }

            if (manuSfcProduceList.Select(x => x.WorkOrderId).Distinct().Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17412)).WithData("SFCs", string.Join(",", param.BindSFCs.Select(x => x.SFC)));
            }

            if (manuSfcProduceList.Select(x => x.ProductId).Distinct().Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17413)).WithData("SFCs", string.Join(",", param.BindSFCs.Select(x => x.SFC)));
            }

            //查询条码绑定信息
            var manuSfcCirculationByBindEntits = await _manuSfcCirculationRepository.GetSfcMoudulesAsync(new ManuSfcCirculationBySfcsQuery
            {
                SiteId = param.SiteId,
                CirculationBarCodes = param.BindSFCs.Select(x => x.SFC)
            });


            if (manuSfcCirculationByBindEntits != null && manuSfcCirculationByBindEntits.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17409)).WithData("SFCs", string.Join(",", manuSfcCirculationByBindEntits.Select(x => x.CirculationBarCode)));
            }
            IEnumerable<ManuSfcCirculationEntity> manuSfcCirculationBySFCEntities = new List<ManuSfcCirculationEntity>();
            var validationFailures = new List<ValidationFailure>();
            List<ManuSfcStepEntity> manuSfcStepList = new();
            //条码流转信息
            List<ManuSfcCirculationEntity> manuSfcCirculationEntities = new();
            List<ManuSfcUpdateStatusByIdCommand> manuSfcUpdateStatusByIdCommands = new List<ManuSfcUpdateStatusByIdCommand>();
            //绑定条码工单 和 产品信息
            long workOrderId = manuSfcProduceList!.FirstOrDefault()?.WorkOrderId ?? 0;
            long productId = manuSfcProduceList!.FirstOrDefault()?.ProductId ?? 0;

            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(workOrderId);

            var manuSfc = new ManuSfcEntity();
            var manuSfcInfo = new ManuSfcInfoEntity();
            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery { Sfc = param.SFC, SiteId = param.SiteId });
            bool isCreate = false;
            if (manuSfcProduceEntity == null)
            {
                isCreate = true;
                //获取产出设置
                var outputProductId = await _masterDataService.GetProductSetIdAsync(new ProductSetBo { SiteId = param.SiteId, ProductId = planWorkOrderEntity.ProductId, ProcedureId = param.ProcedureId, ResourceId = param.ResourceId }) ?? planWorkOrderEntity.ProductId;
                //获取 物料批次大小
                var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(productId);
                var qty = procMaterialEntity.Batch;
                //掩码校验
                if (!await _manuCommonService.CheckBarCodeByMaskCodeRuleAsync(param.SFC, outputProductId))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17425)).WithData("SFC", param.SFC).WithData("MaterialName", procMaterialEntity.MaterialName);
                }

                //插入生产一套表
                manuSfc = new ManuSfcEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = param.SFC,
                    IsUsed = YesOrNoEnum.No,
                    Qty = qty,
                    SiteId = param.SiteId,
                    Status = SfcStatusEnum.Activity,
                    CreatedBy = param.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = param.UserName,
                    UpdatedOn = HymsonClock.Now()
                };
                manuSfcInfo = new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SfcId = manuSfc.Id,
                    WorkOrderId = workOrderId,
                    ProductId = outputProductId,
                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    IsUsed = true,
                    CreatedBy = param.UserName,
                    UpdatedBy = param.UserName
                };
                manuSfcProduceEntity = new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SFC = param.SFC,
                    SFCId = manuSfc.Id,
                    ProductId = outputProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    BarCodeInfoId = manuSfcInfo.Id,
                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    EquipmentId = param.EquipmentId,
                    ResourceId = param.ResourceId,
                    Qty = qty,
                    ProcedureId = param.ProcedureId,
                    Status = SfcStatusEnum.Activity,
                    RepeatedCount = 1,
                    IsScrap = TrueOrFalseEnum.No,
                    CreatedBy = param.UserName,
                    UpdatedBy = param.UserName
                };
                //步骤记录
                var manuSfcStep = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SFC = param.SFC,
                    ProductId = outputProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    ProcedureId = param.ProcedureId,
                    Qty = qty,
                    Operatetype = ManuSfcStepTypeEnum.InStock,
                    CurrentStatus = SfcStatusEnum.Activity,
                    EquipmentId = param.EquipmentId,
                    ResourceId = param.ResourceId,
                    CreatedBy = param.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = param.UserName,
                    UpdatedOn = HymsonClock.Now(),
                };
                manuSfcStepList.Add(manuSfcStep);

                manuSfcStepList.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SFC = param.SFC,
                    ProductId = outputProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    ProcedureId = param.ProcedureId,
                    Qty = qty,
                    Operatetype = ManuSfcStepTypeEnum.InStock,
                    CurrentStatus = SfcStatusEnum.Activity,
                    EquipmentId = param.EquipmentId,
                    ResourceId = param.ResourceId,
                    CreatedBy = param.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = param.UserName,
                    UpdatedOn = HymsonClock.Now(),
                });
            }
            else
            {
                if (manuSfcProduceEntity.Status != SfcStatusEnum.Activity)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17426)).WithData("SFC", param.SFC);
                }
                var unmanuSfcProduceWorkOrders = manuSfcProduceList.Where(x => x.WorkOrderId != manuSfcProduceEntity.WorkOrderId);
                if (unmanuSfcProduceWorkOrders.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17408)).WithData("SFCs", string.Join(",", unmanuSfcProduceWorkOrders.Select(x => x.SFC)));
                }
                manuSfcCirculationBySFCEntities = await _manuSfcCirculationRepository.GetSfcMoudulesAsync(new ManuSfcCirculationQuery
                {
                    SiteId = param.SiteId,
                    Sfc = param.SFC
                });
            }

            //var boms = await _masterDataService.GetInitialMaterialsAsync(new MaterialDeductRequestBo
            //{
            //    SiteId = param.SiteId,
            //    ProcedureId = param.ProcedureId,
            //    ProductBOMId = planWorkOrderEntity.ProductBOMId
            //});

            //if (boms != null && boms.Any())
            //{
            //    var bom = boms.FirstOrDefault(x => x.MaterialId == productId);
            //    if (bom == null)
            //    {
            //        var pprocMaterialEntity = await _procMaterialRepository.GetByIdAsync(productId);
            //        var procBomEntity = await _procBomRepository.GetByIdAsync(planWorkOrderEntity.ProductBOMId);
            //        var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(param.ProcedureId);

            //        throw new CustomerValidationException(nameof(ErrorCode.MES17407)).WithData("MaterialName", pprocMaterialEntity?.MaterialName ?? "")
            //            .WithData("WorkOrder", planWorkOrderEntity?.OrderCode ?? "").WithData("BomName", procBomEntity?.BomName ?? "").WithData("ProcedureName", procProcedureEntity?.Name ?? "");
            //    }
            //if ((manuSfcCirculationBySFCEntities!.Sum(x => x.CirculationQty) ?? 0) + manuSfcProduceList!.Sum(x => x.Qty) > bom.Usages)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES17411)).WithData("BindQty", manuSfcCirculationBySFCEntities!.Sum(x => x.CirculationQty) ?? 0)
            //        .WithData("TreatQty", manuSfcProduceList!.Sum(x => x.Qty))
            //         .WithData("TreatQty", bom.Usages);
            //}
            //}
            //else
            //{
            //    var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(param.ProcedureId);

            //    throw new CustomerValidationException(nameof(ErrorCode.MES17414))
            //        .WithData("WorkOrder", planWorkOrderEntity?.OrderCode ?? "")
            //        .WithData("ProcedureName", procProcedureEntity?.Name ?? "");
            //}

            var deleteIds = new List<long>();
            foreach (var item in param.BindSFCs)
            {
                if (manuSfcCirculationBySFCEntities != null && manuSfcCirculationBySFCEntities.Any(x => x.Location == item.Location))
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
                    validationFailure.FormattedMessagePlaceholderValues.Add("Location", item.Location);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES17410);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                var itemManuSfcProduce = manuSfcProduceList.FirstOrDefault(x => x.SFC == item.SFC);
                if (itemManuSfcProduce == null)
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
                    validationFailure.ErrorCode = nameof(ErrorCode.MES17402);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                //工单验证
                if (workOrderId != itemManuSfcProduce.WorkOrderId)
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
                    var planWorkOrder = await _planWorkOrderRepository.GetByIdAsync(itemManuSfcProduce.WorkOrderId);
                    validationFailure.FormattedMessagePlaceholderValues.Add("WorkOrder", planWorkOrder?.OrderCode);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES17405);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                //状态
                if (itemManuSfcProduce.Status != SfcStatusEnum.Activity)
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
                    validationFailure.ErrorCode = nameof(ErrorCode.MES17403);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                //工序
                if (param.ProcedureId != itemManuSfcProduce.ProcedureId)
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
                    var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(itemManuSfcProduce.ProcedureId);
                    validationFailure.FormattedMessagePlaceholderValues.Add("ProcedureName", procProcedureEntity.Name);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES17406);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                //记录流转信息
                manuSfcCirculationEntities.Add(new ManuSfcCirculationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    ProcedureId = manuSfcProduceEntity.ProcedureId,
                    ResourceId = param.ResourceId,
                    SFC = manuSfcProduceEntity.SFC,
                    WorkOrderId = manuSfcProduceEntity.WorkOrderId,
                    ProductId = manuSfcProduceEntity.ProductId,
                    EquipmentId = param.EquipmentId,
                    CirculationBarCode = itemManuSfcProduce.SFC,
                    CirculationProductId = itemManuSfcProduce.ProductId,//暂时使用原有产品ID
                    CirculationMainProductId = itemManuSfcProduce.ProductId,
                    Location = item.Location,
                    CirculationQty = itemManuSfcProduce.Qty,
                    CirculationType = SfcCirculationTypeEnum.Merge,
                    CreatedBy = param.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = param.UserName,
                    UpdatedOn = HymsonClock.Now()
                });

                manuSfcStepList.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SFC = param.SFC,
                    ProductId = itemManuSfcProduce.ProductId,
                    WorkOrderId = itemManuSfcProduce.WorkOrderId,
                    WorkCenterId = itemManuSfcProduce.WorkCenterId,
                    ProductBOMId = itemManuSfcProduce.ProductBOMId,
                    ProcedureId = param.ProcedureId,//当前绑定条码所在工序
                    Qty = itemManuSfcProduce.Qty,
                    Operatetype = ManuSfcStepTypeEnum.BarcodeBinding,
                    CurrentStatus = itemManuSfcProduce.Status,
                    EquipmentId = param.EquipmentId,
                    ResourceId = param.ResourceId,
                    CreatedBy = param.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = param.UserName,
                    UpdatedOn = HymsonClock.Now(),
                });

                //步骤记录
                var manuSfcStep = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SFC = param.SFC,
                    ProductId = itemManuSfcProduce.ProductId,
                    WorkOrderId = itemManuSfcProduce.WorkOrderId,
                    WorkCenterId = itemManuSfcProduce.WorkCenterId,
                    ProductBOMId = itemManuSfcProduce.ProductBOMId,
                    ProcedureId = param.ProcedureId,//当前绑定条码所在工序
                    Qty = itemManuSfcProduce.Qty,
                    Operatetype = ManuSfcStepTypeEnum.OutStock,
                    CurrentStatus = itemManuSfcProduce.Status,
                    EquipmentId = param.EquipmentId,
                    ResourceId = param.ResourceId,
                    CreatedBy = param.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = param.UserName,
                    UpdatedOn = HymsonClock.Now(),
                };
                manuSfcStepList.Add(manuSfcStep);

                manuSfcUpdateStatusByIdCommands.Add(new ManuSfcUpdateStatusByIdCommand
                {
                    UpdatedBy = param.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    Id = itemManuSfcProduce.SFCId,
                    Status = SfcStatusEnum.Complete,
                    CurrentStatus = itemManuSfcProduce.Status,
                });

                //删除在制品
                deleteIds.Add(itemManuSfcProduce.Id);
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(localizationService.GetResource("SFCError"), validationFailures);
            }

            PhysicalDeleteSFCProduceByIdsCommand deletePhysicalByProduceIdsCommand = new PhysicalDeleteSFCProduceByIdsCommand
            {
                SiteId = param.SiteId,
                Ids = deleteIds
            };

            using var trans = TransactionHelper.GetTransactionScope();
            if (isCreate)
            {
                await _manuSfcProduceRepository.InsertAsync(manuSfcProduceEntity);
                await _manuSfcRepository.InsertAsync(manuSfc);
                await _manuSfcInfoRepository.InsertAsync(manuSfcInfo);
            }

            if (manuSfcStepList != null && manuSfcStepList.Any())
            {
                await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepList);
            }

            if (manuSfcCirculationEntities != null && manuSfcCirculationEntities.Any())
            {
                await _manuSfcCirculationRepository.InsertRangeAsync(manuSfcCirculationEntities);
            }

            if (manuSfcUpdateStatusByIdCommands != null && manuSfcUpdateStatusByIdCommands.Any())
            {
                await _manuSfcRepository.ManuSfcUpdateStatuByIdRangeAsync(manuSfcUpdateStatusByIdCommands);
            }
            await _manuSfcProduceRepository.DeletePhysicalRangeByIdsAsync(deletePhysicalByProduceIdsCommand);
            trans.Complete();
        }

        /// <summary>
        /// 条码绑定-完成
        /// </summary>
        /// <param name="param"></param>
        /// <param name="localizationService"></param>
        /// <returns></returns>
        public async Task BindByComplete(ManuBindDto param, ILocalizationService localizationService)
        {
            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery
            {
                Sfc = param.SFC,
                SiteId = param.SiteId,
            });

            if (manuSfcProduceEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", param.SFC);
            }

            // 获取库存数据
            var whMaterialInventorys = await _whMaterialInventoryRepository.GetByBarCodesOfHasQtyAsync(new WhMaterialInventoryBarCodesQuery
            {
                SiteId = param.SiteId,
                BarCodes = param.BindSFCs.Select(x => x.SFC)
            });

            if (whMaterialInventorys == null || !whMaterialInventorys.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17420)).WithData("SFCs", string.Join(",", param.BindSFCs.Select(x => x.SFC)));
            }

            if (whMaterialInventorys.Select(x => x.WorkOrderId).Distinct().Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17417)).WithData("SFCs", string.Join(",", param.BindSFCs.Select(x => x.SFC)));
            }

            if (whMaterialInventorys.Select(x => x.MaterialId).Distinct().Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17418)).WithData("SFCs", string.Join(",", param.BindSFCs.Select(x => x.SFC)));
            }

            //绑定条码工单 和 产品信息
            long workOrderId = whMaterialInventorys!.FirstOrDefault()?.WorkOrderId ?? 0;

            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(workOrderId);
            if (workOrderId != manuSfcProduceEntity.WorkOrderId)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17419)).WithData("SFC", param.SFC);
            }

            long productId = whMaterialInventorys!.FirstOrDefault()?.MaterialId ?? 0;

            //查询条码绑定信息
            var manuSfcCirculationEntits = await _manuSfcCirculationRepository.GetSfcMoudulesAsync(new ManuSfcCirculationBySfcsQuery
            {
                SiteId = param.SiteId,
                CirculationBarCodes = param.BindSFCs.Select(x => x.SFC)
            });

            if (manuSfcCirculationEntits != null && manuSfcCirculationEntits.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17409)).WithData("SFCs", string.Join(",", manuSfcCirculationEntits.Select(x => x.CirculationBarCode)));
            }

            var manuSfcCirculationBySfcEntits = await _manuSfcCirculationRepository.GetSfcMoudulesAsync(new ManuSfcCirculationQuery
            {
                SiteId = param.SiteId,
                Sfc = param.SFC
            });

            var boms = await _masterDataService.GetInitialMaterialsAsync(new MaterialDeductRequestBo
            {
                SiteId = param.SiteId,
                ProcedureId = param.ProcedureId,
                ProductBOMId = manuSfcProduceEntity.ProductBOMId
            });

            if (boms != null && boms.Any())
            {
                var bom = boms.FirstOrDefault(x => x.MaterialId == productId);
                if (bom == null)
                {
                    var pprocMaterialEntity = await _procMaterialRepository.GetByIdAsync(productId);
                    var procBomEntity = await _procBomRepository.GetByIdAsync(planWorkOrderEntity.ProductBOMId);
                    var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(param.ProcedureId);

                    throw new CustomerValidationException(nameof(ErrorCode.MES17407)).WithData("MaterialName", pprocMaterialEntity?.MaterialName ?? "")
                        .WithData("WorkOrder", planWorkOrderEntity?.OrderCode ?? "").WithData("BomName", procBomEntity?.BomName ?? "").WithData("ProcedureName", procProcedureEntity?.Name ?? "");
                }
                if ((manuSfcCirculationBySfcEntits!.Sum(x => x.CirculationQty) ?? 0) + whMaterialInventorys!.Sum(x => x.QuantityResidue) > bom.Usages)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17411))
                        .WithData("BindQty", manuSfcCirculationBySfcEntits!.Sum(x => x.CirculationQty) ?? 0)
                        .WithData("TreatQty", whMaterialInventorys.Sum(x => x.QuantityResidue))
                         .WithData("NeedQty", bom.Usages);
                }
            }
            else
            {
                var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(param.ProcedureId);

                throw new CustomerValidationException(nameof(ErrorCode.MES17414))
                    .WithData("WorkOrder", planWorkOrderEntity?.OrderCode ?? "")
                    .WithData("ProcedureName", procProcedureEntity?.Name ?? "");
            }

            var validationFailures = new List<ValidationFailure>();
            //条码流转信息
            List<ManuSfcCirculationEntity> manuSfcCirculationEntities = new();
            List<ManuSfcStepEntity> manuSfcStepList = new();
            foreach (var item in param.BindSFCs)
            {
                if (manuSfcCirculationBySfcEntits != null && manuSfcCirculationBySfcEntits.Any(x => x.Location == item.Location))
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
                    validationFailure.FormattedMessagePlaceholderValues.Add("Location", item.Location);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES17406);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                var itemwhMaterialInventory = whMaterialInventorys.FirstOrDefault(x => x.MaterialBarCode == item.SFC);
                if (itemwhMaterialInventory == null)
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
                    validationFailure.ErrorCode = nameof(ErrorCode.MES17416);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                itemwhMaterialInventory.QuantityResidue = 0;
                itemwhMaterialInventory.UpdatedBy = param.UserName;
                itemwhMaterialInventory.UpdatedOn = HymsonClock.Now();

                //记录流转信息
                manuSfcCirculationEntities.Add(new ManuSfcCirculationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    ProcedureId = manuSfcProduceEntity.ProcedureId,
                    ResourceId = param.ResourceId,
                    SFC = manuSfcProduceEntity.SFC,
                    WorkOrderId = manuSfcProduceEntity.WorkOrderId,
                    ProductId = manuSfcProduceEntity.ProductId,
                    EquipmentId = param.EquipmentId,
                    CirculationBarCode = itemwhMaterialInventory.MaterialBarCode,
                    CirculationProductId = itemwhMaterialInventory.MaterialId,//暂时使用原有产品ID
                    CirculationMainProductId = itemwhMaterialInventory.MaterialId,
                    Location = item.Location,
                    CirculationQty = itemwhMaterialInventory.QuantityResidue,
                    CirculationType = SfcCirculationTypeEnum.Merge,
                    CreatedBy = param.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = param.UserName,
                    UpdatedOn = HymsonClock.Now()
                });

                manuSfcStepList.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SFC = item.SFC,
                    ProductId = itemwhMaterialInventory.MaterialId,
                    WorkOrderId = itemwhMaterialInventory.WorkOrderId ?? 0,
                    ProductBOMId = manuSfcProduceEntity.ProductBOMId,
                    ProcedureId = param.ProcedureId,
                    Qty = itemwhMaterialInventory.QuantityResidue,
                    Operatetype = ManuSfcStepTypeEnum.BarcodeBinding,
                    CurrentStatus = SfcStatusEnum.Activity,
                    EquipmentId = param.EquipmentId,
                    ResourceId = param.ResourceId,
                    CreatedBy = param.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = param.UserName,
                    UpdatedOn = HymsonClock.Now(),
                });
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(localizationService.GetResource("SFCError"), validationFailures);
            }

            manuSfcStepList.Add(new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = param.SiteId,
                SFC = manuSfcProduceEntity.SFC,
                ProductId = manuSfcProduceEntity.ProductId,
                WorkOrderId = manuSfcProduceEntity.WorkOrderId,
                ProductBOMId = manuSfcProduceEntity.ProductBOMId,
                ProcedureId = param.ProcedureId,
                Qty = manuSfcProduceEntity.Qty,
                Operatetype = ManuSfcStepTypeEnum.BarcodeBinding,
                CurrentStatus = SfcStatusEnum.Activity,
                EquipmentId = param.EquipmentId,
                ResourceId = param.ResourceId,
                CreatedBy = param.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = param.UserName,
                UpdatedOn = HymsonClock.Now(),
            });

            using var trans = TransactionHelper.GetTransactionScope();

            if (manuSfcStepList != null && manuSfcStepList.Any())
            {
                await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepList);
            }

            await _whMaterialInventoryRepository.UpdatesAsync(whMaterialInventorys);

            if (manuSfcCirculationEntities != null && manuSfcCirculationEntities.Any())
            {
                await _manuSfcCirculationRepository.InsertRangeAsync(manuSfcCirculationEntities);
            }
            trans.Complete();
        }

        /// <summary>
        /// 绑定条码-外部
        /// </summary>
        /// <param name="param"></param>
        /// <param name="localizationService"></param>
        /// <returns></returns>
        public async Task BindByExternal(ManuBindDto param, ILocalizationService localizationService)
        {
            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery
            {
                Sfc = param.SFC,
                SiteId = param.SiteId,
            });

            if (manuSfcProduceEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", param.SFC);
            }

            ///  王飞说状态不需要验证   TODO 

            var manuSfcStepEntity = new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = param.SiteId,
                SFC = param.SFC,
                ProductId = manuSfcProduceEntity.ProductId,
                WorkOrderId = manuSfcProduceEntity.WorkOrderId,
                ProductBOMId = manuSfcProduceEntity.ProductBOMId,
                ProcedureId = param.ProcedureId,
                Qty = manuSfcProduceEntity.Qty,
                Operatetype = ManuSfcStepTypeEnum.BarcodeBinding,
                CurrentStatus = manuSfcProduceEntity.Status,
                EquipmentId = param.EquipmentId,
                ResourceId = param.ResourceId,
                CreatedBy = param.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = param.UserName,
                UpdatedOn = HymsonClock.Now(),
            };

            var boms = await _masterDataService.GetInitialMaterialsAsync(new MaterialDeductRequestBo
            {
                SiteId = param.SiteId,
                ProcedureId = param.ProcedureId,
                ProductBOMId = manuSfcProduceEntity.ProductBOMId
            });

            var bomMaterials = boms.SelectMany((x) =>
            {
                List<BomMaterial> bomMaterial = new();
                bomMaterial.Add(new BomMaterial
                {
                    MaterialId = x.MaterialId,
                    DataCollectionWay = x.DataCollectionWay,
                    Qty = x.Usages,
                });
                foreach (var item in bomMaterial)
                {
                    bomMaterial.Add(new BomMaterial
                    {
                        MaterialId = item.MaterialId,
                        DataCollectionWay = item.DataCollectionWay,
                        Qty = item.Qty,
                    });
                }
                return bomMaterial;
            }
           );
            var validationFailures = new List<ValidationFailure>();
            List<ManuSfcCirculationEntity> manuSfcCirculationEntities = new();
            foreach (var item in param.BindSFCs)
            {
                BomMaterial? material = null;
                // 循环匹配掩码规则来确认接收物料
                foreach (var bom in bomMaterials.Where(x => x.DataCollectionWay == MaterialSerialNumberEnum.Outside))
                {
                    if (await _manuCommonService.CheckBarCodeByMaskCodeRuleAsync(item.SFC, bom.MaterialId))
                    {
                        material = bom;
                        break;
                    }
                }
                if (material == null)
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
                    validationFailure.ErrorCode = nameof(ErrorCode.MES17424);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                else
                {
                    //获取 物料批次大小
                    var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(material.MaterialId);

                    //记录流转信息
                    manuSfcCirculationEntities.Add(new ManuSfcCirculationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = param.SiteId,
                        ProcedureId = manuSfcProduceEntity.ProcedureId,
                        ResourceId = param.ResourceId,
                        SFC = manuSfcProduceEntity.SFC,
                        WorkOrderId = manuSfcProduceEntity.WorkOrderId,
                        ProductId = manuSfcProduceEntity.ProductId,
                        EquipmentId = param.EquipmentId,
                        CirculationBarCode = item.SFC,
                        CirculationProductId = material.MaterialId,//暂时使用原有产品ID
                        //CirculationMainProductId = itemwhMaterialInventory.MaterialId,
                        Location = item.Location,
                        CirculationQty = procMaterialEntity.Id,
                        CirculationType = SfcCirculationTypeEnum.ModuleAdd,
                        CreatedBy = param.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedBy = param.UserName,
                        UpdatedOn = HymsonClock.Now()
                    });
                }
            }


            if (validationFailures.Any())
            {
                throw new ValidationException(localizationService.GetResource("SFCError"), validationFailures);
            }

            using var trans = TransactionHelper.GetTransactionScope();


            await _manuSfcStepRepository.InsertAsync(manuSfcStepEntity);

            if (manuSfcCirculationEntities != null && manuSfcCirculationEntities.Any())
            {
                await _manuSfcCirculationRepository.UpdateRangeAsync(manuSfcCirculationEntities);
            }
            trans.Complete();
        }

        /// <summary>
        /// 解除绑定
        /// </summary>
        /// <param name="param"></param>
        /// <param name="localizationService"></param>
        /// <returns></returns>
        public async Task UnBind(ManuUnBindDto param, ILocalizationService localizationService)
        {
            var manuSfcCirculationBySfcEntits = await _manuSfcCirculationRepository.GetSfcMoudulesAsync(new ManuSfcCirculationQuery
            {
                SiteId = param.SiteId,
                Sfc = param.SFC
            });
            if (manuSfcCirculationBySfcEntits == null || !manuSfcCirculationBySfcEntits.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17422)).WithData("SFC", param.SFC);
            }
            List<ManuSfcCirculationEntity> manuSfcCirculationEntities = new();
            var validationFailures = new List<ValidationFailure>();
            List<ManuSfcStepEntity> manuSfcStepList = new();
            List<WhMaterialInventoryEntity> whMaterialInventoryEntities = new();
            var sfc = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
            {
                SiteId = param.SiteId,
                SFC = param.SFC,
                Type = SfcTypeEnum.Produce
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES17423)).WithData("SFC", param.SFC);

            var manuSfc = await _manuSfcInfoRepository.GetBySFCIdWithIsUseAsync(sfc.Id);
            if (manuSfc == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17423)).WithData("SFC", param.SFC);
            }
            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery
            {
                Sfc = param.SFC,
                SiteId = param.SiteId
            });

            manuSfcStepList.Add(new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = param.SiteId,
                SFC = param.SFC,
                ProductId = manuSfc.ProductId,
                WorkOrderId = manuSfc.WorkOrderId ?? 0,
                ProductBOMId = manuSfcProduceEntity?.ProductBOMId,
                ProcedureId = param.ProcedureId,
                Qty = sfc.Qty,
                Operatetype = ManuSfcStepTypeEnum.BarcodeUnbinding,
                CurrentStatus = SfcStatusEnum.Activity,
                EquipmentId = param.EquipmentId,
                ResourceId = param.ResourceId,
                CreatedBy = param.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = param.UserName,
                UpdatedOn = HymsonClock.Now(),
            });

            //解除
            foreach (var item in param.UnBindSFCs)
            {
                var manuSfcCirculationBySfcEntity = manuSfcCirculationBySfcEntits.FirstOrDefault(x => x.CirculationBarCode == item);

                if (manuSfcCirculationBySfcEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", item}
                            };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item);
                    }
                    validationFailure.FormattedMessagePlaceholderValues.Add("SFC", param.SFC);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES17421);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                manuSfcCirculationBySfcEntity.IsDisassemble = TrueOrFalseEnum.Yes;
                manuSfcCirculationBySfcEntity.UpdatedBy = param.UserName;
                manuSfcCirculationBySfcEntity.DisassembledBy = param.UserName;
                manuSfcCirculationBySfcEntity.DisassembledOn = HymsonClock.Now();
                manuSfcCirculationBySfcEntity.UpdatedOn = HymsonClock.Now();
                manuSfcCirculationEntities.Add(manuSfcCirculationBySfcEntity);

                whMaterialInventoryEntities.Add(new WhMaterialInventoryEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SupplierId = 0,//自制品 没有
                    MaterialId = manuSfcCirculationBySfcEntity.CirculationProductId,
                    MaterialBarCode = manuSfcCirculationBySfcEntity.CirculationBarCode,
                    //Batch = "",//自制品 没有
                    MaterialType = MaterialInventoryMaterialTypeEnum.SelfMadeParts,
                    QuantityResidue = manuSfcCirculationBySfcEntity.CirculationQty ?? 0,
                    Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                    Source = MaterialInventorySourceEnum.Disassembly,
                    SiteId = param.SiteId,
                    CreatedBy = param.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = param.UserName,
                    UpdatedOn = HymsonClock.Now(),
                });
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(localizationService.GetResource("SFCError"), validationFailures);
            }

            using var trans = TransactionHelper.GetTransactionScope();

            if (manuSfcStepList != null && manuSfcStepList.Any())
            {
                await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepList);
            }

            if (manuSfcCirculationEntities != null && manuSfcCirculationEntities.Any())
            {
                await _manuSfcCirculationRepository.UpdateRangeAsync(manuSfcCirculationEntities);
            }

            if (whMaterialInventoryEntities != null && whMaterialInventoryEntities.Any())
            {
                await _whMaterialInventoryRepository.InsertsAsync(whMaterialInventoryEntities);
            }
            trans.Complete();
        }
    }
}
