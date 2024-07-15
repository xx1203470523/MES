using Hymson.Infrastructure.Exceptions;
using Hymson.MES.BackgroundServices.NIO.Dtos.Master;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Material;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Material.View;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Proceduce;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Proceduce.View;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Mavel;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.Utils;
using System;
using System.Security.Policy;

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
        /// 仓储接口（蔚来推送）
        /// </summary>
        private readonly INioPushRepository _nioPushRepository;

        /// <summary>
        /// 系统配置
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 物料
        /// </summary>
        private readonly IProcMaterialMavelRepository _procMaterialMavelRepository;

        /// <summary>
        /// 工序
        /// </summary>
        private readonly IProcProcedureMavelRepository _procProcedureMavelRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nioPushSwitchRepository"></param>
        public MasterDataPushService(INioPushSwitchRepository nioPushSwitchRepository, INioPushRepository nioPushRepository,
            ISysConfigRepository sysConfigRepository,
            IProcMaterialMavelRepository procMaterialMavelRepository,
            IProcProcedureMavelRepository procProcedureMavelRepository)
            : base(nioPushSwitchRepository, nioPushRepository)
        {
            _nioPushSwitchRepository = nioPushSwitchRepository;
            _nioPushRepository = nioPushRepository;
            _sysConfigRepository = sysConfigRepository;
            _procMaterialMavelRepository = procMaterialMavelRepository;
            _procProcedureMavelRepository = procProcedureMavelRepository;
        }

        /// <summary>
        /// 主数据（产品）
        /// </summary>
        /// <returns></returns>
        public async Task ProductAsync()
        {
            var buzScene = BuzSceneEnum.Master_Product;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;
            //站点配置
            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139));
            }
            long siteId = long.Parse(configEntities.ElementAt(0).Value);
            //主数据物料查询
            MavelMaterialQuery mavelMaterialQuery = new MavelMaterialQuery() { SiteId = siteId };
            var materialList = await _procMaterialMavelRepository.GetSelfControlListAsync(mavelMaterialQuery);
            if (materialList == null || materialList.Any() == false)
            {
                return;
            }
            //数据组装
            List<ProductDto> dtos = materialList.Select(m => new ProductDto()
            {
                VendorProductCode = m.MaterialCode,
                VendorProductName = m.MaterialName,
                NioProductCode = m.MaterialCode,
                NioProductName = m.MaterialName,
                NioHardwareRevision = m.Version ?? "",
                NioSoftwareRevision = m.Version ?? "",
                NioModel = "ES8",
                NioProjectName = "",
                Launched = false,
                UpdateTime = GetTimestamp(m.CreatedOn, m.UpdatedOn)
            }).ToList();

            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 主数据（工序）
        /// </summary>
        /// <returns></returns>
        public async Task StationAsync()
        {
            var buzScene = BuzSceneEnum.Master_Station;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;
            //站点配置
            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139));
            }
            long siteId = long.Parse(configEntities.ElementAt(0).Value);
            //工厂，车间，产线配置
            SysConfigQuery configQuery = new SysConfigQuery() { Codes = new List<string>() { "Rotor", "Stator" } };
            var configList = await _sysConfigRepository.GetEntitiesAsync(configQuery);
            if (configList == null || !configList.Any() || configList.Count() != 2)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10140));
            }
            string rotorConfig = configList.Where(m => m.Type == SysConfigEnum.Rotor).FirstOrDefault().Value;
            string statorConfig = configList.Where(m => m.Type == SysConfigEnum.Stator).FirstOrDefault().Value;
            if (string.IsNullOrEmpty(rotorConfig) == true || string.IsNullOrEmpty(statorConfig) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10140));
            }
            List<string> rotorConfigList = rotorConfig.Split(",").ToList();
            List<string> statorConfigList = statorConfig.Split(",").ToList();
            if (rotorConfigList.Count() != 6 || statorConfigList.Count() != 6)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10140));
            }
           
            //主数据查询
            MavelProducreQuery query = new MavelProducreQuery() { SiteId = siteId };
            var producreList = await _procProcedureMavelRepository.GetList(query);
            if (producreList == null || producreList.Any() == false)
            {
                return;
            }
            //组装数据
            List<StationDto> dtos = new List<StationDto>();
            foreach (var item in  producreList)
            {
                StationDto model = new StationDto();
                if (item.Code.Length == 5) //转子
                {
                    model.PlantId = rotorConfigList[0];
                    model.PlantName = rotorConfigList[1];
                    model.WorkshopId = rotorConfigList[2];
                    model.WorkshopName = rotorConfigList[3];
                    model.ProductionLineId = rotorConfigList[4];
                    model.ProductionLineName = rotorConfigList[5];
                }
                else //定子
                {
                    model.PlantId = statorConfigList[0];
                    model.PlantName = statorConfigList[1];
                    model.WorkshopId = statorConfigList[2];
                    model.WorkshopName = statorConfigList[3];
                    model.ProductionLineId = statorConfigList[4];
                    model.ProductionLineName = statorConfigList[5];
                }
                string producreCode = item.Code.Substring(item.Code.Length - 3);
                model.ProductionLineOrder = Convert.ToInt32(producreCode);
                model.StationOrder = Convert.ToInt32(producreCode);
                model.StationId = item.Code;
                model.StationName = item.Name;
                model.KeyStation = true;
                model.VendorProductNum = "";
                model.UpdateTime = GetTimestamp(item.CreatedOn, item.UpdatedOn);
                dtos.Add(model);
            }
            await ExecutePushAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 主数据（控制项）
        /// </summary>
        /// <returns></returns>
        public async Task FieldAsync()
        {
            var buzScene = BuzSceneEnum.Master_Field;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<FieldDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 主数据（一次合格率目标）
        /// </summary>
        /// <returns></returns>
        public async Task PassrateTargetAsync()
        {
            var buzScene = BuzSceneEnum.Master_PassrateTarget;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<PassrateTargetDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 主数据（环境监测）
        /// </summary>
        /// <returns></returns>
        public async Task EnvFieldAsync()
        {
            var buzScene = BuzSceneEnum.Master_EnvField;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<EnvFieldDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 主数据（人员资质）
        /// </summary>
        /// <returns></returns>
        public async Task PersonCertAsync()
        {
            var buzScene = BuzSceneEnum.Master_PersonCert;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<PersonCertDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 主数据（排班）
        /// </summary>
        /// <returns></returns>
        public async Task TeamSchedulingAsync()
        {
            var buzScene = BuzSceneEnum.Master_TeamScheduling;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<TeamSchedulingDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="createTime"></param>
        /// <param name="updateTime"></param>
        /// <returns></returns>
        private long GetTimestamp(DateTime createTime, DateTime? updateTime)
        {
            if (updateTime == null)
            {
                return (long)(createTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            }
            return (long)((DateTime)updateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

    }
}
