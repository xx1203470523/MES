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
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
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
using Microsoft.Extensions.Logging;

namespace Hymson.MES.Services.Services.Manufacture.ManuFeeding
{
    /// <summary>
    /// 服务（物料加载）
    /// </summary>
    public class ManuFeedingService : IManuFeedingService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<ManuFeedingService> _logger;

        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        ///  仓储（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

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
        /// 仓储接口（物料替代料）
        /// </summary>
        private readonly IProcReplaceMaterialRepository _procReplaceMaterialRepository;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

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
        /// <param name="logger"></param>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="equEquipmentRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procLoadPointRepository"></param>
        /// <param name="procLoadPointLinkMaterialRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procBomDetailReplaceMaterialRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procReplaceMaterialRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="planWorkOrderActivationRepository"></param>
        /// <param name="manuFeedingRepository"></param>
        /// <param name="manuFeedingRecordRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        public ManuFeedingService(ILogger<ManuFeedingService> logger,
            ICurrentUser currentUser, ICurrentSite currentSite,
            IEquEquipmentRepository equEquipmentRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IProcResourceRepository procResourceRepository,
            IProcLoadPointRepository procLoadPointRepository,
            IProcLoadPointLinkMaterialRepository procLoadPointLinkMaterialRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcBomDetailReplaceMaterialRepository procBomDetailReplaceMaterialRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcReplaceMaterialRepository procReplaceMaterialRepository,
            IProcProcedureRepository procProcedureRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IPlanWorkOrderActivationRepository planWorkOrderActivationRepository,
            IManuFeedingRepository manuFeedingRepository,
            IManuFeedingRecordRepository manuFeedingRecordRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository)
        {
            _logger = logger;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equEquipmentRepository = equEquipmentRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procResourceRepository = procResourceRepository;
            _procLoadPointRepository = procLoadPointRepository;
            _procLoadPointLinkMaterialRepository = procLoadPointLinkMaterialRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procBomDetailReplaceMaterialRepository = procBomDetailReplaceMaterialRepository;
            _procMaterialRepository = procMaterialRepository;
            _procReplaceMaterialRepository = procReplaceMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
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
                    var equipmentEntity = await _equEquipmentRepository.GetByCodeAsync(new EntityByCodeQuery
                    {
                        Site = _currentSite.SiteId ?? 0,
                        Code = queryDto.Code
                    }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19005)).WithData("Code", queryDto.Code);

                    // 这个方法后面可以改为通过设备ID查询
                    resources.AddRange(await _procResourceRepository.GetByEquipmentCodeAsync(new ProcResourceQuery
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        EquipmentCode = equipmentEntity.EquipmentCode
                    }));
                    break;
                case FeedingSourceEnum.Resource:
                    var resource = await _procResourceRepository.GetByResourceCodeAsync(new ProcResourceQuery
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        ResCode = queryDto.Code
                    }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19109)).WithData("Code", queryDto.Code);

                    resources.Add(resource);
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

            // 只显示"启用"和"保留"
            loadPoints = loadPoints.Where(w => w.Status == SysDataStatusEnum.Enable || w.Status == SysDataStatusEnum.Retain);

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

            // 全部需展示的物料ID
            List<long> materialIds = new();
            var loadSource = 1; // 1:资源;2:上料点

            // 通过物料分组
            Dictionary<long, IGrouping<long, ManuFeedingEntity>>? manuFeedingsDictionary = new();

            if (queryDto.Source == ManuSFCFeedingSourceEnum.BOM)
            {
                // 这句是兼容代码。选择BOM类型时，之前切换上料点的值没有清空，所以这里需要清空
                queryDto.FeedingPointId = null;

                // 2023.10.17 中越和产品说，需要再过滤一次，只要资源关联工序对应的物料（这个方法是有问题的，因为程序没有限制一个资源可以绑定多个工序）
                var procedureEntity = await _procProcedureRepository.GetProcProcedureByResourceIdAsync(new ProcProdureByResourceIdQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    ResourceId = queryDto.ResourceId
                });

                // 读取资源绑定的产线
                var workCenterLineEntity = await _inteWorkCenterRepository.GetByResourceIdAsync(queryDto.ResourceId);

                if (workCenterLineEntity == null) return Array.Empty<ManuFeedingMaterialDto>();

                // 通过产线->工单->BOM->查询物料
                bomMaterialIds = await GetMaterialIdsByWorkCenterIdAsync(workCenterLineEntity.Id, queryDto.WorkOrderId, procedureEntity?.Id);

                // BOM的物料ID
                if (bomMaterialIds != null) materialIds.AddRange(bomMaterialIds);
            }
            else
            {
                // 通过资源->上料点
                var loadPoints = await _procLoadPointRepository.GetByResourceIdAsync(queryDto.ResourceId);
                if (loadPoints != null && loadPoints.Any())
                {
                    // 只显示"启用"和"保留"
                    loadPoints = loadPoints.Where(w => w.Status == SysDataStatusEnum.Enable || w.Status == SysDataStatusEnum.Retain);

                    // 通过上料点->物料
                    var loadPointMaterials = await _procLoadPointLinkMaterialRepository.GetByLoadPointIdAsync(loadPoints.Select(s => s.Id));
                    if (loadPointMaterials != null && loadPointMaterials.Any())
                    {
                        if (queryDto.FeedingPointId.HasValue && queryDto.FeedingPointId.Value > 0)
                        {
                            loadSource = 2;
                            loadPointMaterials = loadPointMaterials.Where(w => w.LoadPointId == queryDto.FeedingPointId.Value);
                        }

                        materialIds.AddRange(loadPointMaterials.Select(s => s.MaterialId).Distinct());
                    }
                }
            }

            IEnumerable<ManuFeedingEntity> manuFeedings;
            if (loadSource == 1)
            {
                // 通过资源ID获取物料库存信息
                manuFeedings = await _manuFeedingRepository.GetByResourceIdAndMaterialIdsAsync(new GetByResourceIdAndMaterialIdsQuery
                {
                    LoadSource = queryDto.Source,
                    ResourceId = queryDto.ResourceId,
                    FeedingPointId = queryDto.FeedingPointId
                });
            }
            else
            {
                // 通过上料点ID获取物料库存信息
                manuFeedings = await _manuFeedingRepository.GetByFeedingPointIdAndMaterialIdsAsync(new GetByFeedingPointIdAndMaterialIdsQuery
                {
                    LoadSource = queryDto.Source,
                    FeedingPointId = queryDto.FeedingPointId!.Value
                });
            }

            // 已加载的物料ID
            if (manuFeedings != null) manuFeedingsDictionary = manuFeedings.ToLookup(w => w.ProductId).ToDictionary(d => d.Key, d => d);

            // 不在集合里面的物料ID
            var notIncludeIds = manuFeedingsDictionary.Keys.Except(materialIds);

            // 集合
            var unionMaterialIds = materialIds.Union(notIncludeIds);

            // 查询不到物料
            if (materialIds == null || !materialIds.Any()) return Array.Empty<ManuFeedingMaterialDto>();

            // 通过物料ID获取物料集合
            var materials = await _procMaterialRepository.GetByIdsAsync(unionMaterialIds);

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
                    IsHistory = notIncludeIds.Any(a => a == item.Id),
                    Children = new()
                };

                if (manuFeedingsDictionary.TryGetValue(material.MaterialId, out var feedingEntities))
                {
                    material.Children.AddRange(feedingEntities.Select(s => new ManuFeedingMaterialItemDto
                    {
                        Id = s.Id,
                        ParentId = s.ProductId,
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
        public async Task<ManuFeedingMaterialResponseDto> CreateAsync(ManuFeedingMaterialSaveDto saveDto)
        {
            // 查询条码
            var inventory = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
            {
                SiteId = _currentSite.SiteId,
                BarCode = saveDto.BarCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16908)).WithData("barCode", saveDto.BarCode);

            // 是否有剩余数量
            if (inventory.QuantityResidue <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16909)).WithData("barCode", saveDto.BarCode);
            }

            // 是否过期
            if (inventory.DueDate.HasValue && HymsonClock.Now() > inventory.DueDate.Value)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15508))
                    .WithData("BarCode", saveDto.BarCode)
                    .WithData("DueDate", inventory.DueDate.Value.ToShortDateString());
            }

            // 当是上料点类型时，一定要选择具体挂载的上料点
            if (saveDto.Source == ManuSFCFeedingSourceEnum.FeedingPoint
                && (!saveDto.FeedingPointId.HasValue || saveDto.FeedingPointId == 0))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16915)).WithData("barCode", saveDto.BarCode);
            }

            // 根据条件再查询一次主物料（再次查询一次会更加严谨）
            var manuFeedingMaterialDtos = await GetFeedingMaterialListAsync(new ManuFeedingMaterialQueryDto
            {
                ResourceId = saveDto.ResourceId,
                Source = saveDto.Source,
                FeedingPointId = saveDto.FeedingPointId
            });

            // 过滤掉历史清单
            manuFeedingMaterialDtos = manuFeedingMaterialDtos.Where(w => !w.IsHistory);
            if (manuFeedingMaterialDtos == null || !manuFeedingMaterialDtos.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES16914));

            // 主物料ID集合
            saveDto.MaterialIds = manuFeedingMaterialDtos.Select(s => s.MaterialId);

            // 查询物料
            var materials = await _procMaterialRepository.GetByIdsAsync(saveDto.MaterialIds);
            if (materials == null || !materials.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15101));
            }

            // 如果有设置物料，就用设置的物料
            if (saveDto.ProductId.HasValue) materials = materials.Where(w => w.Id == saveDto.ProductId.Value);

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuFeedingEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId ?? 0;
            entity.MaterialId = inventory.MaterialId;
            entity.SupplierId = inventory.SupplierId;
            entity.MaterialType = inventory.MaterialType;
            entity.WorkOrderId = inventory.WorkOrderId;
            entity.LoadSource = saveDto.Source;

            // 一次性上完料
            entity.InitQty = inventory.QuantityResidue;
            entity.Qty += entity.InitQty;

            // 匹配物料
            var bo = new ManuFeedingMatchBo
            {
                SiteId = entity.SiteId,
                InventoryMaterialId = inventory.MaterialId,
                ResourceId = saveDto.ResourceId
            };

            ProcMaterialEntity? material = null;
            if (saveDto.Source == ManuSFCFeedingSourceEnum.BOM)
            {
                material = await GetMatchMaterialsByBOMAsync(materials, bo);
            }
            else
            {
                material = await GetMatchMaterialsByPointAsync(materials, bo);
            }

            if (material == null) throw new CustomerValidationException(saveDto.ProductId.HasValue ? nameof(ErrorCode.MES15506) : nameof(ErrorCode.MES15505));
            entity.ProductId = material.Id;

            // 当有上料点值时，判断物料条码是否已上到该上料点
            if (entity.FeedingPointId.HasValue)
            {
                var feedingEntity = await _manuFeedingRepository.GetByBarCodeAndMaterialIdAsync(new GetByBarCodeAndMaterialIdQuery
                {
                    FeedingPointId = entity.FeedingPointId.Value,
                    ProductId = entity.ProductId,
                    BarCode = entity.BarCode
                });

                if (feedingEntity != null)
                {
                    _logger.LogWarning($"MES15507 -> FeedingPointId:{entity.FeedingPointId.Value},ProductId:{entity.ProductId},BarCode:{entity.BarCode}");
                    throw new CustomerValidationException(nameof(ErrorCode.MES15507)).WithData("BarCode", entity.BarCode);
                }
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

            // 因为前端要展开这级表格，所以把ID返回去
            return new ManuFeedingMaterialResponseDto
            {
                ResourceId = entity.ResourceId,
                ProductId = entity.ProductId,
                ProductCode = material.MaterialCode,
                Version = material.Version ?? "",
                Qty = inventory.QuantityResidue,
                BarCode = inventory.MaterialBarCode,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            };
        }

        /// <summary>
        /// 物料移除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            var entities = await _manuFeedingRepository.GetByIdsAsync(idsArr);
            if (!entities.Any()) return 0;

            var feeds = await _manuFeedingRepository.GetByIdsAsync(idsArr);

            // 查询条码
            var inventorys = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
            {
                BarCodes = feeds.Select(s => s.BarCode),
                SiteId = _currentSite.SiteId ?? 0
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
                MaterialId = entity.MaterialId,
                Qty = entity.Qty,
                DirectionType = directionType,
                CreatedBy = entity.CreatedBy,   // 这里用原纪录的值？
                CreatedOn = entity.CreatedOn,   // 这里用原纪录的值？
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn,
                IsDeleted = entity.IsDeleted,
                SiteId = entity.SiteId,
                MaterialType = entity.MaterialType,
                WorkOrderId = entity.WorkOrderId,
                LoadSource = entity.LoadSource
            };
        }

        /// <summary>
        /// 通过工作中心ID获取工单集合
        /// </summary>
        /// <param name="workCenterId">产线ID</param>
        /// <returns></returns>
        private async Task<IEnumerable<PlanWorkOrderEntity>?> GetWorkOrderByWorkCenterIdAsync(long workCenterId)
        {
            // 通过产线查询工单
            var workOrdersOfLine = await _planWorkOrderRepository.GetByWorkLineIdAsync(workCenterId);

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
            if (workOrderId.HasValue) workOrders = workOrders.Where(w => w.Id == workOrderId.Value);

            // 判断是否有激活中的工单
            var planWorkOrderActivationEntities = await _planWorkOrderActivationRepository.GetByWorkOrderIdsAsync(workOrders.Select(s => s.Id).ToArray());
            if (planWorkOrderActivationEntities == null || !planWorkOrderActivationEntities.Any())
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
        /// <param name="procedureId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<long>?> GetMaterialIdsByWorkCenterIdAsync(long workCenterId, long? workOrderId, long? procedureId)
        {
            // 通过工单查询BOM
            var bomIds = await GetBomIdsByWorkCenterIdAsync(workCenterId, workOrderId);
            if (bomIds == null) return null;

            var bomDetailEntities = await _procBomDetailRepository.GetByBomIdsAsync(bomIds);
            if (bomDetailEntities == null) return null;

            // 数据收集方式为“批次”的物料
            var bomDetailEntitiesOfBatch = bomDetailEntities
                .Where(w => w.DataCollectionWay == MaterialSerialNumberEnum.Batch)
                .Where(w => w.ProcedureId == procedureId);

            // 当“数据收集方式”为空，则去检查物料维护中 物料的“数据收集方式”，为批次也可以加载进来
            // 因为目前BOM里面的“数据收集方式”为必填项，因为无需理会
            // UNDO

            return bomDetailEntitiesOfBatch.Select(s => s.MaterialId);
        }

        /// <summary>
        /// 匹配物料（BOM）
        /// </summary>
        /// <param name="materials"></param>
        /// <param name="bo"></param>
        /// <returns></returns>
        private async Task<ProcMaterialEntity?> GetMatchMaterialsByBOMAsync(IEnumerable<ProcMaterialEntity> materials, ManuFeedingMatchBo bo)
        {
            // 读取资源绑定的产线
            var workCenter = await _inteWorkCenterRepository.GetByResourceIdAsync(bo.ResourceId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16803));    // ErrorCode.MES15502

            // 通过产线->工单->BOM
            var bomIds = await GetBomIdsByWorkCenterIdAsync(workCenter.Id, null);
            if (bomIds == null || !bomIds.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10612));

            // 获取关联BOM
            var bomDetailEntities = await _procBomDetailRepository.GetByBomIdsAsync(bomIds);

            // 查询物料基础数据的替代料（批量）
            var replaceMaterialsForMain = await _procReplaceMaterialRepository.GetProcReplaceMaterialViewListAsync(bo.SiteId);
            var replaceMaterialsForMainDic = replaceMaterialsForMain.ToLookup(w => w.MaterialId).ToDictionary(d => d.Key, d => d);

            ProcMaterialEntity? material = null;
            foreach (var item in materials)
            {
                // 检查物料条码和物料是否对应的上
                if (bo.InventoryMaterialId == item.Id)
                {
                    material = item;
                    break;
                }

                // 如果不是主物料，就找下替代料
                var bomDetailEntitiy = bomDetailEntities.FirstOrDefault(w => w.MaterialId == item.Id);
                if (bomDetailEntitiy == null) continue;

                // 填充主物料替代料
                if (bomDetailEntitiy.IsEnableReplace)
                {
                    if (!replaceMaterialsForMainDic.TryGetValue(item.Id, out var replaces)) continue;
                    if (!replaces.Any(a => a.IsEnabled && a.ReplaceMaterialId == bo.InventoryMaterialId)) continue;
                }
                // 填充BOM替代料
                else
                {
                    // 检查是否符合替代料
                    var bomDetailReplaceMaterialEntities = await _procBomDetailReplaceMaterialRepository.GetByBomDetailIdAsync(bomDetailEntitiy.Id);
                    if (!bomDetailReplaceMaterialEntities.Any(a => a.ReplaceMaterialId == bo.InventoryMaterialId)) continue;
                }

                material = item;
            }

            return material;
        }

        /// <summary>
        /// 匹配物料（上料点）
        /// </summary>
        /// <param name="materials"></param>
        /// <param name="bo"></param>
        /// <returns></returns>
        private async Task<ProcMaterialEntity?> GetMatchMaterialsByPointAsync(IEnumerable<ProcMaterialEntity> materials, ManuFeedingMatchBo bo)
        {
            // 查询物料基础数据的替代料（批量）
            var replaceMaterialsForMain = await _procReplaceMaterialRepository.GetProcReplaceMaterialViewListAsync(bo.SiteId);
            var replaceMaterialsForMainDic = replaceMaterialsForMain.ToLookup(w => w.MaterialId).ToDictionary(d => d.Key, d => d);

            ProcMaterialEntity? material = null;
            foreach (var item in materials)
            {
                // 检查物料条码和物料是否对应的上
                if (bo.InventoryMaterialId == item.Id)
                {
                    material = item;
                    break;
                }

                // 如果不是主物料，就找下替代料
                if (!replaceMaterialsForMainDic.TryGetValue(item.Id, out var replaces)) continue;
                if (!replaces.Any(a => a.IsEnabled && a.ReplaceMaterialId == bo.InventoryMaterialId)) continue;

                material = item;
            }

            return material;
        }
        #endregion

    }
}
