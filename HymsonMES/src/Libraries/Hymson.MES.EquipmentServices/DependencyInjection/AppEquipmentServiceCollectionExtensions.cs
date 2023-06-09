using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos.BindContainer;
using Hymson.MES.EquipmentServices.Dtos.BindSFC;
using Hymson.MES.EquipmentServices.Dtos.CCDFileUploadComplete;
using Hymson.MES.EquipmentServices.Dtos.FeedingConsumption;
using Hymson.MES.EquipmentServices.Dtos.GenerateModuleSFC;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Dtos.InboundInContainer;
using Hymson.MES.EquipmentServices.Dtos.InboundInSFCContainer;
using Hymson.MES.EquipmentServices.Dtos.OutBound;
using Hymson.MES.EquipmentServices.Dtos.OutPutQty;
using Hymson.MES.EquipmentServices.Dtos.QueryContainerBindSfc;
using Hymson.MES.EquipmentServices.Dtos.SfcCirculation;
using Hymson.MES.EquipmentServices.Dtos.SingleBarCodeLoadingVerification;
using Hymson.MES.EquipmentServices.Services;
using Hymson.MES.EquipmentServices.Services.BindContainer;
using Hymson.MES.EquipmentServices.Services.BindSFC;
using Hymson.MES.EquipmentServices.Services.CCDFileUploadComplete;
using Hymson.MES.EquipmentServices.Services.EquipmentCollect;
using Hymson.MES.EquipmentServices.Services.Feeding;
using Hymson.MES.EquipmentServices.Services.GenerateModuleSFC;
using Hymson.MES.EquipmentServices.Services.InBound;
using Hymson.MES.EquipmentServices.Services.InboundInContainer;
using Hymson.MES.EquipmentServices.Services.InboundInSFCContainer;
using Hymson.MES.EquipmentServices.Services.OutBound;
using Hymson.MES.EquipmentServices.Services.OutPutQty;
using Hymson.MES.EquipmentServices.Services.QueryContainerBindSfc;
using Hymson.MES.EquipmentServices.Services.SfcCirculation;
using Hymson.MES.EquipmentServices.Services.SingleBarCodeLoadingVerification;
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
using Hymson.MES.EquipmentServices.Validators.SfcCirculation;
using Hymson.MES.EquipmentServices.Validators.SingleBarCodeLoadingVerification;
using FluentValidation;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.EquipmentServices;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Services.Common;
using Hymson.MES.EquipmentServices.Services.Job.Implementing;
using Hymson.MES.EquipmentServices.Services.Manufacture.InStation;
using Hymson.MES.EquipmentServices.Services.SfcBinding;
using Hymson.MES.EquipmentServices.Validators.InStation;
using Hymson.MES.EquipmentServices.Validators.SfcBinding;
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
            services.AddSingleton<IInboundInContainerService, InboundInContainerService>();// 进站-容器
            services.AddSingleton<IGenerateModuleSFCService, GenerateModuleSFCService>();//请求生成模组码-电芯堆叠
            services.AddSingleton<IInboundInSFCContainerService, InboundInSFCContainerService>();//进站-电芯和托盘-装盘2
            services.AddSingleton<ICCDFileUploadCompleteService, CCDFileUploadCompleteService>();//CCD文件上传完成
            services.AddSingleton<IFeedingConsumptionService, FeedingConsumptionService>();//上报物料消耗
            services.AddSingleton<ISingleBarCodeLoadingVerificationService, SingleBarCodeLoadingVerificationService>();//单体条码上料校验
            services.AddSingleton<IOutPutQtyService, OutPutQtyService>();//产出上报数量
            services.AddSingleton<IQueryContainerBindSfcService, QueryContainerBindSfcService>();//容器绑定条码查询
            services.AddSingleton<IEquipmentCollectService, EquipmentCollectService>();   // 设备信息采集
            services.AddSingleton<IFeedingService, FeedingService>();      // 上卸料
            services.AddSingleton<INGDataService, NGDataService>();//NG数据
            services.AddSingleton<ISfcCirculationService, SfcCirculationService>();//流转表绑定
            services.AddSingleton<ICommonService, CommonService>();
            services.AddSingleton<IInStationService, InStationService>();
            services.AddSingleton<IJobManufactureService, JobManuSfcConvertService>();
            services.AddSingleton<ISfcBindingService, SfcBindingService>();

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
            services.AddSingleton<AbstractValidator<BindSFCDto>, BindSFCValidator>();//条码绑定
            services.AddSingleton<AbstractValidator<UnBindSFCDto>, UnBindSFCValidator>();//条码解绑
            services.AddSingleton<AbstractValidator<BindContainerDto>, BindContainerValidator>();//容器绑定
            services.AddSingleton<AbstractValidator<UnBindContainerDto>, UnBindContainerValidator>();//容器解绑
            services.AddSingleton<AbstractValidator<InBoundDto>, InBoundValidator>();//进站
            services.AddSingleton<AbstractValidator<InBoundMoreDto>, InBoundMoreValidator>();//进站（多个）
            services.AddSingleton<AbstractValidator<OutBoundDto>, OutBoundValidator>();//出站
            services.AddSingleton<AbstractValidator<OutBoundMoreDto>, OutBoundMoreValidator>();//出站（多个）
            services.AddSingleton<AbstractValidator<SfcCirculationBindDto>, SfcCirculationBindValidator>();//条码流转绑定
            services.AddSingleton<AbstractValidator<SfcCirculationUnBindDto>, SfcCirculationUnBindValidator>();//条码流转解绑



            services.AddSingleton<AbstractValidator<InboundInContainerDto>, InboundInContainerValidator>();// 进站-容器
            services.AddSingleton<AbstractValidator<GenerateModuleSFCDto>, GenerateModuleSFCValidator>();//请求生成模组码-电芯堆叠
            services.AddSingleton<AbstractValidator<InboundInSFCContainerDto>, InboundInSFCContainerValidator>();//进站-电芯和托盘-装盘2
            services.AddSingleton<AbstractValidator<CCDFileUploadCompleteDto>, CCDFileUploadCompleteValidator>();//CCD文件上传完成
            services.AddSingleton<AbstractValidator<FeedingConsumptionDto>, FeedingConsumptionValidator>();//上报物料消耗
            services.AddSingleton<AbstractValidator<SingleBarCodeLoadingVerificationDto>, SingleBarCodeLoadingVerificationValidator>();//单体条码上料校验
            services.AddSingleton<AbstractValidator<OutPutQtyDto>, OutPutQtyValidator>();//产出上报数量
            services.AddSingleton<AbstractValidator<QueryContainerBindSfcDto>, QueryContainerBindSfcValidator>();//容器绑定条码查询
            services.AddSingleton<AbstractValidator<InStationDto>, InStationValidator>();
            services.AddSingleton<AbstractValidator<SfcBindingDto>, SfcBindingValidator>();
        }

    }
}
