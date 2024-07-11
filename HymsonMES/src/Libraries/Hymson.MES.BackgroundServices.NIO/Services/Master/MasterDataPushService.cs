using Hymson.MES.BackgroundServices.NIO.Dtos.Master;
using Hymson.MES.Core.Enums.Mavel;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务（主数据）
    /// </summary>
    public class MasterDataPushService : BasePushService, IMasterDataPushService
    {
        /// <summary>
        /// 仓储接口（蔚来推送开关）
        /// </summary>
        private readonly INioPushSwitchRepository _nioPushSwitchRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nioPushSwitchRepository"></param>
        public MasterDataPushService(INioPushSwitchRepository nioPushSwitchRepository) : base(nioPushSwitchRepository)
        {
            _nioPushSwitchRepository = nioPushSwitchRepository;
        }

        /// <summary>
        /// 主数据（产品）
        /// </summary>
        /// <returns></returns>
        public async Task ProductAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Master_Product);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<ProductDto> { };
            dtos.Add(new ProductDto
            {
                VendorProductCode = "合作伙伴总成产品代码",
                VendorProductName = "合作伙伴产品名称",
                NioProductCode = "NIO 产品物料号",
                NioProductName = "NIO 产品名称",
                NioHardwareRevision = "A.01.01.01",
                NioSoftwareRevision = "B.01.01.01",
                NioModel = "NIO 车型",
                NioProjectName = "seth",
                Launched = true,
                Debug = true,
                UpdateTime = 1662528391
            });

            await switchEntity.ExecuteAsync(dtos);
        }

        /// <summary>
        /// 主数据（工站）
        /// </summary>
        /// <returns></returns>
        public async Task StationAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Master_Station);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<StationDto> { };
            await switchEntity.ExecuteAsync(dtos);
        }

        /// <summary>
        /// 主数据（控制项）
        /// </summary>
        /// <returns></returns>
        public async Task FieldAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Master_Field);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<FieldDto> { };
            await switchEntity.ExecuteAsync(dtos);
        }

        /// <summary>
        /// 主数据（一次合格率目标）
        /// </summary>
        /// <returns></returns>
        public async Task PassrateTargetAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Master_PassrateTarget);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<PassrateTargetDto> { };
            await switchEntity.ExecuteAsync(dtos);
        }

        /// <summary>
        /// 主数据（环境监测）
        /// </summary>
        /// <returns></returns>
        public async Task EnvFieldAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Master_EnvField);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<EnvFieldDto> { };
            await switchEntity.ExecuteAsync(dtos);
        }

        /// <summary>
        /// 主数据（人员资质）
        /// </summary>
        /// <returns></returns>
        public async Task PersonCertAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Master_PersonCert);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<PersonCertDto> { };
            await switchEntity.ExecuteAsync(dtos);
        }

        /// <summary>
        /// 主数据（排班）
        /// </summary>
        /// <returns></returns>
        public async Task TeamSchedulingAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Master_TeamScheduling);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<TeamSchedulingDto> { };
            await switchEntity.ExecuteAsync(dtos);
        }

    }
}
