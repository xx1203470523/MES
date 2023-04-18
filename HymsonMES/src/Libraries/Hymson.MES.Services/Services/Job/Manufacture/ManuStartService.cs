using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuInStation;
using Newtonsoft.Json;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 开始
    /// </summary>
    public class ManuStartService : IManufactureJobService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 服务接口（进站）
        /// </summary>
        private readonly IManuInStationService _manuInStationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuInStationService"></param>
        public ManuStartService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuInStationService manuInStationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuInStationService = manuInStationService;
        }


        /// <summary>
        /// 执行（开始）
        /// </summary>
        /// <param name="extra"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(string? extra)
        {
            if (string.IsNullOrEmpty(extra) == true) return 0;

            var dto = JsonConvert.DeserializeObject<ManufactureBo>(extra);
            if (dto == null) return 0;

            return await _manuInStationService.InStationAsync(dto);
        }

    }
}
