using FluentValidation;
using Hymson.MES.EquipmentServices.Request.BindContainer;
using Hymson.MES.EquipmentServices.Request.BindSFC;
using Hymson.MES.EquipmentServices.Request.InBound;
using Hymson.MES.EquipmentServices.Request.OutBound;
using Hymson.MES.EquipmentServices.Services.BindContainer;
using Hymson.MES.EquipmentServices.Services.BindSFC;
using Hymson.MES.EquipmentServices.Services.Equipment;
using Hymson.MES.EquipmentServices.Services.InBound;
using Hymson.MES.EquipmentServices.Services.OutBound;
using Hymson.MES.EquipmentServices.Validators.BindContainer;
using Hymson.MES.EquipmentServices.Validators.BindSFC;
using Hymson.MES.EquipmentServices.Validators.InBound;
using Hymson.MES.EquipmentServices.Validators.OutBound;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class AppEquipmentServiceCollectionExtensions
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
            AddValidators(services);
            return services;
        }

        /// <summary>
        /// 添加服务依赖
        /// </summary>
        /// <param name="services"></param>
        private static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IBindSFCService, BindSFCService>();//条码解绑绑定
            services.AddSingleton<IBindContainerService, BindContainerService>();//容器解绑绑定
            services.AddSingleton<IInBoundService, InBoundService>();//进站
            services.AddSingleton<IOutBoundService, OutBoundService>();//出站
            services.AddSingleton<IEquipmentService, EquipmentService>();   // 设备
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddConfig(IServiceCollection services, IConfiguration configuration)
        {
            //数据库连接
            //services.Configure<TestOptions>(configuration.GetSection(nameof(TestOptions)));
            //services.Configure<ConnectionOptions>(configuration);
            //return services;
        }

        /// <summary>
        /// 添加验证器相关服务
        /// </summary>
        /// <param name="services"></param>
        private static void AddValidators(IServiceCollection services)
        {
            services.AddSingleton<AbstractValidator<BindSFCRequest>, BindSFCValidator>();//条码绑定
            services.AddSingleton<AbstractValidator<UnBindSFCRequest>, UnBindSFCValidator>();//条码解绑
            services.AddSingleton<AbstractValidator<BindContainerRequest>, BindContainerValidator>();//容器绑定
            services.AddSingleton<AbstractValidator<UnBindContainerRequest>, UnBindContainerValidator>();//容器解绑
            services.AddSingleton<AbstractValidator<InBoundRequest>, InBoundValidator>();//进站
            services.AddSingleton<AbstractValidator<InBoundMoreRequest>, InBoundMoreValidator>();//进站（多个）
            services.AddSingleton<AbstractValidator<OutBoundRequest>, OutBoundValidator>();//出站
            services.AddSingleton<AbstractValidator<OutBoundMoreRequest>, OutBoundMoreValidator>();//出站（多个）
        }

    }
}
