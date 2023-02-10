using AutoMapper;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;
using Hymson.MES.Services.Dtos.Equipment;

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

        /// <summary>
        /// 排序，决定了加载的顺序
        /// </summary>
        public int Order => 0;
    }
}
