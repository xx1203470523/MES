using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Minio;

namespace Hymson.MES.Services.Services.Report.ManuProductParameterReport
{
    internal class ManuProductParameterReportService : IManuProductParameterReportService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        private readonly IManuProductParameterRepository _manuProductParameterRepository;
        private readonly IMinioService _minioService;
        private readonly IExcelService _excelService;

        public ManuProductParameterReportService(ICurrentUser currentUser,
            ICurrentSite currentSite,
            IManuProductParameterRepository manuProductParameterRepository,
            IMinioService minioService,
            IExcelService excelService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuProductParameterRepository = manuProductParameterRepository;
            _minioService = minioService;
            _excelService = excelService;
        }
        /// <summary>
        /// 产品参数报表查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuProductParameterReportViewDto>> GetManuProductParameterReportPageListAsync(ManuProductParameterReportPagedQueryDto pageQuery)
        {
            //注释SFC非必填
            //if ((pageQuery.SFCS == null || pageQuery.SFCS.Length <= 0) && string.IsNullOrEmpty(pageQuery.SFCStr))
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES10111));
            //}
            if (!string.IsNullOrEmpty(pageQuery.SFCStr))
            {
                pageQuery.SFCStr = pageQuery.SFCStr.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
            }
            var pagedQuery = pageQuery.ToQuery<ManuProductParameterReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _manuProductParameterRepository.GetManuProductParameterReportPagedInfoAsync(pagedQuery);
            var dtos = pagedInfo.Data.Select(s =>
            {
                return s.ToModel<ManuProductParameterReportViewDto>();
            });
            return new PagedInfo<ManuProductParameterReportViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 产品参数导出
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> ManuProductParameterReportExportAsync(ManuProductParameterReportPagedQueryDto pageQuery)
        {
            //if ((pageQuery.SFCS == null || pageQuery.SFCS.Length <= 0) && string.IsNullOrEmpty(pageQuery.SFCStr))
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES10111));
            //}
            if (!string.IsNullOrEmpty(pageQuery.SFCStr))
            {
                pageQuery.SFCStr = pageQuery.SFCStr.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
            }
            string fileName = "产品参数";
            pageQuery.PageSize = 1000000;
            var pagedQuery = pageQuery.ToQuery<ManuProductParameterReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _manuProductParameterRepository.GetManuProductParameterReportPagedInfoAsync(pagedQuery);
            var manuProductParameterReports = pagedInfo.Data;
            var manuProductParameterReportExports = new List<ManuProductParameterReportExportDto>();
            foreach (var manuProductParameterReport in manuProductParameterReports)
            {
                manuProductParameterReportExports.Add(manuProductParameterReport.ToExcelModel<ManuProductParameterReportExportDto>());
            }
            var filePath = await _excelService.ExportAsync(manuProductParameterReportExports, fileName, fileName);
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
