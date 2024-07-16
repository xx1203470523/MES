using Hymson.MES.CoreServices.Events.ManufactureEvents.ManuSfcStepEvents;
using Hymson.MES.Data.Repositories.Manufacture;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuSfcStep
{
    /// <summary>
    /// 条码步骤表订阅服务类
    /// </summary>
    public class ManuSfcStepServices : IManuSfcStepServices
    {
        /// <summary>
        /// 条码步骤
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 
        /// </summary>
        public ManuSfcStepServices(IManuSfcStepRepository manuSfcStepRepository)
        {
            _manuSfcStepRepository = manuSfcStepRepository;
        }

        /// <summary>
        /// 新增步骤表
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task AddAsync(ManuSfcStepEvent @event)
        {
            await _manuSfcStepRepository.InsertAsync(@event.manuSfcStep);
        }

        /// <summary>
        /// 批量新增步骤表
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task AddRangeAsync(ManuSfcStepsEvent @event)
        {
            await _manuSfcStepRepository.InsertRangeAsync(@event.manuSfcStepEntities);
        }
    }
}
