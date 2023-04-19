using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 不良录入
    /// </summary>
    public class ManuBadRecordService : IManufactureJobService
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
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuSfcProduceRepository"></param>
        public ManuBadRecordService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonService manuCommonService,
            IManuSfcProduceRepository manuSfcProduceRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
        }


        /// <summary>
        /// 执行（不良录入）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<JobResponseDto> ExecuteAsync(Dictionary<string, string>? param)
        {
            var defaultDto = new JobResponseDto { };
            if (param == null) return defaultDto;

            defaultDto.Content?.Add("PackageCom", "True");
            defaultDto.Content?.Add("BadEntryCom", "True");
            defaultDto.Content?.Add("Result", "True");
            defaultDto.Message = "成功";

            // TODO
            return await Task.FromResult(defaultDto);
        }



    }
}
