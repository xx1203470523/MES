using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Report;
using Microsoft.AspNetCore.Http;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 标准参数表 service接口
    /// </summary>
    public interface IProcParameterService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procParameterPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcParameterDto>> GetPageListAsync(ProcParameterPagedQueryDto procParameterPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procParameterCreateDto"></param>
        /// <returns></returns>
        Task<long> CreateProcParameterAsync(ProcParameterCreateDto procParameterCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procParameterModifyDto"></param>
        /// <returns></returns>
        Task ModifyProcParameterAsync(ProcParameterModifyDto procParameterModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteProcParameterAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesProcParameterAsync(long[] idsArr);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcParameterDto> QueryProcParameterByIdAsync(long id);

        /// <summary>
        /// 导入参数表格
        /// </summary>
        /// <returns></returns>
        Task ImportParameterAsync(IFormFile formFile);

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task DownloadImportTemplateAsync(Stream stream);

        /// <summary>
        /// 根据查询条件导出标准参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ParameterExportResultDto> ExprotParameterListAsync(ProcParameterPagedQueryDto param);
    }
}
