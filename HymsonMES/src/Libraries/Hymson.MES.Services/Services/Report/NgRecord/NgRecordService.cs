using Hymson.Authentication;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Report;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.QualificationRateReport;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Minio;
using Hymson.Utils;
using OfficeOpenXml.ConditionalFormatting;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report;

/// <summary>
/// Ng记录报表服务层
/// </summary>
public class NgRecordService : INgRecordService
{

    #region 基础信息
    private readonly ICurrentUser _currentUser;
    private readonly IExcelService _excelService;
    private readonly IMinioService _minioService;
    private readonly IEquEquipmentRepository _equipmentRepository;
    private readonly IProcProcedureRepository _procProcedureRepository;
    private readonly IProcResourceRepository _procResourceRepository;
    private readonly IManuSfcStepNgRepository _manuSfcStepNgRepository;
    private readonly INgRecordReportRepository _ngRecordReportRepository;

    #endregion

    public NgRecordService(ICurrentUser currentUser,
        IExcelService excelService,
        IMinioService minioService,
        IEquEquipmentRepository equEquipmentRepository,
        IProcResourceRepository procResourceRepository,
        IProcProcedureRepository procProcedureRepository,
        INgRecordReportRepository ngRecordReportRepository)
    {
        _currentUser = currentUser;
        _excelService = excelService;
        _minioService = minioService;
        _equipmentRepository = equEquipmentRepository;
        _procResourceRepository = procResourceRepository;
        _procProcedureRepository = procProcedureRepository;
        _ngRecordReportRepository = ngRecordReportRepository;
    }

    /// <summary>
    /// 获取列表数据
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<IEnumerable<NgRecordReportDto>> GetListAsync(NgRecordReportQueryDto queryDto)
    {
        List<NgRecordReportDto> list = new();
        var query = queryDto.ToQuery<NgRecordReportQuery>();
        if (query.DateList?.Any() == true)
        {
            query.BeginTime = query.DateList[0];
            query.EndTime = query.DateList[1];
        }

        if (query.EquipmentCode?.Any() == true)
        {
            var equipmentEntity = await _equipmentRepository.GetByCodeAsync(new() { Code = query.EquipmentCode, Site = 123456 });
            if (equipmentEntity == null) return list;
            query.EquipmentId = equipmentEntity.Id;
        }

        if (query.ProcedureId != null)
        {
            var searchProcedureEntities = await _procProcedureRepository.GetByIdsAsync(query.ProcedureId.ToArray());
            if (searchProcedureEntities == null) return list;
            query.ProcedureId = searchProcedureEntities.Select(a => a.Id);
        }


        var ngRecordReportEntities = await _ngRecordReportRepository.GetJoinListAsync(query);

        var equipmentIds = ngRecordReportEntities.Select(a => a.EquipmentId.GetValueOrDefault());
        var equipmentEntities = await _equipmentRepository.GetByIdsAsync(equipmentIds.ToArray());

        var procedureIds = ngRecordReportEntities.Select(a => a.ProcedureId.GetValueOrDefault());
        var procedureEntities = await _procProcedureRepository.GetByIdsAsync(procedureIds.ToArray());

        var resourceIds = ngRecordReportEntities.Select(a => a.ResourceId.GetValueOrDefault());
        var ResourceEntities = await _procResourceRepository.GetByIdsAsync(new() { IdsArr = resourceIds.ToArray(),Status = 1 });

        List<NgRecordReportDto> result = new();
        foreach (var item in ngRecordReportEntities)
        {
            var equipment = equipmentEntities.FirstOrDefault(a => a.Id == item.EquipmentId);
            var procedure = procedureEntities.FirstOrDefault(a => a.Id == item.ProcedureId);
            var resource = ResourceEntities.FirstOrDefault(a => a.Id == item.ResourceId);

            NgRecordReportDto ngData = new NgRecordReportDto()
            {
                SFC = item?.SFC,
                EndTime = item?.EndTime,
                EquipmentCode = equipment?.EquipmentCode,
                EquipmentName = equipment?.EquipmentName,
                QualityStatus = item?.QualityStatus,
                ProcedureCode = procedure?.Code,
                ProcedureName = procedure?.Name,
                ResourceCode = resource?.ResCode,
                ResourceName = resource?.ResName,
            };

            result.Add(ngData);
        }


        return result;
    }

