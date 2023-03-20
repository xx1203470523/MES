using AutoMapper;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;
using Hymson.MES.Data.Repositories.Equipment.EquFaultPhenomenon.Query;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart.Query;
using Hymson.MES.Data.Repositories.Equipment.EquSparePartType.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteCalendar.Query;
using Hymson.MES.Data.Repositories.Integrated.InteClass.Query;
using Hymson.MES.Data.Repositories.Integrated.InteContainer.Query;
using Hymson.MES.Data.Repositories.Integrated.InteJob.Query;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.MaskCode.Query;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Data.Repositories.Process.ResourceType.View;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedGroup.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Dtos.Warehouse;

namespace Hymson.MES.Services.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    public class MapperConfiguration : Profile, IOrderedMapperProfile
    {
        /// <summary>
        /// 
        /// </summary>
        public MapperConfiguration()
        {
            CreateEquipmentMaps();
            CreateIntegratedMaps();
            CreateProcessMaps();
            CreateQualityMaps();
            CreateWarehouseMaps();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateEquipmentMaps()
        {
            #region EquConsumable
            CreateMap<EquConsumableSaveDto, EquSparePartEntity>();
            CreateMap<EquConsumablePagedQueryDto, EquSparePartPagedQuery>();

            CreateMap<EquSparePartEntity, EquConsumableDto>();
            #endregion

            #region EquConsumableType
            CreateMap<EquConsumableTypeSaveDto, EquSparePartTypeEntity>();
            CreateMap<EquConsumableTypePagedQueryDto, EquSparePartTypePagedQuery>();

            CreateMap<EquSparePartTypeEntity, EquConsumableTypeDto>();
            #endregion

            #region EquEquipment
            CreateMap<EquEquipmentSaveDto, EquEquipmentEntity>();
            CreateMap<EquEquipmentPagedQueryDto, EquEquipmentPagedQuery>();

            CreateMap<EquEquipmentView, EquEquipmentDto>();
            CreateMap<EquEquipmentEntity, EquEquipmentListDto>();
            #endregion

            #region EquEquipmentGroup
            CreateMap<EquEquipmentGroupSaveDto, EquEquipmentGroupEntity>();
            CreateMap<EquEquipmentGroupPagedQueryDto, EquEquipmentGroupPagedQuery>();

            CreateMap<EquEquipmentGroupEntity, EquEquipmentGroupListDto>();
            CreateMap<EquEquipmentEntity, EquEquipmentBaseDto>();
            #endregion

            #region EquEquipmentLinkApi
            CreateMap<EquEquipmentLinkApiCreateDto, EquEquipmentLinkApiEntity>();
            CreateMap<EquEquipmentLinkApiModifyDto, EquEquipmentLinkApiEntity>();
            CreateMap<EquEquipmentLinkApiPagedQueryDto, EquEquipmentLinkApiPagedQuery>();

            CreateMap<EquEquipmentLinkApiEntity, EquEquipmentLinkApiBaseDto>();
            #endregion

            #region EquEquipmentLinkHardware
            CreateMap<EquEquipmentLinkHardwareCreateDto, EquEquipmentLinkHardwareEntity>();
            CreateMap<EquEquipmentLinkHardwareModifyDto, EquEquipmentLinkHardwareEntity>();
            CreateMap<EquEquipmentLinkHardwarePagedQueryDto, EquEquipmentLinkHardwarePagedQuery>();

            CreateMap<EquEquipmentLinkHardwareEntity, EquEquipmentLinkHardwareBaseDto>();
            #endregion

            #region EquEquipmentUnit
            CreateMap<EquEquipmentUnitSaveDto, EquEquipmentUnitEntity>();
            CreateMap<EquEquipmentUnitPagedQueryDto, EquEquipmentUnitPagedQuery>();

            CreateMap<EquEquipmentUnitEntity, EquEquipmentUnitDto>();
            #endregion

            #region EquFaultPhenomenon
            CreateMap<EquFaultPhenomenonSaveDto, EquFaultPhenomenonEntity>();
            CreateMap<EquFaultPhenomenonPagedQueryDto, EquFaultPhenomenonPagedQuery>();

            CreateMap<EquFaultPhenomenonView, EquFaultPhenomenonDto>();
            #endregion

            #region EquSparePart
            CreateMap<EquSparePartSaveDto, EquSparePartEntity>();
            CreateMap<EquSparePartPagedQueryDto, EquSparePartPagedQuery>();

            CreateMap<EquSparePartEntity, EquSparePartDto>();
            #endregion

            #region EquSparePartType
            CreateMap<EquSparePartTypeSaveDto, EquSparePartTypeEntity>();
            CreateMap<EquSparePartTypePagedQueryDto, EquSparePartTypePagedQuery>();

            CreateMap<EquSparePartTypeEntity, EquSparePartTypeDto>();
            #endregion

            #region EquFaultReason
            CreateMap<EquFaultReasonCreateDto, EquFaultReasonEntity>();
            CreateMap<EquFaultReasonModifyDto, EquFaultReasonEntity>();
            CreateMap<EquFaultReasonPagedQueryDto, EquFaultReasonPagedQuery>();
            CreateMap<EquFaultReasonEntity, EquFaultReasonDto>();
            CreateMap<EquFaultReasonEntity, CustomEquFaultReasonDto>();
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateIntegratedMaps()
        {
            #region InteCalendar
            CreateMap<InteCalendarSaveDto, InteCalendarEntity>();
            CreateMap<InteCalendarPagedQueryDto, InteCalendarPagedQuery>();

            CreateMap<InteCalendarView, InteCalendarDto>();
            #endregion

            #region InteClassDetail
            CreateMap<InteClassDetailSaveDto, InteClassDetailEntity>();
            #endregion

            #region InteClass
            CreateMap<InteClassSaveDto, InteClassEntity>();
            CreateMap<InteClassPagedQueryDto, InteClassPagedQuery>();

            CreateMap<InteClassEntity, InteClassDto>();
            #endregion

            #region InteContainer
            CreateMap<InteContainerSaveDto, InteContainerEntity>();
            CreateMap<InteContainerPagedQueryDto, InteContainerPagedQuery>();

            CreateMap<InteContainerView, InteContainerDto>();
            CreateMap<InteContainerEntity, InteContainerDto>();
            #endregion

            #region InteJob
            CreateMap<InteJobEntity, InteJobDto>();
            CreateMap<InteJobCreateDto, InteJobEntity>();
            CreateMap<InteJobModifyDto, InteJobEntity>();
            CreateMap<InteJobPagedQueryDto, InteJobPagedQuery>();
            CreateMap<InteJobBusinessRelationCreateDto, InteJobBusinessRelationEntity>();
            CreateMap<InteJobPagedQueryDto, InteJobPagedQuery>();
            #endregion

            #region InteWorkCenter
            CreateMap<InteWorkCenterEntity, InteWorkCenterDto>();
            CreateMap<InteWorkCenterCreateDto, InteWorkCenterEntity>();
            CreateMap<InteWorkCenterModifyDto, InteWorkCenterEntity>();
            CreateMap<InteWorkCenterPagedQueryDto, InteWorkCenterPagedQuery>();
            CreateMap<InteWorkCenterPagedQueryDto, InteWorkCenterPagedQuery>();
            #endregion

            #region CodeRule
            CreateMap<InteCodeRulesCreateDto, InteCodeRulesEntity>();
            CreateMap<InteCodeRulesModifyDto, InteCodeRulesEntity>();
            CreateMap<InteCodeRulesPagedQueryDto, InteCodeRulesPagedQuery>();
            CreateMap<InteCodeRulesEntity, InteCodeRulesDto>();

            CreateMap<InteCodeRulesMakeCreateDto, InteCodeRulesMakeEntity>();
            CreateMap<InteCodeRulesMakeModifyDto, InteCodeRulesMakeEntity>();
            CreateMap<InteCodeRulesMakePagedQueryDto, InteCodeRulesMakePagedQuery>();
            CreateMap<InteCodeRulesMakeEntity, InteCodeRulesMakeDto>();
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateProcessMaps()
        {
            #region MaskCode
            CreateMap<ProcMaskCodeRuleDto, ProcMaskCodeRuleEntity>();
            CreateMap<ProcMaskCodeSaveDto, ProcMaskCodeEntity>();
            CreateMap<ProcMaskCodePagedQueryDto, ProcMaskCodePagedQuery>();

            CreateMap<ProcMaskCodeRuleEntity, ProcMaskCodeRuleDto>();
            CreateMap<ProcMaskCodeEntity, ProcMaskCodeDto>();
            #endregion

            #region Material
            CreateMap<ProcMaterialCreateDto, ProcMaterialEntity>();
            CreateMap<ProcMaterialModifyDto, ProcMaterialEntity>();
            CreateMap<ProcMaterialPagedQueryDto, ProcMaterialPagedQuery>();
            CreateMap<ProcMaterialEntity, ProcMaterialDto>();
            CreateMap<ProcMaterialView, ProcMaterialViewDto>();

            CreateMap<ProcMaterialReplaceDto, ProcReplaceMaterialEntity>();
            CreateMap<ProcReplaceMaterialView, ProcMaterialReplaceViewDto>();

            CreateMap<ProcMaterialGroupCreateDto, ProcMaterialGroupEntity>();
            CreateMap<ProcMaterialGroupModifyDto, ProcMaterialGroupEntity>();
            CreateMap<ProcMaterialGroupPagedQueryDto, ProcMaterialGroupPagedQuery>();
            CreateMap<ProcMaterialGroupEntity, ProcMaterialGroupDto>();
            CreateMap<CustomProcMaterialGroupPagedQueryDto, ProcMaterialGroupCustomPagedQuery>();
            CreateMap<CustomProcMaterialGroupView, CustomProcMaterialGroupViewDto>();

            #endregion

            #region Parameter
            CreateMap<ProcParameterCreateDto, ProcParameterEntity>();
            CreateMap<ProcParameterModifyDto, ProcParameterEntity>();
            CreateMap<ProcParameterPagedQueryDto, ProcParameterPagedQuery>();
            CreateMap<ProcParameterEntity, ProcParameterDto>();
            CreateMap<ProcParameterEntity, CustomProcParameterDto>();
            #endregion

            #region ParameterLinkType
            CreateMap<ProcParameterLinkTypeCreateDto, ProcParameterLinkTypeEntity>();
            CreateMap<ProcParameterLinkTypeModifyDto, ProcParameterLinkTypeEntity>();
            CreateMap<ProcParameterLinkTypePagedQueryDto, ProcParameterLinkTypePagedQuery>();
            CreateMap<ProcParameterDetailPagerQueryDto, ProcParameterDetailPagerQuery>();
            CreateMap<ProcParameterLinkTypeEntity, ProcParameterLinkTypeDto>();
            CreateMap<ProcParameterLinkTypeView, ProcParameterLinkTypeViewDto>();
            #endregion

            #region Bom
            CreateMap<ProcBomCreateDto, ProcBomEntity>();
            CreateMap<ProcBomModifyDto, ProcBomEntity>();
            CreateMap<ProcBomPagedQueryDto, ProcBomPagedQuery>();
            CreateMap<ProcBomEntity, ProcBomDto>();

            CreateMap<ProcBomDetailCreateDto, ProcBomDetailEntity>();
            CreateMap<ProcBomDetailModifyDto, ProcBomDetailEntity>();
            CreateMap<ProcBomDetailPagedQueryDto, ProcBomDetailPagedQuery>();
            CreateMap<ProcBomDetailEntity, ProcBomDetailDto>();
            #endregion

            #region LoadPoint
            CreateMap<ProcLoadPointCreateDto, ProcLoadPointEntity>();
            CreateMap<ProcLoadPointModifyDto, ProcLoadPointEntity>();
            CreateMap<ProcLoadPointPagedQueryDto, ProcLoadPointPagedQuery>();
            CreateMap<ProcLoadPointEntity, ProcLoadPointDto>();
            #endregion

            #region LoadPointLink
            CreateMap<ProcLoadPointLinkMaterialCreateDto, ProcLoadPointLinkMaterialEntity>();
            CreateMap<ProcLoadPointLinkMaterialModifyDto, ProcLoadPointLinkMaterialEntity>();
            CreateMap<ProcLoadPointLinkMaterialPagedQueryDto, ProcLoadPointLinkMaterialPagedQuery>();
            CreateMap<ProcLoadPointLinkMaterialEntity, ProcLoadPointLinkMaterialDto>();
            CreateMap<ProcLoadPointLinkResourceCreateDto, ProcLoadPointLinkResourceEntity>();
            CreateMap<ProcLoadPointLinkResourceModifyDto, ProcLoadPointLinkResourceEntity>();
            CreateMap<ProcLoadPointLinkResourcePagedQueryDto, ProcLoadPointLinkResourcePagedQuery>();
            CreateMap<ProcLoadPointLinkResourceEntity, ProcLoadPointLinkResourceDto>();

            CreateMap<ProcLoadPointLinkMaterialView, ProcLoadPointLinkMaterialViewDto>();
            CreateMap<ProcLoadPointLinkResourceView, ProcLoadPointLinkResourceViewDto>();
            #endregion

            #region ResourceType

            CreateMap<ProcResourceTypeEntity, ProcResourceTypeDto>();
            CreateMap<ProcResourceTypeView, ProcResourceTypeViewDto>();
            CreateMap<ProcResourceTypePagedQueryDto, ProcResourceTypePagedQuery>();

            #endregion

            #region Resource

            CreateMap<ProcResourceEntity, ProcResourceDto>();
            CreateMap<ProcResourceView, ProcResourceViewDto>();
            CreateMap<ProcResourceCreateDto, ProcResourceEntity>();
            CreateMap<ProcResourcePagedQueryDto, ProcResourcePagedQuery>();

            CreateMap<ProcResourceConfigPrintView, ProcResourceConfigPrintViewDto>();
            CreateMap<ProcResourceConfigPrintPagedQueryDto, ProcResourceConfigPrintPagedQuery>();

            CreateMap<ProcResourceConfigResEntity, ProcResourceConfigResDto>();
            CreateMap<ProcResourceConfigResPagedQueryDto, ProcResourceConfigResPagedQuery>();

            CreateMap<ProcResourceEquipmentBindView, ProcResourceEquipmentBindViewDto>();
            CreateMap<ProcResourceEquipmentBindPagedQueryDto, ProcResourceEquipmentBindPagedQuery>();
            #endregion

            #region Procedure
            CreateMap<ProcProcedurePagedQueryDto, ProcProcedurePagedQuery>();
            CreateMap<ProcProcedureView, ProcProcedureViewDto>();
            CreateMap<ProcProcedureEntity, ProcProcedureDto>();
            CreateMap<ProcProcedureCreateDto, ProcProcedureEntity>();
            CreateMap<ProcProcedureModifyDto, ProcProcedureEntity>();
            CreateMap<ProcProcedurePrintRelationEntity, ProcProcedurePrintRelationDto>();
            CreateMap<ProcProcedurePrintReleationCreateDto, ProcProcedurePrintRelationEntity>();
            CreateMap<InteJobBusinessRelationEntity, InteJobBusinessRelationDto>();
            #endregion

            #region ProcessRoute
            CreateMap<ProcProcessRoutePagedQueryDto, ProcProcessRoutePagedQuery>();
            CreateMap<ProcProcessRouteEntity, ProcProcessRouteDto>();
            CreateMap<ProcProcessRouteCreateDto, ProcProcessRouteEntity>();
            CreateMap<ProcProcessRouteModifyDto, ProcProcessRouteEntity>();
            CreateMap<ProcProcessRouteDetailNodeView, ProcProcessRouteDetailNodeViewDto>();
            CreateMap<ProcProcessRouteDetailLinkEntity, ProcProcessRouteDetailLinkDto>();
            #endregion

            #region PrintConfig
            CreateMap<ProcPrinterDto, ProcPrinterEntity>();
            CreateMap<ProcPrinterEntity, ProcPrinterDto>();

            CreateMap<ProcPrinterPagedQueryDto, ProcPrinterPagedQuery>();
            #endregion

            #region LabelTemplate
            CreateMap<ProcLabelTemplateEntity, ProcLabelTemplateDto>();
            CreateMap<ProcLabelTemplateDto, ProcLabelTemplateEntity>();
            CreateMap<ProcLabelTemplateModifyDto, ProcLabelTemplateEntity>();
            CreateMap<ProcLabelTemplateCreateDto, ProcLabelTemplateEntity>();

            CreateMap<ProcLabelTemplatePagedQueryDto, ProcLabelTemplatePagedQuery>();
            #endregion


        }

        /// <summary>
        /// 库存
        /// </summary>
        protected virtual void CreateWarehouseMaps()
        {
            #region Warehouse
            CreateMap<WhSupplierCreateDto, WhSupplierEntity>();
            CreateMap<WhSupplierModifyDto, WhSupplierEntity>();
            CreateMap<WhSupplierPagedQueryDto, WhSupplierPagedQuery>();
            CreateMap<WhSupplierEntity, WhSupplierDto>();
            #endregion

            #region WhMaterialInventory
            CreateMap<WhMaterialInventoryCreateDto, WhMaterialInventoryEntity>();
            CreateMap<WhMaterialInventoryModifyDto, WhMaterialInventoryEntity>();
            CreateMap<WhMaterialInventoryPagedQueryDto, WhMaterialInventoryPagedQuery>();
            CreateMap<WhMaterialInventoryEntity, WhMaterialInventoryDto>();
            CreateMap<WhMaterialInventoryPageListView, WhMaterialInventoryPageListViewDto>();
            #endregion
            #region WhMaterialStandingbook
            CreateMap<WhMaterialStandingbookCreateDto, WhMaterialStandingbookEntity>();
            CreateMap<WhMaterialStandingbookModifyDto, WhMaterialStandingbookEntity>();
            CreateMap<WhMaterialStandingbookPagedQueryDto, WhMaterialStandingbookPagedQuery>();
            CreateMap<WhMaterialStandingbookEntity, WhMaterialStandingbookDto>();
            #endregion

        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateQualityMaps()
        {
            #region QualUnqualifiedCode
            CreateMap<QualUnqualifiedCodeEntity, QualUnqualifiedCodeDto>();
            CreateMap<QualUnqualifiedCodeCreateDto, QualUnqualifiedCodeEntity>();
            CreateMap<QualUnqualifiedCodeModifyDto, QualUnqualifiedCodeEntity>();
            CreateMap<QualUnqualifiedCodePagedQueryDto, QualUnqualifiedCodePagedQuery>();
            #endregion

            #region QualUnqualifiedCode
            CreateMap<QualUnqualifiedGroupEntity, QualUnqualifiedGroupDto>();
            CreateMap<QualUnqualifiedGroupCreateDto, QualUnqualifiedGroupEntity>();
            CreateMap<QualUnqualifiedGroupModifyDto, QualUnqualifiedGroupEntity>();
            CreateMap<QualUnqualifiedGroupPagedQueryDto, QualUnqualifiedGroupPagedQuery>();
            #endregion
        }

        /// <summary>
        /// 生产模块
        /// </summary>
        protected virtual void CreateManufactureMaps()
        {
            #region QualityLock
            CreateMap<ManuSfcProducePagedQueryDto, ManuSfcProducePagedQuery>();
            #endregion
        }
        /// <summary>
        /// 排序，决定了加载的顺序
        /// </summary>
        public int Order => 0;

    }
}
