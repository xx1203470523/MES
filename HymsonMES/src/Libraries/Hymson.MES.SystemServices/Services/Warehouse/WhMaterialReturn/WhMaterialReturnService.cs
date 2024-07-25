using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Services.Quality;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
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
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.SystemServices.Services.Warehouse.WhMaterialReturn
{
    /// <summary>
    /// 退料单
    /// </summary>
    public class WhMaterialReturnService : IWhMaterialReturnService
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

        /// <summary>
        /// 
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
        public WhMaterialReturnService(
            ILocalizationService localizationService, IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository, IProcMaterialRepository procMaterialRepository,
            IManuReturnOrderRepository manuReturnOrderRepository, IManuReturnOrderDetailRepository manuReturnOrderDetailRepository,
            IWhSupplierRepository whSupplierRepository, IIQCOrderCreateService iqcOrderCreateService, IQualIqcOrderReturnRepository qualIqcOrderReturnRepository,
            IQualIqcOrderReturnDetailRepository qualIqcOrderReturnDetailRepository, IInteCodeRulesRepository inteCodeRulesRepository, IManuGenerateBarcodeService manuGenerateBarcodeService,
            IWhWarehouseRepository whWarehouseRepository, IPlanWorkOrderRepository planWorkOrderRepository, ICurrentSystem currentSystem, ISysConfigRepository sysConfigRepository)
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
        }

        /// <summary>
        /// 退料单确认
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<string> WhMaterialReturnConfirmAsync(WhMaterialReturnConfirmDto param)
        {

            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10139));

            var siteId = 30654441397841920;//long.Parse(configEntities.FirstOrDefault()?.Value ?? "0");
            var userName = param.OperateBy;

            var manuReturnOrderEntity = await _manuReturnOrderRepository.GetSingleEntityAsync(new ManuReturnOrderSingleQuery
            {
                SiteId = siteId,
                ReturnOrderCode = param.ReturnOrderCode
            });
            if (manuReturnOrderEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15144)).WithData("ReturnOrderCode", param.ReturnOrderCode);
            }

            if (manuReturnOrderEntity.Status != WhWarehouseMaterialReturnStatusEnum.PendingStorage)
            {
                switch (manuReturnOrderEntity.Status)
                {
                    case WhWarehouseMaterialReturnStatusEnum.Inspectioning:
                        throw new CustomerValidationException(nameof(ErrorCode.MES15145)).WithData("ReturnOrderCode", param.ReturnOrderCode);
                    case WhWarehouseMaterialReturnStatusEnum.Completed:
                        throw new CustomerValidationException(nameof(ErrorCode.MES15146)).WithData("ReturnOrderCode", param.ReturnOrderCode);
                    case WhWarehouseMaterialReturnStatusEnum.ApplicationSuccessful:
                        throw new CustomerValidationException(nameof(ErrorCode.MES15147)).WithData("ReturnOrderCode", param.ReturnOrderCode);
                }
            }

            var manuReturnOrderDetails = await _manuReturnOrderDetailRepository.GetEntitiesAsync(new ManuReturnOrderDetailQuery
            {
                SiteId = siteId,
                ReturnOrderId = manuReturnOrderEntity.Id
            });

            var updateManuReturnOrderStatusByIdCommand = new UpdateManuReturnOrderStatusByIdCommand
            {
                Id = manuReturnOrderEntity.Id,
                Status = GetManuReturnOrderStatus(param.ReceiptResult),
                UpdatedBy = userName,
                UpdatedOn = HymsonClock.Now()
            };

            var whMaterialStandingbookEntities = new List<WhMaterialStandingbookEntity>();
            var updateWhMaterialInventoryStatusAndQtyByIdCommands = new List<UpdateWhMaterialInventoryStatusAndQtyByIdCommand>();
            var updateManuReturnOrderDetailIsReceivedByIdCommands = new List<UpdateManuReturnOrderDetailIsReceivedByIdCommand>();
            if (param.ReceiptResult == WhWarehouseRequistionResultEnum.CancelMaterialReceipt)
            {

                var materialInventoryEntities = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
                {
                    SiteId = _currentSystem.SiteId,
                    BarCodes = manuReturnOrderDetails.Select(x => x.MaterialBarCode)
                });
                //查询到物料信息
                var materialEntities = await _procMaterialRepository.GetByIdsAsync(materialInventoryEntities.Select(x => x.MaterialId));
                foreach (var item in manuReturnOrderDetails)
                {
                    updateManuReturnOrderDetailIsReceivedByIdCommands.Add(new UpdateManuReturnOrderDetailIsReceivedByIdCommand
                    {
                        Id = item.Id,
                        IsReceived = YesOrNoEnum.Yes,
                        UpdatedBy = userName,
                        UpdatedOn = HymsonClock.Now()
                    });

                    var whMaterialInventoryEntity = materialInventoryEntities.FirstOrDefault(x => x.MaterialBarCode == item.MaterialBarCode);

                    if (whMaterialInventoryEntity == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES15138)).WithData("MaterialCode", item.MaterialBarCode);
                    }

                    var materialEntity = materialEntities.FirstOrDefault(x => x.Id == whMaterialInventoryEntity.MaterialId);
                    if (item.IsReceived == YesOrNoEnum.Yes)
                    {
                        updateWhMaterialInventoryStatusAndQtyByIdCommands.Add(new UpdateWhMaterialInventoryStatusAndQtyByIdCommand
                        {
                            Id = whMaterialInventoryEntity.Id,
                            Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                            Qty = item.Qty,
                            UpdatedBy = userName,
                            UpdatedOn = HymsonClock.Now()
                        });

                        whMaterialStandingbookEntities.Add(new WhMaterialStandingbookEntity
                        {
                            MaterialCode = materialEntity.MaterialCode ?? "",
                            MaterialName = materialEntity.MaterialName ?? "",
                            MaterialVersion = materialEntity.Version ?? "",
                            Unit = materialEntity.Unit ?? "",
                            MaterialBarCode = whMaterialInventoryEntity.MaterialBarCode,
                            Type = WhMaterialInventoryTypeEnum.ReturnApplicationConfirm,
                            Source = whMaterialInventoryEntity.Source,
                            SiteId = _currentSystem.SiteId,
                            Batch = whMaterialInventoryEntity.Batch,
                            Quantity = whMaterialInventoryEntity.QuantityResidue,
                            SupplierId = whMaterialInventoryEntity.SupplierId,
                            Id = IdGenProvider.Instance.CreateId(),
                            CreatedBy = userName,
                            UpdatedBy = userName,
                            CreatedOn = HymsonClock.Now(),
                            UpdatedOn = HymsonClock.Now()
                        });
                    }
                }
            }
            else
            {
                if (param.Details != null && param.Details.Any())
                {

                    var materialInventoryEntities = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
                    {
                        SiteId = _currentSystem.SiteId,
                        BarCodes = param.Details.Select(x => x.MaterialBarCode)
                    });
                    //查询到物料信息
                    var materialEntities = await _procMaterialRepository.GetByIdsAsync(materialInventoryEntities.Select(x => x.MaterialId));

                    foreach (var item in param.Details)
                    {
                        var manuReturnOrderDetailEntity = manuReturnOrderDetails.FirstOrDefault(x => x.MaterialBarCode == item.MaterialBarCode);

                        if (manuReturnOrderDetailEntity == null)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES15148))
                                .WithData("ReturnOrderCode", param.ReturnOrderCode)
                                .WithData("MaterialCode", param.ReturnOrderCode);
                        }

                        if (manuReturnOrderDetailEntity.IsReceived == YesOrNoEnum.Yes)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES15150))
                       .WithData("ReturnOrderCode", param.ReturnOrderCode)
                       .WithData("MaterialCode", param.ReturnOrderCode);
                        }
                        if (manuReturnOrderDetailEntity.Qty != item.Qty)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES15150))
                                     .WithData("ReturnOrderCode", param.ReturnOrderCode)
                                     .WithData("MaterialCode", param.ReturnOrderCode)
                                     .WithData("PlanQty", manuReturnOrderDetailEntity.Qty)
                                     .WithData("Qty", item.Qty)
                                     ;
                        }

                        updateManuReturnOrderDetailIsReceivedByIdCommands.Add(new UpdateManuReturnOrderDetailIsReceivedByIdCommand
                        {
                            Id = manuReturnOrderDetailEntity.Id,
                            IsReceived = YesOrNoEnum.Yes,
                            UpdatedBy = userName,
                            UpdatedOn = HymsonClock.Now()
                        });
                        var whMaterialInventoryEntity = materialInventoryEntities.FirstOrDefault(x => x.MaterialBarCode == item.MaterialBarCode);

                        if (whMaterialInventoryEntity == null)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES15138)).WithData("MaterialCode", item.MaterialBarCode);
                        }

                        var materialEntity = materialEntities.FirstOrDefault(x => x.Id == whMaterialInventoryEntity.MaterialId);

                        updateWhMaterialInventoryStatusAndQtyByIdCommands.Add(new UpdateWhMaterialInventoryStatusAndQtyByIdCommand
                        {
                            Id = whMaterialInventoryEntity.Id,
                            Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                            Qty = 0,
                            UpdatedBy = userName,
                            UpdatedOn = HymsonClock.Now()
                        });

                        whMaterialStandingbookEntities.Add(new WhMaterialStandingbookEntity
                        {
                            MaterialCode = materialEntity.MaterialCode ?? "",
                            MaterialName = materialEntity.MaterialName ?? "",
                            MaterialVersion = materialEntity.Version ?? "",
                            Unit = materialEntity.Unit ?? "",
                            MaterialBarCode = whMaterialInventoryEntity.MaterialBarCode,
                            Type = WhMaterialInventoryTypeEnum.ReturnApplicationConfirm,
                            Source = whMaterialInventoryEntity.Source,
                            SiteId = _currentSystem.SiteId,
                            Batch = whMaterialInventoryEntity.Batch,
                            Quantity = whMaterialInventoryEntity.QuantityResidue,
                            SupplierId = whMaterialInventoryEntity.SupplierId,
                            Id = IdGenProvider.Instance.CreateId(),
                            CreatedBy = userName,
                            UpdatedBy = userName,
                            CreatedOn = HymsonClock.Now(),
                            UpdatedOn = HymsonClock.Now()
                        });
                    }
                }
            }

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                await _manuReturnOrderRepository.UpdateManuReturnOrderStatusByIdAsync(updateManuReturnOrderStatusByIdCommand);
                await _manuReturnOrderDetailRepository.UpdateManuReturnOrderDetailIsReceivedByIdRangeAsync(updateManuReturnOrderDetailIsReceivedByIdCommands);
                await _whMaterialInventoryRepository.UpdateWhMaterialInventoryStatusAndQtyByIdRangeAsync(updateWhMaterialInventoryStatusAndQtyByIdCommands);
                await _whMaterialStandingbookRepository.InsertsAsync(whMaterialStandingbookEntities);
                trans.Complete();
            }

            return manuReturnOrderEntity.ReturnOrderCode;
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
