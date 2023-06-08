using Hymson.MES.CoreServices.Dtos.Common;

namespace Hymson.MES.CoreServices.Services.Common
{
    /// <summary>
    /// 生产作业接口
    /// </summary>
    public interface IJobManufactureService
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
