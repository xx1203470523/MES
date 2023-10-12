using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Process.Procedure
{
    /// <summary>
    /// 工序表 service接口
    /// </summary>
    public interface IProcProcedureService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procProcedurePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProcedureViewDto>> GetPageListAsync(ProcProcedurePagedQueryDto procProcedurePagedQueryDto);

        /// <summary>
        /// 获取所有工序-无参
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureViewPDADto>> GetProcProcedurePDAAsync();
        
        /// <summary>
        /// 按编码获取工序信息
        /// </summary>
        /// <param name="procedureCode"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureViewPDADto>> GetProcProcedureByCodeAsync(string[] procedureCode);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QueryProcProcedureDto> GetProcProcedureByIdAsync(long id);

        /// <summary>
        /// 获取工序配置打印信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProcedurePrintReleationDto>> GetProcedureConfigPrintListAsync(ProcProcedurePrintReleationPagedQueryDto queryDto);

        /// <summary>
        /// 获取工序配置Job信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcedureJobReleationDto>> GetProcedureConfigJobListAsync(InteJobBusinessRelationPagedQueryDto queryDto);

        /// <summary>
        /// 获取工序产出设置
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProductSetDto>> GetProcedureProductSetListAsync(ProcProductSetQueryDto queryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProcedureCreateDto"></param>
        /// <returns></returns>
        Task AddProcProcedureAsync(AddProcProcedureDto procProcedureCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procProcedureModifyDto"></param>
        /// <returns></returns>
        Task UpdateProcProcedureAsync(UpdateProcProcedureDto procProcedureModifyDto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsAr"></param>
        /// <returns></returns>
        Task<int> DeleteProcProcedureAsync(long[] idsAr);
    }
}
