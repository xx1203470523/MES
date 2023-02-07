using AutoMapper;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
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
            CreateMap<EquipmentUnitDto, EquipmentUnitEntity>();
            CreateMap<EquipmentUnitEntity, EquipmentUnitDto>();
            CreateMap<EquipmentUnitPagedQueryDto, EquipmentUnitPagedQuery>();
            #endregion
        }

        /// <summary>
        /// 排序，决定了加载的顺序
        /// </summary>
        public int Order => 0;
    }
}
