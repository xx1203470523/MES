using Hymson.MES.SystemServices.Dtos;

namespace Hymson.MES.SystemServices.Services.Process
{
    /// <summary>
    /// 服务接口（BOM）
    /// </summary>
    public interface IProcBomService
    {
        /// <summary>
        /// 同步信息（BOM）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        Task<int> SyncBomAsync(IEnumerable<SyncBomDto> requestDtos);

    }
}
