using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;

namespace Hymson.MES.CoreServices.Services.Common.ManuExtension
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class MapperExtensions
    {
        /// <summary>
        /// 转换BO对象
        /// </summary>
        /// <typeparam name="TBo"></typeparam>
        /// <param name="bo"></param>
        /// <returns></returns>
        public static TBo ToBo<TBo>(this JobBaseBo bo) where TBo : JobBaseBo
        {
            if (bo == null) throw new ArgumentNullException(nameof(bo));
            return AutoMapperConfiguration.Mapper.Map<TBo>(bo);
        }

        /// <summary>
        /// 转换BO对象
        /// </summary>
        /// <typeparam name="TBo"></typeparam>
        /// <param name="bo"></param>
        /// <returns></returns>
        public static TBo ToBo<TBo>(this BaseEntity bo) where TBo : BaseBo
        {
            if (bo == null) throw new ArgumentNullException(nameof(bo));
            return AutoMapperConfiguration.Mapper.Map<TBo>(bo);
        }

        /// <summary>
        /// 转换Entity对象
        /// </summary>
        /// <typeparam name="TBo"></typeparam>
        /// <param name="bo"></param>
        /// <returns></returns>
        public static TEntity ToEntity<TEntity>(this BaseBo bo) where TEntity : BaseEntity
        {
            if (bo == null) throw new ArgumentNullException(nameof(bo));
            return AutoMapperConfiguration.Mapper.Map<TEntity>(bo);
        }


    }
}
