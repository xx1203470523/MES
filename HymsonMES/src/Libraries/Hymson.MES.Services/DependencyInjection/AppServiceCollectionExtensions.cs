using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Options;
using Hymson.MES.Services.Services.EquEquipmentGroup;
using Hymson.MES.Services.Services.Equipment.EquEquipment;
using Hymson.MES.Services.Services.Equipment.EquEquipmentUnit;
using Hymson.MES.Services.Services.InteClass;
using Hymson.MES.Services.Services.Process;
using Hymson.MES.Services.Services.Process.IProcessService;
using Hymson.MES.Services.Validators.Equipment;
using Hymson.MES.Services.Validators.Process;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class AppServiceCollectionExtensions
    {
        /// <summary>
        /// 业务逻辑层依赖服务添加
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddData(configuration);
            AddConfig(services, configuration);
            AddServices(services);
            AddValidators(services);
            return services;
        }

        /// <summary>
        /// 添加服务依赖
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            #region Equipment
            services.AddSingleton<IEquEquipmentService, EquEquipmentService>();
            services.AddSingleton<IEquEquipmentGroupService, EquEquipmentGroupService>();
            services.AddSingleton<IEquEquipmentUnitService, EquEquipmentUnitService>();
            #endregion

            #region Integrated
            services.AddSingleton<IInteClassService, InteClassService>();
            #endregion

            #region ProcMaterial
            services.AddSingleton<IProcMaterialService, ProcMaterialService>();
            #endregion

            #region Resource
            services.AddSingleton<IProcResourceTypeService, ProcResourceTypeService>();
            services.AddSingleton<IProcResourceService, ProcResourceService>();
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
            services.Configure<TestOptions>(configuration.GetSection(nameof(TestOptions)));
            //services.Configure<ConnectionOptions>(configuration);
            return services;
        }

        /// <summary>
        /// 添加验证器相关服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddValidators(IServiceCollection services)
        {
            #region Equipment
            services.AddSingleton<AbstractValidator<EquEquipmentUnitCreateDto>, EquipmentUnitCreateValidator>();
            #endregion

            services.AddSingleton<AbstractValidator<ProcMaterialCreateDto>, ProcMaterialCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcMaterialModifyDto>, ProcMaterialModifyValidator>();

            #region Resource
            services.AddSingleton<AbstractValidator<ProcResourceDto>, ProcResourceValidator>();
            #endregion
            return services;
        }
    }
}
