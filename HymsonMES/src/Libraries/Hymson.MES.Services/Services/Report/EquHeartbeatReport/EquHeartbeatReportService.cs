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

namespace Hymson.MES.Services.Services.Report.EquHeartbeatReport
{
    public class EquHeartbeatReportService : IEquHeartbeatReportService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        private readonly IEquHeartbeatRepository _equHeartbeatRepository;
        private readonly IMinioService _minioService;
        private readonly IExcelService _excelService;
        public EquHeartbeatReportService(IEquHeartbeatRepository equHeartbeatRepository,
            ICurrentUser currentUser,
            ICurrentSite currentSite,
            IMinioService minioService,
            IExcelService excelService)
        {
            _equHeartbeatRepository = equHeartbeatRepository;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _minioService = minioService;
            _excelService = excelService;
        }

        /// <summary>
        /// 根据查询条件获取设备心跳状态报表分页数据
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquHeartbeatReportViewDto>> GetEquHeartbeatReportPageListAsync(EquHeartbeatReportPagedQueryDto pageQuery)
        {
            var pagedQuery = pageQuery.ToQuery<EquHeartbeatReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _equHeartbeatRepository.GetEquHeartbeatReportPageListAsync(pagedQuery);
            var dtos = pagedInfo.Data.Select(s =>
            {
                return s.ToModel<EquHeartbeatReportViewDto>();
            });
            return new PagedInfo<EquHeartbeatReportViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 导出设备心跳
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> EquHeartbeatReportExportAsync(EquHeartbeatReportPagedQueryDto pageQuery)
        {
            string fileName = "设备心跳";
            pageQuery.PageSize = 10000;
            var pagedQuery = pageQuery.ToQuery<EquHeartbeatReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _equHeartbeatRepository.GetEquHeartbeatReportPageListAsync(pagedQuery);
            var equHeartbeatReports = pagedInfo.Data;
            var equHeartbeaExports = new List<EquHeartbeaReportExportDto>();
            foreach (var equHeartbeatReport in equHeartbeatReports)
            {
                equHeartbeaExports.Add(equHeartbeatReport.ToExcelModel<EquHeartbeaReportExportDto>());
            }
            var filePath = await _excelService.ExportAsync(equHeartbeaExports, fileName, fileName);
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
