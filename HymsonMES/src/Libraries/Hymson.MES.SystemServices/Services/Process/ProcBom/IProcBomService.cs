using Hymson.MES.SystemServices.Dtos;

namespace Hymson.MES.SystemServices.Services.Process
{
    /// <summary>
    /// 服务接口（BOM）
    /// </summary>
    public interface IProcBomService
    {
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<BomDto>> GetBomListAsync();

    }
}
