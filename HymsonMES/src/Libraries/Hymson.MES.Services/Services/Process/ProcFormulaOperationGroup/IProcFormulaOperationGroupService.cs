using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 服务接口（配方操作组）
    /// </summary>
    public interface IProcFormulaOperationGroupService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        Task CreateProcFormulaOperationGroupAsync(AddFormulaOperationGroupDto addDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        Task ModifyProcFormulaOperationGroupAsync(AddFormulaOperationGroupDto addDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteProcFormulaOperationGroupAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesProcFormulaOperationGroupAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcFormulaOperationGroupDto?> QueryProcFormulaOperationGroupByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcFormulaOperationGroupDto>> GetPagedListAsync(ProcFormulaOperationGroupPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 获取配方操作
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcFormulaOperationDto>> GetFormulaOperationListAsync(OperationGroupGetOperationPagedQueryDto pagedQueryDto);

    }
}