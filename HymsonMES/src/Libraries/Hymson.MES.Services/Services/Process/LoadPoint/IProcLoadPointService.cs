using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 上料点表 service接口
    /// </summary>
    public interface IProcLoadPointService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procLoadPointPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcLoadPointDto>> GetPageListAsync(ProcLoadPointPagedQueryDto procLoadPointPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLoadPointCreateDto"></param>
        /// <returns></returns>
        Task CreateProcLoadPointAsync(ProcLoadPointCreateDto procLoadPointCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procLoadPointModifyDto"></param>
        /// <returns></returns>
        Task ModifyProcLoadPointAsync(ProcLoadPointModifyDto procLoadPointModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteProcLoadPointAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesProcLoadPointAsync(long[] idsArr);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcLoadPointDetailDto> QueryProcLoadPointByIdAsync(long id);
    }
}
