using Hymson.MES.SystemServices.Dtos;

namespace Hymson.MES.SystemServices.Services.Integrated
{
    /// <summary>
    /// 服务接口（客户）
    /// </summary>
    public interface IInteCustomerService
    {
        /// <summary>
        /// 同步信息（客户）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        Task<int> SyncCustomerAsync(IEnumerable<InteCustomerDto> requestDtos);

    }
}
