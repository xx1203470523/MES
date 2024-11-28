using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.NioPushCollection;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Minio;
using System;
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

        private readonly IExcelService _excelService;
        private readonly ILocalizationService _localizationService;
        private readonly IMinioService _minioService;

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
            IManuSfcSummaryRepository manuSfcSummaryRepository,
            IExcelService excelService,
            ILocalizationService localizationService,
            IMinioService minioService
            )
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
            _excelService = excelService;
            _localizationService = localizationService;
            _minioService = minioService;
        }

        /// <summary>
        /// 根据查询条件获取工单步骤控制报表分页数据
        /// 优化: 不模糊查询，且通过关联ID查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WorkOrderStepControlViewDto>> GetWorkOrderStepControlPageListAsync(WorkOrderStepControlOptimizePagedQueryDto param)
        {
            var siteId = _currentSite.SiteId ?? 0;
            var pagedQuery = param.ToQuery<PlanWorkOrderPagedQuery>();
            pagedQuery.SiteId = siteId;
            // 判断是否有获取到站点码 
            if (string.IsNullOrWhiteSpace(param.OrderCode))
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

            var planWorkOrder = pagedInfo.Data.FirstOrDefault();
            var orderId = planWorkOrder?.Id ?? 0;
            var processRouteId = planWorkOrder?.ProcessRouteId ?? 0;
            var productId = planWorkOrder?.ProductId??0;
            var manuSfcProduceResultquery = new ManuSfcProduceVehiclePagedQuery()
            {
                WorkOrderId = orderId,
                //ProductId = pagedInfo.Data.First().ProductId,
                ProcessRouteId = processRouteId,
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
            var procProcessRouteDetailNodeTask = _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(processRouteId);
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
                    SiteId = siteId,
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
                var material = materials.FirstOrDefault(x => x.Id == productId);
                var passDownQuantity = passViews.Sum(x => x.Qty);
                var processDownQuantity = activityViews.Sum(x => x.Qty);
                var lockQuantity = lockViews.Sum(x => x.Qty);

                var finishProductQuantity = 0m;
                var summaryEntities=summaryResult.Where(x => x.ProcedureId == item.ProcedureId);
                if(summaryEntities.Any())
                {
                    var sfcSummaryEntities = summaryEntities.GroupBy(x => x.SFC).Select(g => g.Last());

                    //一个条码有多个的取后面那个
                    finishProductQuantity = sfcSummaryEntities.Sum(x => x.OutputQty??0);
                }           

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
                    ProcessRout = planWorkOrder?.ProcessRouteCode + "/" + planWorkOrder?.ProcessRouteVersion,
                    OrderCode = planWorkOrder?.OrderCode ?? "",
                    PassDownQuantity = passDownQuantity,
                    ProcessDownQuantity = processDownQuantity,
                    ScrapQuantity = scrapQuantity,
                    LockQuantity = lockQuantity,
                    FinishProductQuantity = finishProductQuantity,
                });
            }

            return new PagedInfo<WorkOrderStepControlViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, listDto.Count());
        }

        /// <summary>
        /// 根据查询条件导出参数数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<NioPushCollectionExportResultDto> ExprotAsync(WorkOrderStepControlOptimizePagedQueryDto param)
        {
            var siteId = _currentSite.SiteId ?? 0;
            var pagedQuery = param.ToQuery<PlanWorkOrderPagedQuery>();
            pagedQuery.PageSize = 1000;
            pagedQuery.SiteId = siteId;
            // 判断是否有获取到站点码 
            if (string.IsNullOrWhiteSpace(param.OrderCode))
            {
                return null;
            }
            //查询工单表信息
            var pagedInfoList = await _planWorkOrderRepository.GetPagedInfoAsyncCode(pagedQuery);

            List<WorkOrderStepControlViewDto> dtos = new();
            
            if (pagedInfoList.Data.Any())
            {
                var planWorkOrder = pagedInfoList.Data.FirstOrDefault();
                var orderId = planWorkOrder?.Id ?? 0;
                var processRouteId = planWorkOrder?.ProcessRouteId ?? 0;
                var productId = planWorkOrder?.ProductId ?? 0;
                var manuSfcProduceResultquery = new ManuSfcProduceVehiclePagedQuery()
                {
                    WorkOrderId = orderId,
                    //ProductId = pagedInfo.Data.First().ProductId,
                    ProcessRouteId = processRouteId,
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
                var materialsTask = _procMaterialRepository.GetByIdsAsync(pagedInfoList.Data.Select(x => x.ProductId));
                // 查询工序节点明细
                var procProcessRouteDetailNodeTask = _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(processRouteId);
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
                        SiteId = siteId,
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
                    var material = materials.FirstOrDefault(x => x.Id == productId);
                    var passDownQuantity = passViews.Sum(x => x.Qty);
                    var processDownQuantity = activityViews.Sum(x => x.Qty);
                    var lockQuantity = lockViews.Sum(x => x.Qty);

                    var finishProductQuantity = 0m;
                    var summaryEntities = summaryResult.Where(x => x.ProcedureId == item.ProcedureId);
                    if (summaryEntities.Any())
                    {
                        var sfcSummaryEntities = summaryEntities.GroupBy(x => x.SFC).Select(g => g.Last());

                        //一个条码有多个的取后面那个
                        finishProductQuantity = sfcSummaryEntities.Sum(x => x.OutputQty ?? 0);
                    }

                    var scrapViews = manuSfcProduceResult.Data.Where(x => x.ProcedureId == item.ProcedureId && x.Status == SfcStatusEnum.Scrapping);
                    var scrapQuantity = 0m;
                    if (scrapViews.Any())
                    {
                        var sfcids = scrapViews.Select(x => x.SFCId.GetValueOrDefault()).Distinct().ToArray();
                        var sfcInfoIds = sfcInfoEntities.Where(x => sfcids.Contains(x.SfcId)).Select(x => x.Id).ToArray();
                        scrapQuantity = sfcScrapEntities.Where(x => sfcInfoIds.Contains(x.SfcinfoId)).ToArray().Sum(x => x.ScrapQty) ?? 0;
                    }

                    var procedures = procProcedures?.FirstOrDefault(x => x.Id == item.ProcedureId);
                    dtos.Add(new WorkOrderStepControlViewDto
                    {
                        OrderId = orderId,
                        ProcedureId = procedures?.Id ?? 0,
                        Serialno = item.ManualSortNumber,
                        ProcedureCode = procedures?.Code ?? "",
                        MaterialCode = material != null ? material.MaterialCode + "/" + material.Version : "",
                        ProcessRout = planWorkOrder?.ProcessRouteCode + "/" + planWorkOrder?.ProcessRouteVersion,
                        OrderCode = planWorkOrder?.OrderCode ?? "",
                        PassDownQuantity = passDownQuantity,
                        ProcessDownQuantity = processDownQuantity,
                        ScrapQuantity = scrapQuantity,
                        LockQuantity = lockQuantity,
                        FinishProductQuantity = finishProductQuantity,
                    });
                }
            }
            

            // 实体到DTO转换 装载数据
            //var dtos = pagedInfoList.Data.Select(s => s.ToModel<WorkOrderStepControlViewDto>());

            var pagedInfo = new PagedInfo<WorkOrderStepControlViewDto>(dtos, pagedInfoList.PageIndex, pagedInfoList.PageSize, pagedInfoList.TotalCount);

            //实体到DTO转换 装载数据
            List<WorkOrderStepControlViewExportDto> listDto = new();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ManuBoxReport"), _localizationService.GetResource("ManuBoxReport"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new NioPushCollectionExportResultDto
                {
                    FileName = _localizationService.GetResource("ManuBoxReport"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }

            if (pagedInfoList.Data.Any())
            {
                var planWorkOrder = pagedInfoList.Data.FirstOrDefault();
                var orderId = planWorkOrder?.Id ?? 0;
                var processRouteId = planWorkOrder?.ProcessRouteId ?? 0;
                var productId = planWorkOrder?.ProductId ?? 0;
                var manuSfcProduceResultquery = new ManuSfcProduceVehiclePagedQuery()
                {
                    WorkOrderId = orderId,
                    //ProductId = pagedInfo.Data.First().ProductId,
                    ProcessRouteId = processRouteId,
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
                var materialsTask = _procMaterialRepository.GetByIdsAsync(pagedInfoList.Data.Select(x => x.ProductId));
                // 查询工序节点明细
                var procProcessRouteDetailNodeTask = _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(processRouteId);
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
                        SiteId = siteId,
                        SfcinfoIds = sfcInfoIds
                    });
                }

                var list = procProcessRouteDetailNode.ToList();
                list.RemoveAt(list.Count - 1);
                //对应的excel数值从这里开始
                foreach (var item in list)
                {
                    var passViews = manuSfcProduceResult.Data.Where(x => x.ProcedureId == item.ProcedureId && x.Status == SfcStatusEnum.lineUp);
                    var activityViews = manuSfcProduceResult.Data.Where(x => x.ProcedureId == item.ProcedureId && x.Status == SfcStatusEnum.Activity);
                    var lockViews = manuSfcProduceResult.Data.Where(x => x.ProcedureId == item.ProcedureId && x.Status == SfcStatusEnum.Locked);
                    var material = materials.FirstOrDefault(x => x.Id == productId);
                    var passDownQuantity = passViews.Sum(x => x.Qty);
                    var processDownQuantity = activityViews.Sum(x => x.Qty);
                    var lockQuantity = lockViews.Sum(x => x.Qty);

                    var finishProductQuantity = 0m;
                    var summaryEntities = summaryResult.Where(x => x.ProcedureId == item.ProcedureId);
                    if (summaryEntities.Any())
                    {
                        var sfcSummaryEntities = summaryEntities.GroupBy(x => x.SFC).Select(g => g.Last());

                        //一个条码有多个的取后面那个
                        finishProductQuantity = sfcSummaryEntities.Sum(x => x.OutputQty ?? 0);
                    }

                    var scrapViews = manuSfcProduceResult.Data.Where(x => x.ProcedureId == item.ProcedureId && x.Status == SfcStatusEnum.Scrapping);
                    var scrapQuantity = 0m;
                    if (scrapViews.Any())
                    {
                        var sfcids = scrapViews.Select(x => x.SFCId.GetValueOrDefault()).Distinct().ToArray();
                        var sfcInfoIds = sfcInfoEntities.Where(x => sfcids.Contains(x.SfcId)).Select(x => x.Id).ToArray();
                        scrapQuantity = sfcScrapEntities.Where(x => sfcInfoIds.Contains(x.SfcinfoId)).ToArray().Sum(x => x.ScrapQty) ?? 0;
                    }

                    var procedures = procProcedures?.FirstOrDefault(x => x.Id == item.ProcedureId);
                    listDto.Add(new WorkOrderStepControlViewExportDto()
                    {
                        Serialno = item.ManualSortNumber,
                        ProcedureCode = procedures?.Code ?? "",
                        MaterialCode = material != null ? material.MaterialCode + "/" + material.Version : "",
                        ProcessRout = planWorkOrder?.ProcessRouteCode + "/" + planWorkOrder?.ProcessRouteVersion,
                        OrderCode = planWorkOrder?.OrderCode ?? "",
                        PassDownQuantity = passDownQuantity,
                        ProcessDownQuantity = processDownQuantity,
                        ScrapQuantity = scrapQuantity,
                        LockQuantity = lockQuantity,
                        FinishProductQuantity = finishProductQuantity,
                    });
                }

            }


            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ManuBoxReport"), _localizationService.GetResource("ManuBoxReport"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new NioPushCollectionExportResultDto
            {
                FileName = _localizationService.GetResource("ManuBoxReport"),
                Path = uploadResult.AbsoluteUrl,
            };

        }


    }
}
