using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Execute;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 完成
    /// </summary>
    public class JobManuCompleteService : IJobManufactureService
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
        /// 服务接口
        /// </summary>
        private readonly IExecuteJobService<JobRequestBo> _executeJobService;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="executeJobService"></param>
        /// <param name="localizationService"></param>
        public JobManuCompleteService(ICurrentUser currentUser, ICurrentSite currentSite,
            IExecuteJobService<JobRequestBo> executeJobService,
            ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _executeJobService = executeJobService;
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
        /// 执行（完成）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<JobResponseDto> ExecuteAsync(Dictionary<string, string>? param)
        {
            var defaultDto = new JobResponseDto { };

            var bo = new ManufactureBo
            {
                SFC = param["SFC"],
                ProcedureId = param["ProcedureId"].ParseToLong(),
                ResourceId = param["ResourceId"].ParseToLong()
            };

            var jobBos = new List<JobBo> { };
            jobBos.Add(new JobBo { Name = "OutStationVerifyJobService" });
            jobBos.Add(new JobBo { Name = "OutStationJobService" });

            var responseBo = await _executeJobService.ExecuteAsync(jobBos, new JobRequestBo
            {
                SiteId = _currentSite.SiteId ?? 123456,
                UserName = _currentUser.UserName,
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
                SFCs = new string[] { bo.SFC }
            });

            defaultDto.Content?.Add("PackageCom", "False");
            defaultDto.Content?.Add("BadEntryCom", "False");
            defaultDto.Content?.Add("Qty", "1");
            if (param.ContainsKey("IsClear")) defaultDto.Content?.Add("IsClear", param["IsClear"]);

            // 判断是否尾工序
            var isLastProcedure = false;
            var nextProcedureCode = "";
            foreach (var item in responseBo)
            {
                var content = item.Value.Content;
                if (item.Key == "OutStationJobService" && content != null && content.Any())
                {
                    isLastProcedure = content["IsLastProcedure"].ParseToBool();
                    nextProcedureCode = content["NextProcedureCode"];
                }

                defaultDto.Rows = item.Value.Rows;
            }

            //defaultDto.Message = $"条码{param["SFC"]}完成，已于NF排队！";
            if (isLastProcedure)
            {
                defaultDto.Message = _localizationService.GetResource(nameof(ErrorCode.MES16349), param["SFC"]);
            }
            else
            {
                defaultDto.Message = _localizationService.GetResource(nameof(ErrorCode.MES16351), param["SFC"], nextProcedureCode);
            }

            return defaultDto;
        }

    }
}
