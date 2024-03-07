using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.ManuEuqipmentNewestInfoEntity;
using Hymson.MES.Services.Dtos.ManuEuqipmentNewestInfo;

namespace Hymson.MES.Services.Services.ManuEuqipmentNewestInfo
{
    /// <summary>
    /// 服务接口（设备最新信息）
    /// </summary>
    public interface IManuEuqipmentNewestInfoService
    {
        /// <summary>
        /// 添加或者更新
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> AddOrUpdateAsync(ManuEuqipmentNewestInfoSaveDto saveDto);

        /// <summary>
        /// 根据设备ID查询实体
        /// </summary>
        /// <param name="equId"></param>
        /// <returns></returns>
        Task<ManuEuqipmentNewestInfoEntity?> QueryByEquIdAsync(long equId);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(ManuEuqipmentNewestInfoSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(ManuEuqipmentNewestInfoSaveDto saveDto);

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
        Task<ManuEuqipmentNewestInfoDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuEuqipmentNewestInfoDto>> GetPagedListAsync(ManuEuqipmentNewestInfoPagedQueryDto pagedQueryDto);

    }
}