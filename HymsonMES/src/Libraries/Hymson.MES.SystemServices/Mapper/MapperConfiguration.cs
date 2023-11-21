using AutoMapper;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Report;
using Hymson.MES.SystemServices.Dtos.Manufacture;
using Hymson.MES.SystemServices.Dtos.Plan;
using Hymson.MES.SystemServices.Dtos.ProductTraceReport;
using Hymson.MES.SystemServices.Dtos.ProductTraceReport.Query;

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
            CreateReportMaps();
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

        /// <summary>
        /// 报表模块
        /// </summary>
        protected virtual void CreateReportMaps()
        {
            CreateMap<ManuSfcStepPagedQueryDto, ManuSfcStepPagedQuery>();
            CreateMap<ManuProductPrameterPagedQueryDto, ManuProductParameterPagedQuery>();
            CreateMap<ProductTracePagedQueryDto, ProductTraceReportPagedQuery>();
            CreateMap<ManuSfcStepEntity, ManuSfcStepViewDto>();
            CreateMap<ManuProductParameterView, ManuProductParameterViewDto>();

            #region PACK码追溯SFC

            CreateMap<PackTraceSFCParameterQuery, PackTraceSFCParameterQueryDto>();
            CreateMap<PackTraceSFCParameterView, PackTraceSFCParameterViewDto>();

            #endregion
        }
    }
}
