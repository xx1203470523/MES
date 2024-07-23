using AutoMapper;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.EquEquipmentRecord;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Core.Domain.Equipment.EquSpotcheck;
using Hymson.MES.Core.Domain.EquRepairOrder;
using Hymson.MES.Core.Domain.EquSparepartInventory;
using Hymson.MES.Core.Domain.EquSparepartRecord;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Qual;
using Hymson.MES.Core.Domain.QualEnvOrder;
using Hymson.MES.Core.Domain.QualEnvOrderDetail;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Domain.WHMaterialReceipt;
using Hymson.MES.Core.Domain.WHMaterialReceiptDetail;
using Hymson.MES.Core.Domain.WhShipment;
using Hymson.MES.Core.Domain.WhWareHouse;
using Hymson.MES.Core.Domain.WhWarehouseLocation;
using Hymson.MES.Core.Domain.WhWarehouseRegion;
using Hymson.MES.Core.Domain.WhWarehouseShelf;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Integrated;
using Hymson.MES.Data.Repositories.EquEquipmentRecord;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;
using Hymson.MES.Data.Repositories.Equipment.EquMaintenance.EquMaintenanceItem.Query;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart.Query;
using Hymson.MES.Data.Repositories.Equipment.EquSparePartType.Query;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.Equipment.View;
using Hymson.MES.Data.Repositories.EquMaintenancePlan;
using Hymson.MES.Data.Repositories.EquMaintenanceTemplate;
using Hymson.MES.Data.Repositories.EquRepairOrder;
using Hymson.MES.Data.Repositories.EquSparepartInventory;
using Hymson.MES.Data.Repositories.EquSparepartRecord;
using Hymson.MES.Data.Repositories.EquSpotcheckPlan;
using Hymson.MES.Data.Repositories.EquSpotcheckTemplate;
using Hymson.MES.Data.Repositories.Inte;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteBusinessField.View;
using Hymson.MES.Data.Repositories.Integrated.InteCalendar.Query;
using Hymson.MES.Data.Repositories.Integrated.InteClass.Query;
using Hymson.MES.Data.Repositories.Integrated.InteContainer.Query;
using Hymson.MES.Data.Repositories.Integrated.InteJob.Query;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.Query;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcInfo.Query;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Marking;
using Hymson.MES.Data.Repositories.Marking.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Plan.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.LoadPointLink.Query;
using Hymson.MES.Data.Repositories.Process.MaskCode.Query;
using Hymson.MES.Data.Repositories.Process.ProcessRoute.Query;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Data.Repositories.Process.ResourceType.View;
using Hymson.MES.Data.Repositories.Process.View;
using Hymson.MES.Data.Repositories.Qual;
using Hymson.MES.Data.Repositories.QualEnvOrder;
using Hymson.MES.Data.Repositories.QualEnvOrderDetail;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.QualIpqcInspection.View;
using Hymson.MES.Data.Repositories.Quality.QualIpqcInspectionHead.View;
using Hymson.MES.Data.Repositories.Quality.QualIpqcInspectionPatrol.View;
using Hymson.MES.Data.Repositories.Quality.QualIpqcInspectionTail.View;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Data.Repositories.Quality.View;
using Hymson.MES.Data.Repositories.Query;
using Hymson.MES.Data.Repositories.Report.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Data.Repositories.WhShipment.Query;
using Hymson.MES.Data.Repositories.WhWareHouse.Query;
using Hymson.MES.Data.Repositories.WhWarehouseLocation.Query;
using Hymson.MES.Data.Repositories.WhWarehouseRegion.Query;
using Hymson.MES.Data.Repositories.WhWarehouseShelf.Query;
using Hymson.MES.Services.Dtos.EquEquipmentRecord;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Equipment.EquMaintenance;
using Hymson.MES.Services.Dtos.EquMaintenancePlan;
using Hymson.MES.Services.Dtos.EquMaintenanceTemplate;
using Hymson.MES.Services.Dtos.EquRepairOrder;
using Hymson.MES.Services.Dtos.EquSparepartInventory;
using Hymson.MES.Services.Dtos.EquSparepartRecord;
using Hymson.MES.Services.Dtos.EquSpotcheckPlan;
using Hymson.MES.Services.Dtos.EquSpotcheckTemplate;
using Hymson.MES.Services.Dtos.Inte;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuSfcOperateDto;
using Hymson.MES.Services.Dtos.Marking;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Qual;
using Hymson.MES.Services.Dtos.QualEnvOrder;
using Hymson.MES.Services.Dtos.QualEnvOrderDetail;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.MES.Services.Dtos.WHMaterialReceipt;
using Hymson.MES.Services.Dtos.WHMaterialReceiptDetail;
using Hymson.MES.Services.Dtos.WhShipment;
using Hymson.MES.Services.Dtos.WhWareHouse;
using Hymson.MES.Services.Dtos.WhWarehouseLocation;
using Hymson.MES.Services.Dtos.WhWarehouseRegion;
using Hymson.MES.Services.Dtos.WhWarehouseShelf;

