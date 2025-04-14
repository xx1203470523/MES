using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Minio;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
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

        /// <summary>
        /// 条码绑定关系参数查询
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<SfcBindParameterReport2PagedDataDto>> GetPagedData2Async(SfcBindParameterReport2PagedQueryDto queryDto)
        {
            if (queryDto.OrderCode.IsNullOrEmpty()) throw new CustomerDataException(nameof(ErrorCode.MES19179));

            var pageData = await _manuSfcCirculationRepository.GetSfcBindParameter2PagedDataAsync(new()
            {
                OrderCode = queryDto.OrderCode,
                PageIndex = 1,
                PageSize = 99999,
            });

            //性能太差

            List<SfcBindParameterReport2PagedDataDto> list = new();

            //// 预转换数据结构（比直接使用IEnumerable更高效）
            var pageDataList = pageData.Data.Select(a => a.ToModel<SfcBindParameterReport2PagedDataDto>()).ToList();

            // 使用字典建立快速索引（时间复杂度从O(n)降到O(1)）
            var packLookup = pageDataList
                .Where(a => string.IsNullOrEmpty(a.CirculationBarCode))
                .ToLookup(a => a.SFC);
            var moduleLookup = pageDataList
                .Where(a => !string.IsNullOrEmpty(a.CirculationBarCode) && a.CirculationBarCode.StartsWith("ES"))
                .ToLookup(a => a.CirculationBarCode);
            var cellLookup = pageDataList
                .Where(a => !string.IsNullOrEmpty(a.CirculationBarCode) && a.CirculationBarCode.StartsWith("YT") && a.SFC.StartsWith("0"))
                .ToLookup(a => a.CirculationBarCode);

            // 批量处理代替逐条处理
            foreach (var item in packLookup.Select(a => a.Key))
            {
                // 添加Pack数据
                list.AddRange(packLookup[item]);

                // 通过索引快速获取关联模块
                if (moduleLookup.Contains(item))
                {
                    var moduleList = moduleLookup[item].ToList();
                    list.AddRange(moduleList);

                    // 通过索引获取关联Cell
                    var moduleSfcs = moduleList.Select(m => m.SFC).ToHashSet();

                    foreach (var moduleSfc in moduleSfcs)
                    {
                        list.AddRange(cellLookup[moduleSfc]);
                    }

                }
            }

            // 内存分页优化（使用Skip/Take需确保有序）
            var pagedData = list
                .Skip((queryDto.PageIndex - 1) * queryDto.PageSize)
                .Take(queryDto.PageSize)
                .ToList();

            //var pagedData = pageData.Data.Select(a => a.ToModel<SfcBindParameterReport2PagedDataDto>())
            //    .Skip((queryDto.PageIndex - 1) * queryDto.PageSize)
            //    .Take(queryDto.PageSize)
            //    .ToList();

            return new PagedInfo<SfcBindParameterReport2PagedDataDto>(
                pagedData,
                queryDto.PageIndex,
                queryDto.PageSize,
                pageData.TotalCount);
        }

        /// <summary>
        /// 条码绑定关系参数导出
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> ReportExport2Async(SfcBindParameterReport2PagedQueryDto queryDto)
        {
            var fileName = $"条码绑定关系参数{DateTime.Now.ToString("yyyyMMddHHmmss")}";

            var exportData = await _manuSfcCirculationRepository.GetSfcBindParameter2ExportAsync(new() { OrderCode = queryDto.OrderCode });

            var exportExcels = new List<SfcBindParameterReport2ExportDto>();
            var list = new List<SfcBindParameterReport2ExportDto>();


            foreach (var item in exportData)
            {
                exportExcels.Add(new()
                {
                    SFC = item.SFC,
                    CirculationBarCode = item.CirculationBarCode,
                    ProcedureCode = item.ProcedureCode,
                    ProcedureName = item.ProcedureName,
                    ParameterCode = item.ParameterCode,
                    ParameterName = item.ParameterName,
                    ParameterValue = item.ParameterValue,
                    CreatedBy = item.CreatedBy,
                    CreatedOn = item.CreatedOn,
                });
            }

            // 使用字典建立快速索引（时间复杂度从O(n)降到O(1)）
            var packLookup = exportExcels
                .Where(a => string.IsNullOrEmpty(a.CirculationBarCode))
                .ToLookup(a => a.SFC);
            var moduleLookup = exportExcels
                .Where(a => !string.IsNullOrEmpty(a.CirculationBarCode) && a.CirculationBarCode.StartsWith("ES"))
                .ToLookup(a => a.CirculationBarCode);
            var cellLookup = exportExcels
                .Where(a => !string.IsNullOrEmpty(a.CirculationBarCode) && a.CirculationBarCode.StartsWith("YT"))
                .ToLookup(a => a.CirculationBarCode);

            // 批量处理代替逐条处理
            foreach (var item in packLookup.Select(a=>a.Key))
            {
                // 添加Pack数据
                list.AddRange(packLookup[item]);

                // 通过索引快速获取关联模块
                if (moduleLookup.Contains(item))
                {
                    var moduleList = moduleLookup[item].ToList();
                    list.AddRange(moduleList);

                    // 通过索引获取关联Cell
                    var moduleSfcs = moduleList.Select(m => m.SFC).ToHashSet();

                    foreach (var moduleSfc in moduleSfcs)
                    {
                        list.AddRange(cellLookup[moduleSfc]);
                    }

                    //var cellList = exportExcels
                    //    .Where(a => a.CirculationBarCode != null
                    //        && moduleSfcs.Contains(a.CirculationBarCode))
                    //    .ToList();
                }
            }


            var filePath = await _excelService.ExportAsync(list, fileName);
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
