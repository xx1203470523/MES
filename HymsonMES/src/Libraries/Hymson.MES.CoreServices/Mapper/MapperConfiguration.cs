using AutoMapper;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Qual;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Parameter;
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
            CreateQualityBoMaps();
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
            CreateMap<JobRequestBo, OutputModifyBo>();
            CreateMap<JobRequestBo, EsopOutRequestBo>();
        }


        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateParameterBoMaps()
        {
            CreateMap<ParameterDto, ManuProductParameterEntity>();
            CreateMap<ParameterBo, ManuProductParameterEntity>();
            CreateMap<ProcProductParameterGroupDetailEntity, ProcProductParameterGroupDetailBo>();
            CreateMap<EquipmentParameterDto, EquipmentParameterEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateQualityBoMaps()
        {
            CreateMap<QualOqcParameterGroupEntity, QualOqcParameterGroupSnapshootEntity>();
            CreateMap<QualOqcParameterGroupDetailEntity, QualOqcParameterGroupDetailSnapshootEntity>();

            CreateMap<QualIqcInspectionItemEntity, QualIqcInspectionItemSnapshotEntity>();
            CreateMap<QualIqcInspectionItemDetailEntity, QualIqcInspectionItemDetailSnapshotEntity>();
        }

        /// <summary>
        /// 排序，决定了加载的顺序
        /// </summary>
        public int Order => 0;

    }
}
