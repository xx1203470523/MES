using FluentValidation;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.MES.Services.Dtos.WhWareHouse;
using Hymson.MES.Services.Dtos.WhWarehouseLocation;
using Hymson.MES.Services.Dtos.WhWarehouseRegion;
using Hymson.MES.Services.Dtos.WhWarehouseShelf;
using Hymson.MES.Services.Services;
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
using Hymson.MES.Services.Services.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuFeeding;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.GenerateBarcode;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
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
using Hymson.MES.Services.Services.Quality.QualUnqualifiedCode;
using Hymson.MES.Services.Services.Quality.QualUnqualifiedGroup;
using Hymson.MES.Services.Services.Report;
using Hymson.MES.Services.Services.Warehouse;
using Hymson.MES.Services.Services.WhWareHouse;
using Hymson.MES.Services.Services.WhWarehouseLocation;
using Hymson.MES.Services.Services.WhWarehouseRegion;
using Hymson.MES.Services.Services.WhWarehouseShelf;
using Hymson.MES.Services.Validators.Equipment;
using Hymson.MES.Services.Validators.Integrated;
using Hymson.MES.Services.Validators.Manufacture;
using Hymson.MES.Services.Validators.Plan;
using Hymson.MES.Services.Validators.Process;
using Hymson.MES.Services.Validators.Quality;
using Hymson.MES.Services.Validators.Warehouse;
using Hymson.MES.Services.Validators.WhWareHouse;
using Hymson.MES.Services.Validators.WhWarehouseLocation;
using Hymson.MES.Services.Validators.WhWarehouseRegion;
using Hymson.MES.Services.Validators.WhWarehouseShelf;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class AppServiceCollectionExtensions
    {
        /// <summary>
        /// 业务逻辑层依赖服务添加
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSqlExecuteTaskService(configuration);
            services.AddWebFrameworkService(configuration);
            services.AddExcelService();
            services.AddMinioService(configuration);
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
            services.AddSingleton<IEquFaultReasonService, EquFaultReasonService>();
            services.AddSingleton<IEquFaultSolutionService, EquFaultSolutionService>();
            services.AddSingleton<IEquSparePartService, EquSparePartService>();
            services.AddSingleton<IEquSparePartTypeService, EquSparePartTypeService>();
            services.AddSingleton<IEquSparePartsGroupService, EquSparePartsGroupService>();
            services.AddSingleton<IEquSparePartsService, EquSparePartsService>();

            #endregion

            #region Integrated
            services.AddSingleton<IInteCalendarService, InteCalendarService>();
            services.AddSingleton<IInteClassService, InteClassService>();
            services.AddSingleton<IInteJobService, InteJobService>();
            services.AddSingleton<IInteContainerService, InteContainerService>();
            services.AddSingleton<IInteWorkCenterService, InteWorkCenterService>();
            services.AddSingleton<IInteSystemTokenService, InteSystemTokenService>();
            services.AddSingleton<IInteCustomService, InteCustomService>();

            services.AddSingleton<IInteVehicleTypeService, InteVehicleTypeService>();
            services.AddSingleton<IInteVehicleService, InteVehicleService>();
            services.AddSingleton<IInteUnitService, InteUnitService>();

            // CodeRule
            services.AddSingleton<IInteCodeRulesService, InteCodeRulesService>();
            services.AddSingleton<IInteCodeRulesMakeService, InteCodeRulesMakeService>();

            services.AddSingleton<IInteMessageGroupService, InteMessageGroupService>();
            services.AddSingleton<IInteEventTypeService, InteEventTypeService>();
            services.AddSingleton<IInteEventService, InteEventService>();
            services.AddSingleton<IInteMessageManageService, InteMessageManageService>();
            services.AddSingleton<IInteCustomFieldService, InteCustomFieldService>();
            #endregion

            #region Process
            services.AddSingleton<IProcMaskCodeService, ProcMaskCodeService>();
            services.AddSingleton<IProcProductParameterGroupService, ProcProductParameterGroupService>();

            // Material
            services.AddSingleton<IProcMaterialService, ProcMaterialService>();
            services.AddSingleton<IProcMaterialGroupService, ProcMaterialGroupService>();

            // Parameter
            services.AddSingleton<IProcParameterService, ProcParameterService>();

            // ParameterLinkType 
            services.AddSingleton<IProcParameterLinkTypeService, ProcParameterLinkTypeService>();

            // Bom
            services.AddSingleton<IProcBomService, ProcBomService>();
            services.AddSingleton<IProcBomDetailService, ProcBomDetailService>();

            // LoadPoint 
            services.AddSingleton<IProcLoadPointService, ProcLoadPointService>();

            // Resource
            services.AddSingleton<IProcResourceTypeService, ProcResourceTypeService>();
            services.AddSingleton<IProcResourceService, ProcResourceService>();

            // 工序
            services.AddSingleton<IProcProcedureService, ProcProcedureService>();

            // 工艺路线
            services.AddSingleton<IProcProcessRouteService, ProcProcessRouteService>();

            services.AddSingleton<IProcPrintConfigService, ProcPrintConfigService>();
            // 标签模板
            services.AddSingleton<IProcLabelTemplateService, ProcLabelTemplateService>();

            services.AddSingleton<IProcSortingRuleService, ProcSortingRuleService>();

            services.AddSingleton<IProcEquipmentGroupParamService, ProcEquipmentGroupParamService>();

            // 工艺设备组
            services.AddSingleton<IProcProcessEquipmentGroupService, ProcProcessEquipmentGroupService>();
            services.AddSingleton<IProcProcessEquipmentGroupRelationService, ProcProcessEquipmentGroupRelationService>();

            services.AddSingleton<IProcFormulaService, ProcFormulaService>();

            //ESOP
            services.AddSingleton<IProcEsopService, ProcEsopService>();

            //配方操作
            services.AddSingleton<IProcFormulaOperationService, ProcFormulaOperationService>();
            //配方操作组
            services.AddSingleton<IProcFormulaOperationGroupService, ProcFormulaOperationGroupService>();

            #endregion

            #region Quality
            services.AddSingleton<IQualEnvParameterGroupService, QualEnvParameterGroupService>();
            services.AddSingleton<IQualInspectionParameterGroupService, QualInspectionParameterGroupService>();
            services.AddSingleton<IQualUnqualifiedCodeService, QualUnqualifiedCodeService>();
            services.AddSingleton<IQualUnqualifiedGroupService, QualUnqualifiedGroupService>();
            services.AddSingleton<IQualIpqcInspectionService, QualIpqcInspectionService>();
            services.AddSingleton<IQualIpqcInspectionHeadService, QualIpqcInspectionHeadService>();
            services.AddSingleton<IQualIpqcInspectionPatrolService, QualIpqcInspectionPatrolService>();
            services.AddSingleton<IQualIpqcInspectionTailService, QualIpqcInspectionTailService>();
            #endregion

            #region Manufacture
            services.AddSingleton<IManuCommonOldService, ManuCommonOldService>();
            services.AddSingleton<IManuFeedingService, ManuFeedingService>();
            services.AddSingleton<IManuSfcService, ManuSfcService>();
            services.AddSingleton<IManuSfcProduceService, ManuSfcProduceService>();
            services.AddSingleton<IManuGenerateBarcodeExampleService, ManuGenerateBarcodeExampleService>();

            services.AddSingleton<IManuProductBadRecordService, ManuProductBadRecordService>();
            services.AddSingleton<IManuFacePlateService, ManuFacePlateService>();
            services.AddSingleton<IManuFacePlateButtonService, ManuFacePlateButtonService>();

            services.AddSingleton<IManuRepairService, ManuRepairService>();
            services.AddSingleton<IManuOutStationService, ManuOutStationService>();
            services.AddSingleton<IInProductDismantleService, InProductDismantleService>();
            services.AddSingleton<IManuFacePlateRepairService, ManuFacePlateRepairService>();

            services.AddSingleton<IManuContainerBarcodeService, ManuContainerBarcodeService>();
            services.AddSingleton<IManuContainerPackService, ManuContainerPackService>();
            services.AddSingleton<IManuContainerPackRecordService, ManuContainerPackRecordService>();

            services.AddSingleton<IManuFacePlateProductionService, ManuFacePlateProductionService>();
            services.AddSingleton<IManuBakingRecordService, ManuBakingRecordService>();
            services.AddSingleton<IManuBakingService, ManuBakingService>();

            services.AddSingleton<IManuDowngradingRuleService, ManuDowngradingRuleService>();

            services.AddSingleton<IManuSfcGradeService, ManuSfcGradeService>();

            services.AddSingleton<IManuDowngradingService, ManuDowngradingService>();
            services.AddSingleton<IManuDowngradingRecordService, ManuDowngradingRecordService>();

            services.AddSingleton<IManuBarcodeAdjustService, ManuBarcodeAdjustService>();

            #endregion

            #region Warehouse 
            services.AddSingleton<IWhSupplierService, WhSupplierService>();
            services.AddSingleton<IWhMaterialInventoryService, WhMaterialInventoryService>();
            services.AddSingleton<IWhMaterialStandingbookService, WhMaterialStandingbookService>();
            services.AddSingleton<IWhWarehouseService, WhWarehouseService>();
            services.AddSingleton<IWhWarehouseRegionService, WhWarehouseRegionService>();
            services.AddSingleton<IWhWarehouseShelfService, WhWarehouseShelfService>();
            services.AddSingleton<IWhWarehouseLocationService, WhWarehouseLocationService>();
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

            #region Report
            #region BadRecordReport
            services.AddSingleton<IBadRecordReportService, BadRecordReportService>();
            #endregion

            #region BadRecordReport
            services.AddSingleton<IWorkshopJobControlReportService, WorkshopJobControlReportService>();
            #endregion

            #region Packaging
            services.AddSingleton<IPackagingReportService, PackagingReportService>();
            #endregion

            #region OriginalSummary
            services.AddSingleton<IOriginalSummaryReportService, OriginalSummaryReportService>();
            #endregion

            #region ComUsage
            services.AddSingleton<IComUsageReportService, ComUsageReportService>();
            #endregion

            services.AddSingleton<ITracingSourceSFCService, TracingSourceSFCService>();

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
            services.AddSingleton<AbstractValidator<EquFaultReasonSaveDto>, EquFaultReasonValidator>();
            services.AddSingleton<AbstractValidator<EquFaultSolutionSaveDto>, EquFaultSolutionValidator>();
            services.AddSingleton<AbstractValidator<EquSparePartsGroupSaveDto>, EquSparePartsGroupSaveValidator>();
            services.AddSingleton<AbstractValidator<EquSparePartsSaveDto>, EquSparePartsSaveValidator>();

            services.AddSingleton<AbstractValidator<EquEquipmentVerifyCreateDto>, EquEquipmentVerifyCreateValidator>();
            #endregion

            #region Process
            services.AddSingleton<AbstractValidator<ProcProductParameterGroupSaveDto>, ProcProductParameterGroupValidator>();

            #region Material
            services.AddSingleton<AbstractValidator<ProcMaterialCreateDto>, ProcMaterialCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcMaterialImportDto>, ProcMaterialImportValidator>();
            services.AddSingleton<AbstractValidator<ProcMaterialModifyDto>, ProcMaterialModifyValidator>();

            services.AddSingleton<AbstractValidator<ProcMaterialSupplierRelationCreateDto>, ProcMaterialSupplierRelationCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcMaterialSupplierRelationModifyDto>, ProcMaterialSupplierRelationModifyValidator>();
            #endregion

            #region Parameter
            services.AddSingleton<AbstractValidator<ProcParameterCreateDto>, ProcParameterCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcParameterModifyDto>, ProcParameterModifyValidator>();
            services.AddSingleton<AbstractValidator<ProcParameterImportDto>, ProcParameterImportValidator>();
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
            services.AddSingleton<AbstractValidator<ImportLoadPointDto>, ProcLoadPointImportValidator>();
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

            services.AddSingleton<AbstractValidator<FlowDynamicLinkDto>, ProcFlowDynamicLinkValidator>();
            services.AddSingleton<AbstractValidator<FlowDynamicNodeDto>, ProcFlowDynamicNodeValidator>();
            #endregion

            #region LabelTemplate

            services.AddSingleton<AbstractValidator<ProcLabelTemplateCreateDto>, ProcLabelTemplateCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcLabelTemplateModifyDto>, ProcLabelTemplateModifyValidator>();
            services.AddSingleton<AbstractValidator<ProcPrinterDto>, ProcPrinterCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcPrinterUpdateDto>, ProcPrinterModifyValidator>();
            #endregion

            #region MaskCode
            services.AddSingleton<AbstractValidator<ProcMaskCodeSaveDto>, ProcMaskCodeValidator>();
            #endregion

            #region ProcSortingRule
            services.AddSingleton<AbstractValidator<ProcSortingRuleCreateDto>, ProcSortingRuleCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcSortingRuleModifyDto>, ProcSortingRuleModifyValidator>();
            #endregion


            services.AddSingleton<AbstractValidator<ProcEquipmentGroupParamCreateDto>, ProcEquipmentGroupParamCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcEquipmentGroupParamModifyDto>, ProcEquipmentGroupParamModifyValidator>();
            services.AddSingleton<AbstractValidator<ProcEquipmentGroupParamDetailCreateDto>, ProcEquipmentGroupParamDetailCreateValidator>();

            services.AddSingleton<AbstractValidator<ProcProcessEquipmentGroupSaveDto>, ProcProcessEquipmentGroupSaveValidator>();
            services.AddSingleton<AbstractValidator<ProcProcessEquipmentGroupRelationSaveDto>, ProcProcessEquipmentGroupRelationSaveValidator>();

            services.AddSingleton<AbstractValidator<ProcFormulaSaveDto>, ProcFormulaSaveValidator>();

            #region Esop
            services.AddSingleton<AbstractValidator<ProcEsopCreateDto>, ProcEsopCreateValidator>();
            services.AddSingleton<AbstractValidator<ProcEsopModifyDto>, ProcEsopModifyValidator>();
            services.AddSingleton<AbstractValidator<ProcEsopGetJobQueryDto>, ProcEsopGetJobValidator>();
            #endregion

            #region ProcFormulaOperation
            services.AddSingleton<AbstractValidator<ProcFormulaOperationSaveDto>, ProcFormulaOperationSaveValidator>();
            #endregion

            #region ProcFormulaOperationGroup
            services.AddSingleton<AbstractValidator<ProcFormulaOperationGroupSaveDto>, ProcFormulaOperationGroupSaveValidator>();
            #endregion

            #endregion

            #region Integrated
            services.AddSingleton<AbstractValidator<InteContainerSaveDto>, InteContainerValidator>();
            services.AddSingleton<AbstractValidator<InteClassSaveDto>, InteClassSaveValidator>();
            services.AddSingleton<AbstractValidator<InteJobCreateDto>, InteJobCreateValidator>();
            services.AddSingleton<AbstractValidator<InteJobModifyDto>, InteJobModifyValidator>();
            services.AddSingleton<AbstractValidator<InteWorkCenterCreateDto>, InteWorkCenterCreateValidator>();
            services.AddSingleton<AbstractValidator<InteWorkCenterModifyDto>, InteWorkCenterModifyValidator>();
            services.AddSingleton<AbstractValidator<InteSystemTokenCreateDto>, InteSystemTokenCreateValidator>();
            services.AddSingleton<AbstractValidator<InteSystemTokenModifyDto>, InteSystemTokenModifyValidator>();
            services.AddSingleton<AbstractValidator<InteCustomImportDto>, InteCustomImportValidator>();
            services.AddSingleton<AbstractValidator<ImportBomDto>, ProcBomImportValidator>();
            services.AddSingleton<AbstractValidator<InteCustomCreateDto>, InteCustomCreateValidator>();
            services.AddSingleton<AbstractValidator<InteCustomModifyDto>, InteCustomModifyValidator>();
            services.AddSingleton<AbstractValidator<InteUnitSaveDto>, InteUnitSaveValidator>();

            services.AddSingleton<AbstractValidator<InteVehicleTypeCreateDto>, InteVehicleTypeCreateValidator>();
            services.AddSingleton<AbstractValidator<InteVehicleTypeModifyDto>, InteVehicleTypeModifyValidator>();

            services.AddSingleton<AbstractValidator<InteVehicleTypeVerifyCreateDto>, InteVehicleTypeVerifyCreateValidator>();

            services.AddSingleton<AbstractValidator<InteVehicleCreateDto>, InteVehicleCreateValidator>();
            services.AddSingleton<AbstractValidator<InteVehicleModifyDto>, InteVehicleModifyValidator>();
            services.AddSingleton<AbstractValidator<InteVehicleBindOperationDto>, InteVehicleBindoptValidator>();
            services.AddSingleton<AbstractValidator<InteVehicleUnbindOperationDto>, InteVehicleUnBindoptValidator>();

            services.AddSingleton<AbstractValidator<InteMessageGroupSaveDto>, InteMessageGroupSaveValidator>();
            services.AddSingleton<AbstractValidator<InteEventTypeSaveDto>, InteEventTypeSaveValidator>();
            services.AddSingleton<AbstractValidator<InteEventSaveDto>, InteEventSaveValidator>();
            services.AddSingleton<AbstractValidator<InteCustomFieldSaveDto>, InteCustomFieldSaveValidator>();
            services.AddSingleton<AbstractValidator<InteCustomFieldInternationalizationDto>, InteCustomFieldInternationalizationValidator>();
            services.AddSingleton<AbstractValidator<InteCustomFieldBusinessEffectuateDto>, InteCustomFieldBusinessEffectuatenValidator>();
            services.AddSingleton<AbstractValidator<InteMessageManageTriggerSaveDto>, InteMessageManageSaveValidator>();

            #region CodeRule
            services.AddSingleton<AbstractValidator<InteCodeRulesCreateDto>, InteCodeRulesCreateValidator>();
            services.AddSingleton<AbstractValidator<InteCodeRulesModifyDto>, InteCodeRulesModifyValidator>();

            services.AddSingleton<AbstractValidator<InteCodeRulesMakeCreateDto>, InteCodeRulesMakeCreateValidator>();
            services.AddSingleton<AbstractValidator<InteCodeRulesMakeModifyDto>, InteCodeRulesMakeModifyValidator>();
            #endregion
            #endregion

            #region Quality
            services.AddSingleton<AbstractValidator<QualEnvParameterGroupSaveDto>, QualEnvParameterGroupValidator>();
            services.AddSingleton<AbstractValidator<QualInspectionParameterGroupSaveDto>, QualInspectionParameterGroupValidator>();
            services.AddSingleton<AbstractValidator<QualUnqualifiedCodeCreateDto>, QualUnqualifiedCodeCreateValidator>();
            services.AddSingleton<AbstractValidator<QualUnqualifiedCodeModifyDto>, QualUnqualifiedCodeModifyValidator>();
            services.AddSingleton<AbstractValidator<QualUnqualifiedGroupCreateDto>, QualUnqualifiedGroupCreateValidator>();
            services.AddSingleton<AbstractValidator<QualUnqualifiedGroupModifyDto>, QualUnqualifiedGroupModifyValidator>();
            services.AddSingleton<AbstractValidator<QualIpqcInspectionSaveDto>, QualIpqcInspectionSaveValidator>();
            services.AddSingleton<AbstractValidator<QualIpqcInspectionHeadSaveDto>, QualIpqcInspectionHeadSaveValidator>();
            services.AddSingleton<AbstractValidator<List<QualIpqcInspectionHeadSampleCreateDto>>, QualIpqcInspectionHeadSampleAddValidator>();
            services.AddSingleton<AbstractValidator<QualIpqcInspectionPatrolSaveDto>, QualIpqcInspectionPatrolSaveValidator>();
            services.AddSingleton<AbstractValidator<List<QualIpqcInspectionPatrolSampleCreateDto>>, QualIpqcInspectionPatrolSampleAddValidator>();
            services.AddSingleton<AbstractValidator<QualIpqcInspectionTailSaveDto>, QualIpqcInspectionTailSaveValidator>();
            services.AddSingleton<AbstractValidator<List<QualIpqcInspectionTailSampleCreateDto>>, QualIpqcInspectionTailSampleAddValidator>();
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

            services.AddSingleton<AbstractValidator<ManuBakingCreateDto>, ManuBakingCreateValidator>();
            services.AddSingleton<AbstractValidator<ManuBakingModifyDto>, ManuBakingModifyValidator>();
            services.AddSingleton<AbstractValidator<ManuBakingRecordCreateDto>, ManuBakingRecordCreateValidator>();
            services.AddSingleton<AbstractValidator<ManuBakingRecordModifyDto>, ManuBakingRecordModifyValidator>();

            services.AddSingleton<AbstractValidator<ManuDowngradingRuleCreateDto>, ManuDowngradingRuleCreateValidator>();
            services.AddSingleton<AbstractValidator<ManuDowngradingRuleModifyDto>, ManuDowngradingRuleModifyValidator>();
           
            services.AddSingleton<AbstractValidator<ManuBarcodeSplitAdjustDto>, ManusBarcodeSplitAdjustValidator>();
            #endregion

            #region Warehouse 

            services.AddSingleton<AbstractValidator<WhSupplierCreateDto>, WhSupplierCreateValidator>();
            services.AddSingleton<AbstractValidator<WhSupplierModifyDto>, WhSupplierModifyValidator>();
            services.AddSingleton<AbstractValidator<WhSupplierImportDto>, WhSupplierImportValidator>();

            services.AddSingleton<AbstractValidator<WhMaterialInventoryCreateDto>, WhMaterialInventoryCreateValidator>();
            services.AddSingleton<AbstractValidator<WhMaterialInventoryModifyDto>, WhMaterialInventoryModifyValidator>();

            services.AddSingleton<AbstractValidator<WhMaterialStandingbookCreateDto>, WhMaterialStandingbookCreateValidator>();
            services.AddSingleton<AbstractValidator<WhMaterialStandingbookModifyDto>, WhMaterialStandingbookModifyValidator>();

            services.AddSingleton<AbstractValidator<WhWarehouseSaveDto>, WhWarehouseSaveValidator>();
            services.AddSingleton<AbstractValidator<WhWarehouseModifyDto>, WhWarehouseModifyValidator>();

            services.AddSingleton<AbstractValidator<WhWarehouseRegionSaveDto>, WhWarehouseRegionSaveValidator>();
            services.AddSingleton<AbstractValidator<WhWarehouseRegionModifyDto>, WhWarehouseRegionModifyValidator>();

            services.AddSingleton<AbstractValidator<WhWarehouseShelfSaveDto>, WhWarehouseShelfSaveValidator>();
            services.AddSingleton<AbstractValidator<WhWarehouseShelfModifyDto>, WhWarehouseShelfModifyValidator>();

            services.AddSingleton<AbstractValidator<WhWarehouseLocationSaveDto>, WhWarehouseLocationSaveValidator>();
            services.AddSingleton<AbstractValidator<WhWarehouseLocationModifyDto>, WhWarehouseLocationModifyValidator>();

            #endregion

            #region Plan
            #region PlanWorkOrder
            services.AddSingleton<AbstractValidator<PlanWorkOrderCreateDto>, PlanWorkOrderCreateValidator>();
            services.AddSingleton<AbstractValidator<PlanWorkOrderModifyDto>, PlanWorkOrderModifyValidator>();
            services.AddSingleton<AbstractValidator<PlanWorkOrderChangeStatusDto>, PlanWorkOrderChangeStatusValidator>();
            #endregion

            #region PlanSfcReceive
            services.AddSingleton<AbstractValidator<PlanSfcReceiveCreateDto>, PlanSfcReceiveCreateValidator>();
            services.AddSingleton<AbstractValidator<PlanSfcReceiveScanCodeDto>, PlanSfcReceiveModifyValidator>();
            #endregion

            #region PlanWorkOrderActivation
            services.AddSingleton<AbstractValidator<PlanWorkOrderActivationCreateDto>, PlanWorkOrderActivationCreateValidator>();
            services.AddSingleton<AbstractValidator<PlanWorkOrderActivationModifyDto>, PlanWorkOrderActivationModifyValidator>();
            #endregion

            #region PlanSfcPrint
            services.AddSingleton<AbstractValidator<PlanSfcPrintCreateDto>, PlanSfcPrintCreateValidator>();
            services.AddSingleton<AbstractValidator<PlanSfcPrintCreatePrintDto>, PlanSfcPrintCreatePrintValidator>();
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
