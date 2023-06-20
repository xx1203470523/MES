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
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
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
            //services.AddScoped<IEquConsumableRepository, EquConsumableRepository>();
            //services.AddScoped<IEquConsumableTypeRepository, EquConsumableTypeRepository>();
            services.AddScoped<IEquEquipmentRepository, EquEquipmentRepository>();
            services.AddScoped<IEquEquipmentGroupRepository, EquEquipmentGroupRepository>();
            services.AddScoped<IEquEquipmentLinkApiRepository, EquEquipmentLinkApiRepository>();
            services.AddScoped<IEquEquipmentLinkHardwareRepository, EquEquipmentLinkHardwareRepository>();
            services.AddScoped<IEquEquipmentUnitRepository, EquEquipmentUnitRepository>();
            services.AddScoped<IEquFaultPhenomenonRepository, EquFaultPhenomenonRepository>();
            services.AddScoped<IEquSparePartRepository, EquSparePartRepository>();
            services.AddScoped<IEquSparePartTypeRepository, EquSparePartTypeRepository>();
            services.AddScoped<IEquEquipmentTokenRepository, EquEquipmentTokenRepository>();


            #region FaultReason
            services.AddScoped<IEquFaultReasonRepository, EquFaultReasonRepository>();

            #endregion
            #endregion

            #region Integrated
            services.AddScoped<IInteCalendarDateDetailRepository, InteCalendarDateDetailRepository>();
            services.AddScoped<IInteCalendarDateRepository, InteCalendarDateRepository>();
            services.AddScoped<IInteCalendarRepository, InteCalendarRepository>();
            services.AddScoped<IInteClassDetailRepository, InteClassDetailRepository>();
            services.AddScoped<IInteClassRepository, InteClassRepository>();
            services.AddScoped<IInteContainerRepository, InteContainerRepository>();
            services.AddScoped<IInteJobClassRepository, InteJobClassRepository>();

            //InteJob
            services.AddScoped<IInteJobBusinessRelationRepository, InteJobBusinessRelationRepository>();
            services.AddScoped<IInteJobRepository, InteJobRepository>();
            services.AddScoped<IInteWorkCenterRepository, InteWorkCenterRepository>();

            #region CodeRule
            services.AddScoped<IInteCodeRulesRepository, InteCodeRulesRepository>();
            services.AddScoped<IInteCodeRulesMakeRepository, InteCodeRulesMakeRepository>();
            #endregion

            #region InteSystemToken
            services.AddScoped<IInteSystemTokenRepository, InteSystemTokenRepository>();
            #endregion
            #endregion

            #region Process
            services.AddScoped<IProcMaskCodeRuleRepository, ProcMaskCodeRuleRepository>();
            services.AddScoped<IProcMaskCodeRepository, ProcMaskCodeRepository>();
            services.AddScoped<IProcProductSetRepository, ProcProductSetRepository>();


            #region Material
            services.AddScoped<IProcMaterialRepository, ProcMaterialRepository>();
            services.AddScoped<IProcReplaceMaterialRepository, ProcReplaceMaterialRepository>();

            services.AddScoped<IProcMaterialGroupRepository, ProcMaterialGroupRepository>();

            services.AddScoped<IProcMaterialSupplierRelationRepository, ProcMaterialSupplierRelationRepository>();
            #endregion

            #region Parameter
            services.AddScoped<IProcParameterRepository, ProcParameterRepository>();

            #endregion

            #region ParameterLinkType
            services.AddScoped<IProcParameterLinkTypeRepository, ProcParameterLinkTypeRepository>();

            #endregion

            #region Bom
            services.AddScoped<IProcBomRepository, ProcBomRepository>();
            services.AddScoped<IProcBomDetailRepository, ProcBomDetailRepository>();
            services.AddScoped<IProcBomDetailReplaceMaterialRepository, ProcBomDetailReplaceMaterialRepository>();
            #endregion

            #region LoadPoint
            services.AddScoped<IProcLoadPointRepository, ProcLoadPointRepository>();

            #endregion

            #region LoadPointLink
            services.AddScoped<IProcLoadPointLinkMaterialRepository, ProcLoadPointLinkMaterialRepository>();
            services.AddScoped<IProcLoadPointLinkResourceRepository, ProcLoadPointLinkResourceRepository>();
            #endregion

            #region Resource
            services.AddScoped<IProcResourceTypeRepository, ProcResourceTypeRepository>();
            services.AddScoped<IProcResourceRepository, ProcResourceRepository>();
            services.AddScoped<IProcResourceConfigPrintRepository, ProcResourceConfigPrintRepository>();
            services.AddScoped<IProcResourceConfigResRepository, ProcResourceConfigResRepository>();
            services.AddScoped<IProcResourceEquipmentBindRepository, ProcResourceEquipmentBindRepository>();
            #endregion

            #region Procedure
            services.AddScoped<IProcProcedureRepository, ProcProcedureRepository>();
            services.AddScoped<IProcProcedurePrintRelationRepository, ProcProcedurePrintRelationRepository>();
            #endregion

            #region ProduceSet
            services.AddScoped<IProcProductSetRepository, ProcProductSetRepository>();
            #endregion

            #region ProcessRoute
            services.AddScoped<IProcProcessRouteRepository, ProcProcessRouteRepository>();
            services.AddScoped<IProcProcessRouteDetailNodeRepository, ProcProcessRouteDetailNodeRepository>();
            services.AddScoped<IProcProcessRouteDetailLinkRepository, ProcProcessRouteDetailLinkRepository>();
            #endregion

            #region LabelTemplate
            services.AddScoped<IProcLabelTemplateRepository, ProcLabelTemplateRepository>();
            #endregion

            #region printConfig
            services.AddScoped<IProcPrintConfigRepository, ProcPrintConfigRepository>();
            #endregion
            #endregion

            #region Quality
            services.AddScoped<IQualUnqualifiedCodeRepository, QualUnqualifiedCodeRepository>();
            services.AddScoped<IQualUnqualifiedGroupRepository, QualUnqualifiedGroupRepository>();
            #endregion

            #region Manufacture
            services.AddScoped<IManuFeedingRepository, ManuFeedingRepository>();
            services.AddScoped<IManuFeedingRecordRepository, ManuFeedingRecordRepository>();
            services.AddScoped<IManuProductBadRecordRepository, ManuProductBadRecordRepository>();
            services.AddScoped<IManuSfcCirculationRepository, ManuSfcCirculationRepository>();
            services.AddScoped<IManuSfcInfoRepository, ManuSfcInfoRepository>();
            services.AddScoped<IManuSfcRepository, ManuSfcRepository>();
            services.AddScoped<IManuSfcProduceRepository, ManuSfcProduceRepository>();
            services.AddScoped<IManuSfcStepRepository, ManuSfcStepRepository>();
            services.AddScoped<IManuFacePlateRepository, ManuFacePlateRepository>();
            services.AddScoped<IManuFacePlateProductionRepository, ManuFacePlateProductionRepository>();
            services.AddScoped<IManuFacePlateRepairRepository, ManuFacePlateRepairRepository>();
            services.AddScoped<IManuFacePlateContainerPackRepository, ManuFacePlateContainerPackRepository>();
            services.AddScoped<IManuFacePlateButtonRepository, ManuFacePlateButtonRepository>();
            services.AddScoped<IManuFacePlateButtonJobRelationRepository, ManuFacePlateButtonJobRelationRepository>();

            services.AddScoped<IManuContainerPackRecordRepository, ManuContainerPackRecordRepository>();
            services.AddScoped<IManuContainerPackRepository, ManuContainerPackRepository>();
            services.AddScoped<IManuContainerBarcodeRepository, ManuContainerBarcodeRepository>();
            #endregion

            #region Warehouse 
            services.AddScoped<IWhSupplierRepository, WhSupplierRepository>();
            services.AddScoped<IWhMaterialInventoryRepository, WhMaterialInventoryRepository>();
            services.AddScoped<IWhMaterialStandingbookRepository, WhMaterialStandingbookRepository>();


            #endregion

            #region Plan
            //services.AddScoped<IPlanWorkOrderActivationRepository, PlanWorkOrderActivationRepository>();

            #region PlanWorkOrder
            services.AddScoped<IPlanWorkOrderRepository, PlanWorkOrderRepository>();
            services.AddScoped<IPlanWorkOrderStatusRecordRepository, PlanWorkOrderStatusRecordRepository>();
            #endregion

            #region PlanSfcReceive
            services.AddScoped<IPlanSfcReceiveRepository, PlanSfcReceiveRepository>();
            #endregion

            #region PlanWorkOrderActivation
            services.AddScoped<IPlanWorkOrderActivationRepository, PlanWorkOrderActivationRepository>();
            services.AddScoped<IPlanWorkOrderActivationRecordRepository, PlanWorkOrderActivationRecordRepository>();
            #endregion

            #region PlanWorkOrderBind/PlanWorkOrderBindRecord
            services.AddScoped<IPlanWorkOrderBindRepository, PlanWorkOrderBindRepository>();
            services.AddScoped<IPlanWorkOrderBindRecordRepository, PlanWorkOrderBindRecordRepository>();
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
            services.Configure<ConnectionOptions>(configuration.GetSection(nameof(ConnectionOptions)));
            //services.Configure<ConnectionOptions>(configuration);
            return services;
        }

    }
}
