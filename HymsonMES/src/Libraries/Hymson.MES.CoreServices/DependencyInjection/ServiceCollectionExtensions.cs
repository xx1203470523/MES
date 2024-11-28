using FluentValidation;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Context;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Execute;
using Hymson.MES.CoreServices.Services.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Services.NewJob;
using Hymson.MES.CoreServices.Services.SysSetting;
using Hymson.MES.Services.Validators.Equipment;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hymson.MES.CoreServices.DependencyInjection
{
    /// <summary>
    /// 依赖注入项配置
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 业务逻辑层依赖服务添加
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCoreService(this IServiceCollection services, IConfiguration configuration)
        {
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
            services.AddSingleton<IManuCreateBarcodeService, ManuCreateBarcodeService>();
            services.AddSingleton<IManuGenerateBarcodeService, ManuGenerateBarcodeService>();
            services.AddSingleton<IManuCommonService, ManuCommonService>();
            services.AddSingleton<IMasterDataService, MasterDataService>();
            services.AddSingleton<IJobCommonService, JobCommonService>();
            services.AddSingleton<ScopedServiceFactory>();
            services.AddScoped<IJobContextProxy, JobContextProxy>();
            //services.AddSingleton(typeof(IJobService<,>), typeof(InStationJobService<,>));
            //services.AddSingleton(typeof(IJobService<,>), typeof(InStationJobService));
            services.AddSingleton<IJobService, InStationVerifyJobService>();
            services.AddSingleton<IJobService, InStationJobService>();
            services.AddSingleton<IJobService, OutStationVerifyJobService>();
            services.AddSingleton<IJobService, OutStationJobService>();
            services.AddSingleton<IJobService, StopJobService>();
            services.AddSingleton<IJobService, BadRecordJobService>();
            services.AddSingleton<IJobService, PackageVerifyJobService>();
            services.AddSingleton<IJobService, RepairStartJobService>();
            services.AddSingleton<IJobService, RepairEndJobService>();
            services.AddSingleton<IJobService, PackageIngJobService>();
            services.AddSingleton<IJobService, PackageOpenJobService>();
            services.AddSingleton<IJobService, PackageCloseJobService>();
            services.AddSingleton<IJobService, BarcodeReceiveService>();
            services.AddSingleton(typeof(IExecuteJobService<>), typeof(ExecuteJobService<>));
            services.AddSingleton<ISysSettingService,SysSettingService>();
            //services.AddSingleton<ExecuteJobService<OutStationRequestBo>, ExecuteJobService<OutStationRequestBo>>();

            return services;
        }

        /// <summary>
        /// 添加验证器相关服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddValidators(IServiceCollection services)
        {
            services.AddSingleton<AbstractValidator<RepairStartRequestBo>, RepairStartJobValidator>();
            services.AddSingleton<AbstractValidator<RepairEndRequestBo>, RepairEndJobValidator>();
            services.AddSingleton<AbstractValidator<PackageIngRequestBo>, PackageIngJobValidator>();
            services.AddSingleton<AbstractValidator<PackageOpenRequestBo>, PackageOpenJobValidator>();
            services.AddSingleton<AbstractValidator<PackageCloseRequestBo>, PackageCloseJobValidator>(); 

            services.AddSingleton<IManuGenerateBarcodeService, ManuGenerateBarcodeService>();//条码生成

            return services;
        }

    }
}
