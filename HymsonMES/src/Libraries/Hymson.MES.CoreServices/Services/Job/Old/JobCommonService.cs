using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.CoreServices.Bos;
using Hymson.MES.CoreServices.Dtos.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Metadata;
using System.Reflection;
using System.Text.RegularExpressions;
using Hymson.MES.Core.Attribute.Job;

namespace Hymson.MES.CoreServices.Services.Job
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
            var responseDtos = new Dictionary<string, JobResponseDto>(); // 返回结果

            // 获取所有实现类
            var services = _serviceProvider.GetServices<IJobManufactureService>();
            foreach (var item in jobs.Select(x=>x.ClassProgram))
            {
                var service = services.FirstOrDefault(f => f.GetType().Name == item);
                if (service == null) continue;

                // TODO 如果job有额外参数，可以在这里进行拼装

                // 验证参数
                await service.VerifyParamAsync(param);

                // 执行job

                var responseDto = await service.ExecuteAsync(param);
                responseDtos.Add(item, responseDto);

                if (responseDto.Rows < 0) break;
            }
            return responseDtos;
        }

        /// <summary>
        /// 查询类
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<JobClassBo>> GetJobClassBoListAsync()
        {
            // 获取所有实现类

            var services = _serviceProvider.GetServices<IJobService>();
            return await Task.FromResult(services.Select(s =>
            {
                var type = s.GetType();
                var jobAttribute = type.GetCustomAttribute<JobAttribute>();
                var classModule = Regex.Replace(type.Module.Name, ".dll", "");
                return new JobClassBo
                {
                    ClassName = type.Name,
                    ClassNamespace = type.Namespace ?? "",
                    ClassModule = classModule,
                    Remark= jobAttribute?.Name
                };
            }));
        }

        /// <summary>
        /// 查询类
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOptionDto>> GetClassProgramOptionsAsync()
        {
            var services = await GetJobClassBoListAsync();
            return services.Select(s =>
            {
                return new SelectOptionDto
                {
                    Key = $"{s.ClassName}",
                    Label = $"【{s.ClassModule}】 {s.ClassName}【{s.Remark}】",
                    Value = $"{s.ClassName}"
                };
            });
        }

    }
}
