using Hymson.MES.CoreServices.Bos.Quality;

namespace Hymson.MES.CoreServices.Services.Quality
{
    /// <summary>
    /// 服务接口（AQL检验水平查询）
    /// </summary>
    public interface IAQLLevelQueryService
    {
        Task<IEnumerable<AQLLevelBo>> QueryListAsync(long siteId);
    }
}
