using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务接口（自定义字段）
    /// </summary>
    public interface IInteCustomFieldService
    {
        /// <summary>
        /// 新增或更新
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> AddOrUpdateAsync(InteCustomFieldSaveDto saveDto);
    }
}