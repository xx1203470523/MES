using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Context;
using Hymson.Utils.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.CoreServices.Services.Job.JobUtility.Execute
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExecuteJobService<T> : IExecuteJobService<T> where T : JobBaseBo
    {
        /// <summary>
        /// 注入反射获取依赖对象
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<T> _logger;

        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime curDate;

        /// <summary>
        /// id标识
        /// </summary>
        public long TimeId;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="localizationService"></param>
        public ExecuteJobService(IServiceProvider serviceProvider, ILocalizationService localizationService,
            ILogger<T> logger)
        {
            _serviceProvider = serviceProvider;
            _localizationService = localizationService;
            _logger = logger;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type">1-开始 2-结束</param>
        private void WriteLog(string message)
        {
            //最小毫秒数，低于这个则不记录
            int minMilliSeconds = 50;

            DateTime now = DateTime.Now;
            int total = (int)((now - curDate).TotalMilliseconds);
            _logger.LogWarning($"进站{TimeId}:时间:{total}:{message}");
            curDate = DateTime.Now;
        }

        /// <summary>
        /// 执行作业
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseBo>> ExecuteAsync(IEnumerable<JobBo> jobBos, T param)
        {
            WriteLog($"ExecuteAsync-开始");
            WriteLog($"获取服务-开始");

            var services = _serviceProvider.GetServices<IJobService>();

            using var scope = _serviceProvider.CreateScope();
            param.LocalizationService = _localizationService;
            param.Proxy = scope.ServiceProvider.GetRequiredService<IJobContextProxy>();

            WriteLog($"获取服务-结束");

            var execJobBos = new List<JobBo>();

            int index = 0;
            // 寻找关联点
            foreach (var job in jobBos)
            {
                var service = services.FirstOrDefault(x => x.GetType().Name == job.Name);
                if (service == null) continue;

                WriteLog($"BeforeExecuteAsync:{index},name:{job.Name}-开始");

                var beforeJobs = await service.BeforeExecuteAsync(param);
                if (beforeJobs != null && beforeJobs.Any())
                {
                    execJobBos.AddRange(beforeJobs);
                }

                WriteLog($"BeforeExecuteAsync:{index},name:{job.Name}-结束");

                execJobBos.Add(job);

                WriteLog($"AfterExecuteAsync:{index},name:{job.Name}-开始");

                var afterJobs = await service.AfterExecuteAsync(param);
                if (afterJobs != null && afterJobs.Any())
                {
                    execJobBos.AddRange(afterJobs);
                }

                WriteLog($"AfterExecuteAsync:{index},name:{job.Name}-结束");
                ++index;
            }
            index = 0;

            WriteLog($"检查重复的作业-开始");

            // 检查重复的作业
            var duplicateNames = execJobBos.GroupBy(g => g.Name)
                .Where(w => w.Count() > 1)
                .Select(s => s.Key);
            if (duplicateNames != null && duplicateNames.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18230))
                    .WithData("Job", string.Join(",", duplicateNames));
            }

            WriteLog($"检查重复的作业-结束");

            foreach (var job in execJobBos)
            {
                var service = services.FirstOrDefault(x => x.GetType().Name == job.Name);
                if (service == null) continue;

                WriteLog($"VerifyParamAsync:{index},name:{job.Name}-开始");

                // 执行参数校验
                await service.VerifyParamAsync(param);

                WriteLog($"VerifyParamAsync:{index},name:{job.Name}-结束");

                WriteLog($"SetDataBaseValueAsync:{index},name:{job.Name}-开始");

                // 执行数据组装
                await param.Proxy.SetDataBaseValueAsync(service.DataAssemblingAsync<T>, param);

                WriteLog($"SetDataBaseValueAsync:{index},name:{job.Name}-结束");

                ++index;
            }
            index = 0;

            // 执行入库
            var responseDtos = new Dictionary<string, JobResponseBo>();
            using var trans = TransactionHelper.GetTransactionScope();

            foreach (var jobName in execJobBos.Select(job => job.Name))
            {
                var service = services.FirstOrDefault(x => x.GetType().Name == jobName);
                if (service == null) continue;

                WriteLog($"DataAssemblingAsync:{index},name:{jobName}-开始");

                var obj = await param.Proxy.GetValueAsync(service.DataAssemblingAsync<T>, param);

                WriteLog($"DataAssemblingAsync:{index},name:{jobName}-结束");

                if (obj == null) continue;

                WriteLog($"ExecuteAsync:{index},name:{jobName}-开始");

                var responseDto = await service.ExecuteAsync(obj);

                WriteLog($"ExecuteAsync:{index},name:{jobName}-结束");

                if (responseDto == null) continue;

                responseDtos.Add(jobName, responseDto);
                if (!responseDto.IsSuccess) break;
            }

            if (responseDtos.Any(a => !a.Value.IsSuccess))
            {
                trans.Dispose();
            }
            else
            {
                trans.Complete();
            }

            WriteLog($"ExecuteAsync-结束");

            return responseDtos;
        }
    }
}