namespace Hymson.MES.Services.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    public class MapperConfiguration : Profile, IOrderedMapperProfile
    {
        /// <summary>
        /// 映射
        /// </summary>
        public MapperConfiguration()
        {
            CreateEquipmentMaps();
            CreateIntegratedMaps();
            CreateProcessMaps();
            CreateQualityMaps();
            CreateWarehouseMaps();
            CreatePlanMaps();
            CreateManufactureMaps();
            CreateReportMaps();
        }

        /// <summary>
        /// 设备模块
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
            CreateMap<EquEquipmentSpotcheckRelationPagedQueryDto, EquEquipmentSpotcheckRelationPagedQuery>();

            CreateMap<EquEquipmentEntity, EquEquipmentDto>();
            CreateMap<EquEquipmentEntity, EquEquipmentListDto>();
            CreateMap<EquEquipmentPageView, EquEquipmentListDto>();
            CreateMap<GetEquSpotcheckPlanEquipmentRelationPageView, GetEquSpotcheckPlanEquipmentRelationListDto>();

            CreateMap<EquInspectionItemPagedQueryDto, EquInspectionItemPagedQuery>();
            CreateMap<EquInspectionItemSaveDto, EquInspectionItemEntity>();
            CreateMap<EquInspectionItemEntity, EquInspectionItemDto>();

            CreateMap<EquInspectionTaskPagedQueryDto, EquInspectionTaskPagedQuery>();
            CreateMap<EquInspectionTaskView, EquInspectionTaskDto>();
            CreateMap<EquInspectionTaskSaveDto, EquInspectionTaskEntity>();
            CreateMap<EquInspectionTaskEntity, EquInspectionTaskDto>();

            CreateMap<EquOperationPermissionsQueryDto, EquOperationPermissionsQuery>();
            CreateMap<EquOperationPermissionsQueryDto, EquOperationPermissionsPagedQuery>();
            CreateMap<EquOperationPermissionsSaveDto, EquOperationPermissionsEntity>();
            CreateMap<EquOperationPermissionsEntity, EquOperationPermissionsDto>();

            CreateMap<EquInspectionRecordPagedQueryDto, EquInspectionRecordPagedQuery>();
            CreateMap<EquInspectionRecordView, EquInspectionRecordDto>();
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
            CreateMap<EquFaultPhenomenonQueryDto, EquFaultPhenomenonQuery>();

            CreateMap<EquFaultPhenomenonEntity, EquFaultPhenomenonDto>();

            #endregion

            #region EquFaultReason
            CreateMap<EquFaultReasonPagedQueryDto, EquFaultReasonPagedQuery>();
            CreateMap<EquFaultReasonEntity, EquFaultReasonDto>();
            #endregion

            #region EquFaultSolution
            CreateMap<EquFaultSolutionSaveDto, EquFaultSolutionEntity>();
            CreateMap<EquFaultSolutionPagedQueryDto, EquFaultSolutionPagedQuery>();
            CreateMap<EquFaultSolutionEntity, EquFaultSolutionDto>();
            #endregion

            #region EquSparePart
            CreateMap<EquSparePartSaveDto, EquSparePartEntity>();
            CreateMap<EquSparePartPagedQueryDto, EquSparePartPagedQuery>();

            CreateMap<EquSparePartEntity, EquSparePartDto>();
            #endregion

            #region EquSparePartType
            CreateMap<EquSparePartTypeSaveDto, EquSparePartTypeEntity>();
            CreateMap<EquSparePartTypePagedQueryDto, EquSparePartTypePagedQuery>();
            CreateMap<EquSparePartsGroupSaveDto, EquSparePartsGroupEntity>();
            CreateMap<EquSparePartTypeEntity, EquSparePartTypeDto>();
            #endregion

            #region EquFaultReason
            CreateMap<EquFaultReasonSaveDto, EquFaultReasonEntity>();
            CreateMap<EquFaultReasonPagedQueryDto, EquFaultReasonPagedQuery>();
            CreateMap<EquFaultReasonEntity, EquFaultReasonDto>();
            CreateMap<EquFaultReasonEntity, CustomEquFaultReasonDto>();
            CreateMap<EquFaultReasonQueryDto, EquFaultReasonQuery>();
            #endregion

            #region EquEquipmentVerify
            CreateMap<EquEquipmentVerifyCreateDto, EquEquipmentVerifyEntity>();
            CreateMap<EquEquipmentVerifyPagedQueryDto, EquEquipmentVerifyPagedQuery>();

            CreateMap<EquEquipmentVerifyEntity, EquEquipmentVerifyDto>();

            #endregion

            #region EquSparePartsType
            CreateMap<EquSparePartsGroupPagedQueryDto, EquSparePartsGroupPagedQuery>();
            CreateMap<EquSparePartsGroupEntity, EquSparePartsGroupDto>();

            #endregion

            #region EquToolingType
            CreateMap<EquToolingTypeQueryDto, EquToolingTypePagedQuery>();
            CreateMap<EquToolingTypeEntity, EquToolingTypeDto>();
            CreateMap<EquToolingTypeSaveDto, EquToolingTypeEntity>();

            #endregion

            #region EquToolsManage
            CreateMap<EquToolingManagePagedQueryDto, IEquToolingManagePagedQuery>();
            CreateMap<EquToolingManageView, EquToolingManageViewDto>();
            CreateMap<AddEquToolingManageDto, EquToolsEntity>();
            CreateMap<EquToolingManageModifyDto, EquToolsEntity>();

            #endregion

            #region EquSpareParts
            CreateMap<EquSparePartsSaveDto, EquSparePartsEntity>();
            CreateMap<EquSparePartsPagedQueryDto, EquSparePartsPagedQuery>();
            CreateMap<EquSparePartsEntity, EquSparePartsDto>();
            #endregion

            #region EquipmentFaultType
            CreateMap<EquEquipmentFaultTypeEntity, EquipmentFaultTypeDto>();
            CreateMap<EQualUnqualifiedGroupCreateDto, EquEquipmentFaultTypeEntity>();
            CreateMap<EQualUnqualifiedGroupModifyDto, EquEquipmentFaultTypeEntity>();
            CreateMap<EquipmentFaultTypePagedQueryDto, EquipmentFaultTypePagedQuery>();
            #endregion            #region 设备点检

            #region
            CreateMap<EquSpotcheckItemSaveDto, EquSpotcheckItemEntity>();
            CreateMap<EquSpotcheckItemPagedQueryDto, EquSpotcheckItemPagedQuery>();

            CreateMap<EquSpotcheckItemEntity, EquSpotcheckItemDto>();
            CreateMap<EquSpotcheckItemUpdateDto, EquSpotcheckItemEntity>();

            //index
            CreateMap<EquSpotcheckTaskPagedQueryDto, EquSpotcheckTaskPagedQuery>();
            CreateMap<EquSpotcheckTaskUnionPlanEntity, EquSpotcheckTaskDto>();
            //item
            CreateMap<EquSpotcheckTaskSnapshotItemEntity, TaskItemUnionSnapshotView>();
            CreateMap<EquSpotcheckTaskItemEntity, TaskItemUnionSnapshotView>();

            #endregion

            #region 设备保养
            CreateMap<EquMaintenanceItemSaveDto, EquMaintenanceItemEntity>();
            CreateMap<EquMaintenanceItemPagedQueryDto, EquMaintenanceItemPagedQuery>();

            CreateMap<EquMaintenanceItemEntity, EquMaintenanceItemDto>();
            CreateMap<EquMaintenanceItemUpdateDto, EquMaintenanceItemEntity>();

            //task index
            CreateMap<EquMaintenanceTaskPagedQueryDto, EquMaintenanceTaskPagedQuery>();
            CreateMap<EquMaintenanceTaskUnionPlanEntity, EquMaintenanceTaskDto>();
            //item
            CreateMap<EquMaintenanceTaskSnapshotItemEntity, EquMaintenanceTaskItemUnionSnapshotView>();
            CreateMap<EquMaintenanceTaskItemEntity, EquMaintenanceTaskItemUnionSnapshotView>();

            #endregion


            #region EquSpotcheckTemplate
            CreateMap<EquSpotcheckTemplateCreateDto, EquSpotcheckTemplateEntity>();
            CreateMap<EquSpotcheckTemplateModifyDto, EquSpotcheckTemplateEntity>();
            CreateMap<EquSpotcheckTemplatePagedQueryDto, EquSpotcheckTemplatePagedQuery>();
            CreateMap<EquSpotcheckTemplateEntity, EquSpotcheckTemplateDto>();

            CreateMap<EquSpotcheckTemplateDto, EquSpotcheckTemplateEntity>();
            #endregion

            #region EquSpotcheckPlan
            CreateMap<EquSpotcheckPlanCreateDto, EquSpotcheckPlanEntity>();
            CreateMap<EquSpotcheckPlanModifyDto, EquSpotcheckPlanEntity>();
            CreateMap<EquSpotcheckPlanPagedQueryDto, EquSpotcheckPlanPagedQuery>();
            CreateMap<EquSpotcheckPlanEntity, EquSpotcheckPlanDto>();

            CreateMap<EquSpotcheckPlanDto, EquSpotcheckPlanEntity>();
            CreateMap<EquSpotcheckPlanEntity, EquSpotcheckPlanDto>();
            #endregion

            #region EquMaintenanceTemplate
            CreateMap<EquMaintenanceTemplateCreateDto, EquMaintenanceTemplateEntity>();
            CreateMap<EquMaintenanceTemplateModifyDto, EquMaintenanceTemplateEntity>();
            CreateMap<EquMaintenanceTemplatePagedQueryDto, EquMaintenanceTemplatePagedQuery>();
            CreateMap<EquMaintenanceTemplateEntity, EquMaintenanceTemplateDto>();

            CreateMap<EquMaintenanceTemplateDto, EquMaintenanceTemplateEntity>();
            #endregion

            #region EquMaintenancePlan
            CreateMap<EquMaintenancePlanCreateDto, EquMaintenancePlanEntity>();
            CreateMap<EquMaintenancePlanModifyDto, EquMaintenancePlanEntity>();
            CreateMap<EquMaintenancePlanPagedQueryDto, EquMaintenancePlanPagedQuery>();
            CreateMap<EquMaintenancePlanEntity, EquMaintenancePlanDto>();

            CreateMap<EquMaintenancePlanDto, EquMaintenancePlanEntity>();
            CreateMap<EquMaintenancePlanEntity, EquMaintenancePlanDto>();
            #endregion

            #region EquRepairOrder
            CreateMap<EquRepairOrderCreateDto, EquRepairOrderEntity>();
            CreateMap<EquRepairOrderModifyDto, EquRepairOrderEntity>();
            CreateMap<EquRepairOrderPagedQueryDto, EquRepairOrderPagedQuery>();
            CreateMap<EquRepairOrderEntity, EquRepairOrderDto>();
            CreateMap<EquRepairOrderPageView, EquRepairOrderDto>();

            CreateMap<EquRepairOrderDto, EquRepairOrderEntity>();
            #endregion

            #region EquSparepartInventory
            CreateMap<EquSparepartInventoryCreateDto, EquSparepartInventoryEntity>();
            CreateMap<EquSparepartInventoryModifyDto, EquSparepartInventoryEntity>();
            CreateMap<EquSparepartInventoryPagedQueryDto, EquSparepartInventoryPagedQuery>();
            CreateMap<EquSparepartInventoryEntity, EquSparepartInventoryDto>();
            CreateMap<EquSparepartInventoryPageView, EquSparepartInventoryPageDto>();

            CreateMap<EquSparepartInventoryDto, EquSparepartInventoryEntity>();
            CreateMap<EquSparePartEntity, EquSparepartRecordEntity>();
            #endregion

            #region EquEquipmentRecord
            CreateMap<EquEquipmentRecordPagedQueryDto, EquEquipmentRecordPagedQuery>();
            CreateMap<EquEquipmentRecordEntity, EquEquipmentRecordDto>();
            CreateMap<EquEquipmentRecordPagedView, EquEquipmentRecordPagedViewDto>();

            CreateMap<EquEquipmentRecordDto, EquEquipmentRecordEntity>();
            #endregion


            #region EquSparepartRecord
            CreateMap<EquSparepartRecordPagedQueryDto, EquSparepartRecordPagedQuery>();
            CreateMap<EquSparepartRecordEntity, EquSparepartRecordDto>();
            CreateMap<EquSparepartRecordPagedView, EquSparepartRecordPagedViewDto>();

            CreateMap<EquSparepartRecordDto, EquSparepartRecordEntity>();
            #endregion

            #region  EquSparepartEquipmentBindRecord
            CreateMap<EquSparepartEquipmentBindRecordPagedQueryDto, EquSparepartEquipmentBindRecordPagedQuery>();
            CreateMap<EquSparepartEquipmentBindRecordView, EquSparepartEquipmentBindRecordViewDto>();
            CreateMap<EquSparepartEquipmentBindRecordEntity, EquSparepartEquipmentBindRecordDto>();
            #endregion

            #region  EquToolsEquipmentBindRecord
            CreateMap<EquToolsEquipmentBindRecordPagedQueryDto, EquToolsEquipmentBindRecordPagedQuery>();
            CreateMap<EquToolsEquipmentBindRecordView, EquToolsEquipmentBindRecordViewDto>();
            CreateMap<EquToolsEquipmentBindRecordEntity, EquToolsEquipmentBindRecordDto>();
            #endregion

        }

        /// <summary>
        /// 综合模块
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

            //#region InteContainer
            CreateMap<InteContainerSaveDto, InteContainerEntity>();
            CreateMap<InteContainerPagedQueryDto, InteContainerPagedQuery>();

            CreateMap<InteContainerView, InteContainerDto>();
            CreateMap<InteContainerEntity, InteContainerDto>();
            //#endregion

            #region InteContainer
            //CreateMap<InteContainerSaveDto, InteContainerEntity>();
            //CreateMap<InteContainerPagedQueryDto, InteContainerPagedQuery>();
            //CreateMap<InteContainerSpecificationGroupsDto, InteContainerSpecificationEntity>();
            //CreateMap<InteContainerSaveDto, InteContainerInfoEntity>();
            //CreateMap<InteContainerInfoQueryDto, InteContainerPagedQuery>();
            //CreateMap<InteContainerFreightDto, InteContainerFreightEntity>();
            //CreateMap<InteContainerSaveDto, InteContainerSpecificationEntity>();

            #region 数据传输对象（操作对象）转换为实体对象

            CreateMap<InteContainerInfoDto, InteContainerInfoCreateCommand>();

            CreateMap<InteContainerInfoUpdateDto, InteContainerInfoUpdateCommand>();

            CreateMap<InteContainerSpecificationDto, InteContainerSpecificationCreateCommand>();

            CreateMap<InteContainerSpecificationUpdateDto, InteContainerSpecificationUpdateCommand>();

            CreateMap<InteContainerFreightDto, InteContainerFreightCreateCommand>();

            CreateMap<InteContainerFreightUpdateDto, InteContainerFreightUpdateCommand>();

            #endregion

            #region 数据传输对象（查询对象）转换为数据查询对象

            CreateMap<InteContainerInfoPagedQueryDto, InteContainerInfoPagedQuery>();

            CreateMap<InteContainerInfoQueryDto, InteContainerInfoQuery>();

            CreateMap<InteContainerSpecificationPagedQueryDto, InteContainerSpecificationPagedQuery>();

            CreateMap<InteContainerSpecificationQueryDto, InteContainerSpecificationQuery>();

            CreateMap<InteContainerFreightPagedQueryDto, InteContainerFreightPagedQuery>();

            CreateMap<InteContainerFreightQueryDto, InteContainerFreightQuery>();

            #endregion

            #region 实体对象转换为数据传输对象（页面输出）

            CreateMap<InteContainerInfoEntity, Dtos.Inte.InteContainerInfoOutputDto>();

            CreateMap<InteContainerSpecificationEntity, InteContainerSpecificationOutputDto>();

            CreateMap<InteContainerFreightEntity, InteContainerFreightOutputDto>();

            #endregion

            CreateMap<InteContainerInfoDto, InteContainerInfoEntity>();
            CreateMap<InteContainerInfoUpdateDto, InteContainerInfoEntity>();
            CreateMap<InteContainerSpecificationDto, InteContainerSpecificationEntity>();
            CreateMap<InteContainerFreightDto, InteContainerFreightEntity>();
            CreateMap<InteContainerInfoPagedQueryDto, InteContainerPagedQuery>();
            CreateMap<InteContainerView, InteContainerReDto>();
            CreateMap<InteContainerView, InteContainerInfoDto>();
            CreateMap<InteContainerEntity, InteContainerReDto>();

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

            CreateMap<InteCodeRulesPageView, InteCodeRulesPageViewDto>();
            CreateMap<InteCodeRulesEntity, InteCodeRulesDetailViewDto>();

            CreateMap<InteCodeRulesMakeCreateDto, InteCodeRulesMakeEntity>();
            CreateMap<InteCodeRulesMakeModifyDto, InteCodeRulesMakeEntity>();
            CreateMap<InteCodeRulesMakePagedQueryDto, InteCodeRulesMakePagedQuery>();
            CreateMap<InteCodeRulesMakeEntity, InteCodeRulesMakeDto>();
            CreateMap<InteCodeRulesMakeDto, InteCodeRulesMakeEntity>();
            #endregion

            #region InteSystemToken
            CreateMap<InteSystemTokenPagedQueryDto, InteSystemTokenPagedQuery>();
            CreateMap<InteSystemTokenEntity, InteSystemTokenDto>();
            CreateMap<InteSystemTokenCreateDto, InteSystemTokenEntity>();
            CreateMap<InteSystemTokenModifyDto, InteSystemTokenEntity>();
            #endregion

            #region InteCustom
            CreateMap<InteCustomPagedQueryDto, InteCustomPagedQuery>();
            CreateMap<InteCustomEntity, InteCustomDto>();
            CreateMap<InteCustomCreateDto, InteCustomEntity>();
            CreateMap<InteCustomModifyDto, InteCustomEntity>();

            #endregion

            #region InteVehicleType
            CreateMap<InteVehicleTypePagedQueryDto, InteVehicleTypePagedQuery>();
            CreateMap<InteVehicleTypeEntity, InteVehicleTypeDto>();
            CreateMap<InteVehicleTypeCreateDto, InteVehicleTypeEntity>();
            CreateMap<InteVehicleTypeModifyDto, InteVehicleTypeEntity>();

            CreateMap<InteVehicleTypeVerifyEntity, InteVehicleTypeVerifyDto>();
            #endregion

            #region InteVehicle
            CreateMap<InteVehiclePagedQueryDto, InteVehiclePagedQuery>();
            CreateMap<LineUpVehicleByProcedureIdDto, InteVehiclePagedQuery>();
            CreateMap<InteVehicleEntity, InteVehicleDto>();
            CreateMap<InteVehicleCreateDto, InteVehicleEntity>();
            CreateMap<InteVehicleModifyDto, InteVehicleEntity>();

            CreateMap<InteVehicleEntity, InteVehicleViewDto>();
            CreateMap<InteVehicleView, InteVehicleViewDto>();

            CreateMap<InteVehicleVerifyEntity, InteVehicleVerifyDto>();
            CreateMap<InteVehicleFreightEntity, InteVehicleFreightDto>();

            #endregion

            #region InteUnit
            CreateMap<InteUnitSaveDto, InteUnitEntity>();
            CreateMap<InteUnitPagedQueryDto, InteUnitPagedQuery>();
            CreateMap<InteUnitEntity, InteUnitDto>();
            #endregion

            #region InteMessageGroup
            CreateMap<InteMessageGroupSaveDto, InteMessageGroupEntity>();
            CreateMap<InteMessageGroupEntity, InteMessageGroupDto>();
            CreateMap<InteMessageGroupPagedQueryDto, InteMessageGroupPagedQuery>();
            CreateMap<InteMessageGroupView, InteMessageGroupDto>();

            CreateMap<InteMessageGroupPushMethodSaveDto, InteMessageGroupPushMethodEntity>();
            CreateMap<InteMessageGroupPushMethodEntity, InteMessageGroupPushMethodDto>();
            #endregion

            #region InteEvent
            CreateMap<InteEventPagedQueryDto, InteEventPagedQuery>();
            CreateMap<InteEventEntity, InteEventInfoDto>();
            CreateMap<InteEventEntity, InteEventBaseDto>();
            CreateMap<InteEventView, InteEventDto>();
            CreateMap<InteEventSaveDto, InteEventEntity>();
            #endregion

            #region InteEventType
            CreateMap<InteEventTypePagedQueryDto, InteEventTypePagedQuery>();
            CreateMap<InteEventTypeEntity, InteEventTypeDto>();
            CreateMap<InteEventTypeView, InteEventTypeDto>();
            CreateMap<InteEventTypeDto, InteEventTypeEntity>();
            CreateMap<InteEventTypeSaveDto, InteEventTypeEntity>();
            CreateMap<InteEventTypeMessageGroupRelationDto, InteEventTypeMessageGroupRelationEntity>();
            CreateMap<InteEventTypeUpgradeDto, InteEventTypeUpgradeEntity>();
            CreateMap<InteEventTypeMessageGroupRelationDto, InteEventTypeUpgradeMessageGroupRelationEntity>();
            CreateMap<InteEventTypePushRuleDto, InteEventTypePushRuleEntity>();
            CreateMap<InteEventTypePushRuleEntity, InteEventTypePushRuleDto>();
            CreateMap<InteEventTypeUpgradeEntity, InteEventTypeUpgradeDto>();
            CreateMap<InteEventTypeMessageGroupRelationEntity, MessageGroupBo>();
            CreateMap<InteEventTypeUpgradeMessageGroupRelationEntity, MessageGroupBo>();
            #endregion

            #region InteMessageManage
            CreateMap<InteMessageManageTriggerSaveDto, InteMessageManageEntity>();
            CreateMap<InteMessageManageReceiveSaveDto, InteMessageManageEntity>();
            CreateMap<InteMessageManageHandleSaveDto, InteMessageManageEntity>();
            CreateMap<InteMessageManageCloseSaveDto, InteMessageManageEntity>();
            CreateMap<InteMessageManagePagedQueryDto, InteMessageManagePagedQuery>();
            CreateMap<InteMessageManageEntity, InteMessageManageTriggerDto>();
            CreateMap<InteMessageManageEntity, InteMessageManageHandleDto>();
            CreateMap<InteMessageManageEntity, InteMessageManageCloseDto>();
            CreateMap<InteMessageManageView, InteMessageManageDto>();
            CreateMap<InteAttachmentEntity, InteAttachmentBaseDto>();
            #endregion

            #region InteCustomField
            CreateMap<InteCustomFieldBusinessEffectuateEntity, InteCustomFieldBusinessEffectuateDto>();
            #endregion

            #region SysReleaseRecord
            CreateMap<SysReleaseRecordEntity, SysReleaseRecordDto>();
            CreateMap<SysReleaseRecordDto, SysReleaseRecordEntity>();
            CreateMap<SysReleaseRecordCreateDto, SysReleaseRecordEntity>();
            CreateMap<SysReleaseRecordModifyDto, SysReleaseRecordEntity>();
            CreateMap<SysReleaseRecordPagedQueryDto, SysReleaseRecordPagedQuery>();
            CreateMap<SysReleaseRecordPagedQueryDto, SysReleaseRecordPagedQuery>();
            #endregion

            #region PlanShift
            CreateMap<PlanShiftPagedQueryDto, PlanShiftPagedQuery>();
            #endregion

            #region InteBusinessField
            CreateMap<InteBusinessFieldSaveDto, InteBusinessFieldEntity>();
            CreateMap<InteBusinessFieldPagedQueryDto, InteBusinessFieldPagedQuery>();
            CreateMap<InteBusinessFieldEntity, InteBusinessFieldDto>();
            CreateMap<InteBusinessFieldEntity, MaskInfoViewDto>();
            CreateMap<InteBusinessFieldListEntity, InteBusinessFieldListDto>();
            CreateMap<InteBusinessFieldView, MaskInfoViewDto>();

            #endregion

            #region InteBusinessFieldDistribute
            CreateMap<InteBusinessFieldDistributeSaveDto, InteBusinessFieldDistributeEntity>();
            CreateMap<InteBusinessFieldDistributePagedQueryDto, InteBusinessFieldDistributePagedQuery>();
            CreateMap<InteBusinessFieldDistributeEntity, InteBusinessFieldDistributeDto>();

            CreateMap<InteBusinessFieldDistributeDetailsEntity, InteBusinessFieldDistributeDetailsDto>();
            CreateMap<InteBusinessFieldDistributeDetailsEntity, BusinessFieldViewDto>();

            #endregion

            #region InteQualificationAuthentication
            CreateMap<InteQualificationAuthenticationPagedQueryDto, InteQualificationAuthenticationPagedQuery>();
            CreateMap<InteQualificationAuthenticationEntity, InteQualificationAuthenticationDto>();
            #endregion
        }

        /// <summary>
        /// 工艺模块
        /// </summary>
        protected virtual void CreateProcessMaps()
        {
            #region ProcProcessEquipmentGroup
            CreateMap<ProcProcessEquipmentGroupEntity, ProcProcessEquipmentGroupDto>();
            CreateMap<ProcProcessEquipmentGroupRelationEntity, ProcProcessEquipmentGroupRelationDto>();

            CreateMap<ProcProcessEquipmentGroupSaveDto, ProcProcessEquipmentGroupEntity>();
            CreateMap<ProcProcessEquipmentGroupPagedQueryDto, ProcProcessEquipmentGroupPagedQuery>();
            CreateMap<ProcProcessEquipmentGroupRelationSaveDto, ProcProcessEquipmentGroupRelationEntity>();
            CreateMap<ProcProcessEquipmentGroupRelationPagedQueryDto, ProcProcessEquipmentGroupRelationPagedQuery>();

            CreateMap<ProcProcessEquipmentGroupSaveDto, ProcProcessEquipmentGroupRelations>();
            CreateMap<ProcProcessEquipmentGroupEntity, ProcProcessEquipmentGroupListDto>();
            #endregion

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
            CreateMap<ProcMaterialImportDto, ProcMaterialEntity>();
            CreateMap<ProcMaterialPagedQueryDto, ProcMaterialPagedQuery>();
            CreateMap<ProcMaterialEntity, ProcMaterialDto>();
            CreateMap<ProcMaterialEntity, ProcMaterialExportDto>();
            CreateMap<ProcMaterialView, ProcMaterialViewDto>();

            CreateMap<ProcMaterialReplaceDto, ProcReplaceMaterialEntity>();
            CreateMap<ProcReplaceMaterialView, ProcMaterialReplaceViewDto>();

            CreateMap<ProcMaterialGroupCreateDto, ProcMaterialGroupEntity>();
            CreateMap<ProcMaterialGroupModifyDto, ProcMaterialGroupEntity>();
            CreateMap<ProcMaterialGroupPagedQueryDto, ProcMaterialGroupPagedQuery>();
            CreateMap<ProcMaterialGroupEntity, ProcMaterialGroupDto>();
            CreateMap<CustomProcMaterialGroupPagedQueryDto, ProcMaterialGroupCustomPagedQuery>();
            CreateMap<CustomProcMaterialGroupView, CustomProcMaterialGroupViewDto>();

            //物料供应商关系
            CreateMap<ProcMaterialSupplierRelationCreateDto, ProcMaterialSupplierRelationEntity>();
            CreateMap<ProcMaterialSupplierRelationDto, ProcMaterialSupplierRelationEntity>();
            CreateMap<ProcMaterialSupplierRelationPagedQueryDto, ProcMaterialSupplierRelationPagedQuery>();
            CreateMap<ProcMaterialSupplierRelationEntity, ProcMaterialSupplierRelationDto>();
            CreateMap<ProcMaterialSupplierView, ProcMaterialSupplierViewDto>();
            #endregion

            #region Parameter
            CreateMap<ProcParameterCreateDto, ProcParameterEntity>();
            CreateMap<ProcParameterModifyDto, ProcParameterEntity>();
            CreateMap<ProcParameterPagedQueryDto, ProcParameterPagedQuery>();
            CreateMap<ProcParameterEntity, ProcParameterDto>();
            CreateMap<ProcParameterEntity, CustomProcParameterDto>();
            CreateMap<ProcParameterEntity, ProcParameterExportDto>();
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
            CreateMap<ProcLoadPointPagedQuery, ProcLoadPointPagedQueryDto>();
            #endregion

            #region ConversionFactor
            CreateMap<AddConversionFactorDto, ProcConversionFactorEntity>();
            CreateMap<ProcProcedureView, ProcConversionFactorViewDto>();
            CreateMap<ProcConversionFactorPagedQueryDto, IProcConversionFactorPagedQuery>();
            CreateMap<ProcConversionFactorView, ProcConversionFactorViewDto>();
            CreateMap<ProcConversionFactorModifyDto, ProcConversionFactorEntity>();
            #endregion

            #region PrintSetup
            CreateMap<AddPrintSetupDto, ProcPrintSetupEntity>();
            CreateMap<ProcProcedureView, ProcPrintSetupViewDto>();
            CreateMap<ProcPrintSetupPagedQueryDto, IProcPrintSetupPagedQuery>();
            CreateMap<ProcPrintSetupView, ProcPrintSetupViewDto>();
            CreateMap<ProcPrintSetupModifyDto, ProcPrintSetupEntity>();
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

            CreateMap<ProcResourceProcedurePagedQueryDto, ProcResourceProcedurePagedQuery>();
            CreateMap<ProcResourcePagedlineIdAndProcProcedureIdDto, ProcResourcePagedlineIdAndProcProcedureIdQuery>();
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
            CreateMap<ProcessRouteProcedureQueryDto, ProcessRouteProcedureQuery>();
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

            CreateMap<ProcLabelTemplateRelationCreateDto, ProcLabelTemplateRelationEntity>();
            CreateMap<ProcLabelTemplateRelationEntity, ProcLabelTemplateRelationDto>();

            #endregion

            #region ProcSortingRule

            CreateMap<ProcSortingRuleEntity, ProcSortingRuleDto>();
            CreateMap<ProcSortingRuleCreateDto, ProcLabelTemplateEntity>();
            CreateMap<ProcSortingRuleModifyDto, ProcLabelTemplateEntity>();
            CreateMap<ProcSortingRulePagedQueryDto, ProcSortingRulePagedQuery>();
            #endregion

            #region ProcProductParameterGroup
            CreateMap<ProcProductParameterGroupSaveDto, ProcProductParameterGroupEntity>();
            CreateMap<ProcProductParameterGroupPagedQueryDto, ProcProductParameterGroupPagedQuery>();
            CreateMap<ProcProductParameterGroupView, ProcProductParameterGroupDto>();
            CreateMap<ProcProductParameterGroupEntity, ProcProductParameterGroupInfoDto>();
            CreateMap<ProcProductParameterGroupDetailSaveDto, ProcProductParameterGroupDetailEntity>();
            CreateMap<ProcProductParameterGroupDetailEntity, ProcProductParameterGroupDetailDto>();
            #endregion

            #region EquipmentGroupParam
            CreateMap<ProcEquipmentGroupParamEntity, ProcEquipmentGroupParamDto>();
            CreateMap<ProcEquipmentGroupParamDto, ProcEquipmentGroupParamEntity>();
            CreateMap<ProcEquipmentGroupParamModifyDto, ProcEquipmentGroupParamEntity>();
            CreateMap<ProcEquipmentGroupParamCreateDto, ProcEquipmentGroupParamEntity>();

            CreateMap<ProcEquipmentGroupParamPagedQueryDto, ProcEquipmentGroupParamPagedQuery>();
            CreateMap<ProcEquipmentGroupParamEntity, ProcEquipmentGroupParamViewDto>();
            CreateMap<ProcEquipmentGroupParamView, ProcEquipmentGroupParamViewDto>();

            CreateMap<ProcEquipmentGroupParamDetailEntity, ProcEquipmentGroupParamDetailDto>();
            CreateMap<ProcEquipmentGroupParamDetailCreateDto, ProcEquipmentGroupParamDetailEntity>();
            #endregion

            #region SortingRule
            CreateMap<ProcSortingRuleEntity, ProcSortingRuleDto>();
            CreateMap<ProcSortingRuleCreateDto, ProcSortingRuleEntity>();
            CreateMap<ProcSortingRuleModifyDto, ProcSortingRuleEntity>();


            CreateMap<ProcSortingRulePagedQueryDto, ProcSortingRulePagedQuery>();

            #endregion

            #region ProcFormula
            CreateMap<ProcFormulaEntity, ProcFormulaDto>();
            CreateMap<ProcFormulaSaveDto, ProcFormulaEntity>();

            CreateMap<ProcFormulaView, ProcFormulaViewDto>();
            CreateMap<ProcFormulaDetailsEntity, ProcFormulaDetailsDto>();
            CreateMap<ProcFormulaDetailsEntity, ProcFormulaDetailsViewDto>();
            CreateMap<ProcFormulaDetailsDto, ProcFormulaDetailsEntity>();

            CreateMap<ProcFormulaEntity, ProcFormulaViewDto>();

            CreateMap<ProcFormulaPagedQueryDto, ProcFormulaPagedQuery>();

            #endregion


            CreateMap<ProcEsopView, ProcEsopDto>();
            CreateMap<ProcEsopPagedQueryDto, ProcEsopPagedQuery>();

            #region Esop 

            #endregion

            #region ProcFormulaOperation
            CreateMap<ProcFormulaOperationEntity, ProcFormulaOperationDto>();
            CreateMap<ProcFormulaOperationEntity, ProcFormulaOperationSaveDto>();
            CreateMap<ProcFormulaOperationSaveDto, ProcFormulaOperationEntity>();
            CreateMap<ProcFormulaOperationPagedQueryDto, ProcFormulaOperationPagedQuery>();
            CreateMap<ProcFormulaOperationSetEntity, ProcFormulaOperationSetDto>();
            #endregion

            #region ProcFormulaOperationGroup
            CreateMap<ProcFormulaOperationGroupEntity, ProcFormulaOperationGroupDto>();
            CreateMap<ProcFormulaOperationGroupDto, ProcFormulaOperationGroupEntity>();
            CreateMap<ProcFormulaOperationGroupSaveDto, ProcFormulaOperationGroupEntity>();
            CreateMap<ProcFormulaOperationGroupEntity, ProcFormulaOperationGroupSaveDto>();
            CreateMap<ProcFormulaOperationGroupPagedQueryDto, ProcFormulaOperationGroupPagedQuery>();
            #endregion

            CreateMap<ProcProcedureTimeControlPagedQueryDto, ProcProcedureTimeControlPagedQuery>();
            CreateMap<ProcProcedureTimeControlView, ProcProcedureTimeControlDto>();
            CreateMap<ProcProcedureTimeControlCreateDto, ProcProcedureTimeControlEntity>();
            CreateMap<ProcProcedureTimeControlModifyDto, ProcProcedureTimeControlEntity>();
            CreateMap<ProcProcedureTimeControlEntity, ProcProcedureTimeControlDetailDto>();

            #region ProcProcedureSubstep
            CreateMap<ProcProcedureSubstepSaveDto, ProcProcedureSubstepEntity>();
            CreateMap<ProcProcedureSubstepEntity, ProcProcedureSubstepDto>();
            CreateMap<ProcProcedureSubstepPagedQueryDto, ProcProcedureSubstepPagedQuery>();
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

            CreateMap<WhMaterialInventoryEntity, WhMaterialInventoryDetailDto>();
            CreateMap<WhMaterialInventoryEntity, WhMaterialInventoryEntity>();
            #endregion

            #region WhMaterialStandingbook
            CreateMap<WhMaterialStandingbookCreateDto, WhMaterialStandingbookEntity>();
            CreateMap<WhMaterialStandingbookModifyDto, WhMaterialStandingbookEntity>();
            CreateMap<WhMaterialStandingbookPagedQueryDto, WhMaterialStandingbookPagedQuery>();
            CreateMap<WhMaterialStandingbookEntity, WhMaterialStandingbookDto>();
            #endregion

            #region WhWareHouse

            CreateMap<WhWarehousePagedQueryDto, WhWarehousePagedQuery>();
            CreateMap<WhWarehouseSaveDto, WhWarehouseEntity>();
            CreateMap<WhWarehouseEntity, WhWarehouseDto>();
            CreateMap<WhWarehouseModifyDto, WhWarehouseEntity>();

            #endregion

            #region WhWareHouseRegion

            CreateMap<WhWarehouseRegionPagedQueryDto, WhWarehouseRegionPagedQuery>();
            CreateMap<WhWarehouseRegionSaveDto, WhWarehouseRegionEntity>();
            CreateMap<WhWarehouseRegionEntity, WhWarehouseRegionDto>();
            CreateMap<WhWarehouseRegionModifyDto, WhWarehouseRegionEntity>();

            #endregion

            #region WhWareHouseShelf

            CreateMap<WhWarehouseShelfPagedQueryDto, WhWarehouseShelfPagedQuery>();
            CreateMap<WhWarehouseShelfSaveDto, WhWarehouseShelfEntity>();
            CreateMap<WhWarehouseShelfEntity, WhWarehouseShelfDto>();
            CreateMap<WhWarehouseShelfModifyDto, WhWarehouseShelfEntity>();

            #endregion

            #region WhWareHouseLocation

            CreateMap<WhWarehouseLocationPagedQueryDto, WhWarehouseLocationPagedQuery>();
            CreateMap<WhWarehouseLocationSaveDto, WhWarehouseLocationEntity>();
            CreateMap<WhWarehouseLocationEntity, WhWarehouseLocationDto>();
            CreateMap<WhWarehouseLocationQueryDto, WhWarehouseLocationQuery>();
            CreateMap<WhWarehouseLocationModifyDto, WhWarehouseLocationEntity>();

            #endregion

            #region WhShipment
            CreateMap<WhShipmentSaveDto, WhShipmentEntity>();
            CreateMap<WhShipmentEntity, WhShipmentDto>();
            CreateMap<WhShipmentPagedQueryDto, WhShipmentPagedQuery>();
            #endregion




            #region WhMaterialReceipt
            CreateMap<WhMaterialReceiptEntity, WhMaterialReceiptOutDto>();
            CreateMap<WhMaterialReceiptSaveDto, WhMaterialReceiptEntity>();
            CreateMap<WhMaterialReceiptEntity, WhMaterialReceiptDto>();
            CreateMap<WHMaterialReceiptDetailEntity, ReceiptMaterialDetailDto>();

            CreateMap<WHMaterialReceiptDetailEntity, WHMaterialReceiptDetailDto>();
            CreateMap<WhMaterialReceiptPagedQueryDto, WhMaterialReceiptPagedQuery>();
            #endregion

        }

        /// <summary>
        /// 质量模块
        /// </summary>
        protected virtual void CreateQualityMaps()
        {
            #region QualEnvParameterGroup
            CreateMap<QualEnvParameterGroupSaveDto, QualEnvParameterGroupEntity>();
            CreateMap<QualEnvParameterGroupPagedQueryDto, QualEnvParameterGroupPagedQuery>();
            CreateMap<QualEnvParameterGroupView, QualEnvParameterGroupDto>();
            CreateMap<QualEnvParameterGroupEntity, QualEnvParameterGroupInfoDto>();
            CreateMap<QualEnvParameterGroupDetailSaveDto, QualEnvParameterGroupDetailEntity>();
            CreateMap<QualEnvParameterGroupDetailEntity, QualEnvParameterGroupDetailDto>();
            #endregion

            #region QualInspectionParameterGroup
            CreateMap<QualInspectionParameterGroupSaveDto, QualInspectionParameterGroupEntity>();
            CreateMap<QualInspectionParameterGroupPagedQueryDto, QualInspectionParameterGroupPagedQuery>();
            CreateMap<QualInspectionParameterGroupView, QualInspectionParameterGroupDto>();
            CreateMap<QualInspectionParameterGroupEntity, QualInspectionParameterGroupInfoDto>();
            CreateMap<QualInspectionParameterGroupDetailSaveDto, QualInspectionParameterGroupDetailEntity>();
            CreateMap<QualInspectionParameterGroupDetailEntity, QualInspectionParameterGroupDetailDto>();
            CreateMap<QualInspectionParameterGroupDetailPagedQueryDto, QualInspectionParameterGroupDetailPagedQuery>();
            CreateMap<QualInspectionParameterGroupDetailView, QualInspectionParameterGroupDetailViewDto>();
            #endregion

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

            #region QualIpqcInspection
            CreateMap<QualIpqcInspectionView, QualIpqcInspectionViewDto>();
            CreateMap<QualIpqcInspectionPagedQueryDto, QualIpqcInspectionPagedQuery>();
            CreateMap<QualIpqcInspectionSaveDto, QualIpqcInspectionEntity>();
            CreateMap<QualIpqcInspectionEntity, QualIpqcInspectionDto>();
            CreateMap<QualIpqcInspectionParameterSaveDto, QualIpqcInspectionParameterEntity>();
            CreateMap<QualIpqcInspectionParameterEntity, QualIpqcInspectionParameterDto>();
            CreateMap<QualIpqcInspectionRuleSaveDto, QualIpqcInspectionRuleEntity>();
            CreateMap<QualIpqcInspectionRuleEntity, QualIpqcInspectionRuleDto>();
            CreateMap<QualIpqcInspectionRuleResourceRelationSaveDto, QualIpqcInspectionRuleResourceRelationEntity>();
            CreateMap<QualIpqcInspectionRuleResourceRelationEntity, QualIpqcInspectionRuleResourceRelationDto>();
            #endregion

            #region QualIpqcInspectionHead
            CreateMap<QualIpqcInspectionHeadPagedQueryDto, QualIpqcInspectionHeadPagedQuery>();
            CreateMap<QualIpqcInspectionHeadSampleCreateDto, QualIpqcInspectionHeadSampleEntity>();
            CreateMap<QualIpqcInspectionHeadView, QualIpqcInspectionHeadDto>();
            CreateMap<QualIpqcInspectionHeadEntity, QualIpqcInspectionHeadDto>();
            CreateMap<QualIpqcInspectionHeadSampleView, QualIpqcInspectionHeadSampleDto>();
            CreateMap<QualIpqcInspectionHeadSampleEntity, QualIpqcInspectionHeadSampleDto>();
            CreateMap<QualIpqcInspectionHeadAnnexEntity, QualIpqcInspectionHeadAnnexDto>();
            CreateMap<QualIpqcInspectionParameterEntity, SampleShouldInspectItemsDto>();
            CreateMap<QualIpqcInspectionHeadSamplePagedQueryDto, QualIpqcInspectionHeadSamplePagedQuery>();
            CreateMap<QualIpqcInspectionHeadSampleView, QualIpqcInspectionHeadSampleDto>();
            #endregion

            #region QualIpqcInspectionPatrol
            CreateMap<QualIpqcInspectionPatrolPagedQueryDto, QualIpqcInspectionPatrolPagedQuery>();
            CreateMap<QualIpqcInspectionHeadSampleCreateDto, QualIpqcInspectionHeadSampleEntity>();
            CreateMap<QualIpqcInspectionPatrolView, QualIpqcInspectionPatrolDto>();
            CreateMap<QualIpqcInspectionPatrolEntity, QualIpqcInspectionPatrolDto>();
            CreateMap<QualIpqcInspectionPatrolSampleView, QualIpqcInspectionPatrolSampleDto>();
            CreateMap<QualIpqcInspectionPatrolSampleEntity, QualIpqcInspectionPatrolSampleDto>();
            CreateMap<QualIpqcInspectionPatrolAnnexEntity, QualIpqcInspectionPatrolAnnexDto>();
            CreateMap<QualIpqcInspectionPatrolSamplePagedQueryDto, QualIpqcInspectionPatrolSamplePagedQuery>();
            #endregion

            #region QualIpqcInspectionTail
            CreateMap<QualIpqcInspectionTailPagedQueryDto, QualIpqcInspectionTailPagedQuery>();
            CreateMap<QualIpqcInspectionTailSampleCreateDto, QualIpqcInspectionTailSampleEntity>();
            CreateMap<QualIpqcInspectionTailView, QualIpqcInspectionTailDto>();
            CreateMap<QualIpqcInspectionTailEntity, QualIpqcInspectionTailDto>();
            CreateMap<QualIpqcInspectionTailSampleView, QualIpqcInspectionTailSampleDto>();
            CreateMap<QualIpqcInspectionTailSampleEntity, QualIpqcInspectionTailSampleDto>();
            CreateMap<QualIpqcInspectionTailAnnexEntity, QualIpqcInspectionTailAnnexDto>();
            CreateMap<QualIpqcInspectionTailSamplePagedQueryDto, QualIpqcInspectionTailSamplePagedQuery>();
            CreateMap<QualIpqcInspectionPatrolSampleCreateDto, QualIpqcInspectionPatrolSampleEntity>();
            #endregion

            CreateMap<QualIqcLevelPagedQueryDto, QualIqcLevelPagedQuery>();
            CreateMap<QualIqcLevelEntity, QualIqcLevelDto>();
            CreateMap<QualIqcLevelSaveDto, QualIqcLevelEntity>();
            CreateMap<QualIqcLevelDetailDto, QualIqcLevelDetailEntity>();
            CreateMap<QualIqcLevelDetailEntity, QualIqcLevelDetailDto>();

            CreateMap<QualOqcLevelPagedQueryDto, QualOqcLevelPagedQuery>();
            CreateMap<QualOqcLevelEntity, QualOqcLevelDto>();
            CreateMap<QualOqcLevelSaveDto, QualOqcLevelEntity>();
            CreateMap<QualOqcLevelDetailDto, QualOqcLevelDetailEntity>();
            CreateMap<QualOqcLevelDetailEntity, QualOqcLevelDetailDto>();

            CreateMap<QualIqcOrderPagedQueryDto, QualIqcOrderPagedQuery>();
            CreateMap<QualIqcOrderEntity, QualIqcOrderDto>();
            CreateMap<QualIqcOrderTypeEntity, QualIqcOrderTypeBaseDto>();
            CreateMap<QualIqcInspectionItemDetailSnapshotEntity, OrderParameterDetailDto>();
            CreateMap<QualIqcOrderSampleDetailEntity, OrderParameterDetailDto>();
            CreateMap<QualIqcOrderSaveDto, QualIqcOrderSampleQuery>();
            CreateMap<OrderParameterDetailQueryDto, QualIqcOrderSampleQuery>();
            CreateMap<OrderParameterDetailDto, QualIqcOrderSampleDetailEntity>();
            CreateMap<OrderParameterDetailPagedQueryDto, QualIqcOrderSampleDetailPagedQuery>();

            #region QualIqcInspectionItem

            #region 数据传输对象（操作对象）转换为实体对象

            CreateMap<QualIqcInspectionItemDto, QualIqcInspectionItemCreateCommand>();

            CreateMap<QualIqcInspectionItemUpdateDto, QualIqcInspectionItemUpdateCommand>();

            #endregion

            #region 数据传输对象（查询对象）转换为数据查询对象

            CreateMap<QualIqcInspectionItemPagedQueryDto, QualIqcInspectionItemPagedQuery>();

            CreateMap<QualIqcInspectionItemQueryDto, QualIqcInspectionItemQuery>();

            #endregion

            #region 实体对象转换为数据传输对象（页面输出）

            CreateMap<QualIqcInspectionItemEntity, QualIqcInspectionItemOutputDto>();

            #endregion

            #endregion

            #region QualIqcInspectionItemDetail

            #region 数据传输对象（操作对象）转换为实体对象

            CreateMap<QualIqcInspectionItemDetailDto, QualIqcInspectionItemDetailCreateCommand>();

            CreateMap<QualIqcInspectionItemDetailUpdateDto, QualIqcInspectionItemDetailUpdateCommand>();

            #endregion

            #region 数据传输对象（查询对象）转换为数据查询对象

            CreateMap<QualIqcInspectionItemDetailPagedQueryDto, QualIqcInspectionItemDetailPagedQuery>();

            CreateMap<QualIqcInspectionItemDetailQueryDto, QualIqcInspectionItemDetailQuery>();

            #endregion

            #region 实体对象转换为数据传输对象（页面输出）

            CreateMap<QualIqcInspectionItemDetailEntity, QualIqcInspectionItemDetailOutputDto>();

            #endregion

            #endregion

            #region QualOqcInspection OQC检验单

            CreateMap<QualOqcOrderPagedQueryDto, QualOqcOrderPagedQuery>();
            CreateMap<QualOqcOrderEntity, QualOqcOrderDto>();
            CreateMap<QualOqcParameterGroupDetailSnapshootEntity, CheckBarCodeOutDto>();
            CreateMap<QualOqcOrderExecSaveDto, QualOqcOrderSampleQuery>();
            CreateMap<QualOqcParameterGroupDetailSnapshootEntity, OqcOrderParameterDetailDto>();
            CreateMap<OqcOrderParameterDetailPagedQueryDto, QualOqcOrderSampleDetailPagedQuery>();
            CreateMap<QualOqcOrderTypeEntity, QualOqcOrderTypeDto>();
            CreateMap<QualOqcOrderTypeEntity, SampleQtyAndCheckedQtyQueryOutDto>();

            #region OQC检验项目
            //查询对象
            CreateMap<QualOqcParameterGroupQueryDto, QualOqcParameterGroupToQuery>();
            CreateMap<QualOqcParameterGroupDetailQueryDto, QualOqcParameterGroupDetailQuery>();
            //创建对象
            CreateMap<QualOqcParameterGroupDto, QualOqcParameterGroupCreateCommand>();
            CreateMap<QualOqcParameterGroupDetailDto, QualOqcParameterGroupDetailCreateCommand>();
            //更新对象
            CreateMap<QualOqcParameterGroupUpdateDto, QualOqcParameterGroupUpdateCommand>();
            CreateMap<QualOqcParameterGroupDetailOutputDto, QualOqcParameterGroupDetailCreateCommand>();



            //实体对象转换为数据传输对象（页面输出)

            CreateMap<QualOqcParameterGroupEntity, QualOqcParameterGroupOutputDto>();
            CreateMap<QualOqcParameterGroupDetailEntity, QualOqcParameterGroupDetailOutputDto>();
            #endregion




            #endregion

            #region env环境检验单
            CreateMap<QualEnvOrderEntity, QualEnvOrderDto>();
            CreateMap<QualEnvOrderCreateDto, QualEnvOrderEntity>();
            CreateMap<QualEnvOrderModifyDto, QualEnvOrderEntity>();
            CreateMap<QualEnvOrderPagedQueryDto, QualEnvOrderPagedQuery>();


            CreateMap<QualEnvOrderDetailEntity, QualEnvOrderDetailDto>();
            CreateMap<QualEnvOrderDetailEntity, QualEnvOrderDetailExtendDto>();
            CreateMap<QualEnvOrderDetailCreateDto, QualEnvOrderDetailEntity>();
            CreateMap<QualEnvOrderDetailModifyDto, QualEnvOrderDetailEntity>();
            CreateMap<QualEnvOrderDetailPagedQueryDto, QualEnvOrderDetailPagedQuery>();
            #endregion

            #region FQC检测单
            CreateMap<QualFqcOrderPagedQueryDto, QualFqcOrderPagedQuery>();
            CreateMap<QualFqcOrderEntity, QualFqcOrderDto>();

            CreateMap<QualFinallyOutputRecordEntity, QualFinallyOutputRecordView>();
            CreateMap<QualFqcParameterGroupDetailSnapshootEntity, FQCParameterDetailDto>();
            CreateMap<QualFqcOrderSampleSaveDto, QualFqcOrderSampleQuery>();
            CreateMap<FQCParameterDetailPagedQueryDto, QualFqcOrderSampleDetailPagedQuery>();
            CreateMap<QualFqcParameterGroupEntity, QualFqcParameterGroupOutputDto>();
            CreateMap<QualFqcParameterGroupDetailEntity, QualFqcParameterGroupDetailOutputDto>();
            CreateMap<QualFqcParameterGroupDetailQueryDto, QualFqcParameterGroupDetailQuery>();
            CreateMap<QualFqcParameterGroupUpdateDto, QualFqcParameterGroupUpdateCommand>();

            CreateMap<QualFqcOrderSampleEntity, FqcSelectionView>();



            #endregion

            #region 车间物料不良录入
            CreateMap<QualMaterialUnqualifiedDataPagedQueryDto, QualMaterialUnqualifiedDataPagedQuery>();
            #endregion

            #region IQC_Lite
            CreateMap<QualIqcOrderLitePagedQueryDto, QualIqcOrderLitePagedQuery>();
            CreateMap<QualIqcOrderLiteEntity, QualIqcOrderLiteDto>();
            CreateMap<QualIqcOrderLiteEntity, QualIqcOrderLiteBaseDto>();
            CreateMap<QualIqcOrderLiteDetailEntity, QualIqcOrderLiteDetailDto>();
            #endregion

            #region IQC_Return
            CreateMap<QualIqcOrderReturnPagedQueryDto, QualIqcOrderReturnPagedQuery>();
            CreateMap<QualIqcOrderReturnEntity, QualIqcOrderReturnDto>();
            CreateMap<QualIqcOrderReturnEntity, QualIqcOrderReturnBaseDto>();
            CreateMap<QualIqcOrderReturnDetailEntity, QualIqcOrderReturnDetailDto>();
            #endregion

        }

        /// <summary>
        /// 生产模块
        /// </summary>
        protected virtual void CreateManufactureMaps()
        {
            #region ManuFeeding
            CreateMap<ManuFeedingMaterialSaveDto, ManuFeedingEntity>();
            #endregion

            #region QualityLock
            CreateMap<ManuSfcProducePagedQueryDto, ManuSfcProducePagedQuery>();
            CreateMap<ManuSfcProduceSelectPagedQueryDto, ManuSfcProduceSelectPagedQuery>();
            #endregion

            #region ManuProductBadRecord

            CreateMap<ManuProductBadRecordEntity, ManuProductBadRecordDto>();
            CreateMap<ManuProductBadRecordCreateDto, ManuProductBadRecordEntity>();
            #endregion

            #region ManuFacePlate
            //FacePlate
            CreateMap<ManuFacePlatePagedQueryDto, ManuFacePlatePagedQuery>();
            CreateMap<ManuFacePlateEntity, ManuFacePlateDto>();
            CreateMap<ManuFacePlateCreateDto, ManuFacePlateEntity>();
            CreateMap<ManuFacePlateModifyDto, ManuFacePlateEntity>();
            //Production
            CreateMap<ManuFacePlateProductionCreateDto, ManuFacePlateProductionEntity>();
            CreateMap<ManuFacePlateProductionModifyDto, ManuFacePlateProductionEntity>();
            CreateMap<ManuFacePlateProductionEntity, ManuFacePlateProductionDto>();
            //Repair
            CreateMap<ManuFacePlateRepairCreateDto, ManuFacePlateRepairEntity>();
            CreateMap<ManuFacePlateRepairModifyDto, ManuFacePlateRepairEntity>();
            CreateMap<ManuFacePlateRepairEntity, ManuFacePlateRepairDto>();
            //containerPack
            CreateMap<ManuFacePlateContainerPackCreateDto, ManuFacePlateContainerPackEntity>();
            CreateMap<ManuFacePlateContainerPackModifyDto, ManuFacePlateContainerPackEntity>();
            CreateMap<ManuFacePlateContainerPackEntity, ManuFacePlateContainerPackDto>();
            //button
            CreateMap<ManuFacePlateButtonPagedQueryDto, ManuFacePlateButtonPagedQuery>();
            CreateMap<ManuFacePlateButtonModifyDto, ManuFacePlateButtonEntity>();
            CreateMap<ManuFacePlateButtonCreateDto, ManuFacePlateButtonEntity>();
            CreateMap<ManuFacePlateButtonEntity, ManuFacePlateButtonDto>();
            //buttonJobRelation
            CreateMap<ManuFacePlateButtonJobRelationModifyDto, ManuFacePlateButtonJobRelationEntity>();
            CreateMap<ManuFacePlateButtonJobRelationCreateDto, ManuFacePlateButtonJobRelationEntity>();
            CreateMap<ManuFacePlateButtonJobRelationEntity, ManuFacePlateButtonJobRelationDto>();
            #endregion

            #region ManuSfcProduce
            CreateMap<ManuSfcProduceEntity, ManuSfcProduceDto>();
            CreateMap<ManuSfcProduceVehiclePagedQueryDto, ManuSfcProduceVehiclePagedQuery>();
            #endregion

            #region ContainerPack
            CreateMap<ManuContainerPackPagedQueryDto, ManuContainerPackPagedQuery>();
            CreateMap<ManuContainerPackEntity, ManuContainerPackDto>();
            CreateMap<ManuContainerPackView, ManuContainerPackDto>();
            CreateMap<ManuContainerPackCreateDto, ManuContainerPackEntity>();
            CreateMap<ManuContainerPackModifyDto, ManuContainerPackEntity>();

            CreateMap<ManuContainerBarcodeCreateDto, ManuContainerBarcodeEntity>();
            CreateMap<ManuContainerBarcodePagedQueryDto, ManuContainerBarcodePagedQuery>();
            CreateMap<ManuContainerBarcodeDto, ManuContainerPackEntity>();
            CreateMap<ManuContainerBarcodeEntity, ManuContainerBarcodeDto>();
            CreateMap<ManuContainerBarcodeQueryView, ManuContainerBarcodeDto>();
            CreateMap<ManuContainerBarcodeModifyDto, ManuContainerBarcodeEntity>();
            CreateMap<CreateManuContainerBarcodeDto, ManuContainerBarcodeEntity>();

            CreateMap<ManuContainerPackRecordCreateDto, ManuContainerPackRecordEntity>();
            #endregion

            #region ContainerPackRecord
            CreateMap<ManuContainerPackRecordPagedQueryDto, ManuContainerPackRecordPagedQuery>();
            CreateMap<ManuContainerPackRecordEntity, ManuContainerPackRecordDto>();
            #endregion

            #region Baking
            CreateMap<ManuBakingCreateDto, ManuBakingEntity>();
            CreateMap<ManuBakingPagedQueryDto, ManuBakingPagedQuery>();
            CreateMap<ManuBakingModifyDto, ManuBakingEntity>();

            CreateMap<ManuBakingEntity, ManuBakingDto>();

            CreateMap<ManuBakingRecordCreateDto, ManuBakingRecordEntity>();
            CreateMap<ManuBakingRecordEntity, ManuBakingRecordDto>();
            CreateMap<ManuBakingRecordModifyDto, ManuBakingRecordEntity>();
            CreateMap<ManuBakingRecordPagedQueryDto, ManuBakingRecordPagedQuery>();
            #endregion

            #region ManuDowngradingRule
            CreateMap<ManuDowngradingRuleCreateDto, ManuDowngradingRuleEntity>();
            CreateMap<ManuDowngradingRulePagedQueryDto, ManuDowngradingRulePagedQuery>();
            CreateMap<ManuDowngradingRuleModifyDto, ManuDowngradingRuleEntity>();
            CreateMap<ManuDowngradingRuleEntity, ManuDowngradingRuleDto>();
            #endregion

            #region ManuDowngrading
            CreateMap<ManuDowngradingEntity, ManuDowngradingDto>();
            CreateMap<ManuDowngradingRecordEntity, ManuDowngradingRecordDto>();
            CreateMap<ManuDowngradingRecordPagedQueryDto, ManuDowngradingRecordPagedQuery>();
            #endregion

            #region ManuSfc
            CreateMap<ManuSfcAboutInfoPagedQueryDto, ManuSfcAboutInfoPagedQuery>();
            CreateMap<ManuSfcAboutInfoView, ManuSfcAboutInfoViewDto>();
            #endregion

            #region ManuSfcOperate

            CreateMap<ManuSfcInstationPagedQueryDto, ManuSfcProduceVehiclePagedQuery>();

            #endregion

            #region ManuProductExceptionHandling
            CreateMap<ManuProductNGBarCodeDto, ManuCompromiseBarCodeDto>();
            CreateMap<ManuProductBarCodeDto, ManuCompromiseBarCodeDto>();
            CreateMap<ManuProductNGBarCodeDto, ManuMisjudgmentBarCodeDto>();
            CreateMap<ManuProductBarCodeDto, ManuReworkBarCodeDto>();
            #endregion

            #region ManuJointProductAndByproductsReceiveRecord
            CreateMap<ManuJointProductAndByproductsReceiveRecordEntity, ManuJointProductAndByproductsReceiveRecordSaveDto>();
            CreateMap<ManuJointProductAndByproductsReceiveRecordPagedQueryDto, ManuJointProductAndByproductsReceiveRecordPagedQuery>();

            #endregion




            #region ManuReturnOrder
            CreateMap<ManuReturnOrderPagedQueryDto, ManuReturnOrderPagedQuery>();
            CreateMap<ManuReturnOrderEntity, ManuReturnOrderDto>();
            CreateMap<ManuReturnOrderDetailEntity, ManuReturnOrderDetailDto>();
            
            #endregion
        }

        /// <summary>
        /// 计划模块
        /// </summary>
        protected virtual void CreatePlanMaps()
        {
            #region WorkOrder
            CreateMap<PlanShiftEntity, PlanShiftDto>();
            CreateMap<PlanShiftDetailEntity, PlanShiftDetailDto>();
            CreateMap<PlanShiftSaveDto, PlanShiftEntity>();
            CreateMap<PlanWorkOrderCreateDto, PlanWorkOrderEntity>();
            CreateMap<PlanWorkOrderModifyDto, PlanWorkOrderEntity>();
            CreateMap<PlanWorkOrderPagedQueryDto, PlanWorkOrderPagedQuery>();
            CreateMap<PlanWorkOrderEntity, PlanWorkOrderDto>();
            CreateMap<PlanWorkOrderEntity, PlanWorkOrderDetailViewDto>();
            CreateMap<PlanWorkOrderListDetailView, PlanWorkOrderListDetailViewDto>();

            CreateMap<PlanWorkOrderEntity, PlanWorkOrderStatusRecordEntity>();
            #endregion

            #region PlanSfcReceive
            CreateMap<PlanSfcReceiveCreateDto, PlanSfcReceiveView>();
            CreateMap<PlanSfcReceiveModifyDto, PlanSfcReceiveView>();
            CreateMap<PlanSfcReceivePagedQueryDto, PlanSfcReceivePagedQuery>();
            CreateMap<PlanSfcReceiveView, PlanSfcReceiveDto>();
            #endregion

            #region WorkOrderActivation
            CreateMap<PlanWorkOrderActivationCreateDto, PlanWorkOrderActivationEntity>();
            CreateMap<PlanWorkOrderActivationModifyDto, PlanWorkOrderActivationEntity>();
            CreateMap<PlanWorkOrderActivationPagedQueryDto, PlanWorkOrderActivationPagedQuery>();
            CreateMap<PlanWorkOrderActivationEntity, PlanWorkOrderActivationDto>();
            CreateMap<PlanWorkOrderActivationListDetailView, PlanWorkOrderActivationListDetailViewDto>();

            CreateMap<PlanWorkOrderActivationAboutResPagedQueryDto, PlanWorkOrderActivationPagedQuery>();
            #endregion

            #region PlanSfcPrint
            CreateMap<PlanSfcPrintPagedQueryDto, ManuSfcPassDownPagedQuery>();
            CreateMap<PlanSfcPrintPagedQueryDto, ManuSfcProduceNewPagedQuery>();
            CreateMap<ManuSfcPassDownView, PlanSfcPrintDto>();
            CreateMap<ManuSfcProduceEntity, PlanSfcPrintDto>();
            #endregion

            #region PlanCalendar

            #region 数据传输对象（操作对象）转换为实体对象

            CreateMap<PlanCalendarDto, PlanCalendarCreateCommand>();

            CreateMap<PlanCalendarUpdateDto, PlanCalendarUpdateCommand>();

            #endregion

            #region 数据传输对象（查询对象）转换为数据查询对象

            CreateMap<PlanCalendarPagedQueryDto, PlanCalendarPagedQuery>();

            CreateMap<PlanCalendarQueryDto, PlanCalendarQuery>();

            #endregion

            #region 实体对象转换为数据传输对象（页面输出）

            CreateMap<PlanCalendarEntity, PlanCalendarOutputDto>();

            #endregion

            #endregion

            #region PlanCalendarDetail

            #region 数据传输对象（操作对象）转换为实体对象

            CreateMap<PlanCalendarDetailDto, PlanCalendarDetailCreateCommand>();

            CreateMap<PlanCalendarDetailUpdateDto, PlanCalendarDetailUpdateCommand>();

            #endregion

            #region 数据传输对象（查询对象）转换为数据查询对象

            CreateMap<PlanCalendarDetailPagedQueryDto, PlanCalendarDetailPagedQuery>();

            CreateMap<PlanCalendarDetailQueryDto, PlanCalendarDetailQuery>();

            #endregion

            #region 实体对象转换为数据传输对象（页面输出）

            CreateMap<PlanCalendarDetailEntity, PlanCalendarDetailOutputDto>();

            #endregion

            #endregion

            #region PlanShift

            CreateMap<PlanShiftEntity, PlanShiftDto>();

            #endregion

            CreateMap<PlanWorkPlanPagedQueryDto, PlanWorkPlanPagedQuery>();
            CreateMap<PlanWorkPlanEntity, PlanWorkPlanDto>();
            CreateMap<PlanWorkPlanProductPagedQueryDto, PlanWorkPlanProductPagedQuery>();
            CreateMap<PlanWorkPlanProductEntity, PlanWorkPlanProductDto>();
            CreateMap<PlanWorkPlanMaterialEntity, PlanWorkPlanMaterialDto>();

        }

        /// <summary>
        /// 报表
        /// </summary>
        protected virtual void CreateReportMaps()
        {
            #region BadRecordReport
            CreateMap<BadRecordReportDto, ManuProductBadRecordReportPagedQuery>();

            CreateMap<ManuProductBadRecordLogReportPagedQueryDto, ManuProductBadRecordLogReportPagedQuery>();
            CreateMap<ManuProductBadRecordLogReportView, ManuProductBadRecordLogReportViewDto>();
            #endregion

            #region WorkshopJobControl
            CreateMap<WorkshopJobControlReportPagedQueryDto, WorkshopJobControlReportPagedQuery>();
            CreateMap<WorkshopJobControlReportView, WorkshopJobControlReportViewDto>();

            CreateMap<WorkshopJobControlReportOptimizePagedQueryDto, WorkshopJobControlReportOptimizePagedQuery>();

            CreateMap<ManuSfcStepBySfcPagedQueryDto, ManuSfcStepBySfcPagedQuery>();
            #endregion

            #region ComUsageReport
            CreateMap<ComUsageReportPagedQueryDto, ComUsageReportPagedQuery>();

            #endregion

            #region WorkOrderControl
            CreateMap<WorkOrderControlReportPagedQueryDto, WorkOrderControlReportPagedQuery>();
            CreateMap<WorkshopJobControlReportView, WorkOrderControlReportViewDto>();

            CreateMap<WorkOrderControlReportOptimizePagedQueryDto, PlanWorkOrderPagedQuery>();

            CreateMap<ManuSfcStepBySfcPagedQueryDto, ManuSfcStepBySfcPagedQuery>();
            #endregion

            #region WorkOrderStepControl

            CreateMap<WorkOrderStepControlOptimizePagedQueryDto, PlanWorkOrderPagedQuery>();
            #endregion

            CreateMap<NodeSourceBo, NodeSourceDto>();

            #region ManuDowngradingDetailReport
            CreateMap<ManuDowngradingDetailReportPagedQueryDto, ManuDowngradingDetailReportPagedQuery>();
            #endregion

            CreateMap<VehicleFreightRecordQueryDto, InteVehicleFreightRecordPagedQuery>();

            #region MarkingInterceptReport
            CreateMap<MarkingInterceptReportPagedQueryDto, MarkingReportReportPagedQuery>();
            CreateMap<MarkingRecordQueryReportView, MarkingRecordReportDto>();
            #endregion
        }

        /// <summary>
        /// 排序，决定了加载的顺序
        /// </summary>
        public int Order => 0;

    }

}
