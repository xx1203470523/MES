using Hymson.MES.SystemServices.Dtos;

namespace Hymson.MES.SystemServices.Services.Process
{
    /// <summary>
    /// 服务接口（物料）
    /// </summary>
    public interface IProcMaterialService
    {
        /// <summary>
        /// 同步信息（物料）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        Task<int> SyncMaterialAsync(IEnumerable<MaterialDto> requestDtos);

    }
}
