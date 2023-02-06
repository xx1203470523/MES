using AutoMapper;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.OnStock;
using Hymson.MES.Data.Repositories.OnStock;
using Hymson.MES.Services.Dtos.OnStock;

namespace Hymson.MES.Services.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    public class MapperConfiguration : Profile, IOrderedMapperProfile
    {
        /// <summary>
        /// 
        /// </summary>
        public MapperConfiguration()
        {
            CreateWhStockChangeRecordMaps();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateWhStockChangeRecordMaps()
        {
            CreateMap<WhStockChangeRecordDto, WhStockChangeRecordEntity>();
            CreateMap<WhStockChangeRecordEntity, WhStockChangeRecordDto>();
            CreateMap<WhStockChangeRecordPagedQueryDto, WhStockChangeRecordPagedQuery>();
        }

        /// <summary>
        /// 排序，决定了加载的顺序
        /// </summary>
        public int Order => 0;
    }
}
