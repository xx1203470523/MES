using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Report;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Minio;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 服务（设备开机参数报表） 
    /// </summary>
    public class EquStartupParameterReportService : IEquStartupParameterReportService
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 工作中心仓储
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 工序仓储
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（设备过程参数报表）
        /// </summary>

        private readonly IManuEquipmentParameterRepository _equipmentParameterRepository;

        private readonly IEquOpenParamRecordRepository _paramRecordRepository;

        private readonly IEquEquipmentRepository _equipmentRepository;

        private readonly IProcParameterRepository _parameterRepository;
        private readonly IProcResourceRepository _resourceRepository;
        private readonly IProcResourceEquipmentBindRepository _equipmentBindRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquStartupParameterReportService(ICurrentUser currentUser, ICurrentSite currentSite,
               IInteWorkCenterRepository inteWorkCenterRepository,
               IProcProcedureRepository procProcedureRepository,
               IManuEquipmentParameterRepository equipmentParameterRepository,
               IEquOpenParamRecordRepository paramRecordRepository,
               IEquEquipmentRepository equipmentRepository,
               IProcParameterRepository parameterRepository,
               IProcResourceRepository resourceRepository,
               IProcResourceEquipmentBindRepository equipmentBindRepository,
               ILocalizationService localizationService,
               IExcelService excelService,
               IMinioService minioService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procProcedureRepository = procProcedureRepository;
            _equipmentParameterRepository = equipmentParameterRepository;
            _paramRecordRepository = paramRecordRepository;
            _equipmentRepository = equipmentRepository;
            _parameterRepository = parameterRepository;
            _resourceRepository = resourceRepository;
            _equipmentBindRepository = equipmentBindRepository;

            _localizationService = localizationService;
            _excelService = excelService;
            _minioService = minioService;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquStartupParameterReportDto>> GetPagedListAsync(EquStartupParameterReportPagedQueryDto pagedQueryDto)
        {
            //选择时间与设备编号显示数据
            if (pagedQueryDto.CreatedOn == null || pagedQueryDto.CreatedOn.Length < 2)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES14953));
            }
            if (pagedQueryDto.EquipmentId <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12601));
            }

            var siteId = _currentSite.SiteId ?? 0;
            //获取设备的参数信息
            var parameterQuery = new EquOpenParamRecordPagedQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                CreatedOn = pagedQueryDto.CreatedOn,
                ParameterId = pagedQueryDto.ParameterId,
                EquipmentId = pagedQueryDto.EquipmentId,
                BatchId = pagedQueryDto.BatchId,
                PageIndex = pagedQueryDto.PageIndex,
                PageSize = pagedQueryDto.PageSize
            };
            var pagedInfo = await _paramRecordRepository.GetPagedListAsync(parameterQuery);
            var listDto = new List<EquStartupParameterReportDto>();
            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return new PagedInfo<EquStartupParameterReportDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }

            var equipmentId = pagedQueryDto.EquipmentId??0;
            var parameterIds = pagedInfo.Data.Select(x => x.ParamId.GetValueOrDefault()).Distinct().ToList();

            //设备
            var equipmentEntity = await _equipmentRepository.GetByIdAsync(equipmentId);

            //参数
            var parameterEntities = await _parameterRepository.GetByIdsAsync(parameterIds);

            //资源
            var resourceEntity = new ProcResourceEntity();
            var workCenterEntity = new InteWorkCenterEntity();
            IEnumerable<ProcProcedureEntity> procedureEntities = new List<ProcProcedureEntity>();
            var equipmentBindEntities = await _equipmentBindRepository.GetByResourceIdAsync(new ProcResourceEquipmentBindQuery
            {
                Ids = new long[] { equipmentId }
            });
            var resourceIds = equipmentBindEntities.Select(x => x.ResourceId).Distinct().ToList();
            if (resourceIds.Any())
            {
                var resourceId = resourceIds[0];
                resourceEntity = await _resourceRepository.GetResByIdAsync(resourceId);
                if (resourceEntity != null)
                {
                    //工作中心
                    workCenterEntity = await _inteWorkCenterRepository.GetByResourceIdAsync(resourceId);

                    //工序
                    procedureEntities = await _procProcedureRepository.GetProcProcedureByResourceTypeIdAsync(resourceEntity.ResTypeId);
                }
            }

            foreach (var item in pagedInfo.Data)
            {
                var resTypeId = resourceEntity?.ResTypeId ?? 0;
                var procedureEntity = procedureEntities.FirstOrDefault(y => y.ResourceTypeId == resTypeId);
                var parameterEntity = parameterEntities.FirstOrDefault(x => x.Id == item.ParamId);
                var equParameterDto = new EquStartupParameterReportDto
                {
                    WorkCenterName = workCenterEntity?.Name ?? "",
                    ProcedureCode = procedureEntity?.Code ?? "",
                    ProcedureName = procedureEntity?.Name ?? "",
                    ResCode = resourceEntity?.ResCode ?? "",
                    ResName = resourceEntity?.ResName ?? "",
                    EquipmentCode = equipmentEntity?.EquipmentCode ?? "",
                    EquipmentName = equipmentEntity?.EquipmentName ?? "",
                    ParameterCode = parameterEntity?.ParameterCode ?? "",
                    ParameterName = parameterEntity?.ParameterName ?? "",
                    ParameterUnit = parameterEntity?.ParameterUnit ?? "",
                    ParameterValue = item.ParamValue,
                    CollectionTime = item.CollectionTime,
                    CreatedOn = item.CreatedOn,
                    BatchId = item.BatchId
                };

                listDto.Add(equParameterDto);
            }
            return new PagedInfo<EquStartupParameterReportDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 导出查询数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<EquStartupParameterExportResultDto> ExprotListAsync(EquStartupParameterReportPagedQueryDto param)
        {
            //选择时间与设备编号显示数据
            if (param.CreatedOn == null || param.CreatedOn.Length < 2)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES14953));
            }
            if (param.EquipmentId <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12601));
            }

            var siteId = _currentSite.SiteId ?? 0;
            //获取设备的参数信息
            var parameterQuery = new EquOpenParamRecordPagedQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                CreatedOn = param.CreatedOn,
                ParameterId = param.ParameterId,
                EquipmentId = param.EquipmentId,
                BatchId = param.BatchId,
                PageIndex = param.PageIndex,
                PageSize = ReportExport.PageSize
            };
            var pagedInfo = await _paramRecordRepository.GetPagedListAsync(parameterQuery);
            List<EquStartupParameterExportDto> listDto = new List<EquStartupParameterExportDto>();
            if (pagedInfo == null || pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("EquStartupParameterReport"), _localizationService.GetResource("EquStartupParameterReport"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new EquStartupParameterExportResultDto
                {
                    FileName = _localizationService.GetResource("EquStartupParameterReport"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }

            var equipmentId = param.EquipmentId ?? 0;
            var parameterIds = pagedInfo.Data.Select(x => x.ParamId.GetValueOrDefault()).Distinct().ToList();

            //设备
            var equipmentEntity = await _equipmentRepository.GetByIdAsync(equipmentId);

            //参数
            var parameterEntities = await _parameterRepository.GetByIdsAsync(parameterIds);

            //资源
            var resourceEntity = new ProcResourceEntity();
            var workCenterEntity = new InteWorkCenterEntity();
            IEnumerable<ProcProcedureEntity> procedureEntities = new List<ProcProcedureEntity>();
            var equipmentBindEntities = await _equipmentBindRepository.GetByResourceIdAsync(new ProcResourceEquipmentBindQuery
            {
                Ids = new long[] { equipmentId }
            });
            var resourceIds = equipmentBindEntities.Select(x => x.ResourceId).Distinct().ToList();
            if (resourceIds.Any())
            {
                var resourceId = resourceIds[0];
                resourceEntity = await _resourceRepository.GetResByIdAsync(resourceId);
                if (resourceEntity != null)
                {
                    //工作中心
                    workCenterEntity = await _inteWorkCenterRepository.GetByResourceIdAsync(resourceId);

                    //工序
                    procedureEntities = await _procProcedureRepository.GetProcProcedureByResourceTypeIdAsync(resourceEntity.ResTypeId);
                }
            }

            foreach (var item in pagedInfo.Data)
            {
                var resTypeId = resourceEntity?.ResTypeId ?? 0;
                var procedureEntity = procedureEntities.FirstOrDefault(y => y.ResourceTypeId == resTypeId);
                var parameterEntity = parameterEntities.FirstOrDefault(x => x.Id == item.ParamId);
                var equParameterDto = new EquStartupParameterExportDto
                {
                    WorkCenterName = workCenterEntity?.Name ?? "",
                    ProcedureCode = procedureEntity?.Code ?? "",
                    ProcedureName = procedureEntity?.Name ?? "",
                    ResCode = resourceEntity?.ResCode ?? "",
                    ResName = resourceEntity?.ResName ?? "",
                    EquipmentCode = equipmentEntity?.EquipmentCode ?? "",
                    EquipmentName = equipmentEntity?.EquipmentName ?? "",
                    ParameterCode = parameterEntity?.ParameterCode ?? "",
                    ParameterName = parameterEntity?.ParameterName ?? "",
                    ParameterUnit = parameterEntity?.ParameterUnit ?? "",
                    ParameterValue = item.ParamValue,
                    CollectionTime = item.CollectionTime,
                    CreatedOn = item.CreatedOn,
                    BatchId = item.BatchId
                };

                listDto.Add(equParameterDto);
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("EquStartupParameterReport"), _localizationService.GetResource("EquStartupParameterReport"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new EquStartupParameterExportResultDto
            {
                FileName = _localizationService.GetResource("EquStartupParameterReport"),
                Path = uploadResult.AbsoluteUrl,
            };
        }

    }
}
