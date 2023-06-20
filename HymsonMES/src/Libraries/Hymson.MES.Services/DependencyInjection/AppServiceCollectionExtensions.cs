using FluentValidation;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Dtos.Warehouse;
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
            services.AddExcelService();
            services.AddMinioService(configuration);
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
            services.AddScoped<IEquConsumableService, EquConsumableService>();
            services.AddScoped<IEquConsumableTypeService, EquConsumableTypeService>();
            services.AddScoped<IEquEquipmentService, EquEquipmentService>();
            services.AddScoped<IEquEquipmentGroupService, EquEquipmentGroupService>();
            services.AddScoped<IEquEquipmentUnitService, EquEquipmentUnitService>();
            services.AddScoped<IEquFaultPhenomenonService, EquFaultPhenomenonService>();
            services.AddScoped<IEquSparePartService, EquSparePartService>();
            services.AddScoped<IEquSparePartTypeService, EquSparePartTypeService>();

            #region FaultReason
            services.AddScoped<IEquFaultReasonService, EquFaultReasonService>();

            #endregion
            #endregion

            #region Integrated
            services.AddScoped<IInteCalendarService, InteCalendarService>();
            services.AddScoped<IInteClassService, InteClassService>();
            services.AddScoped<IInteJobService, InteJobService>();
            services.AddScoped<IInteContainerService, InteContainerService>();
            services.AddScoped<IInteWorkCenterService, InteWorkCenterService>();
            services.AddScoped<IInteSystemTokenService, InteSystemTokenService>();

            #region CodeRule
            services.AddScoped<IInteCodeRulesService, InteCodeRulesService>();
            services.AddScoped<IInteCodeRulesMakeService, InteCodeRulesMakeService>();
            #endregion
            #endregion

            #region Process
            services.AddScoped<IProcMaskCodeService, ProcMaskCodeService>();

            #region Material
            services.AddScoped<IProcMaterialService, ProcMaterialService>();
            services.AddScoped<IProcMaterialGroupService, ProcMaterialGroupService>();
            #endregion

            #region Parameter
            services.AddScoped<IProcParameterService, ProcParameterService>();
            #endregion

            #region ParameterLinkType
            services.AddScoped<IProcParameterLinkTypeService, ProcParameterLinkTypeService>();
            #endregion

            #region Bom
            services.AddScoped<IProcBomService, ProcBomService>();
            services.AddScoped<IProcBomDetailService, ProcBomDetailService>();
            #endregion

            #region LoadPoint
            services.AddScoped<IProcLoadPointService, ProcLoadPointService>();
            #endregion

            #region Resource
            services.AddScoped<IProcResourceTypeService, ProcResourceTypeService>();
            services.AddScoped<IProcResourceService, ProcResourceService>();
            #endregion

            //工序
            services.AddScoped<IProcProcedureService, ProcProcedureService>();
            //工艺路线
            services.AddScoped<IProcProcessRouteService, ProcProcessRouteService>();

            services.AddScoped<IProcPrintConfigService, ProcPrintConfigService>();
            //标签模板
            services.AddScoped<IProcLabelTemplateService, ProcLabelTemplateService>();
            #endregion

            #region Quality
            services.AddScoped<IQualUnqualifiedCodeService, QualUnqualifiedCodeService>();
            services.AddScoped<IQualUnqualifiedGroupService, QualUnqualifiedGroupService>();
            #endregion

            #region Manufacture
            services.AddScoped<IManuCommonOldService, ManuCommonOldService>();
            services.AddScoped<IManuFeedingService, ManuFeedingService>();
            services.AddScoped<IManuSfcService, ManuSfcService>();
            services.AddScoped<IManuSfcProduceService, ManuSfcProduceService>();
            services.AddScoped<IManuCreateBarcodeService, ManuCreateBarcodeService>();
            services.AddScoped<IManuGenerateBarcodeService, ManuGenerateBarcodeService>();
            services.AddScoped<IManuProductBadRecordService, ManuProductBadRecordService>();
            services.AddScoped<IManuFacePlateService, ManuFacePlateService>();
            services.AddScoped<IManuFacePlateButtonService, ManuFacePlateButtonService>();

            services.AddScoped<IManuRepairService, ManuRepairService>();
            services.AddScoped<IManuInStationService, ManuInStationService>();
            services.AddScoped<IManuOutStationService, ManuOutStationService>();
            services.AddScoped<IInProductDismantleService, InProductDismantleService>();
            services.AddScoped<IManuFacePlateRepairService, ManuFacePlateRepairService>();

            services.AddScoped<IManuContainerBarcodeService, ManuContainerBarcodeService>();
            services.AddScoped<IManuContainerPackService, ManuContainerPackService>();
            services.AddScoped<IManuContainerPackRecordService, ManuContainerPackRecordService>();

            services.AddScoped<IManuFacePlateProductionService, ManuFacePlateProductionService>();

            #endregion

            #region Warehouse 
            services.AddScoped<IWhSupplierService, WhSupplierService>();
            services.AddScoped<IWhMaterialInventoryService, WhMaterialInventoryService>();
            services.AddScoped<IWhMaterialStandingbookService, WhMaterialStandingbookService>();

            #endregion

            #region Plan
            #region PlanWorkOrder
            services.AddScoped<IPlanWorkOrderService, PlanWorkOrderService>();
            #endregion

            #region PlanSfcReceive
            services.AddScoped<IPlanSfcReceiveService, PlanSfcReceiveService>();
            #endregion

            #region PlanWorkOrderActivation
            services.AddScoped<IPlanWorkOrderActivationService, PlanWorkOrderActivationService>();
            #endregion

            #region PlanSfcPrint
            services.AddScoped<IPlanSfcPrintService, PlanSfcPrintService>();
            #endregion

            #region PlanWorkOrderBind
            services.AddScoped<IPlanWorkOrderBindService, PlanWorkOrderBindService>();
            #endregion
            #endregion

            #region Job
            services.AddScoped<IJobManufactureService, JobManuBadRecordService>();
            services.AddScoped<IJobManufactureService, JobManuCompleteService>();
            services.AddScoped<IJobManufactureService, JobManuPackageService>();
            services.AddScoped<IJobManufactureService, JobManuRepairEndService>();
            services.AddScoped<IJobManufactureService, JobManuRepairStartService>();
            services.AddScoped<IJobManufactureService, JobManuStartService>();
            services.AddScoped<IJobManufactureService, JobManuStopService>();
            services.AddScoped<IJobManufactureService, JobManuPackageCloseService>();
            services.AddScoped<IJobManufactureService, JobManuPackageOpenService>();
            services.AddScoped<IJobManufactureService, JobManuPackageIngService>();
            #endregion

            #region Report
            #region BadRecordReport
            services.AddScoped<IBadRecordReportService, BadRecordReportService>();
            #endregion

            #region BadRecordReport
            services.AddScoped<IWorkshopJobControlReportService, WorkshopJobControlReportService>();
            #endregion

            #region Packaging
            services.AddScoped<IPackagingReportService, PackagingReportService>();
            #endregion

            #region OriginalSummary
            services.AddScoped<IOriginalSummaryReportService, OriginalSummaryReportService>();
            #endregion

            #region ComUsage
            services.AddScoped<IComUsageReportService, ComUsageReportService>();
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
            //services.Configure<TestOptions>(configuration.GetSection(nameof(TestOptions)));
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
            services.AddScoped<AbstractValidator<EquEquipmentSaveDto>, EquEquipmentValidator>();
            services.AddScoped<AbstractValidator<EquEquipmentGroupSaveDto>, EquEquipmentGroupValidator>();
            services.AddScoped<AbstractValidator<EquEquipmentUnitSaveDto>, EquipmentUnitCreateValidator>();
            services.AddScoped<AbstractValidator<EquFaultPhenomenonSaveDto>, EquFaultPhenomenonValidator>();
            services.AddScoped<AbstractValidator<EquFaultReasonSaveDto>, EquFaultReasonCreateValidator>();
            #endregion

            #region Process
            #region Material
            services.AddScoped<AbstractValidator<ProcMaterialCreateDto>, ProcMaterialCreateValidator>();
            services.AddScoped<AbstractValidator<ProcMaterialModifyDto>, ProcMaterialModifyValidator>();

            services.AddScoped<AbstractValidator<ProcMaterialSupplierRelationCreateDto>, ProcMaterialSupplierRelationCreateValidator>();
            services.AddScoped<AbstractValidator<ProcMaterialSupplierRelationModifyDto>, ProcMaterialSupplierRelationModifyValidator>();
            #endregion

            #region Parameter
            services.AddScoped<AbstractValidator<ProcParameterCreateDto>, ProcParameterCreateValidator>();
            services.AddScoped<AbstractValidator<ProcParameterModifyDto>, ProcParameterModifyValidator>();
            #endregion

            #region ParameterLinkType
            services.AddScoped<AbstractValidator<ProcParameterLinkTypeCreateDto>, ProcParameterLinkTypeCreateValidator>();
            services.AddScoped<AbstractValidator<ProcParameterLinkTypeModifyDto>, ProcParameterLinkTypeModifyValidator>();
            #endregion

            #region Bom
            services.AddScoped<AbstractValidator<ProcBomCreateDto>, ProcBomCreateValidator>();
            services.AddScoped<AbstractValidator<ProcBomModifyDto>, ProcBomModifyValidator>();

            services.AddScoped<AbstractValidator<ProcBomDetailCreateDto>, ProcBomDetailCreateValidator>();
            services.AddScoped<AbstractValidator<ProcBomDetailModifyDto>, ProcBomDetailModifyValidator>();
            #endregion

            #region LoadPoint
            services.AddScoped<AbstractValidator<ProcLoadPointCreateDto>, ProcLoadPointCreateValidator>();
            services.AddScoped<AbstractValidator<ProcLoadPointModifyDto>, ProcLoadPointModifyValidator>();
            #endregion

            #region Resource
            services.AddScoped<AbstractValidator<ProcResourceCreateDto>, ProcResourceCreateValidator>();
            services.AddScoped<AbstractValidator<ProcResourceModifyDto>, ProcResourcelModifyValidator>();
            #endregion

            #region ResourceType
            services.AddScoped<AbstractValidator<ProcResourceTypeAddDto>, ProcResourceTypeCreateValidator>();
            services.AddScoped<AbstractValidator<ProcResourceTypeUpdateDto>, ProcResourceTypeModifyValidator>();
            #endregion

            #region Procedure
            services.AddScoped<AbstractValidator<ProcProcedureCreateDto>, ProcProcedureCreateValidator>();
            services.AddScoped<AbstractValidator<ProcProcedureModifyDto>, ProcProcedureModifyValidator>();
            #endregion

            #region ProcessRoute
            services.AddScoped<AbstractValidator<ProcProcessRouteCreateDto>, ProcProcessRouteCreateValidator>();
            services.AddScoped<AbstractValidator<ProcProcessRouteModifyDto>, ProcProcessRouteModifyValidator>();
            #endregion

            #region LabelTemplate

            services.AddScoped<AbstractValidator<ProcLabelTemplateCreateDto>, ProcLabelTemplateCreateValidator>();
            services.AddScoped<AbstractValidator<ProcLabelTemplateModifyDto>, ProcLabelTemplateModifyValidator>();
            services.AddScoped<AbstractValidator<ProcPrinterDto>, ProcPrinterCreateValidator>();
            services.AddScoped<AbstractValidator<ProcPrinterUpdateDto>, ProcPrinterModifyValidator>();
            #endregion

            #region MaskCode
            services.AddScoped<AbstractValidator<ProcMaskCodeSaveDto>, ProcMaskCodeValidator>();
            #endregion

            #endregion

            #region Integrated
            services.AddScoped<AbstractValidator<InteContainerSaveDto>, InteContainerValidator>();
            services.AddScoped<AbstractValidator<InteClassSaveDto>, InteClassSaveValidator>();
            services.AddScoped<AbstractValidator<InteJobCreateDto>, InteJobCreateValidator>();
            services.AddScoped<AbstractValidator<InteJobModifyDto>, InteJobModifyValidator>();
            services.AddScoped<AbstractValidator<InteWorkCenterCreateDto>, InteWorkCenterCreateValidator>();
            services.AddScoped<AbstractValidator<InteWorkCenterModifyDto>, InteWorkCenterModifyValidator>();
            services.AddScoped<AbstractValidator<InteSystemTokenCreateDto>, InteSystemTokenCreateValidator>();
            services.AddScoped<AbstractValidator<InteSystemTokenModifyDto>, InteSystemTokenModifyValidator>();

            #region CodeRule
            services.AddScoped<AbstractValidator<InteCodeRulesCreateDto>, InteCodeRulesCreateValidator>();
            services.AddScoped<AbstractValidator<InteCodeRulesModifyDto>, InteCodeRulesModifyValidator>();

            services.AddScoped<AbstractValidator<InteCodeRulesMakeCreateDto>, InteCodeRulesMakeCreateValidator>();
            services.AddScoped<AbstractValidator<InteCodeRulesMakeModifyDto>, InteCodeRulesMakeModifyValidator>();
            #endregion
            #endregion

            #region Quality
            services.AddScoped<AbstractValidator<QualUnqualifiedCodeCreateDto>, QualUnqualifiedCodeCreateValidator>();
            services.AddScoped<AbstractValidator<QualUnqualifiedCodeModifyDto>, QualUnqualifiedCodeModifyValidator>();
            services.AddScoped<AbstractValidator<QualUnqualifiedGroupCreateDto>, QualUnqualifiedGroupCreateValidator>();
            services.AddScoped<AbstractValidator<QualUnqualifiedGroupModifyDto>, QualUnqualifiedGroupModifyValidator>();
            #endregion

            #region Manufacture

            services.AddScoped<AbstractValidator<ManuSfcProduceLockDto>, ManuSfcProduceLockValidator>();
            services.AddScoped<AbstractValidator<ManuSfcProduceModifyDto>, ManuSfcProduceModifyValidator>();

            services.AddScoped<AbstractValidator<ManuSfcInfoCreateDto>, ManuSfcInfoCreateValidator>();
            services.AddScoped<AbstractValidator<ManuSfcInfoModifyDto>, ManuSfcInfoModifyValidator>();

            services.AddScoped<AbstractValidator<ManuProductBadRecordCreateDto>, ManuProductBadRecordCreateValidator>();
            services.AddScoped<AbstractValidator<ManuProductBadRecordModifyDto>, ManuProductBadRecordModifyValidator>();

            services.AddScoped<AbstractValidator<ManuFacePlateCreateDto>, ManuFacePlateCreateValidator>();
            services.AddScoped<AbstractValidator<ManuFacePlateModifyDto>, ManuFacePlateModifyValidator>();

            services.AddScoped<AbstractValidator<ManuFacePlateButtonCreateDto>, ManuFacePlateButtonCreateValidator>();
            services.AddScoped<AbstractValidator<ManuFacePlateButtonModifyDto>, ManuFacePlateButtonModifyValidator>();

            services.AddScoped<AbstractValidator<ManuFacePlateProductionCreateDto>, ManuFacePlateProductionCreateValidator>();
            services.AddScoped<AbstractValidator<ManuFacePlateProductionModifyDto>, ManuFacePlateProductionModifyValidator>();

            services.AddScoped<AbstractValidator<ManuFacePlateRepairCreateDto>, ManuFacePlateRepairCreateValidator>();
            services.AddScoped<AbstractValidator<ManuFacePlateRepairModifyDto>, ManuFacePlateRepairModifyValidator>();

            services.AddScoped<AbstractValidator<ManuFacePlateContainerPackCreateDto>, ManuFacePlateContainerPackCreateValidator>();
            services.AddScoped<AbstractValidator<ManuFacePlateContainerPackModifyDto>, ManuFacePlateContainerPackModifyValidator>();

            services.AddScoped<AbstractValidator<ManuContainerBarcodeCreateDto>, ManuContainerBarcodeCreateValidator>();
            services.AddScoped<AbstractValidator<ManuContainerBarcodeModifyDto>, ManuContainerBarcodeModifyValidator>();
            services.AddScoped<AbstractValidator<CreateManuContainerBarcodeDto>, CreateManuContainerBarcodeValidator>();
            services.AddScoped<AbstractValidator<UpdateManuContainerBarcodeStatusDto>, UpdateManuContainerBarcodeStatusValidator>();

            services.AddScoped<AbstractValidator<ManuContainerPackRecordCreateDto>, ManuContainerPackRecordCreateValidator>();
            services.AddScoped<AbstractValidator<ManuContainerPackRecordModifyDto>, ManuContainerPackRecordModifyValidator>();
            services.AddScoped<AbstractValidator<ManuContainerPackCreateDto>, ManuContainerPackCreateValidator>();
            services.AddScoped<AbstractValidator<ManuContainerPackModifyDto>, ManuContainerPackModifyValidator>();
            #endregion

            #region Warehouse 

            services.AddScoped<AbstractValidator<WhSupplierCreateDto>, WhSupplierCreateValidator>();
            services.AddScoped<AbstractValidator<WhSupplierModifyDto>, WhSupplierModifyValidator>();


            services.AddScoped<AbstractValidator<WhMaterialInventoryCreateDto>, WhMaterialInventoryCreateValidator>();
            services.AddScoped<AbstractValidator<WhMaterialInventoryModifyDto>, WhMaterialInventoryModifyValidator>();

            services.AddScoped<AbstractValidator<WhMaterialStandingbookCreateDto>, WhMaterialStandingbookCreateValidator>();
            services.AddScoped<AbstractValidator<WhMaterialStandingbookModifyDto>, WhMaterialStandingbookModifyValidator>();


            #endregion

            #region Plan
            #region PlanWorkOrder
            services.AddScoped<AbstractValidator<PlanWorkOrderCreateDto>, PlanWorkOrderCreateValidator>();
            services.AddScoped<AbstractValidator<PlanWorkOrderModifyDto>, PlanWorkOrderModifyValidator>();
            #endregion

            #region PlanSfcReceive
            services.AddScoped<AbstractValidator<PlanSfcReceiveCreateDto>, PlanSfcReceiveCreateValidator>();
            services.AddScoped<AbstractValidator<PlanSfcReceiveScanCodeDto>, PlanSfcReceiveModifyValidator>();
            #endregion

            #region PlanWorkOrderActivation
            services.AddScoped<AbstractValidator<PlanWorkOrderActivationCreateDto>, PlanWorkOrderActivationCreateValidator>();
            services.AddScoped<AbstractValidator<PlanWorkOrderActivationModifyDto>, PlanWorkOrderActivationModifyValidator>();
            #endregion

            #region PlanSfcPrint
            services.AddScoped<AbstractValidator<PlanSfcPrintCreateDto>, PlanSfcPrintCreateValidator>();
            services.AddScoped<AbstractValidator<PlanSfcPrintCreatePrintDto>, PlanSfcPrintCreatePrintValidator>();
            #endregion

            #region PlanWorkOrderBind 
            services.AddScoped<AbstractValidator<PlanWorkOrderBindCreateDto>, PlanWorkOrderBindCreateValidator>();
            services.AddScoped<AbstractValidator<PlanWorkOrderBindModifyDto>, PlanWorkOrderBindModifyValidator>();

            #endregion

            #endregion

            return services;
        }
       
    }
}
