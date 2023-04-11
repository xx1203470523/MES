using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
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
        /// 仓储接口（面板按钮作业关系）
        /// </summary>
        private readonly IManuFacePlateButtonJobRelationRepository _manuFacePlateButtonJobRelationRepository;

        /// <summary>
        /// 仓储接口（作业）
        /// </summary>
        private readonly IInteJobRepository _inteJobRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="manuFacePlateButtonJobRelationRepository"></param>
        /// <param name="inteJobRepository"></param>
        public JobCommonService(ICurrentUser currentUser, ICurrentSite currentSite,
            IServiceProvider serviceProvider,
            IManuFacePlateButtonJobRelationRepository manuFacePlateButtonJobRelationRepository,
            IInteJobRepository inteJobRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _serviceProvider = serviceProvider;
            _manuFacePlateButtonJobRelationRepository = manuFacePlateButtonJobRelationRepository;
            _inteJobRepository = inteJobRepository;
        }


        /// <summary>
        /// 读取挂载的作业并执行
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ExecuteJobAsync(JobDto dto)
        {
            // 根据面板ID和按钮ID找出绑定的作业job
            var buttonJobs = await _manuFacePlateButtonJobRelationRepository.GetByFacePlateButtonIdAsync(dto.FacePlateButtonId);
            if (buttonJobs.Any() == false) return;

            // 根据 buttonJobs 读取对应的job对象
            var jobs = await _inteJobRepository.GetByIdsAsync(buttonJobs.Select(s => s.JobId).ToArray());

            // 获取实现了 IManufactureJobService 接口的所有类的 Type 对象
            Type[] types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IManufactureJobService))).ToArray();

            // 遍历实现类，执行有绑定在当前按钮下面的job
            var serviceScope = _serviceProvider.CreateScope();
            foreach (Type type in types)
            {
                if (jobs.Any(a => a.Code == type.Name) == false) continue;

                // 通过依赖注入的方式创建该类的实例，并调用 执行 方法
                var obj = (IManufactureJobService)serviceScope.ServiceProvider.GetService(type);
                if (obj == null) continue;

                /*
                // 创建该类的实例，并调用 执行 方法
                var obj = (IManufactureJobService)Activator.CreateInstance(type);
                if (obj == null) continue;
                */

                await obj.ExecuteAsync(dto);
            }
        }

    }
}
