using Hymson.MES.CoreServices.Bos.Quality;

namespace Hymson.MES.CoreServices.Services.Quality
{
    public interface IEnvOrderCreateService
    {
        /// <summary>
        /// 手动生成
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<int> ManualCreateAsync(EnvOrderManualCreateBo bo);

        /// <summary>
        /// 自动生成(指定站点)
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<int> AutoCreateAsync(long siteId);

        /// <summary>
        /// 自动生成(所有站点)
        /// </summary>
        /// <returns></returns>
        Task<int> AutoCreateAsync();
    }
}
