using Hymson.MES.CoreServices.Events.ManufactureEvents.ManuSfcStepEvents;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuSfcStep
{
    /// <summary>
    /// 步骤表订阅事件
    /// </summary>
    public  interface IManuSfcStepServices
    {
        /// <summary>
        /// 新增步骤表
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        Task AddAsync(ManuSfcStepEvent @event);

        /// <summary>
        /// 批量新增步骤表
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        Task AddRangeAsync(ManuSfcStepsEvent @event);
    }
}
