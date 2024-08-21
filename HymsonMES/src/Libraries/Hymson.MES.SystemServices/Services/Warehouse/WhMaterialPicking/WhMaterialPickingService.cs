using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Services.Quality;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteCodeRule.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuRequistionOrder;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Data.Repositories.WhWareHouse;
using Hymson.MES.SystemServices.Dtos.Warehouse;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.SystemServices.Services.Warehouse.WhMaterialPicking
{
    /// <summary>
    /// 收料单
    /// </summary>
    public class WhMaterialPickingService : IWhMaterialPickingService
    {
        /// <summary>
        /// 当前系统
        /// </summary>
        private readonly ICurrentSystem _currentSystem;
        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 物料库存仓储
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 物料台账
        /// </summary>
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;

        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 退料单
        /// </summary>
        private readonly IManuReturnOrderRepository _manuReturnOrderRepository;

        /// <summary>
        /// 退料单详情
        /// </summary>
        private readonly IManuReturnOrderDetailRepository _manuReturnOrderDetailRepository;

        private readonly IWhSupplierRepository _whSupplierRepository;

        /// <summary>
        /// 服务接口（检验单生成）
        /// </summary>
        private readonly IIQCOrderCreateService _iqcOrderCreateService;

        private readonly IQualIqcOrderReturnRepository _qualIqcOrderReturnRepository;

        private readonly IQualIqcOrderReturnDetailRepository _qualIqcOrderReturnDetailRepository;

        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;

        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;

        private readonly IWhWarehouseRepository _whWarehouseRepository;

        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        private readonly ISysConfigRepository _sysConfigRepository;

        private readonly IManuSfcRepository _manuSfcRepository;

        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        private readonly IManuRequistionOrderRepository _manuRequistionOrderRepository;

        private readonly IManuRequistionOrderDetailRepository _manuRequistionOrderDetailRepository;

        private readonly IManuRequistionOrderReceiveRepository _manuRequistionOrderReceiveRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="localizationService"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="manuReturnOrderRepository"></param>
        /// <param name="manuReturnOrderDetailRepository"></param>
        /// <param name="whSupplierRepository"></param>
        /// <param name="iqcOrderCreateService"></param>
        /// <param name="qualIqcOrderReturnRepository"></param>
        /// <param name="qualIqcOrderReturnDetailRepository"></param>
        /// <param name="inteCodeRulesRepository"></param>
        /// <param name="manuGenerateBarcodeService"></param>
        /// <param name="whWarehouseRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="currentSystem"></param>
        /// <param name="sysConfigRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuRequistionOrderRepository"></param>
        /// <param name="manuRequistionOrderDetailRepository"></param>
        /// <param name="manuRequistionOrderReceiveRepository"></param>
        public WhMaterialPickingService(ILocalizationService localizationService,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
            IProcMaterialRepository procMaterialRepository,
            IManuReturnOrderRepository manuReturnOrderRepository,
            IManuReturnOrderDetailRepository manuReturnOrderDetailRepository,
            IWhSupplierRepository whSupplierRepository,
            IIQCOrderCreateService iqcOrderCreateService,
            IQualIqcOrderReturnRepository qualIqcOrderReturnRepository,
            IQualIqcOrderReturnDetailRepository qualIqcOrderReturnDetailRepository,
            IInteCodeRulesRepository inteCodeRulesRepository,
            IManuGenerateBarcodeService manuGenerateBarcodeService,
            IWhWarehouseRepository whWarehouseRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            ICurrentSystem currentSystem,
            ISysConfigRepository sysConfigRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuRequistionOrderRepository manuRequistionOrderRepository,
            IManuRequistionOrderDetailRepository manuRequistionOrderDetailRepository,
            IManuRequistionOrderReceiveRepository manuRequistionOrderReceiveRepository)
        {
            _localizationService = localizationService;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _procMaterialRepository = procMaterialRepository;
            _manuReturnOrderRepository = manuReturnOrderRepository;
            _manuReturnOrderDetailRepository = manuReturnOrderDetailRepository;
            _whSupplierRepository = whSupplierRepository;
            _iqcOrderCreateService = iqcOrderCreateService;
            _qualIqcOrderReturnRepository = qualIqcOrderReturnRepository;
            _qualIqcOrderReturnDetailRepository = qualIqcOrderReturnDetailRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _whWarehouseRepository = whWarehouseRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _currentSystem = currentSystem;
            _sysConfigRepository = sysConfigRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuRequistionOrderRepository = manuRequistionOrderRepository;
            _manuRequistionOrderDetailRepository = manuRequistionOrderDetailRepository;
            _manuRequistionOrderReceiveRepository = manuRequistionOrderReceiveRepository;
        }


        /// <summary>
        /// 领料单接收
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<string> MaterialPickingReceiveAsync(WhMaterialPickingReceiveDto param)
        {
            if (param.ReceiptResult == WhMaterialPickingReceiveResultEnum.CancelMaterialReceipt)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10254));
            }

            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10139));

            var siteId = long.Parse(configEntities.FirstOrDefault()?.Value ?? "0");
            var userName = param.OperateBy;

            // 1. 当前领料单状态
            var reqOrder = await _manuRequistionOrderRepository.GetByCodeAsync(new ManuRequistionOrderQuery
            {
                SiteId = siteId,
                ReqOrderCode = param.RequistionOrderCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES15160)).WithData("ReqOrderCode", param.RequistionOrderCode);

            // 发料单状态校验（收料状态已完成，不能领料）
            if (reqOrder.Status == WhMaterialPickingStatusEnum.Completed)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15161)).WithData("ReqOrderCode", param.RequistionOrderCode);
            }

            // 2. 校验物料明细
            if (param.Details == null || !param.Details.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15162)).WithData("ReqOrderCode", param.RequistionOrderCode);
            }

            // 2.1 查询物料是否在系统中存在
            var materialCodeList = param.Details.Select(m => m.MaterialCode).Distinct();
            var matQuery = new ProcMaterialsByCodeQuery
            {
                SiteId = siteId,
                MaterialCodes = materialCodeList
            };
            var materialList = await _procMaterialRepository.GetByCodesAsync(matQuery);
            if (materialList == null || materialList.Count() != materialCodeList.Count())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15163))
                    .WithData("ReqOrderCode", param.RequistionOrderCode)
                    .WithData("MaterialCodes", materialCodeList);
            }

            // 2.2 查询供应商ID
            var supplierCodes = param.Details.Where(w => !string.IsNullOrWhiteSpace(w.SupplierCode)).Select(s => s.SupplierCode ?? "");
            var supplierEntities = await _whSupplierRepository.GetByCodesAsync(new WhSuppliersByCodeQuery
            {
                SiteId = siteId,
                Codes = supplierCodes
            });

            // 3. 校验领料单和物料明细是否已经存在系统中
            var orderDetailQuery = new ManuRequistionOrderDetailQuery
            {
                SiteId = siteId,
                RequistionOrderIds = new long[] { reqOrder.Id }
            };
            var orderDetailList = await _manuRequistionOrderDetailRepository.GetManuRequistionOrderDetailEntitiesAsync(orderDetailQuery);
            if (orderDetailList == null || !orderDetailList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15164)).WithData("ReqOrderCode", param.RequistionOrderCode);
            }

            // 验证条码是否在系统中已经存在，存在报错
            var barCodes = param.Details.Where(x => !string.IsNullOrWhiteSpace(x.MaterialBarCode)).Select(x => x.MaterialBarCode);
            if (barCodes != null && barCodes.Any())
            {
                var whMaterialInventories = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
                {
                    SiteId = siteId,
                    BarCodes = barCodes
                });
                whMaterialInventories = whMaterialInventories.Where(x => x.QuantityResidue > 0);
                if (whMaterialInventories != null && whMaterialInventories.Any())
                {
                    var existsBarCodes = whMaterialInventories.Select(x => x.MaterialBarCode);
                    throw new CustomerValidationException(nameof(ErrorCode.MES15166))
                        .WithData("ReqOrderCode", param.RequistionOrderCode)
                        .WithData("BarCodes", string.Join(",", existsBarCodes));
                }
            }

            //3.1 校验传输过来的物料是否都在领料单物料明细中
            foreach (var item in param.Details)
            {
                long curMatId = materialList.FirstOrDefault(m => m.MaterialCode == item.MaterialCode)?.Id ?? 0;
                var orderDetailIdList = orderDetailList.Select(m => m.MaterialId).Distinct();
                if (orderDetailIdList.Contains(curMatId) == false)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15165))
                       .WithData("ReqOrderCode", param.RequistionOrderCode)
                       .WithData("MaterialCodes", string.Join(",", materialCodeList));
                }
            }

            //4.1 更新领料单状态
            if (param.ReceiptResult == WhMaterialPickingReceiveResultEnum.Receiving)
            {
                reqOrder.Status = WhMaterialPickingStatusEnum.Inspectioning;
            }
            else if (param.ReceiptResult == WhMaterialPickingReceiveResultEnum.Completed)
            {
                reqOrder.Status = WhMaterialPickingStatusEnum.Completed;
            }
            var userId = param.OperateBy;
            var createOn = HymsonClock.Now();

            reqOrder.UpdatedBy = userId;
            reqOrder.UpdatedOn = createOn;
            //4.2 新增领料单接收明细，台账明细，库存表
            //manu_requistion_order_receive
            List<ManuRequistionOrderReceiveEntity> receiveList = new();
            List<WhMaterialStandingbookEntity> bookList = new();
            List<WhMaterialInventoryEntity> whList = new();
            foreach (var detailDto in param.Details)
            {
                var curMatModel = materialList.FirstOrDefault(m => m.MaterialCode == detailDto.MaterialCode);
                if (string.IsNullOrWhiteSpace(detailDto.MaterialBarCode) && curMatModel != null)
                {
                    detailDto.MaterialBarCode = await GenerateOrderCodeAsync(curMatModel.Id, curMatModel.MaterialCode, siteId, userName);
                }

                long curMatId = curMatModel?.Id ?? 0;
                var curSupModel = supplierEntities.FirstOrDefault(m => m.Code == detailDto.SupplierCode);
                long curSupId = curSupModel == null ? 0 : curSupModel.Id;

                // 接收明细
                var model = new ManuRequistionOrderReceiveEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    RequistionOrderId = reqOrder.Id,
                    MaterialId = curMatId,
                    MaterialBarCode = detailDto.MaterialBarCode,
                    Qty = detailDto.Qty,
                    WarehouseId = 0,
                    Remark = param.Remark ?? "",
                    SiteId = siteId,
                    CreatedBy = param.OperateBy,
                    UpdatedBy = param.OperateBy,
                    CreatedOn = createOn,
                    UpdatedOn = createOn,
                    SupplierId = curSupId,
                    Batch = detailDto.Batch ?? ""
                };
                receiveList.Add(model);

                // 台账明细
                var bookModel = new WhMaterialStandingbookEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    MaterialCode = curMatModel?.MaterialCode ?? "",
                    MaterialName = curMatModel?.MaterialName ?? "",
                    MaterialVersion = curMatModel?.Version ?? "",
                    MaterialBarCode = detailDto.MaterialBarCode,
                    Unit = curMatModel?.Unit ?? "",
                    Quantity = detailDto.Qty,
                    Remark = param.Remark,
                    Type = WhMaterialInventoryTypeEnum.MaterialReceiving,
                    Source = MaterialInventorySourceEnum.WMS,
                    SiteId = siteId,
                    SupplierId = curSupId,
                    CreatedBy = param.OperateBy,
                    UpdatedBy = param.OperateBy,
                    CreatedOn = createOn,
                    UpdatedOn = createOn,
                    Batch = detailDto.Batch ?? ""
                };
                bookList.Add(bookModel);

                // 库存明细
                WhMaterialInventoryEntity whModel = new WhMaterialInventoryEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SupplierId = curSupId,
                    MaterialId = curMatId,
                    MaterialBarCode = detailDto.MaterialBarCode,
                    QuantityResidue = detailDto.Qty,
                    ReceivedQty = detailDto.Qty,
                    Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                    Source = MaterialInventorySourceEnum.WMS,
                    SiteId = siteId,
                    MaterialType = MaterialInventoryMaterialTypeEnum.PurchaseParts,
                    Batch = detailDto.Batch ?? "",
                    WorkOrderId = reqOrder.WorkOrderId,
                    CreatedBy = param.OperateBy,
                    UpdatedBy = param.OperateBy,
                    CreatedOn = createOn,
                    UpdatedOn = createOn
                };
                whList.Add(whModel);
            }
            //4.3 更新领料单明细中数量
            List<ManuRequistionOrderDetailEntity> updateOrderDetailList = new List<ManuRequistionOrderDetailEntity>();
            foreach (var item in materialList)
            {
                decimal sumQty = param.Details.Where(m => m.MaterialCode == item.MaterialCode).Select(m => m.Qty).Sum();
                var curOrderDetail = orderDetailList.Where(m => m.MaterialId == item.Id).FirstOrDefault();
                if (curOrderDetail != null)
                {
                    curOrderDetail.Qty = sumQty;
                    curOrderDetail.UpdatedBy = param.OperateBy;
                    curOrderDetail.UpdatedOn = HymsonClock.Now();
                    updateOrderDetailList.Add(curOrderDetail);
                }
            }

            List<ManuSfcEntity> sfcEntities = new();
            List<ManuSfcInfoEntity> sfcInfoEntities = new();
            foreach (var whMaterialInventoryEntity in whList)
            {
                var sfcId = IdGenProvider.Instance.CreateId();

                // 插入生产条码信息
                sfcEntities.Add(new ManuSfcEntity
                {
                    Id = sfcId,
                    SFC = whMaterialInventoryEntity.MaterialBarCode,
                    Qty = whMaterialInventoryEntity.QuantityResidue,
                    IsUsed = YesOrNoEnum.No,
                    Status = SfcStatusEnum.Complete,
                    Type = Core.Enums.Manufacture.SfcTypeEnum.NoProduce,
                    SiteId = siteId,
                    CreatedBy = param.OperateBy,
                    UpdatedBy = param.OperateBy,
                    CreatedOn = createOn,
                    UpdatedOn = createOn
                });

                sfcInfoEntities.Add(new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    WorkOrderId = whMaterialInventoryEntity.WorkOrderId,
                    ProductId = whMaterialInventoryEntity?.MaterialId ?? 0,
                    SfcId = sfcId,
                    IsUsed = true,
                    SiteId = siteId,
                    CreatedBy = param.OperateBy,
                    UpdatedBy = param.OperateBy,
                    CreatedOn = createOn,
                    UpdatedOn = createOn
                });
            }

            using var trans = TransactionHelper.GetTransactionScope();
            await _manuSfcRepository.InsertRangeAsync(sfcEntities);
            await _manuSfcInfoRepository.InsertsAsync(sfcInfoEntities);
            await _whMaterialInventoryRepository.InsertsAsync(whList);
            await _whMaterialStandingbookRepository.InsertsAsync(bookList);
            await _manuRequistionOrderReceiveRepository.InsertRangeAsync(receiveList);
            await _manuRequistionOrderRepository.UpdateAsync(reqOrder);
            await _manuRequistionOrderDetailRepository.UpdatesAsync(updateOrderDetailList);
            trans.Complete();

            #region 
            //2. 插入明细   manu_requistion_order_detail
            //3. 更新明细数量  
            //4. 数量更新到库存表，台账表
            //5. 需要更新领料单状态到完成么？


            //var manuReturnOrderEntity = await _manuReturnOrderRepository.GetSingleEntityAsync(new ManuReturnOrderSingleQuery
            //{
            //    SiteId = siteId,
            //    ReturnOrderCode = param.ReturnOrderCode
            //});
            //if (manuReturnOrderEntity == null)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES15144)).WithData("ReturnOrderCode", param.ReturnOrderCode);
            //}

            //if (manuReturnOrderEntity.Status != WhWarehouseMaterialReturnStatusEnum.PendingStorage)
            //{
            //    switch (manuReturnOrderEntity.Status)
            //    {
            //        case WhWarehouseMaterialReturnStatusEnum.Inspectioning:
            //            throw new CustomerValidationException(nameof(ErrorCode.MES15145)).WithData("ReturnOrderCode", param.ReturnOrderCode);
            //        case WhWarehouseMaterialReturnStatusEnum.Completed:
            //            throw new CustomerValidationException(nameof(ErrorCode.MES15146)).WithData("ReturnOrderCode", param.ReturnOrderCode);
            //        case WhWarehouseMaterialReturnStatusEnum.ApplicationSuccessful:
            //            throw new CustomerValidationException(nameof(ErrorCode.MES15147)).WithData("ReturnOrderCode", param.ReturnOrderCode);
            //    }
            //}

            //var manuReturnOrderDetails = await _manuReturnOrderDetailRepository.GetEntitiesAsync(new ManuReturnOrderDetailQuery
            //{
            //    SiteId = siteId,
            //    ReturnOrderId = manuReturnOrderEntity.Id
            //});

            //var updateManuReturnOrderStatusByIdCommand = new UpdateManuReturnOrderStatusByIdCommand
            //{
            //    Id = manuReturnOrderEntity.Id,
            //    Status = GetManuReturnOrderStatus(param.ReceiptResult),
            //    UpdatedBy = userName,
            //    UpdatedOn = HymsonClock.Now()
            //};

            //var whMaterialStandingbookEntities = new List<WhMaterialStandingbookEntity>();
            //var updateWhMaterialInventoryStatusAndQtyByIdCommands = new List<UpdateWhMaterialInventoryStatusAndQtyByIdCommand>();
            //var updateManuReturnOrderDetailIsReceivedByIdCommands = new List<UpdateManuReturnOrderDetailIsReceivedByIdCommand>();
            //if (param.ReceiptResult == WhWarehouseRequistionResultEnum.CancelMaterialReceipt)
            //{

            //    var materialInventoryEntities = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
            //    {
            //        SiteId = _currentSystem.SiteId,
            //        BarCodes = manuReturnOrderDetails.Select(x => x.MaterialBarCode)
            //    });
            //    //查询到物料信息
            //    var materialEntities = await _procMaterialRepository.GetByIdsAsync(materialInventoryEntities.Select(x => x.MaterialId));
            //    foreach (var item in manuReturnOrderDetails)
            //    {
            //        updateManuReturnOrderDetailIsReceivedByIdCommands.Add(new UpdateManuReturnOrderDetailIsReceivedByIdCommand
            //        {
            //            Id = item.Id,
            //            IsReceived = YesOrNoEnum.Yes,
            //            UpdatedBy = userName,
            //            UpdatedOn = HymsonClock.Now()
            //        });

            //        var whMaterialInventoryEntity = materialInventoryEntities.FirstOrDefault(x => x.MaterialBarCode == item.MaterialBarCode);

            //        if (whMaterialInventoryEntity == null)
            //        {
            //            throw new CustomerValidationException(nameof(ErrorCode.MES15138)).WithData("MaterialCode", item.MaterialBarCode);
            //        }

            //        var materialEntity = materialEntities.FirstOrDefault(x => x.Id == whMaterialInventoryEntity.MaterialId);
            //        if (item.IsReceived == YesOrNoEnum.Yes)
            //        {
            //            updateWhMaterialInventoryStatusAndQtyByIdCommands.Add(new UpdateWhMaterialInventoryStatusAndQtyByIdCommand
            //            {
            //                Id = whMaterialInventoryEntity.Id,
            //                Status = WhMaterialInventoryStatusEnum.ToBeUsed,
            //                Qty = item.Qty,
            //                UpdatedBy = userName,
            //                UpdatedOn = HymsonClock.Now()
            //            });

            //            whMaterialStandingbookEntities.Add(new WhMaterialStandingbookEntity
            //            {
            //                MaterialCode = materialEntity.MaterialCode ?? "",
            //                MaterialName = materialEntity.MaterialName ?? "",
            //                MaterialVersion = materialEntity.Version ?? "",
            //                Unit = materialEntity.Unit ?? "",
            //                MaterialBarCode = whMaterialInventoryEntity.MaterialBarCode,
            //                Type = WhMaterialInventoryTypeEnum.ReturnApplicationConfirm,
            //                Source = whMaterialInventoryEntity.Source,
            //                SiteId = _currentSystem.SiteId,
            //                Batch = whMaterialInventoryEntity.Batch,
            //                Quantity = whMaterialInventoryEntity.QuantityResidue,
            //                SupplierId = whMaterialInventoryEntity.SupplierId,
            //                Id = IdGenProvider.Instance.CreateId(),
            //                CreatedBy = userName,
            //                UpdatedBy = userName,
            //                CreatedOn = HymsonClock.Now(),
            //                UpdatedOn = HymsonClock.Now()
            //            });
            //        }
            //    }
            //}
            //else
            //{
            //    if (param.Details != null && param.Details.Any())
            //    {

            //        var materialInventoryEntities = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
            //        {
            //            SiteId = _currentSystem.SiteId,
            //            BarCodes = param.Details.Select(x => x.MaterialBarCode)
            //        });
            //        //查询到物料信息
            //        var materialEntities = await _procMaterialRepository.GetByIdsAsync(materialInventoryEntities.Select(x => x.MaterialId));

            //        foreach (var item in param.Details)
            //        {
            //            var manuReturnOrderDetailEntity = manuReturnOrderDetails.FirstOrDefault(x => x.MaterialBarCode == item.MaterialBarCode);

            //            if (manuReturnOrderDetailEntity == null)
            //            {
            //                throw new CustomerValidationException(nameof(ErrorCode.MES15148))
            //                    .WithData("ReturnOrderCode", param.ReturnOrderCode)
            //                    .WithData("MaterialCode", param.ReturnOrderCode);
            //            }

            //            if (manuReturnOrderDetailEntity.IsReceived == YesOrNoEnum.Yes)
            //            {
            //                throw new CustomerValidationException(nameof(ErrorCode.MES15150))
            //           .WithData("ReturnOrderCode", param.ReturnOrderCode)
            //           .WithData("MaterialCode", param.ReturnOrderCode);
            //            }
            //            if (manuReturnOrderDetailEntity.Qty != item.Qty)
            //            {
            //                throw new CustomerValidationException(nameof(ErrorCode.MES15150))
            //                         .WithData("ReturnOrderCode", param.ReturnOrderCode)
            //                         .WithData("MaterialCode", param.ReturnOrderCode)
            //                         .WithData("PlanQty", manuReturnOrderDetailEntity.Qty)
            //                         .WithData("Qty", item.Qty)
            //                         ;
            //            }

            //            updateManuReturnOrderDetailIsReceivedByIdCommands.Add(new UpdateManuReturnOrderDetailIsReceivedByIdCommand
            //            {
            //                Id = manuReturnOrderDetailEntity.Id,
            //                IsReceived = YesOrNoEnum.Yes,
            //                UpdatedBy = userName,
            //                UpdatedOn = HymsonClock.Now()
            //            });
            //            var whMaterialInventoryEntity = materialInventoryEntities.FirstOrDefault(x => x.MaterialBarCode == item.MaterialBarCode);

            //            if (whMaterialInventoryEntity == null)
            //            {
            //                throw new CustomerValidationException(nameof(ErrorCode.MES15138)).WithData("MaterialCode", item.MaterialBarCode);
            //            }

            //            var materialEntity = materialEntities.FirstOrDefault(x => x.Id == whMaterialInventoryEntity.MaterialId);

            //            updateWhMaterialInventoryStatusAndQtyByIdCommands.Add(new UpdateWhMaterialInventoryStatusAndQtyByIdCommand
            //            {
            //                Id = whMaterialInventoryEntity.Id,
            //                Status = WhMaterialInventoryStatusEnum.ToBeUsed,
            //                Qty = 0,
            //                UpdatedBy = userName,
            //                UpdatedOn = HymsonClock.Now()
            //            });

            //            whMaterialStandingbookEntities.Add(new WhMaterialStandingbookEntity
            //            {
            //                MaterialCode = materialEntity.MaterialCode ?? "",
            //                MaterialName = materialEntity.MaterialName ?? "",
            //                MaterialVersion = materialEntity.Version ?? "",
            //                Unit = materialEntity.Unit ?? "",
            //                MaterialBarCode = whMaterialInventoryEntity.MaterialBarCode,
            //                Type = WhMaterialInventoryTypeEnum.ReturnApplicationConfirm,
            //                Source = whMaterialInventoryEntity.Source,
            //                SiteId = _currentSystem.SiteId,
            //                Batch = whMaterialInventoryEntity.Batch,
            //                Quantity = whMaterialInventoryEntity.QuantityResidue,
            //                SupplierId = whMaterialInventoryEntity.SupplierId,
            //                Id = IdGenProvider.Instance.CreateId(),
            //                CreatedBy = userName,
            //                UpdatedBy = userName,
            //                CreatedOn = HymsonClock.Now(),
            //                UpdatedOn = HymsonClock.Now()
            //            });
            //        }
            //    }
            //}

            //using (var trans = TransactionHelper.GetTransactionScope())
            //{
            //    await _manuReturnOrderRepository.UpdateManuReturnOrderStatusByIdAsync(updateManuReturnOrderStatusByIdCommand);
            //    await _manuReturnOrderDetailRepository.UpdateManuReturnOrderDetailIsReceivedByIdRangeAsync(updateManuReturnOrderDetailIsReceivedByIdCommands);
            //    await _whMaterialInventoryRepository.UpdateWhMaterialInventoryStatusAndQtyByIdRangeAsync(updateWhMaterialInventoryStatusAndQtyByIdCommands);
            //    await _whMaterialStandingbookRepository.InsertsAsync(whMaterialStandingbookEntities);
            //    trans.Complete();
            //}

            //return manuReturnOrderEntity.ReturnOrderCode;
            #endregion

            return "";
        }

        /// <summary>
        /// 领料单接收（不校验物料明细）
        /// 2024.08.21 开会讨论暂定方案
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<string> MaterialPickingReceiveWithoutDetailAsync(WhMaterialPickingReceiveDto requestDto)
        {
            if (requestDto.ReceiptResult == WhMaterialPickingReceiveResultEnum.CancelMaterialReceipt)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10254));
            }

            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10139));

            var siteId = long.Parse(configEntities.FirstOrDefault()?.Value ?? "0");
            var userName = requestDto.OperateBy;

            // 1. 当前领料单状态
            var requistionOrderEntity = await _manuRequistionOrderRepository.GetByCodeAsync(new ManuRequistionOrderQuery
            {
                SiteId = siteId,
                ReqOrderCode = requestDto.RequistionOrderCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES15160)).WithData("ReqOrderCode", requestDto.RequistionOrderCode);

            // 领料单状态校验（收料状态已完成，不能领料）
            if (requistionOrderEntity.Status == WhMaterialPickingStatusEnum.Completed)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15161)).WithData("ReqOrderCode", requestDto.RequistionOrderCode);
            }

            // 读取领料单明细
            var requistionOrderDetailEntities = await _manuRequistionOrderDetailRepository.GetManuRequistionOrderDetailEntitiesAsync(new ManuRequistionOrderDetailQuery
            {
                SiteId = siteId,
                RequistionOrderIds = new long[] { requistionOrderEntity.Id }
            });

            // 读取物料
            var materialEntities = await _procMaterialRepository.GetEntitiesAsync(new ProcMaterialQuery
            {
                SiteId = siteId,
                MaterialIds = requistionOrderDetailEntities.Select(s => s.MaterialId)
            });

            // 4.1 更新领料单状态
            if (requestDto.ReceiptResult == WhMaterialPickingReceiveResultEnum.Receiving)
            {
                requistionOrderEntity.Status = WhMaterialPickingStatusEnum.Inspectioning;
            }
            else if (requestDto.ReceiptResult == WhMaterialPickingReceiveResultEnum.Completed)
            {
                requistionOrderEntity.Status = WhMaterialPickingStatusEnum.Completed;
            }

            var userId = requestDto.OperateBy;
            var createOn = HymsonClock.Now();
            requistionOrderEntity.UpdatedBy = userId;
            requistionOrderEntity.UpdatedOn = createOn;

            //4.2 新增领料单接收明细，台账明细，库存表
            //manu_requistion_order_receive
            List<ManuRequistionOrderReceiveEntity> manuRequistionOrderReceiveEntities = new();
            List<WhMaterialStandingbookEntity> whMaterialStandingbookEntities = new();
            List<WhMaterialInventoryEntity> whMaterialInventoryEntities = new();
            foreach (var detailEntity in requistionOrderDetailEntities)
            {
                var materialEntity = materialEntities.FirstOrDefault(f => f.Id == detailEntity.MaterialId);
                if (materialEntity == null) continue;

                var materialPickingDetailDto = new WhMaterialPickingDetailDto
                {
                    MaterialBarCode = await GenerateOrderCodeAsync(materialEntity.Id, materialEntity.MaterialCode, siteId, userName)
                };

                // 接收明细
                manuRequistionOrderReceiveEntities.Add(new ManuRequistionOrderReceiveEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    RequistionOrderId = requistionOrderEntity.Id,
                    MaterialId = materialEntity.Id,
                    MaterialBarCode = materialPickingDetailDto.MaterialBarCode,
                    Qty = detailEntity.Qty,
                    WarehouseId = 0,
                    Remark = requestDto.Remark ?? "",
                    SiteId = siteId,
                    CreatedBy = requestDto.OperateBy,
                    UpdatedBy = requestDto.OperateBy,
                    CreatedOn = createOn,
                    UpdatedOn = createOn,
                    SupplierId = null,
                    Batch = ""
                });

                // 台账明细
                whMaterialStandingbookEntities.Add(new WhMaterialStandingbookEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    MaterialCode = materialEntity?.MaterialCode ?? "",
                    MaterialName = materialEntity?.MaterialName ?? "",
                    MaterialVersion = materialEntity?.Version ?? "",
                    MaterialBarCode = materialPickingDetailDto.MaterialBarCode,
                    Unit = materialEntity?.Unit ?? "",
                    Quantity = detailEntity.Qty,
                    Remark = requestDto.Remark,
                    Type = WhMaterialInventoryTypeEnum.MaterialReceiving,
                    Source = MaterialInventorySourceEnum.WMS,
                    SiteId = siteId,
                    SupplierId = 0,
                    CreatedBy = requestDto.OperateBy,
                    UpdatedBy = requestDto.OperateBy,
                    CreatedOn = createOn,
                    UpdatedOn = createOn,
                    Batch = ""
                });

                // 库存明细
                whMaterialInventoryEntities.Add(new WhMaterialInventoryEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SupplierId = 0,
                    MaterialId = materialEntity!.Id,
                    MaterialBarCode = materialPickingDetailDto.MaterialBarCode,
                    QuantityResidue = detailEntity.Qty,
                    ReceivedQty = detailEntity.Qty,
                    Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                    Source = MaterialInventorySourceEnum.WMS,
                    SiteId = siteId,
                    MaterialType = MaterialInventoryMaterialTypeEnum.PurchaseParts,
                    Batch = "",
                    WorkOrderId = requistionOrderEntity.WorkOrderId,
                    CreatedBy = requestDto.OperateBy,
                    UpdatedBy = requestDto.OperateBy,
                    CreatedOn = createOn,
                    UpdatedOn = createOn
                });
            }

            List<ManuSfcEntity> sfcEntities = new();
            List<ManuSfcInfoEntity> sfcInfoEntities = new();
            foreach (var whMaterialInventoryEntity in whMaterialInventoryEntities)
            {
                var sfcId = IdGenProvider.Instance.CreateId();

                // 插入生产条码信息
                sfcEntities.Add(new ManuSfcEntity
                {
                    Id = sfcId,
                    SFC = whMaterialInventoryEntity.MaterialBarCode,
                    Qty = whMaterialInventoryEntity.QuantityResidue,
                    IsUsed = YesOrNoEnum.No,
                    Status = SfcStatusEnum.Complete,
                    Type = Core.Enums.Manufacture.SfcTypeEnum.NoProduce,
                    SiteId = siteId,
                    CreatedBy = requestDto.OperateBy,
                    UpdatedBy = requestDto.OperateBy,
                    CreatedOn = createOn,
                    UpdatedOn = createOn
                });

                sfcInfoEntities.Add(new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    WorkOrderId = whMaterialInventoryEntity.WorkOrderId,
                    ProductId = whMaterialInventoryEntity?.MaterialId ?? 0,
                    SfcId = sfcId,
                    IsUsed = true,
                    SiteId = siteId,
                    CreatedBy = requestDto.OperateBy,
                    UpdatedBy = requestDto.OperateBy,
                    CreatedOn = createOn,
                    UpdatedOn = createOn
                });
            }

            using var trans = TransactionHelper.GetTransactionScope();
            await _manuSfcRepository.InsertRangeAsync(sfcEntities);
            await _manuSfcInfoRepository.InsertsAsync(sfcInfoEntities);
            await _whMaterialInventoryRepository.InsertsAsync(whMaterialInventoryEntities);
            await _whMaterialStandingbookRepository.InsertsAsync(whMaterialStandingbookEntities);
            await _manuRequistionOrderReceiveRepository.InsertRangeAsync(manuRequistionOrderReceiveEntities);
            await _manuRequistionOrderRepository.UpdateAsync(requistionOrderEntity);
            trans.Complete();

            return "";
        }

        /// <summary>
        /// 退料单状态装换
        /// </summary>
        /// <param name="ReceiptResult"></param>
        /// <returns></returns>
        private WhWarehouseMaterialReturnStatusEnum GetManuReturnOrderStatus(WhWarehouseRequistionResultEnum ReceiptResult)
        {
            var status = WhWarehouseMaterialReturnStatusEnum.PendingStorage;
            switch (ReceiptResult)
            {
                case WhWarehouseRequistionResultEnum.Receiving:
                    status = WhWarehouseMaterialReturnStatusEnum.InStorage;
                    break;
                case WhWarehouseRequistionResultEnum.Completed:
                    status = WhWarehouseMaterialReturnStatusEnum.Completed;
                    break;
                case WhWarehouseRequistionResultEnum.CancelMaterialReceipt:
                    status = WhWarehouseMaterialReturnStatusEnum.CancelMaterialReturn;
                    break;
            }
            return status;
        }

        /// <summary>
        /// 物料条码生成
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="materialCode"></param>
        /// <param name="siteId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private async Task<string> GenerateOrderCodeAsync(long productId, string materialCode, long siteId, string userName)
        {
            var inteCodeRulesEntity = await _inteCodeRulesRepository.GetInteCodeRulesByProductIdAsync(new InteCodeRulesByProductQuery
            {
                ProductId = productId,
                CodeType = CodeRuleCodeTypeEnum.ProcessControlSeqCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16501)).WithData("product", materialCode);

            var orderCodes = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new GenerateBarcodeBo
            {
                SiteId = siteId,
                UserName = userName,
                CodeRuleId = inteCodeRulesEntity.Id,
                Count = 1
            });
            return orderCodes.First();
        }
    }
}
