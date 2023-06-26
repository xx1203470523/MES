using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Dtos.Job;
using Microsoft.Extensions.DependencyInjection;
using System.Transactions;

namespace Hymson.MES.CoreServices.Services.NewJob.Execute
{
    public class ExecuteJobService<T> where T : JobBaseBo
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
        public async Task ExecuteAsync(IEnumerable<JobBo> jobBos, T param)
        {
            var services = _serviceProvider.GetServices<IJobService<T>>();
            //执行参数校验

            foreach (var job in jobBos)
            {
                var service = services.FirstOrDefault(x => x.GetType().Name == job.Name);
                if (service != null)
                {
                    await service.VerifyParam(param);
                }
            }
            //执行数据组装

            foreach (var job in jobBos)
            {
                var service = services.FirstOrDefault(x => x.GetType().Name == job.Name);
                if (service != null)
                {
                    await service.DataAssembling(param);
                }
            }

            //执行入库
            using (TransactionScope ts = new TransactionScope())
            {
                foreach (var job in jobBos)
                {
                    var service = services.FirstOrDefault(x => x.GetType().Name == job.Name);
                    if (service != null)
                    {
                        await service.ExecuteAsync();
                    }
                }
            }
        }

        //  获取作业
    }
}