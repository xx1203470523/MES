using Hymson.MES.BackgroundServices.NIO.Dtos;
using Hymson.MES.Core.Enums.Mavel;
using Hymson.Utils;
using RestSharp;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务（Mock）
    /// </summary>
    public class MockDataPushService : BasePushService, IMockDataPushService
    {
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
        /// <param name="nioPushSwitchRepository"></param>
        public MockDataPushService(INioPushSwitchRepository nioPushSwitchRepository, INioPushRepository nioPushRepository)
            : base(nioPushSwitchRepository, nioPushRepository)
        {
            _nioPushSwitchRepository = nioPushSwitchRepository;
            _nioPushRepository = nioPushRepository;
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        public async Task HelloAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Mock_Hello);
            if (switchEntity == null) return;

            // 组装数据
            var dataObj = new
            {
                Hello = "world"
            };

            var response = await NIOPushClient.ExecuteAsync(switchEntity.Path, dataObj, Method.Post);
            var result = response.Content?.ToDeserializeLower<NIOResponseDto>();
        }

    }
}
