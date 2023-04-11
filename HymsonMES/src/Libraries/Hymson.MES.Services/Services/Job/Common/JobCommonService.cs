using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
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
        /// <param name="classNames"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, int>> ExecuteJobAsync(IEnumerable<string> classNames, JobDto dto)
        {
            var result = new Dictionary<string, int>(); // 返回结果

            // 获取实现了 IManufactureJobService 接口的所有类的 Type 对象
            Type[] types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IManufactureJobService))).ToArray();

            // 遍历实现类，执行有绑定在当前按钮下面的job
            var serviceScope = _serviceProvider.CreateScope();
            foreach (Type type in types)
            {
                if (classNames.Any(a => a == type.Name) == false) continue;

                // 通过依赖注入的方式创建该类的实例，并调用 执行 方法
                var obj = (IManufactureJobService)serviceScope.ServiceProvider.GetService(type);
                if (obj == null) continue;

                /*
                // 创建该类的实例，并调用 执行 方法
                var obj = (IManufactureJobService)Activator.CreateInstance(type);
                if (obj == null) continue;
                */

                result.Add(type.Name, await obj.ExecuteAsync(dto));
            }

            return result;
        }

    }
}
