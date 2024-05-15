using Hymson.MES.Core.Domain.ManuEuqipmentNewestInfoEntity;
using Hymson.MES.Data.Repositories.Equipment.Qkny.ManuEuqipmentNewestInfo.View;
using Hymson.MES.Data.Repositories.ManuEuqipmentNewestInfo;
using Hymson.MES.Data.Repositories.ManuEuqipmentNewestInfo.Query;
using Hymson.MES.Services.Dtos.Board;
using Hymson.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Board.EquipmentStatus
{
    /// <summary>
    /// 获取设备最新信息
    /// </summary>
    public class EquipmentStatusService : IEquipmentStatusService
    {
        /// <summary>
        /// 设备最新信息仓储
        /// </summary>
        private readonly IManuEuqipmentNewestInfoRepository _manuEuqipmentNewestInfoRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuEuqipmentNewestInfoRepository"></param>
        public EquipmentStatusService(IManuEuqipmentNewestInfoRepository manuEuqipmentNewestInfoRepository)
        {
            _manuEuqipmentNewestInfoRepository = manuEuqipmentNewestInfoRepository;
        }

        /// <summary>
        /// 获取设备最新信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<ManuEquipmentNewestInfoView>> GetEquNewestInfoList(EquipmentNewestInfoQueryDto queryDto)
        {
            ManuEuqipmentNewestInfoSiteQuery query = new ManuEuqipmentNewestInfoSiteQuery() { SiteId = queryDto.SiteId };
            var dbList = await _manuEuqipmentNewestInfoRepository.GetListAsync(query);
            if(dbList == null)
            {
                return new List<ManuEquipmentNewestInfoView>();
            }

            DateTime now = HymsonClock.Now();
            int offlineNum = 35; 
            foreach(var item in  dbList)
            {
                int disSeconds = (now - item.HeartUpdatedOn).Seconds;
                if(disSeconds > offlineNum)
                {
                    item.Status = "4";
                }
            }

            return dbList.ToList();
        }
    }
}
