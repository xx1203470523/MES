using Hymson.Infrastructure.Exceptions;
using Hymson.MES.BackgroundServices.NIO.Dtos;
using Hymson.MES.BackgroundServices.NIO.Dtos.Master;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Material;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Param;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Param.View;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Proceduce;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Proceduce.View;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Mavel;
using Hymson.MES.Core.NIO;
using Hymson.MES.Data.NIO;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.Utils;
using Newtonsoft.Json;

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
        /// 转子工序标识
        /// </summary>
        private readonly string ROTOR_PRODUCRE_FLAG = "R";

        /// <summary>
        /// NIO软件版本号
        /// </summary>
        private readonly string NIO_SOFT_VERSION = "A";

        /// <summary>
        /// NIO DEBUG
        /// </summary>
        private readonly bool NIO_DEBUG = true;

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
                //dto.NioSoftwareRevision = NIO_SOFT_VERSION;
                dto.NioHardwareRevision = curConfig.NioHardwareRevision;
                dto.Launched = curConfig.Launched;
                dto.NioProjectName = curConfig.NioProjectName;
                //dto.NioModel = "ES8";
                dto.VendorFactoryCode = curConfig.PlantId;
                dto.VendorHardwareRevision = "A";
                dto.Debug_MES = NIO_DEBUG;
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
                //if(item.Code.Length != 6)
                //{
                //    continue;
                //}
                int lineCode = 0;
                StationDto model = new StationDto();
                NIOConfigBaseDto curConfig = new NIOConfigBaseDto();
                if (item.Code.Contains(ROTOR_PRODUCRE_FLAG) == true) //转子
                {
                    var baseConfigModel = baseConfigList.Where(m => m.Code == "NioRotorConfig").FirstOrDefault();
                    if(baseConfigModel == null)
                    {
                        continue;
                    }
                    curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(baseConfigModel.Value);
                    lineCode = 1;
                }
                else //定子
                {
                    var baseConfigModel = baseConfigList.Where(m => m.Code == "NioStatorConfig").FirstOrDefault();
                    if (baseConfigModel == null)
                    {
                        continue;
                    }
                    curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(baseConfigModel.Value);
                    lineCode = 2;
                }
                model.PlantId = curConfig.PlantId;
                model.PlantName = curConfig.PlantName;
                model.WorkshopId = curConfig.WorkshopId;
                model.WorkshopName = curConfig.WorkshopName;
                model.ProductionLineId = curConfig.ProductionLineId;
                model.ProductionLineName = curConfig.ProductionLineName;
                model.VendorProductNum = curConfig.VendorProductCode;

                string producreCode = item.Code.Substring(item.Code.Length - 3);
                model.ProductionLineOrder = lineCode;
                model.StationOrder = Convert.ToInt32(producreCode);
                model.StationId = item.Code;
                model.StationName = item.Name;
                model.KeyStation = true;
                List<string> mesOp = new List<string>() { "R01OP160", "R01OP170", "R01OP180" };
                if(mesOp.Contains(item.Code) == true)
                {
                    model.KeyStation = false;
                }
                model.Debug_MES = NIO_DEBUG;
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
            paramList = paramList.Take(3);
            //基础数据配置
            var baseConfigList = await GetBaseConfig();

            int maxNum = 20;
            int batchNum = paramList.Count() / maxNum + 1;
            for(int i = 0; i < batchNum; ++i)
            {
                List<ProcedureParamMavelView> curParamList = paramList.Skip(i * maxNum).Take(maxNum).ToList();
                await FieldItemAsync(curParamList, baseConfigList, config, buzScene);
            }        

            #region

            ////组装数据
            //var dtos = new List<FieldDto> { };
            //foreach (var param in paramList)
            //{
            //    char firstChar = param.ParameterCode[0];
            //    if (firstChar != 'R' && firstChar != 'S')
            //    {
            //        continue;
            //    }
            //    string remark = param.Remark ?? "";

            //    NIOConfigBaseDto curConfig = new NIOConfigBaseDto();
            //    FieldDto model = new FieldDto();
            //    if (firstChar == 'R') //转子
            //    {
            //        var baseConfigModel = baseConfigList.Where(m => m.Code == "NioRotorConfig").FirstOrDefault();
            //        if (baseConfigModel == null)
            //        {
            //            continue;
            //        }
            //        curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(baseConfigModel.Value);
            //    }
            //    else //定子
            //    {
            //        var baseConfigModel = baseConfigList.Where(m => m.Code == "NioStatorConfig").FirstOrDefault();
            //        if (baseConfigModel == null)
            //        {
            //            continue;
            //        }
            //        curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(baseConfigModel.Value);
            //    }
            //    model.PlantId = curConfig.PlantId;
            //    model.WorkshopId = curConfig.WorkshopId;
            //    model.ProductionLineId = curConfig.ProductionLineId;
            //    model.VendorProductNum = curConfig.VendorProductCode;
            //    model.StationId = param.ProcedureCode;
            //    model.VendorFieldCode = param.ParameterCode;
            //    model.VendorFieldName = param.ParameterName;
            //    if(param.DataType == 2)
            //    {
            //        model.ValueType = "decimal";
            //    }
            //    else
            //    {
            //        model.ValueType = "string";
            //    }
            //    if(remark.Contains("cc") == true || remark.Contains("CC") == true)
            //    {
            //        model.Cc = true;
            //    }
            //    if (remark.Contains("SC") == true || remark.Contains("sc") == true)
            //    {
            //        model.Sc = true;
            //    }
            //    if(remark.Contains("SPC") == true || remark.Contains("spc") == true)
            //    {
            //        model.Spc = true;
            //    }
            //    model.UpdateTime = GetTimestamp(param.CreatedOn, param.UpdatedOn);
            //    model.UnitCn = string.IsNullOrEmpty(param.ParameterUnit) == true ? "待维护" : param.ParameterUnit;
            //    dtos.Add(model);
            //}

            //// TODO: 替换为实际数据
            ////var dtos = new List<FieldDto> { };
            //await AddToPushQueueAsync(config, buzScene, dtos);

            #endregion
        }

        /// <summary>
        /// 参数项推送
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="baseConfigList"></param>
        /// <returns></returns>
        private async Task FieldItemAsync(IEnumerable<ProcedureParamMavelView> paramList, 
            IEnumerable<SysConfigEntity> baseConfigList, NioPushSwitchEntity config,
            BuzSceneEnum buzScene)
        {
            //组装数据
            var dtos = new List<FieldDto> { };
            foreach (var param in paramList)
            {
                char firstChar = param.ParameterCode[0];
                if (firstChar != 'R' && firstChar != 'S')
                {
                    continue;
                }
                string remark = param.Remark ?? "";

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
                model.UpdateTime = GetTimestamp(param.CreatedOn, param.UpdatedOn);
                model.UnitCn = string.IsNullOrEmpty(param.ParameterUnit) == true ? "待维护" : param.ParameterUnit;
                model.UnitEn = model.UnitCn;
                if (param.DataType == 2)
                {
                    model.ValueType = "decimal";
                }
                else
                {
                    model.ValueType = "string";
                }
                if (remark.Contains("cc") == true || remark.Contains("CC") == true)
                {
                    model.Cc = true;
                }
                if (remark.Contains("SC") == true || remark.Contains("sc") == true)
                {
                    model.Sc = true;
                }
                if (remark.Contains("SPC") == true || remark.Contains("spc") == true)
                {
                    model.Spc = true;
                }
                if(param.UpperLimit != null)
                {
                    model.UpperLimit = (decimal)param.UpperLimit;
                }
                if(param.LowerLimit != null)
                {
                    model.LowerLimit = (decimal)param.LowerLimit;
                }
                if(param.CenterValue != null)
                {
                    model.StdValue = (decimal)param.CenterValue;
                }
                model.Category01 = "未维护";
                if (remark.Contains("测试项") == true)
                {
                    model.Category01 = "测试项";
                }
                if(remark.Contains("工艺参数") == true)
                {
                    model.Category01 = "工艺参数";
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

            ////站点配置
            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139));
            }
            long siteId = long.Parse(configEntities.ElementAt(0).Value);
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
            //获取工序
            MavelProducreQuery query = new MavelProducreQuery() { SiteId = siteId };
            var producreList = await _procProcedureMavelRepository.GetList(query);
            if (producreList == null || producreList.Any() == false)
            {
                return;
            }
            //获取工序一次良率
            var passrateStationConfigList = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.RotorPassrateStation });
            if (passrateStationConfigList == null || !passrateStationConfigList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139)).WithData("name", "RotorPassrateStation");
            }
            string passrateConfig = passrateStationConfigList.ElementAt(0).Value;

            foreach(var item in producreList)
            {
                StationDto model = new StationDto();
                NIOConfigBaseDto curConfig = new NIOConfigBaseDto();
                if (item.Code.Contains(ROTOR_PRODUCRE_FLAG) == true) //转子
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

                PassrateTargetDto dto = new PassrateTargetDto();
                dto.PlantId = curConfig.PlantId;
                dto.VendorProductCode = curConfig.VendorProductCode;
                dto.VendorProductName = curConfig.VendorProductName;
                dto.PassRateType = "station";
                dto.WorkshopId = curConfig.WorkshopId;
                dto.ProductionLineId = curConfig.ProductionLineId;
                dto.StationId = item.Code;
                dto.PassRateTarget = GetPassrateStation(passrateConfig, item.Code).ToString();
                dto.UpdateTime = GetTimestamp(DateTime.Now, DateTime.Now);
                dtos.Add(dto);
            }

            // TODO: 替换为实际数据
            await AddToPushQueueAsync(config, buzScene, dtos);

            decimal GetPassrateStation(string configValue, string procedureCode)
            {
                decimal result = 0.9m;
                if (string.IsNullOrEmpty(configValue) == true)
                {
                    return result;
                }

                List<string> list = configValue.Split('&').ToList();
                foreach (var item in list)
                {
                    List<string> itemList = item.Split('=').ToList();
                    if (itemList.Count != 2)
                    {
                        return result;
                    }
                    if (itemList[0] == "default")
                    {
                        result = Convert.ToDecimal(itemList[1]);
                        continue;
                    }
                    if (itemList[0] == procedureCode)
                    {
                        result = Convert.ToDecimal(itemList[1]);
                        break;
                    }
                }

                result = Math.Round(result, 5);
                return result;
            }
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
