using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuPackage
{
    /// <summary>
    /// 维修
    /// </summary>
    public interface IManuRepairService
    {
        /// <summary>
        /// 维修
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <returns></returns>
        Task<int> StartAsync(ManuSfcProduceEntity sfcProduceEntity);

    }
}
