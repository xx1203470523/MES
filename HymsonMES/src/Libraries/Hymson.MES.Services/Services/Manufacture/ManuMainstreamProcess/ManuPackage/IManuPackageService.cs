using Hymson.MES.Services.Bos.Manufacture;

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
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<int> PackageAsync(ManufactureBo bo);

    }
}
