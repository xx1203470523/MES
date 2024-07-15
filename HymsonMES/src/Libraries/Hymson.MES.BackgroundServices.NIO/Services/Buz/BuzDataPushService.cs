using Hymson.MES.BackgroundServices.NIO.Dtos.Buz;
using Hymson.MES.Core.Enums.Mavel;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务（业务数据）
    /// </summary>
    public class BuzDataPushService : BasePushService, IBuzDataPushService
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
        public BuzDataPushService(INioPushSwitchRepository nioPushSwitchRepository, INioPushRepository nioPushRepository)
            : base(nioPushSwitchRepository, nioPushRepository)
        {
            _nioPushSwitchRepository = nioPushSwitchRepository;
            _nioPushRepository = nioPushRepository;
        }

        /// <summary>
        /// 业务数据（控制项）
        /// </summary>
        /// <returns></returns>
        public async Task CollectionAsync()
        {
            var buzScene = BuzSceneEnum.Buz_Collection;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<CollectionDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 业务数据（生产业务）
        /// </summary>
        /// <returns></returns>
        public async Task ProductionAsync()
        {
            var buzScene = BuzSceneEnum.Buz_Production;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<ProductionDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 业务数据（材料清单）
        /// </summary>
        /// <returns></returns>
        public async Task MaterialAsync()
        {
            var buzScene = BuzSceneEnum.Buz_Material;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<MaterialDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 业务数据（产品一次合格率）
        /// </summary>
        /// <returns></returns>
        public async Task PassrateProductAsync()
        {
            var buzScene = BuzSceneEnum.Buz_PassrateProduct;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<PassrateProductDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 业务数据（工位一次合格率）
        /// </summary>
        /// <returns></returns>
        public async Task PassrateStationAsync()
        {
            var buzScene = BuzSceneEnum.Buz_PassrateStation;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<PassrateStationDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 业务数据（环境业务）
        /// </summary>
        /// <returns></returns>
        public async Task DataEnvAsync()
        {
            var buzScene = BuzSceneEnum.Buz_DataEnv;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<DataEnvDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 业务数据（缺陷业务）
        /// </summary>
        /// <returns></returns>
        public async Task IssueAsync()
        {
            var buzScene = BuzSceneEnum.Buz_Issue;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<IssueDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 业务数据（工单业务）
        /// </summary>
        /// <returns></returns>
        public async Task WorkOrderAsync()
        {
            var buzScene = BuzSceneEnum.Buz_WorkOrder;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<WorkOrderDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 业务数据（通用业务）
        /// </summary>
        /// <returns></returns>
        public async Task CommonAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Buz_Common);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<CommonDto> { };
            await switchEntity.ExecuteAsync(dtos);
        }

        /// <summary>
        /// 业务数据（附件）
        /// </summary>
        /// <returns></returns>
        public async Task AttachmentAsync()
        {
            var buzScene = BuzSceneEnum.Buz_Attachment;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<AttachmentDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

    }
}
