using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.SystemServices.Dtos;

namespace Hymson.MES.SystemServices.Services.Process
{
    /// <summary>
    /// 服务（BOM）
    /// </summary>
    public class ProcBomService : IProcBomService
    {
        /// <summary>
        /// 仓储接口（BOM表）
        /// </summary>
        private readonly IProcBomRepository _procBomRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="procBomRepository"></param>
        public ProcBomService(IProcBomRepository procBomRepository)
        {
            _procBomRepository = procBomRepository;
        }

        /// <summary>
        /// BOM信息（同步）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        public async Task<int> SyncBomAsync(IEnumerable<BomDto> requestDtos)
        {
            // TODO 
            return await Task.FromResult(0);
        }

    }
}
