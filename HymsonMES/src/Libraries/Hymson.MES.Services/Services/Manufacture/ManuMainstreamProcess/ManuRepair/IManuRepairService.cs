using Hymson.MES.Services.Bos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuPackage
{
    /// <summary>
    /// 维修
    /// </summary>
    public interface IManuRepairService
    {
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<int> StartAsync(ManufactureBo bo);

    }
}
