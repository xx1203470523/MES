using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Report;
using System.Collections.Generic;
using System.Security.Policy;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 工单报告 服务
    /// </summary>
    public class WorkOrderStepControlService : IWorkOrderStepControlService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 表 仓储
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcScrapRepository _sfcScrapRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;

        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;
        private readonly IProcBomRepository _procBomRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcSummaryRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="sfcScrapRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcessRouteRepository"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procBomRepository"></param>

        public WorkOrderStepControlService(ICurrentUser currentUser, ICurrentSite currentSite, IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcScrapRepository sfcScrapRepository, IProcProcedureRepository procProcedureRepository,
            IPlanWorkOrderRepository planWorkOrderRepository, IProcMaterialRepository procMaterialRepository,
            IProcProcessRouteRepository procProcessRouteRepository, IProcBomRepository procBomRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository, IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcSummaryRepository manuSfcSummaryRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;

            _manuSfcInfoRepository = manuSfcInfoRepository;
            _sfcScrapRepository = sfcScrapRepository;
            _procProcedureRepository = procProcedureRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _procBomRepository = procBomRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcSummaryRepository = manuSfcSummaryRepository;
        }

        /// <summary>
        /// 根据查询条件获取工单步骤控制报表分页数据
        /// 优化: 不模糊查询，且通过关联ID查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WorkOrderStepControlViewDto>> GetWorkOrderStepControlPageListAsync(WorkOrderStepControlOptimizePagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<PlanWorkOrderPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            // 判断是否有获取到站点码 
            if (param.OrderCode == null)
            {
                return null;
            }
            //查询工单表信息
            var pagedInfo = await _planWorkOrderRepository.GetPagedInfoAsyncCode(pagedQuery);

            List<WorkOrderStepControlViewDto> listDto = new();
            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return new PagedInfo<WorkOrderStepControlViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, listDto.Count());
            }

            var siteId = _currentSite.SiteId ?? 0;
            var orderId = pagedInfo.Data.FirstOrDefault()?.Id ?? 0;
            var manuSfcProduceResultquery = new ManuSfcProduceVehiclePagedQuery()
            {
                WorkOrderId = orderId,
                //ProductId = pagedInfo.Data.First().ProductId,
                ProcessRouteId = pagedInfo.Data.First().ProcessRouteId,
                PageIndex = 1,
                PageSize = 100000,
                SiteId = siteId
            };
            var summaryResultquery = new ManuSfcProduceVehiclePagedQuery()
            {
                WorkOrderId = orderId,
                //ProductId = pagedInfo.Data.First().ProductId,
                PageIndex = 1,
                PageSize = 100000,
                SiteId = siteId
            };
            // 查询物料
            var materialsTask = _procMaterialRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.ProductId));
            // 查询工序节点明细
            var procProcessRouteDetailNodeTask = _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(pagedInfo.Data.First().ProcessRouteId ?? 0);
            //查询工序
            var procProceduresTask = _procProcedureRepository.GetByIdsAsync(procProcessRouteDetailNodeTask.Result.Select(x => x.ProcedureId));

            var materials = await materialsTask;
            var procProcessRouteDetailNode = await procProcessRouteDetailNodeTask;
            var procProcedures = await procProceduresTask;
            var summaryResult = await _manuSfcSummaryRepository.GetWorkOrderAsync(summaryResultquery);
            var manuSfcProduceResult = await _manuSfcProduceRepository.GetStepPageListAsync(manuSfcProduceResultquery);

            var sfcIds = manuSfcProduceResult.Data.Where(x => x.Status == SfcStatusEnum.Scrapping).Select(x => x.SFCId.GetValueOrDefault()).Distinct().ToList();
            IEnumerable<ManuSfcInfoEntity> sfcInfoEntities = new List<ManuSfcInfoEntity>();
            IEnumerable<ManuSfcScrapEntity> sfcScrapEntities = new List<ManuSfcScrapEntity>();
            if (sfcIds.Any())
            {
                sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcIds);
                var sfcInfoIds = sfcInfoEntities.Select(x => x.Id).Distinct().ToList();
                sfcScrapEntities = await _sfcScrapRepository.GetEntitiesAsync(new ManuSfcScrapQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    SfcinfoIds = sfcInfoIds
                });
            }

            var list = procProcessRouteDetailNode.ToList();
            list.RemoveAt(list.Count - 1);
            foreach (var item in list)
            {
                var passViews = manuSfcProduceResult.Data.Where(x => x.ProcedureId == item.ProcedureId && x.Status == SfcStatusEnum.lineUp);
                var activityViews = manuSfcProduceResult.Data.Where(x => x.ProcedureId == item.ProcedureId && x.Status == SfcStatusEnum.Activity);
                var lockViews = manuSfcProduceResult.Data.Where(x => x.ProcedureId == item.ProcedureId && x.Status == SfcStatusEnum.Locked);
                var material = materials.FirstOrDefault(x => x.Id == pagedInfo.Data.First().ProductId);
                var passDownQuantity = passViews.Sum(x => x.Qty);
                var processDownQuantity = activityViews.Sum(x => x.Qty);
                var lockQuantity = lockViews.Sum(x => x.Qty);
                var finishProductQuantity = summaryResult.LastOrDefault(x =>  x.ProcedureId == item.ProcedureId)?.OutputQty;

                var scrapViews = manuSfcProduceResult.Data.Where(x => x.ProcedureId == item.ProcedureId && x.Status == SfcStatusEnum.Scrapping);
                var scrapQuantity = 0m;
                if (scrapViews.Any())
                {
                    var sfcids = scrapViews.Select(x => x.SFCId.GetValueOrDefault()).Distinct().ToArray();
                    var sfcInfoIds = sfcInfoEntities.Where(x => sfcids.Contains(x.SfcId)).Select(x => x.Id).ToArray();
                    scrapQuantity = sfcScrapEntities.Where(x => sfcInfoIds.Contains(x.SfcinfoId)).ToArray().Sum(x => x.ScrapQty) ?? 0;
                }

                var procedures = procProcedures?.FirstOrDefault(x => x.Id == item.ProcedureId);
                listDto.Add(new WorkOrderStepControlViewDto
                {
                    OrderId = orderId,
                    ProcedureId = procedures?.Id ?? 0,
                    Serialno = item.ManualSortNumber,
                    ProcedureCode = procedures?.Code ?? "",
                    MaterialCode = material != null ? material.MaterialCode + "/" + material.Version : "",
                    ProcessRout = pagedInfo.Data.First().ProcessRouteCode + "/" + pagedInfo.Data.First().ProcessRouteVersion,
                    OrderCode = pagedInfo.Data.FirstOrDefault()?.OrderCode ?? "",
                    PassDownQuantity = passDownQuantity,
                    ProcessDownQuantity = processDownQuantity,
                    ScrapQuantity = scrapQuantity,
                    LockQuantity = lockQuantity,
                    FinishProductQuantity = finishProductQuantity ?? 0,
                });
            }

            return new PagedInfo<WorkOrderStepControlViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, listDto.Count());
        }
    }
}
