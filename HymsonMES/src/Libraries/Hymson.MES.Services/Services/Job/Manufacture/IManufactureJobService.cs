using Hymson.MES.Services.Dtos.Common;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public interface IManufactureJobService
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ExecuteAsync(JobDto dto);
    }
}
