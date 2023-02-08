using AutoMapper;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.OnStock;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.OnStock;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.OnStock;
using Hymson.MES.Services.Dtos.Process;

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

            CreateProcMaterialMaps();
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

        protected virtual void CreateProcMaterialMaps()
        {
            CreateMap<ProcMaterialCreateDto, ProcMaterialEntity>();
            CreateMap<ProcMaterialModifyDto, ProcMaterialEntity>();
            CreateMap<ProcMaterialPagedQueryDto, ProcMaterialPagedQuery>();
            CreateMap<ProcMaterialEntity, ProcMaterialDto>();
        }

        /// <summary>
        /// 排序，决定了加载的顺序
        /// </summary>
        public int Order => 0;
    }
}
