using Hymson.MES.SystemServices.Dtos;

namespace Hymson.MES.SystemServices.Services.Process
{
    /// <summary>
    /// 服务（BOM）
    /// </summary>
    public class ProcBomService : IProcBomService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcBomService()
        {
            // TODO 
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BomDto>> GetBomListAsync()
        {
            // TODO 
            return await Task.FromResult(new List<BomDto>());
        }

    }
}
