using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;

namespace Hymson.MES.CoreServices.Services
{
    /// <summary>
    /// 条码追溯服务
    /// </summary>
    public class TracingSourceCoreService : ITracingSourceCoreService
    {
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
        /// <param name="currentEquipment"></param>
        /// <param name="manuSFCNodeRepository"></param>
        /// <param name="manuSFCNodeSourceRepository"></param>
        /// <param name="manuSFCNodeDestinationRepository"></param>
        public TracingSourceCoreService(IManuSFCNodeRepository manuSFCNodeRepository,
            IManuSFCNodeSourceRepository manuSFCNodeSourceRepository,
            IManuSFCNodeDestinationRepository manuSFCNodeDestinationRepository)
        {
            _manuSFCNodeRepository = manuSFCNodeRepository;
            _manuSFCNodeSourceRepository = manuSFCNodeSourceRepository;
            _manuSFCNodeDestinationRepository = manuSFCNodeDestinationRepository;
        }
        /// <summary>
        /// 条码追溯（反向）  原始数据 平铺数据 没经过加工的树
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSFCNodeEntity>> OriginalSourceAsync(EntityBySFCQuery query)
        {
            var rootNodeEntity = await _manuSFCNodeRepository.GetBySFCAsync(query)
              ?? throw new CustomerValidationException(nameof(ErrorCode.MES12802)).WithData("sfc", query.SFC);

            // 取得该根节点下面的所有树节点
            var sourceEntities = await _manuSFCNodeSourceRepository.GetTreeEntitiesAsync(rootNodeEntity.Id);
            //var sourceDict = sourceEntities.ToLookup(x => x.NodeId).ToDictionary(d => d.Key, d => d);

            // 取得整个树的基础信息方便下文填充数据
            var nodeIds = sourceEntities.Select(s => s.NodeId).Union(sourceEntities.Select(s => s.SourceId)).Distinct();
            var nodeEntities = await _manuSFCNodeRepository.GetByIdsAsync(nodeIds);
            return nodeEntities;
        }
        /// <summary>
        /// 条码追溯（反向）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<NodeSourceBo> SourceAsync(EntityBySFCQuery query)
        {
            var rootNodeEntity = await _manuSFCNodeRepository.GetBySFCAsync(query)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES12802)).WithData("sfc", query.SFC);

            // 取得该根节点下面的所有树节点
            var sourceEntities = await _manuSFCNodeSourceRepository.GetTreeEntitiesAsync(rootNodeEntity.Id);
            //var sourceDict = sourceEntities.ToLookup(x => x.NodeId).ToDictionary(d => d.Key, d => d);

            // 取得整个树的基础信息方便下文填充数据
            var nodeIds = sourceEntities.Select(s => s.NodeId).Union(sourceEntities.Select(s => s.SourceId)).Distinct();
            var nodeEntities = await _manuSFCNodeRepository.GetByIdsAsync(nodeIds);
            var nodeDict = nodeEntities.ToDictionary(node => node.Id);

            // 初始化根节点Bo对象
            var rootNode = new NodeSourceBo
            {
                Id = rootNodeEntity.Id,
                SiteId = rootNodeEntity.SiteId,
                ProductId = rootNodeEntity.ProductId,
                SFC = rootNodeEntity.SFC,
                Name = rootNodeEntity.Name,
                Location = rootNodeEntity.Location
            };

            var filledNodes = FillChildrenNodes(rootNode, sourceEntities, nodeDict);
            if (filledNodes == null) return rootNode;

