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
        /// <param name="manuFacePlateButtonJobRelationRepository"></param>
        /// <param name="inteJobRepository"></param>
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

            // 获取实现了 IManufactureJobService 接口的所有类的 Type 对象
            Type[] types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IManufactureJobService))).ToArray();

            // 遍历实现类，执行有绑定在当前按钮下面的job
            var serviceScope = _serviceProvider.CreateAsyncScope();
            if (serviceScope.ServiceProvider == null) return result;

            foreach (var job in jobs)
            {
                var type = types.FirstOrDefault(a => a.Name == job.ClassProgram);
                if (type == null) continue;

                // 通过依赖注入的方式创建该类的实例，并调用 执行 方法
                var obj = serviceScope.ServiceProvider.GetService(type);
                if (obj == null) continue;

                var service = (IManufactureJobService)obj;
                if (service == null) continue;

                /*
                // 创建该类的实例，并调用 执行 方法
                var obj = (IManufactureJobService)Activator.CreateInstance(type);
                if (obj == null) continue;
                */

                // TODO 如果job有额外参数，可以在这里进行拼装
                //extra.Add();

                result.Add(type.Name, await service.ExecuteAsync(param));
            }

            return result;
        }

    }
}
