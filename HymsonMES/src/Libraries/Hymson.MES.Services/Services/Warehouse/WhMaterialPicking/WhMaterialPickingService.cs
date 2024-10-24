using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuRequistionOrder;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.WhWareHouse;
using Hymson.MES.HttpClients;
using Hymson.MES.HttpClients.Options;
using Hymson.MES.HttpClients.Requests.XnebulaWMS;
using Hymson.MES.Services.Dtos.Manufacture.WhMaterialPicking;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Services.Services.Warehouse.WhMaterialPicking
{
    /// <summary>
    /// 领料单
    /// </summary>
    public class WhMaterialPickingService : IWhMaterialPickingService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        private readonly IManuRequistionOrderRepository _manuRequistionOrderRepository;

        private readonly IManuRequistionOrderDetailRepository _manuRequistionOrderDetailRepository;

        /// <summary>
        /// 退料单
        /// </summary>
        private readonly IManuReturnOrderRepository _manuReturnOrderRepository;

        /// <summary>
        /// 退料单详情
        /// </summary>
        private readonly IManuReturnOrderDetailRepository _manuReturnOrderDetailRepository;

        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;

        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;

        private readonly IWhWarehouseRepository _whWarehouseRepository;

        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        private readonly IOptions<WMSOptions> _options;

        private readonly IWMSApiClient _wmsRequest;

        private readonly IPlanWorkPlanRepository _planWorkPlanRepository;

        private readonly IPlanWorkPlanMaterialRepository _planWorkPlanMaterialRepository;

        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuRequistionOrderRepository"></param>
        /// <param name="manuRequistionOrderDetailRepository"></param>
        /// <param name="manuReturnOrderRepository"></param>
        /// <param name="manuReturnOrderDetailRepository"></param>
        /// <param name="inteCodeRulesRepository"></param>
        /// <param name="manuGenerateBarcodeService"></param>
        /// <param name="whWarehouseRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="options"></param>
        /// <param name="wmsRequest"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="planWorkPlanRepository"></param>
        /// <param name="planWorkPlanMaterialRepository"></param>
        public WhMaterialPickingService(ICurrentUser currentUser,
            ICurrentSite currentSite,
            IManuRequistionOrderRepository manuRequistionOrderRepository,
            IManuRequistionOrderDetailRepository manuRequistionOrderDetailRepository,
            IManuReturnOrderRepository manuReturnOrderRepository,
            IManuReturnOrderDetailRepository manuReturnOrderDetailRepository,
            IInteCodeRulesRepository inteCodeRulesRepository,
            IManuGenerateBarcodeService manuGenerateBarcodeService,
            IWhWarehouseRepository whWarehouseRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IOptions<WMSOptions> options,
            IWMSApiClient wmsRequest,
            IProcMaterialRepository procMaterialRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IPlanWorkPlanRepository planWorkPlanRepository,
            IPlanWorkPlanMaterialRepository planWorkPlanMaterialRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuRequistionOrderRepository = manuRequistionOrderRepository;
            _manuRequistionOrderDetailRepository = manuRequistionOrderDetailRepository;
            _manuReturnOrderRepository = manuReturnOrderRepository;
            _manuReturnOrderDetailRepository = manuReturnOrderDetailRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _whWarehouseRepository = whWarehouseRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _options = options;
            _wmsRequest = wmsRequest;
            _procMaterialRepository = procMaterialRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _planWorkPlanRepository = planWorkPlanRepository;
            _planWorkPlanMaterialRepository = planWorkPlanMaterialRepository;
        }


        /// <summary>
        /// 领料单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<string> PickMaterialsRequestAsync(PickMaterialDto param)
        {
            // 派工单校验
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(param.OrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES15151));

            if (planWorkOrderEntity.Status == PlanWorkOrderStatusEnum.Finish)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16048)).WithData("WorkOrder", planWorkOrderEntity.OrderCode);
            }

            // 实仓
            if (param.Type == ManuRequistionTypeEnum.WorkOrderPicking && string.IsNullOrEmpty(param.WarehouseCode))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16057));
            }

            // 虚仓
            if (param.Type == ManuRequistionTypeEnum.WorkOrderReplenishment && string.IsNullOrEmpty(param.WorkOrderCode))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16068));
            }

            // 查询生产计划
            var planWorkPlanEntity = await _planWorkPlanRepository.GetByIdAsync(planWorkOrderEntity.WorkPlanId ?? 0)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16052)).WithData("WorkOrder", planWorkOrderEntity.OrderCode);

            var planWorkPlanMaterialEntities = await _planWorkPlanMaterialRepository.GetEntitiesByPlanIdAsync(new Data.Repositories.Plan.Query.PlanWorkPlanByPlanIdQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                PlanId = planWorkPlanEntity.Id,
                PlanProductId = planWorkOrderEntity.WorkPlanProductId ?? 0
            });

            // 生成领料单号
            var requistionOrderCode = await GenerateOrderCodeAsync(_currentSite.SiteId ?? 0, _currentUser.UserName);

            // 创建领料申请单
            var manuRequistionOrderEntity = new ManuRequistionOrderEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                ReqOrderCode = requistionOrderCode,
                Status = WhMaterialPickingStatusEnum.ApplicationSuccessful,
                Type = param.Type,
                WorkOrderId = planWorkOrderEntity.Id,
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedOn = HymsonClock.Now()
            };

            // 初始化数据
            var responseBo = new PickMaterialResponseBo();

            // 实仓领料
            if (param.Type == ManuRequistionTypeEnum.WorkOrderPicking)
            {
                responseBo = await GetPhysicalDeliveryDetailDtosAsync(new PickMaterialRequestBo
                {
                    PlanWorkPlanEntity = planWorkPlanEntity,
                    PlanWorkPlanMaterialEntities = planWorkPlanMaterialEntities,
                    PlanWorkOrderEntity = planWorkOrderEntity,
                    RequistionOrderEntity = manuRequistionOrderEntity,
                    Details = param.Details
                });
            }

            // 虚仓领料
            if (param.Type == ManuRequistionTypeEnum.WorkOrderReplenishment)
            {
                var returnWorkOrderCode = param.WorkOrderCode ?? "";
                var returnPlanWorkOrderEntity = await _planWorkOrderRepository.GetByCodeAsync(new PlanWorkOrderQuery
                {
                    SiteId = planWorkPlanEntity.SiteId,
                    OrderCode = returnWorkOrderCode
                }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16016)).WithData("WorkOrder", returnWorkOrderCode);

                responseBo = await GetVirtualDeliveryDetailDtosAsync(new PickMaterialRequestBo
                {
                    PlanWorkPlanEntity = planWorkPlanEntity,
                    PlanWorkPlanMaterialEntities = planWorkPlanMaterialEntities,
                    PlanWorkOrderEntity = planWorkOrderEntity,
                    ReturnPlanWorkOrderEntity = returnPlanWorkOrderEntity,
                    RequistionOrderEntity = manuRequistionOrderEntity,
                    Details = param.Details
                });
            }

            var deliveryDto = new DeliveryDto
            {
                Type = BillBusinessTypeEnum.WorkOrderMaterialRequestForm,
                IsAutoExecute = param.Type == ManuRequistionTypeEnum.WorkOrderReplenishment,
                CreatedBy = _currentUser.UserName,
                // WarehouseCode = param.Type == ManuRequistionTypeEnum.WorkOrderReplenishment ? _options.Value.Delivery.VirtuallyWarehouseCode : _options.Value.Delivery.RawWarehouseCode
                WarehouseCode = param.Type == ManuRequistionTypeEnum.WorkOrderReplenishment ? _options.Value.Delivery.VirtuallyWarehouseCode : param.WarehouseCode,
                SyncCode = requistionOrderCode,
                Details = responseBo.WarehousingDeliveryDetails
            };

            using var trans = TransactionHelper.GetTransactionScope();
            await _manuRequistionOrderRepository.InsertAsync(manuRequistionOrderEntity);
            await _manuRequistionOrderDetailRepository.InsertsAsync(responseBo.RequistionOrderDetailEntities);

            var response = await _wmsRequest.WarehousingDeliveryRequestAsync(deliveryDto);
            if (response == null) throw new CustomerValidationException(nameof(ErrorCode.MES15500)).WithData("Message", "结果返回异常，请检查！");
            if (response.Code != 0) throw new CustomerValidationException(nameof(ErrorCode.MES15500)).WithData("Message", response.Message);

            trans.Complete();

            return requistionOrderCode;
        }

        /// <summary>
        /// 领料（实仓）
        /// </summary>
        /// <param name="requestBo"></param>
        /// <returns></returns>
        public async Task<PickMaterialResponseBo> GetPhysicalDeliveryDetailDtosAsync(PickMaterialRequestBo requestBo)
        {
            // 初始化数据
            var responseBo = new PickMaterialResponseBo();

            // 物料基础信息
            var procMaterialEntities = await _procMaterialRepository.GetByIdsAsync(requestBo.Details.Select(x => x.MaterialId));

            // 咱不验证数量 TODO by王克明
            string errorMessage = string.Empty;
            foreach (var item in requestBo.Details)
            {
                var procMaterialEntity = procMaterialEntities.FirstOrDefault(x => x.Id == item.MaterialId);

                // 2024.07.27 TODO: 临时处理
                var planWorkPlanMaterialEntity = requestBo.PlanWorkPlanMaterialEntities.FirstOrDefault(x => x.MaterialId == item.MaterialId && x.BomId == item.BomId);
                if (planWorkPlanMaterialEntity == null) continue;

                responseBo.WarehousingDeliveryDetails.Add(new DeliveryDetailDto
                {
                    ProductionOrder = requestBo.PlanWorkPlanEntity.WorkPlanCode,
                    WorkOrderCode = requestBo.PlanWorkOrderEntity.OrderCode,

                    ProductionOrderDetailID = requestBo.PlanWorkOrderEntity.WorkPlanProductId,
                    ProductionOrderComponentID = planWorkPlanMaterialEntity.Id, //planWorkPlanMaterialEntities.FirstOrDefault(x => x.MaterialId == item.MaterialId)?.Id,
                    MaterialCode = procMaterialEntity?.MaterialCode,
                    //LotCode = whMaterialInventoryEntity.Batch,
                    UnitCode = procMaterialEntity?.Unit,
                    Quantity = item.Qty
                });

                // 添加领料明细
                responseBo.RequistionOrderDetailEntities.Add(new ManuRequistionOrderDetailEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    RequistionOrderId = requestBo.RequistionOrderEntity.Id,
                    ProductionOrderComponentID = planWorkPlanMaterialEntity.Id,
                    MaterialId = item.MaterialId,
                    Qty = item.Qty,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now()
                });

                // 校验是否是最小包装量
                int batchPlacesNum = CountDecimalPlaces(item.Batch); //批次数量小数
                int qtyPlacesNum = CountDecimalPlaces(item.Qty); //用量数量小数
                int maxPlacesNum = Math.Max(batchPlacesNum, qtyPlacesNum); //最大数量小数
                int qtyNum = (int)(item.Qty * (int)Math.Pow(10, maxPlacesNum)); //转成整数
                int batchNum = (int)(item.Batch * (int)Math.Pow(10, maxPlacesNum)); //转成整数

                if (batchNum == 0 || qtyNum % batchNum == 0) continue;
                errorMessage += $"{procMaterialEntity?.MaterialCode}物料不能按最小包装数量{item.Batch}去领{item.Qty}用量;";
            }

            if (string.IsNullOrEmpty(errorMessage)) return responseBo;
            throw new CustomerValidationException(nameof(ErrorCode.MES15167)).WithData("Msg", errorMessage);
        }

        /// <summary>
        /// 领料（虚仓）
        /// </summary>
        /// <param name="requestBo"></param>
        /// <returns></returns>
        public async Task<PickMaterialResponseBo> GetVirtualDeliveryDetailDtosAsync(PickMaterialRequestBo requestBo)
        {
            // 初始化数据
            var responseBo = new PickMaterialResponseBo();

            // 读取虚退工单对应的虚仓退料记录
            var returnOrderEntities = await _manuReturnOrderRepository.GetEntitiesAsync(new Data.Repositories.Manufacture.Query.ManuReturnOrderQuery
            {
                SiteId = requestBo.PlanWorkOrderEntity.SiteId,
                WorkOrderId = requestBo.ReturnPlanWorkOrderEntity?.Id,
                Type = ManuReturnTypeEnum.WorkOrderBorrow
            });
            if (returnOrderEntities == null || !returnOrderEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15167)).WithData("Msg", $"工单【{requestBo.PlanWorkOrderEntity.OrderCode}】没有可以进行虚领的虚退记录！");
            }

            // 读取工单对应的虚仓退料明细
            var returnOrderDetailEntities = await _manuReturnOrderDetailRepository.GetEntitiesAsync(new Data.Repositories.Manufacture.Query.ManuReturnOrderDetailQuery
            {
                SiteId = requestBo.PlanWorkOrderEntity.SiteId,
                ReturnOrderIds = returnOrderEntities.Select(x => x.Id)
            });

            // TODO 要过滤掉已经虚领的退料单

            // 查询生产计划物料
            var planWorkPlanMaterialEntities = await _planWorkPlanMaterialRepository.GetEntitiesByPlanIdAsync(new Data.Repositories.Plan.Query.PlanWorkPlanByPlanIdQuery
            {
                SiteId = requestBo.PlanWorkOrderEntity.SiteId,
                PlanId = requestBo.PlanWorkPlanEntity.Id,
                PlanProductId = requestBo.PlanWorkOrderEntity.WorkPlanProductId ?? 0
            });

            // 查询bom详情
            var procBomDetailEntities = await _procBomDetailRepository.GetByIdsAsync(planWorkPlanMaterialEntities.Select(x => x.BomId.GetValueOrDefault()));
            var bomMaterialIds = procBomDetailEntities.Where(w => w.BomProductType != ManuProductTypeEnum.ByProduct).Select(s => s.MaterialId);

            // 找出planWorkPlanMaterialEntities中MaterialId，存在bomMaterialIds中的数据
            planWorkPlanMaterialEntities = planWorkPlanMaterialEntities.Where(w => bomMaterialIds.Contains(w.MaterialId));

            // 物料基础信息
            var procMaterialEntities = await _procMaterialRepository.GetByIdsAsync(returnOrderDetailEntities.Select(x => x.MaterialId));
            foreach (var returnOrderDetailEntity in returnOrderDetailEntities)
            {
                var procMaterialEntity = procMaterialEntities.FirstOrDefault(x => x.Id == returnOrderDetailEntity.MaterialId);

                // 读取生产计划物料
                var planWorkPlanMaterialEntity = planWorkPlanMaterialEntities.FirstOrDefault(x => x.MaterialId == returnOrderDetailEntity.MaterialId);
                if (planWorkPlanMaterialEntity == null) continue;

                responseBo.WarehousingDeliveryDetails.Add(new DeliveryDetailDto
                {
                    ProductionOrder = requestBo.PlanWorkPlanEntity.WorkPlanCode,
                    WorkOrderCode = requestBo.PlanWorkOrderEntity.OrderCode,

                    ProductionOrderDetailID = requestBo.PlanWorkOrderEntity.WorkPlanProductId,
                    ProductionOrderComponentID = planWorkPlanMaterialEntity.Id, //planWorkPlanMaterialEntities.FirstOrDefault(x => x.MaterialId == item.MaterialId)?.Id,
                    MaterialCode = procMaterialEntity?.MaterialCode,
                    LotNo = returnOrderDetailEntity.Batch,
                    UnitCode = procMaterialEntity?.Unit,
                    Quantity = returnOrderDetailEntity.Qty
                });

                // 添加领料明细
                responseBo.RequistionOrderDetailEntities.Add(new ManuRequistionOrderDetailEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    RequistionOrderId = requestBo.RequistionOrderEntity.Id,
                    ProductionOrderComponentID = planWorkPlanMaterialEntity.Id,
                    MaterialId = returnOrderDetailEntity.MaterialId,
                    Qty = returnOrderDetailEntity.Qty,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now()
                });
            }

            return responseBo;
        }

        /// <summary>
        /// 计算decimal小数位数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private int CountDecimalPlaces(decimal number)
        {
            // 将数字转换为字符串  
            string numberAsString = number.ToString();

            // 找到小数点位置  
            int decimalPointIndex = numberAsString.IndexOf('.');

            // 如果没有小数点，返回0  
            if (decimalPointIndex < 0) return 0;

            // 计算小数位数  
            return numberAsString.Length - decimalPointIndex - 1;
        }

        /// <summary>
        /// 退料单号生成
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        private async Task<string> GenerateOrderCodeAsync(long siteId, string userName)
        {
            var codeRules = await _inteCodeRulesRepository.GetListAsync(new InteCodeRulesReQuery
            {
                SiteId = siteId,
                CodeType = Core.Enums.Integrated.CodeRuleCodeTypeEnum.MaterialPickingOrder
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

    /// <summary>
    /// 
    /// </summary>
    public class PickMaterialRequestBo
    {
        /// <summary>
        /// 
        /// </summary>
        public PlanWorkPlanEntity PlanWorkPlanEntity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<PlanWorkPlanMaterialEntity> PlanWorkPlanMaterialEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PlanWorkOrderEntity PlanWorkOrderEntity { get; set; }

        /// <summary>
        /// 虚退的工单
        /// </summary>
        public PlanWorkOrderEntity? ReturnPlanWorkOrderEntity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ManuRequistionOrderEntity RequistionOrderEntity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<PickBomDetailDto> Details { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class PickMaterialResponseBo
    {
        /// <summary>
        /// 
        /// </summary>
        public List<DeliveryDetailDto> WarehousingDeliveryDetails { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public List<ManuRequistionOrderDetailEntity> RequistionOrderDetailEntities { get; set; } = new();

    }

}
