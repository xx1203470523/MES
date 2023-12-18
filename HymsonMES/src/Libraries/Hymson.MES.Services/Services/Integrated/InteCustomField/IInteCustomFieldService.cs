using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;
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
        /// <param name="saveDtos"></param>
        /// <returns></returns>
        Task AddOrUpdateAsync(IEnumerable<InteCustomFieldSaveDto> saveDtos);


        /// <summary>
        /// 获取业务类型下的自定义字段信息（包含对应的语言设置）
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        Task<IEnumerable<InteCustomFieldDto>> GetDataByBusinessTypeAsync(InteCustomFieldBusinessTypeEnum businessType);
    }
}