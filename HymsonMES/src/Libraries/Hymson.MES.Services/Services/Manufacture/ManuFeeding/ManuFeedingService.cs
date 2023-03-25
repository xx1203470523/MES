using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Sequences;
using Hymson.Snowflake;
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
        ///  仓储（物料加载）
        /// </summary>
        private readonly IManuFeedingRepository _manuFeedingRepository;


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
        /// <param name="manuFeedingRepository"></param>
        public ManuFeedingService(ICurrentUser currentUser, ICurrentSite currentSite, ISequenceService sequenceService,
            IProcResourceRepository procResourceRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IProcLoadPointLinkMaterialRepository procLoadPointLinkMaterialRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcMaterialRepository procMaterialRepository,
            IManuFeedingRepository manuFeedingRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procResourceRepository = procResourceRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procLoadPointLinkMaterialRepository = procLoadPointLinkMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _manuFeedingRepository = manuFeedingRepository;
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
            if (workCenter == null) return list;

            /*
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
            else
            {
                return list;
            }
            */

            // 通过产线->工单->BOM->查询物料
            materialIds = await GetMaterialIdsByWorkCenterIdAsync(workCenter.Id);

            // 查询不到物料
            if (materialIds == null || materialIds.Any() == false) return list;

            // 通过物料ID获取物料集合
            var materials = await _procMaterialRepository.GetByIdsAsync(materialIds.ToArray());

            // 通过物料ID获取物料库存信息
            var manuFeedings = await _manuFeedingRepository.GetByResourceIdAndMaterialIdsAsync(new ManuFeedingQuery
            {
                ResourceId = queryDto.ResourceId,
                MaterialIds = materialIds,
            });

            // 通过物料分组
            var manuFeedingsDictionary = manuFeedings.ToLookup(w => w.ProductId).ToDictionary(d => d.Key, d => d);

            // 填充返回集合
            foreach (var item in materials)
            {
                var material = new ManuFeedingMaterialDto
                {
                    MaterialId = item.Id,
                    MaterialCode = item.MaterialCode,
                    MaterialName = item.MaterialName,
                    Version = item.Version ?? "-",
                    Children = new List<ManuFeedingMaterialItemDto>()
                };

                if (manuFeedingsDictionary.TryGetValue(material.MaterialId, out var feedingEntities) == true)
                {
                    material.Children.AddRange(feedingEntities.Select(s => new ManuFeedingMaterialItemDto
                    {
                        Id = s.Id,
                        MaterialId = s.ProductId,
                        BarCode = s.BarCode,
                        InitQty = s.InitQty,
                        Qty = s.Qty,
                        UpdatedBy = s.UpdatedBy ?? "-",
                        UpdatedOn = s.UpdatedOn ?? HymsonClock.Now()
                    }));
                }

                list.Add(material);
            }

            return list;
        }


        /// <summary>
        /// 添加（物料加载）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ManuFeedingMaterialSaveDto saveDto)
        {
            //验证DTO
            //await _validationCreateRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuFeedingEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存实体
            return await _manuFeedingRepository.InsertAsync(entity); ;
        }

        /// <summary>
        /// 删除（物料加载）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            return await _manuFeedingRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }





        #region 内部方法
        /// <summary>
        /// 通过资源ID关联上料点获取物料ID集合
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
