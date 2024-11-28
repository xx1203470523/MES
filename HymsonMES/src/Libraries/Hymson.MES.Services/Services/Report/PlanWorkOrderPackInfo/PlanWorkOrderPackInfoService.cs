using Hymson.Excel;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Data.Repositories.Report;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Dtos.Report.Excel;
using Hymson.Minio;
using Microsoft.Extensions.Caching.Memory;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report;

/// <summary>
/// 工单Pack数据查询接口
/// </summary>
public class PlanWorkOrderPackInfoService : IPlanWorkOrderPackInfoService
{
    private readonly IPlanWorkOrderPackInfoRepository _planWorkOrderPackInfoRepository;

    private readonly IExcelService _excelService;

    private readonly IMinioService _minioService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public PlanWorkOrderPackInfoService(IExcelService excelService,
        IMinioService minioService,
        IPlanWorkOrderPackInfoRepository planWorkOrderPackInfoRepository)
    {
        _excelService = excelService;
        _minioService = minioService;
        _planWorkOrderPackInfoRepository = planWorkOrderPackInfoRepository;
    }

    public async Task<PagedInfo<PackTraceOutputDto>> GetTraceListAsync(PackTraceQueryDto queryDto)
    {
        var result = new PagedInfo<PackTraceOutputDto>(new List<PackTraceOutputDto>(), queryDto.PageIndex, queryDto.PageSize);

        var query = queryDto.ToQuery<PackTraceQuery>();
        var pageData = await _planWorkOrderPackInfoRepository.GetTraceListAsync(query);
        result.TotalCount = pageData.Count();

        var data = pageData.ToList().Skip(query.PageSize * (query.PageIndex - 1)).Take(query.PageSize).ToList();

        result.Data = data.Select(a => a.ToModel<PackTraceOutputDto>());

        return result;
    }

    public async Task<PagedInfo<PackTestOutputDto>> GetTestListAsync(PackTestQueryDto queryDto)
    {
        var result = new PagedInfo<PackTestOutputDto>(new List<PackTestOutputDto>(), queryDto.PageIndex, queryDto.PageSize);

        var query = queryDto.ToQuery<PackTestQuery>();
        var pageData = await _planWorkOrderPackInfoRepository.GetTestListAsync(query);
        result.TotalCount = pageData.Count();

        var data = pageData.ToList().Skip(query.PageSize * (query.PageIndex - 1)).Take(query.PageSize).ToList();

        result.Data = data.Select(a => a.ToModel<PackTestOutputDto>());

        return result;
    }


    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<ExportResultDto> ExportExcelAsync(PackTraceQueryDto queryDto)
    {
        string fileName = string.Format("({0})工单Pack数据", DateTime.Now.ToString("yyyyMMddHHmmss"));

        if (!string.IsNullOrEmpty(queryDto.WorkOrderCode)) fileName = queryDto.WorkOrderCode;
        if (!string.IsNullOrEmpty(queryDto.SFC)) fileName = queryDto.SFC;

        var packTraceData = await _planWorkOrderPackInfoRepository.GetTraceListAsync(new() { SFC = queryDto.SFC, WorkOrderCode = queryDto.WorkOrderCode });
        var packTestData = await _planWorkOrderPackInfoRepository.GetTestListAsync(new() { SFC = queryDto.SFC, WorkOrderCode = queryDto.WorkOrderCode });

        List<PackTraceExcelDto> exportExcels1 = new();
        List<PackTestExcelDto> exportExcels2 = new();

        foreach (var item in packTraceData)
        {
            exportExcels1.Add(item.ToExcelModel<PackTraceExcelDto>());
        }

        foreach (var item in packTestData)
        {
            exportExcels2.Add(item.ToExcelModel<PackTestExcelDto>());
        }

        var filePath = await _excelService.ExportAsync(exportExcels1,exportExcels2, new List<Null3ExcelDto>(), new List<Null4ExcelDto>(), new List<Null5ExcelDto>(), new List<Null6ExcelDto>(), fileName);
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