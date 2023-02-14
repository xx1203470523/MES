using AutoMapper;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart.Query;
using Hymson.MES.Data.Repositories.Equipment.EquSparePartType.Query;
using Hymson.MES.Data.Repositories.Integrated.InteCalendar.Query;
using Hymson.MES.Data.Repositories.Integrated.InteClass.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Data.Repositories.Process.ResourceType.View;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;

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
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateEquipmentMaps()
        {
            #region EquEquipment
            CreateMap<EquEquipmentCreateDto, EquEquipmentEntity>();
            CreateMap<EquEquipmentModifyDto, EquEquipmentEntity>();
            CreateMap<EquEquipmentPagedQueryDto, EquEquipmentPagedQuery>();

            CreateMap<EquEquipmentEntity, EquEquipmentDto>();
            #endregion

            #region EquEquipmentGroup
            CreateMap<EquEquipmentGroupCreateDto, EquEquipmentGroupEntity>();
            CreateMap<EquEquipmentGroupModifyDto, EquEquipmentGroupEntity>();
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
            CreateMap<EquEquipmentUnitCreateDto, EquEquipmentUnitEntity>();
            CreateMap<EquEquipmentUnitModifyDto, EquEquipmentUnitEntity>();
            CreateMap<EquEquipmentUnitPagedQueryDto, EquEquipmentUnitPagedQuery>();

            CreateMap<EquEquipmentUnitEntity, EquEquipmentUnitDto>();
            #endregion

            #region EquSparePart
            CreateMap<EquSparePartCreateDto, EquSparePartEntity>();
            CreateMap<EquSparePartModifyDto, EquSparePartEntity>();
            CreateMap<EquSparePartPagedQueryDto, EquSparePartPagedQuery>();

            CreateMap<EquSparePartEntity, EquSparePartDto>();
            #endregion

            #region EquSparePartType
            CreateMap<EquSparePartTypeCreateDto, EquSparePartTypeEntity>();
            CreateMap<EquSparePartTypeModifyDto, EquSparePartTypeEntity>();
            CreateMap<EquSparePartTypePagedQueryDto, EquSparePartTypePagedQuery>();

            CreateMap<EquSparePartTypeEntity, EquSparePartTypeDto>();
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateIntegratedMaps()
        {
            #region InteCalendar
            CreateMap<InteCalendarCreateDto, InteCalendarEntity>();
            CreateMap<InteCalendarModifyDto, InteCalendarEntity>();
            CreateMap<InteCalendarPagedQueryDto, InteCalendarPagedQuery>();

            CreateMap<InteCalendarEntity, InteCalendarDto>();
            #endregion

            #region InteClassDetail
            CreateMap<InteClassDetailCreateDto, InteClassDetailEntity>();
            CreateMap<InteClassDetailModifyDto, InteClassDetailEntity>();
            #endregion

            #region InteClass
            CreateMap<InteClassCreateDto, InteClassEntity>();
            CreateMap<InteClassModifyDto, InteClassEntity>();
            CreateMap<InteClassPagedQueryDto, InteClassPagedQuery>();

            CreateMap<InteClassEntity, InteClassDto>();
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateProcessMaps()
        {
            #region Material
            CreateMap<ProcMaterialCreateDto, ProcMaterialEntity>();
            CreateMap<ProcMaterialModifyDto, ProcMaterialEntity>();
            CreateMap<ProcMaterialPagedQueryDto, ProcMaterialPagedQuery>();
            CreateMap<ProcMaterialEntity, ProcMaterialDto>();

            CreateMap<ProcMaterialReplaceDto, ProcReplaceMaterialEntity>();

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
            CreateMap<ProcParameterLinkTypeEntity, ProcParameterLinkTypeDto>();
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

            #region ResourceType

            CreateMap<ProcResourceTypeEntity, ProcResourceTypeDto>();
            CreateMap<ProcResourceTypeView, ProcResourceTypeViewDto>();
            CreateMap<ProcResourceTypePagedQueryDto, ProcResourceTypePagedQuery>();

            #endregion

            #region Resource

            CreateMap<ProcResourceEntity, ProcResourceDto>();
            CreateMap<ProcResourceView, ProcResourceDto>();
            CreateMap<ProcResourcePagedQueryDto, ProcResourcePagedQuery>();

            CreateMap<ProcResourceConfigPrintViewDto, ProcResourceConfigPrintView>();
            CreateMap<ProcResourceConfigPrintPagedQueryDto, ProcResourceConfigPrintPagedQuery>();

            CreateMap<ProcResourceConfigResEntity, ProcResourceConfigResDto>();
            CreateMap<ProcResourceConfigResPagedQueryDto, ProcResourceConfigResPagedQuery>();

            CreateMap<ProcResourceEquipmentBindView, ProcResourceEquipmentBindDto>();
            CreateMap<ProcResourceEquipmentBindPagedQueryDto, ProcResourceEquipmentBindPagedQuery>();

            CreateMap<ProcResourceConfigJobView, ProcResourceConfigJobDto>();
            CreateMap<ProcResourceConfigJobPagedQueryDto, ProcResourceConfigJobPagedQuery>();
            #endregion

            #region Procedure
            CreateMap<ProcProcedurePagedQueryDto, ProcProcedurePagedQuery>();
            CreateMap<ProcProcedureView, ProcProcedureViewDto>();
            CreateMap<ProcProcedureEntity, ProcProcedureDto>();
            CreateMap<ProcProcedureOperDto, ProcProcedureEntity>();
            CreateMap<ProcProcedureJobReleationEntity, ProcProcedureJobReleationDto>();
            CreateMap<ProcProcedurePrintReleationEntity, ProcProcedurePrintReleationDto>();
            #endregion
        }

        /// <summary>
        /// 排序，决定了加载的顺序
        /// </summary>
        public int Order => 0;

    }
}
