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
        /// 构造函数
        /// </summary>
        /// <param name="nioPushSwitchRepository"></param>
        public BuzDataPushService(INioPushSwitchRepository nioPushSwitchRepository) : base(nioPushSwitchRepository)
        {
            _nioPushSwitchRepository = nioPushSwitchRepository;
        }

        /// <summary>
        /// 业务数据（控制项）
        /// </summary>
        /// <returns></returns>
        public async Task CollectionAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Buz_Collection);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<CollectionDto> { };
            await switchEntity.ExecuteAsync(dtos);
        }

        /// <summary>
        /// 业务数据（生产业务）
        /// </summary>
        /// <returns></returns>
        public async Task ProductionAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Buz_Production);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<ProductionDto> { };
            await switchEntity.ExecuteAsync(dtos);
        }

        /// <summary>
        /// 业务数据（材料清单）
        /// </summary>
        /// <returns></returns>
        public async Task MaterialAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Buz_Material);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<MaterialDto> { };
            await switchEntity.ExecuteAsync(dtos);
        }

        /// <summary>
        /// 业务数据（产品一次合格率）
        /// </summary>
        /// <returns></returns>
        public async Task PassrateProductAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Buz_PassrateProduct);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<PassrateProductDto> { };
            await switchEntity.ExecuteAsync(dtos);
        }

        /// <summary>
        /// 业务数据（工位一次合格率）
        /// </summary>
        /// <returns></returns>
        public async Task PassrateStationAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Buz_PassrateStation);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<PassrateStationDto> { };
            await switchEntity.ExecuteAsync(dtos);
        }

        /// <summary>
        /// 业务数据（环境业务）
        /// </summary>
        /// <returns></returns>
        public async Task DataEnvAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Buz_DataEnv);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<DataEnvDto> { };
            await switchEntity.ExecuteAsync(dtos);
        }

        /// <summary>
        /// 业务数据（缺陷业务）
        /// </summary>
        /// <returns></returns>
        public async Task IssueAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Buz_Issue);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<IssueDto> { };
            await switchEntity.ExecuteAsync(dtos);
        }

        /// <summary>
        /// 业务数据（工单业务）
        /// </summary>
        /// <returns></returns>
        public async Task WorkOrderAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Buz_WorkOrder);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<WorkOrderDto> { };
            await switchEntity.ExecuteAsync(dtos);
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
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Buz_Attachment);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<AttachmentDto> { };
            await switchEntity.ExecuteAsync(dtos);
        }

    }
}
