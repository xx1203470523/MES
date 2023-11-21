﻿using FluentValidation;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Integrated;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Context;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Execute;
using Hymson.MES.CoreServices.Services.Manufacture;
using Hymson.MES.CoreServices.Services.Manufacture.ManuBind;
using Hymson.MES.CoreServices.Services.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Services.Manufacture.ManuSfcSummary;
using Hymson.MES.CoreServices.Services.Parameter;
using Hymson.MES.CoreServices.Validators;
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
            services.AddSequenceService(configuration);
            services.AddMessagePushService(configuration);
            services.AddData(configuration);
            AddManuServices(services);
            AddIntegratedServices(services);
            AddValidators(services);
            AddConfig(services, configuration);
            return services;
        }

        /// <summary>
        /// 添加服务依赖
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddManuServices(this IServiceCollection services)
        {
            services.AddSingleton<IManuDegradedProductExtendService, ManuDegradedProductExtendService>();
            services.AddSingleton<IManuCreateBarcodeService, ManuCreateBarcodeService>();
            services.AddSingleton<IManuGenerateBarcodeService, ManuGenerateBarcodeService>();
            services.AddSingleton<IManuCommonService, ManuCommonService>();
            services.AddSingleton<IMasterDataService, MasterDataService>();
            services.AddSingleton<IJobCommonService, JobCommonService>();
            services.AddSingleton<ScopedServiceFactory>();
            services.AddTransient<IJobContextProxy, JobContextProxy>();

            services.AddSingleton<IJobService, BadRecordJobService>();
            services.AddSingleton<IJobService, BarcodeReceiveService>();
            services.AddSingleton<IJobService, EsopOutJobService>();
            services.AddSingleton<IJobService, InStationInterceptJobService>();
            services.AddSingleton<IJobService, InStationJobService>();
            services.AddSingleton<IJobService, InStationMarkInterceptJobService>();
            services.AddSingleton<IJobService, IOutputModifyService>();
            services.AddSingleton<IJobService, OutStationJobService>();
            services.AddSingleton<IJobService, PackageCloseJobService>();
            services.AddSingleton<IJobService, PackageIngJobService>();
            services.AddSingleton<IJobService, PackageOpenJobService>();
            services.AddSingleton<IJobService, PackageVerifyJobService>();
            services.AddSingleton<IJobService, ParameterCollectJobService>();
            services.AddSingleton<IJobService, PartialScrapJobService>();
            services.AddSingleton<IJobService, ProductBadRecordJobService>();
            services.AddSingleton<IJobService, ProductsSortingJobService>();
            services.AddSingleton<IJobService, RepairEndJobService>();
            services.AddSingleton<IJobService, RepairStartJobService>();
            services.AddSingleton<IJobService, SFCRequestJobService>();
            services.AddSingleton<IJobService, SmiFinishedJobService>();
            services.AddSingleton<IJobService, StopJobService>();

            services.AddSingleton<IManuProductParameterService, ManuProductParameterService>();
            services.AddSingleton(typeof(IExecuteJobService<>), typeof(ExecuteJobService<>));
            services.AddSingleton<IManuEquipmentParameterService, ManuEquipmentParameterService>();
            services.AddSingleton<IManuSfcSummaryService, ManuSfcSummaryService>();
            services.AddSingleton<IManuPassStationService, ManuPassStationService>();
            services.AddSingleton<IManuBindService, ManuBindService>();

            return services;
        }

        /// <summary>
        /// 添加服务依赖（综合模块）
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddIntegratedServices(this IServiceCollection services)
        {
            services.AddSingleton<IMessagePushService, MessagePushService>();
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
            services.AddSingleton<AbstractValidator<EsopOutRequestBo>, EsopOutJobValidator>();

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
            services.Configure<Data.Options.ParameterOptions>(configuration.GetSection(nameof(Data.Options.ParameterOptions)));
            return services;
        }
    }
}
