using Hymson.MES.BackgroundServices.NIO.Dtos.Master;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Mavel;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务基类
    /// </summary>
    public class BasePushService
    {
        /// <summary>
        /// 仓储接口（蔚来推送开关）
        /// </summary>
        private readonly INioPushSwitchRepository _nioPushSwitchRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nioPushSwitchRepository"></param>
        public BasePushService(INioPushSwitchRepository nioPushSwitchRepository)
        {
            _nioPushSwitchRepository = nioPushSwitchRepository;
        }

        /// <summary>
        /// 是否允许推送
        /// </summary>
        /// <param name="buzSceneEnum"></param>
        /// <returns></returns>
        public async Task<NioPushSwitchEntity?> GetSwitchEntityAsync(BuzSceneEnum buzSceneEnum)
        {
            // 总开关是否开启
            var masterSwitchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.All);
            if (masterSwitchEntity == null || masterSwitchEntity.IsEnabled == TrueOrFalseEnum.No) return default;

            // 子开关是否开启
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(buzSceneEnum);
            if (switchEntity == null || switchEntity.IsEnabled == TrueOrFalseEnum.No) return default;

            return switchEntity;
        }

    }
}
