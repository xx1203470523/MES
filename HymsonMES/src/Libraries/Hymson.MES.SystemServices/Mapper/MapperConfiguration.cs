using AutoMapper;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.SystemServices.Dtos.Manufacture;
using Hymson.MES.SystemServices.Dtos.Plan;

namespace Hymson.MES.SystemServices.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    public class MapperConfiguration : Profile, IOrderedMapperProfile
    {
        /// <summary>
        /// 
        /// </summary>
        public int Order => 0;

        /// <summary>
        /// 
        /// </summary>
        public MapperConfiguration()
        {
            CreatePlanMaps();
            CreateManufactureMaps();
        }

        /// <summary>
        /// 计划模块
        /// </summary>
        protected virtual void CreatePlanMaps()
        {
            CreateMap<PlanWorkOrderDto, PlanWorkOrderEntity>();
        }

        /// <summary>
        /// 生产模块
        /// </summary>
        protected virtual void CreateManufactureMaps()
        {
            CreateMap<ManuSfcCirculationEntity, ManuSfcCirculationViewDto>();
        }
    }
}
