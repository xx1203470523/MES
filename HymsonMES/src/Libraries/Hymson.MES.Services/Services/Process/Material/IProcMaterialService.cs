using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 物料维护 service接口
    /// </summary>
    public interface IProcMaterialService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procMaterialPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcMaterialDto>> GetPageListAsync(ProcMaterialPagedQueryDto procMaterialPagedQueryDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procMaterialPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcMaterialDto>> GetPageListForGroupAsync(ProcMaterialPagedQueryDto procMaterialPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procMaterialCreateDto"></param>
        /// <returns></returns>
        Task CreateProcMaterialAsync(ProcMaterialCreateDto procMaterialCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procMaterialModifyDto"></param>
        /// <returns></returns>
        Task ModifyProcMaterialAsync(ProcMaterialModifyDto procMaterialModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteProcMaterialAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task DeletesProcMaterialAsync(long[] idsArr);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcMaterialViewDto> QueryProcMaterialByIdAsync(long id);

        /// <summary>
        /// 根据物料ID查询对应的关联供应商
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<ProcMaterialSupplierViewDto>> QueryProcMaterialSupplierByMaterialIdAsync(long id);

        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateStatusAsync(ChangeStatusDto param);
    }
}
