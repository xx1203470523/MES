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
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.View;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCommonDto;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.MES.Services.Services.Manufacture.ManuSfcProduce;
using Hymson.MES.Services.Services.Warehouse;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        private readonly IManuCommonService _manuCommonService;
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


        private readonly AbstractValidator<ManuSfcProduceLockDto> _validationLockRules;
        private readonly AbstractValidator<ManuSfcProduceModifyDto> _validationModifyRules;
        private readonly ILogger<ManuSfcProduceService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuSfcProduceService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IProcResourceRepository resourceRepository,
            IManuSfcRepository manuSfcRepository,
            IPlanWorkOrderActivationRepository planWorkOrderActivationRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuCommonService manuCommonService,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcedureRepository procProcedureRepository,
            ILocalizationService localizationService,
             IManuContainerPackRepository manuContainerPackRepository,
              IWhMaterialInventoryService whMaterialInventoryService,
              IProcMaterialRepository procMaterialRepository,
               IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository,
        AbstractValidator<ManuSfcProduceLockDto> validationLockRules,
            AbstractValidator<ManuSfcProduceModifyDto> validationModifyRules,
            ILogger<ManuSfcProduceService> logger)
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
            _procProcedureRepository = procProcedureRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _localizationService = localizationService;
            _manuContainerPackRepository = manuContainerPackRepository;
            _whMaterialInventoryService = whMaterialInventoryService;
            _procMaterialRepository = procMaterialRepository;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
            _validationLockRules = validationLockRules;
            _validationModifyRules = validationModifyRules;
            this._logger = logger;
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
            if (!string.IsNullOrWhiteSpace(manuSfcProducePagedQueryDto.Sfcs))
            {
                manuSfcProducePagedQuery.SfcArray = manuSfcProducePagedQueryDto.Sfcs.Split(',');
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
        /// 质量锁定
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task QualityLockAsync(ManuSfcProduceLockDto parm)
        {
            #region 验证
            if (parm == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            await _validationLockRules.ValidateAndThrowAsync(parm);


            var sfcListTask = _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery { Sfcs = parm.Sfcs.Distinct().ToArray() });
            var sfcProduceBusinesssListTask = _manuSfcProduceRepository.GetSfcProduceBusinessListBySFCAsync(new SfcListProduceBusinessQuery { Sfcs = parm.Sfcs, BusinessType = ManuSfcProduceBusinessType.Lock });
            var sfcList = await sfcListTask;
            var sfcProduceBusinesssList = await sfcProduceBusinesssListTask;

            if (sfcListTask == null || !sfcList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15309));
            }

            //未来锁只能使用一个工单
            if (parm.OperationType == QualityLockEnum.FutureLock)
            {
                var workOrders = sfcList.Select(x => x.WorkOrderId).Distinct().ToList();
                if (workOrders.Count > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15308));
                }
                //验证工艺路线
                var procProcessRouteDetailNodeEntity = await _procProcessRouteDetailNodeRepository.GetByProcessRouteIdAsync(new ProcProcessRouteDetailNodeQuery
                {
                    ProcessRouteId = sfcList.FirstOrDefault()?.ProcessRouteId ?? 0,
                    ProcedureId = parm.LockProductionId ?? 0
                });

                if (procProcessRouteDetailNodeEntity == null)
                {
                    var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(parm.LockProductionId ?? 0);
                    if (procProcedureEntity == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES15311));
                    }
                    else
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES15310)).WithData("lockproduction", procProcedureEntity.Name);
                    }
                }
            }
            //TODO  验证未完成 wangkeming
            var validationFailures = new List<ValidationFailure>();
            foreach (var sfc in parm.Sfcs)
            {
                var sfcEntity = sfcList.FirstOrDefault(x => x.SFC == sfc);
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
                var sfcProduceBusinessEntity = sfcProduceBusinesssList.FirstOrDefault(x => x.SfcInfoId == sfcEntity.Id);
                switch (parm.OperationType)
                {
                    case
                        QualityLockEnum.FutureLock:
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

                        //验证工序
                        if (await _manuCommonService.IsProcessStartBeforeEnd(sfcEntity.ProcessRouteId, sfcEntity.ProcedureId, parm.LockProductionId ?? 0))
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

                        break;
                    case
                        QualityLockEnum.InstantLock:
                        if (sfcProduceBusinessEntity != null)
                        {
                            var sfcProduceLockBo = JsonSerializer.Deserialize<SfcProduceLockBo>(sfcProduceBusinessEntity.BusinessContent);

                            if (sfcProduceLockBo != null && sfcProduceLockBo.Lock == QualityLockEnum.InstantLock)
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
                                validationFailure.ErrorCode = nameof(ErrorCode.MES15315);
                                validationFailures.Add(validationFailure);
                            }
                        }
                        break;
                    case
                        QualityLockEnum.Unlock:
                        if (sfcProduceBusinessEntity == null)
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
                        }
                        break;
                    default:
                        break;
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }
            #endregion

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
                SfcProduceLockBo sfcProduceLockBo = new SfcProduceLockBo()
                {
                    Lock = parm.OperationType,
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
                    CurrentStatus = sfc.Status,
                    CreatedBy = sfc.CreatedBy,
                    UpdatedBy = sfc.UpdatedBy
                };

                if (parm.OperationType == QualityLockEnum.Unlock)
                {
                    stepEntity.Operatetype = ManuSfcStepTypeEnum.Unlock;
                    unLockList.Add(sfc.Id);
                }
                else
                {
                    stepEntity.Operatetype = parm.OperationType == QualityLockEnum.InstantLock ? ManuSfcStepTypeEnum.InstantLock : ManuSfcStepTypeEnum.FutureLock;
                    sfcProduceBusinessList.Add(new ManuSfcProduceBusinessEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        SfcInfoId = sfc.Id,
                        BusinessType = ManuSfcProduceBusinessType.Lock,
                        BusinessContent = JsonSerializer.Serialize(sfcProduceLockBo),
                        CreatedBy = sfc.CreatedBy,
                        UpdatedBy = sfc.UpdatedBy
                    });
                }

                sfcStepList.Add(stepEntity);

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

                if (unLockList != null && unLockList.Any())
                {
                    await _manuSfcProduceRepository.DeleteSfcProduceBusinesssAsync(new DeleteSfcProduceBusinesssCommand { SfcInfoIds = unLockList, BusinessType = ManuSfcProduceBusinessType.Lock });
                }

                //插入操作数据
                await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);

                //插入步骤业务表
                await _manuSfcStepRepository.InsertSfcStepBusinessRangeAsync(sfcStepBusinessList);

                trans.Complete();
            }
        }

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
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = parm.Sfcs };
            //获取条码列表
            var manuSfcs = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);
            if (!manuSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15402));
            }

            var scrapSfcs = await _manuSfcRepository.GetManuSfcInfoEntitiesAsync(new ManuSfcStatusQuery
            {
                Sfcs = parm.Sfcs,
                Statuss = new SfcStatusEnum?[1] { SfcStatusEnum.Scrapping }
            });
            //类型为报废时判断条码是否已经报废,若已经报废提示:存在已报废的条码，不可再次报废
            if (scrapSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15401));
            }
            #endregion

            #region  组装数据
            var sfcStepList = GetSfcStepList(manuSfcs, parm.Remark ?? "", ManuSfcStepTypeEnum.Discard);

            var sfcs = manuSfcs.Select(a => a.SFC).ToArray();
            var isScrapCommand = new UpdateIsScrapCommand
            {
                Sfcs = sfcs,
                UserId = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                IsScrap = TrueOrFalseEnum.Yes
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
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = parm.Sfcs };
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

            //取消报废， 验证工单是否已经激活，若已经取消激活，不能取消报废条码
            var orderIds = manuSfcs.Select(x => x.WorkOrderId).Distinct().ToArray();
            var activeOrders = await _planWorkOrderActivationRepository.GetByIdsAsync(orderIds);
            if (activeOrders == null)
            {
                var orders = await _planWorkOrderRepository.GetByIdsAsync(orderIds);
                var orderCodes = orders.Select(x => x.OrderCode).ToArray();
                throw new CustomerValidationException(nameof(ErrorCode.MES15404)).WithData("orders", string.Join("','", orderCodes));
            }

            var activeOrderList = activeOrders.ToList();
            if (activeOrderList.Count < orderIds.Length)
            {
                var activeOrderIds = activeOrderList.Select(x => x.WorkOrderId).ToArray();
                //找出相同元素(即交集)
                var diffOrderIds = orderIds.Where(c => !activeOrderIds.Contains(c)).ToArray();
                var orders = await _planWorkOrderRepository.GetByIdsAsync(diffOrderIds);
                var orderCodesStr = string.Join(",", orders.Select(x => x.OrderCode).ToArray());
                throw new CustomerValidationException(nameof(ErrorCode.MES15404)).WithData("orders", orderCodesStr);
            }
            #endregion

            #region  组装数据
            var sfcStepList = GetSfcStepList(manuSfcs, parm.Remark ?? "", ManuSfcStepTypeEnum.CancelDiscard);

            var sfcs = manuSfcs.Select(a => a.SFC).ToArray();
            var isScrapCommand = new UpdateIsScrapCommand
            {
                Sfcs = sfcs,
                UserId = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                IsScrap = TrueOrFalseEnum.No
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
                //1.插入数据操作类型为取消报废
                rows += await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);

                //2.修改在制品表,IsScrap
                rows += await _manuSfcProduceRepository.UpdateIsScrapAsync(isScrapCommand);

                //3.条码信息表
                rows += await _manuSfcRepository.UpdateStatusAsync(manuSfcInfoUpdate);
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
            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(sfc);
            if (manuSfcProduceEntity == null)
            {
                return null;
            }
            return manuSfcProduceEntity.ToModel<ManuSfcProduceDto>();
        }


        #region 在制品步骤控制
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

            #region 主数据
            //获取条码
            //var manuSfcs = sfcs.Select(it => it.Sfc).ToArray();
            //var manuSfcInfoEntitiesParam = new ManuSfcStatusQuery { Sfcs = manuSfcs, Statuss = new SfcStatusEnum?[3] { SfcStatusEnum.InProcess, SfcStatusEnum.Complete, SfcStatusEnum.Received } };
            //var manuSfcInfos = await _manuSfcRepository.GetManuSfcInfoEntitiesAsync(manuSfcInfoEntitiesParam);

            //if (manuSfcInfos == null || manuSfcInfos.Count() == 0)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES18001));
            //}

            //if (manuSfcs.Count() != manuSfcInfos.Count())
            //{
            //    var differentSfcs = sfcs.Where(it => !manuSfcInfos.Where(info => info.SFC.Contains(it.Sfc)).Any()).Select(it => it.Sfc).ToList();
            //    throw new CustomerValidationException(nameof(ErrorCode.MES18006)).WithData("SFC", string.Join(",", differentSfcs));
            //}

            ////获取工单
            //var workOrderArr = manuSfcInfos.Select(it => it.WorkOrderId).Distinct().ToArray();
            //if (workOrderArr.Count() > 1)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES18002));
            //}
            //var planWorkOrders = await _planWorkOrderRepository.GetByIdsAsync(workOrderArr);
            //if (planWorkOrders == null)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES18003));
            //}
            //var planWorkOrdersWhStatus = planWorkOrders.Where(it => it.Status != PlanWorkOrderStatusEnum.InProduction && it.Status != PlanWorkOrderStatusEnum.Finish).Any();
            ////生产中/已完工的工单
            //if (planWorkOrdersWhStatus)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES18009));
            //}
            ////验证同一工艺路线 
            //var processRouteIds = planWorkOrders.Select(it => it.ProcessRouteId).Distinct();
            //if (processRouteIds.Count() > 1)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES18004));
            //}
            //var processRouteId = processRouteIds.FirstOrDefault();
            ////获取工艺路线节点
            //var processRouteNodes = await _manuCommonService.GetProcessRoute(processRouteId);
            //if (processRouteNodes == null || processRouteIds.Count() == 0)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES18005));
            //}

            #endregion

            var (manuSfcInfos, processRouteNodes, endProcessRouteDetailId) = await VerifySfcAsync(sfcs);

            #region 组装节点
            //组装节点
            var nodeList = new List<long>();
            foreach (var item in processRouteNodes)
            {
                foreach (var node in item.ProcedureIds)
                {
                    nodeList.Add(node);
                }
            }
            //获取工序
            var procProcedures = await _procProcedureRepository.GetByIdsAsync(nodeList.ToArray());
            if (!procProcedures.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18011));
            }

            //组装工序
            var manuSfcProduceStepList = new List<ManuSfcProduceStepViewDto>();
            var manuSfcProduceStepEnd = new ManuSfcProduceStepViewDto(); //尾工序
            int i = 0;
            foreach (var item in procProcedures)
            {
                //尾工序放最后
                if (item.Id == endProcessRouteDetailId)
                {
                    manuSfcProduceStepEnd.ProcedureId = item.Id;
                    manuSfcProduceStepEnd.ProcedureCode = item.Code;
                    manuSfcProduceStepEnd.ProcedureName = item.Name;
                }
                else
                {
                    i++;
                    var manuSfcProduceStep = new ManuSfcProduceStepViewDto()
                    {
                        ProcedureId = item.Id,
                        ProcedureCode = item.Code,
                        ProcedureName = item.Name,
                        Step = i
                    };
                    manuSfcProduceStepList.Add(manuSfcProduceStep);
                }
            }
            manuSfcProduceStepList.Add(manuSfcProduceStepEnd);
            #endregion

            #region 组装步骤数据
            var validationFailures = new List<ValidationFailure>();
            //为节点载入步骤数据

            //在制数据
            var manuSfcs = sfcs.Select(it => it.Sfc).ToArray();
            var manuSfcProduceEntit = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery { Sfcs = manuSfcs });
            foreach (var item in manuSfcProduceEntit)
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
                if (item.Lock == QualityLockEnum.FutureLock || item.Lock == QualityLockEnum.InstantLock)
                {
                    validationFailure.ErrorCode = nameof(ErrorCode.MES18010);
                    validationFailures.Add(validationFailure);
                    continue;
                }

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
        /// 验证
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        private async Task<(IEnumerable<ManuSfcView> manuSfcInfos, IEnumerable<ProcessRouteDetailDto> processRouteNodes, long endProcessRouteDetailId)> VerifySfcAsync(List<ManuSfcProduceStepSFCDto> sfcs)
        {
            #region 主数据
            //获取条码
            var manuSfcs = sfcs.Select(it => it.Sfc).ToArray();
            var manuSfcInfoEntitiesParam = new ManuSfcStatusQuery { Sfcs = manuSfcs, Statuss = new SfcStatusEnum?[3] { SfcStatusEnum.InProcess, SfcStatusEnum.Complete, SfcStatusEnum.Received } };
            var manuSfcInfos = await _manuSfcRepository.GetManuSfcInfoEntitiesAsync(manuSfcInfoEntitiesParam);

            if (manuSfcInfos == null || manuSfcInfos.Count() == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18001));
            }

            if (manuSfcs.Count() != manuSfcInfos.Count())
            {
                var differentSfcs = sfcs.Where(it => !manuSfcInfos.Where(info => info.SFC.Contains(it.Sfc)).Any()).Select(it => it.Sfc).ToList();
                throw new CustomerValidationException(nameof(ErrorCode.MES18006)).WithData("SFC", string.Join(",", differentSfcs));
            }

            //获取工单
            var workOrderArr = manuSfcInfos.Select(it => it.WorkOrderId).Distinct().ToArray();
            if (workOrderArr.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18002));
            }
            var planWorkOrders = await _planWorkOrderRepository.GetByIdsAsync(workOrderArr);
            if (planWorkOrders == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18003));
            }
            var planWorkOrdersWhStatus = planWorkOrders.Where(it => it.Status != PlanWorkOrderStatusEnum.InProduction && it.Status != PlanWorkOrderStatusEnum.Finish).Any();
            //生产中/已完工的工单
            if (planWorkOrdersWhStatus)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18009));
            }
            //验证同一工艺路线 
            var processRouteIds = planWorkOrders.Select(it => it.ProcessRouteId).Distinct();
            if (processRouteIds.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18004));
            }
            var processRouteId = processRouteIds.FirstOrDefault();
            //获取工艺路线节点
            var processRouteNodes = await _manuCommonService.GetProcessRoute(processRouteId);
            if (processRouteNodes == null || processRouteIds.Count() == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18005));
            }

            //获取尾工序
            // 因为可能有分叉，所以返回的下一步工序是集合
            var netxtProcessRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetPreProcessRouteDetailLinkAsync(new ProcProcessRouteDetailLinkQuery
            {
                ProcessRouteId = processRouteId,
                ProcedureId = ProcessRoute.LastProcedureId
            });
            if (netxtProcessRouteDetailLinks == null || !netxtProcessRouteDetailLinks.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18017));
            }
            //TODO 尾工序会存在多个吗？
            //if (netxtProcessRouteDetailLinks.Count() > 1)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES18018));
            //}
            var endProcessRouteDetailId = netxtProcessRouteDetailLinks.FirstOrDefault()?.PreProcessRouteDetailId ?? 0;
            if (endProcessRouteDetailId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18017));
            }
            #endregion
            return (manuSfcInfos, processRouteNodes, endProcessRouteDetailId);
        }

        /// <summary>
        /// 保存在制品步骤
        /// </summary>
        /// <param name="sfcProduceStepDto"></param>
        /// <returns></returns>
        public async Task SaveManuSfcProduceStepAsync(SaveManuSfcProduceStepDto sfcProduceStepDto)
        {
            #region 参数验证
            if (sfcProduceStepDto.ProcedureId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18012));
            }
            //if (sfcProduceStepDto.Type == null)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES18013));
            //}
            if (sfcProduceStepDto.Sfcs == null || !sfcProduceStepDto.Sfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18014));
            }
            var (manuSfcInfos, processRouteNodes, endProcessRouteDetailId) = await VerifySfcAsync(sfcProduceStepDto.Sfcs);
            #endregion


            #region 逻辑 
            var sfcsArr = sfcProduceStepDto.Sfcs.Select(it => it.Sfc).ToArray();
            //验证装箱
            var manuContainerPacks = await _manuContainerPackRepository.GetByLadeBarCodesAsync(new ManuContainerPackQuery { LadeBarCodes = sfcsArr, SiteId = _currentSite.SiteId ?? 0 });
            if (manuContainerPacks != null && manuContainerPacks.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18015)).WithData("SFC", string.Join(",", manuContainerPacks.Select(it => it.LadeBarCode).ToList()));
            }


            //在制数据
            var sfcProduceEntity = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery { Sfcs = sfcsArr });
            var sfcStepList = new List<ManuSfcStepEntity>();
            var operatetype = sfcProduceStepDto.Type == SfcProduceStatusEnum.Complete ? ManuSfcStepTypeEnum.Complete : ManuSfcStepTypeEnum.InStock;//TODO 是转换？

            //组装步骤 
            foreach (var item in sfcProduceEntity)
            {
                // 初始化步骤
                var sfcStep = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = item.SFC,
                    ProductId = item.ProductId,
                    WorkOrderId = item.WorkOrderId,
                    WorkCenterId = item.WorkCenterId,
                    ProductBOMId = item.ProductBOMId,
                    Qty = item.Qty,
                    EquipmentId = item.EquipmentId,
                    ResourceId = item.ResourceId,
                    Operatetype = operatetype,
                    CurrentStatus = sfcProduceStepDto.Type,

                    CreatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),

                };
                sfcStepList.Add(sfcStep);
            }

            try
            {
                using (var trans = TransactionHelper.GetTransactionScope())
                {
                    if (sfcProduceStepDto.Type == SfcProduceStatusEnum.Complete)
                    {
                        //写步骤
                        await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);

                        //如果尾工序
                        if (sfcProduceStepDto.ProcedureId == endProcessRouteDetailId)
                        {
                            //删除条码记录
                            await _manuSfcProduceRepository.DeletePhysicalRangeAsync(sfcsArr.ToList());

                            // 更新条码信息
                            var sfcInfo = await _manuSfcRepository.GetBySFCsAsync(sfcsArr);
                            await _manuSfcRepository.UpdateStatusAsync(new ManuSfcUpdateCommand { Sfcs = sfcsArr, Status = SfcStatusEnum.Complete, UserId = _currentUser.UserName, UpdatedOn = HymsonClock.Now() });
                            //不用更新这个  因为是唯一的生产肯定使用了  
                            //await _manuSfcInfoRepository.UpdatesIsUsedAsync(new ManuSfcInfoUpdateCommand { Sfcs = sfcsArr, IsUsed = YesOrNoEnum.No, UserId = _currentUser.UserName, UpdatedOn = HymsonClock.Now() });

                            //入库
                            var whMaterialInventorys = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarcodeQuery { BarCodes = sfcsArr, SiteId = _currentSite.SiteId ?? 0 });
                            var notwhMaterialInventorySfcs = manuSfcInfos.Where(it => !whMaterialInventorys.Where(wmi => wmi.MaterialBarCode == it.SFC).Any()).ToList();
                            var productIds = notwhMaterialInventorySfcs.Select(it => it.ProductId).ToArray();
                            var procMaterials = await _procMaterialRepository.GetByIdsAsync(productIds);

                            var whMaterialInventoryListCreateDtoList = new List<WhMaterialInventoryListCreateDto>();
                            foreach (var item in manuSfcInfos)
                            {
                                var procMaterial = procMaterials.Where(it => it.Id == item.ProductId).First();
                                var whMaterialInventoryListCreateDto = new WhMaterialInventoryListCreateDto()
                                {
                                    Batch = HymsonClock.Now().ToShortDateString(),
                                    MaterialBarCode = item.SFC,
                                    MaterialCode = procMaterial.MaterialCode,
                                    QuantityResidue = procMaterial.Batch,
                                    Source = WhMaterialInventorySourceEnum.manuComplete,
                                    SupplierId = 0,
                                    Type = WhMaterialInventoryTypeEnum.manuComplete,
                                    Version = procMaterial.Version
                                };
                                whMaterialInventoryListCreateDtoList.Add(whMaterialInventoryListCreateDto);

                            }
                            await _whMaterialInventoryService.CreateWhMaterialInventoryListAsync(whMaterialInventoryListCreateDtoList);
                        }
                        else
                        {
                            //不是尾工序 直接更改状态
                            var updateProcedureAndStatusCommand = new UpdateProcedureAndStatusCommand
                            {
                                Sfcs = sfcsArr,
                                ProcedureId = sfcProduceStepDto.ProcedureId,
                                Status = sfcProduceStepDto.Type,
                                UpdatedOn = HymsonClock.Now(),
                                UserId = _currentUser.UserName
                            };
                            await _manuSfcProduceRepository.UpdateProcedureAndStatusRangeAsync(updateProcedureAndStatusCommand);
                        }
                    }
                    else
                    {
                        //指定活动或排队的
                        //写步骤
                        await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);
                        //指定工序
                        var updateProcedureAndStatusCommand = new UpdateProcedureAndStatusCommand
                        {
                            Sfcs = sfcsArr,
                            ProcedureId = sfcProduceStepDto.ProcedureId,
                            Status = sfcProduceStepDto.Type,
                            UpdatedOn = HymsonClock.Now(),
                            UserId = _currentUser.UserName
                        };
                        await _manuSfcProduceRepository.UpdateProcedureAndStatusRangeAsync(updateProcedureAndStatusCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveManuSfcProduceStepAsync,在制品步骤控制保存,写库失败:", ex);
                throw new CustomerValidationException(nameof(ErrorCode.MES18016));
            }

            #endregion
        }
        #endregion

    }
}
