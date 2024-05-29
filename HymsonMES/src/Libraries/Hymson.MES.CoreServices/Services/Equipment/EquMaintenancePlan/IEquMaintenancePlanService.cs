
using Hymson.MES.CoreServices.Events.Equipment;
using Hymson.MES.CoreServices.Events.Quality;

namespace Hymson.MES.CoreServices.Services.EquMaintenancePlan
{
    /// <summary>
    /// 设备保养计划 service接口
    /// </summary>
    public interface IEquMaintenancePlanCoreService
    {
        /// <summary>
        /// 生成保养任务
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task GenerateEquMaintenanceTaskAsync(GenerateEquMaintenanceTaskDto param);

        /// <summary>
        /// 生成保养任务
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task GenerateEquMaintenanceTaskAsync(EquMaintenanceAutoCreateIntegrationEvent param);


        /// <summary>
        /// 停止保养任务
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>  
        Task StopEquMaintenanceTaskAsync(EquMaintenanceAutoStopIntegrationEvent param);
    }
}
