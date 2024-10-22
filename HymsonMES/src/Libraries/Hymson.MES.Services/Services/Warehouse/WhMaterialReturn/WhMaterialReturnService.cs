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
using Hymson.MES.CoreServices.Bos.Common;
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

        private readonly IPlanWorkPlanRepository _planWorkPlanRepository;

        private readonly IPlanWorkPlanMaterialRepository _planWorkPlanMaterialRepository;

        /// <summary>
        /// 构造函数
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
        /// <param name="planWorkPlanRepository"></param>
        /// <param name="planWorkPlanMaterialRepository"></param>
        public WhMaterialReturnService(ICurrentUser currentUser, ICurrentSite currentSite,
            ILocalizationService localizationService,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
            IProcMaterialRepository procMaterialRepository,
            IManuReturnOrderRepository manuReturnOrderRepository,
            IManuReturnOrderDetailRepository manuReturnOrderDetailRepository,
            IWhSupplierRepository whSupplierRepository,
            IIQCOrderCreateService iQCOrderCreateService,
            IOptions<WMSOptions> options,
            IWMSApiClient xnebulaWMSApiClient,
            IQualIqcOrderReturnRepository qualIqcOrderReturnRepository,
            IQualIqcOrderReturnDetailRepository qualIqcOrderReturnDetailRepository,
            IInteCodeRulesRepository inteCodeRulesRepository,
            IManuGenerateBarcodeService manuGenerateBarcodeService,
            IWhWarehouseRepository whWarehouseRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IPlanWorkPlanRepository planWorkPlanRepository,
            IPlanWorkPlanMaterialRepository planWorkPlanMaterialRepository)
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
            _planWorkPlanRepository = planWorkPlanRepository;
            _planWorkPlanMaterialRepository = planWorkPlanMaterialRepository;
        }


        /// <summary>
        /// 物料退料
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task MaterialReturnAsync(MaterialReturnDto requestDto)
        {
            // 初始化
            var baseBo = new BaseBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                User = _currentUser.UserName,
                Time = HymsonClock.Now()
            };

            var whWarehouseEntity = await _whWarehouseRepository.GetOneAsync(new Data.Repositories.WhWareHouse.Query.WhWarehouseQuery
            {
                SiteId = baseBo.SiteId,
                Code = _options.Value.Receipt.WarehouseCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES15142)).WithData("Code", _options.Value.Receipt.WarehouseCode);

            var materialInventoryEntities = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
            {
                SiteId = baseBo.SiteId,
                BarCodes = requestDto.MaterialBarCodes
            });

            if (materialInventoryEntities == null || !materialInventoryEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15124));
            }

            // 查询到物料信息
            var materialEntities = await _procMaterialRepository.GetByIdsAsync(materialInventoryEntities.Select(x => x.MaterialId));
            List<WhMaterialStandingbookEntity> whMaterialStandingbookEntities = new();
            List<UpdateAndCheckStatusByIdCommand> updateAndCheckStatusByIdCommands = new();

            // 退料单明细
            List<ManuReturnOrderDetailEntity> manuReturnOrderDetailEntities = new();

            // 检验单
            QualIqcOrderReturnEntity qualIqcOrderReturnEntity = new();
            // 检验单明细
            List<QualIqcOrderReturnDetailEntity> qualIqcOrderReturnDetailEntities = new();

            var warehousingEntryDetails = new List<ReceiptDetailDto>();
            var returnOrderCode = await GenerateMaintenanceOrderCodeAsync(baseBo.SiteId, baseBo.User);

            var warehousingEntryDto = new WarehousingEntryDto()
            {
                Type = BillBusinessTypeEnum.WorkOrderMaterialReturnForm,
                IsAutoExecute = requestDto.Type == ManuReturnTypeEnum.WorkOrderBorrow,
                SyncCode = returnOrderCode,
                CreatedBy = baseBo.User,
                WarehouseCode = _options.Value.Receipt.VirtuallyWarehouseCode,
                Remark = requestDto.Remark,
            };

            // 创建领料申请单
            var manuReturnOrderEntity = new ManuReturnOrderEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = baseBo.SiteId,
                ReturnOrderCode = returnOrderCode,
                WorkOrderId = requestDto.WorkOrderId,
                Status = WhWarehouseMaterialReturnStatusEnum.PendingStorage,
                ReceiveWarehouseId = whWarehouseEntity.Id,
                Type = requestDto.Type,
                Remark = requestDto.Remark,
                CreatedBy = baseBo.User,
                UpdatedBy = baseBo.User,
                CreatedOn = baseBo.Time,
                UpdatedOn = baseBo.Time
            };

            // 实物退料
            if (requestDto.Type == ManuReturnTypeEnum.WorkOrderReturn)
            {
                manuReturnOrderEntity.Status = WhWarehouseMaterialReturnStatusEnum.ApplicationSuccessful;

                // 生成检验单号
                var inspectionOrder = await _iqcOrderCreateService.GenerateCommonIQCOrderCodeAsync(new BaseBo
                {
                    SiteId = baseBo.SiteId,
                    User = baseBo.User,
                });

                qualIqcOrderReturnEntity = new QualIqcOrderReturnEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = baseBo.SiteId,
                    InspectionOrder = inspectionOrder,
                    ReturnOrderId = manuReturnOrderEntity.Id,
                    WorkOrderId = materialInventoryEntities.FirstOrDefault()?.WorkOrderId,
                    Status = IQCLiteStatusEnum.WaitInspect,
                    IsQualified = null,
                    CreatedBy = baseBo.User,
                    UpdatedBy = baseBo.User,
                    CreatedOn = baseBo.Time,
                    UpdatedOn = baseBo.Time
                };
            }

            // 查询工单信息
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(requestDto.WorkOrderId);

            // 查询生产计划
            var planWorkPlanEntity = await _planWorkPlanRepository.GetByIdAsync(planWorkOrderEntity.WorkPlanId ?? 0)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16052)).WithData("WorkOrder", planWorkOrderEntity.OrderCode);

            // 查询生产物料
            var planWorkPlanMaterialEntities = await _planWorkPlanMaterialRepository.GetEntitiesByPlanIdAsync(new Data.Repositories.Plan.Query.PlanWorkPlanByPlanIdQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                PlanId = planWorkPlanEntity.Id,
                PlanProductId = planWorkOrderEntity.WorkPlanProductId ?? 0
            });

            // 遍历提交退料的物料条码
            foreach (var materialBarCode in requestDto.MaterialBarCodes)
            {
                var whMaterialInventoryEntity = materialInventoryEntities.FirstOrDefault(x => x.MaterialBarCode == materialBarCode)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES15138)).WithData("MaterialCode", materialBarCode);

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
                    UpdatedBy = baseBo.User,
                    UpdatedOn = baseBo.Time,
                    Status = WhMaterialInventoryStatusEnum.InUse,
                    CurrentStatus = whMaterialInventoryEntity.Status,
                    Id = whMaterialInventoryEntity.Id
                });

                var materialEntity = materialEntities.FirstOrDefault(x => x.Id == whMaterialInventoryEntity.MaterialId);
                whMaterialStandingbookEntities.Add(new WhMaterialStandingbookEntity
                {
                    MaterialCode = materialEntity?.MaterialCode ?? "",
                    MaterialName = materialEntity?.MaterialName ?? "",
                    MaterialVersion = materialEntity?.Version ?? "",
                    Unit = materialEntity?.Unit ?? "",
                    MaterialBarCode = whMaterialInventoryEntity.MaterialBarCode,
                    Type = WhMaterialInventoryTypeEnum.ReturnApplication,
                    Source = whMaterialInventoryEntity.Source,
                    SiteId = baseBo.SiteId,
                    Batch = whMaterialInventoryEntity.Batch,
                    Quantity = whMaterialInventoryEntity.QuantityResidue,
                    SupplierId = whMaterialInventoryEntity.SupplierId,
                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = baseBo.User,
                    UpdatedBy = baseBo.User,
                    CreatedOn = baseBo.Time,
                    UpdatedOn = baseBo.Time
                });

                // 退料单明细
                var manuReturnOrderDetailId = IdGenProvider.Instance.CreateId();
                manuReturnOrderDetailEntities.Add(new ManuReturnOrderDetailEntity
                {
                    ReturnOrderId = manuReturnOrderEntity.Id,
                    MaterialId = materialEntity?.Id ?? 0,
                    WarehouseId = whWarehouseEntity.Id,
                    MaterialBarCode = materialBarCode,
                    Batch = whMaterialInventoryEntity.Batch,
                    Qty = whMaterialInventoryEntity.QuantityResidue,
                    SupplierId = whMaterialInventoryEntity.SupplierId,
                    ExpirationDate = whMaterialInventoryEntity.DueDate,
                    SiteId = whMaterialInventoryEntity.SiteId,
                    Id = manuReturnOrderDetailId,
                    CreatedBy = baseBo.User,
                    UpdatedBy = baseBo.User,
                    CreatedOn = baseBo.Time,
                    UpdatedOn = baseBo.Time
                });

                if (requestDto.Type == ManuReturnTypeEnum.WorkOrderReturn)
                {
                    qualIqcOrderReturnDetailEntities.Add(new QualIqcOrderReturnDetailEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = baseBo.SiteId,
                        IQCOrderId = qualIqcOrderReturnEntity.Id,
                        ReturnOrderDetailId = manuReturnOrderDetailId,
                        BarCode = materialBarCode,
                        MaterialId = whMaterialInventoryEntity.MaterialId,
                        IsQualified = null,
                        CreatedBy = baseBo.User,
                        UpdatedBy = baseBo.User,
                        CreatedOn = baseBo.Time,
                        UpdatedOn = baseBo.Time
                    });
                }

                // 2024.07.27 TODO: 临时处理
                var planWorkPlanMaterialEntity = planWorkPlanMaterialEntities.FirstOrDefault(x => x.MaterialId == materialEntity?.Id);
                if (planWorkPlanMaterialEntity != null)
                {
                    warehousingEntryDetails.Add(new ReceiptDetailDto
                    {
                        ProductionOrder = planWorkPlanEntity.WorkPlanCode,
                        ProductionOrderDetailID = planWorkOrderEntity?.WorkPlanProductId,
                        ProductionOrderComponentID = planWorkPlanMaterialEntity.Id,

                        ProductionOrderNumber = planWorkPlanEntity.WorkPlanCode,
                        WorkOrderCode = planWorkOrderEntity?.OrderCode,

                        SyncId = manuReturnOrderDetailId,
                        MaterialBarCode = materialBarCode,
                        MaterialCode = materialEntity?.MaterialCode,
                        LotCode = whMaterialInventoryEntity.Batch,
                        UnitCode = materialEntity?.Unit,
                        UniqueCode = materialBarCode,
                        Quantity = whMaterialInventoryEntity.QuantityResidue,
                        Batch = whMaterialInventoryEntity.Batch ?? ""
                    });
                }
            }
            warehousingEntryDto.Details = warehousingEntryDetails;

            using var trans = TransactionHelper.GetTransactionScope();
            var row = await _whMaterialInventoryRepository.UpdateAndCheckStatusByIdAsync(updateAndCheckStatusByIdCommands);
            if (row != materialInventoryEntities.Count())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15137));
            }

            await _whMaterialStandingbookRepository.InsertsAsync(whMaterialStandingbookEntities);
            await _manuReturnOrderRepository.InsertAsync(manuReturnOrderEntity);
            await _manuReturnOrderDetailRepository.InsertRangeAsync(manuReturnOrderDetailEntities);

            // 退实物
            if (requestDto.Type == ManuReturnTypeEnum.WorkOrderReturn)
            {
                await _qualIqcOrderReturnRepository.InsertAsync(qualIqcOrderReturnEntity);
                await _qualIqcOrderReturnDetailRepository.InsertRangeAsync(qualIqcOrderReturnDetailEntities);
            }

            trans.Complete();
            trans.Dispose();

            // 退虚拟库
            if (requestDto.Type == ManuReturnTypeEnum.WorkOrderBorrow)
            {
                var response = await _wmsRequest.WarehousingEntryRequestAsync(warehousingEntryDto);
                if (response == null) throw new CustomerValidationException(nameof(ErrorCode.MES15500)).WithData("Message", "结果返回异常，请检查！");
                if (response.Code != 0) throw new CustomerValidationException(nameof(ErrorCode.MES15500)).WithData("Message", response.Message);
            }
        }

        /// <summary>
        /// 退料（实物）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task ReturnPhysicalAsync(MaterialReturnDto requestDto)
        {
            // TODO: Implement this method
            await Task.CompletedTask;
        }

        /// <summary>
        /// 退料（虚拟）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task ReturnVirtualAsync(MaterialReturnDto requestDto)
        {
            // TODO: Implement this method
            await Task.CompletedTask;
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
