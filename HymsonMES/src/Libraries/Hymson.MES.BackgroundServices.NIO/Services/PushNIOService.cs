using Hymson.MES.BackgroundServices.NIO.Dtos;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Mavel;
using Hymson.MES.Core.Enums.Plan;
using Hymson.MES.Core.NIO;
using Hymson.MES.CoreServices.Extension;
using Hymson.MES.Data.NIO;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.WaterMark;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务（蔚来）
    /// </summary>
    public class PushNIOService : IPushNIOService
    {
        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        public readonly IWaterMarkService _waterMarkService;

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
        /// <param name="waterMarkService"></param>
        /// <param name="nioPushSwitchRepository"></param>
        /// <param name="nioPushRepository"></param>
        public PushNIOService(IWaterMarkService waterMarkService,
            INioPushSwitchRepository nioPushSwitchRepository,
            INioPushRepository nioPushRepository)
        {
            _waterMarkService = waterMarkService;
            _nioPushSwitchRepository = nioPushSwitchRepository;
            _nioPushRepository = nioPushRepository;
        }

        /// <summary>
        /// 发送推送
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task<int> ExecutePushAsync(int limitCount = 1000)
        {
            // 查询全部开关配置
            var configEntities = await _nioPushSwitchRepository.GetEntitiesAsync(new NioPushSwitchQuery { });
            if (configEntities == null || !configEntities.Any()) return default;

            // 总开关是否开启
            var masterConfig = configEntities.FirstOrDefault(f => f.BuzScene == BuzSceneEnum.All);
            if (masterConfig == null || masterConfig.IsEnabled != TrueOrFalseEnum.Yes) return default;

            // 水位ID
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(WaterMarkKey.PushToNIO);

            // 获取步骤表数据
            var waitPushEntities = await _nioPushRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            {
                StartWaterMarkId = waterMarkId,
                Rows = limitCount
            });
            if (waitPushEntities == null || !waitPushEntities.Any()) return default;

            // 遍历推送数据
            List<NioPushEntity> updates = new();
            foreach (var data in waitPushEntities)
            {
                var config = configEntities.FirstOrDefault(f => f.BuzScene == data.BuzScene);
                if (config == null || config.IsEnabled != TrueOrFalseEnum.Yes) continue;

                // 推送
                var restResponse = await config.ExecuteAsync(data.Content);

                // 处理推送结果
                data.Status = PushStatusEnum.Failure;
                if (restResponse.IsSuccessStatusCode)
                {
                    var responseContent = restResponse.Content?.ToDeserializeLower<NIOResponseDto>();
                    if (responseContent != null && responseContent.NexusOpenapi.Code == "QM-000000")
                    {
                        data.Status = PushStatusEnum.Success;
                    }
                }

                data.Result = restResponse.Content;
                data.UpdatedBy = "PushToNIO";
                data.UpdatedOn = HymsonClock.Now();
                updates.Add(data);
            }

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();

            // 更新状态
            rows = await _nioPushRepository.UpdateRangeAsync(updates);

            // 更新水位
            await _waterMarkService.RecordWaterMarkAsync(WaterMarkKey.PushToNIO, waitPushEntities.Max(x => x.Id));
            trans.Complete();

            return rows;
        }

    }
}
