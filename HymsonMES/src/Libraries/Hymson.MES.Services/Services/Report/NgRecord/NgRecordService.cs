using Hymson.Authentication;
using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Report;
using OfficeOpenXml.ConditionalFormatting;
using System;
using System.Collections.Generic;
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
    private readonly IEquEquipmentRepository _equipmentRepository;
    private readonly IProcProcedureRepository _procProcedureRepository;
    private readonly IProcResourceRepository _procResourceRepository;
    private readonly IManuSfcStepNgRepository _manuSfcStepNgRepository;

    #endregion

    public NgRecordService(ICurrentUser currentUser,
        IEquEquipmentRepository equEquipmentRepository,
        IProcResourceRepository procResourceRepository,
        IProcProcedureRepository procProcedureRepository)
    {
        _currentUser = currentUser;
        _equipmentRepository = equEquipmentRepository;
        _procResourceRepository = procResourceRepository;
        _procProcedureRepository = procProcedureRepository;
    }

    public async Task<IEnumerable<NgRecordReportDto>> GetListAsync(NgRecordReportQueryDto queryDto)
    {
        List<NgRecordReportDto> list = new();


        await Task.CompletedTask;
        return list;
    }

    public async Task<PagedInfo<NgRecordReportDto>> GetPageInfoAsync(NgRecordReportPageQueryDto pageQueryDto)
    {
        PagedInfo<NgRecordReportDto> pagedInfo = new(new List<NgRecordReportDto>(),pageQueryDto.PageIndex,pageQueryDto.PageSize);

        var manuSfcStepNgEntities = await _manuSfcStepNgRepository.GetPagedInfoAsync(new() { 
            PageIndex = pageQueryDto.PageIndex,
            PageSize = pageQueryDto.PageSize,  
        });


        return pagedInfo;
    }

    public async Task<IEnumerable<NgRecordReportExportDto>> ExportExcelAsync(NgRecordReportQueryDto queryDto)
    {

        List<NgRecordReportExportDto> list = new();


        await Task.CompletedTask;
        return list;
    }
}
