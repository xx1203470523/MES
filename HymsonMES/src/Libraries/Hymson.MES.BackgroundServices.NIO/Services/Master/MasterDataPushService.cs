using Hymson.Infrastructure.Exceptions;
using Hymson.MES.BackgroundServices.NIO.Dtos;
using Hymson.MES.BackgroundServices.NIO.Dtos.Master;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Material;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Material.View;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Param;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Param.View;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Proceduce;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Proceduce.View;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Mavel;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.Utils;
using Newtonsoft.Json;
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
        private readonly IProcProductParameterGroupMavelRepository _procProductParameterGroupMavelRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nioPushSwitchRepository"></param>
        public MasterDataPushService(INioPushSwitchRepository nioPushSwitchRepository, INioPushRepository nioPushRepository,
            ISysConfigRepository sysConfigRepository,
            IProcMaterialMavelRepository procMaterialMavelRepository,
            IProcProcedureMavelRepository procProcedureMavelRepository,
            IProcProductParameterGroupMavelRepository procProductParameterGroupMavelRepository)
            : base(nioPushSwitchRepository, nioPushRepository)
        {
            _nioPushSwitchRepository = nioPushSwitchRepository;
            _nioPushRepository = nioPushRepository;
            _sysConfigRepository = sysConfigRepository;
            _procMaterialMavelRepository = procMaterialMavelRepository;
            _procProcedureMavelRepository = procProcedureMavelRepository;
            _procProductParameterGroupMavelRepository = procProductParameterGroupMavelRepository;
        }

        /// <summary>
        /// 获取基础配置数据
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<SysConfigEntity>> GetBaseConfig()
        {
            //基础数据配置
            SysConfigQuery configQuery = new SysConfigQuery();
            configQuery.Type = SysConfigEnum.NioBaseConfig;
            configQuery.Codes = new List<string>() { "NioRotorConfig", "NioStatorConfig" };
            var baseConfigList = await _sysConfigRepository.GetEntitiesAsync(configQuery);
            if (baseConfigList == null || !baseConfigList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139)).WithData("name", "NioRotorConfig&NioStatorConfig");
            }

            return baseConfigList;
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

            //基础数据配置
            var baseConfigList = await GetBaseConfig();

            List<ProductDto> dtos = new List<ProductDto>();
            foreach (var item in baseConfigList)
            {
                var curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(item.Value);

                ProductDto dto = new ProductDto();
                dto.VendorProductCode = curConfig.VendorProductCode;
                dto.VendorProductName = curConfig.VendorProductName;
                dto.NioProductCode = curConfig.NioProductCode;
                dto.NioProductName = curConfig.NioProductName;
                dto.NioHardwareRevision = "1.0";
                dto.NioSoftwareRevision = "1.0";
                dto.NioModel = "ES8";
                dto.Launched = false;
                dto.UpdateTime = GetTimestamp(HymsonClock.Now(),HymsonClock.Now());

                dtos.Add(dto);
            }

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
            ////站点配置
            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139));
            }
            long siteId = long.Parse(configEntities.ElementAt(0).Value);

            //基础数据配置
            var baseConfigList = await GetBaseConfig();

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
                NIOConfigBaseDto curConfig = new NIOConfigBaseDto();
                if (item.Code.Length == 5) //转子
                {
                    var baseConfigModel = baseConfigList.Where(m => m.Code == "NioRotorConfig").FirstOrDefault();
                    if(baseConfigModel == null)
                    {
                        continue;
                    }
                    curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(baseConfigModel.Value);
                }
                else //定子
                {
                    var baseConfigModel = baseConfigList.Where(m => m.Code == "NioStatorConfig").FirstOrDefault();
                    if (baseConfigModel == null)
                    {
                        continue;
                    }
                    curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(baseConfigModel.Value);
                }
                model.PlantId = curConfig.PlantId;
                model.PlantName = curConfig.PlantName;
                model.WorkshopId = curConfig.WorkshopId;
                model.WorkshopName = curConfig.WorkshopName;
                model.ProductionLineId = curConfig.ProductionLineId;
                model.ProductionLineName = curConfig.ProductionLineName;

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
            MavelParamQuery query = new MavelParamQuery();
            query.SiteId = siteId;
            var paramList = await _procProductParameterGroupMavelRepository.GetParamListAsync(query);
            if(paramList == null || paramList.Any() == false)
            {
                return;
            }
            //基础数据配置
            var baseConfigList = await GetBaseConfig();

            //组装数据
            var dtos = new List<FieldDto> { };
            foreach (var param in paramList)
            {
                char firstChar = param.ParameterCode[0];
                if (firstChar != 'R' && firstChar != 'S')
                {
                    continue;
                }

                NIOConfigBaseDto curConfig = new NIOConfigBaseDto();
                FieldDto model = new FieldDto();
                if (firstChar == 'R') //转子
                {
                    var baseConfigModel = baseConfigList.Where(m => m.Code == "NioRotorConfig").FirstOrDefault();
                    if (baseConfigModel == null)
                    {
                        continue;
                    }
                    curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(baseConfigModel.Value);
                }
                else //定子
                {
                    var baseConfigModel = baseConfigList.Where(m => m.Code == "NioStatorConfig").FirstOrDefault();
                    if (baseConfigModel == null)
                    {
                        continue;
                    }
                    curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(baseConfigModel.Value);
                }
                model.PlantId = curConfig.PlantId;
                model.WorkshopId = curConfig.WorkshopId;
                model.ProductionLineId = curConfig.ProductionLineId;
                model.VendorProductNum = curConfig.VendorProductCode;
                model.StationId = param.ProcedureCode;
                model.VendorFieldCode = param.ParameterCode;
                model.VendorFieldName = param.ParameterName;
                model.VendorFieldDesc = param.ParameterName;
                model.FieldObject = "定子&转子";
                model.FieldType = "无";
                if(param.DataType == 2)
                {
                    model.ValueType = "decimal";
                }
                else
                {
                    model.ValueType = "string";
                }
                model.LifecycleCode = "process";
                model.DetectionMethod = "设备检测";
                model.DeviceId = "123";
                model.LimitUpdateTime = GetTimestamp(param.CreatedOn, param.UpdatedOn);
                model.UpdateTime = GetTimestamp(HymsonClock.Now(),HymsonClock.Now());

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
            //基础数据配置
            var baseConfigList = await GetBaseConfig();

            //产品一次良率
            var dtos = new List<PassrateTargetDto> { };
            foreach (var item in baseConfigList)
            {
                NIOConfigBaseDto curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(item.Value);
                PassrateTargetDto dto = new PassrateTargetDto();
                dto.PlantId = curConfig.PlantId;
                dto.VendorProductCode = curConfig.VendorProductCode;
                dto.VendorProductName = curConfig.VendorProductName;
                dto.PassRateType = "product";
                dto.WorkshopId = curConfig.WorkshopId;
                dto.ProductionLineId = curConfig.ProductionLineId;
                dto.StationId = "";
                dto.PassRateTarget = curConfig.PassRateTarget;
                dto.UpdateTime = GetTimestamp(DateTime.Now, DateTime.Now);

                dtos.Add(dto);
            }
            //工序一次良率，待确认

            // TODO: 替换为实际数据
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
