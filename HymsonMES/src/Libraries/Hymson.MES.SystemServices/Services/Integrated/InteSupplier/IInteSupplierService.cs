using Hymson.MES.SystemServices.Dtos;

namespace Hymson.MES.SystemServices.Services.Integrated
{
    /// <summary>
    /// 服务接口（供应商）
    /// </summary>
    public interface IInteSupplierService
    {
        /// <summary>
        /// 同步信息（供应商）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        Task<int> SyncSupplierAsync(IEnumerable<InteSupplierDto> requestDtos);

    }
}
