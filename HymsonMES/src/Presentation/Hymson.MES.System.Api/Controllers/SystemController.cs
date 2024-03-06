
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Dtos.SystemApi;
using Hymson.MES.Services.Dtos.SystemApi.Kanban;
using Hymson.MES.Services.Services.Report;
using Hymson.MES.Services.Services.SystemApi;
using Hymson.MES.SystemServices.Dtos.Manufacture;
using Hymson.MES.SystemServices.Dtos.Plan;
using Hymson.MES.SystemServices.Services.Manufacture;
using Hymson.MES.SystemServices.Services.Plan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.System.Api.Controllers
{
    /// <summary>
    /// ϵͳ�Խ�
    /// </summary>
    [Route("SystemService/api/v1/SystemApi")]
    [ApiController]
    public class SystemController : Controller
    {
        private readonly ILogger<SystemController> _logger;
        private readonly IPlanWorkOrderService _planWorkOrderService;
        private readonly IManuSfcCirculationService _manuSfcCirculationService;
        private readonly IProductTraceReportService _productTraceReportService;
        private readonly ISystemApiService _systemApiService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="planWorkOrderService"></param>
        /// <param name="manuSfcCirculationService"></param>
        /// <param name="productTraceReportService"></param>
        /// <param name="systemApiService"></param>
        public SystemController(ILogger<SystemController> logger,
            IPlanWorkOrderService planWorkOrderService,
            IManuSfcCirculationService manuSfcCirculationService,
            IProductTraceReportService productTraceReportService,
            ISystemApiService systemApiService)
        {
            _logger = logger;
            _planWorkOrderService = planWorkOrderService;
            _manuSfcCirculationService = manuSfcCirculationService;
            _productTraceReportService = productTraceReportService;
            _systemApiService = systemApiService;
        }

        [HttpGet]
        public async Task GetToken()
        {

        }

        /// <summary>
        /// ����ͬ��
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("workorderadd")]
        public async Task AddWorkOrderAsync(PlanWorkOrderDto request)
        {
            await _planWorkOrderService.AddWorkOrderAsync(request);
        }

        /// <summary>
        /// ����Ʒ����/ģ�������ѯ�����㼶��Ϣ
        /// </summary>
        /// <param name="manuSfcCirculationQueryDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("relationship")]
        public async Task<ManuSfcCirculationDto?> GetRelationShipByPackAsync(ManuSfcCirculationQueryDto manuSfcCirculationQueryDto)
        {
            return await _manuSfcCirculationService.GetRelationShipByPackAsync(manuSfcCirculationQueryDto.SFC);
        }

        /// <summary>
        /// ����Ʒ����/ģ�����뷴��㼶��Ϣ
        /// </summary>
        /// <param name="manuSfcTraceQueryDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("producttrace")]
        public async Task<IEnumerable<ManuSfcTracedDto>> GetProductTracePagedListAsync(ManuSfcTraceQueryDto manuSfcTraceQueryDto)
        {
            if (string.IsNullOrEmpty(manuSfcTraceQueryDto.SFC)) { throw new CustomerValidationException(nameof(ErrorCode.MES19203)); }
            ProductTracePagedQueryDto productTracePagedQueryDto = new ProductTracePagedQueryDto()
            {
                SFC = manuSfcTraceQueryDto.SFC,
                TraceDirection = false,
                PageSize = 2000,
                PageIndex = 1
            };
            var manuSfcCirculationList = await _productTraceReportService.GetProductTracePagedListAsync(productTracePagedQueryDto);
            List<ManuSfcTracedDto> manuSfcTracedDtoList = new List<ManuSfcTracedDto>();
            foreach (var manuSfcCirculationViewDto in manuSfcCirculationList.Data)
            {
                ManuSfcTracedDto manuSfcTracedDto = new()
                {
                    DeviceCode = manuSfcCirculationViewDto.EquipentCode,
                    CodeId = manuSfcCirculationViewDto.SFC,
                    CirculationBarCode = manuSfcCirculationViewDto.CirculationBarCode
                };
                manuSfcTracedDtoList.Add(manuSfcTracedDto);
            }
            return manuSfcTracedDtoList;
        }

        /// <summary>
        /// ����Ʒ����/ģ�������ѯ������Ϣ
        /// </summary>
        /// <param name="manuSfcPrameterQueryDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("productprameter")]
        public async Task<IEnumerable<ManuSfcPrameterDto>> GetProductPrameterPagedListAsync(ManuSfcPrameterQueryDto manuSfcPrameterQueryDto)
        {
            if (string.IsNullOrEmpty(manuSfcPrameterQueryDto.SFC)) { throw new CustomerValidationException(nameof(ErrorCode.MES19203)); }
            ManuProductPrameterPagedQueryDto param = new ManuProductPrameterPagedQueryDto()
            {
                SFC = manuSfcPrameterQueryDto.SFC,
                ParameterType = ParameterTypeEnum.Product,
                PageSize = 2000,
                PageIndex = 1
            };
            var manuProductParameterViewDtoList = await _productTraceReportService.GetProductPrameterPagedListAsync(param);

            List<ManuSfcPrameterDto> manuSfcPrameterDtoList = new List<ManuSfcPrameterDto>();
            foreach (var ManuSfcCirculationViewDto in manuProductParameterViewDtoList.Data)
            {
                ManuSfcPrameterDto manuSfcTracedDto = new()
                {
                    ParameterName = ManuSfcCirculationViewDto.ParameterName,
                    ParameterCode = ManuSfcCirculationViewDto.ParameterCode,
                    StandardUpperLimit = ManuSfcCirculationViewDto.StandardUpperLimit,
                    StandardLowerLimit = ManuSfcCirculationViewDto.StandardLowerLimit,
                    ProcedureCode = ManuSfcCirculationViewDto.ProcedureCode,
                    ProcedureName = ManuSfcCirculationViewDto.ProcedureName,
                    LocalTime = ManuSfcCirculationViewDto.LocalTime,
                    EquipmentName = ManuSfcCirculationViewDto.EquipmentName,
                };
                manuSfcPrameterDtoList.Add(manuSfcTracedDto);
            }
            return manuSfcPrameterDtoList;
        }

        /// <summary>
        /// ����Ʒ����/ģ�������ѯ��վ��Ϣ
        /// </summary>
        /// <param name="manuSfcStepQueryDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("sfcstep")]
        public async Task<IEnumerable<ManuSfcStepDto>> GetSfcStepPagedListAsync(ManuSfcStepQueryDto manuSfcStepQueryDto)
        {
            if (string.IsNullOrEmpty(manuSfcStepQueryDto.SFC)) { throw new CustomerValidationException(nameof(ErrorCode.MES19203)); }
            ManuSfcStepPagedQueryDto param = new ManuSfcStepPagedQueryDto()
            {
                SFC = manuSfcStepQueryDto.SFC,
                PageSize = 2000,
                PageIndex = 1
            };
            var manuSfcStepViewDtoList = await _productTraceReportService.GetSfcStepPagedListAsync(param);

            List<ManuSfcStepDto> manuSfcPrameterDtoList = new List<ManuSfcStepDto>();
            foreach (var ManuSfcCirculationViewDto in manuSfcStepViewDtoList.Data)
            {
                ManuSfcStepDto manuSfcTracedDto = new()
                {
                    EquipmentName = ManuSfcCirculationViewDto.EquipmentName,
                    WorkOrderType = ManuSfcCirculationViewDto.WorkOrderType,
                    ProductName = ManuSfcCirculationViewDto.ProductName,
                    CreatedOn = ManuSfcCirculationViewDto.CreatedOn,
                    ProcedureCode = ManuSfcCirculationViewDto.ProcedureCode,
                    ProcedureName = ManuSfcCirculationViewDto.ProcedureName,
                    ResourceName = ManuSfcCirculationViewDto.ResourceName,
                    ProcedureType = 0,
                    Passed = ManuSfcCirculationViewDto.Passed
                };
                manuSfcPrameterDtoList.Add(manuSfcTracedDto);
            }

            return manuSfcPrameterDtoList;
        }

        #region ����


        /// <summary>
        /// ��ҳ-����������Ϣ
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("kanban/planWorkOrderInfo")]
        public async Task<IEnumerable<PlanWorkOrderInfoViewDto>> GetPlanWorkOrderInfoAsync([FromQuery] PlanWorkOrderInfoQueryDto queryDto)
        {
            return await _systemApiService.GetPlanWorkOrderInfoAsync(queryDto);
        }

        /// <summary>
        /// ��ҳ-OEE����ͼ
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("kanban/oeeTrendChart")]
        public async Task<IEnumerable<OEETrendChartViewDto>> GetOEETrendChartAsync([FromQuery] OEETrendChartQueryDto queryDto)
        {
            return await _systemApiService.GetOEETrendChartAsync(queryDto);
        }

        /// <summary>
        /// ����һ�κϸ��ʣ�����/Һ�䣩
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("kanban/oneQualifiedRate")]
        public async Task<OneQualifiedViewDto> GetOneQualifiedRateAsync([FromQuery] OneQualifiedQueryDto queryDto)
        {
            return await _systemApiService.GetOneQualifiedRateAsync(queryDto);
        }

        /// <summary>
        /// ���»�ȡһ�κϸ��ʣ�����/Һ�䣩
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("kanban/monthOneQualifiedRate")]
        public async Task<IEnumerable<OneQualifiedMonthViewDto>> GetMonthOneQualifiedRateAsync([FromQuery] OneQualifiedMonthQueryDto queryDto)
        {
            return await _systemApiService.GetMonthOneQualifiedRateAsync(queryDto);
        }

        /// <summary>
        /// ��о��ģ�飬Pack��ȡ�����ֲ�����/�£�����/Һ�䣩
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("kanban/defectDistribution")]
        public async Task<IEnumerable<DefectDistributionViewDto>> GetDefectDistributionAsync([FromQuery] DefectDistributionQueryDto queryDto)
        {
            return await _systemApiService.GetDefectDistributionAsync(queryDto);
        }

        /// <summary>
        /// �����ղ�������ͼ
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("kanban/procedureDayOutput")]
        public async Task<IEnumerable<ProcedureDayOutputViewDto>> GetProcedureDayOutputAsync([FromQuery] ProcedureDayOutputQueryDto queryDto)
        {
            return await _systemApiService.GetProcedureDayOutputAsync(queryDto);
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("kanban/productionCapacity")]
        public async Task<IEnumerable<ProductionCapacityViewDto>> GetProductionCapacityAsync([FromQuery] ProductionCapacityQueryDto queryDto)
        {
            return await _systemApiService.GetProductionCapacityAsync(queryDto);
        }

        /// <summary>
        /// ��ȡ�豸����״̬
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("kanban/equStatus")]
        public async Task<IEnumerable<EquipmentStatusViewDto>> GetEquipmentStatusAsync()
        {
            return await _systemApiService.GetEquipmentStatusAsync();
        }

        /// <summary>
        /// �豸����״̬�ֲ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("kanban/equStatusDistribution")]
        public async Task<IEnumerable<EquStatusDistributionViewDto>> GetEquipmentStatusDistributionAsync()
        {
            return await _systemApiService.GetEquipmentStatusDistributionAsync();
        }

        /// <summary>
        /// ��ȡ�豸�����ʣ���/�£�
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("kanban/equFaultRate")]
        public async Task<IEnumerable<EquFaultRateViewDto>> GetEquFaultRateAsync([FromQuery] EquFaultRateQueryDto queryDto)
        {
            return await _systemApiService.GetEquFaultRateAsync(queryDto);
        }

        /// <summary>
        /// OEE����ͼ����/�£�
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("kanban/equOEETrendChart")]
        public async Task<IEnumerable<EquOEETrendChartViewDto>> GetEquOEETrendChartAsync([FromQuery] EquOEETrendChartQueryDto queryDto)
        {
            return await _systemApiService.GetEquOEETrendChartAsync(queryDto);
        }

        #endregion
    }
}