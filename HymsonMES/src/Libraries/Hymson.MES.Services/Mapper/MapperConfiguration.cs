using AutoMapper;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Services.Dtos.Equipment;
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
            CreateProcMaterialMaps();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateEquipmentMaps()
        {
            #region EquipmentUnit
            CreateMap<EquEquipmentUnitCreateDto, EquEquipmentUnitEntity>();
            CreateMap<EquEquipmentUnitModifyDto, EquEquipmentUnitEntity>();
            //CreateMap<EquipmentUnitEntity, EquipmentUnitCreateDto>();
            CreateMap<EquEquipmentUnitPagedQueryDto, EquEquipmentUnitPagedQuery>();
            #endregion

            #region Equipment
            CreateMap<EquEquipmentCreateDto, EquEquipmentEntity>();
            CreateMap<EquEquipmentModifyDto, EquEquipmentEntity>();
            //CreateMap<EquipmentUnitEntity, EquipmentUnitCreateDto>();
            CreateMap<EquEquipmentPagedQueryDto, EquEquipmentPagedQuery>();
            #endregion

            #region EquEquipmentLinkApi
            CreateMap<EquEquipmentLinkApiCreateDto, EquEquipmentLinkApiEntity>();
            CreateMap<EquEquipmentLinkApiModifyDto, EquEquipmentLinkApiEntity>();
            //CreateMap<EquEquipmentLinkApiDto, EquEquipmentLinkApiEntity>();
            CreateMap<EquEquipmentLinkApiPagedQueryDto, EquEquipmentLinkApiPagedQuery>();
            #endregion

            #region EquEquipmentLinkHardware
            CreateMap<EquEquipmentLinkHardwareCreateDto, EquEquipmentLinkHardwareEntity>();
            CreateMap<EquEquipmentLinkHardwareModifyDto, EquEquipmentLinkHardwareEntity>();
            CreateMap<EquEquipmentLinkHardwarePagedQueryDto, EquEquipmentLinkHardwarePagedQuery>();
            #endregion
        }

        protected virtual void CreateProcMaterialMaps()
        {
            CreateMap<ProcMaterialCreateDto, ProcMaterialEntity>();
            CreateMap<ProcMaterialReplaceDto, ProcReplaceMaterialEntity>();

            CreateMap<ProcMaterialModifyDto, ProcMaterialEntity>();
            CreateMap<ProcMaterialPagedQueryDto, ProcMaterialPagedQuery>();
            CreateMap<ProcMaterialEntity, ProcMaterialDto>();

            CreateMap<ProcResourceTypeDto, ProcResourceTypeEntity>();
            CreateMap<ProcResourceTypeViewDto, ProcResourceTypeView>();
            CreateMap<ProcResourceTypePagedQueryDto, ProcResourceTypePagedQuery>();

            CreateMap<ProcResourceDto, ProcResourceEntity>();
            CreateMap<ProcResourceViewDto, ProcResourceView>();
            CreateMap<ProcResourcePagedQueryDto, ProcResourcePagedQuery>();
            CreateMap<ProcResourceConfigPrintViewDto, ProcResourceConfigPrintView>();
            CreateMap<ProcResourceConfigPrintPagedQueryDto, ProcResourceConfigPrintPagedQuery>();
        }

        /// <summary>
        /// 排序，决定了加载的顺序
        /// </summary>
        public int Order => 0;
    }
}
