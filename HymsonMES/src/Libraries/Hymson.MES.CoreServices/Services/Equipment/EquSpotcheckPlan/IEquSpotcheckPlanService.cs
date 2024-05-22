
namespace Hymson.MES.CoreServices.Services.EquSpotcheckPlan
{
    /// <summary>
    /// 设备点检计划 service接口
    /// </summary>
    public interface IEquSpotcheckPlanCoreService 
    {
        /// <summary>
        /// 生成点检任务
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task GenerateEquSpotcheckTaskAsync(GenerateEquSpotcheckTaskDto param);

    }
}
