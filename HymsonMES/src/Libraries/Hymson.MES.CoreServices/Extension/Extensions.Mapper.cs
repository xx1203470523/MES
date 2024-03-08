using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;

namespace Hymson.MES.CoreServices.Extension
{
    public static class MapperExtensions
    {
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
    }
}
