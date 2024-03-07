using AutoMapper;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.EquEquipmentHeartRecord;
using Hymson.MES.Core.Domain.EquEquipmentLoginRecord;
using Hymson.MES.Core.Domain.ManuEquipmentStatusTime;
using Hymson.MES.Core.Domain.ManuEuqipmentNewestInfoEntity;
using Hymson.MES.Services.Dtos.EquEquipmentHeartRecord;
using Hymson.MES.Services.Dtos.EquEquipmentLoginRecord;
using Hymson.MES.Services.Dtos.ManuEquipmentStatusTime;
using Hymson.MES.Services.Dtos.ManuEuqipmentNewestInfo;

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
            CreateMap<ManuEuqipmentNewestInfoSaveDto, ManuEuqipmentNewestInfoEntity>();
            CreateMap<EquEquipmentHeartRecordSaveDto, EquEquipmentHeartRecordEntity>();
            CreateMap<ManuEquipmentStatusTimeSaveDto, ManuEquipmentStatusTimeEntity>();
            #endregion

        }


        /// <summary>
        /// 排序，决定了加载的顺序
        /// </summary>
        public int Order => 0;

    }
}
