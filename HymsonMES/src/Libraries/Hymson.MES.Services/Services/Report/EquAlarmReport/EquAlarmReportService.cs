using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Minio;

namespace Hymson.MES.Services.Services.Report.EquAlarmReport
{
    public class EquAlarmReportService : IEquAlarmReportService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        private readonly IEquAlarmRepository _equAlarmRepository;
        private readonly IMinioService _minioService;
        private readonly IExcelService _excelService;
        public EquAlarmReportService(IEquAlarmRepository equAlarmRepository, 
            ICurrentUser currentUser,
            ICurrentSite currentSite,
            IMinioService minioService, 
            IExcelService excelService)
        {
            _equAlarmRepository = equAlarmRepository;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _minioService = minioService;
            _excelService = excelService;
        }

        /// <summary>
        /// 根据查询条件获取设备报警报表分页数据
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquAlarmReportViewDto>> GetEquAlarmReportPageListAsync(EquAlarmReportPagedQueryDto pageQuery)
        {
            var pagedQuery = pageQuery.ToQuery<EquAlarmReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _equAlarmRepository.GetEquAlarmReportPageListAsync(pagedQuery);
            var dtos = pagedInfo.Data.Select(s =>
            {
                return s.ToModel<EquAlarmReportViewDto>();
            });
            return new PagedInfo<EquAlarmReportViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> EquAlarmReportExportAsync(EquAlarmReportPagedQueryDto pageQuery)
        {
            string fileName = "设备报警";
            pageQuery.PageSize = 10000;
            var pagedQuery = pageQuery.ToQuery<EquAlarmReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _equAlarmRepository.GetEquAlarmReportPageListAsync(pagedQuery);
            var equAlarmReports = pagedInfo.Data;
            var equHeartbeaReportExports = new List<EquAlarmReportExportDto>();
            foreach (var equAlarmReport in equAlarmReports)
            {
                equHeartbeaReportExports.Add(equAlarmReport.ToExcelModel<EquAlarmReportExportDto>());
            }
            var filePath = await _excelService.ExportAsync(equHeartbeaReportExports, fileName, fileName);
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new ExportResultDto
            {
                FileName = fileName,
                Path = uploadResult.RelativeUrl,
                RelativePath = uploadResult.RelativeUrl
            };
        }
    }
}
