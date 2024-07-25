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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manuRequistionOrderDetailRepository"></param>
        /// <param name="manuRequistionOrderRepository"></param>
        public WhMaterialPickingService(ICurrentUser currentUser, ICurrentSite currentSite, IManuRequistionOrderDetailRepository manuRequistionOrderDetailRepository, IManuRequistionOrderRepository manuRequistionOrderRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuRequistionOrderDetailRepository = manuRequistionOrderDetailRepository;
            _manuRequistionOrderRepository = manuRequistionOrderRepository;
        }

        /// <summary>
        /// 领料单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<string> PickMaterialsRequestAsync(PickMaterialDto param)
        {
            //派工单校验
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(param.OrderId);
            if (planWorkOrderEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15151));
            }

            if (planWorkOrderEntity.Status == PlanWorkOrderStatusEnum.Finish)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16048)).WithData("WorkOrder", planWorkOrderEntity.OrderCode);
            }

            var requistionOrderCode = await GenerateOrderCodeAsync(_currentSite.SiteId??0, _currentUser.UserName);

            //创建领料申请单
            ManuRequistionOrderEntity manuRequistionOrderEntity = new ManuRequistionOrderEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                ReqOrderCode= requistionOrderCode,
                Status = WhMaterialPickingStatusEnum.ApplicationSuccessful,
                Type = ManuRequistionTypeEnum.WorkOrderPicking,
                WorkOrderId = planWorkOrderEntity.Id,
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedOn = HymsonClock.Now()
            };

           //foreach(var item )

            return requistionOrderCode;
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
