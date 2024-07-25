using Hymson.MES.SystemServices.Dtos;

namespace Hymson.MES.SystemServices.Services.Quality
{
    /// <summary>
    /// 服务接口（NIO）
    /// </summary>
    public interface INIOService
    {
        /// <summary>
        /// 队列（添加）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        Task<int> AddQueueAsync(IEnumerable<NIOAddDto> requestDtos);

    }
}
