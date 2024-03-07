using Hymson.MES.CoreServices.Bos.Quality;

namespace Hymson.MES.CoreServices.Services.Quality
{
    public interface IAQLPlanQueryService
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<IEnumerable<AQLPlanBo>> QueryListAsync(long siteId);
    }
}
