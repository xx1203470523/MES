using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services.Report.PalletUnbindingRecord
{
    public class PalletUnbindingRecordService : IPalletUnbindingRecordService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        private readonly IInteVehicleFreightRecordRepository _inteVehicleFreightRecordRepository;
        /// <summary>
        /// 工序表 仓储
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IEquEquipmentRepository _equipmentRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IInteVehicleRepository _inteVehicleRepository;
        private readonly IInteVehicleFreightRepository _inteVehicleFreightRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        public PalletUnbindingRecordService(IProcResourceRepository procResourceRepository, IInteVehicleFreightRepository inteVehicleFreightRepository, IInteVehicleRepository inteVehicleRepository, IInteWorkCenterRepository inteWorkCenterRepository, IPlanWorkOrderRepository planWorkOrderRepository, IEquEquipmentRepository equEquipmentRepository, IProcProcedureRepository procProcedureRepository, ICurrentUser currentUser, ICurrentSite currentSite, IInteVehicleFreightRecordRepository inteVehicleFreightRecordRepository)
        {
            _procResourceRepository = procResourceRepository;
            _inteVehicleFreightRepository = inteVehicleFreightRepository;
            _inteVehicleRepository = inteVehicleRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _equipmentRepository = equEquipmentRepository;
            _procProcedureRepository = procProcedureRepository;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _inteVehicleFreightRecordRepository = inteVehicleFreightRecordRepository;
        }

        /// <summary>
        /// 根据查询报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<VehicleFreightRecordDto>> GetVehicleFreightRecorPageListAsync(VehicleFreightRecordQueryDto param)
        {
            var pagedQuery = param.ToQuery<InteVehicleFreightRecordPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _inteVehicleFreightRecordRepository.GetPagedInfoAsync(pagedQuery);
            List<VehicleFreightRecordDto> listDto = new List<VehicleFreightRecordDto>();
            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return new PagedInfo<VehicleFreightRecordDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }
            var pageInfoData = pagedInfo.Data;
            var procedureIds = pageInfoData.Select(x => x.ProcedureId).Distinct().ToList();
            var procProceduresList = await _procProcedureRepository.GetByIdsAsync(procedureIds);
            var equipmentIds = pageInfoData.Select(x => x.EquipmentId).Distinct().ToList();
            var equipmentList = await _equipmentRepository.GetByIdAsync(equipmentIds);
            var workCenterIds = pageInfoData.Select(x => x.WorkCenterId).Distinct().ToArray();
            var workCenterList = await _inteWorkCenterRepository.GetByIdsAsync(workCenterIds);
            var workOrderIds = pageInfoData.Select(x => x.WorkOrderId).Distinct().ToArray();
            var workOrderList = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);
            var vehicleIds = pageInfoData.Select(x => x.VehicleId).Distinct().ToArray();
            var vehicleList = await _inteVehicleRepository.GetByIdsAsync(vehicleIds);
            var locationIds = pageInfoData.Select(x => x.LocationId).Distinct().ToArray();
            var locationList = await _inteVehicleFreightRepository.GetByIdsAsync(locationIds);
            var resourceIds = pageInfoData.Select(x => x.ResourceId).Distinct().ToArray();
            var resourceList = await _procResourceRepository.GetListByIdsAsync(resourceIds);

            foreach (var pageInfo in pageInfoData)
            {
                var proceduresInfo = procProceduresList.FirstOrDefault(item => item.Id == pageInfo.ProcedureId);
                var equipmentInfo = equipmentList.FirstOrDefault(item => item.Id == pageInfo.EquipmentId);
                var workCenterInfo = workCenterList.FirstOrDefault(item => item.Id == pageInfo.WorkCenterId);
                var workOrderInfo = workOrderList.FirstOrDefault(item => item.Id == pageInfo.WorkOrderId);
                var vehicleInfo = vehicleList.FirstOrDefault(item => item.Id == pageInfo.VehicleId);
                var locatioInfo = locationList.FirstOrDefault(item => item.Id == pageInfo.LocationId);
                var resourceInfo = resourceList.FirstOrDefault(item => item.Id == pageInfo.ResourceId);
                VehicleFreightRecordDto vehicleFreightRecord = new VehicleFreightRecordDto();
                vehicleFreightRecord.ProcedureCode = proceduresInfo?.Code ?? "";
                vehicleFreightRecord.ProcedureName = proceduresInfo?.Name ?? "";
                vehicleFreightRecord.EquipmentCode = equipmentInfo?.EquipmentCode ?? "";
                vehicleFreightRecord.WorkCenterCode = workCenterInfo?.Code ?? "";
                vehicleFreightRecord.OrderCode = workOrderInfo?.OrderCode ?? "";
                vehicleFreightRecord.VehicleCode = vehicleInfo?.Code ?? "";
                vehicleFreightRecord.BarCode = pageInfo.BarCode;
                vehicleFreightRecord.Location = locatioInfo?.Location ?? "";
                vehicleFreightRecord.ResourceCode = resourceInfo?.ResCode ?? "";
                if (pageInfo.OperateType == (int)VehicleOperationEnum.Bind)
                {
                    vehicleFreightRecord.OperateType = (int)VehicleOperationEnum.Bind;
                }
                if (pageInfo.OperateType == (int)VehicleOperationEnum.Unbind)
                {
                    vehicleFreightRecord.OperateType = (int)VehicleOperationEnum.Unbind;
                }
                vehicleFreightRecord.BindAndUnbindTime = pageInfo.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");
                vehicleFreightRecord.CreateBy = pageInfo.CreatedBy;
                listDto.Add(vehicleFreightRecord);
            }
            return new PagedInfo<VehicleFreightRecordDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
    }
}