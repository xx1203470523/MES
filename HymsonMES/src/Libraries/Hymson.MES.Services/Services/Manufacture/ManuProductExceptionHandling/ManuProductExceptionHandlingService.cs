using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务（生产异常处理）
    /// </summary>
    public class ManuProductExceptionHandlingService : IManuProductExceptionHandlingService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<ManuProductExceptionHandlingService> _logger;

        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        ///  仓储（条码）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        ///  仓储（条码信息）
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        ///  仓储（条码在制）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        ///  仓储（物料）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        ///  仓储（工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        ///  仓储（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        ///  仓储（物料台账）
        /// </summary>
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;

        /// <summary>
        /// 不允许的类型
        /// </summary>
        private static readonly SfcStatusEnum[] _noAllowedStatus = new SfcStatusEnum[] {
            SfcStatusEnum.Locked,
            SfcStatusEnum.Scrapping,
            SfcStatusEnum.Delete,
            SfcStatusEnum.Invalid,
            SfcStatusEnum.Detachment
        };

        /// <summary>
        /// 构造函数（生产异常处理）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        public ManuProductExceptionHandlingService(ILogger<ManuProductExceptionHandlingService> logger,
            ICurrentUser currentUser, ICurrentSite currentSite,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcProcedureRepository procProcedureRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository)
        {
            _logger = logger;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
        }


        /// <summary>
        /// 查询条码（离脱）
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<ManuBarCodeDto> GetBarCodeAsync(string barCode)
        {
            if (string.IsNullOrEmpty(barCode)) throw new CustomerValidationException(nameof(ErrorCode.MES15445));

            // 查询条码
            var sfcEntity = await _manuSfcRepository.GetBySFCAsync(new EntityBySFCQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = barCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES15446)).WithData("barCode", barCode);

            // 状态校验
            if (_noAllowedStatus.Contains(sfcEntity.Status)) throw new CustomerValidationException(nameof(ErrorCode.MES15447))
                    .WithData("barCode", barCode)
                    .WithData("status", sfcEntity.Status.GetDescription());

            // 初始化返回值
            var dto = new ManuBarCodeDto
            {
                BarCode = sfcEntity.SFC,
                Qty = sfcEntity.Qty,
                Type = sfcEntity.Type
            };

            // 查询条码信息
            var sfcInfoEntity = await _manuSfcInfoRepository.GetBySFCIdAsync(sfcEntity.Id)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES15446)).WithData("barCode", barCode);

            // 填充工单
            if (sfcInfoEntity.WorkOrderId.HasValue)
            {
                var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(sfcInfoEntity.WorkOrderId.Value);
                if (workOrderEntity != null) dto.WorkOrderCode = workOrderEntity.OrderCode;
            }

            // 填充产品
            var productEntity = await _procMaterialRepository.GetByIdAsync(sfcInfoEntity.ProductId);
            if (productEntity != null)
            {
                dto.ProductCode = productEntity.MaterialCode;
                dto.ProductName = productEntity.MaterialName;
            }

            // 填充工序
            if (sfcEntity.Type == SfcTypeEnum.Produce)
            {
                var sfcProduceEntity = await _manuSfcProduceRepository.GetBySFCIdAsync(sfcEntity.Id);
                if (sfcProduceEntity != null)
                {
                    var procedureEntity = await _procProcedureRepository.GetByIdAsync(sfcProduceEntity.ProcedureId);
                    if (procedureEntity != null)
                    {
                        dto.ProcedureCode = procedureEntity.Code;
                        dto.ProcedureName = procedureEntity.Name;
                    }
                }
            }

            return dto;
        }

        /// <summary>
        /// 提交（离脱）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SubmitDetachmentAsync(ManuDetachmentDto requestDto)
        {
            if (requestDto == null) return 0;
            if (requestDto.BarCodes == null || !requestDto.BarCodes.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15445));

            // 查询条码
            var sfcEntities = await _manuSfcRepository.GetAllBySFCsAsync(new EntityBySFCsQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFCs = requestDto.BarCodes
            });
            if (sfcEntities == null || !sfcEntities.Any()) return 0;

            // 状态校验
            var validationFailures = new List<ValidationFailure>();
            var noMatchSFCEntities = sfcEntities.Where(w => _noAllowedStatus.Contains(w.Status));
            if (noMatchSFCEntities.Any())
            {
                foreach (var sfcEntity in noMatchSFCEntities)
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("barCode", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("status", sfcEntity.Status.GetDescription());
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15447);
                    validationFailures.Add(validationFailure);
                }

                if (validationFailures.Any()) throw new ValidationException("", validationFailures);
            }

            // 更新时间
            var siteId = _currentSite.SiteId ?? 0;
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 查询条码信息
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcEntities.Select(s => s.Id));

            // 读取在制品
            var sfcProduceEntities = await _manuSfcProduceRepository.GetBySFCIdsAsync(sfcEntities.Where(w => w.Type == SfcTypeEnum.Produce).Select(s => s.Id));

            // 读取产品
            var productEntities = await _procMaterialRepository.GetByIdsAsync(sfcInfoEntities.Select(s => s.ProductId));

            // 遍历所有条码
            List<ManuSfcEntity> updateManuSfcEntities = new();
            List<DeletePhysicalBySfcCommand> deletePhysicalBySfcCommands = new();
            List<ManuSfcStepEntity> sfcStepEntities = new();
            List<WhMaterialStandingbookEntity> materialStandingbookEntities = new();

            PhysicalDeleteSFCProduceByIdsCommand physicalDeleteSFCProduceByIdsCommand = new() { SiteId = siteId };
            UpdateWhMaterialInventoryEmptyCommand updateWhMaterialInventoryEmptyCommand = new()
            {
                SiteId = siteId,
                UserName = updatedBy,
                UpdateTime = updatedOn
            };

            List<long> sfcProduceIds = new();
            List<string> sfcInventorys = new();
            foreach (var sfcEntity in sfcEntities)
            {
                if (sfcEntity.Type == SfcTypeEnum.Produce)
                {
                    // 在制品
                    var sfcProduceEntity = sfcProduceEntities.FirstOrDefault(f => f.SFCId == sfcEntity.Id);
                    if (sfcProduceEntity == null) continue;

                    // 删除在制；
                    sfcProduceIds.Add(sfcProduceEntity.Id);

                    // 添加步骤；
                    sfcStepEntities.Add(new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        Operatetype = ManuSfcStepTypeEnum.Detachment,
                        CurrentStatus = sfcProduceEntity.Status,
                        SFC = sfcProduceEntity.SFC,
                        ProductId = sfcProduceEntity.ProductId,
                        WorkOrderId = sfcProduceEntity.WorkOrderId,
                        WorkCenterId = sfcProduceEntity.WorkCenterId,
                        ProductBOMId = sfcProduceEntity.ProductBOMId,
                        SFCInfoId = sfcProduceEntity.BarCodeInfoId,
                        Qty = sfcProduceEntity.Qty,
                        VehicleCode = "", // 这里要赋值？
                        ProcedureId = sfcProduceEntity.ProcedureId,
                        ResourceId = sfcProduceEntity.ResourceId,
                        EquipmentId = sfcProduceEntity.EquipmentId,
                        SiteId = sfcEntity.SiteId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    });
                }
                else if (sfcEntity.Type == SfcTypeEnum.NoProduce)
                {
                    // 条码信息
                    var sfcInfoEntity = sfcInfoEntities.FirstOrDefault(f => f.SfcId == sfcEntity.Id);
                    if (sfcInfoEntity == null) continue;

                    // 产品信息
                    var productEntity = productEntities.FirstOrDefault(f => f.Id == sfcInfoEntity.ProductId);
                    if (productEntity == null) continue;

                    // 添加步骤；
                    sfcStepEntities.Add(new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        Operatetype = ManuSfcStepTypeEnum.Detachment,
                        CurrentStatus = SfcStatusEnum.Complete,
                        SFC = sfcEntity.SFC,
                        ProductId = sfcInfoEntity.ProductId,
                        WorkOrderId = sfcInfoEntity.WorkOrderId ?? 0,
                        SFCInfoId = sfcInfoEntity.Id,
                        Qty = sfcEntity.Qty,
                        SiteId = sfcEntity.SiteId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    });

                    // 清零库存；
                    sfcInventorys.Add(sfcEntity.SFC);

                    // 记录台账；
                    materialStandingbookEntities.Add(new WhMaterialStandingbookEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        MaterialCode = productEntity.MaterialCode,
                        MaterialName = productEntity.MaterialName,
                        MaterialVersion = productEntity.Version ?? "",
                        MaterialBarCode = sfcEntity.SFC,
                        Batch = "",//自制品 没有
                        Quantity = sfcEntity.Qty,
                        Unit = productEntity.Unit ?? "",
                        Type = WhMaterialInventoryTypeEnum.Detachment,
                        Source = MaterialInventorySourceEnum.Detachment,
                        SiteId = sfcEntity.SiteId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    });
                }
                else continue;

                // 条码表改状态；
                sfcEntity.Status = SfcStatusEnum.Detachment;
                sfcEntity.UpdatedBy = updatedBy;
                sfcEntity.UpdatedOn = updatedOn;

                updateManuSfcEntities.Add(sfcEntity);
            }

            // 需要删除的条码ID
            physicalDeleteSFCProduceByIdsCommand.Ids = sfcProduceIds;
            updateWhMaterialInventoryEmptyCommand.BarCodeList = sfcInventorys;

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();

            // 删除在制
            rows += await _manuSfcProduceRepository.DeletePhysicalRangeByIdsAsync(physicalDeleteSFCProduceByIdsCommand);

            // 插入 manu_sfc_step
            rows += await _manuSfcStepRepository.InsertRangeAsync(sfcStepEntities);

            // 清零库存
            rows += await _whMaterialInventoryRepository.UpdateWhMaterialInventoryEmptyByBarCodeAync(updateWhMaterialInventoryEmptyCommand);

            // 台账
            rows += await _whMaterialStandingbookRepository.InsertsAsync(materialStandingbookEntities);

            // manu_sfc 更新状态
            rows += await _manuSfcRepository.UpdateRangeWithStatusCheckAsync(updateManuSfcEntities);

            // 删除工单统计条码


            trans.Complete();
            return rows;
        }


    }
}
