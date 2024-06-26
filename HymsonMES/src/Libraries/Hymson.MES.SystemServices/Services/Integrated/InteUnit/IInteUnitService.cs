using Hymson.MES.SystemServices.Dtos;

namespace Hymson.MES.SystemServices.Services.Integrated
{
    /// <summary>
    /// 服务接口（单位）
    /// </summary>
    public interface IInteUnitService
    {
        /// <summary>
        /// 同步信息（供应商）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        Task<int> SyncUnitAsync(IEnumerable<InteUnitDto> requestDtos);

    }
}
