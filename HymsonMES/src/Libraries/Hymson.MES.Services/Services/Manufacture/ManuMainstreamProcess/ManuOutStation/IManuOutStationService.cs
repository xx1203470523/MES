using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation
{
    /// <summary>
    /// 出站
    /// </summary>
    public interface IManuOutStationService
    {
        /// <summary>
        /// 执行（出站）
        /// </summary>
        /// <param name="dto"></param>
        Task ExecuteAsync(SFCWorkDto dto);
    }
}
