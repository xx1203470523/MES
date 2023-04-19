using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Services.Job.Manufacture;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Hymson.MES.Services.Services.Job.Common
{
    /// <summary>
    /// 生产通用
    /// </summary>
    public class JobCommonService : IJobCommonService
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
        /// 注入反射获取依赖对象
        /// </summary>
        private readonly IServiceProvider _serviceProvider;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="serviceProvider"></param>
        public JobCommonService(ICurrentUser currentUser, ICurrentSite currentSite,
            IServiceProvider serviceProvider)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// 读取挂载的作业并执行
        /// </summary>
        /// <param name="jobs"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseDto>> ExecuteJobAsync(IEnumerable<InteJobEntity> jobs, Dictionary<string, string>? param)
        {
            var result = new Dictionary<string, JobResponseDto>(); // 返回结果

            // 获取所有实现类
            var services = _serviceProvider.GetServices<IJobManufactureService>();
            foreach (var job in jobs)
            {
                var service = services.FirstOrDefault(f => f.GetType().Name == job.ClassProgram);
                if (service == null) continue;

                // TODO 如果job有额外参数，可以在这里进行拼装
                //param.Add(extra);

                // 验证参数
                await service.VerifyParamAsync(param);

                // 执行job
                result.Add(job.ClassProgram, await service.ExecuteAsync(param));
            }

            return result;
        }

    }
}
