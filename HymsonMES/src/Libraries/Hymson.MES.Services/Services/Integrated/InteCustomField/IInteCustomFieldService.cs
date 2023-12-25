using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
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

        /// <summary>
        /// 保存各个业务ID的自定义字段数据
        /// </summary>
        /// <param name="saveBusinessDtos"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        Task SaveBusinessDataAsync(IEnumerable<InteCustomFieldBusinessEffectuateDto> saveBusinessDtos);

        /// <summary>
        /// 根据业务ID获取业务数据
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        Task<IEnumerable<InteCustomFieldBusinessEffectuateDto>> GetBusinessEffectuatesAsync(long businessId);

        /// <summary>
        /// 根据业务ID删除业务数据
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        Task<int> DelBusinessEffectuatesAsync(long[] businessId);
    }
}