using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.EquipmentServices.Services.Job.Implementing;
using Microsoft.Extensions.DependencyInjection;

namespace Hymson.MES.EquipmentServices.Services.Job.Common
{
    /// <summary>
    /// 生产通用
    /// </summary>
    public class JobCommonService : IJobCommonService
    {
        /// <summary>
        /// 注入反射获取依赖对象
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider"></param>
        public JobCommonService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 读取挂载的作业并执行
        /// </summary>
        /// <param name="jobs"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task ExecuteJobAsync(IEnumerable<InteJobEntity> jobs, Dictionary<string, string>? param)
        {
            // 获取所有实现类
            var services = _serviceProvider.GetServices<IJobImplementingService>();
            foreach (var job in jobs)
            {
                var service = services.FirstOrDefault(f => f.GetType().Name == job.ClassProgram);
                if (service == null) continue;

                // TODO 如果job有额外参数，可以在这里进行拼装
                //param.Add(job.extra);

                // 验证参数
                await service.VerifyParamAsync(param);

                // 执行job
                await service.ExecuteAsync(param);
            }
        }

    }
}
