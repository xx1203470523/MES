using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.ManuFeedingCompletedZjyjRecord;

namespace Hymson.MES.Services.Services.ManuFeedingCompletedZjyjRecord
{
    /// <summary>
    /// 服务接口（manu_feeding_completed_zjyj_record）
    /// </summary>
    public interface IManuFeedingCompletedZjyjRecordService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> AddAsync(ManuFeedingCompletedZjyjRecordSaveDto saveDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(ManuFeedingCompletedZjyjRecordSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(ManuFeedingCompletedZjyjRecordSaveDto saveDto);

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
        Task<ManuFeedingCompletedZjyjRecordDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuFeedingCompletedZjyjRecordDto>> GetPagedListAsync(ManuFeedingCompletedZjyjRecordPagedQueryDto pagedQueryDto);

    }
}