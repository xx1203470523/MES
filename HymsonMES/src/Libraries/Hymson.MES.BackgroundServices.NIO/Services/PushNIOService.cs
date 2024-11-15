using Google.Protobuf.WellKnownTypes;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.BackgroundServices.NIO.Dtos;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Mavel;
using Hymson.MES.Core.Enums.Plan;
using Hymson.MES.Core.NIO;
using Hymson.MES.CoreServices.Extension;
using Hymson.MES.Data.NIO;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.NioPushSwitch;
using Hymson.MES.Data.Repositories.NioPushSwitch.Query;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.WaterMark;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Hymson.MES.BackgroundServices.NIO.Utils;
using Hymson.MES.BackgroundServices.NIO.Dtos.Buz;
using Hymson.MES.BackgroundServices.NIO.Dtos.Master;
using Hymson.MES.Data.Repositories.NioPushCollection;
using Hymson.MES.BackgroundServices.NIO.Dtos.ERP;
using Hymson.MES.Data.Repositories.NIO;
using RestSharp;
using Microsoft.Extensions.Logging;
using Quartz.Logging;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务（蔚来）
    /// </summary>
    public class PushNIOService : IPushNIOService
    {
        private readonly ILogger<PushNIOService> _logger;

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
        /// 系统配置
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// NIO参数
        /// </summary>
        private readonly INioPushCollectionRepository _nioPushCollectionRepository;

        /// <summary>
        /// NIO
        /// </summary>
        private readonly INioPushProductioncapacityRepository _nioPushProductioncapacityRepository;

        /// <summary>
        /// NIO
        /// </summary>
        private readonly INioPushKeySubordinateRepository _nioPushKeySubordinateRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly INioPushActualDeliveryRepository _nioPushActualDeliveryRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waterMarkService"></param>
        /// <param name="nioPushSwitchRepository"></param>
        /// <param name="nioPushRepository"></param>
        public PushNIOService(ILogger<PushNIOService> logger,
            IWaterMarkService waterMarkService,
            INioPushSwitchRepository nioPushSwitchRepository,
            INioPushRepository nioPushRepository,
            ISysConfigRepository sysConfigRepository,
            INioPushCollectionRepository nioPushCollectionRepository,
            INioPushProductioncapacityRepository nioPushProductioncapacityRepository,
            INioPushKeySubordinateRepository nioPushKeySubordinateRepository,
            INioPushActualDeliveryRepository nioPushActualDeliveryRepository)
        {
            _logger = logger;
            _waterMarkService = waterMarkService;
            _nioPushSwitchRepository = nioPushSwitchRepository;
            _nioPushRepository = nioPushRepository;
            _sysConfigRepository = sysConfigRepository;
            _nioPushCollectionRepository = nioPushCollectionRepository;
            _nioPushProductioncapacityRepository = nioPushProductioncapacityRepository;
            _nioPushKeySubordinateRepository = nioPushKeySubordinateRepository;
            _nioPushActualDeliveryRepository = nioPushActualDeliveryRepository;
        }

        /// <summary>
        /// 发送推送
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task<int> ExecutePushAsync(int limitCount = 100)
        {
            // 获取步骤表数据
            var waitPushEntities = await _nioPushRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            {
                Rows = limitCount
            });
            if (waitPushEntities == null || !waitPushEntities.Any()) return default;

            List<Task<int>> tasks = new();

            int maxNum = 30;
            int batchNum = limitCount / maxNum + 1;
            for (int i = 0;i < batchNum; ++i)
            {
                var curList = waitPushEntities.Skip(i * maxNum).Take(maxNum);
                tasks.Add(ExecuteAsync(limitCount, curList));
            }
            var rowArray = await Task.WhenAll(tasks);

            //return await ExecuteAsync(limitCount, waitPushEntities);

            return 0;

            // 查询全部开关配置
            var configEntities = await _nioPushSwitchRepository.GetEntitiesAsync(new NioPushSwitchQuery { });
            if (configEntities == null || !configEntities.Any()) return default;

            // 总开关是否开启
            var masterConfig = configEntities.FirstOrDefault(f => f.BuzScene == BuzSceneEnum.All);
            if (masterConfig == null || masterConfig.IsEnabled != TrueOrFalseEnum.Yes) return default;

            //站点配置
            string host = string.Empty;
            string hostsuffix = string.Empty;
            IEnumerable<SysConfigEntity>? nioUrlList = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.NioUrl });
            if (nioUrlList != null && nioUrlList.Count() > 0)
            {
                var hostEntity = nioUrlList.Where(m => m.Code.ToLower() == "host").FirstOrDefault();
                if(hostEntity != null)
                {
                    host = hostEntity.Value;
                }
                var hostsuffixEntity = nioUrlList.Where(m => m.Code.ToLower() == "hostsuffix").FirstOrDefault();
                if (hostsuffixEntity != null)
                {
                    hostsuffix = hostsuffixEntity.Value;
                }
            }

            // 水位ID
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(WaterMarkKey.PushToNIO);

            // 获取步骤表数据
            //var waitPushEntities = await _nioPushRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            //{
            //    StartWaterMarkId = waterMarkId,
            //    Rows = limitCount
            //});
            //if (waitPushEntities == null || !waitPushEntities.Any()) return default;

            // 遍历推送数据
            List<NioPushEntity> updates = new();
            foreach (var data in waitPushEntities)
            {
                var config = configEntities.FirstOrDefault(f => f.BuzScene == data.BuzScene);
                if (config == null)
                {
                    //data.Status = PushStatusEnum.NotConfigured;
                    continue;
                }
                else
                {
                    config.IsEnabled = GetMapEnum(data, config, configEntities);

                    if (config.IsEnabled == TrueOrFalseEnum.Yes)
                    {
                        // 推送
                        var restResponse = await config.ExecuteAsync(data.Content, host, hostsuffix, "");

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
                    }
                    else
                    {
                        //data.Status = PushStatusEnum.Off;
                        continue;
                    }
                }

                data.UpdatedBy = "PushToNIO";
                data.UpdatedOn = HymsonClock.Now();
                updates.Add(data);
            }

            if(updates == null || updates.Count == 0)
            {
                return 0;
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

        /// <summary>
        /// 推送失败的数据
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task<int> ExecutePushFailAsync(int limitCount = 100)
        {
            // 获取步骤表数据
            var waitPushEntities = await _nioPushRepository.GetFailListAsync(new EntityByWaterMarkQuery
            {
                Rows = limitCount
            });
            if (waitPushEntities == null || !waitPushEntities.Any()) return default;
            return await ExecuteAsync(limitCount, waitPushEntities);

            // 查询全部开关配置
            var configEntities = await _nioPushSwitchRepository.GetEntitiesAsync(new NioPushSwitchQuery { });
            if (configEntities == null || !configEntities.Any()) return default;

            // 总开关是否开启
            var masterConfig = configEntities.FirstOrDefault(f => f.BuzScene == BuzSceneEnum.All);
            if (masterConfig == null || masterConfig.IsEnabled != TrueOrFalseEnum.Yes) return default;

            //站点配置
            string host = string.Empty;
            string hostsuffix = string.Empty;
            IEnumerable<SysConfigEntity>? nioUrlList = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.NioUrl });
            if (nioUrlList != null && nioUrlList.Count() > 0)
            {
                var hostEntity = nioUrlList.Where(m => m.Code.ToLower() == "host").FirstOrDefault();
                if (hostEntity != null)
                {
                    host = hostEntity.Value;
                }
                var hostsuffixEntity = nioUrlList.Where(m => m.Code.ToLower() == "hostsuffix").FirstOrDefault();
                if (hostsuffixEntity != null)
                {
                    hostsuffix = hostsuffixEntity.Value;
                }
            }

            // 水位ID
            //var waterMarkId = await _waterMarkService.GetWaterMarkAsync(WaterMarkKey.PushToNIO);

            // 获取步骤表数据
            //var waitPushEntities = await _nioPushRepository.GetFailListAsync(new EntityByWaterMarkQuery
            //{
            //    Rows = limitCount
            //});
            //if (waitPushEntities == null || !waitPushEntities.Any()) return default;

            // 遍历推送数据
            List<NioPushEntity> updates = new();
            foreach (var data in waitPushEntities)
            {
                var config = configEntities.FirstOrDefault(f => f.BuzScene == data.BuzScene);
                if (config == null)
                {
                    //data.Status = PushStatusEnum.NotConfigured;
                    continue;
                }
                else
                {
                    config.IsEnabled = GetMapEnum(data, config, configEntities);

                    if (config.IsEnabled == TrueOrFalseEnum.Yes)
                    {
                        // 推送
                        var restResponse = await config.ExecuteAsync(data.Content, host, hostsuffix, "");

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
                    }
                    else
                    {
                        //data.Status = PushStatusEnum.Off;
                        continue;
                    }
                }

                data.UpdatedBy = "PushToNIO";
                data.UpdatedOn = HymsonClock.Now();
                updates.Add(data);
            }

            if (updates == null || updates.Count == 0)
            {
                return 0;
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

        /// <summary>
        /// 发送推送
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        private async Task<int> ExecuteAsync(int limitCount, IEnumerable<NioPushEntity> waitPushEntities)
        {
            if (waitPushEntities == null || !waitPushEntities.Any()) return default;

            // 查询全部开关配置
            var configEntities = await _nioPushSwitchRepository.GetEntitiesAsync(new NioPushSwitchQuery { });
            if (configEntities == null || !configEntities.Any()) return default;

            // 总开关是否开启
            var masterConfig = configEntities.FirstOrDefault(f => f.BuzScene == BuzSceneEnum.All);
            if (masterConfig == null || masterConfig.IsEnabled != TrueOrFalseEnum.Yes) return default;

            //站点配置
            string host = string.Empty;
            string hostsuffix = string.Empty;
            string appsec = string.Empty;
            IEnumerable<SysConfigEntity>? nioUrlList = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.NioUrl });
            if (nioUrlList != null && nioUrlList.Count() > 0)
            {
                var hostEntity = nioUrlList.Where(m => m.Code.ToLower() == "host").FirstOrDefault();
                if (hostEntity != null)
                {
                    host = hostEntity.Value;
                }
                var hostsuffixEntity = nioUrlList.Where(m => m.Code.ToLower() == "hostsuffix").FirstOrDefault();
                if (hostsuffixEntity != null)
                {
                    hostsuffix = hostsuffixEntity.Value;
                }
                var appsecEntity = nioUrlList.Where(m => m.Code.ToLower() == "appsecrect").FirstOrDefault();
                if(appsecEntity != null)
                {
                    appsec = appsecEntity.Value;
                }
            }

            // 水位ID
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(WaterMarkKey.PushToNIO);

            //// 获取步骤表数据
            //var waitPushEntities = await _nioPushRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            //{
            //    StartWaterMarkId = waterMarkId,
            //    Rows = limitCount
            //});
            //if (waitPushEntities == null || !waitPushEntities.Any()) return default;

            // 遍历推送数据
            List<NioPushEntity> updates = new();
            foreach (var data in waitPushEntities)
            {
                var config = configEntities.FirstOrDefault(f => f.BuzScene == data.BuzScene);
                if (config == null)
                {
                    //data.Status = PushStatusEnum.NotConfigured;
                    continue;
                }
                else
                {
                    config.IsEnabled = GetMapEnum(data, config, configEntities);

                    if (config.IsEnabled == TrueOrFalseEnum.Yes)
                    {
                        string pushContent = string.Empty;
                        if (data.BuzScene == BuzSceneEnum.Buz_Collection || data.BuzScene == BuzSceneEnum.Buz_Collection_Summary)
                        {
                            pushContent = await GetCollectionData(data.Id, data.SchemaCode);
                        }
                        else if(data.BuzScene == BuzSceneEnum.ERP_ProductionCapacity || data.BuzScene == BuzSceneEnum.ERP_ProductionCapacity_Summary)
                        {
                            pushContent = await GetNioStockInfoData(data.Id, data.SchemaCode);
                        }
                        else if(data.BuzScene == BuzSceneEnum.ERP_KeySubordinate || data.BuzScene == BuzSceneEnum.ERP_KeySubordinate_Summary)
                        {
                            pushContent = await GetNioKeyItemInfoData(data.Id, data.SchemaCode);
                        }
                        else if(data.BuzScene == BuzSceneEnum.ERP_ActualDelivery || data.BuzScene == BuzSceneEnum.ERP_ActualDelivery_Summary)
                        {
                            pushContent = await GetNioActualDeliveryData(data.Id, data.SchemaCode);
                        }
                        else
                        {
                            //这里将数据序列化在反序列化，更新时间戳字段
                            pushContent = NioUpdateTime(data.BuzScene, data.Content);
                        }

                        if(string.IsNullOrEmpty(pushContent) == false)
                        {
                            //MES推送业务数据到NIO系统，执行的方法

                            _logger.LogInformation($"MES推送NIO的定时任务，推送前 -> pushContent = {pushContent}；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");

                            _logger.LogInformation($"MES推送NIO的定时任务，推送前 -> Request: {data.ToSerialize()}；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");

                            var restResponse = await config.ExecuteAsync(pushContent, host, hostsuffix, appsec);

                            _logger.LogInformation($"MES推送NIO的定时任务，推送后 -> 结果【restResponse】= {restResponse}；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");

                            // 处理推送结果
                            data.Status = PushStatusEnum.Failure;
                            if (restResponse.IsSuccessStatusCode)
                            {
                                var responseContent = restResponse.Content?.ToDeserializeLower<NIOResponseDto>();

                                _logger.LogInformation($"MES推送NIO的定时任务，推送后 -> 结果【responseContent】= {responseContent}；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");

                                if (responseContent != null && responseContent.NexusOpenapi.Code == "QM-000000")
                                {
                                    data.Status = PushStatusEnum.Success;
                                }
                            }

                            data.Result = restResponse.Content;
                        }
                        else
                        {
                            data.Status = PushStatusEnum.Success;
                            data.Result = "数据为0，无需推送";
                        }
                    }
                    else
                    {
                        //data.Status = PushStatusEnum.Off;
                        continue;
                    }
                }

                data.UpdatedBy = "PushToNIO";
                data.UpdatedOn = HymsonClock.Now();

                _logger.LogInformation($"MES推送NIO的定时任务，推送后 -> data = {data}；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");


                updates.Add(data);
            }

            if (updates == null || updates.Count == 0)
            {
                return 0;
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


        /// <summary>
        /// 获取枚举映射
        /// </summary>
        /// <param name="sum">推送场景</param>
        /// <param name="data">推送开关</param>
        /// <param name="list">配置列表</param>
        /// <returns></returns>
        private TrueOrFalseEnum GetMapEnum(NioPushEntity sum, NioPushSwitchEntity data, IEnumerable<NioPushSwitchEntity> list)
        {
            if (list == null || list.Count() == 0)
            {
                return data.IsEnabled;
            }

            //主数据
            if (sum.BuzScene == BuzSceneEnum.Master_Product_Summary)
            {
                var curBuz = list.Where(m => m.BuzScene == BuzSceneEnum.Master_Product).FirstOrDefault();
                if (curBuz != null)
                {
                    return curBuz.IsEnabled;
                }
            }
            if (sum.BuzScene == BuzSceneEnum.Master_Station_Summary)
            {
                var curBuz = list.Where(m => m.BuzScene == BuzSceneEnum.Master_Station).FirstOrDefault();
                if (curBuz != null)
                {
                    return curBuz.IsEnabled;
                }
            }
            if (sum.BuzScene == BuzSceneEnum.Master_Field_Summary)
            {
                var curBuz = list.Where(m => m.BuzScene == BuzSceneEnum.Master_Field).FirstOrDefault();
                if (curBuz != null)
                {
                    return curBuz.IsEnabled;
                }
            }
            if (sum.BuzScene == BuzSceneEnum.Master_PassrateTarget_Summary)
            {
                var curBuz = list.Where(m => m.BuzScene == BuzSceneEnum.Master_PassrateTarget).FirstOrDefault();
                if (curBuz != null)
                {
                    return curBuz.IsEnabled;
                }
            }
            //业务数据
            if (sum.BuzScene == BuzSceneEnum.Buz_Collection_Summary)
            {
                var curBuz = list.Where(m => m.BuzScene == BuzSceneEnum.Buz_Collection).FirstOrDefault();
                if (curBuz != null)
                {
                    return curBuz.IsEnabled;
                }
            }
            if (sum.BuzScene == BuzSceneEnum.Buz_Production_Summary)
            {
                var curBuz = list.Where(m => m.BuzScene == BuzSceneEnum.Buz_Production).FirstOrDefault();
                if (curBuz != null)
                {
                    return curBuz.IsEnabled;
                }
            }
            if (sum.BuzScene == BuzSceneEnum.Buz_Material_Summary)
            {
                var curBuz = list.Where(m => m.BuzScene == BuzSceneEnum.Buz_Material).FirstOrDefault();
                if (curBuz != null)
                {
                    return curBuz.IsEnabled;
                }
            }
            if (sum.BuzScene == BuzSceneEnum.Buz_PassrateProduct_Summary)
            {
                var curBuz = list.Where(m => m.BuzScene == BuzSceneEnum.Buz_PassrateProduct).FirstOrDefault();
                if (curBuz != null)
                {
                    return curBuz.IsEnabled;
                }
            }
            if (sum.BuzScene == BuzSceneEnum.Buz_Issue_Summary)
            {
                var curBuz = list.Where(m => m.BuzScene == BuzSceneEnum.Buz_Issue).FirstOrDefault();
                if (curBuz != null)
                {
                    return curBuz.IsEnabled;
                }
            }
            if (sum.BuzScene == BuzSceneEnum.Buz_WorkOrder_Summary)
            {
                var curBuz = list.Where(m => m.BuzScene == BuzSceneEnum.Buz_WorkOrder).FirstOrDefault();
                if (curBuz != null)
                {
                    return curBuz.IsEnabled;
                }
            }
            //ERP&WMS
            if (sum.BuzScene == BuzSceneEnum.ERP_ProductionCapacity_Summary)
            {
                var curBuz = list.Where(m => m.BuzScene == BuzSceneEnum.ERP_ProductionCapacity).FirstOrDefault();
                if (curBuz != null)
                {
                    return curBuz.IsEnabled;
                }
            }
            if (sum.BuzScene == BuzSceneEnum.ERP_KeySubordinate_Summary)
            {
                var curBuz = list.Where(m => m.BuzScene == BuzSceneEnum.ERP_KeySubordinate).FirstOrDefault();
                if (curBuz != null)
                {
                    return curBuz.IsEnabled;
                }
            }
            if (sum.BuzScene == BuzSceneEnum.ERP_ActualDelivery_Summary)
            {
                var curBuz = list.Where(m => m.BuzScene == BuzSceneEnum.ERP_ActualDelivery).FirstOrDefault();
                if (curBuz != null)
                {
                    return curBuz.IsEnabled;
                }
            }

            return data.IsEnabled;
        }
    
        /// <summary>
        /// 更新NIO的时间戳
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private string NioUpdateTime(BuzSceneEnum scene, string content)
        {
            string result = content;

            JsonSerializerSettings settings = NioHelper.GetJsonSerializer();
            long timestmap = NioHelper.GetTimestamp(HymsonClock.Now());

            #region 主数据
            if (scene == BuzSceneEnum.Master_Field || scene == BuzSceneEnum.Master_Field_Summary)
            {
                NioFieldDto dto = JsonConvert.DeserializeObject<NioFieldDto>(content);
                dto.List.ForEach(m => m.UpdateTime = timestmap);
                result = JsonConvert.SerializeObject(dto, settings);
            }
            else if (scene == BuzSceneEnum.Master_PassrateTarget || scene == BuzSceneEnum.Master_PassrateTarget_Summary)
            {
                NioPassrateTargetDto dto = JsonConvert.DeserializeObject<NioPassrateTargetDto>(content);
                dto.List.ForEach(m => m.UpdateTime = timestmap);
                result = JsonConvert.SerializeObject(dto, settings);
            }
            else if (scene == BuzSceneEnum.Master_Product || scene == BuzSceneEnum.Master_Product_Summary)
            {
                NioProductDto dto = JsonConvert.DeserializeObject<NioProductDto>(content);
                dto.List.ForEach(m => m.UpdateTime = timestmap);
                result = JsonConvert.SerializeObject(dto, settings);
            }
            else if (scene == BuzSceneEnum.Master_Station || scene == BuzSceneEnum.Master_Station_Summary)
            {
                NioStationDto dto = JsonConvert.DeserializeObject<NioStationDto>(content);
                dto.List.ForEach(m => m.UpdateTime = timestmap);
                result = JsonConvert.SerializeObject(dto, settings);
            }
            #endregion

            #region 业务数据
            if (scene == BuzSceneEnum.Buz_Collection || scene == BuzSceneEnum.Buz_Collection_Summary)
            {
                NioCollectionDto dto = JsonConvert.DeserializeObject<NioCollectionDto>(content);
                dto.List.ForEach(m => m.UpdateTime = timestmap);
                result = JsonConvert.SerializeObject(dto, settings);
            }
            else if(scene == BuzSceneEnum.Buz_Production || scene == BuzSceneEnum.Buz_Production_Summary)
            {
                NioProductionDto dto = JsonConvert.DeserializeObject<NioProductionDto>(content);
                dto.List.ForEach(m => m.UpdateTime = timestmap);
                result = JsonConvert.SerializeObject(dto, settings);
            }
            else if (scene == BuzSceneEnum.Buz_PassrateProduct || scene == BuzSceneEnum.Buz_PassrateProduct_Summary)
            {
                NioPassrateProductDto dto = JsonConvert.DeserializeObject<NioPassrateProductDto>(content);
                dto.List.ForEach(m => m.UpdateTime = timestmap);
                result = JsonConvert.SerializeObject(dto, settings);
            }
            else if (scene == BuzSceneEnum.Buz_Issue || scene == BuzSceneEnum.Buz_Issue_Summary)
            {
                NioIssueDto dto = JsonConvert.DeserializeObject<NioIssueDto>(content);
                dto.List.ForEach(m => m.UpdateTime = timestmap);
                result = JsonConvert.SerializeObject(dto, settings);
            }
            else if (scene == BuzSceneEnum.Buz_Material || scene == BuzSceneEnum.Buz_Material_Summary)
            {
                //备注说明：update_time是子节点扣料时间，不是上传时间

                //NioMaterialDto dto = JsonConvert.DeserializeObject<NioMaterialDto>(content);
                //dto.List.ForEach(m => m.UpdateTime = timestmap);
                //result = JsonConvert.SerializeObject(dto, settings);
            }
            else if (scene == BuzSceneEnum.Buz_WorkOrder || scene == BuzSceneEnum.Buz_WorkOrder_Summary)
            {
                NioWorkOrderDto dto = JsonConvert.DeserializeObject<NioWorkOrderDto>(content);
                dto.List.ForEach(m => m.UpdateTime = timestmap);
                result = JsonConvert.SerializeObject(dto, settings);
            }
            #endregion

            //NioWorkOrderDto
            return result;
        }
    
        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="nioPushId"></param>
        /// <returns></returns>
        private async Task<string> GetCollectionData(long nioPushId, string schemaCode)
        {
            //获取对应的数据
            var dbList = await _nioPushCollectionRepository.GetByPushIdAsync(nioPushId);
            //获取配置
            long timestmap = NioHelper.GetTimestamp(HymsonClock.Now());
            JsonSerializerSettings settings = NioHelper.GetJsonSerializer();
            //将列表转为字符串
            string tmpPushContext = JsonConvert.SerializeObject(dbList);
            List<CollectionDto> pushList = JsonConvert.DeserializeObject<List<CollectionDto>>(tmpPushContext);
            //pushList.ForEach(m => m.UpdateTime = timestmap);
            NioCollectionDto nioSch = new NioCollectionDto() { List = pushList };
            nioSch.SchemaCode = schemaCode;

            return JsonConvert.SerializeObject(nioSch, settings);
        }

        /// <summary>
        /// 获取NIO合作伙伴精益与库存信息数据
        /// </summary>
        /// <param name="nioPushId"></param>
        /// <param name="schemaCode"></param>
        /// <returns></returns>
        private async Task<string> GetNioStockInfoData(long nioPushId, string schemaCode)
        {
            //获取对应的数据
            var dbList = await _nioPushProductioncapacityRepository.GetByPushIdAsync(nioPushId);
            //获取配置
            JsonSerializerSettings settings = NioHelper.GetJsonSerializer();
            string tmpPushContext = JsonConvert.SerializeObject(dbList);
            List<ProductionCapacityDto> pushList = JsonConvert.DeserializeObject<List<ProductionCapacityDto>>(tmpPushContext);
            NioProductionCapacityDto nioSch = new NioProductionCapacityDto() { List = pushList };
            nioSch.SchemaCode = schemaCode;

            return JsonConvert.SerializeObject(nioSch, settings);
        }

        /// <summary>
        /// 获取NIO物料及其关键下级件
        /// </summary>
        /// <param name="nioPushId"></param>
        /// <param name="schemaCode"></param>
        /// <returns></returns>
        private async Task<string> GetNioKeyItemInfoData(long nioPushId, string schemaCode)
        {
            //获取对应的数据
            var dbList = await _nioPushKeySubordinateRepository.GetByPushIdAsync(nioPushId);
            //获取配置
            JsonSerializerSettings settings = NioHelper.GetJsonSerializer();
            string tmpPushContext = JsonConvert.SerializeObject(dbList);
            List<KeySubordinateDto> pushList = JsonConvert.DeserializeObject<List<KeySubordinateDto>>(tmpPushContext);
            NioKeySubordinateDto nioSch = new NioKeySubordinateDto() { List = pushList };
            nioSch.SchemaCode = schemaCode;

            return JsonConvert.SerializeObject(nioSch, settings);
        }

        /// <summary>
        /// 获取NIO物料发货信息
        /// </summary>
        /// <param name="nioPushId"></param>
        /// <param name="schemaCode"></param>
        /// <returns></returns>
        private async Task<string> GetNioActualDeliveryData(long nioPushId, string schemaCode)
        {
            //获取对应的数据
            var dbList = await _nioPushActualDeliveryRepository.GetByPushIdAsync(nioPushId);
            dbList = dbList.Where(m => m.ShippedQty > 0).ToList();
            if(dbList == null || dbList.Count() == 0)
            {
                return "";
            }
            //获取配置
            JsonSerializerSettings settings = NioHelper.GetJsonSerializer();
            string tmpPushContext = JsonConvert.SerializeObject(dbList);
            List<ActualDeliveryDto> pushList = JsonConvert.DeserializeObject<List<ActualDeliveryDto>>(tmpPushContext);
            NioActualDeliveryDto nioSch = new NioActualDeliveryDto() { List = pushList };
            nioSch.SchemaCode = schemaCode;

            return JsonConvert.SerializeObject(nioSch, settings);
        }
    }
}
