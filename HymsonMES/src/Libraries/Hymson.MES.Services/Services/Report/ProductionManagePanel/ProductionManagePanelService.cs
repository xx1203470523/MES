using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report.ProductionManagePanel
{
    /// <summary>
    /// 生产管理看板服务
    /// </summary>
    public class ProductionManagePanelService : IProductionManagePanelService
    {

        public readonly PlanWorkOrderActivationRepository _planWorkOrderActivationRepository;
        public readonly PlanWorkOrderRepository _planWorkOrderRepository;
        public ProductionManagePanelService(PlanWorkOrderActivationRepository planWorkOrderActivationRepository, PlanWorkOrderRepository planWorkOrderRepository)
        {
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
        }

        /// <summary>
        /// 获取SiteId下的激活的第一个工单
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private async Task<PlanWorkOrderEntity?> GetPlanWorkOrderInfo(long siteId)
        {
            //查询激活工单
            var workOrderActivationQuery = new PlanWorkOrderActivationQuery { SiteId = siteId };
            var planWorkOrderActivations = await _planWorkOrderActivationRepository.GetPlanWorkOrderActivationEntitiesAsync(workOrderActivationQuery);
            var planWorkOrderActivationEntity = planWorkOrderActivations.OrderByDescending(c => c.Id)?.FirstOrDefault();
            if (planWorkOrderActivationEntity == null)
            {
                return null;
            }
            return await _planWorkOrderRepository.GetByIdAsync(planWorkOrderActivationEntity.WorkOrderId);
        }

        public async Task<ProductionManagePanelReportDto?> GetOverallInfoAsync(long siteId)
        {
            var planWorkOrderEntity = await GetPlanWorkOrderInfo(siteId);
            if (planWorkOrderEntity == null) { return null; }
            var planWorkOrderRecordEntity = await _planWorkOrderRepository.GetByWorkOrderIdAsync(planWorkOrderEntity.Id);
            if (planWorkOrderRecordEntity == null) { return null; }
            var managePanelReportDto = new ProductionManagePanelReportDto()
            {
                CompletedQty = planWorkOrderRecordEntity?.FinishProductQuantity.Value ?? 0,
                CompletedRate = 80,
                DayConsume = 120,
                DayShift = 0,
                InputQty = 120,
                OverallPlanAchievingRate = 10,
                OverallYieldRate = 90,
                ProcessRouteCode = "1111",
                ProcessRouteName = "工艺路线",
                ProductCode = "AAA",
                ProductName = "产品",
                UnqualifiedQty = 10,
                UnqualifiedRate = 10,
                WorkLineName = "ajkfaw",
                WorkOrderCode = "W292421AAFA",
                WorkOrderDownTime = new DateTime(),
                WorkOrderQty = 200
            };
            return managePanelReportDto;
        }

    }
}
