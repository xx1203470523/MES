using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants.Process;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.ProcessRoute.Query;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Utils;
using IdGen;
using System.Drawing.Printing;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 产品追溯报表服务
    /// </summary>
    public class ProductTraceReportService : IProductTraceReportService
    {
        #region 依赖注入
        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;
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

        public ProductTraceReportService(ICurrentSite currentSite,
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
         IManuSfcRepository manuSfcRepository)
        {
            _currentSite = currentSite;
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
            productTraceReportPagedQuery.SiteId = _currentSite.SiteId ?? 0;
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
            manuProductParameterPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            //参数分页查询
            var pagedInfo = await _manuProductParameterRepository.GetManuProductParameterPagedInfoAsync(manuProductParameterPagedQuery);
            //如果查询为产品参数
            if (manuProductParameterPagedQuery.ParameterType == ParameterTypeEnum.Product)
            {
                pagedInfo.Data = pagedInfo.Data.Where(c => c.ParameterType == null || c.ParameterType == ParameterTypeEnum.Product);
            }
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
            manuSfcStepPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuSfcStepRepository.GetPagedInfoAsync(manuSfcStepPagedQuery);
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
            //查询条码信息
            var manuSfcEntity = await _manuSfcRepository.GetBySFCAsync(new GetBySFCQuery { SFC = procSfcProcessRoutePagedQueryDto.SFC, SiteId = _currentSite.SiteId });
            if (manuSfcEntity == null)
            {
                return new PagedInfo<ProcSfcProcessRouteViewDto>(procSfcProcessRouteViewDtos, procSfcProcessRoutePagedQueryDto.PageIndex, procSfcProcessRoutePagedQueryDto.PageSize, 0);
            }
            var sfcinfo = await _manuSfcInfoRepository.GetBySFCAsync(manuSfcEntity?.Id ?? 0);
            if (sfcinfo == null)
            {
                return new PagedInfo<ProcSfcProcessRouteViewDto>(procSfcProcessRouteViewDtos, procSfcProcessRoutePagedQueryDto.PageIndex, procSfcProcessRoutePagedQueryDto.PageSize, 0);
            }
            //条码对应工单信息
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(sfcinfo.WorkOrderId);
            //工艺路线明细
            procProcessRouteDetailNodeQuery.ProcessRouteId = planWorkOrderEntity.ProcessRouteId;
            var pagedInfo = await _procProcessRouteDetailNodeRepository.GetPagedInfoAsync(procProcessRouteDetailNodeQuery);
            //过滤结束工序并按序号排序
            pagedInfo.Data = pagedInfo.Data.Where(c => !string.IsNullOrEmpty(c.Code)).OrderBy(x => x.SerialNo.ParseToInt());
            //查询条码步骤
            var manuSfcStepEntities = await _manuSfcStepRepository.GetManuSfcStepEntitiesAsync(new ManuSfcStepQuery { SFC = manuSfcEntity.SFC, SiteId = _currentSite.SiteId ?? 0 });
            procSfcProcessRouteViewDtos = pagedInfo.Data.Select(c =>
            {
                return GetProcessRouteDetailStep(c, manuSfcStepEntities);
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
                Qty = qty
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
            var manuSfcEntity = await _manuSfcRepository.GetBySFCAsync(new GetBySFCQuery { SFC = planWorkOrderPagedQueryDto.SFC, SiteId = _currentSite.SiteId });
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
    }
}
