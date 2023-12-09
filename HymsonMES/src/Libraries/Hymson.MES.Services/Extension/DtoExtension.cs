using Hymson.Infrastructure.Mapper;

namespace Hymson.MES.Data.Repositories;

/// <summary>
/// 指令扩展
/// </summary>
public static class DtoExtension
{
    /// <summary>
    /// 转换到查询对象
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static TQuery ToQuery<TQuery>(this QueryDtoAbstraction dto) where TQuery : QueryAbstraction
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        return AutoMapperConfiguration.Mapper.Map<TQuery>(dto);
    }
}
