using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services.Report.ProductionManagePanel
{
    /// <summary>
    /// 生产管理看板服务
    /// </summary>
    public class ProductionManagePanelService : IProductionManagePanelService
    {

        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;

        public ProductionManagePanelService(IPlanWorkOrderActivationRepository planWorkOrderActivationRepository,
            IPlanWorkOrderRepository planWorkOrderRepository, IProcProcessRouteRepository procProcessRouteRepository,
            IProcMaterialRepository procMaterialRepository, IInteWorkCenterRepository inteWorkCenterRepository,
            IManuSfcSummaryRepository manuSfcSummaryRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository)
        {
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _procMaterialRepository = procMaterialRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _manuSfcSummaryRepository = manuSfcSummaryRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
        }

        /// <summary>
        /// 获取SiteId下的激活的第一个工单
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private async Task<PlanWorkOrderEntity?> GetPlanWorkOrderFirstActivation(long siteId)
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

        /// <summary>
        /// 当天开始时间
        /// 白班开始时间
        /// </summary>
        private DateTime DayStartTime
        {
            get
            {
                DateTime checkTime = DateTime.Now; // 当前时间
                DateTime startTime = new DateTime(checkTime.Year, checkTime.Month, checkTime.Day, 08, 30, 0); // 开始时间
                return startTime;
            }
        }
        /// <summary>
        /// 当天结束时间
        /// 第二天08:30
        /// </summary>
        private DateTime DayEndTime
        {
            get
            {
                return DayStartTime.AddDays(1);
            }
        }
        /// <summary>
        /// 今天白班结束时间
        /// </summary>
        private DateTime DayShiftEndTime
        {
            get
            {
                DateTime checkTime = DateTime.Now; // 当前时间
                DateTime endTime = new DateTime(checkTime.Year, checkTime.Month, checkTime.Day, 18, 30, 0); // 结束时间
                return endTime;
            }
        }
        /// <summary>
        /// 当前时间是否是在白班范围
        /// </summary>
        /// <returns></returns>
        private bool IsDayShift()
        {
            DateTime checkTime = DateTime.Now; // 当前时间
            DateTime startTime = DayStartTime; // 开始时间
            DateTime endTime = DayShiftEndTime; // 结束时间
            // 检查时间是否在区间内
            if (checkTime >= startTime && checkTime <= endTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取综合信息
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<ProductionManagePanelReportDto?> GetOverallInfoAsync(long siteId)
        {
            //获取最新的一个激活工单
            var planWorkOrderEntity = await GetPlanWorkOrderFirstActivation(siteId);
            if (planWorkOrderEntity == null) { return null; }
            //查询工作中心
            var inteWorkCenterEntity = await _inteWorkCenterRepository.GetByIdAsync(planWorkOrderEntity.WorkCenterId ?? -1);
            //查询工艺路线
            var processRouteEntity = await _procProcessRouteRepository.GetByIdAsync(planWorkOrderEntity.ProcessRouteId);
            //查询产品信息
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(planWorkOrderEntity.ProductId);
            //查询工单记录
            var workOrderRecord = await _planWorkOrderRepository.GetByWorkOrderIdAsync(planWorkOrderEntity.Id);
            if (workOrderRecord == null) { return null; }
            //查询工单维度生产汇总信息
            var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(new ManuSfcSummaryQuery
            {
                WorkOrderId = planWorkOrderEntity.Id,
                SiteId = siteId,
                QualityStatus = 0//不合格数
            });
            //获取首工序
            var processRouteDetailNodeEntity = await _procProcessRouteDetailNodeRepository.GetFirstProcedureByProcessRouteIdAsync(planWorkOrderEntity.ProcessRouteId);
            var firstProcedureSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(new ManuSfcSummaryQuery
            {
                SiteId = siteId,
                ProcedureId = processRouteDetailNodeEntity.ProcedureId,
                StartTime = DayStartTime,
                EndTime = DayEndTime,
            });
            //当天投入量
            decimal dayConsume = firstProcedureSummaryEntities.Count();

            //投入数
            decimal inputQty = workOrderRecord?.InputQty ?? 0;
            //完成数量
            decimal completedQty = workOrderRecord?.FinishProductQuantity ?? 0;
            //完成率
            decimal completedRate = decimal.Parse((completedQty / (inputQty == 0 ? 1 : inputQty) * 100).ToString("#0.00"));
            //计划数量
            decimal planQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100);
            //计划达成率
            decimal planAchievingRate = completedQty / (planQuantity == 0 ? 1 : planQuantity);
            //Ng数量
            decimal summaryNoPassQty = manuSfcSummaryEntities.Count();
            //录入报废数量
            decimal unqualifiedQuantity = workOrderRecord?.UnqualifiedQuantity ?? 0;
            //不良数量
            decimal unqualifiedQty = summaryNoPassQty + unqualifiedQuantity;
            //不良率
            decimal unqualifiedRate = decimal.Parse((unqualifiedQty / (completedQty == 0 ? 1 : completedQty) * 100).ToString("#0.00"));

            var managePanelReportDto = new ProductionManagePanelReportDto()
            {
                CompletedQty = completedQty,
                CompletedRate = completedRate,
                DayConsume = dayConsume,
                DayShift = IsDayShift() ? 1 : 0,
                InputQty = workOrderRecord?.InputQty,
                OverallPlanAchievingRate = planAchievingRate,
                OverallYieldRate = 100 - unqualifiedRate,
                ProcessRouteCode = processRouteEntity?.Code,
                ProcessRouteName = processRouteEntity?.Name,
                ProductCode = procMaterialEntity?.MaterialCode,
                ProductName = procMaterialEntity?.MaterialName,
                UnqualifiedQty = unqualifiedQty,
                UnqualifiedRate = unqualifiedRate,
                WorkLineName = inteWorkCenterEntity?.Name,
                WorkOrderCode = planWorkOrderEntity?.OrderCode,
                WorkOrderDownTime = planWorkOrderEntity?.CreatedOn,
                WorkOrderQty = planWorkOrderEntity?.Qty
            };
            return managePanelReportDto;
        }

        /// <summary>
        /// 时间段定义
        /// </summary>
        private static List<ProductionManagePanelModuleRangeDto> DateTimeRanges = new List<ProductionManagePanelModuleRangeDto> {
            new ProductionManagePanelModuleRangeDto
            {
                Sort=1,
                DateTimeRange="08:30-10:30",
                StartTime= new TimeSpan(8, 30, 0),
                EndTime = new TimeSpan(10, 30,0)
            }
        };
        /// <summary>
        /// 获取模组达成信息
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<ProductionManagePanelModuleDto> GetModuleAchievingInfoAsync(long siteId)
        {
            return new ProductionManagePanelModuleDto();
        }

    }
}
