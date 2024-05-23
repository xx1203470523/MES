using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment.EquMaintenance;

namespace Hymson.MES.Services.Services.Equipment.EquMaintenance.EquMaintenanceTask
{
    /// <summary>
    /// 服务接口（设备保养任务）
    /// </summary>
    public interface IEquMaintenanceTaskService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(EquMaintenanceTaskSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(EquMaintenanceTaskSaveDto saveDto);

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
        Task<EquMaintenanceTaskDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquMaintenanceTaskDto>> GetPagedListAsync(EquMaintenanceTaskPagedQueryDto pagedQueryDto);

    }
}