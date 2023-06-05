using Hymson.MES.Core.Domain.Integrated;

namespace Hymson.MES.EquipmentServices.Services.Job.Common
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
        Task ExecuteJobAsync(IEnumerable<InteJobEntity> jobs, Dictionary<string, string>? param);

    }
}
