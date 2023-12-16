using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Services;
using Hymson.MES.EquipmentServices.Services.Common;
using Hymson.MES.EquipmentServices.Services.Manufacture;
using Hymson.MES.EquipmentServices.Services.Parameter.ProcessCollection;
using Hymson.MES.EquipmentServices.Services.SfcBinding;
using Hymson.MES.EquipmentServices.Validators.Manufacture;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class AppEquipmentServiceCollectionExtensions
    {
        /// <summary>
        /// 业务逻辑层依赖服务添加
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddEquipmentService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWebFrameworkService(configuration);
            services.AddData(configuration);
            AddConfig(services, configuration);

            AddServices(services);
            AddServicesForXinShiJie(services);

            AddValidators(services);
            AddValidatorsForXinShiJie(services);

            return services;
        }

        /// <summary>
        /// 添加服务依赖
        /// </summary>
        /// <param name="services"></param>
        private static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ICommonService, CommonService>();
            services.AddSingleton<IManufactureService, ManufactureService>();
            services.AddSingleton<IProcessCollectionService, ProcessCollectionService>();
            services.AddSingleton<ISfcBindingService, SfcBindingService>();
            services.AddSingleton<ITracingSourceSFCService, TracingSourceSFCService>();
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddConfig(IServiceCollection services, IConfiguration configuration)
        {

        }

        /// <summary>
        /// 添加验证器相关服务
        /// </summary>
        /// <param name="services"></param>
        private static void AddValidators(IServiceCollection services)
        {
            services.AddSingleton<AbstractValidator<InBoundDto>, InBoundValidator>();//进站
            services.AddSingleton<AbstractValidator<InBoundMoreDto>, InBoundMoreValidator>();//进站（多个）
            services.AddSingleton<AbstractValidator<OutBoundDto>, OutBoundValidator>();//出站
            services.AddSingleton<AbstractValidator<OutBoundMoreDto>, OutBoundMoreValidator>();//出站（多个）

            services.AddSingleton<AbstractValidator<SfcBindingDto>, SfcBindingValidator>();
        }

    }
}
