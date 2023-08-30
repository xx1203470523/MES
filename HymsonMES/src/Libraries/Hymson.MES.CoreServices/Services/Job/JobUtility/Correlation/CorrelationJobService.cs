using Hymson.MES.CoreServices.Bos.Job;

namespace Hymson.MES.CoreServices.Services.Job.JobUtility.Correlation
{
    /// <summary>
    /// 
    /// </summary>
    internal class CorrelationJobService : ICorrelationJobService
    {
        /// <summary>
        /// 
        /// </summary>
        public CorrelationJobService() { }

        /// <summary>
        /// 获取job数据
        /// </summary>
        /// <returns></returns>
        public async Task<JobBo> GetJob()
        {
            return await Task.FromResult(new JobBo { });
        }
    }
}
