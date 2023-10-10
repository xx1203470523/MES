using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.SystemServices.Dtos.ProductTraceReport;
using Hymson.MES.SystemServices.Dtos.ProductTraceReport.query;
namespace Hymson.MES.SystemServices.Services.ProductTrace;

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

    public ProductTraceReportService(ICurrentSite currentSite,
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

}
