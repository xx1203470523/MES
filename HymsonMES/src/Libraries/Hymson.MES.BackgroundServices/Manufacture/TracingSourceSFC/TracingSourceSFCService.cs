using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
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
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.TracingSourceSFC);

            // 获取流转表数据
            var manuSfcCirculationList = await _manuSfcCirculationRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            {
                StartWaterMarkId = waterMarkId,
                Rows = limitCount
            });
            if (manuSfcCirculationList == null || !manuSfcCirculationList.Any()) return;

            var user = $"{BusinessKey.TracingSourceSFC}作业";

            // 通过站点ID分组
            var manuSfcCirculationSiteIdDic = manuSfcCirculationList.ToLookup(x => x.SiteId).ToDictionary(d => d.Key, d => d);

            List<ManuSFCNodeEntity> nodeEntities = new();
            List<ManuSFCNodeSourceEntity> nodeSourceEntities = new();
            List<ManuSFCNodeDestinationEntity> nodeDestinationEntities = new();
            List<ManuSfcEntity> sfcEntities = new();
            foreach (var item in manuSfcCirculationSiteIdDic)
            {
                var barCodes = item.Value.Select(s => s.SFC).Union(item.Value.Select(s => s.CirculationBarCode)).Distinct();

                // 根据流转条码批量查询条码
                sfcEntities.AddRange(await _manuSfcRepository.GetBySFCsAsync(new EntityBySFCsQuery
                {
                    SiteId = item.Key,
                    SFCs = barCodes
                }));
            }

            // 获取所有条码ID
            var sfcIds = sfcEntities.Select(s => s.Id);

            // 加载数据已经存在的节点信息，不存在的条码在后面会实例一个节点
            nodeEntities.AddRange(await _manuSFCNodeRepository.GetByIdsAsync(sfcIds));

            // 根据条码批量查询条码信息
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcIds);

            // 根据条码信息批量查询产品信息
            var productEntities = await _procMaterialRepository.GetByIdsAsync(sfcInfoEntities.Select(s => s.ProductId));

            // 遍历流转记录
            foreach (var item in manuSfcCirculationList)
            {
                var beforeNode = nodeEntities.FirstOrDefault(x => x.SiteId == item.SiteId && x.SFC == item.SFC);
                var afterNode = nodeEntities.FirstOrDefault(x => x.SiteId == item.SiteId && x.SFC == item.CirculationBarCode);

                if (beforeNode == null)
                {
                    var sfcEntity = sfcEntities.FirstOrDefault(x => x.SiteId == item.SiteId && x.SFC == item.SFC);
                    if (sfcEntity == null) continue;

                    var sfcInfoEntity = sfcInfoEntities.FirstOrDefault(x => x.SfcId == sfcEntity.Id);
                    if (sfcInfoEntity == null) continue;

                    beforeNode = new ManuSFCNodeEntity { Id = sfcEntity.Id, SiteId = sfcEntity.SiteId, ProductId = sfcInfoEntity.ProductId, SFC = sfcEntity.SFC };
                }

                if (afterNode == null)
                {
                    var sfcEntity = sfcEntities.FirstOrDefault(x => x.SiteId == item.SiteId && x.SFC == item.SFC);
                    if (sfcEntity == null) continue;

                    var sfcInfoEntity = sfcInfoEntities.FirstOrDefault(x => x.SfcId == sfcEntity.Id);
                    if (sfcInfoEntity == null) continue;

                    afterNode = new ManuSFCNodeEntity { Id = sfcEntity.Id, SiteId = sfcEntity.SiteId, ProductId = sfcInfoEntity.ProductId, SFC = sfcEntity.SFC };
                }

                // 这个放外面是为了产品的名字有变动时，会随着节点更新同步
                var beforeProductEntity = productEntities.FirstOrDefault(x => x.Id == beforeNode.ProductId);
                var afterProductEntity = productEntities.FirstOrDefault(x => x.Id == afterNode.ProductId);
                if (beforeProductEntity != null) beforeNode.Name = beforeProductEntity.MaterialName;
                if (afterProductEntity != null) afterNode.Name = afterProductEntity.MaterialName;

                // 将流转记录的条码ID追加到节点的去向集合中
                nodeDestinationEntities.Add(new ManuSFCNodeDestinationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = item.SiteId,
                    NodeId = beforeNode.Id,
                    DestinationId = afterNode.Id,
                    CreatedBy = user,
                    UpdatedBy = user
                });

                // 将流转记录的条码ID追加到节点的来源集合中
                nodeSourceEntities.Add(new ManuSFCNodeSourceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = item.SiteId,
                    NodeId = afterNode.Id,
                    SourceId = beforeNode.Id,
                    CreatedBy = user,
                    UpdatedBy = user
                });

                if (!nodeEntities.Any(a => a.Id == beforeNode.Id)) nodeEntities.Add(beforeNode);
                if (!nodeEntities.Any(a => a.Id == beforeNode.Id)) nodeEntities.Add(afterNode);
            }

            
            using var trans = TransactionHelper.GetTransactionScope();

            // 保存节点信息
            await _manuSFCNodeRepository.InsertRangeAsync(nodeEntities);

            // 保存节点的来源信息
            await _manuSFCNodeSourceRepository.InsertRangeAsync(nodeSourceEntities);

            // 保存节点的去向信息
            await _manuSFCNodeDestinationRepository.InsertRangeAsync(nodeDestinationEntities);

            // 更新水位
            await _waterMarkService.RecordWaterMarkAsync(BusinessKey.TracingSourceSFC, manuSfcCirculationList.Max(x => x.Id));
            trans.Complete();
           

        }

    }
}
