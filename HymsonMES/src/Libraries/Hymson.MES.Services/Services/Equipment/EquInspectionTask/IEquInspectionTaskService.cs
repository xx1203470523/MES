using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务接口（点检任务）
    /// </summary>
    public interface IEquInspectionTaskService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<long> CreateAsync(EquInspectionTaskSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<long> ModifyAsync(EquInspectionTaskSaveDto saveDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquInspectionTaskDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquInspectionTaskDto>> GetPagedListAsync(EquInspectionTaskPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 查询点检任务详情
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task<IEnumerable<EquInspectionTaskDetailDto>> QueryItemsByTaskIdAsync(long taskId);

        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateStatusAsync(ChangeStatusDto param);

        /// <summary>
        /// 生成录入任务
        /// </summary>
        /// <param name="recordDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        Task<long> GeneratedTaskRecordAsync(GenerateInspectionRecordDto recordDto);
    }
}