using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Minio;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report.SfcBindParameter
{
    public class SfcBindParameterService : ISfcBindParameterService
    {
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;

        public SfcBindParameterService(IManuSfcCirculationRepository manuSfcCirculationRepository, IExcelService excelService, IMinioService minioService)
        {
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _excelService = excelService;
            _minioService = minioService;
        }

        /// <summary>
        /// 条码绑定关系扭矩参数查询
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerDataException"></exception>
        public async Task<PagedInfo<SfcBindParameterReportPagedDataDto>> GetPagedDataAsync(SfcBindParameterReportPagedQueryDto queryDto)
        {
            if (queryDto.OrderCode.IsNullOrEmpty()) throw new CustomerDataException(nameof(ErrorCode.MES19179));

            var pageData = await _manuSfcCirculationRepository.GetSfcBindParameterPagedDataAsync(new()
            {
                OrderCode = queryDto.OrderCode,
                PageIndex = queryDto.PageIndex,
                PageSize = queryDto.PageSize,
            });

            var list = pageData.Data.Select(a => a.ToModel<SfcBindParameterReportPagedDataDto>());

            var result = new PagedInfo<SfcBindParameterReportPagedDataDto>(list, queryDto.PageIndex, queryDto.PageSize, pageData.TotalCount);
            return result;
        }

        /// <summary>
        /// 条码绑定关系扭矩参数导出
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> ReportExportAsync(SfcBindParameterReportPagedQueryDto queryDto)
        {
            var fileName = $"条码绑定关系扭矩参数{DateTime.Now.ToString("yyyyMMddHHmmss")}";

            var exportData = await _manuSfcCirculationRepository.GetSfcBindParameterExportAsync(new() { OrderCode = queryDto.OrderCode });

            var exportExcels = new List<SfcBindParameterReportExportDto>();

            foreach (var item in exportData)
            {
                exportExcels.Add(new()
                {
                    Sfc = item.Sfc,
                    CirculationBarcode = item.CirculationBarcode,
                    Column1 = item.Column1,
                    Column2 = item.Column2,
                    Column3 = item.Column3,
                    Column4 = item.Column4,
                    Column5 = item.Column5,
                    Column6 = item.Column6,
                    Column7 = item.Column7,
                    Column8 = item.Column8,
                    OrderCode = item.OrderCode,
                    CreatedBy = item.CreatedBy,
                    CreatedOn = item.CreatedOn,
                    ProcedureCode = item.ProcedureCode,
                    ProcedureName = item.ProcedureName,
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName
                });
            }

            var filePath = await _excelService.ExportAsync(exportExcels, fileName);
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
