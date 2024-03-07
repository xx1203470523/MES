using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.ManuEquipmentStatusTime;

namespace Hymson.MES.Services.Services.ManuEquipmentStatusTime
{
    /// <summary>
    /// 服务接口（设备状态时间）
    /// </summary>
    public interface IManuEquipmentStatusTimeService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> AddAsync(ManuEquipmentStatusTimeSaveDto saveDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(ManuEquipmentStatusTimeSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(ManuEquipmentStatusTimeSaveDto saveDto);

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
        Task<ManuEquipmentStatusTimeDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuEquipmentStatusTimeDto>> GetPagedListAsync(ManuEquipmentStatusTimePagedQueryDto pagedQueryDto);

    }
}