using FluentValidation;
using Hymson.MES.EquipmentServices.Request.BindContainer;
using Hymson.MES.EquipmentServices.Request.BindSFC;
using Hymson.MES.EquipmentServices.Request.CCDFileUploadComplete;
using Hymson.MES.EquipmentServices.Request.FeedingConsumption;
using Hymson.MES.EquipmentServices.Request.GenerateModuleSFC;
using Hymson.MES.EquipmentServices.Request.InBound;
using Hymson.MES.EquipmentServices.Request.InboundInContainer;
using Hymson.MES.EquipmentServices.Request.InboundInSFCContainer;
using Hymson.MES.EquipmentServices.Request.OutBound;
using Hymson.MES.EquipmentServices.Request.OutPutQty;
using Hymson.MES.EquipmentServices.Request.QueryContainerBindSfc;
using Hymson.MES.EquipmentServices.Request.SingleBarCodeLoadingVerification;
//using Hymson.MES.EquipmentServices.Request.UnBindContainer;
using Hymson.MES.EquipmentServices.Services.BindContainer;
using Hymson.MES.EquipmentServices.Services.BindSFC;
using Hymson.MES.EquipmentServices.Services.CCDFileUploadComplete;
using Hymson.MES.EquipmentServices.Services.Equipment;
using Hymson.MES.EquipmentServices.Services.FeedingConsumption;
using Hymson.MES.EquipmentServices.Services.GenerateModuleSFC;
using Hymson.MES.EquipmentServices.Services.InBound;
using Hymson.MES.EquipmentServices.Services.InboundInContainer;
using Hymson.MES.EquipmentServices.Services.InboundInSFCContainer;
using Hymson.MES.EquipmentServices.Services.OutBound;
using Hymson.MES.EquipmentServices.Services.OutPutQty;
using Hymson.MES.EquipmentServices.Services.QueryContainerBindSfc;
using Hymson.MES.EquipmentServices.Services.SingleBarCodeLoadingVerification;
//using Hymson.MES.EquipmentServices.Services.UnBindContainer;
using Hymson.MES.EquipmentServices.Validators.BindContainer;
using Hymson.MES.EquipmentServices.Validators.BindSFC;
using Hymson.MES.EquipmentServices.Validators.CCDFileUploadComplete;
using Hymson.MES.EquipmentServices.Validators.FeedingConsumption;
using Hymson.MES.EquipmentServices.Validators.GenerateModuleSFC;
using Hymson.MES.EquipmentServices.Validators.InBound;
using Hymson.MES.EquipmentServices.Validators.InboundInContainer;
using Hymson.MES.EquipmentServices.Validators.InboundInSFCContainer;
using Hymson.MES.EquipmentServices.Validators.OutBound;
using Hymson.MES.EquipmentServices.Validators.OutPutQty;
using Hymson.MES.EquipmentServices.Validators.QueryContainerBindSfc;
using Hymson.MES.EquipmentServices.Validators.SingleBarCodeLoadingVerification;
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
            //services.AddSingleton<IOutBoundMoreService, OutBoundMoreService>();//出站（多个）


            services.AddSingleton<IInboundInContainerService, InboundInContainerService>();// 进站-容器
            services.AddSingleton<IGenerateModuleSFCService, GenerateModuleSFCService>();//请求生成模组码-电芯堆叠
            services.AddSingleton<IInboundInSFCContainerService, InboundInSFCContainerService>();//进站-电芯和托盘-装盘2
            services.AddSingleton<ICCDFileUploadCompleteService, CCDFileUploadCompleteService>();//CCD文件上传完成
            services.AddSingleton<IFeedingConsumptionService, FeedingConsumptionService>();//上报物料消耗
            services.AddSingleton<ISingleBarCodeLoadingVerificationService, SingleBarCodeLoadingVerificationService>();//单体条码上料校验
            services.AddSingleton<IOutPutQtyService, OutPutQtyService>();//产出上报数量
            services.AddSingleton<IQueryContainerBindSfcService, QueryContainerBindSfcService>();//容器绑定条码查询


            services.AddSingleton<IEquipmentMonitorService, EquipmentMonitorService>();   // 设备
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



            services.AddSingleton<AbstractValidator<InboundInContainerRequest>, InboundInContainerValidator>();// 进站-容器
            services.AddSingleton<AbstractValidator<GenerateModuleSFCRequest>, GenerateModuleSFCValidator>();//请求生成模组码-电芯堆叠
            services.AddSingleton<AbstractValidator<InboundInSFCContainerRequest>, InboundInSFCContainerValidator>();//进站-电芯和托盘-装盘2
            services.AddSingleton<AbstractValidator<CCDFileUploadCompleteRequest>, CCDFileUploadCompleteValidator>();//CCD文件上传完成
            services.AddSingleton<AbstractValidator<FeedingConsumptionRequest>, FeedingConsumptionValidator>();//上报物料消耗
            services.AddSingleton<AbstractValidator<SingleBarCodeLoadingVerificationRequest>, SingleBarCodeLoadingVerificationValidator>();//单体条码上料校验
            services.AddSingleton<AbstractValidator<OutPutQtyRequest>, OutPutQtyValidator>();//产出上报数量
            services.AddSingleton<AbstractValidator<QueryContainerBindSfcRequest>, QueryContainerBindSfcValidator>();//容器绑定条码查询
        }

    }
}
