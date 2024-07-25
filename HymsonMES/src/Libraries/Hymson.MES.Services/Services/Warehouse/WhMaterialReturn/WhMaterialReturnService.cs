using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Services.Quality;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Data.Repositories.WhWareHouse;
using Hymson.MES.HttpClients;
using Hymson.MES.HttpClients.Options;
using Hymson.MES.HttpClients.Requests.XnebulaWMS;
using Hymson.MES.Services.Dtos.Manufacture.WhMaterialReturn;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Services.Services.Warehouse.WhMaterialReturn
{
    /// <summary>
    /// 仓库退料
    /// </summary>
    public class WhMaterialReturnService : IWhMaterialReturnService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

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

        private readonly IOptions<WMSOptions> _options;
        /// <summary>
        /// 服务接口（检验单生成）
        /// </summary>
        private readonly IIQCOrderCreateService _iqcOrderCreateService;

        private readonly IWMSApiClient _wmsRequest;

        private readonly IQualIqcOrderReturnRepository _qualIqcOrderReturnRepository;

        private readonly IQualIqcOrderReturnDetailRepository _qualIqcOrderReturnDetailRepository;

        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;

        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;

        private readonly IWhWarehouseRepository _whWarehouseRepository;

        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="localizationService"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="manuReturnOrderRepository"></param>
        /// <param name="manuReturnOrderDetailRepository"></param>
        /// <param name="whSupplierRepository"></param>
        /// <param name="iQCOrderCreateService"></param>
        /// <param name="options"></param>
        /// <param name="xnebulaWMSApiClient"></param>
        /// <param name="qualIqcOrderReturnRepository"></param>
        /// <param name="qualIqcOrderReturnDetailRepository"></param>
        /// <param name="inteCodeRulesRepository"></param>
        /// <param name="manuGenerateBarcodeService"></param>
        /// <param name="whWarehouseRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        public WhMaterialReturnService(ICurrentUser currentUser, ICurrentSite currentSite,
            ILocalizationService localizationService,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
            IProcMaterialRepository procMaterialRepository,
            IManuReturnOrderRepository manuReturnOrderRepository,
            IManuReturnOrderDetailRepository manuReturnOrderDetailRepository,
            IWhSupplierRepository whSupplierRepository,
            IIQCOrderCreateService iQCOrderCreateService,
            IOptions<WMSOptions> options, IWMSApiClient xnebulaWMSApiClient,
            IQualIqcOrderReturnRepository qualIqcOrderReturnRepository,
            IQualIqcOrderReturnDetailRepository qualIqcOrderReturnDetailRepository,
            IInteCodeRulesRepository inteCodeRulesRepository,
            IManuGenerateBarcodeService manuGenerateBarcodeService,
            IWhWarehouseRepository whWarehouseRepository,
            IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _localizationService = localizationService;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _procMaterialRepository = procMaterialRepository;
            _manuReturnOrderRepository = manuReturnOrderRepository;
            _manuReturnOrderDetailRepository = manuReturnOrderDetailRepository;
            _whSupplierRepository = whSupplierRepository;
            _iqcOrderCreateService = iQCOrderCreateService;
            _options = options;
            _wmsRequest = xnebulaWMSApiClient;
            _qualIqcOrderReturnRepository = qualIqcOrderReturnRepository;
            _qualIqcOrderReturnDetailRepository = qualIqcOrderReturnDetailRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _whWarehouseRepository = whWarehouseRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
        }

        /// <summary>
        /// 物料退料
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task MaterialReturnAsync(MaterialReturnDto param)
        {
            var whWarehouseEntity = await _whWarehouseRepository.GetOneAsync(new Data.Repositories.WhWareHouse.Query.WhWarehouseQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Code = _options.Value.Receipt.WarehouseCode
            });

            if (whWarehouseEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15142));
            }
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(param.WorkOrderId);
            var materialInventoryEntities = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                BarCodes = param.MaterialBarCodes
            });

            if (materialInventoryEntities == null || !materialInventoryEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15124));
            }

            //查询到物料信息
            var materialEntities = await _procMaterialRepository.GetByIdsAsync(materialInventoryEntities.Select(x => x.MaterialId));
            var whMaterialStandingbookEntities = new List<WhMaterialStandingbookEntity>();
            var updateAndCheckStatusByIdCommands = new List<UpdateAndCheckStatusByIdCommand>();

            var manuReturnOrderDetailEntities = new List<ManuReturnOrderDetailEntity>();

            //检验单
            QualIqcOrderReturnEntity qualIqcOrderReturnEntity = new();
            // 检验单明细
            List<QualIqcOrderReturnDetailEntity> qualIqcOrderReturnDetailEntities = new();

            var warehousingEntryDto = new WarehousingEntryDto()
            {
                Type = BillBusinessTypeEnum.WorkOrderMaterialReturnForm,
                IsAutoExecute = param.Type == ManuReturnTypeEnum.WorkOrderBorrow,
                CreatedBy = _currentUser.UserName,
                WarehouseCode = _options.Value.Receipt.WarehouseCode,
                Remark = param.Remark,
            };

            var warehousingEntryDetails = new List<ReceiptDetailDto>();

            var returnOrderCode = await GenerateMaintenanceOrderCodeAsync(_currentSite.SiteId ?? 0, _currentUser.UserName);

            //创建领料申请单
            ManuReturnOrderEntity manuReturnOrderEntity = new ManuReturnOrderEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                ReturnOrderCode = returnOrderCode,
                WorkOrderId = param.WorkOrderId,
                Status = WhWarehouseMaterialReturnStatusEnum.ApplicationSuccessful,
                ReceiveWarehouseId = whWarehouseEntity.Id,
                Type = param.Type,
                Remark = param.Remark,
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedOn = HymsonClock.Now()
            };

            if (param.Type == ManuReturnTypeEnum.WorkOrderReturn)
            {
                // 生成检验单号
                var inspectionOrder = await _iqcOrderCreateService.GenerateCommonIQCOrderCodeAsync(new CoreServices.Bos.Common.BaseBo
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    User = _currentUser.UserName,
                });

                qualIqcOrderReturnEntity = new QualIqcOrderReturnEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    InspectionOrder = inspectionOrder,
                    ReturnOrderId = manuReturnOrderEntity.Id,
                    WorkOrderId = materialInventoryEntities.FirstOrDefault()?.WorkOrderId,
                    Status = IQCLiteStatusEnum.WaitInspect,
                    IsQualified = null,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now()
                };
            }

            foreach (var materialBarCode in param.MaterialBarCodes)
            {
                var whMaterialInventoryEntity = materialInventoryEntities.FirstOrDefault(x => x.MaterialBarCode == materialBarCode);

                if (whMaterialInventoryEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15138)).WithData("MaterialCode", materialBarCode);
                }

                if (whMaterialInventoryEntity.QuantityResidue == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15138)).WithData("MaterialCode", materialBarCode);
                }

                if (whMaterialInventoryEntity.Status != WhMaterialInventoryStatusEnum.ToBeUsed)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15143)).WithData("MaterialCode", materialBarCode);
                }

                updateAndCheckStatusByIdCommands.Add(new UpdateAndCheckStatusByIdCommand
                {
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    Status = WhMaterialInventoryStatusEnum.InUse,
                    CurrentStatus = whMaterialInventoryEntity.Status,
                    Id = whMaterialInventoryEntity.Id
                });

                var materialEntity = materialEntities.FirstOrDefault(x => x.Id == whMaterialInventoryEntity.MaterialId);

                whMaterialStandingbookEntities.Add(new WhMaterialStandingbookEntity
                {
                    MaterialCode = materialEntity.MaterialCode ?? "",
                    MaterialName = materialEntity.MaterialName ?? "",
                    MaterialVersion = materialEntity.Version ?? "",
                    Unit = materialEntity.Unit ?? "",
                    MaterialBarCode = whMaterialInventoryEntity.MaterialBarCode,
                    Type = WhMaterialInventoryTypeEnum.ReturnApplication,
                    Source = whMaterialInventoryEntity.Source,
                    SiteId = _currentSite.SiteId ?? 0,
                    Batch = whMaterialInventoryEntity.Batch,
                    Quantity = whMaterialInventoryEntity.QuantityResidue,
                    SupplierId = whMaterialInventoryEntity.SupplierId,
                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now()
                });

                var manuReturnOrderDetailEntity = new ManuReturnOrderDetailEntity
                {
                    ReturnOrderId = manuReturnOrderEntity.Id,
                    MaterialId = materialEntity.Id,
                    WarehouseId = whWarehouseEntity.Id,
                    MaterialBarCode = materialBarCode,
                    Batch = whMaterialInventoryEntity.Batch,
                    Qty = whMaterialInventoryEntity.QuantityResidue,
                    SupplierId = whMaterialInventoryEntity.SupplierId,
                    ExpirationDate = whMaterialInventoryEntity.DueDate,
                    SiteId = whMaterialInventoryEntity.SiteId,
                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now()
                };

                manuReturnOrderDetailEntities.Add(manuReturnOrderDetailEntity);

                if (param.Type == ManuReturnTypeEnum.WorkOrderReturn)
                {
                    var qualIqcOrderReturnDetailEntity = new QualIqcOrderReturnDetailEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        IQCOrderId = qualIqcOrderReturnEntity.Id,
                        ReturnOrderDetailId = manuReturnOrderDetailEntity.Id,
                        BarCode = materialBarCode,
                        MaterialId = whMaterialInventoryEntity.MaterialId,
                        IsQualified = null,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now()
                    };

                    qualIqcOrderReturnDetailEntities.Add(qualIqcOrderReturnDetailEntity);
                }

                warehousingEntryDetails.Add(new ReceiptDetailDto
                {
                    ProductionOrderNumber = planWorkOrderEntity?.OrderCode,
                    SyncId = manuReturnOrderDetailEntity.Id,
                    MaterialCode = materialEntity.MaterialCode,
                    LotCode = whMaterialInventoryEntity.Batch,
                    UnitCode = materialEntity.Unit,
                    UniqueCode = materialBarCode,
                    Quantity = whMaterialInventoryEntity.QuantityResidue
                });
            }
            warehousingEntryDto.Details = warehousingEntryDetails;

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                var row = await _whMaterialInventoryRepository.UpdateAndCheckStatusByIdAsync(updateAndCheckStatusByIdCommands);
                if (row != materialInventoryEntities.Count())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15137));
                }
                await _whMaterialStandingbookRepository.InsertsAsync(whMaterialStandingbookEntities);
                await _manuReturnOrderRepository.InsertAsync(manuReturnOrderEntity);
                await _manuReturnOrderDetailRepository.InsertRangeAsync(manuReturnOrderDetailEntities);

                if (param.Type == ManuReturnTypeEnum.WorkOrderReturn)
                {
                    await _qualIqcOrderReturnRepository.InsertAsync(qualIqcOrderReturnEntity);
                    await _qualIqcOrderReturnDetailRepository.InsertRangeAsync(qualIqcOrderReturnDetailEntities);
                }

                var response = await _wmsRequest.WarehousingEntryRequestAsync(warehousingEntryDto);
                if (!response)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15139)).WithData("System", "WMS");
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 退料单号生成
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        private async Task<string> GenerateMaintenanceOrderCodeAsync(long siteId, string userName)
        {
            var codeRules = await _inteCodeRulesRepository.GetListAsync(new InteCodeRulesReQuery
            {
                SiteId = siteId,
                CodeType = Core.Enums.Integrated.CodeRuleCodeTypeEnum.MaterialReturnOrder
            });
            if (codeRules == null || !codeRules.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15140));
            }
            if (codeRules.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12312));
            }

            var orderCodes = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new GenerateBarcodeBo
            {
                SiteId = siteId,
                UserName = userName,
                CodeRuleId = codeRules.First().Id,
                Count = 1
            });

            return orderCodes.First();
        }
    }
}
