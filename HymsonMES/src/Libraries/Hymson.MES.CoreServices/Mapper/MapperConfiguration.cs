using AutoMapper;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Dtos.Parameter;

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
            CreateParameterBoMaps();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateRequestBoMaps()
        {
            CreateMap<JobRequestBo, InStationRequestBo>();
            CreateMap<JobRequestBo, OutStationRequestBo>();
            CreateMap<JobRequestBo, StopRequestBo>();
            CreateMap<JobRequestBo, BadRecordRequestBo>();
            CreateMap<JobRequestBo, PackageRequestBo>();
            CreateMap<JobRequestBo, RepairStartRequestBo>();
            CreateMap<JobRequestBo, RepairEndRequestBo>();
            CreateMap<JobRequestBo, PackageIngRequestBo>();
            CreateMap<JobRequestBo, PackageOpenRequestBo>();
            CreateMap<JobRequestBo, PackageCloseRequestBo>();
            CreateMap<JobRequestBo, BarcodeSfcReceiveBo>();
            CreateMap<JobRequestBo, ProductBadRecordRequestBo>();
        }


        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateParameterBoMaps()
        {
            CreateMap<ParameterDto,ManuProductParameterEntity> ();
            CreateMap<EquipmentParameterDto, EquipmentParameterEntity>();
        }

        /// <summary>
        /// 排序，决定了加载的顺序
        /// </summary>
        public int Order => 0;

    }
}
