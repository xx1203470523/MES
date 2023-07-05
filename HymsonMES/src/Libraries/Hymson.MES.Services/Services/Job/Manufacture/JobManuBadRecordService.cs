﻿using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Execute;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 不良录入
    /// </summary>
    public class JobManuBadRecordService : IJobManufactureService
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
        public JobManuBadRecordService(ICurrentUser currentUser, ICurrentSite currentSite,
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
        /// 执行（不良录入）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<JobResponseDto> ExecuteAsync(Dictionary<string, string>? param)
        {
            var defaultDto = new JobResponseDto { };
            defaultDto.Content?.Add("PackageCom", "False");

            var bo = new ManufactureBo
            {
                SFC = param["SFC"],
                ProcedureId = param["ProcedureId"].ParseToLong(),
                ResourceId = param["ResourceId"].ParseToLong()
            };

            var jobBos = new List<JobBo> { };
            jobBos.Add(new JobBo { Name = "BadRecordJobService" });

            var result = await _executeJobService.ExecuteAsync(jobBos, new JobRequestBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                UserName = _currentUser.UserName,
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
                SFCs = new string[] { bo.SFC }
            });

            // 判断面板是否显示
            var isShow = false;
            var currentResponse = result.FirstOrDefault(f => f.Key == "BadRecordJobService").Value;
            if (currentResponse != null)
            {
                isShow = currentResponse.Content["IsShow"].ParseToBool();
            }

            defaultDto.Content?.Add("BadEntryCom", $"{isShow}".ToString());
            if (param.ContainsKey("IsClear")) defaultDto.Content?.Add("IsClear", param["IsClear"]);
            //defaultDto.Message = $"条码{bo.SFC}" + (isShow ? "开始录入" : "已经完成录入，无需重复录入！");
            defaultDto.Message = _localizationService.GetResource(isShow?nameof(ErrorCode.MES16342) : nameof(ErrorCode.MES16343), bo.SFC);

            return defaultDto;
        }


    }
}
