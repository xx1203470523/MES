using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common;
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
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedGroup;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.WhWareHouse;
using Hymson.MES.Data.Repositories.WhWarehouseLocation;
using Hymson.MES.Data.Repositories.WhWarehouseRegion;
using Hymson.MES.Data.Repositories.WhWarehouseShelf;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入服务类
    /// </summary>
    public static partial class DataCollectionExtensions
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
            AddRepositoryForXinShiJie(services);
            return services;
        }

        /// <summary>
        /// 添加仓储依赖
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddRepository(this IServiceCollection services)
        {
            #region Commonm
            services.AddSingleton<IMessageTemplateRepository, MessageTemplateRepository>();
            services.AddSingleton<IMessagePushRepository, MessagePushRepository>();
            #endregion

            #region Equipment
            services.AddSingleton<IEquEquipmentRepository, EquEquipmentRepository>();
            services.AddSingleton<IEquEquipmentGroupRepository, EquEquipmentGroupRepository>();
            services.AddSingleton<IEquEquipmentLinkApiRepository, EquEquipmentLinkApiRepository>();
            services.AddSingleton<IEquEquipmentLinkHardwareRepository, EquEquipmentLinkHardwareRepository>();
            services.AddSingleton<IEquEquipmentUnitRepository, EquEquipmentUnitRepository>();
            services.AddSingleton<IEquFaultPhenomenonRepository, EquFaultPhenomenonRepository>();
            services.AddSingleton<IEquSparePartRepository, EquSparePartRepository>();
            services.AddSingleton<IEquSparePartTypeRepository, EquSparePartTypeRepository>();
            services.AddSingleton<IEquEquipmentTokenRepository, EquEquipmentTokenRepository>();

            services.AddSingleton<IEquEquipmentVerifyRepository, EquEquipmentVerifyRepository>();

            #region FaultReason
            services.AddSingleton<IEquFaultReasonRepository, EquFaultReasonRepository>();

            #endregion
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
            services.AddSingleton<IInteUnitRepository, InteUnitRepository>();

            //InteJob
            services.AddSingleton<IInteJobBusinessRelationRepository, InteJobBusinessRelationRepository>();
            services.AddSingleton<IInteJobRepository, InteJobRepository>();
            services.AddSingleton<IInteWorkCenterRepository, InteWorkCenterRepository>();

            services.AddSingleton<IInteCustomRepository, InteCustomRepository>();

            services.AddSingleton<IInteVehicleTypeRepository, InteVehicleTypeRepository>();
            services.AddSingleton<IInteVehicleTypeVerifyRepository, InteVehicleTypeVerifyRepository>();

            services.AddSingleton<IInteVehicleRepository, InteVehicleRepository>();
            services.AddSingleton<IInteVehicleVerifyRepository, InteVehicleVerifyRepository>();

            services.AddSingleton<IInteVehicleFreightRepository, InteVehicleFreightRepository>();
            services.AddSingleton<IInteVehiceFreightStackRepository, InteVehiceFreightStackRepository>();
            services.AddSingleton<IInteVehicleFreightRecordRepository, InteVehicleFreightRecordRepository>();

            services.AddSingleton<IInteMessageGroupRepository, InteMessageGroupRepository>();
            services.AddSingleton<IInteMessageGroupPushMethodRepository, InteMessageGroupPushMethodRepository>();
            services.AddSingleton<IInteEventRepository, InteEventRepository>();
            services.AddSingleton<IInteEventTypeRepository, InteEventTypeRepository>();
            services.AddSingleton<IInteEventTypeMessageGroupRelationRepository, InteEventTypeMessageGroupRelationRepository>();
            services.AddSingleton<IInteEventTypePushRuleRepository, InteEventTypePushRuleRepository>();
            services.AddSingleton<IInteEventTypeUpgradeRepository, InteEventTypeUpgradeRepository>();
            services.AddSingleton<IInteEventTypeUpgradeMessageGroupRelationRepository, InteEventTypeUpgradeMessageGroupRelationRepository>();

            services.AddSingleton<IInteTimeWildcardRepository, InteTimeWildcardRepository>();

            services.AddSingleton<IInteMessageManageRepository, InteMessageManageRepository>();
            services.AddSingleton<IInteMessageManageAnalysisReportAttachmentRepository, InteMessageManageAnalysisReportAttachmentRepository>();
            services.AddSingleton<IInteMessageManageHandleProgrammeAttachmentRepository, InteMessageManageHandleProgrammeAttachmentRepository>();

            services.AddSingleton<IInteCustomFieldRepository, InteCustomFieldRepository>();
            services.AddSingleton<IInteCustomFieldInternationalizationRepository, InteCustomFieldInternationalizationRepository>();

            #region CodeRule
            services.AddSingleton<IInteCodeRulesRepository, InteCodeRulesRepository>();
            services.AddSingleton<IInteCodeRulesMakeRepository, InteCodeRulesMakeRepository>();
            #endregion

            #region InteSystemToken
            services.AddSingleton<IInteSystemTokenRepository, InteSystemTokenRepository>();
            #endregion

            //InteUnit
            services.AddSingleton<IInteUnitRepository, InteUnitRepository>();
            #endregion

            #region Process
            services.AddSingleton<IProcMaskCodeRuleRepository, ProcMaskCodeRuleRepository>();
            services.AddSingleton<IProcMaskCodeRepository, ProcMaskCodeRepository>();
            services.AddSingleton<IProcProductSetRepository, ProcProductSetRepository>();

            services.AddSingleton<IProcProductParameterGroupRepository, ProcProductParameterGroupRepository>();
            services.AddSingleton<IProcProductParameterGroupDetailRepository, ProcProductParameterGroupDetailRepository>();

            services.AddSingleton<IProcProcessEquipmentGroupRepository, ProcProcessEquipmentGroupRepository>();
            services.AddSingleton<IProcProcessEquipmentGroupRelationRepository, ProcProcessEquipmentGroupRelationRepository>();

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
            services.AddSingleton<IProcProcedureRejudgeRepository, ProcProcedureRejudgeRepository>();
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
            services.AddSingleton<IProcLabelTemplateRelationRepository, ProcLabelTemplateRelationRepository>();

            #endregion

            #region printConfig
            services.AddSingleton<IProcPrintConfigRepository, ProcPrintConfigRepository>();
            #endregion

            #region ProcSortingRule

            services.AddSingleton<IProcSortingRuleRepository, ProcSortingRuleRepository>();
            services.AddSingleton<IProcSortingRuleDetailRepository, ProcSortingRuleDetailRepository>();
            services.AddSingleton<IProcSortingRuleGradeRepository, ProcSortingRuleGradeRepository>();
            services.AddSingleton<IProcSortingRuleGradeDetailsRepository, ProcSortingRuleGradeDetailsRepository>();
            #endregion

            #region EquipmentGroupParam
            services.AddSingleton<IProcEquipmentGroupParamRepository, ProcEquipmentGroupParamRepository>();
            services.AddSingleton<IProcEquipmentGroupParamDetailRepository, ProcEquipmentGroupParamDetailRepository>();
            #endregion

            #endregion

            #region Quality
            services.AddSingleton<IQualEnvParameterGroupRepository, QualEnvParameterGroupRepository>();
            services.AddSingleton<IQualEnvParameterGroupDetailRepository, QualEnvParameterGroupDetailRepository>();
            services.AddSingleton<IQualInspectionParameterGroupRepository, QualInspectionParameterGroupRepository>();
            services.AddSingleton<IQualInspectionParameterGroupDetailRepository, QualInspectionParameterGroupDetailRepository>();

            services.AddSingleton<IQualUnqualifiedCodeRepository, QualUnqualifiedCodeRepository>();
            services.AddSingleton<IQualUnqualifiedGroupRepository, QualUnqualifiedGroupRepository>();

            services.AddSingleton<IQualIpqcInspectionRepository, QualIpqcInspectionRepository>();
            services.AddSingleton<IQualIpqcInspectionParameterRepository, QualIpqcInspectionParameterRepository>();
            services.AddSingleton<IQualIpqcInspectionRuleRepository, QualIpqcInspectionRuleRepository>();
            services.AddSingleton<IQualIpqcInspectionRuleResourceRelationRepository, QualIpqcInspectionRuleResourceRelationRepository>();

            services.AddSingleton<IQualIpqcInspectionHeadRepository, QualIpqcInspectionHeadRepository>();
            services.AddSingleton<IQualIpqcInspectionHeadResultRepository, QualIpqcInspectionHeadResultRepository>();
            services.AddSingleton<IQualIpqcInspectionHeadSampleRepository, QualIpqcInspectionHeadSampleRepository>();
            services.AddSingleton<IQualIpqcInspectionHeadAnnexRepository, QualIpqcInspectionHeadAnnexRepository>();

            services.AddSingleton<IQualIpqcInspectionPatrolRepository, QualIpqcInspectionPatrolRepository>();
            services.AddSingleton<IQualIpqcInspectionPatrolSampleRepository, QualIpqcInspectionPatrolSampleRepository>();
            services.AddSingleton<IQualIpqcInspectionPatrolAnnexRepository, QualIpqcInspectionPatrolAnnexRepository>();

            services.AddSingleton<IQualIpqcInspectionTailRepository, QualIpqcInspectionTailRepository>();
            services.AddSingleton<IQualIpqcInspectionTailSampleRepository, QualIpqcInspectionTailSampleRepository>();
            services.AddSingleton<IQualIpqcInspectionTailAnnexRepository, QualIpqcInspectionTailAnnexRepository>();
            services.AddSingleton<IManuEquipmentParameterRepository, ManuEquipmentParameterRepository>();
            #endregion

            #region Manufacture
            services.AddSingleton<IManuFeedingRepository, ManuFeedingRepository>();
            services.AddSingleton<IManuFeedingRecordRepository, ManuFeedingRecordRepository>();
            services.AddSingleton<IManuProductBadRecordRepository, ManuProductBadRecordRepository>();
            services.AddSingleton<IManuProductNgRecordRepository, ManuProductNgRecordRepository>();
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
            services.AddSingleton<IManuBakingRecordRepository, ManuBakingRecordRepository>();
            services.AddSingleton<IManuBakingRepository, ManuBakingRepository>();

            services.AddSingleton<IManuDowngradingRuleRepository, ManuDowngradingRuleRepository>();

            services.AddSingleton<IManuDowngradingRepository, ManuDowngradingRepository>();
            services.AddSingleton<IManuDowngradingRecordRepository, ManuDowngradingRecordRepository>();
            services.AddSingleton<IManuSfcSummaryRepository, ManuSfcSummaryRepository>();
            services.AddSingleton<IManuSfcScrapRepository, ManuSfcScrapRepository>();

            services.AddSingleton<IManuSfcGradeRepository, ManuSfcGradeRepository>();
            services.AddSingleton<IManuSfcGradeDetailRepository, ManuSfcGradeDetailRepository>();

            services.AddSingleton<IManuWorkOrderSFCRepository, ManuWorkOrderSFCRepository>();
            #endregion

            #region Warehouse 

            services.AddSingleton<IWhSupplierRepository, WhSupplierRepository>();
            services.AddSingleton<IWhMaterialInventoryRepository, WhMaterialInventoryRepository>();
            services.AddSingleton<IWhMaterialStandingbookRepository, WhMaterialStandingbookRepository>();
            services.AddSingleton<IWhWarehouseRepository, WhWarehouseRepository>();
            services.AddSingleton<IWhWarehouseRegionRepository, WhWarehouseRegionRepository>();
            services.AddSingleton<IWhWarehouseShelfRepository, WhWarehouseShelfRepository>();
            services.AddSingleton<IWhWarehouseLocationRepository, WhWarehouseLocationRepository>();

            #endregion

            #region Plan

            #region PlanWorkOrder
            services.AddSingleton<IPlanWorkOrderRepository, PlanWorkOrderRepository>();
            services.AddSingleton<IPlanWorkOrderStatusRecordRepository, PlanWorkOrderStatusRecordRepository>();
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

            #region Parameter
            services.AddSingleton<IManuProductParameterRepository, ManuProductParameterRepository>();
            #endregion

            #region ESOP 
            services.AddSingleton<IProcEsopFileRepository, ProcEsopFileRepository>();
            services.AddSingleton<IProcEsopRepository, ProcEsopRepository>();
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
            services.Configure<ParameterOptions>(configuration.GetSection(nameof(ParameterOptions)));
            return services;
        }

    }
}
