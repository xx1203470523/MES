using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Sequences;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture.ManuFeeding
{
    /// <summary>
    /// 服务（容器维护）
    /// </summary>
    public class ManuFeedingService : IManuFeedingService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        ///  仓储（资源）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        ///  仓储（工作中心资源关联）
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        ///  仓储（上料点物料关联）
        /// </summary>
        private readonly IProcLoadPointLinkMaterialRepository _procLoadPointLinkMaterialRepository;

        /// <summary>
        ///  仓储（工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        ///  仓储（Bom明细）
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        ///  仓储（物料）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="sequenceService"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="procLoadPointLinkMaterialRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procMaterialRepository"></param>
        public ManuFeedingService(ICurrentUser currentUser, ICurrentSite currentSite, ISequenceService sequenceService,
            IProcResourceRepository procResourceRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IProcLoadPointLinkMaterialRepository procLoadPointLinkMaterialRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcMaterialRepository procMaterialRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procResourceRepository = procResourceRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procLoadPointLinkMaterialRepository = procLoadPointLinkMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procMaterialRepository = procMaterialRepository;
        }


        /// <summary>
        /// 查询资源（物料加载）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingResourceDto>> GetFeedingResourceListAsync(ManuFeedingResourceQueryDto queryDto)
        {
            List<ProcResourceEntity> resources = new();
            switch (queryDto.Source)
            {
                case FeedingSourceEnum.Equipment:
                    resources.AddRange(await _procResourceRepository.GetByEquipmentCodeAsync(queryDto.Code));
                    break;
                default:
                case FeedingSourceEnum.Resource:
                    resources.AddRange(await _procResourceRepository.GetByResourceCodeAsync(queryDto.Code));
                    break;
            }

            return resources.Select(s => new ManuFeedingResourceDto { ResourceId = s.Id, ResourceCode = s.ResCode });
        }

        /// <summary>
        /// 查询物料（物料加载）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingMaterialDto>> GetFeedingMaterialListAsync(ManuFeedingMaterialQueryDto queryDto)
        {
            List<ManuFeedingMaterialDto> list = new();
            IEnumerable<long>? materialIds = null;

            // 读取资源绑定的产线
            var workCenter = await _inteWorkCenterRepository.GetByResourceIdAsync(queryDto.ResourceId);
            if (workCenter == null)
            {
                materialIds = await GetMaterialIdsByResourceIdAsync(queryDto.ResourceId);
            }
            // 混线
            else if (workCenter.IsMixLine == true)
            {
                materialIds = await GetMaterialIdsByResourceIdAsync(queryDto.ResourceId);
            }
            // 不混线
            else if (workCenter.IsMixLine == false)
            {
                materialIds = await GetMaterialIdsByWorkCenterIdAsync(workCenter.Id);
            }

            // 查询不到物料
            if (materialIds == null) return list;

            // 通过物料ID获取物料集合
            var materials = await _procMaterialRepository.GetByIdsAsync(materialIds.ToArray());
            foreach (var item in materials)
            {
                // TODO
            }

            // TODO
            for (int i = 1; i < 4; i++)
            {
                var item = new ManuFeedingMaterialDto
                {
                    MaterialId = i,
                    MaterialCode = $"MaterialCode-{i}",
                    MaterialName = $"{queryDto.ResourceId}",
                    Version = $"v-{i}",
                    Children = new List<ManuFeedingMaterialItemDto>()
                };

                for (int j = 1; j < 4; j++)
                {
                    item.Children.Add(new ManuFeedingMaterialItemDto
                    {
                        MaterialId = i,
                        BarCode = $"BarCode-{j}",
                        InitQty = new Random().Next(50, 100),
                        Qty = new Random().Next(0, 20),
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now()
                    });
                }

                list.Add(item);
            }

            return list;
        }


        /*
        /// <summary>
        /// 添加（容器维护）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(InteContainerSaveDto createDto)
        {
            //验证DTO
            //await _validationCreateRules.ValidateAndThrowAsync(createDto);

            // DTO转换实体
            var entity = createDto.ToEntity<InteContainerEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存实体
            return await _inteContainerRepository.InsertAsync(entity); ;
        }

        /// <summary>
        /// 更新（容器维护）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(InteContainerSaveDto modifyDto)
        {
            var entity = modifyDto.ToEntity<InteContainerEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            // 更新实体
            return await _inteContainerRepository.UpdateAsync(entity);
        }
        */





        #region 内部方法
        /// <summary>
        /// 通过资源ID获取物料ID集合
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<long>?> GetMaterialIdsByResourceIdAsync(long resourceId)
        {
            var linkMaterials = await _procLoadPointLinkMaterialRepository.GetByResourceIdAsync(resourceId);
            if (linkMaterials == null) return null;

            return linkMaterials.Select(s => s.MaterialId);
        }

        /// <summary>
        /// 通过工作中心ID获取物料ID集合
        /// </summary>
        /// <param name="workCenterId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<long>?> GetMaterialIdsByWorkCenterIdAsync(long workCenterId)
        {
            // 通过车间查询工单
            var workOrdersOfFarm = await _planWorkOrderRepository.GetByWorkFarmIdAsync(workCenterId);

            // 通过产线查询工单
            var workOrdersOfLine = await _planWorkOrderRepository.GetByWorkFarmIdAsync(workCenterId);

            // 合并结果
            var workOrders = workOrdersOfFarm.Union(workOrdersOfLine);
            if (workOrders == null) return null;

            // 通过工单查询BOM
            var bomIds = workOrders.Select(s => s.ProductBOMId);
            var materials = await _procBomDetailRepository.GetByBomIdsAsync(bomIds);
            if (materials == null) return null;

            return materials.Select(s => s.MaterialId);
        }

        #endregion

    }
}
