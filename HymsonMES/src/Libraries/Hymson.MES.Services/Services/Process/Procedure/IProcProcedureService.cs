using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
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
        /// 分页查询工艺路线的工序列表
        /// </summary>
        /// <param name="procProcedurePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProcedureDto>> GetPagedInfoByProcessRouteIdAsync(ProcProcedurePagedQueryDto procProcedurePagedQueryDto);

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
        /// 获取资质认证设置
        /// </summary>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcQualificationAuthenticationDto>> GetProcedureAuthSetListAsync(long procedureId);

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

        /// <summary>
        /// 根据工序编码获取工序信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<ProcProcedureCodeDto> GetByCodeAsync(string code);

        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateStatusAsync(ChangeStatusDto param);

        Task CreateProductParameterAsync();

    }
}
