using AutoMapper;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;
using Hymson.MES.Data.Repositories.Equipment.EquSparePartType.Query;
using Hymson.MES.Data.Repositories.Integrated.InteCalendar.Query;
using Hymson.MES.Data.Repositories.Integrated.InteClass.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Process.ResourceType;
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
            //CreateMap<EquipmentUnitEntity, EquipmentUnitCreateDto>();
            CreateMap<EquEquipmentPagedQueryDto, EquEquipmentPagedQuery>();
            #endregion

            #region EquEquipmentGroup
            CreateMap<EquEquipmentGroupCreateDto, EquEquipmentGroupEntity>();
            CreateMap<EquEquipmentGroupModifyDto, EquEquipmentGroupEntity>();
            CreateMap<EquEquipmentGroupPagedQueryDto, EquEquipmentGroupPagedQuery>();
            #endregion

            #region EquEquipmentLinkApi
            CreateMap<EquEquipmentLinkApiCreateDto, EquEquipmentLinkApiEntity>();
            CreateMap<EquEquipmentLinkApiModifyDto, EquEquipmentLinkApiEntity>();
            CreateMap<EquEquipmentLinkApiPagedQueryDto, EquEquipmentLinkApiPagedQuery>();
            #endregion

            #region EquEquipmentLinkHardware
            CreateMap<EquEquipmentLinkHardwareCreateDto, EquEquipmentLinkHardwareEntity>();
            CreateMap<EquEquipmentLinkHardwareModifyDto, EquEquipmentLinkHardwareEntity>();
            CreateMap<EquEquipmentLinkHardwarePagedQueryDto, EquEquipmentLinkHardwarePagedQuery>();
            #endregion

            #region EquEquipmentUnit
            CreateMap<EquEquipmentUnitCreateDto, EquEquipmentUnitEntity>();
            CreateMap<EquEquipmentUnitModifyDto, EquEquipmentUnitEntity>();
            CreateMap<EquEquipmentUnitPagedQueryDto, EquEquipmentUnitPagedQuery>();
            #endregion

            #region EquSparePartType
            CreateMap<EquSparePartTypeCreateDto, EquSparePartTypeEntity>();
            CreateMap<EquSparePartTypeModifyDto, EquSparePartTypeEntity>();
            CreateMap<EquSparePartTypePagedQueryDto, EquSparePartTypePagedQuery>();
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
            #endregion

            #region InteClassDetail
            CreateMap<InteClassDetailCreateDto, InteClassDetailEntity>();
            CreateMap<InteClassDetailModifyDto, InteClassDetailEntity>();
            #endregion

            #region InteClass
            CreateMap<InteClassCreateDto, InteClassEntity>();
            CreateMap<InteClassModifyDto, InteClassEntity>();
            CreateMap<InteClassPagedQueryDto, InteClassPagedQuery>();
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

            #region ResourceType

            CreateMap<ProcResourceTypeEntity, ProcResourceTypeDto>();
            CreateMap<ProcResourceTypeView, ProcResourceTypeDto>();
            CreateMap<ProcResourceTypePagedQueryDto, ProcResourceTypePagedQuery>();

            #endregion

            #region ProcResource

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
        }

        /// <summary>
        /// 排序，决定了加载的顺序
        /// </summary>
        public int Order => 0;

    }
}
