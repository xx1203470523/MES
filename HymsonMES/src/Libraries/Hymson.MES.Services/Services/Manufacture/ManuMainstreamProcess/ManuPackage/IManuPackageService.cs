using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuPackage
{
    /// <summary>
    /// 组装
    /// </summary>
    public interface IManuPackageService
    {
        /// <summary>
        /// 执行（组装）
        /// </summary>
        /// <param name="dto"></param>
        Task ExecuteAsync(SFCWorkDto dto);
    }
}
