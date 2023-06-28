using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Context;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Execute;
using Hymson.MES.CoreServices.Services.NewJob;
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
            return services;
        }

        /// <summary>
        /// 添加服务依赖
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IManuCommonService, ManuCommonService>();
            services.AddSingleton<IMasterDataService, MasterDataService>();
            services.AddSingleton<IJobCommonService, JobCommonService>();
            services.AddSingleton<ScopedServiceFactory>();
            services.AddScoped<IJobContextProxy, JobContextProxy>();
            //services.AddSingleton(typeof(IJobService<,>), typeof(InStationJobService<,>));
            //services.AddSingleton(typeof(IJobService<,>), typeof(InStationJobService));
            services.AddSingleton<IJobService, InStationJobService>();
            services.AddSingleton<IJobService, OutStationJobService>();
            services.AddSingleton<IJobService, RepairStartJobService>();
            services.AddSingleton(typeof(IExecuteJobService<>), typeof(ExecuteJobService<>));
            //services.AddSingleton<ExecuteJobService<OutStationRequestBo>, ExecuteJobService<OutStationRequestBo>>();
            //services.AddSingleton<ExecuteJobService<SfcConvertRequestBo>, ExecuteJobService<SfcConvertRequestBo>>();

            return services;
        }

    }
}
