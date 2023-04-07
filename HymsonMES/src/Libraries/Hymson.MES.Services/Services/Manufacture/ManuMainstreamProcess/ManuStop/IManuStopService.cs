using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation
{
    /// <summary>
    /// 中止
    /// </summary>
    public interface IManuStopService
    {
        /// <summary>
        /// 中止
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ExecuteAsync(SFCWorkDto dto);
    }
}