            return filledNodes;
        }

        /// <summary>
        /// 条码追溯（正向）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<NodeSourceBo> DestinationAsync(EntityBySFCQuery query)
        {
            var rootNodeEntity = await _manuSFCNodeRepository.GetBySFCAsync(query)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES12802)).WithData("sfc", query.SFC);

            // 取得该根节点下面的所有树节点
            var destinationEntities = await _manuSFCNodeDestinationRepository.GetTreeEntitiesAsync(rootNodeEntity.Id);
            //var sourceDict = sourceEntities.ToLookup(x => x.NodeId).ToDictionary(d => d.Key, d => d);

            // 取得整个树的基础信息方便下文填充数据
            var nodeIds = destinationEntities.Select(s => s.NodeId).Union(destinationEntities.Select(s => s.DestinationId)).Distinct();
            var nodeEntities = await _manuSFCNodeRepository.GetByIdsAsync(nodeIds);
            var nodeDict = nodeEntities.ToDictionary(node => node.Id);

            // 初始化根节点Bo对象
            var rootNode = new NodeSourceBo
            {
                Id = rootNodeEntity.Id,
                SiteId = rootNodeEntity.SiteId,
                ProductId = rootNodeEntity.ProductId,
                SFC = rootNodeEntity.SFC,
                Name = rootNodeEntity.Name,
                Location = rootNodeEntity.Location
            };

            var filledNodes = FillChildrenNodes(rootNode, destinationEntities, nodeDict);
            if (filledNodes == null) return rootNode;

            return filledNodes;
        }

        /// <summary>
        /// 填充来源
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="sourceEntities"></param>
        /// <param name="nodeDict"></param>
        /// <returns></returns>
        public NodeSourceBo? FillChildrenNodes(NodeSourceBo? currentNode, IEnumerable<ManuSFCNodeSourceEntity> sourceEntities, Dictionary<long, ManuSFCNodeEntity> nodeDict)
        {
            if (currentNode == null) return currentNode;
            currentNode.Label = $"{currentNode.SFC} - {currentNode.Name}";
            if (!string.IsNullOrWhiteSpace(currentNode.Location)) currentNode.Label = $" - {currentNode.Location}";

            if (sourceEntities == null || !sourceEntities.Any()) return currentNode;

            var childrenSourceEntities = sourceEntities.Where(x => x.NodeId == currentNode.Id);
            if (childrenSourceEntities == null || !childrenSourceEntities.Any()) return currentNode;

            // 填充node的Children
            foreach (var item in childrenSourceEntities)
            {
                if (!nodeDict.ContainsKey(item.SourceId)) continue;

                var nodeEntity = nodeDict[item.SourceId];
                var nodeBo = new NodeSourceBo
                {
                    Id = nodeEntity.Id,
                    SiteId = nodeEntity.SiteId,
                    ProductId = nodeEntity.ProductId,
                    SFC = nodeEntity.SFC,
                    Name = nodeEntity.Name,
                    Location = nodeEntity.Location
                };

                nodeBo = FillChildrenNodes(nodeBo, sourceEntities, nodeDict) ?? nodeBo;
                currentNode.Children.Add(nodeBo);
            }
            return currentNode;
        }

        /// <summary>
        /// 填充去向
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="destinationEntities"></param>
        /// <param name="nodeDict"></param>
        /// <returns></returns>
        public NodeSourceBo? FillChildrenNodes(NodeSourceBo? currentNode, IEnumerable<ManuSFCNodeDestinationEntity> destinationEntities, Dictionary<long, ManuSFCNodeEntity> nodeDict)
        {
            if (currentNode == null) return currentNode;
            currentNode.Label = $"{currentNode.SFC} - {currentNode.Name}";
            if (!string.IsNullOrWhiteSpace(currentNode.Location)) currentNode.Label = $" - {currentNode.Location}";

            if (destinationEntities == null || !destinationEntities.Any()) return currentNode;

            var childrenDestinationEntities = destinationEntities.Where(x => x.NodeId == currentNode.Id);
            if (childrenDestinationEntities == null || !childrenDestinationEntities.Any()) return currentNode;

            // 填充node的Children
            foreach (var item in childrenDestinationEntities)
            {
                if (!nodeDict.ContainsKey(item.DestinationId)) continue;

                var nodeEntity = nodeDict[item.DestinationId];
                var nodeBo = new NodeSourceBo
                {
                    Id = nodeEntity.Id,
                    SiteId = nodeEntity.SiteId,
                    ProductId = nodeEntity.ProductId,
                    SFC = nodeEntity.SFC,
                    Name = nodeEntity.Name,
                    Location = nodeEntity.Location
                };

                nodeBo = FillChildrenNodes(nodeBo, destinationEntities, nodeDict) ?? nodeBo;
                currentNode.Children.Add(nodeBo);
            }
            return currentNode;
        }

    }
}