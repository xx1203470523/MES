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
            CreateRequestBoMaps();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateRequestBoMaps()
        {
            CreateMap<JobRequestBo, InStationRequestBo>();
            CreateMap<JobRequestBo, OutStationRequestBo>();
            CreateMap<JobRequestBo, StopRequestBo>();
        }

        /// <summary>
        /// 排序，决定了加载的顺序
        /// </summary>
        public int Order => 0;

    }
}
