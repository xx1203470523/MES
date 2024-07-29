
using Hymson.MES.CoreServices.Events.Equipment;
using Hymson.MES.CoreServices.Events.Quality;

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


        /// <summary>
        /// 生成点检任务
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task GenerateEquSpotcheckTaskAsync(EquSpotcheckAutoCreateIntegrationEvent param);

        /// <summary>
        /// 停止点检任务
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns> 
        Task StopEquSpotcheckTaskAsync(EquSpotcheckAutoStopIntegrationEvent param);
    }
}
