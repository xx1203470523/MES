using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Process;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.View;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCommonDto;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.MES.Services.Services.Manufacture.ManuSfcProduce;
using Hymson.MES.Services.Services.Warehouse;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using IdGen;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 条码生产信息（物理删除） 服务
    /// </summary>
    public class ManuSfcProduceService : IManuSfcProduceService
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
        /// 条码生产信息（物理删除） 仓储
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 资源仓储
        /// </summary>
        private readonly IProcResourceRepository _resourceRepository;

        /// <summary>
        /// 条码步骤表仓储 仓储
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 条码信息表 仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 工单激活 仓储
        /// </summary>
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IManuCommonOldService _manuCommonOldService;

        /// <summary>
        /// 工单信息表 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 工序仓储
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        ///工艺路线工序节点明细仓储
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;

        /// <summary>
        /// 容器装载表（物理删除）仓储
        /// </summary>
        private readonly IManuContainerPackRepository _manuContainerPackRepository;

        /// <summary>
        /// 条码信息表  仓储
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 物料库存 仓储
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 物料库存 服务
        /// </summary>
        private readonly IWhMaterialInventoryService _whMaterialInventoryService;

        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（工艺路线工序连线）
        /// </summary>
        private readonly IProcProcessRouteDetailLinkRepository _procProcessRouteDetailLinkRepository;

        /// <summary>
        /// 物料台账 仓储
        /// </summary>
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;

        /// <summary>
        /// 工艺路线表 仓储
        /// </summary>
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;

        /// <summary>
        /// BOM表 仓储
        /// </summary>
        private readonly IProcBomRepository _procBomRepository;

        /// <summary>
        /// 仓储（工艺路线节点）
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteNodeRepository;

        private readonly AbstractValidator<ManuSfcProduceLockDto> _validationLockRules;
        private readonly AbstractValidator<ManuSfcProduceModifyDto> _validationModifyRules;
        private readonly ILogger<ManuSfcProduceService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="resourceRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="planWorkOrderActivationRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuCommonOldService"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="localizationService"></param>
        /// <param name="manuContainerPackRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialInventoryService"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        /// <param name="procProcessRouteRepository"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="validationLockRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="logger"></param>
        public ManuSfcProduceService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IProcResourceRepository resourceRepository,
            IManuSfcRepository manuSfcRepository,
            IPlanWorkOrderActivationRepository planWorkOrderActivationRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuCommonService manuCommonService,
            IManuCommonOldService manuCommonOldService,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcedureRepository procProcedureRepository,
            ILocalizationService localizationService,
            IManuContainerPackRepository manuContainerPackRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialInventoryService whMaterialInventoryService,
            IProcMaterialRepository procMaterialRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
            IProcProcessRouteRepository procProcessRouteRepository,
            IProcBomRepository procBomRepository,
            AbstractValidator<ManuSfcProduceLockDto> validationLockRules,
            AbstractValidator<ManuSfcProduceModifyDto> validationModifyRules,
            ILogger<ManuSfcProduceService> logger, IProcProcessRouteDetailNodeRepository procProcessRouteNodeRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _resourceRepository = resourceRepository;
            _manuSfcRepository = manuSfcRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuCommonService = manuCommonService;
            _manuCommonOldService = manuCommonOldService;
            _procProcedureRepository = procProcedureRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _localizationService = localizationService;
            _manuContainerPackRepository = manuContainerPackRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialInventoryService = whMaterialInventoryService;
            _procMaterialRepository = procMaterialRepository;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _procBomRepository = procBomRepository;
            _validationLockRules = validationLockRules;
            _validationModifyRules = validationModifyRules;
            this._logger = logger;
            _procProcessRouteNodeRepository = procProcessRouteNodeRepository;
        }


        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuSfcProducePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcProduceViewDto>> GetPageListAsync(ManuSfcProducePagedQueryDto manuSfcProducePagedQueryDto)
        {
            var manuSfcProducePagedQuery = manuSfcProducePagedQueryDto.ToQuery<ManuSfcProducePagedQuery>();
            manuSfcProducePagedQuery.SiteId = _currentSite.SiteId;

            //查询多个条码
            if (manuSfcProducePagedQueryDto.Sfcs != null && manuSfcProducePagedQueryDto.Sfcs.Any())
            {
                manuSfcProducePagedQuery.SfcArray = manuSfcProducePagedQueryDto.Sfcs;
            }

            //根据资源查询
            if (manuSfcProducePagedQueryDto.ResourceId.HasValue)
            {
                var resource = await _resourceRepository.GetByIdAsync(manuSfcProducePagedQueryDto.ResourceId.Value);
                manuSfcProducePagedQuery.ResourceTypeId = resource.ResTypeId;
            }

            var pagedInfo = await _manuSfcProduceRepository.GetPagedInfoAsync(manuSfcProducePagedQuery);

            //实体到DTO转换 装载数据
            List<ManuSfcProduceViewDto> manuSfcProduceDtos = new List<ManuSfcProduceViewDto>();
            foreach (var item in pagedInfo.Data)
            {
                manuSfcProduceDtos.Add(new ManuSfcProduceViewDto
                {
                    Id = item.Id,
                    Sfc = item.Sfc,
                    //Lock = item.Lock,
                    //LockProductionId = item.LockProductionId,
                    ProcessRouteId = item.ProcessRouteId,
                    ProductBOMId = item.ProductBOMId,
                    ProcedureId = item.ProcedureId,
                    Status = item.Status,
                    OrderCode = item.OrderCode,
                    Code = item.Code,
                    Name = item.Name,
                    MaterialCode = item.MaterialCode,
                    MaterialName = item.MaterialName,
                    Version = item.Version,
                    IsScrap = item.IsScrap,
                    ResCode = item.ResCode
                });
            }
            return new PagedInfo<ManuSfcProduceViewDto>(manuSfcProduceDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        #region 质量锁定
        /// <summary>
        /// 质量锁定
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task QualityLockAsync(ManuSfcProduceLockDto parm)
        {
            if (parm == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            await _validationLockRules.ValidateAndThrowAsync(parm);

            switch (parm.OperationType)
            {
                case
                    QualityLockEnum.FutureLock:
                    await FutureLockAsync(new FutureLockDto
                    {
                        Sfcs = parm.Sfcs,
                        LockProductionId = parm.LockProductionId ?? 0
                    });
                    break;
                case
                    QualityLockEnum.InstantLock:
                    await InstantLockAsync(new InstantLockDto
                    {
                        Sfcs = parm.Sfcs,
                    });
                    break;
                case
                    QualityLockEnum.Unlock:
                    await UnLockAsync(new UnLockDto
                    {
                        Sfcs = parm.Sfcs,
                    });
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 将来锁
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private async Task FutureLockAsync(FutureLockDto parm)
        {
            var sfcListTask = _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(
           new ManuSfcProduceQuery
           {
               Sfcs = parm.Sfcs.Distinct().ToArray(),
               SiteId = _currentSite.SiteId ?? 0
           });

            var sfcProduceBusinesssListTask = _manuSfcProduceRepository.GetSfcProduceBusinessListBySFCAsync(new SfcListProduceBusinessQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = parm.Sfcs, BusinessType = ManuSfcProduceBusinessType.Lock });
            var sfcList = await sfcListTask;
            var sfcProduceBusinesssList = await sfcProduceBusinesssListTask;

            if (sfcListTask == null || !sfcList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15309));
            }

            var workOrders = sfcList.Select(x => x.WorkOrderId).Distinct().ToList();
            if (workOrders.Count > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15308));
            }

            //验证工艺路线
            await VeifyQualityLockProductionAsync(sfcList.ToArray(), parm.LockProductionId);

            var validationFailures = new List<ValidationFailure>();
            foreach (var sfc in parm.Sfcs)
            {
                var sfcEntity = sfcList.FirstOrDefault(x => x.SFC == sfc);
                //是否是在制品
                if (sfcEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15312);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                var sfcProduceBusinessEntity = sfcProduceBusinesssList.FirstOrDefault(x => x.SfcProduceId == sfcEntity.Id);

                //是否被锁定
                if (sfcEntity.Status == SfcProduceStatusEnum.Locked)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }

                    validationFailure.ErrorCode = nameof(ErrorCode.MES15318);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                //是否存在将来锁
                if (sfcProduceBusinessEntity != null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", sfc } };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }

                    validationFailure.ErrorCode = nameof(ErrorCode.MES15313);
                    validationFailures.Add(validationFailure);
                }

                //验证将来锁工序是否在条码所在工序之后
                if (await _manuCommonOldService.IsProcessStartBeforeEndAsync(sfcEntity.ProcessRouteId, sfcEntity.ProcedureId, parm.LockProductionId))
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", sfc } };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }
                    validationFailure.FormattedMessagePlaceholderValues.Add("sfcproduction", sfc);
                    validationFailure.FormattedMessagePlaceholderValues.Add("lockproductionname", sfc);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15313);
                    validationFailures.Add(validationFailure);
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }

            #region  组装数据
            var sfcStepList = new List<ManuSfcStepEntity>();
            var sfcProduceBusinessList = new List<ManuSfcProduceBusinessEntity>();
            var sfcStepBusinessList = new List<MaunSfcStepBusinessEntity>();
            foreach (var sfc in sfcList)
            {
                SfcProduceLockBo sfcProduceLockBo = new SfcProduceLockBo()
                {
                    Lock = QualityLockEnum.FutureLock,
                    LockProductionId = parm.LockProductionId
                };

                var stepEntity = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = sfc.SFC,
                    ProductId = sfc.ProductId,
                    WorkOrderId = sfc.WorkOrderId,
                    WorkCenterId = sfc.WorkCenterId,
                    ProductBOMId = sfc.ProductBOMId,
                    Qty = sfc.Qty,
                    EquipmentId = sfc.EquipmentId,
                    ResourceId = sfc.ResourceId,
                    ProcedureId = sfc.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.FutureLock,
                    CurrentStatus = sfc.Status,
                    CreatedBy = sfc.CreatedBy,
                    UpdatedBy = sfc.UpdatedBy
                };
                sfcStepList.Add(stepEntity);

                sfcProduceBusinessList.Add(new ManuSfcProduceBusinessEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SfcProduceId = sfc.Id,
                    BusinessType = ManuSfcProduceBusinessType.Lock,
                    BusinessContent = JsonSerializer.Serialize(sfcProduceLockBo),
                    CreatedBy = sfc.CreatedBy,
                    UpdatedBy = sfc.UpdatedBy
                });

                sfcStepBusinessList.Add(new MaunSfcStepBusinessEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SfcStepId = stepEntity.Id,
                    BusinessType = ManuSfcProduceBusinessType.Lock,
                    BusinessContent = JsonSerializer.Serialize(sfcProduceLockBo),
                    CreatedBy = sfc.CreatedBy,
                    UpdatedBy = sfc.UpdatedBy
                });
            }
            #endregion

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                if (sfcProduceBusinessList != null && sfcProduceBusinessList.Any())
                {
                    //插入业务表
                    await _manuSfcProduceRepository.InsertOrUpdateSfcProduceBusinessRangeAsync(sfcProduceBusinessList);
                }

                //插入操作数据
                await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);

                //插入步骤业务表
                await _manuSfcStepRepository.InsertSfcStepBusinessRangeAsync(sfcStepBusinessList);

                trans.Complete();
            }
        }

        /// <summary>
        /// 及时锁
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private async Task InstantLockAsync(InstantLockDto parm)
        {
            var sfcListTask = _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(
            new ManuSfcProduceQuery
            {
                Sfcs = parm.Sfcs.Distinct().ToArray(),
                SiteId = _currentSite.SiteId ?? 00
            });

            var sfcProduceBusinesssListTask = _manuSfcProduceRepository.GetSfcProduceBusinessListBySFCAsync(new SfcListProduceBusinessQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = parm.Sfcs, BusinessType = ManuSfcProduceBusinessType.Lock });
            var sfcList = await sfcListTask;
            var sfcProduceBusinesssList = await sfcProduceBusinesssListTask;

            if (sfcListTask == null || !sfcList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15309));
            }
            var validationFailures = new List<ValidationFailure>();
            foreach (var sfc in parm.Sfcs)
            {
                var sfcEntity = sfcList.FirstOrDefault(x => x.SFC == sfc);
                //是否是在制品
                if (sfcEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15312);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                //是否被锁定
                if (sfcEntity.Status == SfcProduceStatusEnum.Locked)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15318);
                    validationFailures.Add(validationFailure);
                    continue;
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }

            #region  组装数据
            var sfcStepList = new List<ManuSfcStepEntity>();
            var sfcProduceBusinessList = new List<ManuSfcProduceBusinessEntity>();
            var sfcStepBusinessList = new List<MaunSfcStepBusinessEntity>();
            foreach (var sfc in sfcList)
            {
                SfcProduceLockBo sfcProduceLockBo = new SfcProduceLockBo()
                {
                    Lock = QualityLockEnum.InstantLock
                };

                var stepEntity = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = sfc.SFC,
                    ProductId = sfc.ProductId,
                    WorkOrderId = sfc.WorkOrderId,
                    WorkCenterId = sfc.WorkCenterId,
                    ProductBOMId = sfc.ProductBOMId,
                    Qty = sfc.Qty,
                    EquipmentId = sfc.EquipmentId,
                    ResourceId = sfc.ResourceId,
                    ProcedureId = sfc.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.InstantLock,
                    CurrentStatus = sfc.Status,
                    CreatedBy = sfc.CreatedBy,
                    UpdatedBy = sfc.UpdatedBy
                };
                sfcStepList.Add(stepEntity);

                sfcProduceBusinessList.Add(new ManuSfcProduceBusinessEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SfcProduceId = sfc.Id,
                    BusinessType = ManuSfcProduceBusinessType.Lock,
                    BusinessContent = JsonSerializer.Serialize(sfcProduceLockBo),
                    CreatedBy = sfc.CreatedBy,
                    UpdatedBy = sfc.UpdatedBy
                });

                sfcStepBusinessList.Add(new MaunSfcStepBusinessEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SfcStepId = stepEntity.Id,
                    BusinessType = ManuSfcProduceBusinessType.Lock,
                    BusinessContent = JsonSerializer.Serialize(sfcProduceLockBo),
                    CreatedBy = sfc.CreatedBy,
                    UpdatedBy = sfc.UpdatedBy
                });
            }
            #endregion

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                if (sfcProduceBusinessList != null && sfcProduceBusinessList.Any())
                {
                    //插入业务表
                    await _manuSfcProduceRepository.InsertOrUpdateSfcProduceBusinessRangeAsync(sfcProduceBusinessList);
                }

                //插入操作数据
                await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);

                //插入步骤业务表
                await _manuSfcStepRepository.InsertSfcStepBusinessRangeAsync(sfcStepBusinessList);

                await _manuSfcProduceRepository.LockedSfcProcedureAsync(new LockedProcedureCommand
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Sfcs = parm.Sfcs,
                    UserId = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    Status = SfcProduceStatusEnum.Locked
                });
                trans.Complete();
            }
        }

        /// <summary>
        /// 解除锁
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private async Task UnLockAsync(UnLockDto parm)
        {
            var sfcListTask = _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(
         new ManuSfcProduceQuery
         {
             Sfcs = parm.Sfcs.Distinct().ToArray(),
             SiteId = _currentSite.SiteId ?? 00
         });

            var sfcProduceBusinesssListTask = _manuSfcProduceRepository.GetSfcProduceBusinessListBySFCAsync(new SfcListProduceBusinessQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = parm.Sfcs, BusinessType = ManuSfcProduceBusinessType.Lock });
            var sfcList = await sfcListTask;
            var sfcProduceBusinesssList = await sfcProduceBusinesssListTask;

            if (sfcListTask == null || !sfcList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15309));
            }
            var validationFailures = new List<ValidationFailure>();
            foreach (var sfc in parm.Sfcs)
            {
                var sfcEntity = sfcList.FirstOrDefault(x => x.SFC == sfc);
                //是否是在制品
                if (sfcEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15312);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                var sfcProduceBusinessEntity = sfcProduceBusinesssList.FirstOrDefault(x => x.SfcProduceId == sfcEntity.Id);
                //是否被锁定或者存在将来锁
                if (sfcProduceBusinessEntity == null && sfcEntity.Status != SfcProduceStatusEnum.Locked)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", sfc } };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15316);
                    validationFailures.Add(validationFailure);
                    continue;
                }
            }
            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }

            /* 1.即时锁定：将条码更新为“锁定”状态；
            2.将来锁定：保存列表中的条码信息，及指定锁定的工序，供条码过站校验时调用；
            3.取消锁定：产品条码已经是锁定状态：将条码更新到锁定前状态
           指定将来锁定工序，且条码还没流转到指定工序：关闭将来锁定的工序指定，即取消将来锁定*/
            var sfcStepList = new List<ManuSfcStepEntity>();
            var sfcProduceBusinessList = new List<ManuSfcProduceBusinessEntity>();
            var unLockList = new List<long>();
            var sfcStepBusinessList = new List<MaunSfcStepBusinessEntity>();
            #region  组装数据
            foreach (var sfc in sfcList)
            {
                var stepEntity = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = sfc.SFC,
                    ProductId = sfc.ProductId,
                    WorkOrderId = sfc.WorkOrderId,
                    WorkCenterId = sfc.WorkCenterId,
                    ProductBOMId = sfc.ProductBOMId,
                    Qty = sfc.Qty,
                    EquipmentId = sfc.EquipmentId,
                    ResourceId = sfc.ResourceId,
                    ProcedureId = sfc.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.Unlock,
                    CurrentStatus = sfc.Status,
                    CreatedBy = sfc.CreatedBy,
                    UpdatedBy = sfc.UpdatedBy
                };
                sfcStepList.Add(stepEntity);
                unLockList.Add(sfc.Id);
            }
            #endregion


            var lockSfc = sfcList.Where(x => x.Status == SfcProduceStatusEnum.Locked).Select(x => x.SFC).ToArray();

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                if (unLockList != null && unLockList.Any())
                {
                    await _manuSfcProduceRepository.DeleteSfcProduceBusinesssAsync(new DeleteSfcProduceBusinesssCommand { SfcInfoIds = unLockList, BusinessType = ManuSfcProduceBusinessType.Lock });
                }

                await _manuSfcProduceRepository.UnLockedSfcProcedureAsync(new UnLockedProcedureCommand
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Sfcs = lockSfc,
                    UserId = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                });

                //插入操作数据
                await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);
                trans.Complete();
            }
        }
        #endregion

        /// <summary>
        /// 条码报废
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task QualityScrapAsync(ManuSfScrapDto parm)
        {
            #region 验证
            if (parm == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            if (parm.Sfcs == null || parm.Sfcs.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15400));
            }

            var sfc = parm.Sfcs.Distinct();
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = parm.Sfcs, SiteId = _currentSite.SiteId ?? 00 };
            //获取条码列表
            var manuSfcs = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);
            if (!manuSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15402));
            }

            var scrapSfcs = manuSfcs.Where(x => x.IsScrap == TrueOrFalseEnum.Yes).Select(x => x.SFC).ToArray();
            //类型为报废时判断条码是否已经报废,若已经报废提示:存在已报废的条码，不可再次报废
            if (scrapSfcs.Any())
            {
                var strs = string.Join("','", scrapSfcs);
                throw new CustomerValidationException(nameof(ErrorCode.MES15401));
            }

            await VerifySfcsLockAsync(manuSfcs.ToArray());
            #endregion

            #region  组装数据
            var sfcStepList = GetSfcStepList(manuSfcs, parm.Remark ?? "", ManuSfcStepTypeEnum.Discard);

            var sfcs = manuSfcs.Select(a => a.SFC).ToArray();
            var isScrapCommand = new UpdateIsScrapCommand
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfcs = sfcs,
                UserId = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                IsScrap = TrueOrFalseEnum.Yes,
                CurrentIsScrap = TrueOrFalseEnum.No
            };
            var manuSfcInfoUpdate = new ManuSfcUpdateCommand
            {
                Sfcs = sfcs,
                UserId = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                Status = SfcStatusEnum.Scrapping
            };
            #endregion

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //1.插入数据操作类型为报废
                rows += await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);

                //2.修改在制品表,IsScrap
                rows += await _manuSfcProduceRepository.UpdateIsScrapAsync(isScrapCommand);

                //3.条码信息表
                rows += await _manuSfcRepository.UpdateStatusAsync(manuSfcInfoUpdate);
                trans.Complete();
            }
        }

        /// <summary>
        /// 条码取消报废
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task QualityCancelScrapAsync(ManuSfScrapDto parm)
        {
            #region 验证
            if (parm == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            if (parm.Sfcs == null || parm.Sfcs.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15400));
            }

            var sfc = parm.Sfcs.Distinct();
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = parm.Sfcs, SiteId = _currentSite.SiteId ?? 00 };
            //获取条码列表
            var manuSfcs = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);
            if (!manuSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15402));
            }

            var noScrapSfcs = manuSfcs.Where(x => x.IsScrap == TrueOrFalseEnum.No).Select(x => x.SFC).ToArray();
            if (noScrapSfcs.Any())
            {
                var strs = string.Join("','", noScrapSfcs);
                throw new CustomerValidationException(nameof(ErrorCode.MES15403)).WithData("sfcs", strs);
            }

            //await VerifySfcsLockAsync(manuSfcs.ToArray());

            //取消报废， 验证工单是否已经激活，若已经取消激活，不能取消报废条码
            var orderIds = manuSfcs.Select(x => x.WorkOrderId).Distinct().ToArray();
            //var activeOrders = await _planWorkOrderActivationRepository.GetByWorkOrderIdsAsync(orderIds);
            //if (!activeOrders.Any())
            //{
            //    var orders = await _planWorkOrderRepository.GetByIdsAsync(orderIds);
            //    var orderCodes = orders.Select(x => x.OrderCode).ToArray();
            //    throw new CustomerValidationException(nameof(ErrorCode.MES15404)).WithData("orders", string.Join("','", orderCodes));
            //}

            //var activeOrderList = activeOrders.ToList();
            //if (activeOrderList.Count < orderIds.Length)
            //{
            //    var activeOrderIds = activeOrderList.Select(x => x.WorkOrderId).ToArray();
            //    //找出相同元素(即交集)
            //    var diffOrderIds = orderIds.Where(c => !activeOrderIds.Contains(c)).ToArray();
            //    var orders = await _planWorkOrderRepository.GetByIdsAsync(diffOrderIds);
            //    var orderCodesStr = string.Join(",", orders.Select(x => x.OrderCode).ToArray());
            //    throw new CustomerValidationException(nameof(ErrorCode.MES15404)).WithData("orders", orderCodesStr);
            //}
            #endregion

            #region  组装数据
            var sfcStepList = GetSfcStepList(manuSfcs, parm.Remark ?? "", ManuSfcStepTypeEnum.CancelDiscard);

            var sfcs = manuSfcs.Select(a => a.SFC).ToArray();
            //把更改前的状态作为状态传入
            var isScrapCommand = new UpdateIsScrapCommand
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfcs = sfcs,
                UserId = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                IsScrap = TrueOrFalseEnum.No,
                CurrentIsScrap = TrueOrFalseEnum.Yes
            };

            var manuSfcInfoUpdate = new ManuSfcUpdateCommand
            {
                Sfcs = sfcs,
                UserId = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                Status = SfcStatusEnum.InProcess
            };
            #endregion

            //入库
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //1.修改在制品表,IsScrap
                rows += await _manuSfcProduceRepository.UpdateIsScrapAsync(isScrapCommand);
                if (rows < sfcs.Count())
                {
                    //报错
                    throw new CustomerValidationException(nameof(ErrorCode.MES15412)).WithData("sfcs", string.Join("','", sfcs));
                }

                //2.条码信息表状态更改
                rows += await _manuSfcRepository.UpdateStatusAsync(manuSfcInfoUpdate);

                //3.插入数据操作类型为取消报废
                rows += await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);

                trans.Complete();
            }
        }

        /// <summary>
        /// 获取条码步骤数据
        /// </summary>
        /// <param name="manuSfcs"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        private List<ManuSfcStepEntity> GetSfcStepList(IEnumerable<ManuSfcProduceEntity> manuSfcs, string remark, ManuSfcStepTypeEnum type)
        {
            var sfcStepList = new List<ManuSfcStepEntity>();
            foreach (var sfc in manuSfcs)
            {
                sfcStepList.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfc.SFC,
                    ProductId = sfc.ProductId,
                    WorkOrderId = sfc.WorkOrderId,
                    WorkCenterId = sfc.WorkCenterId,
                    ProductBOMId = sfc.ProductBOMId,
                    Qty = sfc.Qty,
                    EquipmentId = sfc.EquipmentId,
                    ResourceId = sfc.ResourceId,
                    ProcedureId = sfc.ProcedureId,
                    Operatetype = type,
                    CurrentStatus = sfc.Status,
                    //Lock = sfc.Lock,
                    Remark = remark,
                    SiteId = _currentSite.SiteId ?? 0,
                    CreatedBy = sfc.CreatedBy,
                    UpdatedBy = sfc.UpdatedBy
                });
            }
            return sfcStepList;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuSfcProduceCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuSfcProduceAsync(ManuSfcProduceCreateDto manuSfcProduceCreateDto)
        {
            //验证DTO
            //await _validationCreateRules.ValidateAndThrowAsync(manuSfcProduceCreateDto);

            //DTO转换实体
            var manuSfcProduceEntity = manuSfcProduceCreateDto.ToEntity<ManuSfcProduceEntity>();
            manuSfcProduceEntity.Id = IdGenProvider.Instance.CreateId();
            manuSfcProduceEntity.CreatedBy = _currentUser.UserName;
            manuSfcProduceEntity.UpdatedBy = _currentUser.UserName;

            //入库
            await _manuSfcProduceRepository.InsertAsync(manuSfcProduceEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuSfcProduceAsync(long id)
        {
            await _manuSfcProduceRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuSfcProduceAsync(string ids)
        {
            var idsArr = ids.ToSpitLongArray();
            return await _manuSfcProduceRepository.DeleteRangeAsync(idsArr);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuSfcProduceModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyManuSfcProduceAsync(ManuSfcProduceModifyDto manuSfcProduceModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuSfcProduceModifyDto);

            //DTO转换实体
            var manuSfcProduceEntity = manuSfcProduceModifyDto.ToEntity<ManuSfcProduceEntity>();
            manuSfcProduceEntity.UpdatedBy = _currentUser.UserName;

            await _manuSfcProduceRepository.UpdateAsync(manuSfcProduceEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcProduceDto> QueryManuSfcProduceByIdAsync(long id)
        {
            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetByIdAsync(id);
            if (manuSfcProduceEntity == null)
            {
                return null;
            }
            return manuSfcProduceEntity.ToModel<ManuSfcProduceDto>();
        }

        /// <summary>
        /// 根据SFC查询
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<ManuSfcProduceDto> QueryManuSfcProduceBySFCAsync(string sfc)
        {
            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfc = sfc
            });
            if (manuSfcProduceEntity == null)
            {
                return null;
            }
            return manuSfcProduceEntity.ToModel<ManuSfcProduceDto>();
        }


        #region 在制品步骤控制
        /// <summary>
        /// 分页查询（查询所有条码信息）
        /// </summary>
        /// <param name="manuSfcProducePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcProduceViewDto>> GetManuSfcPagedInfoAsync(ManuSfcProducePagedQueryDto manuSfcProducePagedQueryDto)
        {
            var manuSfcProducePagedQuery = manuSfcProducePagedQueryDto.ToQuery<ManuSfcProducePagedQuery>();
            manuSfcProducePagedQuery.SiteId = _currentSite.SiteId;

            //查询多个条码
            if (manuSfcProducePagedQueryDto.Sfcs != null && manuSfcProducePagedQueryDto.Sfcs.Any())
            {
                manuSfcProducePagedQuery.SfcArray = manuSfcProducePagedQueryDto.Sfcs;
            }

            //根据资源查询
            if (manuSfcProducePagedQueryDto.ResourceId.HasValue)
            {
                var resource = await _resourceRepository.GetByIdAsync(manuSfcProducePagedQueryDto.ResourceId.Value);
                manuSfcProducePagedQuery.ResourceTypeId = resource.ResTypeId;
            }

            var pagedInfo = await _manuSfcRepository.GetManuSfcPagedInfoAsync(manuSfcProducePagedQuery);
            if (pagedInfo == null || !pagedInfo.Data.Any())
            {
                if (manuSfcProducePagedQuery.SfcArray != null && manuSfcProducePagedQuery.SfcArray.Length > 0)
                    throw new CustomerValidationException(nameof(ErrorCode.MES18022)).WithData("SFC", string.Join(",", manuSfcProducePagedQuery.SfcArray));
            }

            //实体到DTO转换 装载数据
            List<ManuSfcProduceViewDto> manuSfcProduceDtos = new List<ManuSfcProduceViewDto>();
            foreach (var item in pagedInfo.Data)
            {
                manuSfcProduceDtos.Add(new ManuSfcProduceViewDto
                {
                    Id = item.Id,
                    Sfc = item.Sfc,
                    Lock = item.Lock,
                    LockProductionId = item.LockProductionId,
                    ProductBOMId = item.ProductBOMId,
                    ProcedureId = item.ProcedureId,
                    Status = item.Status,
                    OrderCode = item.OrderCode,
                    Code = item.Code,
                    Name = item.Name,
                    MaterialCode = item.MaterialCode,
                    MaterialName = item.MaterialName,
                    Version = item.Version
                });
            }
            return new PagedInfo<ManuSfcProduceViewDto>(manuSfcProduceDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 分页查询（查询所有条码信息）
        /// 优化
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcProduceSelectViewDto>> GetManuSfcSelectPagedInfoAsync(ManuSfcProduceSelectPagedQueryDto queryDto)
        {
            var pagedQuery = queryDto.ToQuery<ManuSfcProduceSelectPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;

            //查询多个条码
            if (queryDto.Sfcs != null && queryDto.Sfcs.Any())
            {
                pagedQuery.SfcArray = queryDto.Sfcs;
            }

            var pagedInfo = await _manuSfcRepository.GetManuSfcSelectPagedInfoAsync(pagedQuery);
            if (pagedInfo == null || !pagedInfo.Data.Any())
            {
                if (pagedQuery.SfcArray != null && pagedQuery.SfcArray.Length > 0)
                    throw new CustomerValidationException(nameof(ErrorCode.MES18022)).WithData("SFC", string.Join(",", pagedQuery.SfcArray));
            }

            //实体到DTO转换 装载数据
            List<ManuSfcProduceSelectViewDto> manuSfcProduceDtos = new List<ManuSfcProduceSelectViewDto>();

            if (pagedInfo != null && pagedInfo.Data.Any())
            {
                //查询工单
                var workOrders = await _planWorkOrderRepository.GetByIdsAsync(pagedInfo.Data.Where(x => x.WorkOrderId > 0).Select(x => x.WorkOrderId).ToArray());

                //查询工序
                var procedures = await _procProcedureRepository.GetByIdsAsync(pagedInfo.Data.Where(x => x.ProcedureId > 0).Select(x => x.ProcedureId).ToArray());

                //查询资源
                var resources = await _resourceRepository.GetListByIdsAsync(pagedInfo.Data.Where(x => x.ResourceId > 0).Select(x => x.ResourceId).ToArray());

                //查询物料
                var materials = await _procMaterialRepository.GetByIdsAsync(pagedInfo.Data.Where(x => x.ProductId > 0).Select(x => x.ProductId).ToArray());

                foreach (var item in pagedInfo.Data)
                {
                    var workOrder = workOrders != null && workOrders.Any() ? workOrders.FirstOrDefault(x => x.Id == item.WorkOrderId) : null;
                    var procedure = procedures != null && procedures.Any() ? procedures.FirstOrDefault(x => x.Id == item.ProcedureId) : null;
                    var resource = resources != null && resources.Any() ? resources.FirstOrDefault(x => x.Id == item.ResourceId) : null;
                    var material = materials != null && materials.Any() ? materials.FirstOrDefault(x => x.Id == item.ProductId) : null;

                    manuSfcProduceDtos.Add(new ManuSfcProduceSelectViewDto
                    {
                        Id = item.Id,
                        Sfc = item.Sfc,
                        Lock = item.Lock,
                        LockProductionId = item.LockProductionId,
                        ProductBOMId = item.ProductBOMId,
                        ProcedureId = item.ProcedureId,
                        Status = item.Status,
                        OrderCode = workOrder != null ? workOrder.OrderCode : "",
                        Code = procedure != null ? procedure.Code : "",
                        Name = procedure != null ? procedure.Name : "",
                        ResCode = resource != null ? resource.ResCode : "",
                        MaterialCode = material != null ? material.MaterialCode : "",
                        MaterialName = material != null ? material.MaterialName : "",
                        Version = material != null ? material.Version ?? "" : ""
                    });
                }

            }

            return new PagedInfo<ManuSfcProduceSelectViewDto>(manuSfcProduceDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }


        /// <summary>
        /// 根据SFC查询在制品步骤列表
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        public async Task<List<ManuSfcProduceStepViewDto>> QueryManuSfcProduceStepBySFCsAsync(List<ManuSfcProduceStepSFCDto> sfcs)
        {

            #region 参数验证
            if (sfcs == null || sfcs.Count() == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            #endregion

            #region 组装

            #region 组装工序
            var manuSfcs = sfcs.Select(it => it.Sfc).ToArray();

            //获取条码信息
            var manuSfcInfos = await GetManuSfcInfos(manuSfcs);

            long processRouteId = 0;
            //在制数据
            var manuSfcProduceList = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = manuSfcs });

            var groupData = manuSfcProduceList.GroupBy(a => a.SFC).Select(a => new { a.Key, WorkOrderId = a.Max(b => b.WorkOrderId) });
            //取最新的工单
            manuSfcProduceList = manuSfcProduceList.Where(a => groupData.FirstOrDefault(b => b.Key == a.SFC)?.WorkOrderId == a.WorkOrderId);

            if (manuSfcProduceList != null && manuSfcProduceList.Any())
            {
                if (manuSfcProduceList.GroupBy(it => it.WorkOrderId).Distinct().Count() > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18002));
                }
                if (manuSfcProduceList.GroupBy(it => it.ProcessRouteId).Distinct().Count() > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18004));
                }
                var workOrderId = manuSfcProduceList.Select(it => it.WorkOrderId).Distinct().FirstOrDefault();
                var workOrder = await _planWorkOrderRepository.GetByIdAsync(workOrderId);
                if (workOrder == null || workOrder.Id <= 0)
                {
                    throw new BusinessException(nameof(ErrorCode.MES16404));
                }

                //查询是否被暂停
                if (workOrder.Status == PlanWorkOrderStatusEnum.Pending)
                {
                    throw new BusinessException(nameof(ErrorCode.MES16415)).WithData("orderCode", workOrder.OrderCode);
                }
                processRouteId = manuSfcProduceList.FirstOrDefault().ProcessRouteId;

                //过滤条码信息，只查询选择的在制品工单信息
                manuSfcInfos = manuSfcInfos.Where(a => manuSfcProduceList.Any(b => b.WorkOrderId == a.WorkOrderId));
            }
            var processRouteNodes = await GetProcessRouteNode(manuSfcInfos, processRouteId);

            //组装工序
            var manuSfcProduceStepList = new List<ManuSfcProduceStepViewDto>();
            foreach (var item in processRouteNodes)
            {
                var manuSfcProduceStep = new ManuSfcProduceStepViewDto()
                {
                    ProcedureId = item.ProcedureId,
                    ProcedureCode = item.Code,
                    ProcedureName = item.Name,
                    Step = item.ManualSortNumber
                };
                manuSfcProduceStepList.Add(manuSfcProduceStep);
            }
            var endProcessRouteDetailId = processRouteNodes.OrderByDescending(it => it.SerialNo).FirstOrDefault().ProcedureId;

            #endregion

            #region 组装步骤数据
            var validationFailures = new List<ValidationFailure>();
            //为节点载入步骤数据
            foreach (var item in manuSfcProduceList)
            {

                //为错误信息添加SFC头
                var validationFailure = new ValidationFailure();
                if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                {
                    validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", item.SFC}
                        };
                }
                else
                {
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.SFC);
                }

                //锁定不允许操作
                //if (item.Lock == QualityLockEnum.FutureLock || item.Lock == QualityLockEnum.InstantLock)
                //{
                //    validationFailure.ErrorCode = nameof(ErrorCode.MES18010);
                //    validationFailures.Add(validationFailure);
                //    continue;
                //}

                var manuSfcProduceStep = manuSfcProduceStepList.Where(it => it.ProcedureId == item.ProcedureId).FirstOrDefault();
                if (manuSfcProduceStep == null)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES18007);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                switch (item.Status)
                {
                    case SfcProduceStatusEnum.lineUp:
                        manuSfcProduceStep.lineUpNumber += 1;
                        break;
                    case SfcProduceStatusEnum.Activity:
                        manuSfcProduceStep.activityNumber += 1;
                        break;
                    case SfcProduceStatusEnum.Complete:
                        manuSfcProduceStep.completeNumber += 1;
                        break;
                    default:
                        validationFailure.ErrorCode = nameof(ErrorCode.MES18008);
                        validationFailures.Add(validationFailure);
                        break;
                }
            }

            //已完成入库数据
            var manuSfcInfoList = manuSfcInfos.Where(it => it.Status == SfcStatusEnum.Complete || it.Status == SfcStatusEnum.Received).ToList();
            foreach (var item in manuSfcInfoList)
            {
                var validationFailure = new ValidationFailure();
                if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                {
                    validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", item.SFC}
                        };
                }
                else
                {
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.SFC);
                }
                //var manuSfcProduceStep = manuSfcProduceStepList.OrderByDescending(it=>it.Step).FirstOrDefault();
                var manuSfcProduceStep = manuSfcProduceStepList.Where(it => it.ProcedureId == endProcessRouteDetailId).FirstOrDefault();
                if (manuSfcProduceStep == null)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES18007);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                manuSfcProduceStep.completeNumber += 1;
            }

            //是否存在错误
            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }
            #endregion

            #endregion
            return manuSfcProduceStepList;
        }

        /// <summary>
        /// 获取条码信息（带验证）
        /// </summary>
        /// <param name="manuSfcs"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        private async Task<IEnumerable<ManuSfcView>> GetManuSfcInfos(string[] manuSfcs, long procedureId = 0)
        {
            //获取条码
            var manuSfcInfoEntitiesParam = new ManuSfcStatusQuery { Sfcs = manuSfcs, Statuss = new SfcStatusEnum?[3] { SfcStatusEnum.InProcess, SfcStatusEnum.Complete, SfcStatusEnum.Received } };
            var manuSfcInfos = await _manuSfcRepository.GetManuSfcInfoEntitiesAsync(manuSfcInfoEntitiesParam);

            var groupData = manuSfcInfos.GroupBy(a => a.SFC).Select(a => new { a.Key, WorkOrderId = a.Max(b => b.WorkOrderId) });
            //取最新的工单信息
            manuSfcInfos = manuSfcInfos.Where(a => groupData.FirstOrDefault(b => b.Key == a.SFC)?.WorkOrderId == a.WorkOrderId);

            if (manuSfcInfos == null || manuSfcInfos.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18001));
            }

            if (manuSfcs.Length != manuSfcInfos.GroupBy(a => a.SFC).Select(a => a.Key).Count())
            {
                var differentSfcs = manuSfcs.Where(it => !manuSfcInfos.Where(info => info.SFC.Contains(it)).Any()).Select(it => it).ToList();
                throw new CustomerValidationException(nameof(ErrorCode.MES18006)).WithData("SFC", string.Join(",", differentSfcs));
            }

            // 验证条码锁定
            await _manuCommonService.VerifySfcsLockAsync(new ManuProcedureBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFCs = manuSfcs,
                ProcedureId = procedureId
            });
            //包装
            var conPackList = await _manuContainerPackRepository.GetByLadeBarCodesAsync(new ManuContainerPackQuery { LadeBarCodes = manuSfcs, SiteId = _currentSite.SiteId ?? 0 });
            if (conPackList != null && conPackList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18019)).WithData("SFCs", string.Join(",", conPackList.Select(it => it.LadeBarCode).ToArray()));
            }
            return manuSfcInfos;
        }

        /// <summary>
        /// 获取工艺路线（带验证） 
        /// </summary> 
        /// <returns></returns>
        private async Task<IEnumerable<ProcProcessRouteDetailNodeView>> GetProcessRouteNode(IEnumerable<ManuSfcView> manuSfcInfos, long processRouteId = 0)
        {
            //获取工单
            var workOrderArr = manuSfcInfos.Select(it => it.WorkOrderId).Distinct().ToArray();
            //去掉多工单校验
            //if (workOrderArr.Length > 1)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES18002));
            //}
            var planWorkOrders = await _planWorkOrderRepository.GetByIdsAsync(workOrderArr);
            if (planWorkOrders == null || !planWorkOrders.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18003));
            }
            //工单同一个
            var planWorkOrderEntity = planWorkOrders.FirstOrDefault();

            //生产中/已完工的工单
            if (planWorkOrderEntity.Status == PlanWorkOrderStatusEnum.Closed)
            {
                var statusMsg = _localizationService.GetResource($"Hymson.MES.Core.Enums.PlanWorkOrderStatusEnum.{Enum.GetName(typeof(PlanWorkOrderStatusEnum), planWorkOrderEntity.Status) ?? ""}");
                throw new CustomerValidationException(nameof(ErrorCode.MES18009)).WithData("OrderCode", planWorkOrderEntity.OrderCode).WithData("EquipmentStatus", statusMsg);
            }
            //验证同一工艺路线 
            var processRouteIds = planWorkOrders.Select(it => it.ProcessRouteId).Distinct();
            if (processRouteIds.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18004));
            }
            if (processRouteId == 0)
            {
                processRouteId = processRouteIds.FirstOrDefault();
            }

            var nodes = await _procProcessRouteNodeRepository.GetListAsync(new ProcProcessRouteDetailNodeQuery { ProcessRouteId = processRouteId });
            if (nodes == null || !nodes.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18011));
            }
            return nodes = nodes.Where(it => it.ProcedureId != ProcessRoute.LastProcedureId).OrderBy(x => x.ManualSortNumber).Distinct();
        }


        /// <summary>
        /// 保存在制品步骤
        /// </summary>
        /// <param name="sfcProduceStepDto"></param>
        /// <returns></returns>
        public async Task SaveManuSfcProduceStepAsync(SaveManuSfcProduceStepDto sfcProduceStepDto)
        {
            if (sfcProduceStepDto.ProcedureId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES18012));
            if (sfcProduceStepDto.Sfcs == null || !sfcProduceStepDto.Sfcs.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES18014));


            var manuSfcs = sfcProduceStepDto.Sfcs.Select(it => it.Sfc).ToArray();

            //获取条码信息
            var manuSfcInfos = await GetManuSfcInfos(manuSfcs);


            long processRouteId = 0;
            //在制数据
            var sfcProduces = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = manuSfcs });

            if (sfcProduces != null && sfcProduces.Any())
            {
                if (sfcProduces.GroupBy(it => it.WorkOrderId).Distinct().Count() > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18002));
                }
                if (sfcProduces.GroupBy(it => it.ProcessRouteId).Distinct().Count() > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18004));
                }

                processRouteId = sfcProduces.FirstOrDefault().ProcessRouteId;

                //根据选择在制条码的工单信息过滤条码信息
                manuSfcInfos = manuSfcInfos.Where(a => sfcProduces.Any(b => b.WorkOrderId == a.WorkOrderId));
            }

            //工艺、工序
            var processRouteNodes = await GetProcessRouteNode(manuSfcInfos, processRouteId);
            var endProcessRouteDetailId = processRouteNodes.OrderByDescending(it => it.SerialNo).FirstOrDefault().ProcedureId;

            #region 逻辑 

            // 验证装箱
            var manuContainerPacks = await _manuContainerPackRepository.GetByLadeBarCodesAsync(new ManuContainerPackQuery
            {
                LadeBarCodes = manuSfcs,
                SiteId = _currentSite.SiteId ?? 0
            });
            if (manuContainerPacks != null && manuContainerPacks.Any() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18015)).WithData("SFC", string.Join(",", manuContainerPacks.Select(it => it.LadeBarCode).ToList()));
            }



            // 更新数据集合
            var updateInventoryQuantityList = new List<UpdateQuantityRangeCommand>();
            var whMaterialInventoryList = new List<WhMaterialInventoryEntity>();
            var whMaterialStandingbookList = new List<WhMaterialStandingbookEntity>();
            var manuSfcProduceList = new List<ManuSfcProduceEntity>();

            // 不在在制品信息表的条码数据
            var notManuSfcInfoList = manuSfcInfos.Where(it => sfcProduces.Where(sp => sp.SFC == it.SFC).Any() == false);
            var notManuSfcs = notManuSfcInfoList.Select(it => it.SFC);


            // 界面的选择步骤操作类型
            switch (sfcProduceStepDto.Type)
            {
                case SfcProduceStatusEnum.Complete:
                    // 如果是完成，且是尾工序
                    if (sfcProduceStepDto.ProcedureId == endProcessRouteDetailId)
                    {
                        // 入库
                        var whMaterialInventorys = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
                        {
                            SiteId = _currentSite.SiteId ?? 0,
                            BarCodes = manuSfcs
                        });

                        var notwhMaterialInventorySfcs = manuSfcInfos.Where(it => !whMaterialInventorys.Where(wmi => wmi.MaterialBarCode == it.SFC).Any());
                        var procMaterials = await _procMaterialRepository.GetByIdsAsync(notwhMaterialInventorySfcs.Select(it => it.ProductId).ToArray());
                        bool isWhMaterialInventorys = whMaterialInventorys != null && whMaterialInventorys.Any() == true;
                        foreach (var item in manuSfcInfos)
                        {
                            var procMaterial = procMaterials.Where(it => it.Id == item.ProductId).First();
                            if (isWhMaterialInventorys && whMaterialInventorys.Any(it => it.MaterialBarCode == item.SFC))
                            {
                                updateInventoryQuantityList.Add(new UpdateQuantityRangeCommand
                                {
                                    Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                                    BarCode = item.SFC,
                                    QuantityResidue = procMaterial.Batch,
                                    UpdatedBy = _currentUser.UserName,
                                    UpdatedOn = HymsonClock.Now()
                                });
                            }
                            else
                            {
                                // 物料库存
                                whMaterialInventoryList.Add(new WhMaterialInventoryEntity
                                {
                                    SupplierId = 0,//自制品 没有
                                    MaterialId = procMaterial.Id,
                                    MaterialBarCode = item.SFC,
                                    Batch = "",//自制品 没有
                                    QuantityResidue = procMaterial.Batch,
                                    Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                                    Source = MaterialInventorySourceEnum.ManuComplete,
                                    SiteId = _currentSite.SiteId ?? 0,
                                    Id = IdGenProvider.Instance.CreateId(),
                                    CreatedBy = _currentUser.UserName,
                                    UpdatedBy = _currentUser.UserName,
                                    CreatedOn = HymsonClock.Now(),
                                    UpdatedOn = HymsonClock.Now()
                                });

                                // 台账数据
                                whMaterialStandingbookList.Add(new WhMaterialStandingbookEntity
                                {
                                    MaterialCode = procMaterial.MaterialCode,
                                    MaterialName = procMaterial.MaterialName,
                                    MaterialVersion = procMaterial.Version ?? "",
                                    MaterialBarCode = item.SFC,
                                    Batch = "",//自制品 没有
                                    Quantity = procMaterial.Batch,
                                    Unit = procMaterial.Unit ?? "",
                                    Type = WhMaterialInventoryTypeEnum.StepControl,
                                    Source = MaterialInventorySourceEnum.ManuComplete,
                                    SiteId = _currentSite.SiteId ?? 0,
                                    Id = IdGenProvider.Instance.CreateId(),
                                    CreatedBy = _currentUser.UserName,
                                    UpdatedBy = _currentUser.UserName,
                                    CreatedOn = HymsonClock.Now(),
                                    UpdatedOn = HymsonClock.Now()
                                });
                            }
                        }
                    }
                    break;
                case SfcProduceStatusEnum.lineUp:
                case SfcProduceStatusEnum.Activity:
                    // 仓库 =》生产
                    if (notManuSfcInfoList != null && notManuSfcInfoList.Any() == true)
                    {
                        var workOrderIds = notManuSfcInfoList.Select(it => it.WorkOrderId).ToArray();
                        var workOrders = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);

                        // 入库
                        var whMaterialInventorys = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
                        {
                            BarCodes = notManuSfcs,
                            SiteId = _currentSite.SiteId ?? 0
                        });
                        var productIds = notManuSfcInfoList.Select(it => it.ProductId).ToArray();
                        var procMaterials = await _procMaterialRepository.GetByIdsAsync(productIds);
                        var validationFailures = new List<ValidationFailure>();
                        foreach (var item in notManuSfcInfoList)
                        {
                            var isSuccess = true;
                            var validationFailure = new ValidationFailure();
                            if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                            {
                                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", item.SFC } };
                            }
                            else
                            {
                                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.SFC);
                            }

                            var planWorkOrderEntity = workOrders.Where(it => it.Id == item.WorkOrderId).FirstOrDefault();
                            if (planWorkOrderEntity == null)
                            {
                                validationFailure.ErrorCode = nameof(ErrorCode.MES18003);
                                validationFailures.Add(validationFailure);
                                isSuccess = false;
                            }

                            //库存没用上
                            //var procMaterial = procMaterials.Where(it => it.Id == item.ProductId).FirstOrDefault();
                            //var whMaterialInventoryWhereEntit = whMaterialInventorys.Where(it => it.MaterialBarCode == item.SFC).FirstOrDefault();
                            //if (whMaterialInventoryWhereEntit == null)
                            //{
                            //    validationFailure.ErrorCode = nameof(ErrorCode.MES18020);
                            //    validationFailures.Add(validationFailure);
                            //    isSuccess = false;
                            //}
                            //if (whMaterialInventoryWhereEntit?.QuantityResidue < procMaterial?.Batch)
                            //{
                            //    validationFailure.ErrorCode = nameof(ErrorCode.MES18021);
                            //    validationFailures.Add(validationFailure);
                            //    isSuccess = false;
                            //}

                            if (isSuccess)
                            {
                                #region 数据组装
                                // 生产
                                manuSfcProduceList.Add(new ManuSfcProduceEntity
                                {
                                    Id = IdGenProvider.Instance.CreateId(),
                                    SiteId = _currentSite.SiteId ?? 0,
                                    SFC = item.SFC,
                                    ProductId = item.ProductId,
                                    WorkOrderId = item.WorkOrderId,
                                    BarCodeInfoId = item.Id,
                                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                                    Qty = item.Qty,
                                    ProcedureId = sfcProduceStepDto.ProcedureId,
                                    Status = SfcProduceStatusEnum.lineUp,
                                    RepeatedCount = 0,
                                    IsScrap = TrueOrFalseEnum.No,
                                    CreatedBy = _currentUser.UserName,
                                    UpdatedBy = _currentUser.UserName
                                });

                                //// 更改库存
                                //updateInventoryQuantityList.Add(new UpdateQuantityRangeCommand
                                //{
                                //    Status = WhMaterialInventoryStatusEnum.InUse,
                                //    BarCode = item.SFC,
                                //    QuantityResidue = procMaterial.Batch,
                                //    UpdatedBy = _currentUser.UserName,
                                //    UpdatedOn = HymsonClock.Now()
                                //});

                                //// 台账数据
                                //whMaterialStandingbookList.Add(new WhMaterialStandingbookEntity
                                //{
                                //    MaterialCode = procMaterial.MaterialCode,
                                //    MaterialName = procMaterial.MaterialName,
                                //    MaterialVersion = procMaterial.Version,
                                //    MaterialBarCode = item.SFC,
                                //    Batch = "",//自制品 没有
                                //    Quantity = procMaterial.Batch,
                                //    Unit = procMaterial.Unit ?? "",
                                //    Type = WhMaterialInventoryTypeEnum.StepControl,
                                //    Source = MaterialInventorySourceEnum.ManuComplete,
                                //    SiteId = _currentSite.SiteId ?? 0,
                                //    Id = IdGenProvider.Instance.CreateId(),
                                //    CreatedBy = _currentUser.UserName,
                                //    UpdatedBy = _currentUser.UserName,
                                //    CreatedOn = HymsonClock.Now(),
                                //    UpdatedOn = HymsonClock.Now()
                                //});
                                #endregion
                            }
                        }

                        // 是否存在错误
                        if (validationFailures.Any() == true) throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
                    }
                    break;
                default:
                    break;
            }

            // 组装步骤 
            var sfcStepList = sfcProduces.Select(item => new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                SFC = item.SFC,
                ProductId = item.ProductId,
                ProcedureId = sfcProduceStepDto.ProcedureId,
                Remark = sfcProduceStepDto.Remark ?? "",
                WorkOrderId = item.WorkOrderId,
                WorkCenterId = item.WorkCenterId,
                ProductBOMId = item.ProductBOMId,
                Qty = item.Qty,
                EquipmentId = item.EquipmentId,
                ResourceId = item.ResourceId,
                Operatetype = ManuSfcStepTypeEnum.StepControl,
                CurrentStatus = sfcProduceStepDto.Type,

                CreatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            });

            try
            {
                //TODO 这里提示事务内有不同数据库连接字符串，导致直接抛出异常，暂时先注释掉
                //using var trans = TransactionHelper.GetTransactionScope();

                // 写步骤
                await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);

                switch (sfcProduceStepDto.Type)
                {
                    case SfcProduceStatusEnum.Complete:
                        // 如果是完成，且是尾工序
                        if (sfcProduceStepDto.ProcedureId == endProcessRouteDetailId)
                        {
                            // 删除条码记录
                            await _manuSfcProduceRepository.DeletePhysicalRangeAsync(new DeletePhysicalBySfcsCommand()
                            {
                                SiteId = _currentSite.SiteId ?? 0,
                                Sfcs = manuSfcs
                            });

                            // 状态和是否再用一起改
                            //await _manuSfcRepository.UpdateSfcStatusAndIsUsedAsync(new ManuSfcUpdateStatusAndIsUsedCommand
                            //{
                            //    Sfcs = manuSfcs,
                            //    EquipmentStatus = SfcStatusEnum.Complete,
                            //    IsUsed = YesOrNoEnum.Yes,
                            //    UserId = _currentUser.UserName,
                            //    UpdatedOn = HymsonClock.Now()
                            //});

                            // 库存增加
                            if (updateInventoryQuantityList.Any() == true) await _whMaterialInventoryRepository.UpdateIncreaseQuantityResidueRangeAsync(updateInventoryQuantityList);

                            // 添加库存
                            if (whMaterialInventoryList.Any() == true) await _whMaterialInventoryRepository.InsertsAsync(whMaterialInventoryList);

                            // 台账
                            if (whMaterialStandingbookList.Any() == true) await _whMaterialStandingbookRepository.InsertsAsync(whMaterialStandingbookList);
                        }
                        // 其他
                        else
                        {
                            // 指定工序
                            await _manuSfcProduceRepository.UpdateProcedureAndStatusRangeAsync(new UpdateProcedureAndStatusCommand
                            {
                                SiteId = _currentSite.SiteId ?? 0,
                                ResourceId = null,
                                Sfcs = manuSfcs,
                                ProcedureId = sfcProduceStepDto.ProcedureId,
                                Status = sfcProduceStepDto.Type,
                                UpdatedOn = HymsonClock.Now(),
                                UserId = _currentUser.UserName
                            });
                        }
                        break;
                    case SfcProduceStatusEnum.lineUp:
                    case SfcProduceStatusEnum.Activity:
                        // 生产
                        await _manuSfcProduceRepository.InsertRangeAsync(manuSfcProduceList);

                        // 指定工序
                        await _manuSfcProduceRepository.UpdateProcedureAndStatusRangeAsync(new UpdateProcedureAndStatusCommand
                        {
                            SiteId = _currentSite.SiteId ?? 0,
                            Sfcs = manuSfcs,
                            ResourceId = null,
                            ProcedureId = sfcProduceStepDto.ProcedureId,
                            Status = sfcProduceStepDto.Type,
                            UpdatedOn = HymsonClock.Now(),
                            UserId = _currentUser.UserName
                        });

                        // 更新条码状态 
                        //if (notManuSfcs != null && notManuSfcs.Any()) await _manuSfcRepository.UpdateSfcStatusAndIsUsedAsync(new ManuSfcUpdateStatusAndIsUsedCommand
                        //{
                        //    Sfcs = notManuSfcs.ToArray(),
                        //    EquipmentStatus = SfcStatusEnum.InProcess,
                        //    IsUsed = YesOrNoEnum.Yes,
                        //    UserId = _currentUser.UserName,
                        //    UpdatedOn = HymsonClock.Now()
                        //});

                        // 库存减少
                        if (updateInventoryQuantityList.Any() == true) await _whMaterialInventoryRepository.UpdateReduceQuantityResidueRangeAsync(updateInventoryQuantityList);

                        // 台账
                        if (whMaterialStandingbookList.Any() == true) await _whMaterialStandingbookRepository.InsertsAsync(whMaterialStandingbookList);
                        break;
                    default:
                        break;
                }

                //trans.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError($"在制品步骤控制保存,写库失败:{ex.Message}");
                throw new CustomerValidationException(nameof(ErrorCode.MES18016));
            }
            #endregion
        }
        #endregion


        /// <summary>
        /// 获取更改生产列表数据
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        public async Task<List<ManuUpdateViewDto>> GetManuUpdateListAsync(string[] sfcs)
        {
            var manuUpdateViewDtoList = new List<ManuUpdateViewDto>();
            //在制数据
            var manuSfcProduces = await ManuUpdateVerifyAsync(sfcs);
            //工序
            var procedureIds = manuSfcProduces.Select(it => it.ProcedureId).ToArray();
            var procedures = await _procProcedureRepository.GetByIdsAsync(procedureIds);
            //产品
            var productIds = manuSfcProduces.Select(it => it.ProductId).ToArray();
            var products = await _procMaterialRepository.GetByIdsAsync(productIds);
            //工艺路线
            var processRouteIds = manuSfcProduces.Select(it => it.ProcessRouteId).ToArray();
            var processRoutes = await _procProcessRouteRepository.GetByIdsAsync(processRouteIds);
            //bom
            var productBOMIds = manuSfcProduces.Select(it => it.ProductBOMId).ToArray();
            var productBOMs = await _procBomRepository.GetByIdsAsync(productBOMIds);

            foreach (var item in manuSfcProduces)
            {
                var procedure = procedures.Where(it => it.Id == item.ProcedureId).First();
                var product = products.Where(it => it.Id == item.ProductId).First();
                var processRoute = processRoutes.Where(it => it.Id == item.ProcessRouteId).First();
                var productBOM = productBOMs.Where(it => it.Id == item.ProductBOMId).First();
                var manuUpdateViewDto = new ManuUpdateViewDto()
                {
                    SFC = item.SFC,
                    Status = item.Status,
                    ProcedureCode = procedure.Code,
                    MaterialAndVersion = product.MaterialCode + " / " + product.Version,
                    ProcessRouteAndVersion = processRoute.Code + " / " + processRoute.Version,
                    BomAndVersion = productBOM.BomCode + " / " + productBOM.Version,
                };
                manuUpdateViewDtoList.Add(manuUpdateViewDto);
            }
            return manuUpdateViewDtoList;
        }

        /// <summary>
        /// 获取更改生产工序列表数据
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public async Task<List<ManuUpdateProcedureViewDto>> GetProcedureByOrderIdListAsync(long workOrderId)
        {
            if (workOrderId <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18202));
            }
            var order = await _planWorkOrderRepository.GetByIdAsync(workOrderId);
            var processRouteDetailNode = await _procProcessRouteDetailNodeRepository.GetListAsync(new ProcProcessRouteDetailNodeQuery { ProcessRouteId = order.ProcessRouteId });
            if (processRouteDetailNode == null || !processRouteDetailNode.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18208));
            }

            var list = new List<ManuUpdateProcedureViewDto>();
            foreach (var item in processRouteDetailNode)
            {
                if (!string.IsNullOrWhiteSpace(item.Code))
                    list.Add(new ManuUpdateProcedureViewDto
                    {
                        ProcedureId = item.ProcedureId,
                        ProcedureCode = item.Code,
                        ProcedureName = item.Name,
                    });
            }
            return list;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="sfcs"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ManuSfcProduceEntity>> ManuUpdateVerifyAsync(string[] sfcs, long procedureId = 0)
        {
            if (sfcs == null || sfcs.Length <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18208));
            }
            if (sfcs.Length > 100)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18200)).WithData("number", 100);
            }
            //验证条码 在制数据
            var manuSfcProduces = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = sfcs });
            var workOrderIdDistinct = manuSfcProduces.Select(it => it.WorkOrderId).Distinct();
            if (workOrderIdDistinct.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18206));
            }

            //验证条码状态
            SfcStatusEnum?[] sfcStatusArr = { SfcStatusEnum.Complete, SfcStatusEnum.Received, SfcStatusEnum.Scrapping };
            var sfcList = await _manuSfcRepository.GetManuSfcInfoEntitiesAsync(new ManuSfcStatusQuery { Sfcs = sfcs, Statuss = sfcStatusArr });
            if (sfcList != null && sfcList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18211));
            }

            // 验证条码锁定
            await _manuCommonService.VerifySfcsLockAsync(new ManuProcedureBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFCs = sfcs,
                ProcedureId = procedureId
            });

            //这个是物料删除 所以查到就是有锁
            //var sfcProduceBusinesss = await _manuSfcProduceRepository.GetSfcProduceBusinessListBySFCAsync(new SfcListProduceBusinessQuery { Sfcs = sfcs, BusinessType = ManuSfcProduceBusinessType.Lock });
            //if (sfcProduceBusinesss != null && sfcProduceBusinesss.Any())
            //{
            //    var sfcInfoIds = sfcProduceBusinesss.Select(it => it.SfcInfoId).ToArray();
            //    var sfcInfos = await _manuSfcInfoRepository.GetByIdsAsync(sfcInfoIds);
            //    var sfcids = sfcInfos.Select(it => it.SfcId).ToArray();
            //    var manuSfcs = await _manuSfcRepository.GetByIdsAsync(sfcids);
            //    throw new CustomerValidationException(nameof(ErrorCode.MES18204)).WithData("SFC", string.Join(",", manuSfcs.Select(it => it.SFC).ToArray()));
            //}

            //工单
            var WorkOrderIds = manuSfcProduces.Select(it => it.WorkOrderId).ToArray();
            var workOrders = await _planWorkOrderRepository.GetByIdsAsync(WorkOrderIds);
            PlanWorkOrderStatusEnum[] statusArr = { PlanWorkOrderStatusEnum.NotStarted, PlanWorkOrderStatusEnum.Finish, PlanWorkOrderStatusEnum.Closed };
            var workOrdersOrLosck = workOrders.Where(it => statusArr.Contains(it.Status) || it.Status == PlanWorkOrderStatusEnum.Pending);
            if (workOrdersOrLosck.Any())
            {
                var sfcInfoIds = manuSfcProduces.Where(it => workOrdersOrLosck.Where(order => order.Id == it.WorkOrderId).Any()).Select(it => it.SFC).ToArray();
                throw new CustomerValidationException(nameof(ErrorCode.MES18205)).WithData("SFC", string.Join(",", sfcInfoIds));
            }

            return manuSfcProduces;
        }

        /// <summary>
        /// 保存生产更改
        /// </summary>
        /// <param name="manuUpdateSaveDto"></param>
        /// <returns></returns>
        public async Task SaveManuUpdateListAsync(ManuUpdateSaveDto manuUpdateSaveDto)
        {
            #region 验证
            if (manuUpdateSaveDto.WorkOrderId <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18202));
            }
            if (manuUpdateSaveDto.ProcedureId <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18203));
            }
            var procedure = _procProcedureRepository.GetByIdAsync(manuUpdateSaveDto.ProcedureId) ?? throw new CustomerValidationException(nameof(ErrorCode.MES18214));
            #endregion

            #region 逻辑
            //在制数据
            var manuSfcProduces = await ManuUpdateVerifyAsync(manuUpdateSaveDto.Sfcs, manuUpdateSaveDto.ProcedureId);

            //老工单
            var workOrderId = manuSfcProduces.First().WorkOrderId;
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(workOrderId);
            var workOrderQty = manuSfcProduces.Sum(it => it.Qty);
            if (workOrderId == manuUpdateSaveDto.WorkOrderId)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18212)).WithData("Code", planWorkOrderEntity.OrderCode);
            }
            //新工单
            var newPlanWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(manuUpdateSaveDto.WorkOrderId);

            PlanWorkOrderStatusEnum[] statusArr = { PlanWorkOrderStatusEnum.NotStarted, PlanWorkOrderStatusEnum.Pending, PlanWorkOrderStatusEnum.Closed };//PlanWorkOrderStatusEnum.Finish, 
            var workOrdersOrLosck = statusArr.Contains(newPlanWorkOrderEntity.Status);
            if (workOrdersOrLosck)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18209)).WithData("Code", newPlanWorkOrderEntity.OrderCode);
            }


            var orderRecord = await _planWorkOrderRepository.GetByWorkOrderIdAsync(newPlanWorkOrderEntity.Id);
            var PlanQuantity = newPlanWorkOrderEntity.Qty * (1 + newPlanWorkOrderEntity.OverScale / 100);
            var remainingQuantity = PlanQuantity - orderRecord.PassDownQuantity;
            if (remainingQuantity < workOrderQty)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18207));
            }

            //组装步骤
            var sfcStepList = new List<ManuSfcStepEntity>();
            foreach (var item in manuSfcProduces)
            {
                item.ProcedureId = manuUpdateSaveDto.ProcedureId;
                item.ProductId = newPlanWorkOrderEntity.ProductId;
                item.ProcessRouteId = newPlanWorkOrderEntity.ProcessRouteId;
                item.ProductBOMId = newPlanWorkOrderEntity.ProductBOMId;
                item.WorkOrderId = newPlanWorkOrderEntity.Id;
                item.ResourceId = null; //更改步骤后 更改资源为null   为null则生产不限制匹配

                // 初始化步骤
                var sfcStep = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = item.SFC,
                    ProductId = item.ProductId,
                    ProcedureId = manuUpdateSaveDto.ProcedureId,
                    Remark = manuUpdateSaveDto.Remark,
                    WorkOrderId = item.WorkOrderId,
                    WorkCenterId = item.WorkCenterId,
                    ProductBOMId = item.ProductBOMId,
                    Qty = item.Qty,
                    EquipmentId = item.EquipmentId,
                    ResourceId = item.ResourceId,
                    Operatetype = ManuSfcStepTypeEnum.ManuUpdate,
                    CurrentStatus = SfcProduceStatusEnum.Activity,

                    CreatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),

                };
                sfcStepList.Add(sfcStep);
            }


            //处理条码信息 manu_sfc_info select sum(Qty) from manu_sfc_produce where  WorkOrderId='10362438214823936'
            var manuSfcs = await _manuSfcRepository.GetBySFCsAsync(manuUpdateSaveDto.Sfcs);
            var sfcIds = manuSfcs.Select(it => it.Id).ToArray();
            var sfcInfos = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcIds);

            //更改老条码状态 这里不直接改老条码  而是新增新条码  便于追溯
            var newSfcInfos = new List<ManuSfcInfoEntity>();
            foreach (var item in sfcInfos)
            {
                item.IsUsed = false;
                var newSfcInfo = new ManuSfcInfoEntity()
                {
                    SfcId = item.SfcId,
                    WorkOrderId = manuUpdateSaveDto.WorkOrderId,
                    ProductId = newPlanWorkOrderEntity.ProductId,
                    IsUsed = true,

                    SiteId = _currentSite.SiteId ?? 0,
                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now()
                };
                newSfcInfos.Add(newSfcInfo);
            }

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //在制
                await _manuSfcProduceRepository.UpdateRangeAsync(manuSfcProduces);

                //新工单
                await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderId(new UpdatePassDownQuantityCommand
                {
                    WorkOrderId = newPlanWorkOrderEntity.Id,
                    PlanQuantity = newPlanWorkOrderEntity.Qty * (1 + newPlanWorkOrderEntity.OverScale / 100),
                    PassDownQuantity = workOrderQty,
                    UserName = _currentUser.UserName,
                    UpdateDate = HymsonClock.Now()
                });

                //老工单
                await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderId(new UpdatePassDownQuantityCommand
                {
                    WorkOrderId = workOrderId,
                    PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100),
                    PassDownQuantity = -workOrderQty,
                    UserName = _currentUser.UserName,
                    UpdateDate = HymsonClock.Now()
                });

                //条码
                await _manuSfcInfoRepository.UpdatesAsync(sfcInfos);
                await _manuSfcInfoRepository.InsertsAsync(newSfcInfos);

                //步骤
                await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);
                trans.Complete();
            }
            #endregion
        }

        /// <summary>
        /// 获取工艺路线末尾工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        public async Task<long> GetLastProcedureAsync(long processRouteId)
        {
            long id = 0;
            //获取工艺路线节点
            var processRouteNodes = await _manuCommonOldService.GetProcessRouteAsync(processRouteId);
            if (processRouteNodes.Any())
            {
                id = processRouteNodes?.FirstOrDefault()?.ProcedureIds.Last() ?? 0;
            }
            return id;
        }

        /// <summary>
        /// 验证sfc是否锁定
        /// </summary>
        /// <param name="manuSfcs"></param>
        /// <returns></returns>
        private async Task VerifySfcsLockAsync(ManuSfcProduceEntity[] manuSfcs)
        {
            var sfcs = manuSfcs.Select(x => x.SFC).ToArray();
            var sfcProduceBusinesss = await _manuSfcProduceRepository.GetSfcProduceBusinessListBySFCAsync(new SfcListProduceBusinessQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = sfcs, BusinessType = ManuSfcProduceBusinessType.Lock });
            if (sfcProduceBusinesss != null && sfcProduceBusinesss.Any())
            {
                //  var sfcInfoIds = sfcProduceBusinesss.Select(it => it.SfcProduceId).ToArray();
                var sfcProduceBusinesssList = sfcProduceBusinesss.ToList();
                var instantLockSfcs = new List<string>();
                foreach (var business in sfcProduceBusinesssList)
                {
                    var manuSfc = manuSfcs.FirstOrDefault(x => x.Id == business.SfcProduceId);
                    if (manuSfc == null)
                    {
                        continue;
                    }
                    var sfcProduceLockBo = System.Text.Json.JsonSerializer.Deserialize<SfcProduceLockBo>(business.BusinessContent);
                    if (sfcProduceLockBo == null)
                    {
                        continue;
                    }
                    if (sfcProduceLockBo.Lock == QualityLockEnum.InstantLock)
                    {
                        instantLockSfcs.Add(manuSfc.SFC);
                    }
                    if (sfcProduceLockBo.Lock == QualityLockEnum.FutureLock && sfcProduceLockBo.LockProductionId == manuSfc.ProcedureId)
                    {
                        instantLockSfcs.Add(manuSfc.SFC);
                    }
                }

                if (instantLockSfcs.Any())
                {
                    var strs = string.Join(",", instantLockSfcs.Distinct().ToArray());
                    throw new CustomerValidationException(nameof(ErrorCode.MES15407)).WithData("sfcs", strs);
                }
            }
        }

        /// <summary>
        /// 验证将来工序
        /// </summary>
        /// <param name="sfcList"></param>
        /// <param name="lockProductionId"></param>
        /// <returns></returns>
        private async Task VeifyQualityLockProductionAsync(ManuSfcProduceEntity[] sfcList, long lockProductionId)
        {
            var processRouteId = sfcList.FirstOrDefault()?.ProcessRouteId ?? 0;
            var sfcProcedureIds = sfcList.Select(x => x.ProcedureId).Distinct().ToList();

            //判断将来工序是不是工艺路线上的工序，而且是选择的条码的接下来的工序
            var processRouteNodes = await _manuCommonOldService.GetProcessRouteAsync(processRouteId);
            if (processRouteNodes == null || !processRouteNodes.Any())
            {
                var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(lockProductionId);
                throw new CustomerValidationException(nameof(ErrorCode.MES15310)).WithData("lockproduction", procProcedureEntity.Code);
            }

            //条码工序
            var procedureIds = processRouteNodes?.FirstOrDefault()?.ProcedureIds.ToList();
            if (procedureIds == null || !procedureIds.Any())
            {
                var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(lockProductionId);
                throw new CustomerValidationException(nameof(ErrorCode.MES15310)).WithData("lockproduction", procProcedureEntity.Code);
            }

            if (!procedureIds.Contains(lockProductionId))
            {
                var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(lockProductionId);
                throw new CustomerValidationException(nameof(ErrorCode.MES15310)).WithData("lockproduction", procProcedureEntity.Code);
            }

            var index = 0;
            sfcProcedureIds.ForEach(item =>
            {
                int sfcIndex = procedureIds.FindLastIndex(x => x.Equals(item));
                index = sfcIndex > index ? sfcIndex : index;
            });
            int sfcIndex = procedureIds.FindLastIndex(x => x.Equals(lockProductionId));
            if (sfcIndex <= index)
            {
                var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(lockProductionId);
                throw new CustomerValidationException(nameof(ErrorCode.MES15317)).WithData("lockproduction", procProcedureEntity.Code);
            }
        }

    }
}
