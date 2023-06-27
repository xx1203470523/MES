using Hymson.MES.CoreServices.Bos.Job;
using Microsoft.Extensions.DependencyInjection;
using System.Transactions;

namespace Hymson.MES.CoreServices.Services.Job.JobUtility.Execute
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExecuteJobService<T> where T : JobBaseBo
    {
        /// <summary>
        /// 注入反射获取依赖对象
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        private readonly JobContextProxy _jobContextProxy;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="jobContextProxy"></param>
        public ExecuteJobService(IServiceProvider serviceProvider, JobContextProxy jobContextProxy)
        {
            _serviceProvider = serviceProvider;
            _jobContextProxy = jobContextProxy;
        }

        /// <summary>
        /// 执行作业
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync(IEnumerable<JobBo> jobBos, T param)
        {
            var services = _serviceProvider.GetServices<IJobService>();

            /*
            var inStationService = services.FirstOrDefault(w => w.GetType().Name == "InStationJobService");
            await inStationService.VerifyParamAsync(new InStationRequestBo
            {
                SiteId = 11111
            });
            */

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

                //await _jobContextProxy.GetValueAsync(service.DataAssemblingAsync, param);
            }

            // 执行入库
            using var trans = new TransactionScope();
            foreach (var job in jobBos)
            {
                var service = services.FirstOrDefault(x => x.GetType().Name == job.Name);
                if (service == null) continue;

                await service.ExecuteAsync();
            }

            trans.Complete();
        }

        //  获取作业
    }
}