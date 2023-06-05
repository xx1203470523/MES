
namespace Hymson.MES.EquipmentServices.Services.Job.Implementing
{
    /// <summary>
    /// 生产作业接口
    /// </summary>
    public interface IJobImplementingService  
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
        Task ExecuteAsync(Dictionary<string, string>? param);
    }
}
