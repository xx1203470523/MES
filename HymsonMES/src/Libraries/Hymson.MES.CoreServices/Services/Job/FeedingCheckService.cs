using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common;

using Hymson.MES.Data.Repositories.Manufacture;

using Hymson.MES.Data.Repositories.Process;


namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 上料完整性校验JOB
    /// </summary>
    [Job("上料完整性校验JOB", JobTypeEnum.Standard)]
    public class FeedingCheckService : IJobService
    {
        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;
        /// <summary>
        /// 仓储接口（上料信息）
        /// </summary>
        private readonly IManuFeedingRepository _manuFeedingRepository;
        /// <summary>
        ///  仓储（上料点关联资源）
        /// </summary>
        private readonly IProcLoadPointLinkResourceRepository _procLoadPointLinkResourceRepository;

        public FeedingCheckService(IMasterDataService masterDataService
            , IProcLoadPointLinkResourceRepository procLoadPointLinkResourceRepository
            ,IManuFeedingRepository manuFeedingRepository)
        {
            _masterDataService = masterDataService;
            _manuFeedingRepository = manuFeedingRepository;
            _procLoadPointLinkResourceRepository = procLoadPointLinkResourceRepository;
        }
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return null;
        }

        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return new List<JobBo>();
        }

        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            return null;
        }

        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            return null;
        }

        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return ;
            if (commonBo == null) return ;
            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.InStationRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', multiSFCBo.SFCs));
            }
            // 全部物料加载数据
            List<ManuFeedingEntity> allFeedingEntities = new();

            // 批量读取物料加载数据（需要实时数据，勿缓存）
            var resourceFeeds = await commonBo.Proxy.GetValueAsync(_manuFeedingRepository.GetByResourceIdAndMaterialIdsWithOutZeroAsync, new GetByResourceIdAndMaterialIdsQuery
            {
                ResourceId = commonBo.ResourceId
            });
            if (resourceFeeds != null) allFeedingEntities.AddRange(resourceFeeds);
            //获取上料点物料
            var loadPoints = await _procLoadPointLinkResourceRepository.GetByResourceIdAsync(commonBo.ResourceId);
            var pointFeeds = await _manuFeedingRepository.GetByFeedingPointIdWithOutZeroAsync(new GetByFeedingPointIdsQuery
            {
                FeedingPointIds = loadPoints.Select(s => s.LoadPointId)
            });
            if (pointFeeds != null) allFeedingEntities = allFeedingEntities.UnionBy(pointFeeds, s => s.Id).ToList();
            // 组合物料数据（放缓存）
            var initialMaterials = await commonBo.Proxy.GetValueAsync(_masterDataService.GetInitialMaterialsAsync, new MaterialDeductRequestBo
            {
                SiteId = commonBo.SiteId,
                ProcedureId = commonBo.ProcedureId,
                ProductBOMId = sfcProduceEntities.First().ProductBOMId
            });
            // 是否有未上料的BOM的物料
            var notIncludeMaterialIds = initialMaterials.Select(m=>m.MaterialId).Except(allFeedingEntities.Select(s => s.MaterialId));
            if (notIncludeMaterialIds.Any())
            {
                var codes = initialMaterials.Where(i=>notIncludeMaterialIds.Contains(i.MaterialId)).Select(i=>i.MaterialCode);
                throw new CustomerValidationException(nameof(ErrorCode.MES15509)).WithData("code", string.Join(',', codes));
            }
        }
    }
}
