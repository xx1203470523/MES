using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.CoreServices.Bos;
using Hymson.MES.CoreServices.Dtos.Common;

namespace Hymson.MES.CoreServices.Services.Common
{
    /// <summary>
    /// 生产共用
    /// </summary>
    public interface IJobCommonService
    {
        /// <summary>
        /// 读取挂载的作业并执行
        /// </summary>
        /// <param name="jobs"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseDto>> ExecuteJobAsync(IEnumerable<InteJobEntity> jobs, Dictionary<string, string>? param);

        /// <summary>
        /// 查询类
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<JobClassBo>> GetJobClassBoListAsync();

        /// <summary>
        /// 查询类
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SelectOptionDto>> GetClassProgramOptionsAsync();

    }
}
