using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Process.Procedure
{
    /// <summary>
    /// 打印设置 service接口
    /// </summary>
    public interface IProcPrintSetupService
    {


        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procConversionFactorPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcPrintSetupViewDto>> GetPageListAsync(ProcPrintSetupPagedQueryDto procConversionFactorPagedQueryDto);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcPrintSetupDto> QueryProcPrintSetupByIdAsync(long id);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        Task<ProcPrintSetupDto> QueryProcPrintSetupByMaterialIdAsync(long materialId);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procPrintSetupCreateDto"></param>
        /// <returns></returns>
        Task<long> AddProcPrintSetupAsync(AddPrintSetupDto procPrintSetupCreateDto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteProcPrintSetupAsync(long[] ids);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procPrintSetupModifyDto"></param>
        /// <returns></returns>
        Task ModifyProcPrintSetupAsync(ProcPrintSetupModifyDto procPrintSetupModifyDto);


    }
}
