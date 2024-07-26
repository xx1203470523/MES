using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Services.Quality;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuRequistionOrder;
using Hymson.MES.Data.Repositories.Manufacture.ManuReturnOrder.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuReturnOrderDetail.Command;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Data.Repositories.WhWareHouse;
using Hymson.MES.SystemServices.Dtos.Warehouse;
using Hymson.MES.SystemServices.Services.Integrated;
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

        private readonly IManuRequistionOrderRepository _manuRequistionOrderRepository;

        private readonly IManuRequistionOrderDetailRepository _manuRequistionOrderDetailRepository;

        private readonly IManuRequistionOrderReceiveRepository _manuRequistionOrderReceiveRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WhMaterialPickingService(
            ILocalizationService localizationService, IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository, IProcMaterialRepository procMaterialRepository,
            IManuReturnOrderRepository manuReturnOrderRepository, IManuReturnOrderDetailRepository manuReturnOrderDetailRepository,
            IWhSupplierRepository whSupplierRepository, IIQCOrderCreateService iqcOrderCreateService, IQualIqcOrderReturnRepository qualIqcOrderReturnRepository,
            IQualIqcOrderReturnDetailRepository qualIqcOrderReturnDetailRepository, IInteCodeRulesRepository inteCodeRulesRepository, IManuGenerateBarcodeService manuGenerateBarcodeService,
            IWhWarehouseRepository whWarehouseRepository, IPlanWorkOrderRepository planWorkOrderRepository, ICurrentSystem currentSystem, ISysConfigRepository sysConfigRepository,
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
            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10139));

            var siteId = long.Parse(configEntities.FirstOrDefault()?.Value ?? "0"); //30654441397841920;//
            var userName = param.OperateBy;

            //1. 当前领料单状态
            ManuRequistionOrderQuery reqQuery = new ManuRequistionOrderQuery();
            reqQuery.SiteId = siteId;
            reqQuery.ReqOrderCode = param.RequistionOrderCode;
            var reqOrder = await _manuRequistionOrderRepository.GetByCodeAsync(reqQuery);
            if (reqOrder == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15160)).WithData("ReqOrderCode", param.RequistionOrderCode);
            }
            if (reqOrder.Status == WhMaterialPickingStatusEnum.Completed)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15161)).WithData("ReqOrderCode", param.RequistionOrderCode);
            }

            //2. 校验物料明细
            if (param.Details == null || param.Details.Count() == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15162)).WithData("ReqOrderCode", param.RequistionOrderCode);
            }

            //2.1 查询物料是否在系统中存在
            List<string> materialCodeList = param.Details.Select(m => m.MaterialCode).Distinct().ToList();
            ProcMaterialsByCodeQuery matQuery = new ProcMaterialsByCodeQuery();
            matQuery.SiteId = siteId;
            matQuery.MaterialCodes = materialCodeList;
            var materialList = await _procMaterialRepository.GetByCodesAsync(matQuery);
            if (materialList == null || materialList.Count() != materialCodeList.Count)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15163))
                    .WithData("ReqOrderCode", param.RequistionOrderCode)
                    .WithData("MaterialCodes", materialCodeList);
            }

            //2.2 查询供应商ID
            List<string> supCodeList = param.Details.Where(m => string.IsNullOrEmpty(m.SupplierCode))
                .Select(m => m.SupplierCode).Distinct().ToList();
            WhSuppliersByCodeQuery supQuery = new WhSuppliersByCodeQuery();
            supQuery.SiteId = siteId;
            supQuery.Codes = supCodeList;
            var supList = await _whSupplierRepository.GetByCodesAsync(supQuery);

            //3. 校验领料单和物料明细是否已经存在系统中
            ManuRequistionOrderDetailQuery orderDetailQuery = new ManuRequistionOrderDetailQuery();
            orderDetailQuery.SiteId = siteId;
            orderDetailQuery.RequistionOrderIds = new long[] { reqOrder.Id };
            var orderDetailList = await _manuRequistionOrderDetailRepository.GetManuRequistionOrderDetailEntitiesAsync(orderDetailQuery);
            if (orderDetailList == null || orderDetailList.Count() == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15164)).WithData("ReqOrderCode", param.RequistionOrderCode);
            }

            //3.1 校验传输过来的物料是否都在领料单物料明细中
            foreach (var item in param.Details)
            {
                long curMatId = materialList.Where(m => m.MaterialCode == item.MaterialCode).FirstOrDefault()!.Id;
                List<long> orderDetailIdList = orderDetailList.Select(m => m.MaterialId).Distinct().ToList();
                if (orderDetailIdList.Contains(curMatId) == false)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15165))
                       .WithData("ReqOrderCode", param.RequistionOrderCode)
                       .WithData("MaterialCodes", materialCodeList);
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
            var userId= param.OperateBy;
            var createOn = HymsonClock.Now();

            reqOrder.UpdatedBy = userId;
            reqOrder.UpdatedOn = createOn;
            //4.2 新增领料单接收明细，台账明细，库存表
            //manu_requistion_order_receive
            List<ManuRequistionOrderReceiveEntity> receiveList = new List<ManuRequistionOrderReceiveEntity>();
            List<WhMaterialStandingbookEntity> bookList = new List<WhMaterialStandingbookEntity>();
            List<WhMaterialInventoryEntity> whList = new List<WhMaterialInventoryEntity>();
            foreach (var item in param.Details)
            {
                var curMatModel = materialList.Where(m => m.MaterialCode == item.MaterialCode).FirstOrDefault();
                long curMatId = curMatModel?.Id??0;
                var curSupModel = supList.Where(m => m.Code == item.SupplierCode).FirstOrDefault();
                long curSupId = curSupModel == null ? 0 : curSupModel.Id;
                //接收明细
                ManuRequistionOrderReceiveEntity model = new ManuRequistionOrderReceiveEntity();
                model.Id = IdGenProvider.Instance.CreateId();
                model.RequistionOrderId = reqOrder.Id;
                model.MaterialId = curMatId;
                model.MaterialBarCode = item.MaterialBarCode;
                model.Qty = item.Qty;
                model.WarehouseId = 0;
                model.Remark = param.Remark??"";
                model.SiteId = siteId;
                model.CreatedBy = param.OperateBy;
                model.UpdatedBy = param.OperateBy;
                model.CreatedOn = HymsonClock.Now();
                model.UpdatedOn = model.CreatedOn;
                model.SupplierId = curSupId;
                model.Batch = item.Batch ?? "";
                receiveList.Add(model);
                //台账明细
                WhMaterialStandingbookEntity bookModel = new WhMaterialStandingbookEntity();
                bookModel.Id = IdGenProvider.Instance.CreateId();
                bookModel.MaterialCode = curMatModel?.MaterialCode??"";
                bookModel.MaterialName = curMatModel?.MaterialName??"";
                bookModel.MaterialVersion = curMatModel?.Version ?? "";
                bookModel.MaterialBarCode = item.MaterialBarCode;
                bookModel.Unit = curMatModel?.Unit ?? "";
                bookModel.Quantity = item.Qty;
                bookModel.Remark = param.Remark;
                bookModel.Type = WhMaterialInventoryTypeEnum.MaterialReceiving;
                bookModel.Source = MaterialInventorySourceEnum.WMS;
                bookModel.SiteId = siteId;
                bookModel.SupplierId = curSupId;
                bookModel.CreatedBy = param.OperateBy;
                bookModel.UpdatedBy = param.OperateBy;
                bookModel.CreatedOn = HymsonClock.Now();
                bookModel.UpdatedOn = bookModel.CreatedOn;
                bookModel.Batch = item.Batch ?? "";
                bookList.Add(bookModel);
                //库存明细
                WhMaterialInventoryEntity whModel = new WhMaterialInventoryEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SupplierId = curSupId,
                    MaterialId = curMatId,
                    MaterialBarCode = item.MaterialBarCode,
                    QuantityResidue = item.Qty,
                    ReceivedQty = item.Qty,
                    Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                    Source = MaterialInventorySourceEnum.WMS,
                    SiteId = siteId,
                    MaterialType = MaterialInventoryMaterialTypeEnum.PurchaseParts,
                    Batch = item.Batch ?? "",
                    WorkOrderId = reqOrder.WorkOrderId,
                    CreatedBy = param.OperateBy,
                    UpdatedBy = param.OperateBy,
                    CreatedOn = createOn,
                    UpdatedOn=createOn
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
                    curOrderDetail.Qty += sumQty;
                    curOrderDetail.UpdatedBy = param.OperateBy;
                    curOrderDetail.UpdatedOn = HymsonClock.Now();
                    updateOrderDetailList.Add(curOrderDetail);
                }
            }

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                await _whMaterialInventoryRepository.InsertsAsync(whList);
                await _whMaterialStandingbookRepository.InsertsAsync(bookList);
                await _manuRequistionOrderReceiveRepository.InsertRangeAsync(receiveList);
                await _manuRequistionOrderRepository.UpdateAsync(reqOrder);
                await _manuRequistionOrderDetailRepository.UpdatesAsync(updateOrderDetailList);
                trans.Complete();
            }

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
    }
}
