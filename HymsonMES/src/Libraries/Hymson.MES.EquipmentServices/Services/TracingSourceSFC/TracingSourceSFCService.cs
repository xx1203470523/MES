using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services
{
    /// <summary>
    /// 条码追溯服务
    /// </summary>
    public class TracingSourceSFCService : ITracingSourceSFCService
    {
        /// <summary>
        /// 当前设备对象
        /// </summary>
        private readonly ICurrentEquipment _currentEquipment;

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
        public TracingSourceSFCService(ICurrentEquipment currentEquipment,
            IManuSFCNodeRepository manuSFCNodeRepository,
            IManuSFCNodeSourceRepository manuSFCNodeSourceRepository,
            IManuSFCNodeDestinationRepository manuSFCNodeDestinationRepository)
        {
            _currentEquipment = currentEquipment;
            _manuSFCNodeRepository = manuSFCNodeRepository;
            _manuSFCNodeSourceRepository = manuSFCNodeSourceRepository;
            _manuSFCNodeDestinationRepository = manuSFCNodeDestinationRepository;
        }


        /// <summary>
        /// 条码追溯（反向）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<NodeSourceBo> SourceAsync(string sfc)
        {
            var rootNode = await _manuSFCNodeRepository.GetBySFCAsync(new EntityBySFCQuery
            {
                SiteId = _currentEquipment.SiteId,
                SFC = sfc
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES12802)).WithData("sfc", sfc);

            // 初始化根节点Bo对象
            var responseBo = new NodeSourceBo
            {
                Id = rootNode.Id,
                SiteId = rootNode.SiteId,
                ProductId = rootNode.ProductId,
                SFC = rootNode.SFC,
                Name = rootNode.Name,
                Location = rootNode.Location
            };

            // 取得该根节点下面的所有树节点
            var sourceEntities = await _manuSFCNodeSourceRepository.GetTreeEntitiesAsync(rootNode.Id);
            var sourceDict = sourceEntities.ToDictionary(source => source.NodeId);
            //var sourceDict = sourceEntities.ToLookup(x => x.NodeId).ToDictionary(d => d.Key, d => d);

            // 取得整个树的基础信息方便下文填充数据
            var nodeEntities = await _manuSFCNodeRepository.GetByIdsAsync(sourceEntities.Select(s => s.NodeId).Union(sourceEntities.Select(s => s.SourceId)).Distinct());
            var nodeDict = nodeEntities.ToDictionary(node => node.Id);



            return responseBo;
        }

        /// <summary>
        /// 条码追溯（正向）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<NodeSourceBo> DestinationAsync(string sfc)
        {
            var rootNode = await _manuSFCNodeRepository.GetBySFCAsync(new EntityBySFCQuery
            {
                SiteId = _currentEquipment.SiteId,
                SFC = sfc
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES12802)).WithData("sfc", sfc);

            // 初始化根节点Bo对象
            var responseBo = new NodeSourceBo
            {
                Id = rootNode.Id,
                SiteId = rootNode.SiteId,
                ProductId = rootNode.ProductId,
                SFC = rootNode.SFC,
                Name = rootNode.Name,
                Location = rootNode.Location
            };

            return responseBo;
        }

    }

}