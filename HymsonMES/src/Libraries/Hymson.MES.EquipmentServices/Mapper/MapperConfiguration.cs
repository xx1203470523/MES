using AutoMapper;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.EquEquipmentLoginRecord;
using Hymson.MES.Services.Dtos.EquEquipmentLoginRecord;

namespace Hymson.MES.EquipmentServices.Mapper
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
            #region 留个示例 使用时修改
            //CreateMap<EquConsumableSaveDto, EquSparePartEntity>();
            //CreateMap<EquConsumablePagedQueryDto, EquSparePartPagedQuery>();

            //CreateMap<EquSparePartEntity, EquConsumableDto>();
            CreateMap<EquEquipmentLoginRecordSaveDto, EquEquipmentLoginRecordEntity>();
            #endregion

        }


        /// <summary>
        /// 排序，决定了加载的顺序
        /// </summary>
        public int Order => 0;

    }
}
