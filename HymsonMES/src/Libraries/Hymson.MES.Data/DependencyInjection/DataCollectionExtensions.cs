using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentLinkApi;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit;
using Hymson.MES.Data.Repositories.Equipment.EquFaultPhenomenon;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart;
using Hymson.MES.Data.Repositories.Equipment.EquSparePartType;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteCalendar;
using Hymson.MES.Data.Repositories.Integrated.InteClass;
using Hymson.MES.Data.Repositories.Integrated.InteContainer;
using Hymson.MES.Data.Repositories.Integrated.InteJob;
using Hymson.MES.Data.Repositories.Integrated.InteJobClass;
using Hymson.MES.Data.Repositories.Integrated.InteSFCBox;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Data.Repositories.Report;
using Hymson.MES.Data.Repositories.Warehouse;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataCollectionExtensions
    {
        /// <summary>
        /// 数据层依赖服务注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCore();
            AddConfig(services, configuration);
            AddRepository(services);
            return services;
        }

        /// <summary>
        /// 添加仓储依赖
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddRepository(this IServiceCollection services)
        {
            #region Equipment
            //services.AddSingleton<IEquConsumableRepository, EquConsumableRepository>();
            //services.AddSingleton<IEquConsumableTypeRepository, EquConsumableTypeRepository>();
            services.AddSingleton<IEquEquipmentRepository, EquEquipmentRepository>();
            services.AddSingleton<IEquEquipmentGroupRepository, EquEquipmentGroupRepository>();
            services.AddSingleton<IEquEquipmentLinkApiRepository, EquEquipmentLinkApiRepository>();
            services.AddSingleton<IEquEquipmentLinkHardwareRepository, EquEquipmentLinkHardwareRepository>();
            services.AddSingleton<IEquEquipmentUnitRepository, EquEquipmentUnitRepository>();
            services.AddSingleton<IEquFaultPhenomenonRepository, EquFaultPhenomenonRepository>();
            services.AddSingleton<IEquSparePartRepository, EquSparePartRepository>();
            services.AddSingleton<IEquSparePartTypeRepository, EquSparePartTypeRepository>();
            services.AddSingleton<IEquHeartbeatRepository, EquHeartbeatRepository>();
            services.AddSingleton<IEquAlarmRepository, EquAlarmRepository>();
            services.AddSingleton<IEquStatusRepository, EquStatusRepository>();
            services.AddSingleton<IEquProductParameterRepository, EquProductParameterRepository>();
            services.AddSingleton<IEquEquipmentTokenRepository, EquEquipmentTokenRepository>();

            services.AddSingleton<IEquFaultReasonRepository, EquFaultReasonRepository>();
            services.AddSingleton<IEquEquipmentTheoryRepository, EquEquipmentTheoryRepository>();
            #endregion

            #region Integrated
            services.AddSingleton<IInteAttachmentRepository, InteAttachmentRepository>();
            services.AddSingleton<IInteCalendarDateDetailRepository, InteCalendarDateDetailRepository>();
            services.AddSingleton<IInteCalendarDateRepository, InteCalendarDateRepository>();
            services.AddSingleton<IInteCalendarRepository, InteCalendarRepository>();
            services.AddSingleton<IInteClassDetailRepository, InteClassDetailRepository>();
            services.AddSingleton<IInteClassRepository, InteClassRepository>();
            services.AddSingleton<IInteContainerRepository, InteContainerRepository>();
            services.AddSingleton<IInteJobClassRepository, InteJobClassRepository>();

            //InteJob
            services.AddSingleton<IInteJobBusinessRelationRepository, InteJobBusinessRelationRepository>();
            services.AddSingleton<IInteJobRepository, InteJobRepository>();
            services.AddSingleton<IInteWorkCenterRepository, InteWorkCenterRepository>();
            services.AddSingleton<IInteTrayRepository, InteTrayRepository>();
            services.AddSingleton<IInteSFCBoxRepository, InteSFCBoxRepository>();

            #region CodeRule
            services.AddSingleton<IInteCodeRulesRepository, InteCodeRulesRepository>();
            services.AddSingleton<IInteCodeRulesMakeRepository, InteCodeRulesMakeRepository>();
            #endregion

            #region InteSystemToken
            services.AddSingleton<IInteSystemTokenRepository, InteSystemTokenRepository>();
            #endregion
            #endregion

            #region Process
            services.AddSingleton<IProcMaskCodeRuleRepository, ProcMaskCodeRuleRepository>();
            services.AddSingleton<IProcMaskCodeRepository, ProcMaskCodeRepository>();
            services.AddSingleton<IProcProductSetRepository, ProcProductSetRepository>();
            services.AddSingleton<IProcProcessEquipmentGroupRepository, ProcProcessEquipmentGroupRepository>();
            services.AddSingleton<IProcProcessEquipmentGroupRelationRepository, ProcProcessEquipmentGroupRelationRepository>();
            services.AddSingleton<IProcEquipmentGroupParamRepository, ProcEquipmentGroupParamRepository>();
            services.AddSingleton<IProcEquipmentGroupParamDetailRepository, ProcEquipmentGroupParamDetailRepository>();
            services.AddSingleton<IInteUnitRepository, InteUnitRepository>();

            //开机参数
            services.AddSingleton<IProcBootupparamrecordRepository, ProcBootupparamrecordRepository>();
            services.AddSingleton<IProcBootupparamRepository, ProcBootupparamRepository>();
            services.AddSingleton<IProcBootuprecipeRepository, ProcBootuprecipeRepository>();


            #region Material
            services.AddSingleton<IProcMaterialRepository, ProcMaterialRepository>();
            services.AddSingleton<IProcReplaceMaterialRepository, ProcReplaceMaterialRepository>();

            services.AddSingleton<IProcMaterialGroupRepository, ProcMaterialGroupRepository>();

            services.AddSingleton<IProcMaterialSupplierRelationRepository, ProcMaterialSupplierRelationRepository>();
            #endregion

            #region Parameter
            services.AddSingleton<IProcParameterRepository, ProcParameterRepository>();

            #endregion

            #region ParameterLinkType
            services.AddSingleton<IProcParameterLinkTypeRepository, ProcParameterLinkTypeRepository>();

            #endregion

            #region Bom
            services.AddSingleton<IProcBomRepository, ProcBomRepository>();
            services.AddSingleton<IProcBomDetailRepository, ProcBomDetailRepository>();
            services.AddSingleton<IProcBomDetailReplaceMaterialRepository, ProcBomDetailReplaceMaterialRepository>();
            #endregion

            #region LoadPoint
            services.AddSingleton<IProcLoadPointRepository, ProcLoadPointRepository>();

            #endregion

            #region LoadPointLink
            services.AddSingleton<IProcLoadPointLinkMaterialRepository, ProcLoadPointLinkMaterialRepository>();
            services.AddSingleton<IProcLoadPointLinkResourceRepository, ProcLoadPointLinkResourceRepository>();
            #endregion

            #region Resource
            services.AddSingleton<IProcResourceTypeRepository, ProcResourceTypeRepository>();
            services.AddSingleton<IProcResourceRepository, ProcResourceRepository>();
            services.AddSingleton<IProcResourceConfigPrintRepository, ProcResourceConfigPrintRepository>();
            services.AddSingleton<IProcResourceConfigResRepository, ProcResourceConfigResRepository>();
            services.AddSingleton<IProcResourceEquipmentBindRepository, ProcResourceEquipmentBindRepository>();
            #endregion

            #region Procedure
            services.AddSingleton<IProcProcedureRepository, ProcProcedureRepository>();
            services.AddSingleton<IProcProcedurePrintRelationRepository, ProcProcedurePrintRelationRepository>();
            #endregion

            #region ProduceSet
            services.AddSingleton<IProcProductSetRepository, ProcProductSetRepository>();
            #endregion

            #region ProcessRoute
            services.AddSingleton<IProcProcessRouteRepository, ProcProcessRouteRepository>();
            services.AddSingleton<IProcProcessRouteDetailNodeRepository, ProcProcessRouteDetailNodeRepository>();
            services.AddSingleton<IProcProcessRouteDetailLinkRepository, ProcProcessRouteDetailLinkRepository>();
            #endregion

            #region LabelTemplate
            services.AddSingleton<IProcLabelTemplateRepository, ProcLabelTemplateRepository>();
            #endregion

            #region printConfig
            services.AddSingleton<IProcPrintConfigRepository, ProcPrintConfigRepository>();
            #endregion

            #region ESOP 
            services.AddSingleton<IProcEsopFileRepository, ProcEsopFileRepository>();
            services.AddSingleton<IProcEsopRepository, ProcEsopRepository>();
            #endregion

            #endregion

            #region Quality
            services.AddSingleton<IQualUnqualifiedCodeRepository, QualUnqualifiedCodeRepository>();
            services.AddSingleton<IQualUnqualifiedGroupRepository, QualUnqualifiedGroupRepository>();
            #endregion

            #region Manufacture
            services.AddSingleton<IManuFeedingRepository, ManuFeedingRepository>();
            services.AddSingleton<IManuFeedingRecordRepository, ManuFeedingRecordRepository>();
            services.AddSingleton<IManuFeedingLiteRepository, ManuFeedingLiteRepository>();
            services.AddSingleton<IManuFeedingLiteRecordRepository, ManuFeedingLiteRecordRepository>();
            services.AddSingleton<IManuProductBadRecordRepository, ManuProductBadRecordRepository>();
            services.AddSingleton<IManuSfcCirculationRepository, ManuSfcCirculationRepository>();
            services.AddSingleton<IManuSfcInfoRepository, ManuSfcInfoRepository>();
            services.AddSingleton<IManuSfcRepository, ManuSfcRepository>();
            services.AddSingleton<IManuSfcProduceRepository, ManuSfcProduceRepository>();
            services.AddSingleton<IManuSfcStepRepository, ManuSfcStepRepository>();
            services.AddSingleton<IManuFacePlateRepository, ManuFacePlateRepository>();
            services.AddSingleton<IManuFacePlateProductionRepository, ManuFacePlateProductionRepository>();
            services.AddSingleton<IManuFacePlateRepairRepository, ManuFacePlateRepairRepository>();
            services.AddSingleton<IManuFacePlateContainerPackRepository, ManuFacePlateContainerPackRepository>();
            services.AddSingleton<IManuFacePlateButtonRepository, ManuFacePlateButtonRepository>();
            services.AddSingleton<IManuFacePlateButtonJobRelationRepository, ManuFacePlateButtonJobRelationRepository>();

            services.AddSingleton<IManuContainerPackRecordRepository, ManuContainerPackRecordRepository>();
            services.AddSingleton<IManuContainerPackRepository, ManuContainerPackRepository>();
            services.AddSingleton<IManuContainerBarcodeRepository, ManuContainerBarcodeRepository>();

            services.AddSingleton<IManuTrayLoadRepository, ManuTrayLoadRepository>();
            services.AddSingleton<IManuTraySfcRecordRepository, ManuTraySfcRecordRepository>();
            services.AddSingleton<IManuTraySfcRelationRepository, ManuTraySfcRelationRepository>();
            services.AddSingleton<IManuSfcBindRecordRepository, ManuSfcBindRecordRepository>();
            services.AddSingleton<IManuSfcBindRepository, ManuSfcBindRepository>();
            services.AddSingleton<IManuProductParameterRepository, ManuProductParameterRepository>();
            services.AddSingleton<IManuCcdFileRepository, ManuCcdFileRepository>();
            services.AddSingleton<IManuSfcStepNgRepository, ManuSfcStepNgRepository>();
            services.AddSingleton<IManuOutputBindMaterialRepository, ManuOutputBindMaterialRepository>();
            services.AddSingleton<IManuOutputNgRepository, ManuOutputNgRepository>();
            services.AddSingleton<IManuOutputRepository, ManuOutputRepository>();
            services.AddSingleton<IManuSfcStepMaterialRepository, ManuSfcStepMaterialRepository>();
            services.AddSingleton<IManuSfcSummaryRepository, ManuSfcSummaryRepository>();
            services.AddSingleton<IManuSfcCcsNgRecordRepository, ManuSfcCcsNgRecordRepository>();
            #endregion

            #region Warehouse 
            services.AddSingleton<IWhSupplierRepository, WhSupplierRepository>();
            services.AddSingleton<IWhMaterialInventoryRepository, WhMaterialInventoryRepository>();
            services.AddSingleton<IWhMaterialStandingbookRepository, WhMaterialStandingbookRepository>();


            #endregion

            #region Plan
            //services.AddSingleton<IPlanWorkOrderActivationRepository, PlanWorkOrderActivationRepository>();

            #region PlanWorkOrder
            services.AddSingleton<IPlanWorkOrderRepository, PlanWorkOrderRepository>();
            services.AddSingleton<IPlanWorkOrderStatusRecordRepository, PlanWorkOrderStatusRecordRepository>();
            services.AddSingleton<IPlanWorkOrderConversionRepository, PlanWorkOrderConversionRepository>();
            #endregion

            #region PlanSfcReceive
            services.AddSingleton<IPlanSfcReceiveRepository, PlanSfcReceiveRepository>();
            #endregion

            #region PlanWorkOrderActivation
            services.AddSingleton<IPlanWorkOrderActivationRepository, PlanWorkOrderActivationRepository>();
            services.AddSingleton<IPlanWorkOrderActivationRecordRepository, PlanWorkOrderActivationRecordRepository>();
            #endregion

            #region PlanWorkOrderBind/PlanWorkOrderBindRecord
            services.AddSingleton<IPlanWorkOrderBindRepository, PlanWorkOrderBindRepository>();
            services.AddSingleton<IPlanWorkOrderBindRecordRepository, PlanWorkOrderBindRecordRepository>();
            #endregion
            #endregion

            #region Report
            services.AddSingleton<IProductDetailReportRepository, ProductDetailReportRepository>();
            services.AddSingleton<IPackTraceSFCParameterRepository, PackTraceSFCParameterRepository>();

            services.AddSingleton<IProductionDetailsReportRepository, ProductionDetailsReportRepository>();

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
            // 数据库连接
            services.Configure<ConnectionOptions>(configuration.GetSection(nameof(ConnectionOptions)));
            //services.Configure<ConnectionOptions>(configuration);
            return services;
        }

    }
}
