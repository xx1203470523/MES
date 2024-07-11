using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Utils;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.CoreServices.Dtos.Qkny;
using System.Transactions;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process.LoadPointLink.Query;
using Hymson.MessagePush.Helper;
using Microsoft.Extensions.Logging;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Core.Constants.Common;

namespace Hymson.MES.CoreServices.Services.Qkny
{
    /// <summary>
    /// 物料加载
    /// </summary>
    public class ManuFeedingService : IManuFeedingService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<ManuFeedingService> _logger;

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
        /// 系统配置
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 上料点关联资源&设备
        /// </summary>
        private readonly IProcLoadPointLinkResourceRepository _procLoadPointLinkResourceRepository;

        /// <summary>
        /// 自定义字段
        /// </summary>
        private readonly IInteCustomFieldBusinessEffectuateRepository _inteCustomFieldBusinessEffectuateRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuFeedingService(
            ILogger<ManuFeedingService> logger,
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
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
            ISysConfigRepository sysConfigRepository,
            IProcLoadPointLinkResourceRepository procLoadPointLinkResourceRepository,
            IInteCustomFieldBusinessEffectuateRepository inteCustomFieldBusinessEffectuateRepository)
        {
            _logger = logger;
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
            _sysConfigRepository = sysConfigRepository;
            _procLoadPointLinkResourceRepository = procLoadPointLinkResourceRepository;
            _inteCustomFieldBusinessEffectuateRepository = inteCustomFieldBusinessEffectuateRepository;
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
                SiteId = saveDto.SiteId,
                BarCode = saveDto.BarCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16908)).WithData("barCode", saveDto.BarCode);
            //TODO 顷刻
            //1. 添加条码状态校验，lims过来的条码可能是NG状态，需要进行提示

            if (inventory.QuantityResidue <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16909)).WithData("barCode", saveDto.BarCode);
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
                SiteId = saveDto.SiteId,
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
            //var entity = saveDto.ToEntity<ManuFeedingEntity>();
            ManuFeedingEntity entity = new ManuFeedingEntity();
            entity.ResourceId = saveDto.ResourceId;
            entity.FeedingPointId = saveDto.FeedingPointId;
            //entity.ProductId = (long)saveDto.ProductId;
            entity.ProductId = inventory.MaterialId;
            entity.BarCode = saveDto.BarCode;

            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = saveDto.UserName;
            entity.UpdatedBy = saveDto.UserName;
            entity.SiteId = saveDto.SiteId;
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
                    //_logger.LogWarning($"MES15507 -> FeedingPointId:{entity.FeedingPointId.Value},ProductId:{entity.ProductId},BarCode:{entity.BarCode}");
                    throw new CustomerValidationException(nameof(ErrorCode.MES15507)).WithData("BarCode", entity.BarCode);
                }
            }

            #region 校验已上料(自制件)条码个数

            if (saveDto.ProcedureId.HasValue)
            {
                //查询工序自定义配置
                var customFieldValue = await _inteCustomFieldBusinessEffectuateRepository.GetCustomeFieldValue(saveDto.ProcedureId.Value, Core.Enums.Integrated.InteCustomFieldBusinessTypeEnum.Procedure, CustomFieldName.PolarReelLoadLimit);
                var limitCount = string.IsNullOrWhiteSpace(customFieldValue) ? 0 : customFieldValue.ParseToInt();
                if (limitCount != 0)
                {
                    //查询已上料条码个数
                    var feedingList = await _manuFeedingRepository.GetByResourceIdAndMaterialIdsWithOutZeroAsync(new GetByResourceIdAndMaterialIdsQuery
                    {
                        ResourceId = saveDto.ResourceId
                    });
                    if (feedingList != null && feedingList.Count(x => x.MaterialType != Core.Enums.Warehouse.MaterialInventoryMaterialTypeEnum.PurchaseParts) >= limitCount)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES15510));
                    }
                }
            }

            #endregion

            #region 上料点上料条码下发到设备
            //上料点才需要下发给设备
            if (saveDto.Source == ManuSFCFeedingSourceEnum.FeedingPoint && saveDto.FeedingPointId != null)
            {
                //判断上料点是否存在
                ProcLoadPointEntity loadPointEntity = await _procLoadPointRepository.GetByIdAsync((long)saveDto.FeedingPointId);
                if (loadPointEntity != null)
                {
                    //获取配置
                    SysConfigQuery query = new SysConfigQuery();
                    query.SiteId = saveDto.SiteId;
                    query.Type = SysConfigEnum.UpMaterialSendEqu;
                    List<SysConfigEntity> configList = (await _sysConfigRepository.GetEntitiesAsync(query)).ToList();
                    if (configList != null && configList.Count > 0)
                    {
                        List<string> loadPointList = configList.Select(m => m.Value).ToList();
                        //系统中配置了相应的上料点，则需要下发给设备
                        if (loadPointList.Contains(loadPointEntity.LoadPoint) == true)
                        {
                            //根据上料点，获取对应的资源，设备，设备IP
                            ProcLoadPointCodeLinkResourceQuery linkResQuery = new ProcLoadPointCodeLinkResourceQuery();
                            linkResQuery.LoadPoint = loadPointEntity.LoadPoint;
                            var sendEquList = await _procLoadPointLinkResourceRepository.GetEquByCodeAsync(linkResQuery);
                            if (sendEquList != null && sendEquList.Count() > 0)
                            {
                                //IP不为空才会下发
                                sendEquList = sendEquList.Where(m => string.IsNullOrEmpty(m.Ip) == false).ToList();
                                foreach (var item in sendEquList)
                                {
                                    string url = $"http://{item.Ip}:6868/api/SendBatchSfc";
                                    string body = $"{{\"BatchSfc\":\"{saveDto.BarCode}\",\"EquipmentCode\":\"{item.EquipmentCode}\"}}";
                                    try
                                    {
                                        await HttpHelper.HttpsPostAsync(url, body, "application/json");
                                    }
                                    catch (Exception ex)
                                    {
                                        string logErrorMsg = $"【{item.EquipmentCode}】设备SendBatchSfc异常。URL:{url}。内容:{body}。异常信息:{ex}";
                                        _logger.LogError(logErrorMsg);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

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
                Quantity = -entity.InitQty,
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
                // 读取资源绑定的产线
                var workCenterLineEntity = await _inteWorkCenterRepository.GetByResourceIdAsync(queryDto.ResourceId);

                if (workCenterLineEntity == null) return Array.Empty<ManuFeedingMaterialDto>();

                // 这句是兼容代码。选择BOM类型时，之前切换上料点的值没有清空，所以这里需要清空
                queryDto.FeedingPointId = null;

                // 2023.10.17 中越和产品说，需要再过滤一次，只要资源关联工序对应的物料（这个方法是有问题的，因为程序没有限制一个资源可以绑定多个工序）
                var procedureEntity = await _procProcedureRepository.GetProcProcedureByResourceIdAsync(new ProcProdureByResourceIdQuery
                {
                    SiteId = queryDto.SiteId,
                    ResourceId = queryDto.ResourceId
                });

                // 通过产线->工单->BOM->查询物料
                bomMaterialIds = await GetMaterialIdsByWorkCenterIdAsync(workCenterLineEntity.Id, queryDto.WorkOrderId, procedureEntity?.Id);

                // BOM的物料ID
                if (bomMaterialIds != null) materialIds.AddRange(bomMaterialIds);
            }
            else
            {
                // 通过资源->上料点
                //var loadPoints = await _procLoadPointRepository.GetByResourceIdAsync(queryDto.ResourceId);
                var loadPoint = await _procLoadPointRepository.GetByIdAsync((long)queryDto.FeedingPointId!);
                if (loadPoint != null && (loadPoint.Status == SysDataStatusEnum.Enable || loadPoint.Status == SysDataStatusEnum.Retain))
                {
                    // 只显示"启用"和"保留"
                    //loadPoints = loadPoints.Where(w => w.Status == SysDataStatusEnum.Enable || w.Status == SysDataStatusEnum.Retain);

                    // 通过上料点->物料
                    List<long> idList = new List<long>() { loadPoint.Id };
                    var loadPointMaterials = await _procLoadPointLinkMaterialRepository.GetByLoadPointIdAsync(idList);
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
        /// 根据上料点id获取最新上料记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuFeedingEntity> GetFeedingPointNewAsync(GetFeedingPointNewQuery query)
        {
            var dbModel = await _manuFeedingRepository.GetFeedingPointNewAsync(query);
            if(dbModel == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45071));
            }
            return dbModel!;
        }

        /// <summary>
        /// 上料物料转移
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<bool> ManuFeedingTransferAsync(ManuFeedingTransferSaveDto saveDto)
        {
            if (saveDto.SourceId == saveDto.DestId)
            {
                return true;
            }
            ManuSFCFeedingSourceEnum addSource = ManuSFCFeedingSourceEnum.FeedingPoint;
            //查询条码信息
            GetManuFeedingSfcQuery sfcQuery = new GetManuFeedingSfcQuery();
            sfcQuery.BarCode = saveDto.Sfc;
            if(saveDto.TransferType == ManuSFCFeedingTransferEnum.FeedingPoint 
                || saveDto.TransferType == ManuSFCFeedingTransferEnum.FeedingPointResource)
            {
                sfcQuery.LoadSource = ManuSFCFeedingSourceEnum.FeedingPoint;
                addSource = ManuSFCFeedingSourceEnum.BOM;
            }
            else
            {
                sfcQuery.LoadSource = ManuSFCFeedingSourceEnum.BOM;
                addSource = ManuSFCFeedingSourceEnum.FeedingPoint;
            }
            var manuFeedingEntity = await _manuFeedingRepository.GetManuFeedingSfcAsync(sfcQuery);

            //校验
            if(manuFeedingEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45072))
                    .WithData("Sfc", saveDto.Sfc);
            }
            if(manuFeedingEntity.Qty <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45073))
                    .WithData("Sfc", saveDto.Sfc);
            }

            DateTime curDate = HymsonClock.Now();
            //更新数据
            UpdateFeedingQtyCommand command = new UpdateFeedingQtyCommand();
            command.BarCode = saveDto.Sfc;
            command.Qty = saveDto.Qty ?? manuFeedingEntity.Qty;
            command.UpdatedBy = saveDto.OpeationBy;
            command.UpdatedOn = curDate;
            if (saveDto.Qty == manuFeedingEntity.Qty)
            {
                command.IsDeleted = 1;
            }
            //新增数据
            manuFeedingEntity.Id = IdGenProvider.Instance.CreateId();
            manuFeedingEntity.CreatedOn = curDate;
            manuFeedingEntity.CreatedBy = saveDto.OpeationBy;
            manuFeedingEntity.InitQty = command.Qty;
            manuFeedingEntity.Qty = command.Qty;
            manuFeedingEntity.WorkOrderId = null;
            manuFeedingEntity.LoadSource = addSource;
            manuFeedingEntity.UpdatedOn = curDate;
            manuFeedingEntity.UpdatedBy = "";

            //更新数量
            if (saveDto.TransferType == ManuSFCFeedingTransferEnum.Resource)
            {
                //更新-修改旧资源数量
                command.ResourceId = saveDto.SourceId;
                command.FeedingPointId = 0;
                //添加记录-添加新资源数据，修改资源id
                manuFeedingEntity.ResourceId = saveDto.DestId;
                manuFeedingEntity.FeedingPointId = null;
            }
            else if(saveDto.TransferType == ManuSFCFeedingTransferEnum.FeedingPoint)
            {
                //更新
                command.ResourceId = 0;
                command.FeedingPointId = saveDto.SourceId;
                //添加记录
                manuFeedingEntity.ResourceId = saveDto.LoadPointResoucesId;
                manuFeedingEntity.FeedingPointId = saveDto.DestId;
            }
            else if(saveDto.TransferType == ManuSFCFeedingTransferEnum.FeedingPointResource)
            {
                //更新
                command.ResourceId = 0;
                command.FeedingPointId = saveDto.SourceId;
                //添加记录
                manuFeedingEntity.FeedingPointId = null;
                manuFeedingEntity.ResourceId = saveDto.DestId;
            }
            else if(saveDto.TransferType == ManuSFCFeedingTransferEnum.ResourceFeedingPoint)
            {
                //更新
                command.ResourceId = saveDto.SourceId;
                command.FeedingPointId = 0;
                //添加记录
                manuFeedingEntity.ResourceId = saveDto.LoadPointResoucesId;
                manuFeedingEntity.FeedingPointId = saveDto.DestId;
            }

            await _manuFeedingRepository.UpdateFeedingQtyAsync(command);
            await _manuFeedingRepository.InsertAsync(manuFeedingEntity);

            return true;
        }

        /// <summary>
        /// 上料点条码数量更新
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateManuFeedingBarcodeQtyAsync(UpdateFeedingBarcodeQtyCommand command)
        {
            return await _manuFeedingRepository.UpdateManuFeedingBarcodeQtyAsync(command);
        }

        /// <summary>
        /// 根据资源获取所有的上料信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingEntity>> GetAllByResourceIdAsync(EntityByResourceIdQuery query)
        {
            var dbList = await _manuFeedingRepository.GetAllByResourceIdAsync(query);
            if(dbList == null || dbList.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45075));
            }

            return dbList.ToList();
        }

        /// <summary>
        /// 根据条码获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingEntity>> GetAllBySfcListAsync(GetManuFeedingSfcListQuery query)
        {
            var dbList = await _manuFeedingRepository.GetAllBySfcListAsync(query);
            if(dbList == null || dbList.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45072));
            }
            return dbList;
        }
    }
}
