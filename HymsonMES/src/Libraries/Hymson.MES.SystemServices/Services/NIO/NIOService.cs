using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Mavel;
using Hymson.MES.Core.Enums.Plan;
using Hymson.MES.Core.NIO;
using Hymson.MES.CoreServices.Extension;
using Hymson.MES.Data.NIO;
using Hymson.MES.Data.Repositories.NioPushSwitch;
using Hymson.MES.SystemServices.Dtos;
using Hymson.Snowflake;
using Hymson.Utils;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.SystemServices.Services.Quality
{
    /// <summary>
    /// 服务（NIO）
    /// </summary>
    public class NIOService : INIOService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<NIOService> _logger;

        /// <summary>
        /// 仓储接口（蔚来推送开关）
        /// </summary>
        private readonly INioPushSwitchRepository _nioPushSwitchRepository;

        /// <summary>
        /// 仓储接口（蔚来推送）
        /// </summary>
        private readonly INioPushRepository _nioPushRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="nioPushSwitchRepository"></param>
        /// <param name="nioPushRepository"></param>
        public NIOService(ILogger<NIOService> logger,
            INioPushSwitchRepository nioPushSwitchRepository,
            INioPushRepository nioPushRepository)
        {
            _logger = logger;
            _nioPushSwitchRepository = nioPushSwitchRepository;
            _nioPushRepository = nioPushRepository;
        }


        /// <summary>
        /// 队列（添加）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        public async Task<int> AddQueueAsync(IEnumerable<NIOAddDto> requestDtos)
        {
            if (requestDtos == null || !requestDtos.Any()) return 0;

            // 总开关是否开启
            var masterConfig = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.All);
            if (masterConfig == null || masterConfig.IsEnabled != TrueOrFalseEnum.Yes) return 0;

            // 初始化数据
            var user = "MES.System.Api";
            var time = HymsonClock.Now();

            // 通过BuzScene分组
            var buzSceneDict = requestDtos.ToLookup(x => x.BuzScene).ToDictionary(d => d.Key, d => d);

            var rows = 0;
            foreach (var buzScene in buzSceneDict)
            {
                // 子开关是否开启
                var config = await _nioPushSwitchRepository.GetBySceneAsync(buzScene.Key);
                if (config == null) continue;

                // 默认状态
                var status = PushStatusEnum.Wait;
                if (config.IsEnabled == TrueOrFalseEnum.No) status = PushStatusEnum.Off;

                // 保存
                rows += await _nioPushRepository.InsertRangeAsync(buzScene.Value.Select(s => new NioPushEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SchemaCode = config.SchemaCode,
                    BuzScene = s.BuzScene,
                    Status = status,
                    Content = new
                    {
                        config.SchemaCode,
                        List = s.Data
                    }.ToSerializeLower(),
                    Remark = s.BuzScene.GetDescription(),
                    CreatedBy = user,
                    CreatedOn = time,
                    UpdatedBy = user,
                    UpdatedOn = time
                }));
            }

            return rows;
        }

    }
}
