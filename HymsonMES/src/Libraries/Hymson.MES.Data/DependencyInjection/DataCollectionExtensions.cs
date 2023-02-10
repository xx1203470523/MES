using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentLinkApi;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit;
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
            services.AddSingleton<IEquEquipmentRepository, EquEquipmentRepository>();
            services.AddSingleton<IEquEquipmentGroupRepository, EquEquipmentGroupRepository>();
            services.AddSingleton<IEquEquipmentLinkApiRepository, EquEquipmentLinkApiRepository>();
            services.AddSingleton<IEquEquipmentLinkHardwareRepository, EquEquipmentLinkHardwareRepository>();
            services.AddSingleton<IEquEquipmentUnitRepository, EquEquipmentUnitRepository>();
            #endregion

            #region Integrated
            services.AddSingleton<IInteClassDetailRepository, InteClassDetailRepository>();
            services.AddSingleton<IInteClassRepository, InteClassRepository>();
            #endregion

            #region ProcMaterial
            services.AddSingleton<IProcMaterialRepository, ProcMaterialRepository>();
            services.AddSingleton<IProcReplaceMaterialRepository, ProcReplaceMaterialRepository>();
            #endregion

            #region Resource
            services.AddSingleton<IProcResourceTypeRepository, ProcResourceTypeRepository>();
            services.AddSingleton<IProcResourceRepository, ProcResourceRepository>();
            services.AddSingleton<IProcResourceConfigPrintRepository, ProcResourceConfigPrintRepository>();
            services.AddSingleton<IProcResourceConfigResRepository, ProcResourceConfigResRepository>();
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
