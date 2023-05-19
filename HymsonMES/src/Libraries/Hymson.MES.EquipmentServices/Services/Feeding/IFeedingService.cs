using Hymson.MES.EquipmentServices.Dtos.Feeding;

namespace Hymson.MES.EquipmentServices.Services.Feeding
{
    /// <summary>
    /// 上卸料服务接口
    /// @author Czhipu
    /// @date 2023-05-16 04:51:15
    /// </summary>
    public interface IFeedingService
    {
        /// <summary>
        /// 上料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task FeedingLoadingAsync(FeedingLoadingDto request);

        /// <summary>
        /// 卸料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task FeedingUnloadingAsync(FeedingUnloadingDto request);

    }
}
