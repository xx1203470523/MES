using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation
{
    /// <summary>
    /// 开始
    /// </summary>
    public interface IManuStartupService
    {
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ExecuteAsync(SFCWorkDto dto);
    }
}
