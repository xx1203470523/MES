using Hymson.Localization.Services;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Context;
using Hymson.Utils.Tools;
using Microsoft.Extensions.DependencyInjection;

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
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="localizationService"></param>
        public ExecuteJobService(IServiceProvider serviceProvider, ILocalizationService localizationService)
        {
            _serviceProvider = serviceProvider;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 执行作业
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseBo>> ExecuteAsync(IEnumerable<JobBo> jobBos, T param)
        {
            var services = _serviceProvider.GetServices<IJobService>();

            using var scope = _serviceProvider.CreateScope();
            param.LocalizationService = _localizationService;
            param.Proxy = scope.ServiceProvider.GetRequiredService<IJobContextProxy>();

            var execJobBos = new List<JobBo>();

            // 寻找关联点
            foreach (var job in jobBos)
            {
                var service = services.FirstOrDefault(x => x.GetType().Name == job.Name);
                if (service == null) continue;

                var beforeJobs = await service.BeforeExecuteAsync(param);
                if (beforeJobs != null && beforeJobs.Any())
                {
                    execJobBos.AddRange(beforeJobs);
                }

                execJobBos.Add(job);

                var afterJobs = await service.AfterExecuteAsync(param);
                if (afterJobs != null && afterJobs.Any())
                {
                    execJobBos.AddRange(afterJobs);
                }
            }

            foreach (var job in execJobBos)
            {
                var service = services.FirstOrDefault(x => x.GetType().Name == job.Name);
                if (service == null) continue;

                // 执行参数校验
                await service.VerifyParamAsync(param);

                // 执行数据组装
                await param.Proxy.SetDataBaseValueAsync(service.DataAssemblingAsync<T>, param);
            }

            // 执行入库
            var responseDtos = new Dictionary<string, JobResponseBo>();
            using var trans = TransactionHelper.GetTransactionScope();

            foreach (var jobName in execJobBos.Select(job => job.Name))
            {
                var service = services.FirstOrDefault(x => x.GetType().Name == jobName);
                if (service == null) continue;

                var obj = await param.Proxy.GetValueAsync(service.DataAssemblingAsync<T>, param);
                if (obj == null) continue;

                var responseDto = await service.ExecuteAsync(obj);
                responseDtos.Add(jobName, responseDto);

                if (responseDto.Rows < 0) break;
            }

            if (responseDtos.Any(a => a.Value.Rows < 0))
            {
                trans.Dispose();
            }
            else
            {
                trans.Complete();
            }

            return responseDtos;
        }
    }
}