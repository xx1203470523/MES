/*
 *creator: Karl
 *
 *describe: 工序表    服务接口 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-13 09:06:05
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Process.IProcessService
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
        Task<PagedInfo<QueryProcProcedurePrintReleationDto>> GetProcedureConfigPrintListAsync(ProcProcedurePrintReleationPagedQueryDto queryDto);

        /// <summary>
        /// 获取工序配置Job信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QueryProcedureJobReleationDto>> GetProcedureConfigJobListAsync(InteJobBusinessRelationPagedQueryDto queryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProcedureCreateDto"></param>
        /// <returns></returns>
        Task CreateProcProcedureAsync(AddProcProcedureDto procProcedureCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procProcedureModifyDto"></param>
        /// <returns></returns>
        Task ModifyProcProcedureAsync(UpdateProcProcedureDto procProcedureModifyDto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteProcProcedureAsync(string ids);
    }
}
