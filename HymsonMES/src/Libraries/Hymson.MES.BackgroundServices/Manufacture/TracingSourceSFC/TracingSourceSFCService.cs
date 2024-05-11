using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using Hymson.WaterMark;

namespace Hymson.MES.BackgroundServices.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class TracingSourceSFCService : ITracingSourceSFCService
    {
        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        public readonly IWaterMarkService _waterMarkService;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（条码）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 仓储接口（条码流转）
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 仓储接口（条码追溯）
        /// </summary>
        private readonly IManuSFCNodeRepository _manuSFCNodeRepository;

        /// <summary>
        /// 仓储接口（条码追溯-反向）
        /// </summary>
        private readonly IManuSFCNodeSourceRepository _manuSFCNodeSourceRepository;

        /// <summary>
        /// 仓储接口（条码追溯-正向）
        /// </summary>
        private readonly IManuSFCNodeDestinationRepository _manuSFCNodeDestinationRepository;

        /// <summary>
        /// 信号量数组
        /// </summary>
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waterMarkService"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        public TracingSourceSFCService(IWaterMarkService waterMarkService,
            IProcMaterialRepository procMaterialRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuSFCNodeRepository manuSFCNodeRepository,
            IManuSFCNodeSourceRepository manuSFCNodeSourceRepository,
            IManuSFCNodeDestinationRepository manuSFCNodeDestinationRepository)
        {
            _waterMarkService = waterMarkService;
            _procMaterialRepository = procMaterialRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuSFCNodeRepository = manuSFCNodeRepository;
            _manuSFCNodeSourceRepository = manuSFCNodeSourceRepository;
            _manuSFCNodeDestinationRepository = manuSFCNodeDestinationRepository;
        }

        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(int limitCount = 1000)
        {
            // 等待进入信号量
            await _semaphore.WaitAsync();

            try
            {
                var waterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.TracingSourceSFC);

                // 获取流转表数据（因为这张表的数据会有更新操作，所以不能用常规水位）
                DateTime startWaterMarkTime = ConvertFromUnixTimeMilliseconds(waterMarkId);
                var manuSfcCirculationList = await _manuSfcCirculationRepository.GetListByStartWaterMarkTimeAsync(new EntityByWaterMarkTimeQuery
                {
                    StartWaterMarkTime = startWaterMarkTime,
                    Rows = limitCount
                });
                if (manuSfcCirculationList == null || !manuSfcCirculationList.Any()) return;

                var user = $"{BusinessKey.TracingSourceSFC}作业";

                // 通过站点ID分组
                var manuSfcCirculationSiteIdDict = manuSfcCirculationList.ToLookup(x => x.SiteId).ToDictionary(d => d.Key, d => d);

                List<ManuSFCNodeEntity> nodeEntities = new();
                List<ManuSFCNodeSourceEntity> nodeSourceEntities = new();
                List<ManuSFCNodeDestinationEntity> nodeDestinationEntities = new();
                List<ManuSfcEntity> sfcEntities = new();
                foreach (var item in manuSfcCirculationSiteIdDict)
                {
                    var barCodes = item.Value.Select(s => s.SFC).Union(item.Value.Select(s => s.CirculationBarCode)).Distinct();

                    // 根据流转条码批量查询条码
                    sfcEntities.AddRange(await _manuSfcRepository.GetListAsync(new ManuSfcQuery
                    {
                        SiteId = item.Key,
                        SFCs = barCodes,
                        //Type = SfcTypeEnum.Produce （注意：经过这步之后，仅在库存，而不在条码表的数据会被过滤掉）
                    }));
                }

                // 获取所有条码ID
                var sfcIds = sfcEntities.Select(s => s.Id);

                // 加载数据已经存在的节点信息，不存在的条码在后面会实例一个节点
                nodeEntities.AddRange(await _manuSFCNodeRepository.GetByIdsAsync(sfcIds));

                // 加载已存在的节点的来源信息
                nodeSourceEntities.AddRange(await _manuSFCNodeSourceRepository.GetEntitiesAsync(sfcIds));

                // 加载已存在的节点的去向信息
                nodeDestinationEntities.AddRange(await _manuSFCNodeDestinationRepository.GetEntitiesAsync(sfcIds));

                // 根据条码批量查询条码信息
                var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(sfcIds);
                var sfcInfoDict = sfcInfoEntities.ToDictionary(node => node.SfcId);

                // 根据条码信息批量查询产品信息
                var productEntities = await _procMaterialRepository.GetByIdsAsync(sfcInfoEntities.Select(s => s.ProductId));
                var productDict = productEntities.ToDictionary(node => node.Id);

                // 遍历流转记录
                foreach (var item in manuSfcCirculationList)
                {
                    var beforeNode = nodeEntities.FirstOrDefault(x => x.SiteId == item.SiteId && x.SFC == item.SFC);
                    var beforeSFCEntity = sfcEntities.FirstOrDefault(x => x.SiteId == item.SiteId && x.SFC == item.SFC);

                    var afterNode = nodeEntities.FirstOrDefault(x => x.SiteId == item.SiteId && x.SFC == item.CirculationBarCode);
                    var afterSFCEntity = sfcEntities.FirstOrDefault(x => x.SiteId == item.SiteId && x.SFC == item.CirculationBarCode);

                    // 流转类型为消耗，特殊处理
                    if (item.CirculationType == SfcCirculationTypeEnum.Consume)
                    {
                        // 将beforeNode和afterNode的值互换
                        (afterNode, beforeNode) = (beforeNode, afterNode);
                        (afterSFCEntity, beforeSFCEntity) = (beforeSFCEntity, afterSFCEntity);
                    }

                    if (beforeNode == null)
                    {
                        if (beforeSFCEntity == null) continue;

                        if (!sfcInfoDict.ContainsKey(beforeSFCEntity.Id)) continue;
                        var sfcInfoEntity = sfcInfoDict[beforeSFCEntity.Id];

                        if (!productDict.ContainsKey(sfcInfoEntity.ProductId)) continue;
                        var beforeProductEntity = productDict[sfcInfoEntity.ProductId];

                        beforeNode = new ManuSFCNodeEntity
                        {
                            Id = beforeSFCEntity.Id,
                            SiteId = beforeSFCEntity.SiteId,
                            ProductId = sfcInfoEntity.ProductId,
                            SFC = beforeSFCEntity.SFC,
                            Name = beforeProductEntity.MaterialName,
                            CreatedBy = user,
                            UpdatedBy = user
                        };
                    }

                    if (afterNode == null)
                    {
                        if (afterSFCEntity == null) continue;

                        if (!sfcInfoDict.ContainsKey(afterSFCEntity.Id)) continue;
                        var sfcInfoEntity = sfcInfoDict[afterSFCEntity.Id];

                        if (!productDict.ContainsKey(sfcInfoEntity.ProductId)) continue;
                        var afterProductEntity = productDict[sfcInfoEntity.ProductId];

                        afterNode = new ManuSFCNodeEntity
                        {
                            Id = afterSFCEntity.Id,
                            SiteId = afterSFCEntity.SiteId,
                            ProductId = sfcInfoEntity.ProductId,
                            SFC = afterSFCEntity.SFC,
                            Name = afterProductEntity.MaterialName,
                            CreatedBy = user,
                            UpdatedBy = user
                        };
                    }

                    // 是否解除绑定
                    if (item.IsDisassemble == TrueOrFalseEnum.Yes)
                    {
                        nodeDestinationEntities.RemoveAll(x => x.NodeId == beforeNode.Id && x.DestinationId == afterNode.Id);
                        nodeSourceEntities.RemoveAll(x => x.NodeId == afterNode.Id && x.SourceId == beforeNode.Id);
                    }
                    else
                    {
                        // 将流转记录的条码ID追加到节点的来源集合中
                        nodeSourceEntities.Add(new ManuSFCNodeSourceEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = item.SiteId,
                            CirculationId = item.Id,
                            NodeId = afterNode.Id,
                            SourceId = beforeNode.Id,
                            CreatedBy = user,
                            UpdatedBy = user
                        });

                        // 将流转记录的条码ID追加到节点的去向集合中
                        nodeDestinationEntities.Add(new ManuSFCNodeDestinationEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = item.SiteId,
                            CirculationId = item.Id,
                            NodeId = beforeNode.Id,
                            DestinationId = afterNode.Id,
                            CreatedBy = user,
                            UpdatedBy = user
                        });
                    }

                    if (!nodeEntities.Any(a => a.Id == beforeNode.Id)) nodeEntities.Add(beforeNode);
                    if (!nodeEntities.Any(a => a.Id == afterNode.Id)) nodeEntities.Add(afterNode);
                }

                var rows = 0;
                using var trans = TransactionHelper.GetTransactionScope();

                // 保存节点信息
                rows += await _manuSFCNodeRepository.InsertRangeAsync(nodeEntities);

                // 保存节点的来源信息
                rows += await _manuSFCNodeSourceRepository.InsertRangeAsync(nodeSourceEntities);

                // 保存节点的去向信息
                rows += await _manuSFCNodeDestinationRepository.InsertRangeAsync(nodeDestinationEntities);

                // 更新水位
                var maxUpdateWaterMarkUpdatedOn = manuSfcCirculationList.Max(x => x.UpdatedOn);
                if (maxUpdateWaterMarkUpdatedOn != null)
                {
                    long timestamp = ConvertToUnixTimeMilliseconds(maxUpdateWaterMarkUpdatedOn.Value);
                    rows += await _waterMarkService.RecordWaterMarkAsync(BusinessKey.TracingSourceSFC, timestamp);
                }
                trans.Complete();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // 释放信号量
                _semaphore.Release();
            }
        }

        /// <summary>
        /// 执行统计（达梦）
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task ExecuteForDMAsync(int limitCount = 1000)
        {
            // 等待进入信号量
            await _semaphore.WaitAsync();

            try
            {
                var waterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.TracingSourceSFC);

                // 获取流转表数据（因为这张表的数据会有更新操作，所以不能用常规水位）
                DateTime startWaterMarkTime = ConvertFromUnixTimeMilliseconds(waterMarkId);
                var manuSfcCirculationList = await _manuSfcCirculationRepository.GetListByStartWaterMarkTimeAsync(new EntityByWaterMarkTimeQuery
                {
                    StartWaterMarkTime = startWaterMarkTime,
                    Rows = limitCount
                });
                if (manuSfcCirculationList == null || !manuSfcCirculationList.Any()) return;

                var user = $"{BusinessKey.TracingSourceSFC}作业";

                // 通过站点ID分组
                var manuSfcCirculationSiteIdDict = manuSfcCirculationList.ToLookup(x => x.SiteId).ToDictionary(d => d.Key, d => d);

                List<ManuSFCNodeSourceEntity> addNodeSourceEntities = new();
                List<ManuSFCNodeSourceEntity> removeNodeSourceEntities = new();
                List<ManuSFCNodeDestinationEntity> addNodeDestinationEntities = new();
                List<ManuSFCNodeDestinationEntity> removeNodeDestinationEntities = new();

                List<ManuSFCNodeEntity> nodeEntities = new();
                List<ManuSFCNodeSourceEntity> nodeSourceEntities = new();
                List<ManuSFCNodeDestinationEntity> nodeDestinationEntities = new();
                List<ManuSfcEntity> sfcEntities = new();
                foreach (var item in manuSfcCirculationSiteIdDict)
                {
                    var barCodes = item.Value.Select(s => s.SFC).Union(item.Value.Select(s => s.CirculationBarCode)).Distinct();

                    // 根据流转条码批量查询条码
                    sfcEntities.AddRange(await _manuSfcRepository.GetListAsync(new ManuSfcQuery
                    {
                        SiteId = item.Key,
                        SFCs = barCodes,
                        //Type = SfcTypeEnum.Produce （注意：经过这步之后，仅在库存，而不在条码表的数据会被过滤掉）
                    }));
                }

                // 获取所有条码ID
                var sfcIds = sfcEntities.Select(s => s.Id);

                // 加载数据已经存在的节点信息，不存在的条码在后面会实例一个节点
                nodeEntities.AddRange(await _manuSFCNodeRepository.GetByIdsAsync(sfcIds));

                // 加载已存在的节点的来源信息
                nodeSourceEntities.AddRange(await _manuSFCNodeSourceRepository.GetEntitiesAsync(sfcIds));

                // 加载已存在的节点的去向信息
                nodeDestinationEntities.AddRange(await _manuSFCNodeDestinationRepository.GetEntitiesAsync(sfcIds));

                // 根据条码批量查询条码信息
                var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(sfcIds);
                var sfcInfoDict = sfcInfoEntities.ToDictionary(node => node.SfcId);

                // 根据条码信息批量查询产品信息
                var productEntities = await _procMaterialRepository.GetByIdsAsync(sfcInfoEntities.Select(s => s.ProductId));
                var productDict = productEntities.ToDictionary(node => node.Id);

                // 遍历流转记录
                foreach (var item in manuSfcCirculationList)
                {
                    var beforeNode = nodeEntities.FirstOrDefault(x => x.SiteId == item.SiteId && x.SFC == item.SFC);
                    var beforeSFCEntity = sfcEntities.FirstOrDefault(x => x.SiteId == item.SiteId && x.SFC == item.SFC);

                    var afterNode = nodeEntities.FirstOrDefault(x => x.SiteId == item.SiteId && x.SFC == item.CirculationBarCode);
                    var afterSFCEntity = sfcEntities.FirstOrDefault(x => x.SiteId == item.SiteId && x.SFC == item.CirculationBarCode);

                    // 流转类型为消耗，特殊处理
                    if (item.CirculationType == SfcCirculationTypeEnum.Consume)
                    {
                        // 将beforeNode和afterNode的值互换
                        (afterNode, beforeNode) = (beforeNode, afterNode);
                        (afterSFCEntity, beforeSFCEntity) = (beforeSFCEntity, afterSFCEntity);
                    }

                    if (beforeNode == null)
                    {
                        if (beforeSFCEntity == null) continue;

                        if (!sfcInfoDict.ContainsKey(beforeSFCEntity.Id)) continue;
                        var sfcInfoEntity = sfcInfoDict[beforeSFCEntity.Id];

                        if (!productDict.ContainsKey(sfcInfoEntity.ProductId)) continue;
                        var beforeProductEntity = productDict[sfcInfoEntity.ProductId];

                        beforeNode = new ManuSFCNodeEntity
                        {
                            Id = beforeSFCEntity.Id,
                            SiteId = beforeSFCEntity.SiteId,
                            ProductId = sfcInfoEntity.ProductId,
                            SFC = beforeSFCEntity.SFC,
                            Name = beforeProductEntity.MaterialName,
                            CreatedBy = user,
                            UpdatedBy = user
                        };
                    }

                    if (afterNode == null)
                    {
                        if (afterSFCEntity == null) continue;

                        if (!sfcInfoDict.ContainsKey(afterSFCEntity.Id)) continue;
                        var sfcInfoEntity = sfcInfoDict[afterSFCEntity.Id];

                        if (!productDict.ContainsKey(sfcInfoEntity.ProductId)) continue;
                        var afterProductEntity = productDict[sfcInfoEntity.ProductId];

                        afterNode = new ManuSFCNodeEntity
                        {
                            Id = afterSFCEntity.Id,
                            SiteId = afterSFCEntity.SiteId,
                            ProductId = sfcInfoEntity.ProductId,
                            SFC = afterSFCEntity.SFC,
                            Name = afterProductEntity.MaterialName,
                            CreatedBy = user,
                            UpdatedBy = user
                        };
                    }

                    // 是否解除绑定
                    if (item.IsDisassemble == TrueOrFalseEnum.Yes)
                    {
                        removeNodeSourceEntities.AddRange(nodeSourceEntities.Where(x => x.NodeId == afterNode.Id && x.SourceId == beforeNode.Id));
                        removeNodeDestinationEntities.AddRange(nodeDestinationEntities.Where(x => x.NodeId == beforeNode.Id && x.DestinationId == afterNode.Id));
                        //nodeDestinationEntities.RemoveAll(x => x.NodeId == beforeNode.Id && x.DestinationId == afterNode.Id);
                        //nodeSourceEntities.RemoveAll(x => x.NodeId == afterNode.Id && x.SourceId == beforeNode.Id);
                    }
                    else
                    {
                        if (!nodeSourceEntities.Any(a => a.NodeId == afterNode.Id && a.SourceId == beforeNode.Id))
                        {
                            // 将流转记录的条码ID追加到节点的来源集合中
                            addNodeSourceEntities.Add(new ManuSFCNodeSourceEntity
                            {
                                Id = IdGenProvider.Instance.CreateId(),
                                SiteId = item.SiteId,
                                CirculationId = item.Id,
                                NodeId = afterNode.Id,
                                SourceId = beforeNode.Id,
                                CreatedBy = user,
                                UpdatedBy = user
                            });
                        }

                        if (!nodeDestinationEntities.Any(a => a.NodeId == beforeNode.Id && a.DestinationId == afterNode.Id))
                        {
                            // 将流转记录的条码ID追加到节点的去向集合中
                            addNodeDestinationEntities.Add(new ManuSFCNodeDestinationEntity
                            {
                                Id = IdGenProvider.Instance.CreateId(),
                                SiteId = item.SiteId,
                                CirculationId = item.Id,
                                NodeId = beforeNode.Id,
                                DestinationId = afterNode.Id,
                                CreatedBy = user,
                                UpdatedBy = user
                            });
                        }
                    }

                    if (!nodeEntities.Any(a => a.Id == beforeNode.Id)) nodeEntities.Add(beforeNode);
                    if (!nodeEntities.Any(a => a.Id == afterNode.Id)) nodeEntities.Add(afterNode);
                }

                var rows = 0;
                using var trans = TransactionHelper.GetTransactionScope(timeout: 180);

                // 保存节点信息
                rows += await _manuSFCNodeRepository.DeleteAsync(nodeEntities.Select(x => x.Id));
                rows += await _manuSFCNodeRepository.InsertRangeAsync(nodeEntities);

                // 保存节点的来源信息
                rows += await _manuSFCNodeSourceRepository.DeleteAsync(removeNodeSourceEntities);
                rows += await _manuSFCNodeSourceRepository.InsertRangeAsync(addNodeSourceEntities);

                // 保存节点的去向信息
                rows += await _manuSFCNodeDestinationRepository.DeleteAsync(removeNodeDestinationEntities);
                rows += await _manuSFCNodeDestinationRepository.InsertRangeAsync(addNodeDestinationEntities);

                // 更新水位
                var maxUpdateWaterMarkUpdatedOn = manuSfcCirculationList.Max(x => x.UpdatedOn);
                if (maxUpdateWaterMarkUpdatedOn.HasValue)
                {
                    long timestamp = ConvertToUnixTimeMilliseconds(maxUpdateWaterMarkUpdatedOn.Value);
                    rows += await _waterMarkService.RecordWaterMarkAsync(BusinessKey.TracingSourceSFC, timestamp);
                }

                trans.Complete();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // 释放信号量
                _semaphore.Release();
            }
        }

        /// <summary>
        /// 将DateTime转换为Unix时间戳（毫秒）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private static long ConvertToUnixTimeMilliseconds(DateTime dateTime)
        {
            // 转换为东八区时区的时间
            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"); // 示例时区
            DateTime nowInTargetTimeZone = TimeZoneInfo.ConvertTimeFromUtc(dateTime, targetTimeZone);

            // 将DateTime转换为DateTimeOffset，并计算时间戳
            DateTimeOffset nowOffset = new(nowInTargetTimeZone);
            long timestamp = nowOffset.ToUnixTimeMilliseconds();

            return timestamp;
        }

        /// <summary>
        /// 将Unix时间戳（毫秒）转换为DateTime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        private static DateTime ConvertFromUnixTimeMilliseconds(long timestamp)
        {
            // 将时间戳转换为DateTimeOffset
            DateTimeOffset nowOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);

            // 转换为东八区时区的时间
            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"); // 示例时区
            DateTime nowInTargetTimeZone = TimeZoneInfo.ConvertTime(nowOffset.DateTime, targetTimeZone);

            return nowInTargetTimeZone;
        }

    }
}
