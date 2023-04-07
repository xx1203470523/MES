using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuInStation
{
    /// <summary>
    /// 进站
    /// </summary>
    public interface IManuInStationService
    {
        /// <summary>
        /// 执行（进站）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ExecuteAsync(SFCWorkDto dto);
    }
}
