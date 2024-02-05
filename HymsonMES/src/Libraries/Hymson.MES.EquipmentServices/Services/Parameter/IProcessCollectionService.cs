using Hymson.MES.EquipmentServices.Dtos.Parameter;

namespace Hymson.MES.EquipmentServices.Services.Parameter.ProcessCollection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProcessCollectionService
    {
        /// <summary>
        /// 参数采集
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task ProductCollectionAsync(ProductProcessParameterDto request);

        /// <summary>
        /// 设备参数采集
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentCollectionAsync(IEnumerable<EquipmentProcessParameterDto> request);

    }
}