    /// <summary>
    /// 获取分页数据
    /// </summary>
    /// <param name="pageQueryDto"></param>
    /// <returns></returns>
    public async Task<PagedInfo<NgRecordReportDto>> GetPageInfoAsync(NgRecordReportPageQueryDto pageQueryDto)
    {
        PagedInfo<NgRecordReportDto> pagedInfo = new(new List<NgRecordReportDto>(), pageQueryDto.PageIndex, pageQueryDto.PageSize);
        var pageQuery = pageQueryDto.ToQuery<NgRecordReportPageQuery>();
        if (pageQuery.DateList?.Any()==true) {
            pageQuery.BeginTime = pageQuery.DateList[0];
            pageQuery.EndTime = pageQuery.DateList[1];
        }

        if (pageQuery.EquipmentCode?.Any() == true)
        {
            var equipmentEntity = await _equipmentRepository.GetByCodeAsync(new() { Code = pageQuery.EquipmentCode, Site = 123456 });
            if (equipmentEntity == null) return pagedInfo;
            pageQuery.EquipmentId = equipmentEntity.Id;
        }

        if (pageQuery.ProcedureId != null)
        {
            var searchProcedureEntities = await _procProcedureRepository.GetByIdsAsync(pageQuery.ProcedureId.ToArray());
            if (searchProcedureEntities == null) return pagedInfo;
            pageQuery.ProcedureId = searchProcedureEntities.Select(a=>a.Id);
        }


        var ngRecordReportEntities = await _ngRecordReportRepository.GetJoinPagedInfoAsync(pageQuery);

        var pageData = ngRecordReportEntities.Data;

        var equipmentIds = pageData.Select(a => a.EquipmentId.GetValueOrDefault());
        var equipmentEntities = await _equipmentRepository.GetByIdsAsync(equipmentIds.ToArray());

        var procedureIds = pageData.Select(a => a.ProcedureId.GetValueOrDefault());
        var procedureEntities = await _procProcedureRepository.GetByIdsAsync(procedureIds.ToArray());

        var resourceIds = pageData.Select(a => a.ResourceId.GetValueOrDefault());
        var ResourceEntities = await _procResourceRepository.GetByIdsAsync(new() { IdsArr = resourceIds.ToArray(), Status = 1 });

        List<NgRecordReportDto> result = new();
        foreach (var item in pageData)
        {
            var equipment = equipmentEntities.FirstOrDefault(a => a.Id == item.EquipmentId);
            var procedure = procedureEntities.FirstOrDefault(a => a.Id == item.ProcedureId);
            var resource = ResourceEntities.FirstOrDefault(a => a.Id == item.ResourceId);

            NgRecordReportDto ngData = new NgRecordReportDto()
            {
                SFC = item?.SFC,
                EndTime = item?.EndTime,
                BeginTime = item?.BeginTime,
                EquipmentCode = equipment?.EquipmentCode,
                EquipmentName = equipment?.EquipmentName,
                QualityStatus = item?.QualityStatus,
                ProcedureCode = procedure?.Code,
                ProcedureName = procedure?.Name,
                ResourceCode = resource?.ResCode,
                ResourceName = resource?.ResName,
            };

            result.Add(ngData);
        }

        pagedInfo.Data = result;
        pagedInfo.TotalCount = ngRecordReportEntities.TotalCount;

        return pagedInfo;
    }

    public async Task<ExportResultDto> ExportExcelAsync(NgRecordReportQueryDto queryDto)
    {
        string fileName = string.Format("({0})生产明细报表", DateTime.Now.ToString("yyyyMMddHHmmss"));

        var listData = await GetListAsync(queryDto);

        List<NgRecordReportExportDto> exportExcels = new();

        foreach (var item in listData)
        {
            NgRecordReportExportDto exportExcel = new()
            {
                SFC = item.SFC,
                CreatedOn = item?.EndTime,
                EquipmentCode = item?.EquipmentCode,
                EquipmentName = item?.EquipmentName,
                Passed = item?.QualityStatus,
                ProcedureCode = item?.ProcedureCode,
                ProcedureName = item?.ProcedureName,
                ResourceCode = item?.ResourceCode,
                ResourceName = item?.ResourceName
            };

            exportExcels.Add(exportExcel);
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
