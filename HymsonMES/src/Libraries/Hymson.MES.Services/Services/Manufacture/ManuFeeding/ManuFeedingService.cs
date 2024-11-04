using Dapper;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Manufacture.ManuFeeding
{
    /// <summary>
    /// 服务（物料加载）
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
        ///  仓储（工作中心资源关联）
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        ///  仓储（资源）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        ///  仓储（上料点）
        /// </summary>
        private readonly IProcLoadPointRepository _procLoadPointRepository;

        /// <summary>
        ///  仓储（上料点物料关联）
        /// </summary>
        private readonly IProcLoadPointLinkMaterialRepository _procLoadPointLinkMaterialRepository;

        /// <summary>
        ///  仓储（Bom明细）
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        ///  仓储（Bom替代料）
        /// </summary>
        private readonly IProcBomDetailReplaceMaterialRepository _procBomDetailReplaceMaterialRepository;

        /// <summary>
        ///  仓储（物料）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        ///  仓储（工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        ///  仓储（工单激活）
        /// </summary>
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;

        /// <summary>
        ///  仓储（物料加载）
        /// </summary>
        private readonly IManuFeedingRepository _manuFeedingRepository;

        /// <summary>
        ///  仓储（物料加载记录）
        /// </summary>
        private readonly IManuFeedingRecordRepository _manuFeedingRecordRepository;

        /// <summary>
        ///  仓储（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        ///  仓储（物料台账）
        /// </summary>
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;


        /// <summary>
        /// 构造函数（物料加载）
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procLoadPointRepository"></param>
        /// <param name="procLoadPointLinkMaterialRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procBomDetailReplaceMaterialRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="planWorkOrderActivationRepository"></param>
        /// <param name="manuFeedingRepository"></param>
        /// <param name="manuFeedingRecordRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        public ManuFeedingService(ICurrentUser currentUser, ICurrentSite currentSite,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IProcResourceRepository procResourceRepository,
            IProcLoadPointRepository procLoadPointRepository,
            IProcLoadPointLinkMaterialRepository procLoadPointLinkMaterialRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcBomDetailReplaceMaterialRepository procBomDetailReplaceMaterialRepository,
            IProcMaterialRepository procMaterialRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IPlanWorkOrderActivationRepository planWorkOrderActivationRepository,
            IManuFeedingRepository manuFeedingRepository,
            IManuFeedingRecordRepository manuFeedingRecordRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procResourceRepository = procResourceRepository;
            _procLoadPointRepository = procLoadPointRepository;
            _procLoadPointLinkMaterialRepository = procLoadPointLinkMaterialRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procBomDetailReplaceMaterialRepository = procBomDetailReplaceMaterialRepository;
            _procMaterialRepository = procMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _manuFeedingRepository = manuFeedingRepository;
            _manuFeedingRecordRepository = manuFeedingRecordRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
        }


        /// <summary>
        /// 查询资源
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOptionDto>> GetFeedingResourceListAsync(ManuFeedingResourceQueryDto queryDto)
        {
            List<ProcResourceEntity> resources = new();
            switch (queryDto.Source)
            {
                case FeedingSourceEnum.Equipment:
                    resources.AddRange(await _procResourceRepository.GetByEquipmentCodeAsync(new ProcResourceQuery
                    {
                        SiteId = _currentSite.SiteId ?? 123456,
                        EquipmentCode = queryDto.Code
                    }));
                    break;
                default:
                case FeedingSourceEnum.Resource:
                    resources.AddRange(await _procResourceRepository.GetByResourceCodeAsync(new ProcResourceQuery
                    {
                        SiteId = _currentSite.SiteId ?? 123456,
                        ResCode = queryDto.Code
                    }));
                    break;
            }

            return resources.Select(s => new SelectOptionDto
            {
                Key = $"{s.Id}",
                Label = s.ResCode,
                Value = $"{s.Id}"
            });
        }

        /// <summary>
        /// 查询上料点
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOptionDto>> GetFeedingLoadPointListAsync(ManuFeedingLoadPointQueryDto queryDto)
        {
            // 通过资源->上料点
            var loadPoints = await _procLoadPointRepository.GetByResourceIdAsync(queryDto.ResourceId);
            if (loadPoints == null) return Array.Empty<SelectOptionDto>();

            return loadPoints.Select(s => new SelectOptionDto
            {
                Key = $"{s.Id}",
                Label = s.LoadPoint,
                Value = $"{s.Id}"
            });
        }

        /// <summary>
        /// 查询工单
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOptionDto>> GetFeedingWorkOrderListAsync(ManuFeedingWorkOrderQueryDto queryDto)
        {
            // 读取资源绑定的产线
            var workCenter = await _inteWorkCenterRepository.GetByResourceIdAsync(queryDto.ResourceId);
            if (workCenter == null) return Array.Empty<SelectOptionDto>();

            // 通过产线->激活的工单
            var workOrders = await GetWorkOrderByWorkCenterIdAsync(workCenter.Id);
            if (workOrders == null) return Array.Empty<SelectOptionDto>();

            return workOrders.Select(s => new SelectOptionDto
            {
                Key = $"{s.Id}",
                Label = s.OrderCode,
                Value = $"{s.Id}"
            });
        }

        /// <summary>
        /// 查询物料
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingMaterialDto>> GetFeedingMaterialListAsync(ManuFeedingMaterialQueryDto queryDto)
        {
            IEnumerable<long>? bomMaterialIds = null;

            // 读取资源绑定的产线
            var workCenterLineEntity = await _inteWorkCenterRepository.GetByResourceIdAsync(queryDto.ResourceId);

            /*
            if (workCenterLineEntity == null)
            {
                materialIds = await GetMaterialIdsByResourceIdAsync(queryDto.ResourceId);
            }
            // 混线
            else if (workCenterLineEntity.IsMixLine == true)
            {
                materialIds = await GetMaterialIdsByResourceIdAsync(queryDto.ResourceId);
            }
            // 不混线
            else if (workCenterLineEntity.IsMixLine == false)
            {
                materialIds = await GetMaterialIdsByWorkCenterIdAsync(workCenterLineEntity.Id, queryDto.WorkOrderId);
            }
            else
            {
                return Array.Empty<ManuFeedingMaterialDto>();
            }
            */

            if (workCenterLineEntity == null) return Array.Empty<ManuFeedingMaterialDto>();

            // 全部需展示的物料ID
            List<long> materialIds = new();
            // 通过物料分组
            Dictionary<long, IGrouping<long, ManuFeedingEntity>>? manuFeedingsDictionary = new();

            // 通过产线->工单->BOM->查询物料
            bomMaterialIds = await GetMaterialIdsByWorkCenterIdAsync(workCenterLineEntity.Id, queryDto.WorkOrderId);

            // BOM的物料ID
            if (bomMaterialIds != null) materialIds.AddRange(bomMaterialIds);

            // 通过物料ID获取物料库存信息
            var manuFeedings = await _manuFeedingRepository.GetByResourceIdAndMaterialIdsAsync(new GetByResourceIdAndMaterialIdsQuery
            {
                ResourceId = queryDto.ResourceId,
                //MaterialIds = materialIds,
            });

            // 已加载的物料ID
            if (manuFeedings != null)
            {
                materialIds.AddRange(manuFeedings.Select(s => s.ProductId));
                manuFeedingsDictionary = manuFeedings.ToLookup(w => w.ProductId).ToDictionary(d => d.Key, d => d);

                materialIds = materialIds.Distinct().AsList();
            }

            // 查询不到物料
            if (materialIds == null || materialIds.Any() == false) return Array.Empty<ManuFeedingMaterialDto>();

            // 通过物料ID获取物料集合
            var materials = await _procMaterialRepository.GetByIdsAsync(materialIds);

            // 填充返回集合
            List<ManuFeedingMaterialDto> list = new();
            foreach (var item in materials)
            {
                var material = new ManuFeedingMaterialDto
                {
                    MaterialId = item.Id,
                    MaterialCode = item.MaterialCode,
                    MaterialName = item.MaterialName,
                    Version = item.Version ?? "-",
                    Children = new()
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
        /// 物料添加
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ManuFeedingMaterialSaveDto saveDto)
        {
            // 查询条码
            var inventory = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery { SiteId = _currentSite.SiteId, BarCode = saveDto.BarCode });
            if (inventory.QuantityResidue <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16909)).WithData("barCode", saveDto.BarCode);
            }

            // 查询物料
            var material = await _procMaterialRepository.GetByIdAsync(saveDto.ProductId) ?? throw new CustomerValidationException(nameof(ErrorCode.MES10204));

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuFeedingEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId ?? 123456;
            entity.MaterialId = inventory.MaterialId;
            entity.SupplierId = inventory.SupplierId;

            // 一次性上完料
            entity.InitQty = inventory.QuantityResidue;
            entity.Qty += entity.InitQty;

            // 检查物料条码和物料是否对应的上，如果不是主物料，就找下替代料
            if (inventory.MaterialId != material.Id)
            {
                // 读取资源绑定的产线
                var workCenter = await _inteWorkCenterRepository.GetByResourceIdAsync(entity.ResourceId)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES16803));    // ErrorCode.MES15502

                // 通过产线->工单->BOM
                var bomIds = await GetBomIdsByWorkCenterIdAsync(workCenter.Id, null);
                if (bomIds == null || bomIds.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES10612));

                // 获取关联BOM
                var bomDetailEntities = await _procBomDetailRepository.GetByBomIdsAsync(bomIds);
                var bomDetailEntitiy = bomDetailEntities.FirstOrDefault(w => w.MaterialId == material.Id)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES16315)).WithData("barCode", inventory.MaterialBarCode);

                // 检查是否符合替代料
                var bomDetailReplaceMaterialEntities = await _procBomDetailReplaceMaterialRepository.GetByBomDetailIdAsync(bomDetailEntitiy.Id);
                if (bomDetailReplaceMaterialEntities.Any(a => a.ReplaceMaterialId == inventory.MaterialId) == false) throw new CustomerValidationException(nameof(ErrorCode.MES16315)).WithData("barCode", inventory.MaterialBarCode);
            }

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();

            // 添加物料台账记录
            rows += await _whMaterialStandingbookRepository.InsertAsync(new WhMaterialStandingbookEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                MaterialCode = material.MaterialCode,
                MaterialName = material.MaterialName,
                MaterialVersion = material.Version ?? "",
                MaterialBarCode = saveDto.BarCode,
                Batch = inventory.Batch,
                Quantity = entity.InitQty,
                Unit = material.Unit ?? "",
                Type = WhMaterialInventoryTypeEnum.MaterialLoading,
                Source = inventory.Source,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn
            });

            // 将状态恢复为"使用中"
            rows += await _whMaterialInventoryRepository.UpdatePointByBarCodeAsync(new UpdateStatusByBarCodeCommand
            {
                BarCode = saveDto.BarCode,
                QuantityResidue = inventory.QuantityResidue - entity.InitQty,
                Status = WhMaterialInventoryStatusEnum.InUse,
                UpdatedBy = entity.CreatedBy,
                UpdatedOn = entity.CreatedOn
            });

            rows += await _manuFeedingRepository.InsertAsync(entity);
            rows += await _manuFeedingRecordRepository.InsertAsync(GetManuFeedingRecord(entity, FeedingDirectionTypeEnum.Load));
            trans.Complete();

            return rows;
        }

        /// <summary>
        /// 物料移除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            var entities = await _manuFeedingRepository.GetByIdsAsync(idsArr);
            if (entities.Any() == false) return 0;

            var feeds = await _manuFeedingRepository.GetByIdsAsync(idsArr);

            // 查询条码
            var inventorys = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
            {
                BarCodes = feeds.Select(s => s.BarCode),
                SiteId = _currentSite.SiteId ?? 123456
            });

            // 查询物料
            var materials = await _procMaterialRepository.GetByIdsAsync(feeds.Select(s => s.ProductId).ToArray());

            var now = HymsonClock.Now();
            List<WhMaterialStandingbookEntity> whMaterialStandingbookEntities = new();
            List<UpdateStatusByBarCodeCommand> updateStatusByBarCodeCommands = new();
            List<ManuFeedingRecordEntity> manuFeedingRecordEntities = new();
            foreach (var entity in entities)
            {
                entity.UpdatedBy = _currentUser.UserName;
                entity.UpdatedOn = now;

                var inventory = inventorys.FirstOrDefault(w => w.MaterialBarCode == entity.BarCode);
                if (inventory == null) continue;

                var material = materials.FirstOrDefault(w => w.Id == entity.ProductId);
                if (material == null) continue;

                // 添加物料台账记录
                whMaterialStandingbookEntities.Add(new WhMaterialStandingbookEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = entity.SiteId,
                    MaterialCode = material.MaterialCode,
                    MaterialName = material.MaterialName,
                    MaterialVersion = material.Version ?? "",
                    MaterialBarCode = entity.BarCode,
                    Batch = inventory.Batch,
                    Quantity = entity.Qty,
                    Unit = material.Unit ?? "",
                    Type = WhMaterialInventoryTypeEnum.MaterialReturn,
                    Source = inventory.Source,
                    CreatedBy = entity.UpdatedBy,
                    CreatedOn = now,
                    UpdatedBy = entity.UpdatedBy,
                    UpdatedOn = now
                });

                // 将状态恢复为"待使用"
                updateStatusByBarCodeCommands.Add(new UpdateStatusByBarCodeCommand
                {
                    BarCode = entity.BarCode,
                    QuantityResidue = entity.Qty,
                    Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                    UpdatedBy = entity.UpdatedBy,
                    UpdatedOn = entity.UpdatedOn
                });

                // 添加操作记录
                manuFeedingRecordEntities.Add(GetManuFeedingRecord(entity, FeedingDirectionTypeEnum.Unload));
            }

            // 开启事务
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();

            // 保存物料台账记录
            rows += await _whMaterialStandingbookRepository.InsertsAsync(whMaterialStandingbookEntities);

            // 更新状态
            rows += await _whMaterialInventoryRepository.UpdatePointByBarCodesAsync(updateStatusByBarCodeCommands);

            // 卸料
            rows += await _manuFeedingRepository.DeleteByIdsAsync(idsArr);

            // 保存操作记录
            rows += await _manuFeedingRecordRepository.InsertsAsync(manuFeedingRecordEntities);
            trans.Complete();

            return rows;
        }





        #region 内部方法
        /// <summary>
        /// 获取上料记录对象
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="directionType"></param>
        /// <returns></returns>
        private static ManuFeedingRecordEntity GetManuFeedingRecord(ManuFeedingEntity entity, FeedingDirectionTypeEnum directionType)
        {
            return new ManuFeedingRecordEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                ResourceId = entity.ResourceId,
                FeedingPointId = entity.FeedingPointId,
                ProductId = entity.ProductId,
                BarCode = entity.BarCode,
                Qty = entity.Qty,
                DirectionType = directionType,
                CreatedBy = entity.CreatedBy,   // 这里用原纪录的值？
                CreatedOn = entity.CreatedOn,   // 这里用原纪录的值？
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn,
                IsDeleted = entity.IsDeleted,
                SiteId = entity.SiteId,
            };
        }

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
        /// 通过工作中心ID获取工单集合
        /// </summary>
        /// <param name="workCenterId">产线ID</param>
        /// <returns></returns>
        private async Task<IEnumerable<PlanWorkOrderEntity>?> GetWorkOrderByWorkCenterIdAsync(long workCenterId)
        {
            // 通过车间查询工单
            //var workOrdersOfFarm = await _planWorkOrderRepository.GetByWorkFarmIdAsync(workCenterId);

            // 通过产线查询工单
            var workOrdersOfLine = await _planWorkOrderRepository.GetByWorkLineIdAsync(workCenterId);

            /*
            // 合并结果
            var workOrders = workOrdersOfFarm.Union(workOrdersOfLine);
            return workOrders;
            */

            return workOrdersOfLine;
        }

        /// <summary>
        /// 通过工作中心ID获取BomID集合
        /// </summary>
        /// <param name="workCenterId"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<long>?> GetBomIdsByWorkCenterIdAsync(long workCenterId, long? workOrderId)
        {
            var workOrders = await GetWorkOrderByWorkCenterIdAsync(workCenterId);
            if (workOrders == null) return null;

            // 保留指定的工单
            if (workOrderId.HasValue == true) workOrders = workOrders.Where(w => w.Id == workOrderId.Value);

            // 判断是否有激活中的工单
            var planWorkOrderActivationEntities = await _planWorkOrderActivationRepository.GetByWorkOrderIdsAsync(workOrders.Select(s => s.Id).ToArray());
            if (planWorkOrderActivationEntities == null || planWorkOrderActivationEntities.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15501));
            }

            // 通过工单查询BOM
            return workOrders.Select(s => s.ProductBOMId);
        }

        /// <summary>
        /// 通过工作中心ID获取物料ID集合（不混线）
        /// </summary>
        /// <param name="workCenterId"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<long>?> GetMaterialIdsByWorkCenterIdAsync(long workCenterId, long? workOrderId)
        {
            // 通过工单查询BOM
            var bomIds = await GetBomIdsByWorkCenterIdAsync(workCenterId, workOrderId);
            if (bomIds == null) return null;

            var bomDetailEntities = await _procBomDetailRepository.GetByBomIdsAsync(bomIds);
            if (bomDetailEntities == null) return null;

            // 数据收集方式为“批次”的物料
            var bomDetailEntitiesOfBatch = bomDetailEntities.Where(w => w.DataCollectionWay == MaterialSerialNumberEnum.Batch);

            // 当“数据收集方式”为空，则去检查物料维护中 物料的“数据收集方式”，为批次也可以加载进来
            // 因为目前BOM里面的“数据收集方式”为必填项，因为无需理会
            // UNDO

            return bomDetailEntitiesOfBatch.Select(s => s.MaterialId);
        }
        #endregion

    }
}
