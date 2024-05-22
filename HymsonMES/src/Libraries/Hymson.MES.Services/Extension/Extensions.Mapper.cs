using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.Data.Repositories;
using Hymson.MES.Services.Dtos;
using Hymson.MES.Services.Dtos.Common;

namespace Hymson.MES.Services
{
    /// <summary>
    /// 指令扩展
    /// </summary>
    public static class MapperExtensions
    {
        /// <summary>
        /// 转换到实体
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TCommand ToCommand<TCommand>(this BaseEntityDto dto) where TCommand : CommandAbstraction
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            return AutoMapperConfiguration.Mapper.Map<TCommand>(dto);
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

        /// <summary>
        /// 转换Dto对象
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="bo"></param>
        /// <returns></returns>
        public static TDto ToDto<TDto>(this BaseBo bo) where TDto : BaseDto
        {
            if (bo == null) throw new ArgumentNullException(nameof(bo));
            return AutoMapperConfiguration.Mapper.Map<TDto>(bo);
        }

        /// <summary>
        /// 转换Dto对象
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="bo"></param>
        /// <returns></returns>
        public static TDto ToDto<TDto>(this BaseDto bo) where TDto : BaseDto
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
        public static TQuery ToQuery<TQuery>(this BaseEntityDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return AutoMapperConfiguration.Mapper.Map<TQuery>(dto);
        }

        /// <summary>
        /// 转换到实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="baseEntity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TEntity ToEntity<TEntity>(this BaseEntity baseEntity) where TEntity : BaseEntity
        {
            if (baseEntity == null)
            {
                throw new ArgumentNullException(nameof(baseEntity));
            }
            return AutoMapperConfiguration.Mapper.Map<TEntity>(baseEntity);
        }

        /// <summary>
        /// 深度复制
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TEntity ToCopy<TEntity>(this TEntity source) where TEntity : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            return AutoMapperConfiguration.Mapper.Map<TEntity>(source);
        }

        /// <summary>
        /// 两实体合并映射
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TDestination ToCombineMap<TSource, TDestination>(this TSource source, TDestination destination)
        where TSource : class
        where TDestination : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

    }
}
