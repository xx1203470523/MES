using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 完成
    /// </summary>
    public class ManuCompleteService : IManufactureJobService
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
        /// 服务接口（出站）
        /// </summary>
        private readonly IManuOutStationService _manuOutStationService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuOutStationService"></param>
        public ManuCompleteService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuOutStationService manuOutStationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuOutStationService = manuOutStationService;
        }


        /// <summary>
        /// 执行（完成）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(JobDto dto)
        {
            return await _manuOutStationService.OutStationAsync(dto);
        }

    }
}
