using AutoMapper;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.OnStock;
using Hymson.MES.Data.Repositories.OnStock;
using Hymson.MES.Services.Dtos.OnStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Mapper
{
    public class MapperConfiguration : Profile, IOrderedMapperProfile
    {

        public MapperConfiguration() {
            CreateWhStockChangeRecordMaps();
        }

        protected virtual void CreateWhStockChangeRecordMaps() {
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
