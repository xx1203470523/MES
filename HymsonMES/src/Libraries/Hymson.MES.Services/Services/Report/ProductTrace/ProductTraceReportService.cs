using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.ProcessRoute.Query;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Globalization;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 产品追溯报表服务
    /// </summary>
    public class ProductTraceReportService : IProductTraceReportService
    {
        #region 依赖注入
        private readonly IMinioService _minioService;
        private readonly IExcelService _excelService;
        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;
        /// <summary>
        /// 资源
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;
        /// <summary>
        /// 工序
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;
        /// <summary>
        /// 工单信息表 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        /// <summary>
        /// 条码流转信息
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
        /// <summary>
        /// 产品参数/设备参数仓储
        /// </summary>
        private readonly IManuProductParameterRepository _manuProductParameterRepository;
        /// <summary>
        /// 设备仓储
        /// </summary>
        private readonly IEquEquipmentRepository _equipmentRepository;
        /// <summary>
        /// 条码步骤仓储
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        /// <summary>
        /// 条码记录仓储
        /// </summary>
        private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;


        /// <summary>
        /// 生产工艺路线节点
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;
        /// <summary>
        /// 条码信息
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        /// <summary>
        /// 条码仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        public ProductTraceReportService(IMinioService minioService, IExcelService excelService, ICurrentSite currentSite,
            ICurrentUser currentUser,
        IProcMaterialRepository procMaterialRepository,
         IPlanWorkOrderRepository planWorkOrderRepository,
         IManuSfcCirculationRepository manuSfcCirculationRepository,
         IProcResourceRepository procResourceRepository,
         IProcProcedureRepository procProcedureRepository,
         IManuProductParameterRepository manuProductParameterRepository,
         IEquEquipmentRepository equipmentRepository,
         IManuSfcStepRepository manuSfcStepRepository,
         IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
         IManuSfcInfoRepository manuSfcInfoRepository,
         IManuSfcRepository manuSfcRepository,
         IManuSfcSummaryRepository manuSfcSummaryRepository
          )
        {
            _minioService = minioService;
            _excelService = excelService;
            _currentSite = currentSite;
            _currentUser = currentUser;
            _procMaterialRepository = procMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _procResourceRepository = procResourceRepository;
            _procProcedureRepository = procProcedureRepository;
            _manuProductParameterRepository = manuProductParameterRepository;
            _equipmentRepository = equipmentRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcSummaryRepository = manuSfcSummaryRepository;
        }
        #endregion

        /// <summary>
        /// 获取条码追溯信息
        /// </summary>
        /// <param name="productTracePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcCirculationViewDto>> GetProductTracePagedListAsync(ProductTracePagedQueryDto productTracePagedQueryDto)
        {
            var productTraceReportPagedQuery = productTracePagedQueryDto.ToQuery<ProductTraceReportPagedQuery>();
            productTraceReportPagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            //追溯分页查询
            var pagedInfo = await _manuSfcCirculationRepository.GetProductTraceReportPagedInfoAsync(productTraceReportPagedQuery);
            //工单信息
            IEnumerable<PlanWorkOrderEntity> planWorkOrders = new List<PlanWorkOrderEntity>();
            var workOrderIds = pagedInfo.Data.Select(c => c.WorkOrderId).ToArray();
            if (workOrderIds.Any())
            {
                planWorkOrders = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);
            }
            //产品信息
            IEnumerable<ProcMaterialEntity> procMaterials = new List<ProcMaterialEntity>();
            var procMaterialIds = pagedInfo.Data.Select(c => c.ProductId).ToArray();
            if (procMaterialIds.Any())
            {
                procMaterials = await _procMaterialRepository.GetByIdsAsync(procMaterialIds);
            }
            //资源信息
            IEnumerable<ProcResourceEntity> procResources = new List<ProcResourceEntity>();
            var procResourcesIds = pagedInfo.Data.Select(c => c.ResourceId ?? -1).ToArray();
            if (procResourcesIds.Any())
            {
                procResources = await _procResourceRepository.GetListByIdsAsync(procResourcesIds);
            }
            //工序信息
            IEnumerable<ProcProcedureEntity> procProcedures = new List<ProcProcedureEntity>();
            var procProcedureIds = pagedInfo.Data.Select(c => c.ProcedureId).ToArray();
            if (procProcedureIds.Any())
            {
                procProcedures = await _procProcedureRepository.GetByIdsAsync(procProcedureIds);
            }
            //设备信息
            IEnumerable<EquEquipmentEntity> equEquipments = new List<EquEquipmentEntity>();
            var equEquipmentIds = pagedInfo.Data.Select(c => c.EquipmentId ?? -1).ToArray();
            if (equEquipmentIds.Any())
            {
                equEquipments = await _equipmentRepository.GetByIdsAsync(equEquipmentIds);
            }
            var dtos = pagedInfo.Data.Select(s =>
            {
                var returnView = s.ToModel<ManuSfcCirculationViewDto>();
                //工单信息
                var planWorkOrder = planWorkOrders.Where(c => c.Id == s.WorkOrderId).FirstOrDefault();
                if (planWorkOrder != null)
                {
                    returnView.WorkOrderCode = planWorkOrder.OrderCode;
                }
                //产品信息
                var procMaterial = procMaterials.Where(c => c.Id == s.ProductId).FirstOrDefault();
                if (procMaterial != null)
                {
                    returnView.ProductCode = procMaterial.MaterialCode;
                    returnView.ProductName = procMaterial.MaterialName;
                }
                //资源信息
                var procResource = procResources.Where(c => c.Id == s.ResourceId).FirstOrDefault();
                if (procResource != null)
                {
                    returnView.ResourceCode = procResource.ResCode;
                    returnView.ResourceName = procResource.ResName;
                }
                //工序信息
                var procProcedure = procProcedures.Where(c => c.Id == s.ProcedureId).FirstOrDefault();
                if (procProcedure != null)
                {
                    returnView.ProcedureCode = procProcedure.Code;
                    returnView.ProcedureName = procProcedure.Name;
                }
                //设备信息
                var equEquipment = equEquipments.Where(c => c.Id == s.EquipmentId).FirstOrDefault();
                if (equEquipment != null)
                {
                    returnView.EquipentCode = equEquipment.EquipmentCode;
                    returnView.EquipentName = equEquipment.EquipmentName;
                }
                return returnView;
            });
            return new PagedInfo<ManuSfcCirculationViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取产品参数/设备参数信息
        /// </summary>
        /// <returns></returns>
        public async Task<PagedInfo<ManuProductParameterViewDto>> GetProductPrameterPagedListAsync(ManuProductPrameterPagedQueryDto manuProductPrameterPagedQueryDto)
        {
            var manuProductParameterPagedQuery = manuProductPrameterPagedQueryDto.ToQuery<ManuProductParameterPagedQuery>();
            manuProductParameterPagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            //参数分页查询
            var pagedInfo = await _manuProductParameterRepository.GetManuProductParameterPagedInfoAsync(manuProductParameterPagedQuery);
            //资源信息
            IEnumerable<ProcResourceEntity> procResources = new List<ProcResourceEntity>();
            var procResourcesIds = pagedInfo.Data.Select(c => c.ResourceId).ToArray();
            if (procResourcesIds.Any())
            {
                procResources = await _procResourceRepository.GetListByIdsAsync(procResourcesIds);
            }
            //工序信息
            IEnumerable<ProcProcedureEntity> procProcedures = new List<ProcProcedureEntity>();
            var procProcedureIds = pagedInfo.Data.Select(c => c.ProcedureId).ToArray();
            if (procProcedureIds.Any())
            {
                procProcedures = await _procProcedureRepository.GetByIdsAsync(procProcedureIds);
            }
            //设备信息
            IEnumerable<EquEquipmentEntity> equEquipments = new List<EquEquipmentEntity>();
            var equEquipmentIds = pagedInfo.Data.Select(c => c.EquipmentId).ToArray();
            if (equEquipmentIds.Any())
            {
                equEquipments = await _equipmentRepository.GetByIdsAsync(equEquipmentIds);
            }
            var dtos = pagedInfo.Data.Select(s =>
            {
                var returnView = s.ToModel<ManuProductParameterViewDto>();
                //资源信息
                var procResource = procResources.Where(c => c.Id == s.ResourceId).FirstOrDefault();
                if (procResource != null)
                {
                    returnView.ResourceCode = procResource.ResCode;
                    returnView.ResourceName = procResource.ResName;
                }
                //工序信息
                var procProcedure = procProcedures.Where(c => c.Id == s.ProcedureId).FirstOrDefault();
                if (procProcedure != null)
                {
                    returnView.ProcedureCode = procProcedure.Code;
                    returnView.ProcedureName = procProcedure.Name;
                }
                //设备信息
                var equEquipment = equEquipments.Where(c => c.Id == s.EquipmentId).FirstOrDefault();
                if (equEquipment != null)
                {
                    returnView.EquipmentCode = equEquipment.EquipmentCode;
                    returnView.EquipmentName = equEquipment.EquipmentName;
                }
                return returnView;
            });
            return new PagedInfo<ManuProductParameterViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询生产步骤（条码履历）
        /// </summary>
        /// <param name="manuSfcStepPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcStepViewDto>> GetSfcStepPagedListAsync(ManuSfcStepPagedQueryDto manuSfcStepPagedQueryDto)
        {
            //查询条码所有步骤数据
            var manuSfcStepPagedQuery = manuSfcStepPagedQueryDto.ToQuery<ManuSfcStepPagedQuery>();
            manuSfcStepPagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _manuSfcStepRepository.GetPagedInfoAsync(manuSfcStepPagedQuery);
            //工单信息
            IEnumerable<PlanWorkOrderEntity> planWorkOrders = new List<PlanWorkOrderEntity>();
            var workOrderIds = pagedInfo.Data.Select(c => c.WorkOrderId).ToArray();
            if (workOrderIds.Any())
            {
                planWorkOrders = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);
            }
            //产品ID
            IEnumerable<ProcMaterialEntity> procMaterials = new List<ProcMaterialEntity>();
            var procMaterialIds = planWorkOrders.Select(c => c.ProductId).ToArray();
            if (procMaterialIds.Any())
            {
                procMaterials = await _procMaterialRepository.GetByIdsAsync(procMaterialIds);
            }
            //资源信息
            IEnumerable<ProcResourceEntity> procResources = new List<ProcResourceEntity>();
            var procResourcesIds = pagedInfo.Data.Select(c => c.ResourceId ?? -1).ToArray();
            if (procResourcesIds.Any())
            {
                procResources = await _procResourceRepository.GetListByIdsAsync(procResourcesIds);
            }
            //工序信息
            IEnumerable<ProcProcedureEntity> procProcedures = new List<ProcProcedureEntity>();
            var procProcedureIds = pagedInfo.Data.Select(c => c.ProcedureId ?? -1).ToArray();
            if (procProcedureIds.Any())
            {
                procProcedures = await _procProcedureRepository.GetByIdsAsync(procProcedureIds);
            }
            //设备信息
            IEnumerable<EquEquipmentEntity> equEquipments = new List<EquEquipmentEntity>();
            var equEquipmentIds = pagedInfo.Data.Select(c => c.EquipmentId ?? -1).ToArray();
            if (equEquipmentIds.Any())
            {
                equEquipments = await _equipmentRepository.GetByIdsAsync(equEquipmentIds);
            }
            //填充工序信息
            var returnDtos = pagedInfo.Data.Select(s =>
            {
                var returnView = s.ToModel<ManuSfcStepViewDto>();
                //资源信息
                var procResource = procResources.Where(c => c.Id == s.ResourceId).FirstOrDefault();
                if (procResource != null)
                {
                    returnView.ResourceCode = procResource.ResCode;
                    returnView.ResourceName = procResource.ResName;
                }
                //工序信息
                var procProcedure = procProcedures.Where(c => c.Id == s.ProcedureId).FirstOrDefault();
                if (procProcedure != null)
                {
                    returnView.ProcedureCode = procProcedure.Code;
                    returnView.ProcedureName = procProcedure.Name;
                }
                //设备信息
                var equEquipment = equEquipments.Where(c => c.Id == s.EquipmentId).FirstOrDefault();
                if (equEquipment != null)
                {
                    returnView.EquipmentCode = equEquipment.EquipmentCode;
                    returnView.EquipmentName = equEquipment.EquipmentName;
                }

                //工单信息
                var planWorkOrder = planWorkOrders.Where(c => c.Id == s.WorkOrderId).FirstOrDefault();
                if (planWorkOrder != null)
                {
                    returnView.WorkOrderType = planWorkOrder.Type;
                    returnView.ProductName = procMaterials?.FirstOrDefault(x => x.Id == planWorkOrder.ProductId)?.MaterialName ?? "";
                }

                return returnView;
            });
            return new PagedInfo<ManuSfcStepViewDto>(returnDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询生产工艺信息
        /// </summary>
        /// <param name="procSfcProcessRoutePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcSfcProcessRouteViewDto>> GetSfcProcessRoutePagedListAsync(ProcSfcProcessRoutePagedQueryDto procSfcProcessRoutePagedQueryDto)
        {
            var procProcessRouteDetailNodeQuery = procSfcProcessRoutePagedQueryDto.ToQuery<ProcProcessRouteDetailNodePagedQuery>();
            IEnumerable<ProcSfcProcessRouteViewDto> procSfcProcessRouteViewDtos = new List<ProcSfcProcessRouteViewDto>();
            var emptyResult = new PagedInfo<ProcSfcProcessRouteViewDto>(procSfcProcessRouteViewDtos, procSfcProcessRoutePagedQueryDto.PageIndex, procSfcProcessRoutePagedQueryDto.PageSize, 0);
            //查询条码信息
            var manuSfcEntity = await _manuSfcRepository.GetBySFCAsync(new GetBySfcQuery { SFC = procSfcProcessRoutePagedQueryDto.SFC, SiteId = _currentSite.SiteId });
            if (manuSfcEntity == null)
            {
                return emptyResult;
            }
            var sfcinfo = await _manuSfcInfoRepository.GetBySFCNoCheckUsedAsync(manuSfcEntity?.Id ?? 0);
            if (sfcinfo == null)
            {
                return emptyResult;
            }
            //条码对应工单信息
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(sfcinfo.WorkOrderId);
            if (planWorkOrderEntity == null)
            {
                return emptyResult;
            }
            //工艺路线明细
            procProcessRouteDetailNodeQuery.ProcessRouteId = planWorkOrderEntity.ProcessRouteId;
            var pagedInfo = await _procProcessRouteDetailNodeRepository.GetPagedInfoAsync(procProcessRouteDetailNodeQuery);
            //查询条码步骤
            var manuSfcStepEntities = await _manuSfcStepRepository.GetManuSfcStepEntitiesAsync(new ManuSfcStepQuery { SFC = manuSfcEntity.SFC, SiteId = _currentSite.SiteId ?? 123456 });
            procSfcProcessRouteViewDtos = pagedInfo.Data.Select(c =>
            {
                var sfcProcessRouteViewDto = GetProcessRouteDetailStep(c, manuSfcStepEntities);
                //处理结束工序编码为空的显示
                if (string.IsNullOrEmpty(sfcProcessRouteViewDto.ProcedureCode))
                {
                    sfcProcessRouteViewDto.ProcedureName = "结束";
                    sfcProcessRouteViewDto.CurrentStatus = SfcProduceStatusEnum.Complete;
                }
                return sfcProcessRouteViewDto;
            });

            return new PagedInfo<ProcSfcProcessRouteViewDto>(procSfcProcessRouteViewDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取工艺路线明细的进出站步骤记录
        /// </summary>
        /// <param name="routeDetailNodeView"></param>
        /// <param name="manuSfcStepEntities"></param>
        /// <returns></returns>
        private ProcSfcProcessRouteViewDto GetProcessRouteDetailStep(ProcProcessRouteDetailNodeView routeDetailNodeView, IEnumerable<ManuSfcStepEntity> manuSfcStepEntities)
        {
            //进站步骤
            var inStockSfcStepEntities = manuSfcStepEntities.Where(m => m.ProcedureId == routeDetailNodeView.ProcedureId && m.Operatetype == ManuSfcStepTypeEnum.InStock)
                                            .OrderBy(o => o.CreatedOn).ToList();
            //出站步骤
            var outStockSfcStepEntities = manuSfcStepEntities.Where(m => m.ProcedureId == routeDetailNodeView.ProcedureId && m.Operatetype == ManuSfcStepTypeEnum.OutStock)
                                            .OrderBy(o => o.CreatedOn).ToList();
            SfcProduceStatusEnum? currentStatus = null; //状态
            DateTime? inBountTime = null;//进站时间
            DateTime? outBountTime = null;//出站时间
            int? passed = null;//是否合格
            DateTime? createdOn = null;
            string createdBy = string.Empty;
            decimal? qty = null;
            string sfc = string.Empty;
            if (inStockSfcStepEntities.Count > 0)
            {
                sfc = inStockSfcStepEntities.First().SFC;
                inBountTime = inStockSfcStepEntities.First().CreatedOn;
                currentStatus = inStockSfcStepEntities.First().CurrentStatus;
                createdOn = inStockSfcStepEntities.First().CreatedOn;
                createdBy = inStockSfcStepEntities.First().CreatedBy;
                qty = inStockSfcStepEntities.First().Qty;
            }
            if (outStockSfcStepEntities.Count > 0)
            {
                currentStatus = outStockSfcStepEntities.Last().CurrentStatus;
                outBountTime = outStockSfcStepEntities.Last().CreatedOn;
                createdOn = outStockSfcStepEntities.Last().CreatedOn;
                createdBy = outStockSfcStepEntities.Last().CreatedBy;
                passed = outStockSfcStepEntities.Last().Passed ?? 1;
                qty = outStockSfcStepEntities.Last().Qty;
            }
            return new ProcSfcProcessRouteViewDto
            {
                ProcedureId = routeDetailNodeView.ProcedureId,
                ProcedureCode = routeDetailNodeView.Code,
                ProcedureName = routeDetailNodeView.Name,
                SFC = sfc,
                CurrentStatus = currentStatus,
                InBountTime = inBountTime,
                OutBountTime = outBountTime,
                Passed = passed,
                CreatedOn = createdOn,
                CreatedBy = createdBy,
                Qty = qty,
                SerialNo = routeDetailNodeView.SerialNo.ParseToInt(),
                Remark = routeDetailNodeView.Remark
            };
        }

        /// <summary>
        /// 查询条码对应工单信息
        /// </summary>
        /// <returns></returns>
        public async Task<PagedInfo<ProductTracePlanWorkOrderViewDto>> GetWorkOrderPagedListAsync(ProductTracePlanWorkOrderPagedQueryDto planWorkOrderPagedQueryDto)
        {
            var planWorkOrderPagedQuery = planWorkOrderPagedQueryDto.ToQuery<PlanWorkOrderPagedQuery>();
            IEnumerable<ProductTracePlanWorkOrderViewDto> productTracePlanWorkOrderViews = new List<ProductTracePlanWorkOrderViewDto>();
            //查询条码信息
            var manuSfcEntity = await _manuSfcRepository.GetBySFCAsync(new GetBySfcQuery { SFC = planWorkOrderPagedQueryDto.SFC, SiteId = _currentSite.SiteId });
            if (manuSfcEntity == null)
            {
                return new PagedInfo<ProductTracePlanWorkOrderViewDto>(productTracePlanWorkOrderViews, planWorkOrderPagedQueryDto.PageIndex, planWorkOrderPagedQueryDto.PageSize, 0);
            }
            var sfcinfo = await _manuSfcInfoRepository.GetBySFCNoCheckUsedAsync(manuSfcEntity?.Id ?? 0);
            if (sfcinfo == null)
            {
                return new PagedInfo<ProductTracePlanWorkOrderViewDto>(productTracePlanWorkOrderViews, planWorkOrderPagedQueryDto.PageIndex, planWorkOrderPagedQueryDto.PageSize, 0);
            }
            planWorkOrderPagedQuery.WorkOrderId = sfcinfo.WorkOrderId;
            planWorkOrderPagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _planWorkOrderRepository.GetPagedInfoAsync(planWorkOrderPagedQuery);
            var productTracePlanWorkOrderViewDtos = pagedInfo.Data.Select(s =>
            {
                return s.ToModel<ProductTracePlanWorkOrderViewDto>();
            });
            return new PagedInfo<ProductTracePlanWorkOrderViewDto>(productTracePlanWorkOrderViewDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 产品追溯报表导出
        /// </summary>
        /// <param name="planWorkOrderPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> ProductTracingReportExportAsync(ProductTracePagedQueryDto planWorkOrderPagedQueryDto)
        {
            string fileName = string.Format("({0})产品追朔", planWorkOrderPagedQueryDto.SFC);
            planWorkOrderPagedQueryDto.PageSize = 1000000;

            #region 工单信息
            var planWorkOrderPagedQuery = planWorkOrderPagedQueryDto.ToQuery<PlanWorkOrderPagedQuery>();
            IEnumerable<ProductTracePlanWorkOrderViewDto> productTracePlanWorkOrderViews = new List<ProductTracePlanWorkOrderViewDto>();
            //查询条码信息
            var manuSfcEntity = await _manuSfcRepository.GetBySFCAsync(new GetBySfcQuery { SFC = planWorkOrderPagedQueryDto.SFC, SiteId = _currentSite.SiteId });
            if (manuSfcEntity == null)
            {
                manuSfcEntity = new ManuSfcEntity();
            }
            var sfcinfo = await _manuSfcInfoRepository.GetBySFCNoCheckUsedAsync(manuSfcEntity?.Id ?? 0);
            if (sfcinfo == null)
            {
                sfcinfo = new ManuSfcInfoEntity();
            }
            planWorkOrderPagedQuery.WorkOrderId = sfcinfo.WorkOrderId;
            planWorkOrderPagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _planWorkOrderRepository.GetPagedInfoAsync(planWorkOrderPagedQuery);
            var planWorkOrderLists = pagedInfo.Data;
            var planWorkOrderReportExportDtos = new List<ProductTracePlanWorkOrderReportExportDto>();
            foreach (var manuProductParameterReport in planWorkOrderLists)
            {
                planWorkOrderReportExportDtos.Add(manuProductParameterReport.ToExcelModel<ProductTracePlanWorkOrderReportExportDto>());
            }
            #endregion

            #region 生产工艺

            //条码对应工单信息
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(sfcinfo.WorkOrderId);
            if (planWorkOrderEntity == null)
            {

                planWorkOrderEntity = new PlanWorkOrderEntity();
            }
            //工艺路线明细
            var procProcessRouteDetailNodeQuery = planWorkOrderPagedQueryDto.ToQuery<ProcProcessRouteDetailNodePagedQuery>();
            procProcessRouteDetailNodeQuery.ProcessRouteId = planWorkOrderEntity.ProcessRouteId;
            var processroutePagedInfo = await _procProcessRouteDetailNodeRepository.GetPagedInfoAsync(procProcessRouteDetailNodeQuery);
            //查询条码步骤
            var manuSfcStepEntities = await _manuSfcStepRepository.GetManuSfcStepEntitiesAsync(new ManuSfcStepQuery { SFC = manuSfcEntity?.SFC ?? "", SiteId = _currentSite.SiteId ?? 123456 });
            var procSfcProcessRouteReports = new List<ProcSfcProcessRouteReportExportDto>();
            var processRouteLists = processroutePagedInfo.Data;
            foreach (var procProcess in processRouteLists)
            {
                ProcSfcProcessRouteReportExportDto procSfcProcessRouteReportExportDto = new ProcSfcProcessRouteReportExportDto();
                var sfcProcessRouteViewDto = GetProcessRouteDetailStep(procProcess, manuSfcStepEntities);
                //处理结束工序编码为空的显示
                if (string.IsNullOrEmpty(sfcProcessRouteViewDto.ProcedureCode))
                {
                    procSfcProcessRouteReportExportDto.ProcedureName = "结束";
                    sfcProcessRouteViewDto.CurrentStatus = SfcProduceStatusEnum.Complete;
                }
                else
                {
                    procSfcProcessRouteReportExportDto.ProcedureCode = sfcProcessRouteViewDto.ProcedureCode;
                    procSfcProcessRouteReportExportDto.ProcedureName = sfcProcessRouteViewDto.ProcedureName;
                }
                if (sfcProcessRouteViewDto.CurrentStatus == null)
                {
                    procSfcProcessRouteReportExportDto.CurrentStatusStr = "未开始";
                }
                if (sfcProcessRouteViewDto.CurrentStatus == SfcProduceStatusEnum.lineUp)
                {
                    procSfcProcessRouteReportExportDto.CurrentStatusStr = "排队中";
                }
                if (sfcProcessRouteViewDto.CurrentStatus == SfcProduceStatusEnum.Activity)
                {
                    procSfcProcessRouteReportExportDto.CurrentStatusStr = "进行中";
                }
                if (sfcProcessRouteViewDto.CurrentStatus == SfcProduceStatusEnum.Complete)
                {
                    procSfcProcessRouteReportExportDto.CurrentStatusStr = "完成";
                }
                if (sfcProcessRouteViewDto.CurrentStatus == SfcProduceStatusEnum.Locked)
                {
                    procSfcProcessRouteReportExportDto.CurrentStatusStr = "锁定";
                }

                procSfcProcessRouteReportExportDto.Qty = sfcProcessRouteViewDto.Qty;
                if (sfcProcessRouteViewDto.Passed == 0)
                {
                    procSfcProcessRouteReportExportDto.Passed = "不合格";
                }
                if (sfcProcessRouteViewDto.Passed == 1)
                {
                    procSfcProcessRouteReportExportDto.Passed = "合格";
                }
                procSfcProcessRouteReportExportDto.CreatedOn = sfcProcessRouteViewDto.CreatedOn;
                procSfcProcessRouteReportExportDto.CreatedBy = sfcProcessRouteViewDto.CreatedBy;
                procSfcProcessRouteReports.Add(procSfcProcessRouteReportExportDto);
            }

            #endregion

            #region 设备参数
            var equProductprameterQuery = planWorkOrderPagedQueryDto.ToQuery<ManuProductParameterPagedQuery>();
            equProductprameterQuery.SiteId = _currentSite.SiteId ?? 123456;
            equProductprameterQuery.ParameterType = ParameterTypeEnum.Equipment;
            var equProductprameterpagedInfo = await _manuProductParameterRepository.GetManuProductParameterPagedInfoAsync(equProductprameterQuery);

            IEnumerable<ProcResourceEntity> equProcResources = new List<ProcResourceEntity>();
            var equprocResourcesIds = equProductprameterpagedInfo.Data.Select(c => c.ResourceId).ToArray();
            if (equprocResourcesIds.Any())
            {
                equProcResources = await _procResourceRepository.GetListByIdsAsync(equprocResourcesIds);
            }
            //工序信息
            IEnumerable<ProcProcedureEntity> equProcProcedures = new List<ProcProcedureEntity>();
            var equProcProcedureIds = equProductprameterpagedInfo.Data.Select(c => c.ProcedureId).ToArray();
            if (equProcProcedureIds.Any())
            {
                equProcProcedures = await _procProcedureRepository.GetByIdsAsync(equProcProcedureIds);
            }
            //设备信息
            IEnumerable<EquEquipmentEntity> equEquipmentsList = new List<EquEquipmentEntity>();
            var equEquipmentListIds = equProductprameterpagedInfo.Data.Select(c => c.EquipmentId).ToArray();
            if (equEquipmentListIds.Any())
            {
                equEquipmentsList = await _equipmentRepository.GetByIdsAsync(equEquipmentListIds);
            }


            var equManuProductsList = equProductprameterpagedInfo.Data;
            var equipmentPrameterReportExportDtos = new List<EquipmentPrameterReportExportDto>();
            foreach (var equmanuProduct in equManuProductsList)
            {
                EquipmentPrameterReportExportDto returnView = new EquipmentPrameterReportExportDto();
                //资源信息
                var procResource = equProcResources.Where(c => c.Id == equmanuProduct.ResourceId).FirstOrDefault();
                if (procResource != null)
                {
                    returnView.ResourceCode = procResource.ResCode;
                    returnView.ResourceName = procResource.ResName;
                }
                //工序信息
                var procProcedure = equProcProcedures.Where(c => c.Id == equmanuProduct.ProcedureId).FirstOrDefault();
                if (procProcedure != null)
                {
                    returnView.ProcedureCode = procProcedure.Code;
                    returnView.ProcedureName = procProcedure.Name;
                }
                //设备信息
                var equEquipment = equEquipmentsList.Where(c => c.Id == equmanuProduct.EquipmentId).FirstOrDefault();
                if (equEquipment != null)
                {
                    returnView.EquipmentCode = equEquipment.EquipmentCode;
                    returnView.EquipmentName = equEquipment.EquipmentName;
                }
                returnView.ParameterCode = equmanuProduct.ParameterCode;
                returnView.ParameterName = equmanuProduct.ParameterName;
                returnView.ParameterValue = equmanuProduct.ParameterValue;
                returnView.CreatedOn = equmanuProduct.CreatedOn;

                equipmentPrameterReportExportDtos.Add(returnView);
            }
            #endregion

            #region 产品参数
            var productprameterQuery = planWorkOrderPagedQueryDto.ToQuery<ManuProductParameterPagedQuery>();
            productprameterQuery.SiteId = _currentSite.SiteId ?? 123456;
            productprameterQuery.ParameterType = ParameterTypeEnum.Product;
            var productprameterpagedInfo = await _manuProductParameterRepository.GetManuProductParameterPagedInfoAsync(productprameterQuery);

            IEnumerable<ProcResourceEntity> procResources = new List<ProcResourceEntity>();
            var procResourcesIds = productprameterpagedInfo.Data.Select(c => c.ResourceId).ToArray();
            if (procResourcesIds.Any())
            {
                procResources = await _procResourceRepository.GetListByIdsAsync(procResourcesIds);
            }
            //工序信息
            IEnumerable<ProcProcedureEntity> procProcedures = new List<ProcProcedureEntity>();
            var procProcedureIds = productprameterpagedInfo.Data.Select(c => c.ProcedureId).ToArray();
            if (procProcedureIds.Any())
            {
                procProcedures = await _procProcedureRepository.GetByIdsAsync(procProcedureIds);
            }
            var productManuProductsList = productprameterpagedInfo.Data;
            var productPrameterReportExportDtos = new List<ProductPrameterReportExportDto>();
            foreach (var equmanuProduct in productManuProductsList)
            {
                ProductPrameterReportExportDto returnView = new ProductPrameterReportExportDto();
                //资源信息
                var procResource = procResources.Where(c => c.Id == equmanuProduct.ResourceId).FirstOrDefault();
                if (procResource != null)
                {
                    returnView.ResourceCode = procResource.ResCode;
                }
                //工序信息
                var procProcedure = procProcedures.Where(c => c.Id == equmanuProduct.ProcedureId).FirstOrDefault();
                if (procProcedure != null)
                {
                    returnView.ProcedureCode = procProcedure.Code;
                    returnView.ProcedureName = procProcedure.Name;
                }

                returnView.ParameterCode = equmanuProduct.ParameterCode;
                returnView.ParameterName = equmanuProduct.ParameterName;
                returnView.ParameterValue = equmanuProduct.ParameterValue;
                returnView.CreatedOn = equmanuProduct.CreatedOn;
                returnView.CreatedBy = equmanuProduct.CreatedBy;
                productPrameterReportExportDtos.Add(returnView);
            }
            #endregion

            #region 产品追朔
            var productTraceReportPagedQuery = planWorkOrderPagedQueryDto.ToQuery<ProductTraceReportPagedQuery>();
            productTraceReportPagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            //追溯分页查询
            var manuSfcCirculationPagedInfo = await _manuSfcCirculationRepository.GetProductTraceReportPagedInfoAsync(productTraceReportPagedQuery);
            //工序信息
            IEnumerable<ProcProcedureEntity> manuSfcprocProcedures = new List<ProcProcedureEntity>();
            var manuSfcprocProcedureIds = manuSfcCirculationPagedInfo.Data.Select(c => c.ProcedureId).ToArray();
            if (manuSfcprocProcedureIds.Any())
            {
                manuSfcprocProcedures = await _procProcedureRepository.GetByIdsAsync(manuSfcprocProcedureIds);
            }
            var manuSfcCirculationReportExportDtos = new List<ManuSfcCirculationReportExportDto>();
            foreach (var productParameterView in manuSfcCirculationPagedInfo.Data)
            {
                ManuSfcCirculationReportExportDto returnView = new ManuSfcCirculationReportExportDto();
                //工序信息
                var procProcedure = manuSfcprocProcedures.Where(c => c.Id == productParameterView.ProcedureId).FirstOrDefault();
                if (procProcedure != null)
                {
                    returnView.ProcedureCode = procProcedure.Code;
                    returnView.ProcedureName = procProcedure.Name;
                }
                returnView.SFC = productParameterView.SFC;
                returnView.CirculationBarCode = productParameterView.CirculationBarCode;
                returnView.CreatedBy = productParameterView.CreatedBy;
                returnView.CreatedOn = productParameterView.CreatedOn;
                manuSfcCirculationReportExportDtos.Add(returnView);
            }
            #endregion

            #region 条码履历
            //查询条码所有步骤数据
            var manuSfcStepPagedQuery = planWorkOrderPagedQueryDto.ToQuery<ManuSfcStepPagedQuery>();
            manuSfcStepPagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var manuSfcStepPagedInfo = await _manuSfcStepRepository.GetPagedInfoAsync(manuSfcStepPagedQuery);
            //资源信息
            IEnumerable<ProcResourceEntity> manuSfcStepProcResources = new List<ProcResourceEntity>();
            var manuSfcStepProcResourcesIds = manuSfcStepPagedInfo.Data.Select(c => c.ResourceId ?? -1).ToArray();
            if (procResourcesIds.Any())
            {
                manuSfcStepProcResources = await _procResourceRepository.GetListByIdsAsync(manuSfcStepProcResourcesIds);
            }
            //工序信息
            IEnumerable<ProcProcedureEntity> manuSfcStepProcProcedures = new List<ProcProcedureEntity>();
            var manuSfcStepProcProcedureIds = manuSfcStepPagedInfo.Data.Select(c => c.ProcedureId ?? -1).ToArray();
            if (manuSfcStepProcProcedureIds.Any())
            {
                manuSfcStepProcProcedures = await _procProcedureRepository.GetByIdsAsync(manuSfcStepProcProcedureIds);
            }
            //设备信息
            IEnumerable<EquEquipmentEntity> manuSfcStepEquEquipments = new List<EquEquipmentEntity>();
            var manuSfcStepEquEquipmentIds = manuSfcStepPagedInfo.Data.Select(c => c.EquipmentId ?? -1).ToArray();
            if (manuSfcStepEquEquipmentIds.Any())
            {
                manuSfcStepEquEquipments = await _equipmentRepository.GetByIdsAsync(manuSfcStepEquEquipmentIds);
            }
            var productTracePagedReportExportDtos = new List<ProductTracePagedReportExportDto>();
            foreach (var manuSfcStep in manuSfcStepPagedInfo.Data)
            {
                ProductTracePagedReportExportDto returnView = new ProductTracePagedReportExportDto();
                var procProcedure = manuSfcStepProcProcedures.Where(c => c.Id == manuSfcStep.ProcedureId).FirstOrDefault();
                if (procProcedure != null)
                {
                    returnView.ProcedureCode = procProcedure.Code;
                    returnView.ProcedureName = procProcedure.Name;
                }
                //设备信息
                var equEquipment = manuSfcStepEquEquipments.Where(c => c.Id == manuSfcStep.EquipmentId).FirstOrDefault();
                if (equEquipment != null)
                {
                    returnView.EquipmentCode = equEquipment.EquipmentCode;
                    returnView.EquipmentName = equEquipment.EquipmentName;
                }
                //资源信息
                var procResource = manuSfcStepProcResources.Where(c => c.Id == manuSfcStep.ResourceId).FirstOrDefault();
                if (procResource != null)
                {
                    returnView.ResourceCode = procResource.ResCode;
                    returnView.ResourceName = procResource.ResName;
                }
                returnView.SFC = manuSfcStep.SFC;
                switch (manuSfcStep.Operatetype)
                {
                    case ManuSfcStepTypeEnum.InStock:
                        returnView.OperatetypeStr = "进站";
                        break;
                    case ManuSfcStepTypeEnum.OutStock:
                        returnView.OperatetypeStr = "出站";
                        break;
                    case ManuSfcStepTypeEnum.Change:
                        returnView.OperatetypeStr = "转换";
                        break;
                    case ManuSfcStepTypeEnum.Add:
                        returnView.OperatetypeStr = "组件添加";
                        break;
                    case ManuSfcStepTypeEnum.Assemble:
                        returnView.OperatetypeStr = "组装";
                        break;
                    default:
                        break;
                }
                returnView.CreatedOn = manuSfcStep.CreatedOn;
                productTracePagedReportExportDtos.Add(returnView);
            }
            #endregion

            var filePath = await _excelService.ExportAsync(planWorkOrderReportExportDtos, procSfcProcessRouteReports, equipmentPrameterReportExportDtos, productPrameterReportExportDtos, manuSfcCirculationReportExportDtos, productTracePagedReportExportDtos, fileName);
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new ExportResultDto
            {
                FileName = fileName,
                Path = uploadResult.AbsoluteUrl,
                RelativePath = uploadResult.RelativeUrl
            };
        }

    }
}
