using Hymson.MES.BackgroundServices.NIO.Dtos;
using Hymson.MES.BackgroundServices.NIO.Dtos.Process;
using Hymson.MES.Core.Enums.Mavel;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务（业务数据）
    /// </summary>
    public class BuzDataPushService : BasePushService
    {
        /// <summary>
        /// 仓储接口（蔚来推送开关）
        /// </summary>
        private readonly INioPushSwitchRepository _nioPushSwitchRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nioPushSwitchRepository"></param>
        public BuzDataPushService(INioPushSwitchRepository nioPushSwitchRepository)
        {
            _nioPushSwitchRepository = nioPushSwitchRepository;
        }

        /// <summary>
        /// 业务数据（控制项）
        /// </summary>
        /// <returns></returns>
        public async Task CollectionAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.Buz_Collection);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<CollectionDto> { };

            await ExecuteAsync(switchEntity.Path, new ClientRequestDto<CollectionDto> { SchemaCode = switchEntity.SchemaCode, List = dtos });
        }

        /// <summary>
        /// 业务数据（生产业务）
        /// </summary>
        /// <returns></returns>
        public async Task ProductionAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.Buz_Production);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<ProductionDto> { };

            await ExecuteAsync(switchEntity.Path, new ClientRequestDto<ProductionDto> { SchemaCode = switchEntity.SchemaCode, List = dtos });
        }

        /// <summary>
        /// 业务数据（材料清单）
        /// </summary>
        /// <returns></returns>
        public async Task MaterialAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.Buz_Material);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<MaterialDto> { };

            await ExecuteAsync(switchEntity.Path, new ClientRequestDto<MaterialDto> { SchemaCode = switchEntity.SchemaCode, List = dtos });
        }

        /// <summary>
        /// 业务数据（产品一次合格率）
        /// </summary>
        /// <returns></returns>
        public async Task PassrateProductAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.Buz_PassrateProduct);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<PassrateProductDto> { };

            await ExecuteAsync(switchEntity.Path, new ClientRequestDto<PassrateProductDto> { SchemaCode = switchEntity.SchemaCode, List = dtos });
        }

        /// <summary>
        /// 业务数据（工位一次合格率）
        /// </summary>
        /// <returns></returns>
        public async Task PassrateStationAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.Buz_PassrateStation);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<PassrateStationDto> { };

            await ExecuteAsync(switchEntity.Path, new ClientRequestDto<PassrateStationDto> { SchemaCode = switchEntity.SchemaCode, List = dtos });
        }

        /// <summary>
        /// 业务数据（环境业务）
        /// </summary>
        /// <returns></returns>
        public async Task DataEnvAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.Buz_DataEnv);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<DataEnvDto> { };

            await ExecuteAsync(switchEntity.Path, new ClientRequestDto<DataEnvDto> { SchemaCode = switchEntity.SchemaCode, List = dtos });
        }

        /// <summary>
        /// 业务数据（缺陷业务）
        /// </summary>
        /// <returns></returns>
        public async Task IssueAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.Buz_Issue);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<IssueDto> { };

            await ExecuteAsync(switchEntity.Path, new ClientRequestDto<IssueDto> { SchemaCode = switchEntity.SchemaCode, List = dtos });
        }

        /// <summary>
        /// 业务数据（工单业务）
        /// </summary>
        /// <returns></returns>
        public async Task WorkOrderAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.Buz_WorkOrder);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<WorkOrderDto> { };

            await ExecuteAsync(switchEntity.Path, new ClientRequestDto<WorkOrderDto> { SchemaCode = switchEntity.SchemaCode, List = dtos });
        }

        /// <summary>
        /// 业务数据（通用业务）
        /// </summary>
        /// <returns></returns>
        public async Task CommonAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.Buz_Common);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<CommonDto> { };

            await ExecuteAsync(switchEntity.Path, new ClientRequestDto<CommonDto> { SchemaCode = switchEntity.SchemaCode, List = dtos });
        }

        /// <summary>
        /// 业务数据（附件）
        /// </summary>
        /// <returns></returns>
        public async Task AttachmentAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.Buz_Attachment);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<AttachmentDto> { };

            await ExecuteAsync(switchEntity.Path, new ClientRequestDto<AttachmentDto> { SchemaCode = switchEntity.SchemaCode, List = dtos });
        }

    }
}
