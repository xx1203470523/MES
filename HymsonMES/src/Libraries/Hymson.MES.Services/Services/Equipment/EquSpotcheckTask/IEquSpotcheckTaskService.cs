using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务接口（点检任务）
    /// </summary>
    public interface IEquSpotcheckTaskService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(EquSpotcheckTaskSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(EquSpotcheckTaskSaveDto saveDto);

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
        Task<EquSpotcheckTaskDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSpotcheckTaskDto>> GetPagedListAsync(EquSpotcheckTaskPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 查询点检单明细项数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<TaskItemUnionSnapshotView>> querySnapshotItemAsync(SpotcheckTaskSnapshotItemQueryDto pagedQueryDto);
        
        /// <summary>
        /// 保存点检明细项
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<int> SaveAndUpdateTaskItemAsync(SpotcheckTaskItemSaveDto pagedQueryDto);

        /// <summary>
        /// 完成检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<int> CompleteOrderAsync(SpotcheckTaskCompleteDto requestDto);

        /// <summary>
        /// 结果处理 关闭检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<int> CloseOrderAsync(SpotcheckTaskCloseDto requestDto);

    }
}