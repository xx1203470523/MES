using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 服务接口（配方操作）
    /// </summary>
    public interface IProcFormulaOperationService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        Task CreateProcFormulaOperationAsync(AddFormulaOperationDto addDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        Task ModifyProcFormulaOperationAsync(AddFormulaOperationDto addDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteProcFormulaOperationAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesProcFormulaOperationAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcFormulaOperationDto?> QueryProcFormulaOperationByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcFormulaOperationDto>> GetPagedListAsync(ProcFormulaOperationPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 获取配方操作设置值
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcFormulaOperationSetDto>> GetFormulaOperationConfigSetListAsync(ProcFormulaOperationSetPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateStatusAsync(ChangeStatusDto param);
    }
}