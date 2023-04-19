using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Services.Bos.Manufacture;
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
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<JobResponseDto> ExecuteAsync(Dictionary<string, string>? param)
        {
            var defaultDto = new JobResponseDto { };
            if (param == null) return defaultDto;

            var rows = 0;
            if (param.ContainsKey("SFC") == false || param.ContainsKey("ProcedureId") == false || param.ContainsKey("ResourceId") == false)
            {
                defaultDto.Message = "失败";
            }
            else
            {
                rows = await _manuOutStationService.OutStationAsync(new ManufactureBo
                {
                    SFC = param["SFC"],
                    ProcedureId = param["ProcedureId"].ParseToLong(),
                    ResourceId = param["ResourceId"].ParseToLong()
                });

                defaultDto.Message = "成功";
            }

            var result = (rows > 0).ToString();
            defaultDto.Content?.Add("PackageCom", result);
            defaultDto.Content?.Add("BadEntryCom", result);
            defaultDto.Content?.Add("Qty", "1");
            defaultDto.Content?.Add("Result", result);

            return defaultDto;
        }

    }
}
