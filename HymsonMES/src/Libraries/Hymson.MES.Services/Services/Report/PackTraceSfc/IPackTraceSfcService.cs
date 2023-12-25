
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Dtos.Report.PackTraceSfcDto;

namespace Hymson.MES.Services.Services.Report.PackTraceSfc;

/// <summary>
/// Pack追溯电芯码报表
/// </summary>
public interface IPackTraceSfcService
{

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<IEnumerable<PackTraceSfcViewDto>> GetListAsync(PackTraceSfcQueryDto queryDto);

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<PagedInfo<PackTraceSfcViewDto>> GetPageInfoAsync(PackTraceSfcPageQueryDto queryDto);

    #region Excel导出

    /// <summary>
    /// Excel导出
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<ExportResultDto> ExportExcelAsyc(PackTraceSfcQueryDto queryDto);

    #endregion
}