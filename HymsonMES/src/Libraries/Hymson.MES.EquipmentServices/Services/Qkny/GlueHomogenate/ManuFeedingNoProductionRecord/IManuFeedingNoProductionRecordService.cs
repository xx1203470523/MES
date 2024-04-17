using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.ManuFeedingNoProductionRecord;

namespace Hymson.MES.Services.Services.ManuFeedingNoProductionRecord
{
    /// <summary>
    /// 服务接口（设备投料非生产投料(洗罐子)）
    /// </summary>
    public interface IManuFeedingNoProductionRecordService
    {
        /// <summary>
        /// 添加多个
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> AddMultAsync(List<ManuFeedingNoProductionRecordSaveDto> saveDtoList);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(ManuFeedingNoProductionRecordSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(ManuFeedingNoProductionRecordSaveDto saveDto);

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
        Task<ManuFeedingNoProductionRecordDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuFeedingNoProductionRecordDto>> GetPagedListAsync(ManuFeedingNoProductionRecordPagedQueryDto pagedQueryDto);

    }
}