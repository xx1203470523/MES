using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Text.Json;

namespace Hymson.MES.Services.Services.Manufacture.ManuSfcProduce
{
    /// <summary>
    /// 条码锁定
    /// </summary>
    public partial class ManuSfcProduceService
    {
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
                if (sfcEntity.Status == SfcStatusEnum.Locked)
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
                if (!await _manuCommonOldService.IsProcessStartBeforeEndAsync(sfcEntity.ProcessRouteId, sfcEntity.ProcedureId, parm.LockProductionId))
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

                    var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(sfcEntity.ProcedureId);

                    var lockProcProcedureEntity = await _procProcedureRepository.GetByIdAsync(parm.LockProductionId);

                    validationFailure.FormattedMessagePlaceholderValues.Add("sfcproduction", procProcedureEntity.Code);
                    validationFailure.FormattedMessagePlaceholderValues.Add("lockproductionname", lockProcProcedureEntity.Code);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15314);
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
                   ProcessRouteId = sfc.ProcessRouteId,
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

            if (sfcList == null || !sfcList.Any())
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
                if (sfcEntity.Status == SfcStatusEnum.Locked)
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
            List<ManuSfcUpdateStatusByIdCommand> manuSfcUpdateStatusByIdCommands = new();
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
                    ProcessRouteId = sfc.ProcessRouteId,
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

                manuSfcUpdateStatusByIdCommands.Add(new ManuSfcUpdateStatusByIdCommand
                {
                    Id = sfc.SFCId,
                    CurrentStatus = sfc.Status,
                    Status = SfcStatusEnum.Locked,
                    UpdatedBy = _currentUser.UserName
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
                //条码状态修改
                var row = await _manuSfcRepository.ManuSfcUpdateStatuByIdRangeAsync(manuSfcUpdateStatusByIdCommands);
                if (row != manuSfcUpdateStatusByIdCommands.Count)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15426));
                }
                //插入操作数据
                await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);

                //插入步骤业务表
                await _manuSfcStepRepository.InsertSfcStepBusinessRangeAsync(sfcStepBusinessList);

                //修改在制品表状态
                await _manuSfcProduceRepository.LockedSfcProcedureAsync(new LockedProcedureCommand
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Sfcs = parm.Sfcs,
                    UserId = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    Status = SfcStatusEnum.Locked
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
                if (sfcProduceBusinessEntity == null && sfcEntity.Status != SfcStatusEnum.Locked)
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
            List<ManuSfcUpdateStatusByIdCommand> manuSfcUpdateStatusByIdCommands = new();
            var unLockList = new List<long>();
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
                    ProcessRouteId=sfc.ProcessRouteId,
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
                if (sfc.Status == SfcStatusEnum.Locked)
                {
                    manuSfcUpdateStatusByIdCommands.Add(new ManuSfcUpdateStatusByIdCommand
                    {
                        Id = sfc.SFCId,
                        CurrentStatus = sfc.Status,
                        Status = sfc.BeforeLockedStatus ?? sfc.Status,
                        UpdatedBy = _currentUser.UserName
                    });
                }
            }
            #endregion


            var lockSfc = sfcList.Where(x => x.Status == SfcStatusEnum.Locked).Select(x => x.SFC).ToArray();

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                if (unLockList != null && unLockList.Any())
                {
                    await _manuSfcProduceRepository.DeleteSfcProduceBusinesssAsync(new DeleteSfcProduceBusinesssCommand { SfcInfoIds = unLockList, BusinessType = ManuSfcProduceBusinessType.Lock });
                }
                var row = 0;
                //条码状态修改
                if (manuSfcUpdateStatusByIdCommands != null && manuSfcUpdateStatusByIdCommands.Any())
                {
                    row += await _manuSfcRepository.ManuSfcUpdateStatuByIdRangeAsync(manuSfcUpdateStatusByIdCommands);
                    if (row != manuSfcUpdateStatusByIdCommands.Count)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES15426));
                    }
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

    }
}
