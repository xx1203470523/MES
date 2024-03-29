using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture.ManuFeeding
{
    /// <summary>
    /// 服务接口（物料加载）
    /// </summary>
    public interface IManuFeedingService
    {
        /// <summary>
        /// 查询资源（物料加载）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<SelectOptionDto>> GetFeedingResourceListAsync(ManuFeedingResourceQueryDto queryDto);

        /// <summary>
        /// 查询上料点（物料加载）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<SelectOptionDto>> GetFeedingLoadPointListAsync(ManuFeedingLoadPointQueryDto queryDto);

        /// <summary>
        /// 查询工单（物料加载）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<SelectOptionDto>> GetFeedingWorkOrderListAsync(ManuFeedingWorkOrderQueryDto queryDto);

        /// <summary>
        /// 查询物料（物料加载）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFeedingMaterialDto>> GetFeedingMaterialListAsync(ManuFeedingMaterialQueryDto queryDto);

        /// <summary>
        /// 添加（物料加载）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<ManuFeedingMaterialResponseDto> CreateAsync(ManuFeedingMaterialSaveDto saveDto);

        /// <summary>
        /// 删除（物料加载）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

    }
}
