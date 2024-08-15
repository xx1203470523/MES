using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants.Report;
using Hymson.MES.Core.Domain.ManuEquipmentStatusTime;
using Hymson.MES.Data.Repositories.ManuEquipmentStatusTime;
using Hymson.MES.Data.Repositories.ManuEquipmentStatusTime.Query;
using Hymson.MES.Data.Repositories.ManuEuqipmentNewestInfo;
using Hymson.MES.Data.Repositories.ManuEuqipmentNewestInfo.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务（设备状态时间） 
    /// </summary>
    public class ManuEquipmentStatusTimeService : IManuEquipmentStatusTimeService
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
        /// 仓储接口（设备状态时间）
        /// </summary>
        private readonly IManuEquipmentStatusTimeRepository _manuEquipmentStatusTimeRepository;

        /// <summary>
        /// 设备最新状态
        /// </summary>
        private readonly IManuEuqipmentNewestInfoRepository _manuEuqipmentNewestInfoRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuEquipmentStatusTimeRepository"></param>
        /// <param name="manuEuqipmentNewestInfoRepository"></param>
        /// <param name="localizationService"></param>
        /// <param name="excelService"></param>
        /// <param name="minioService"></param>
        public ManuEquipmentStatusTimeService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuEquipmentStatusTimeRepository manuEquipmentStatusTimeRepository,
            IManuEuqipmentNewestInfoRepository manuEuqipmentNewestInfoRepository, 
            ILocalizationService localizationService,
            IExcelService excelService,
            IMinioService minioService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuEquipmentStatusTimeRepository = manuEquipmentStatusTimeRepository;
            _manuEuqipmentNewestInfoRepository = manuEuqipmentNewestInfoRepository;

            _localizationService = localizationService;
            _excelService = excelService;
            _minioService = minioService;
        }

        /// <summary>
        /// 设备状态监控报表分页查询
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuEquipmentStatusReportViewDto>> GetPageListAsync(ManuEquipmentStatusTimePagedQueryDto pagedQueryDto)
        {
            var pageQuery = new ManuEquipmentStatusTimePagedQuery()
            {
                WorkCenterId = pagedQueryDto.WorkCenterId,
                EquipmentId = pagedQueryDto.EquipmentId,
                SiteId = _currentSite.SiteId ?? 0,
                PageIndex = pagedQueryDto.PageIndex,
                PageSize = pagedQueryDto.PageSize,
            };
            var pagedInfo = await _manuEquipmentStatusTimeRepository.GetPagedListAsync(pageQuery);
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ManuEquipmentStatusReportViewDto>());
            return new PagedInfo<ManuEquipmentStatusReportViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 设备状态监控报表分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ManuEquipmentStatusExportResultDto> ExprotListAsync(ManuEquipmentStatusTimePagedQueryDto param)
        {
            var pageQuery = new ManuEquipmentStatusTimePagedQuery()
            {
                WorkCenterId = param.WorkCenterId,
                EquipmentId = param.EquipmentId,
                SiteId = _currentSite.SiteId ?? 0,
                PageIndex = param.PageIndex,
                PageSize = ReportExport.PageSize
            };
            var pagedInfo = await _manuEquipmentStatusTimeRepository.GetPagedListAsync(pageQuery);
            List<ManuEquipmentStatusRExportDto> listDto = new List<ManuEquipmentStatusRExportDto>();
            if (pagedInfo == null || pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ManuEquipmentStatusExport"), _localizationService.GetResource("ManuEquipmentStatusExport"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new ManuEquipmentStatusExportResultDto
                {
                    FileName = _localizationService.GetResource("ManuEquipmentStatusExport"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }

            foreach (var item in pagedInfo.Data)
            {
                listDto.Add(new ManuEquipmentStatusRExportDto
                {
                    WorkCenterCode = item.WorkCenterCode,
                    WorkCenterName= item.WorkCenterName,
                    ProcedureCode= item.ProcedureCode,
                    ProcedureName = item.ProcedureName,
                    EquipmentCode = item.EquipmentCode,
                    EquipmentName = item.EquipmentName,
                    CurrentStatus=item.CurrentStatus.GetDescription(),
                    StatusDuration = item.StatusDuration,
                    BeginTime = item.BeginTime,
                    EndTime = item.EndTime,
                    UpdatedOn=item.UpdatedOn
                });
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ManuEquipmentStatusExport"), _localizationService.GetResource("ManuEquipmentStatusExport"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new ManuEquipmentStatusExportResultDto
            {
                FileName = _localizationService.GetResource("ManuEquipmentStatusExport"),
                Path = uploadResult.AbsoluteUrl,
            };
        }
    }
}
