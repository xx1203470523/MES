using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.Data.Repositories;
using Hymson.MES.Services.Dtos.Common;

namespace Hymson.MES.Services
{
    /// <summary>
    /// 指令扩展
    /// </summary>
    public static class MapperExtensions
    {
        /// <summary>
        /// 转换Dto对象
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="bo"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TDto ToDto<TDto>(this BaseBo bo) where TDto : BaseDto
        {
            if (bo == null) throw new ArgumentNullException(nameof(bo));
            return AutoMapperConfiguration.Mapper.Map<TDto>(bo);
        }

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
}
