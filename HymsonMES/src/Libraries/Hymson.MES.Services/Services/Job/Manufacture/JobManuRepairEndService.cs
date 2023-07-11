using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Services.Dtos.Common;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 结束（维修）
    /// </summary>
    public class JobManuRepairEndService : IJobManufactureService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        public JobManuRepairEndService(ICurrentUser currentUser, ICurrentSite currentSite,ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 验证参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync(Dictionary<string, string>? param)
        {
            if (param == null ||
                param.ContainsKey("SFC") == false
                || param.ContainsKey("ProcedureId") == false
                || param.ContainsKey("ResourceId") == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16312));
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 执行（维修）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<JobResponseDto> ExecuteAsync(Dictionary<string, string>? param)
        {
            var defaultDto = new JobResponseDto { };

            defaultDto.Content?.Add("PackageCom", "True");
            defaultDto.Content?.Add("BadEntryCom", "True");
            if (param.ContainsKey("IsClear")) defaultDto.Content?.Add("IsClear", param["IsClear"]);

            //defaultDto.Message = $"条码{param?["SFC"]}已于NF排队！";
            defaultDto.Message = _localizationService.GetResource(nameof(ErrorCode.MES16348), param["SFC"]); 

            return await Task.FromResult(defaultDto);
        }

    }
}
