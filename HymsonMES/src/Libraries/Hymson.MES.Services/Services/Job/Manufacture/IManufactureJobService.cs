using Hymson.MES.Services.Dtos.Common;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public interface IManufactureJobService
    {
        /// <summary>
        /// 验证参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task VerifyParamAsync(Dictionary<string, string>? param);

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<JobResponseDto> ExecuteAsync(Dictionary<string, string>? param);
    }
}
