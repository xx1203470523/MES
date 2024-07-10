using Hymson.MES.BackgroundServices.NIO.Dtos;
using Hymson.MES.BackgroundServices.NIO.Dtos.Master;
using Hymson.MES.Core.Enums.Mavel;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务（主数据）
    /// </summary>
    public class MasterDataPushService : BasePushService
    {
        /// <summary>
        /// 仓储接口（蔚来推送开关）
        /// </summary>
        private readonly INioPushSwitchRepository _nioPushSwitchRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nioPushSwitchRepository"></param>
        public MasterDataPushService(INioPushSwitchRepository nioPushSwitchRepository)
        {
            _nioPushSwitchRepository = nioPushSwitchRepository;
        }

        /// <summary>
        /// 主数据（产品）
        /// </summary>
        /// <returns></returns>
        public async Task ProductAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.Master_Product);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<ProductDto> { };

            await ExecuteAsync(switchEntity.Path, new ClientRequestDto<ProductDto> { SchemaCode = switchEntity.SchemaCode, List = dtos });
        }

        /// <summary>
        /// 主数据（工站）
        /// </summary>
        /// <returns></returns>
        public async Task StationAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.Master_Station);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<StationDto> { };

            await ExecuteAsync(switchEntity.Path, new ClientRequestDto<StationDto> { SchemaCode = switchEntity.SchemaCode, List = dtos });
        }

        /// <summary>
        /// 主数据（控制项）
        /// </summary>
        /// <returns></returns>
        public async Task FieldAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.Master_Field);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<FieldDto> { };

            await ExecuteAsync(switchEntity.Path, new ClientRequestDto<FieldDto> { SchemaCode = switchEntity.SchemaCode, List = dtos });
        }

        /// <summary>
        /// 主数据（一次合格率目标）
        /// </summary>
        /// <returns></returns>
        public async Task PassrateTargetAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.Master_PassrateTarget);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<PassrateTargetDto> { };

            await ExecuteAsync(switchEntity.Path, new ClientRequestDto<PassrateTargetDto> { SchemaCode = switchEntity.SchemaCode, List = dtos });
        }

        /// <summary>
        /// 主数据（环境监测）
        /// </summary>
        /// <returns></returns>
        public async Task EnvFieldAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.Master_EnvField);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<EnvFieldDto> { };

            await ExecuteAsync(switchEntity.Path, new ClientRequestDto<EnvFieldDto> { SchemaCode = switchEntity.SchemaCode, List = dtos });
        }

        /// <summary>
        /// 主数据（人员资质）
        /// </summary>
        /// <returns></returns>
        public async Task PersonCertAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.Master_PersonCert);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<PersonCertDto> { };

            await ExecuteAsync(switchEntity.Path, new ClientRequestDto<PersonCertDto> { SchemaCode = switchEntity.SchemaCode, List = dtos });
        }

        /// <summary>
        /// 主数据（排班）
        /// </summary>
        /// <returns></returns>
        public async Task TeamSchedulingAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.Master_TeamScheduling);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<TeamSchedulingDto> { };

            await ExecuteAsync(switchEntity.Path, new ClientRequestDto<TeamSchedulingDto> { SchemaCode = switchEntity.SchemaCode, List = dtos });
        }

    }
}
