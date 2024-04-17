using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.ManuEuqipmentNewestInfoEntity;
using Hymson.MES.Services.Dtos.ManuEuqipmentNewestInfo;

namespace Hymson.MES.Services.Services.ManuEuqipmentNewestInfo
{
    /// <summary>
    /// 服务接口（设备最新信息）
    /// </summary>
    public interface IManuEuqipmentNewestInfoService
    {
        /// <summary>
        /// 添加或者更新
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> AddOrUpdateAsync(ManuEuqipmentNewestInfoSaveDto saveDto);
    }
}