using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Process.Procedure
{
    /// <summary>
    /// 转换系数表 service接口
    /// </summary>
    public interface IProcConversionFactorService
    {


        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procConversionFactorPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcConversionFactorViewDto>> GetPageListAsync(ProcConversionFactorPagedQueryDto procConversionFactorPagedQueryDto);

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
        Task<ProcConversionFactorDto> QueryProcLoadPointByIdAsync(long id);

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
        /// 新增
        /// </summary>
        /// <param name="procProcedureCreateDto"></param>
        /// <returns></returns>
        Task<long> AddProcProcedureAsync(AddConversionFactorDto procProcedureCreateDto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteProcProcedureAsync(long[] ids);

        /// <summary>
        /// 根据工序编码获取工序信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<ProcProcedureCodeDto> GetByCodeAsync(string code);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procLoadPointModifyDto"></param>
        /// <returns></returns>
        Task ModifyProcConversionFactorAsync(ProcConversionFactorModifyDto procConversionFactorModifyDto);


    }
}
