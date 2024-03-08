using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Quality;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Services.Quality
{
    /// <summary>
    /// 服务（AQL检验水平查询）
    /// </summary>
    public class AQLLevelQueryService : IAQLLevelQueryService
    {
        /// <summary>
        /// 仓储接口（系统配置）
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysConfigRepository"></param>
        public AQLLevelQueryService(ISysConfigRepository sysConfigRepository)
        {
            _sysConfigRepository = sysConfigRepository;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AQLLevelBo>> QueryListAsync(long siteId)
        {
            var entities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery
            {
                SiteId = siteId,
                Type = SysConfigEnum.AQLLevel
                //Codes = new List<string> { AQLStandard.MIL }
            });
            if (entities == null || !entities.Any()) return Array.Empty<AQLLevelBo>();

            // 转换为导出模型
            List<AQLLevelBo> dtos = new();
            foreach (var item in entities)
            {
                var ranges = item.Value.ToDeserialize<IEnumerable<AQLLevelBo>>();
                if (ranges == null) continue;

                dtos.AddRange(ranges);
            }
            if (dtos == null || !dtos.Any()) return Array.Empty<AQLLevelBo>();

            return dtos;
        }
    }
}
