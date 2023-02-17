using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Equipment.EquConsumableType;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentLinkApi;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit;
using Hymson.MES.Data.Repositories.Equipment.EquFaultPhenomenon;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart;
using Hymson.MES.Data.Repositories.Equipment.EquSparePartType;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteCalendar;
using Hymson.MES.Data.Repositories.Integrated.InteClass;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataCollectionExtensions
    {
        /// <summary>
        /// 数据层依赖服务注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCore();
            AddConfig(services, configuration);
            AddRepository(services);
            return services;
        }

        /// <summary>
        /// 添加仓储依赖
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddRepository(this IServiceCollection services)
        {
            #region Equipment
            services.AddSingleton<IEquConsumableTypeRepository, EquConsumableTypeRepository>();
            services.AddSingleton<IEquEquipmentRepository, EquEquipmentRepository>();
            services.AddSingleton<IEquEquipmentGroupRepository, EquEquipmentGroupRepository>();
            services.AddSingleton<IEquEquipmentLinkApiRepository, EquEquipmentLinkApiRepository>();
            services.AddSingleton<IEquEquipmentLinkHardwareRepository, EquEquipmentLinkHardwareRepository>();
            services.AddSingleton<IEquEquipmentUnitRepository, EquEquipmentUnitRepository>();
            services.AddSingleton<IEquFaultPhenomenonRepository, EquFaultPhenomenonRepository>();
            services.AddSingleton<IEquSparePartRepository, EquSparePartRepository>();
            services.AddSingleton<IEquSparePartTypeRepository, EquSparePartTypeRepository>();
            #endregion

            #region Integrated
            services.AddSingleton<IInteCalendarDateDetailRepository, InteCalendarDateDetailRepository>();
            services.AddSingleton<IInteCalendarDateRepository, InteCalendarDateRepository>();
            services.AddSingleton<IInteCalendarRepository, InteCalendarRepository>();
            services.AddSingleton<IInteClassDetailRepository, InteClassDetailRepository>();
            services.AddSingleton<IInteClassRepository, InteClassRepository>();

            //InteJob
            services.AddSingleton<IInteJobBusinessRelationRepository, InteJobBusinessRelationRepository>();
            services.AddSingleton<IInteJobRepository, InteJobRepository>();
            #endregion

            #region  Process

            #region Material
            services.AddSingleton<IProcMaterialRepository, ProcMaterialRepository>();
            services.AddSingleton<IProcReplaceMaterialRepository, ProcReplaceMaterialRepository>();

            services.AddSingleton<IProcMaterialGroupRepository, ProcMaterialGroupRepository>();
            #endregion

            #region Parameter
            services.AddSingleton<IProcParameterRepository, ProcParameterRepository>();

            #endregion

            #region ParameterLinkType
            services.AddSingleton<IProcParameterLinkTypeRepository, ProcParameterLinkTypeRepository>();

            #endregion

            #region Bom
            services.AddSingleton<IProcBomRepository, ProcBomRepository>();
            services.AddSingleton<IProcBomDetailRepository, ProcBomDetailRepository>();
            services.AddSingleton<IProcBomDetailReplaceMaterialRepository, ProcBomDetailReplaceMaterialRepository>();
            #endregion

            #region LoadPoint
            services.AddSingleton<IProcLoadPointRepository, ProcLoadPointRepository>();

            #endregion

            #region Resource
            services.AddSingleton<IProcResourceTypeRepository, ProcResourceTypeRepository>();
            services.AddSingleton<IProcResourceRepository, ProcResourceRepository>();
            services.AddSingleton<IProcResourceConfigPrintRepository, ProcResourceConfigPrintRepository>();
            services.AddSingleton<IProcResourceConfigResRepository, ProcResourceConfigResRepository>();
            services.AddSingleton<IProcResourceEquipmentBindRepository, ProcResourceEquipmentBindRepository>();
            services.AddSingleton<IProcResourceConfigJobRepository, ProcResourceConfigJobRepository>();
            #endregion

            #region Procedure
            services.AddSingleton<IProcProcedureRepository, ProcProcedureRepository>();
            services.AddSingleton<IProcProcedurePrintReleationRepository, ProcProcedurePrintReleationRepository>();
            #endregion
            #region ProcessRoute
            services.AddSingleton<IProcProcessRouteRepository, ProcProcessRouteRepository>();
            services.AddSingleton<IProcProcessRouteDetailNodeRepository, ProcProcessRouteDetailNodeRepository>();
            services.AddSingleton<IProcProcessRouteDetailLinkRepository, ProcProcessRouteDetailLinkRepository>();
            #endregion
            #endregion

            return services;
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static IServiceCollection AddConfig(IServiceCollection services, IConfiguration configuration)
        {
            //数据库连接
            services.Configure<ConnectionOptions>(configuration.GetSection(nameof(ConnectionOptions)));
            //services.Configure<ConnectionOptions>(configuration);
            return services;
        }

    }
}
