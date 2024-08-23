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

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务（蔚来）
    /// </summary>
    public class PushNIOService : IPushNIOService
    {
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
        /// 构造函数
        /// </summary>
        /// <param name="waterMarkService"></param>
        /// <param name="nioPushSwitchRepository"></param>
        /// <param name="nioPushRepository"></param>
        public PushNIOService(IWaterMarkService waterMarkService,
            INioPushSwitchRepository nioPushSwitchRepository,
            INioPushRepository nioPushRepository,
            ISysConfigRepository sysConfigRepository)
        {
            _waterMarkService = waterMarkService;
            _nioPushSwitchRepository = nioPushSwitchRepository;
            _nioPushRepository = nioPushRepository;
            _sysConfigRepository = sysConfigRepository;
        }

        /// <summary>
        /// 发送推送
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task<int> ExecutePushAsync(int limitCount = 100)
        {
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
            var waitPushEntities = await _nioPushRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            {
                StartWaterMarkId = waterMarkId,
                Rows = limitCount
            });
            if (waitPushEntities == null || !waitPushEntities.Any()) return default;

            // 遍历推送数据
            List<NioPushEntity> updates = new();
            foreach (var data in waitPushEntities)
            {
                var config = configEntities.FirstOrDefault(f => f.BuzScene == data.BuzScene);
                if (config == null)
                {
                    data.Status = PushStatusEnum.NotConfigured;
                }
                else
                {
                    config.IsEnabled = GetMapEnum(data, config, configEntities);

                    if (config.IsEnabled == TrueOrFalseEnum.Yes)
                    {
                        // 推送
                        var restResponse = await config.ExecuteAsync(data.Content, host, hostsuffix);

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
                        data.Status = PushStatusEnum.Off;
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
    }
}
