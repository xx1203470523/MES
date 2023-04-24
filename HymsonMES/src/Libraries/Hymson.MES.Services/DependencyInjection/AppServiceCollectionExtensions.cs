using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.MES.Services.Options;
using Hymson.MES.Services.Services.EquEquipmentGroup;
using Hymson.MES.Services.Services.Equipment;
using Hymson.MES.Services.Services.Equipment.EquEquipment;
using Hymson.MES.Services.Services.Equipment.EquEquipmentUnit;
using Hymson.MES.Services.Services.Equipment.EquFaultPhenomenon;
using Hymson.MES.Services.Services.Equipment.EquSparePart;
using Hymson.MES.Services.Services.Equipment.EquSparePartType;
using Hymson.MES.Services.Services.Integrated;
using Hymson.MES.Services.Services.Integrated.IIntegratedService;
using Hymson.MES.Services.Services.Integrated.InteCalendar;
using Hymson.MES.Services.Services.Integrated.InteClass;
using Hymson.MES.Services.Services.Integrated.InteContainer;
using Hymson.MES.Services.Services.Job.Common;
using Hymson.MES.Services.Services.Job.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuFeeding;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.GenerateBarcode;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCreateBarcode;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuInStation;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuOutStation;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuPackage;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation;
using Hymson.MES.Services.Services.Manufacture.ManuSfc;
using Hymson.MES.Services.Services.Manufacture.ManuSfcProduce;
using Hymson.MES.Services.Services.Plan;
using Hymson.MES.Services.Services.Process;
using Hymson.MES.Services.Services.Process.LabelTemplate;
using Hymson.MES.Services.Services.Process.MaskCode;
using Hymson.MES.Services.Services.Process.PrintConfig;
using Hymson.MES.Services.Services.Process.Procedure;
using Hymson.MES.Services.Services.Process.ProcessRoute;
using Hymson.MES.Services.Services.Process.Resource;
using Hymson.MES.Services.Services.Process.ResourceType;
using Hymson.MES.Services.Services.Quality;
using Hymson.MES.Services.Services.Quality.IQualityService;
using Hymson.MES.Services.Services.Report;
using Hymson.MES.Services.Services.Warehouse;
using Hymson.MES.Services.Validators.Equipment;
using Hymson.MES.Services.Validators.Integrated;
using Hymson.MES.Services.Validators.Manufacture;
using Hymson.MES.Services.Validators.Plan;
using Hymson.MES.Services.Validators.Process;
using Hymson.MES.Services.Validators.Quality;
using Hymson.MES.Services.Validators.Warehouse;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;

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
        /// <returns></returns>
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            #region Equipment
            services.AddSingleton<IEquConsumableService, EquConsumableService>();
            services.AddSingleton<IEquConsumableTypeService, EquConsumableTypeService>();
            services.AddSingleton<IEquEquipmentService, EquEquipmentService>();
            services.AddSingleton<IEquEquipmentGroupService, EquEquipmentGroupService>();
            services.AddSingleton<IEquEquipmentUnitService, EquEquipmentUnitService>();
            services.AddSingleton<IEquFaultPhenomenonService, EquFaultPhenomenonService>();
            services.AddSingleton<IEquSparePartService, EquSparePartService>();
            services.AddSingleton<IEquSparePartTypeService, EquSparePartTypeService>();

            #region FaultReason
            services.AddSingleton<IEquFaultReasonService, EquFaultReasonService>();

            #endregion
            #endregion

            #region Integrated
            services.AddSingleton<IInteCalendarService, InteCalendarService>();
            services.AddSingleton<IInteClassService, InteClassService>();
            services.AddSingleton<IInteJobService, InteJobService>();
            services.AddSingleton<IInteContainerService, InteContainerService>();
            services.AddSingleton<IInteWorkCenterService, InteWorkCenterService>();

            #region CodeRule
            services.AddSingleton<IInteCodeRulesService, InteCodeRulesService>();
            services.AddSingleton<IInteCodeRulesMakeService, InteCodeRulesMakeService>();
            #endregion
            #endregion

            #region Process
            services.AddSingleton<IProcMaskCodeService, ProcMaskCodeService>();

            #region Material
            services.AddSingleton<IProcMaterialService, ProcMaterialService>();
            services.AddSingleton<IProcMaterialGroupService, ProcMaterialGroupService>();
            #endregion

            #region Parameter
            services.AddSingleton<IProcParameterService, ProcParameterService>();
            #endregion

            #region ParameterLinkType
            services.AddSingleton<IProcParameterLinkTypeService, ProcParameterLinkTypeService>();
            #endregion

            #region Bom
            services.AddSingleton<IProcBomService, ProcBomService>();
            services.AddSingleton<IProcBomDetailService, ProcBomDetailService>();
            #endregion

            #region LoadPoint
            services.AddSingleton<IProcLoadPointService, ProcLoadPointService>();
            #endregion

            #region Resource
            services.AddSingleton<IProcResourceTypeService, ProcResourceTypeService>();
            services.AddSingleton<IProcResourceService, ProcResourceService>();
            #endregion

            //工序
            services.AddSingleton<IProcProcedureService, ProcProcedureService>();
            //工艺路线
            services.AddSingleton<IProcProcessRouteService, ProcProcessRouteService>();

            services.AddSingleton<IProcPrintConfigService, ProcPrintConfigService>();
            //标签模板
            services.AddSingleton<IProcLabelTemplateService, ProcLabelTemplateService>();
            #endregion

            #region Quality
            services.AddSingleton<IQualUnqualifiedCodeService, QualUnqualifiedCodeService>();
            services.AddSingleton<IQualUnqualifiedGroupService, QualUnqualifiedGroupService>();
            #endregion

            #region Manufacture
            services.AddSingleton<IManuCommonService, ManuCommonService>();
            services.AddSingleton<IManuFeedingService, ManuFeedingService>();
            services.AddSingleton<IManuSfcService, ManuSfcService>();
            services.AddSingleton<IManuSfcProduceService, ManuSfcProduceService>();
            services.AddSingleton<IManuCreateBarcodeService, ManuCreateBarcodeService>();
            services.AddSingleton<IManuGenerateBarcodeService, ManuGenerateBarcodeService>();
            services.AddSingleton<IManuProductBadRecordService, ManuProductBadRecordService>();
            services.AddSingleton<IManuFacePlateService, ManuFacePlateService>();
            services.AddSingleton<IManuFacePlateButtonService, ManuFacePlateButtonService>();

            services.AddSingleton<IManuRepairService, ManuRepairService>();
            services.AddSingleton<IManuInStationService, ManuInStationService>();
            services.AddSingleton<IManuOutStationService, ManuOutStationService>();
            services.AddSingleton<IManuPackageService, ManuPackageService>();
            services.AddSingleton<IInProductDismantleService, InProductDismantleService>();
            services.AddSingleton<IManuFacePlateRepairService, ManuFacePlateRepairService>();

            services.AddSingleton<IManuContainerBarcodeService, ManuContainerBarcodeService>();
            services.AddSingleton<IManuContainerPackService, ManuContainerPackService>();
            services.AddSingleton<IManuContainerPackRecordService, ManuContainerPackRecordService>();

            services.AddSingleton<IManuFacePlateProductionService, ManuFacePlateProductionService>();

            #endregion

            #region Warehouse 
            services.AddSingleton<IWhSupplierService, WhSupplierService>();
            services.AddSingleton<IWhMaterialInventoryService, WhMaterialInventoryService>();
            services.AddSingleton<IWhMaterialStandingbookService, WhMaterialStandingbookService>();

            #endregion

            #region Plan
            #region PlanWorkOrder
            services.AddSingleton<IPlanWorkOrderService, PlanWorkOrderService>();
            #endregion

            #region PlanSfcReceive
            services.AddSingleton<IPlanSfcReceiveService, PlanSfcReceiveService>();
            #endregion

            #region PlanWorkOrderActivation
            services.AddSingleton<IPlanWorkOrderActivationService, PlanWorkOrderActivationService>();
            #endregion

            #region PlanSfcPrint
            services.AddSingleton<IPlanSfcPrintService, PlanSfcPrintService>();
            #endregion

            #region PlanWorkOrderBind
            services.AddSingleton<IPlanWorkOrderBindService, PlanWorkOrderBindService>();
            #endregion
            #endregion

            #region Job
            services.AddSingleton<IJobCommonService, JobCommonService>();
            services.AddSingleton<IJobManufactureService, JobManuBadRecordService>();
            services.AddSingleton<IJobManufactureService, JobManuCompleteService>();
            services.AddSingleton<IJobManufactureService, JobManuPackageService>();
            services.AddSingleton<IJobManufactureService, JobManuRepairEndService>();
            services.AddSingleton<IJobManufactureService, JobManuRepairStartService>();
            services.AddSingleton<IJobManufactureService, JobManuStartService>();
            services.AddSingleton<IJobManufactureService, JobManuStopService>();
            #endregion

            #region Report
            #region BadRecordReport
            services.AddSingleton<IBadRecordReportService, BadRecordReportService>();
            #endregion

            #region Packaging
            services.AddSingleton<IPackagingReportService, PackagingReportService>();
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
            services.AddSingleton<AbstractValidator<EquEquipmentSaveDto>, EquEquipmentValidator>();
            services.AddSingleton<AbstractValidator<EquEquipmentGroupSaveDto>, EquEquipmentGroupValidator>();
            services.AddSingleton<AbstractValidator<EquEquipmentUnitSaveDto>, EquipmentUnitCreateValidator>();
            services.AddSingleton<AbstractValidator<EquFaultPhenomenonSaveDto>, EquFaultPhenomenonValidator>();
            services.AddSingleton<AbstractValidator<EquFaultReasonSaveDto>, EquFaultReasonCreateValidator>();
            #endregion

            #region Process
            #region Material
            services.AddSingleton<AbstractValidator<ProcMaterialCreateDto>, ProcMaterialCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcMaterialModifyDto>, ProcMaterialModifyValidator>();

            services.AddSingleton<AbstractValidator<ProcMaterialGroupCreateDto>, ProcMaterialGroupCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcMaterialGroupModifyDto>, ProcMaterialGroupModifyValidator>();

            services.AddSingleton<AbstractValidator<ProcMaterialSupplierRelationCreateDto>, ProcMaterialSupplierRelationCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcMaterialSupplierRelationModifyDto>, ProcMaterialSupplierRelationModifyValidator>();
            #endregion

            #region Parameter
            services.AddSingleton<AbstractValidator<ProcParameterCreateDto>, ProcParameterCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcParameterModifyDto>, ProcParameterModifyValidator>();
            #endregion

            #region ParameterLinkType
            services.AddSingleton<AbstractValidator<ProcParameterLinkTypeCreateDto>, ProcParameterLinkTypeCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcParameterLinkTypeModifyDto>, ProcParameterLinkTypeModifyValidator>();
            #endregion

            #region Bom
            services.AddSingleton<AbstractValidator<ProcBomCreateDto>, ProcBomCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcBomModifyDto>, ProcBomModifyValidator>();

            services.AddSingleton<AbstractValidator<ProcBomDetailCreateDto>, ProcBomDetailCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcBomDetailModifyDto>, ProcBomDetailModifyValidator>();
            #endregion

            #region LoadPoint
            services.AddSingleton<AbstractValidator<ProcLoadPointCreateDto>, ProcLoadPointCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcLoadPointModifyDto>, ProcLoadPointModifyValidator>();
            #endregion

            #region Resource
            services.AddSingleton<AbstractValidator<ProcResourceCreateDto>, ProcResourceCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcResourceModifyDto>, ProcResourcelModifyValidator>();
            #endregion

            #region ResourceType
            services.AddSingleton<AbstractValidator<ProcResourceTypeAddDto>, ProcResourceTypeCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcResourceTypeUpdateDto>, ProcResourceTypeModifyValidator>();
            #endregion

            #region Procedure
            services.AddSingleton<AbstractValidator<ProcProcedureCreateDto>, ProcProcedureCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcProcedureModifyDto>, ProcProcedureModifyValidator>();
            #endregion

            #region ProcessRoute
            services.AddSingleton<AbstractValidator<ProcProcessRouteCreateDto>, ProcProcessRouteCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcProcessRouteModifyDto>, ProcProcessRouteModifyValidator>();
            #endregion

            #region LabelTemplate

            services.AddSingleton<AbstractValidator<ProcLabelTemplateCreateDto>, ProcLabelTemplateCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcLabelTemplateModifyDto>, ProcLabelTemplateModifyValidator>();
            services.AddSingleton<AbstractValidator<ProcPrinterDto>, ProcPrinterCreateValidator>();
            #endregion

            #region MaskCode
            services.AddSingleton<AbstractValidator<ProcMaskCodeSaveDto>, ProcMaskCodeValidator>();
            #endregion

            #endregion

            #region Integrated
            services.AddSingleton<AbstractValidator<InteContainerSaveDto>, InteContainerValidator>();
            services.AddSingleton<AbstractValidator<InteClassSaveDto>, InteClassSaveValidator>();
            services.AddSingleton<AbstractValidator<InteJobCreateDto>, InteJobCreateValidator>();
            services.AddSingleton<AbstractValidator<InteJobModifyDto>, InteJobModifyValidator>();
            services.AddSingleton<AbstractValidator<InteWorkCenterCreateDto>, InteWorkCenterCreateValidator>();
            services.AddSingleton<AbstractValidator<InteWorkCenterModifyDto>, InteWorkCenterModifyValidator>();

            #region CodeRule
            services.AddSingleton<AbstractValidator<InteCodeRulesCreateDto>, InteCodeRulesCreateValidator>();
            services.AddSingleton<AbstractValidator<InteCodeRulesModifyDto>, InteCodeRulesModifyValidator>();

            services.AddSingleton<AbstractValidator<InteCodeRulesMakeCreateDto>, InteCodeRulesMakeCreateValidator>();
            services.AddSingleton<AbstractValidator<InteCodeRulesMakeModifyDto>, InteCodeRulesMakeModifyValidator>();
            #endregion
            #endregion

            #region Quality
            services.AddSingleton<AbstractValidator<QualUnqualifiedCodeCreateDto>, QualUnqualifiedCodeCreateValidator>();
            services.AddSingleton<AbstractValidator<QualUnqualifiedCodeModifyDto>, QualUnqualifiedCodeModifyValidator>();
            services.AddSingleton<AbstractValidator<QualUnqualifiedGroupCreateDto>, QualUnqualifiedGroupCreateValidator>();
            services.AddSingleton<AbstractValidator<QualUnqualifiedGroupModifyDto>, QualUnqualifiedGroupModifyValidator>();
            #endregion

            #region Manufacture

            services.AddSingleton<AbstractValidator<ManuSfcProduceLockDto>, ManuSfcProduceLockValidator>();
            services.AddSingleton<AbstractValidator<ManuSfcProduceModifyDto>, ManuSfcProduceModifyValidator>();

            services.AddSingleton<AbstractValidator<ManuSfcInfoCreateDto>, ManuSfcInfoCreateValidator>();
            services.AddSingleton<AbstractValidator<ManuSfcInfoModifyDto>, ManuSfcInfoModifyValidator>();

            services.AddSingleton<AbstractValidator<ManuProductBadRecordCreateDto>, ManuProductBadRecordCreateValidator>();
            services.AddSingleton<AbstractValidator<ManuProductBadRecordModifyDto>, ManuProductBadRecordModifyValidator>();

            services.AddSingleton<AbstractValidator<ManuFacePlateCreateDto>, ManuFacePlateCreateValidator>();
            services.AddSingleton<AbstractValidator<ManuFacePlateModifyDto>, ManuFacePlateModifyValidator>();

            services.AddSingleton<AbstractValidator<ManuFacePlateButtonCreateDto>, ManuFacePlateButtonCreateValidator>();
            services.AddSingleton<AbstractValidator<ManuFacePlateButtonModifyDto>, ManuFacePlateButtonModifyValidator>();

            services.AddSingleton<AbstractValidator<ManuFacePlateProductionCreateDto>, ManuFacePlateProductionCreateValidator>();
            services.AddSingleton<AbstractValidator<ManuFacePlateProductionModifyDto>, ManuFacePlateProductionModifyValidator>();

            services.AddSingleton<AbstractValidator<ManuFacePlateRepairCreateDto>, ManuFacePlateRepairCreateValidator>();
            services.AddSingleton<AbstractValidator<ManuFacePlateRepairModifyDto>, ManuFacePlateRepairModifyValidator>();

            services.AddSingleton<AbstractValidator<ManuFacePlateContainerPackCreateDto>, ManuFacePlateContainerPackCreateValidator>();
            services.AddSingleton<AbstractValidator<ManuFacePlateContainerPackModifyDto>, ManuFacePlateContainerPackModifyValidator>();

            services.AddSingleton<AbstractValidator<ManuContainerBarcodeCreateDto>, ManuContainerBarcodeCreateValidator>();
            services.AddSingleton<AbstractValidator<ManuContainerBarcodeModifyDto>, ManuContainerBarcodeModifyValidator>();
            services.AddSingleton<AbstractValidator<CreateManuContainerBarcodeDto>, CreateManuContainerBarcodeValidator>();
            services.AddSingleton<AbstractValidator<UpdateManuContainerBarcodeStatusDto>, UpdateManuContainerBarcodeStatusValidator>();

            services.AddSingleton<AbstractValidator<ManuContainerPackRecordCreateDto>, ManuContainerPackRecordCreateValidator>();
            services.AddSingleton<AbstractValidator<ManuContainerPackRecordModifyDto>, ManuContainerPackRecordModifyValidator>();
            services.AddSingleton<AbstractValidator<ManuContainerPackCreateDto>, ManuContainerPackCreateValidator>();
            services.AddSingleton<AbstractValidator<ManuContainerPackModifyDto>, ManuContainerPackModifyValidator>();
            #endregion

            #region Warehouse 

            services.AddSingleton<AbstractValidator<WhSupplierCreateDto>, WhSupplierCreateValidator>();
            services.AddSingleton<AbstractValidator<WhSupplierModifyDto>, WhSupplierModifyValidator>();


            services.AddSingleton<AbstractValidator<WhMaterialInventoryCreateDto>, WhMaterialInventoryCreateValidator>();
            services.AddSingleton<AbstractValidator<WhMaterialInventoryModifyDto>, WhMaterialInventoryModifyValidator>();

            services.AddSingleton<AbstractValidator<WhMaterialStandingbookCreateDto>, WhMaterialStandingbookCreateValidator>();
            services.AddSingleton<AbstractValidator<WhMaterialStandingbookModifyDto>, WhMaterialStandingbookModifyValidator>();


            #endregion

            #region Plan
            #region PlanWorkOrder
            services.AddSingleton<AbstractValidator<PlanWorkOrderCreateDto>, PlanWorkOrderCreateValidator>();
            services.AddSingleton<AbstractValidator<PlanWorkOrderModifyDto>, PlanWorkOrderModifyValidator>();
            #endregion

            #region PlanSfcReceive
            services.AddSingleton<AbstractValidator<PlanSfcReceiveCreateDto>, PlanSfcReceiveCreateValidator>();
            services.AddSingleton<AbstractValidator<PlanSfcReceiveModifyDto>, PlanSfcReceiveModifyValidator>();
            #endregion

            #region PlanWorkOrderActivation
            services.AddSingleton<AbstractValidator<PlanWorkOrderActivationCreateDto>, PlanWorkOrderActivationCreateValidator>();
            services.AddSingleton<AbstractValidator<PlanWorkOrderActivationModifyDto>, PlanWorkOrderActivationModifyValidator>();
            #endregion

            #region PlanSfcPrint
            services.AddSingleton<AbstractValidator<PlanSfcPrintCreateDto>, PlanSfcPrintCreateValidator>();

            #endregion

            #region PlanWorkOrderBind 
            services.AddSingleton<AbstractValidator<PlanWorkOrderBindCreateDto>, PlanWorkOrderBindCreateValidator>();
            services.AddSingleton<AbstractValidator<PlanWorkOrderBindModifyDto>, PlanWorkOrderBindModifyValidator>();

            #endregion

            #endregion

            return services;
        }
       
    }
}
