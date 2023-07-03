using AutoMapper;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.CoreServices.Bos.Job;

namespace Hymson.MES.CoreServices.Mapper
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
            CreateInStationMaps();
            CreateOutStationMaps();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateInStationMaps()
        {
            CreateMap<OutStationRequestBo, InStationRequestBo>();
            CreateMap<RepairStartRequestBo, InStationRequestBo>();
            CreateMap<RepairEndRequestBo, InStationRequestBo>();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateOutStationMaps()
        {
            CreateMap<InStationRequestBo, OutStationRequestBo>();
            CreateMap<RepairStartRequestBo, OutStationRequestBo>();
            CreateMap<RepairEndRequestBo, OutStationRequestBo>();
        }

        /// <summary>
        /// 排序，决定了加载的顺序
        /// </summary>
        public int Order => 0;

    }
}
