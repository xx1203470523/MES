using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Job.Manufacture;
using Microsoft.Extensions.DependencyInjection;
using MySqlX.XDevAPI.Common;
using System.ComponentModel;

namespace Hymson.MES.Services.Services.Job.Common
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
                //param.Add(job.extra);

                // 验证参数
                await service.VerifyParamAsync(param);

                // 执行job
                result.Add(job.ClassProgram, await service.ExecuteAsync(param));
            }

            return result;
        }

        /// <summary>
        /// 查询类
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOptionDto>> GetClassProgramListAsync()
        {
            // 获取所有实现类
            var services = _serviceProvider.GetServices<IJobManufactureService>();
            return await Task.FromResult(services.Select(s =>
            {
                var name = s.GetType().Name;
                return new SelectOptionDto
                {
                    Key = $"{name}",
                    Label = name,
                    Value = $"{name}"
                };
            }));
        }

    }
}
