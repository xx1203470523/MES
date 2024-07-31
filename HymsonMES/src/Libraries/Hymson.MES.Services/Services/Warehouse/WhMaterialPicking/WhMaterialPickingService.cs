using Hymson.Authentication.JwtBearer.Security;
using Hymson.Authentication;
using Hymson.MES.Data.Repositories.Manufacture.ManuRequistionOrder;
using Hymson.MES.Services.Dtos.Manufacture.WhMaterialPicking;
using Hymson.MES.Services.Dtos.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.WhWareHouse;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.Snowflake;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.Utils;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.HttpClients.Requests.XnebulaWMS;
using Hymson.Utils.Tools;
using Hymson.MES.HttpClients.Options;
using Microsoft.Extensions.Options;
using Hymson.MES.HttpClients;
using Hymson.MES.Data.Repositories.Process;
using Minio.DataModel;
using Azure;

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

        public WhMaterialPickingService(ICurrentUser currentUser,
            ICurrentSite currentSite,
            IManuRequistionOrderRepository manuRequistionOrderRepository,
            IManuRequistionOrderDetailRepository manuRequistionOrderDetailRepository,
            IInteCodeRulesRepository inteCodeRulesRepository,
            IManuGenerateBarcodeService manuGenerateBarcodeService,
            IWhWarehouseRepository whWarehouseRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IOptions<WMSOptions> options,
            IWMSApiClient wmsRequest,
            IProcMaterialRepository procMaterialRepository,
            IPlanWorkPlanRepository planWorkPlanRepository,
            IPlanWorkPlanMaterialRepository planWorkPlanMaterialRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuRequistionOrderRepository = manuRequistionOrderRepository;
            _manuRequistionOrderDetailRepository = manuRequistionOrderDetailRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _whWarehouseRepository = whWarehouseRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _options = options;
            _wmsRequest = wmsRequest;
            _procMaterialRepository = procMaterialRepository;
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

            // 查询生产计划
            var planWorkPlanEntity = await _planWorkPlanRepository.GetByIdAsync(planWorkOrderEntity.WorkPlanId ?? 0)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16052)).WithData("WorkOrder", planWorkOrderEntity.OrderCode);

            var planWorkPlanMaterialEntities = await _planWorkPlanMaterialRepository.GetEntitiesByPlanIdAsync(new Data.Repositories.Plan.Query.PlanWorkPlanByPlanIdQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                PlanId = planWorkPlanEntity.Id,
                PlanProductId = planWorkOrderEntity.WorkPlanProductId ?? 0
            });

            var requistionOrderCode = await GenerateOrderCodeAsync(_currentSite.SiteId ?? 0, _currentUser.UserName);

            //创建领料申请单
            ManuRequistionOrderEntity manuRequistionOrderEntity = new ManuRequistionOrderEntity
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
            var manuRequistionOrderDetailEntities = new List<ManuRequistionOrderDetailEntity>();

            var deliveryDto = new DeliveryDto
            {
                Type = BillBusinessTypeEnum.WorkOrderMaterialRequestForm,
                IsAutoExecute = param.Type == ManuRequistionTypeEnum.WorkOrderReplenishment,
                CreatedBy = _currentUser.UserName,
                WarehouseCode = param.Type == ManuRequistionTypeEnum.WorkOrderReplenishment ? _options.Value.Delivery.VirtuallyWarehouseCode : _options.Value.Delivery.RawWarehouseCode,
                SyncCode = requistionOrderCode,
            };

            var warehousingDeliveryDetails = new List<DeliveryDetailDto>();
            var procMaterialEntities = await _procMaterialRepository.GetByIdsAsync(param.Details.Select(x => x.MaterialId));
            string batchErrMsg = string.Empty;
            //咱不验证数量 TODO by王克明
            foreach (var item in param.Details)
            {
                manuRequistionOrderDetailEntities.Add(new ManuRequistionOrderDetailEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    RequistionOrderId = manuRequistionOrderEntity.Id,
                    MaterialId = item.MaterialId,
                    Qty = item.Qty,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now()
                });
                var procMaterialEntity = procMaterialEntities.FirstOrDefault(x => x.Id == item.MaterialId);

                // 2024.07.27 TODO: 临时处理
                var planWorkPlanMaterialEntity = planWorkPlanMaterialEntities.FirstOrDefault(x => x.MaterialId == item.MaterialId);
                if (planWorkPlanMaterialEntity != null)
                {
                    warehousingDeliveryDetails.Add(new DeliveryDetailDto
                    {
                        ProductionOrder = planWorkPlanEntity.WorkPlanCode,
                        WorkOrderCode = planWorkOrderEntity.OrderCode,

                        ProductionOrderDetailID = planWorkOrderEntity.WorkPlanProductId,
                        ProductionOrderComponentID = planWorkPlanMaterialEntity.Id, //planWorkPlanMaterialEntities.FirstOrDefault(x => x.MaterialId == item.MaterialId)?.Id,
                        MaterialCode = procMaterialEntity?.MaterialCode,
                        UnitCode = procMaterialEntity?.Unit,
                        Quantity = item.Qty
                    });
                }

                // 校验是否是最小包装量
                int batchPlacesNum = CountDecimalPlaces(item.Batch); //批次数量小数
                int qtyPlacesNum = CountDecimalPlaces(item.Qty); //用量数量小数
                int maxPlacesNum = Math.Max(batchPlacesNum, qtyPlacesNum); //最大数量小数
                int qtyNum = (int)(item.Qty * (int)Math.Pow(10, maxPlacesNum)); //转成整数
                int batchNum = (int)(item.Batch * (int)Math.Pow(10, maxPlacesNum)); //转成整数
                if (batchNum != 0 && qtyNum % batchNum != 0)
                {
                    batchErrMsg += $"{procMaterialEntity?.MaterialCode}物料不能按最小包装数量{item.Batch}去领{item.Qty}用量;";
                }
            }
            if (string.IsNullOrEmpty(batchErrMsg) == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15167)).WithData("Msg", batchErrMsg);
            }

            deliveryDto.Details = warehousingDeliveryDetails;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                await _manuRequistionOrderRepository.InsertAsync(manuRequistionOrderEntity);
                await _manuRequistionOrderDetailRepository.InsertsAsync(manuRequistionOrderDetailEntities);

                var response = await _wmsRequest.WarehousingDeliveryRequestAsync(deliveryDto);
                if (response.Code != 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15152)).WithData("System", "WMS").WithData("Msg", response.Message);
                }
                trans.Complete();
            }
            return requistionOrderCode;

            //计算decimal小数位数
            int CountDecimalPlaces(decimal number)
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
}
