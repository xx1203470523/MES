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
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
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
        /// 参数
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nioPushSwitchRepository"></param>
        public MasterDataPushService(INioPushSwitchRepository nioPushSwitchRepository, INioPushRepository nioPushRepository,
            ISysConfigRepository sysConfigRepository,
            IProcMaterialMavelRepository procMaterialMavelRepository,
            IProcProcedureMavelRepository procProcedureMavelRepository,
            IProcParameterRepository procParameterRepository)
            : base(nioPushSwitchRepository, nioPushRepository)
        {
            _nioPushSwitchRepository = nioPushSwitchRepository;
            _nioPushRepository = nioPushRepository;
            _sysConfigRepository = sysConfigRepository;
            _procMaterialMavelRepository = procMaterialMavelRepository;
            _procProcedureMavelRepository = procProcedureMavelRepository;
            _procParameterRepository = procParameterRepository;
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
                throw new CustomerValidationException(nameof(ErrorCode.MES10139)).WithData("name", SysConfigEnum.MainSite.ToString());
            }
            long siteId = long.Parse(configEntities.ElementAt(0).Value);
            //主数据物料查询
            var nioMatList = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.NioMaterial });
            if (nioMatList == null || !configEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139)).WithData("name", SysConfigEnum.NioMaterial.ToString());
            }
            string nioMatConfigValue = nioMatList.ElementAt(0).Value;
            DateTime createdOn = nioMatList.ElementAt(0).CreatedOn;
            List<string> configList = nioMatConfigValue.Split('&').ToList();
            List<ProductDto> dtos = new List<ProductDto>();
            foreach (var item in configList)
            {
                List<string> mapMat = item.Split('=').ToList();
                List<string> curMesList = mapMat[0].Split(',').ToList();
                List<string> curNioList = mapMat[1].Split(',').ToList();

                ProductDto dto = new ProductDto();
                dto.VendorProductCode = curMesList[0];
                dto.VendorProductName = curMesList[1];
                dto.NioProductCode = curNioList[0];
                dto.NioProductName = curNioList[1];
                dto.NioHardwareRevision = "1.0";
                dto.NioSoftwareRevision = "1.0";
                dto.NioModel = "ES8";
                dto.NioProductName = "";
                dto.Launched = false;
                dto.UpdateTime = GetTimestamp(createdOn, createdOn);

                dtos.Add(dto);
            }

            //MavelMaterialQuery mavelMaterialQuery = new MavelMaterialQuery() { SiteId = siteId };
            //var materialList = await _procMaterialMavelRepository.GetSelfControlListAsync(mavelMaterialQuery);
            //if (materialList == null || materialList.Any() == false)
            //{
            //    return;
            //}

            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 获取转子，定义仙子配置
        /// </summary>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        private async Task<(List<string>,List<string>)> GetLineConfig()
        {
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

            return (rotorConfigList, statorConfigList);
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
            ////站点配置
            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139));
            }
            long siteId = long.Parse(configEntities.ElementAt(0).Value);
            //工厂，车间，产线配置
            var lineConfig = await GetLineConfig();
            List<string> rotorConfigList = lineConfig.Item1;
            List<string> statorConfigList = lineConfig.Item2;

            //主数据查询
            MavelProducreQuery query = new MavelProducreQuery() { SiteId = siteId };
            var producreList = await _procProcedureMavelRepository.GetList(query);
            if (producreList == null || producreList.Any() == false)
            {
                return;
            }
            //组装数据
            List<StationDto> dtos = new List<StationDto>();
            foreach (var item in producreList)
            {
                if(item.Code.Length != 5 && item.Code.Length != 6)
                {
                    continue;
                }
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
            await AddToPushQueueAsync(config, buzScene, dtos);
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
            //站点配置
            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139));
            }
            long siteId = long.Parse(configEntities.ElementAt(0).Value);
            //参数查询
            ProcParameterQuery query = new ProcParameterQuery();
            query.SiteId = siteId;
            var paramList = await _procParameterRepository.GetProcParameterEntitiesAsync(query);
            if(paramList == null || paramList.Any() == false)
            {
                return;
            }
            //获取车间线体配置
            var lineConfig = await GetLineConfig();
            List<string> rotorConfigList = lineConfig.Item1;
            List<string> statorConfigList = lineConfig.Item2;

            //组装数据
            var dtos = new List<FieldDto> { };
            foreach (var param in paramList)
            {
                char firstChar = param.ParameterCode[0];
                if(firstChar != 'R' && firstChar != 'S')
                {
                    continue;
                }

                FieldDto model = new FieldDto();
                if (firstChar == 'R') //转子
                {
                    model.PlantId = rotorConfigList[0];
                    model.WorkshopId = rotorConfigList[2];
                    model.ProductionLineId = rotorConfigList[4];
                }
                else //定子
                {
                    model.PlantId = statorConfigList[0];
                    model.WorkshopId = statorConfigList[2];
                    model.ProductionLineId = statorConfigList[4];
                }

                dtos.Add(model);
            }

            // TODO: 替换为实际数据
            //var dtos = new List<FieldDto> { };
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
