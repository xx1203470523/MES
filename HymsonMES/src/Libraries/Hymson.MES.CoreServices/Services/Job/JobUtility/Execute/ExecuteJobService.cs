﻿using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Context;
using Microsoft.Extensions.DependencyInjection;
using System.Transactions;

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
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ExecuteJobService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 执行作业
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseBo>> ExecuteAsync(IEnumerable<JobBo> jobBos, T param)
        {
            var services = _serviceProvider.GetServices<IJobService>();

            using var scope = _serviceProvider.CreateScope();
            param.Proxy = scope.ServiceProvider.GetRequiredService<IJobContextProxy>();

            // 执行参数校验
            foreach (var job in jobBos)
            {
                var service = services.FirstOrDefault(x => x.GetType().Name == job.Name);
                if (service == null) continue;

                await service.VerifyParamAsync(param);
            }

            // 执行数据组装
            foreach (var job in jobBos)
            {
                var service = services.FirstOrDefault(x => x.GetType().Name == job.Name);
                if (service == null) continue;
                await param.Proxy.GetValueAsync(service.DataAssemblingAsync<T>, param);
            }

            // 执行入库
            var responseDtos = new Dictionary<string, JobResponseBo>();
            using var trans = new TransactionScope();
            foreach (var job in jobBos)
            {
                var service = services.FirstOrDefault(x => x.GetType().Name == job.Name);
                if (service == null) continue;

                var obj = await param.Proxy.GetValueAsync(service.DataAssemblingAsync<T>, param);
                if (obj == null) continue;

                var responseDto = await service.ExecuteAsync(obj);
                if (responseDto.Rows < 0)
                {
                    trans.Dispose();
                    break; ;
                }

                responseDtos.Add(job.Name, responseDto);
            }

            trans.Complete();
            return responseDtos;
        }
    }
}