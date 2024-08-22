using Hymson.Infrastructure;
using Hymson.Logging;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Services.Integrated.InteIntefaceLog
{
    public interface IInteIntefaceLogService
    {
        Task<PagedInfo<HitTraceLogDto>> GetPagedListAsync(InteIntefaceLogPagedQueryDto pagedQueryDto);

        /// <summary>
        ///根据主键获取document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="indexName">索引名称</param>
        /// <returns></returns>
        Task<TraceLogEntry> QueryByIdAsync(string id,string indexName);


    }
}
