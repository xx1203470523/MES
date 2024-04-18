using AutoMapper;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.AgvTaskRecord;
using Hymson.MES.Core.Domain.CcdFileUploadCompleteRecord;
using Hymson.MES.Core.Domain.EquEquipmentAlarm;
using Hymson.MES.Core.Domain.EquEquipmentHeartRecord;
using Hymson.MES.Core.Domain.EquEquipmentLoginRecord;
using Hymson.MES.Core.Domain.EquProcessParamRecord;
using Hymson.MES.Core.Domain.EquProductParamRecord;
using Hymson.MES.Core.Domain.EquToolLifeRecord;
using Hymson.MES.Core.Domain.ManuEquipmentStatusTime;
using Hymson.MES.Core.Domain.ManuEuqipmentNewestInfoEntity;
using Hymson.MES.Core.Domain.ManuFeedingCompletedZjyjRecord;
using Hymson.MES.Core.Domain.ManuFeedingNoProductionRecord;
using Hymson.MES.Core.Domain.ManuFeedingTransferRecord;
using Hymson.MES.Core.Domain.ManuFillingDataRecord;
using Hymson.MES.Core.Domain.ManuJzBind;
using Hymson.MES.Core.Domain.ManuJzBindRecord;
using Hymson.MES.Services.Dtos.AgvTaskRecord;
using Hymson.MES.Services.Dtos.CcdFileUploadCompleteRecord;
using Hymson.MES.Services.Dtos.EquEquipmentAlarm;
using Hymson.MES.Services.Dtos.EquEquipmentHeartRecord;
using Hymson.MES.Services.Dtos.EquEquipmentLoginRecord;
using Hymson.MES.Services.Dtos.EquProcessParamRecord;
using Hymson.MES.Services.Dtos.EquProductParamRecord;
using Hymson.MES.Services.Dtos.EquToolLifeRecord;
using Hymson.MES.Services.Dtos.ManuEquipmentStatusTime;
using Hymson.MES.Services.Dtos.ManuEuqipmentNewestInfo;
using Hymson.MES.Services.Dtos.ManuFeedingCompletedZjyjRecord;
using Hymson.MES.Services.Dtos.ManuFeedingNoProductionRecord;
using Hymson.MES.Services.Dtos.ManuFeedingTransferRecord;
using Hymson.MES.Services.Dtos.ManuFillingDataRecord;
using Hymson.MES.Services.Dtos.ManuJzBind;
using Hymson.MES.Services.Dtos.ManuJzBindRecord;

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
            CreateMap<EquEquipmentAlarmSaveDto, EquEquipmentAlarmEntity>();
            CreateMap<CcdFileUploadCompleteRecordSaveDto, CcdFileUploadCompleteRecordEntity>();
            CreateMap<AgvTaskRecordSaveDto, AgvTaskRecordEntity>();
            CreateMap<EquProcessParamRecordSaveDto, EquProcessParamRecordEntity>();
            CreateMap<EquProductParamRecordSaveDto, EquProductParamRecordEntity>();
            CreateMap<ManuFeedingCompletedZjyjRecordSaveDto, ManuFeedingCompletedZjyjRecordEntity>();
            CreateMap<ManuFeedingNoProductionRecordSaveDto, ManuFeedingNoProductionRecordEntity>();
            CreateMap<ManuFillingDataRecordSaveDto, ManuFillingDataRecordEntity>();
            CreateMap<EquToolLifeRecordSaveDto, EquToolLifeRecordEntity>();
            CreateMap<ManuFeedingTransferRecordSaveDto, ManuFeedingTransferRecordEntity>();
            CreateMap<ManuJzBindSaveDto, ManuJzBindEntity>();
            CreateMap<ManuJzBindRecordSaveDto, ManuJzBindRecordEntity>();
            #endregion

        }


        /// <summary>
        /// 排序，决定了加载的顺序
        /// </summary>
        public int Order => 0;

    }
}
