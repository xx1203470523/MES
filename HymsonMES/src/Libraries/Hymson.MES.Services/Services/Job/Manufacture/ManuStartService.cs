using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuInStation;
using Hymson.Utils;

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
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<JobResponseDto> ExecuteAsync(Dictionary<string, string>? param)
        {
            var defaultDto = new JobResponseDto { };
            if (param == null) return defaultDto;

            if (param.ContainsKey("SFC") == false || param.ContainsKey("ProcedureId") == false || param.ContainsKey("ResourceId") == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16312));
            }

            var rows = await _manuInStationService.InStationAsync(new ManufactureBo
            {
                SFC = param["SFC"],
                ProcedureId = param["ProcedureId"].ParseToLong(),
                ResourceId = param["ResourceId"].ParseToLong()
            });

            var result = (rows > 0).ToString();
            defaultDto.Content?.Add("PackageCom", result);
            defaultDto.Content?.Add("BadEntryCom", result);

            defaultDto.Message = $"条码{param["SFC"]}已于NF排队！";
            return defaultDto;
        }

    }
}
