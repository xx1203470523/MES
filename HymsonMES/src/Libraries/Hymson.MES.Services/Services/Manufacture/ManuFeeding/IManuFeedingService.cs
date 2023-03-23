using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture.ManuFeeding
{
    /// <summary>
    /// 服务接口（容器维护）
    /// </summary>
    public interface IManuFeedingService
    {
        /// <summary>
        /// 查询类型（物料加载）
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<FeedingSourceEnum> GetFeedingSourceAsync(string code);

        /// <summary>
        /// 查询物料（物料加载）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFeedingMaterialDto>> GetFeedingListeAsync(ManuFeedingMaterialQueryDto queryDto);

        /*
        /// <summary>
        /// 添加（容器维护）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(InteContainerSaveDto createDto);

        /// <summary>
        /// 更新（容器维护）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(InteContainerSaveDto modifyDto);

        /// <summary>
        /// 删除（容器维护）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 获取分页数据（容器维护）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<InteContainerDto>> GetPagedListAsync(InteContainerPagedQueryDto pagedQueryDto);
        */

    }
}
