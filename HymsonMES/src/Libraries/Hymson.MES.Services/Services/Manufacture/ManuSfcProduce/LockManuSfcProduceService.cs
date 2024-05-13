using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Security.Policy;
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

            if (parm.OperationObject == QualityLockObjectEnum.BarCode)
            {
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
            else if (parm.OperationObject == QualityLockObjectEnum.Material)
            {
                switch (parm.OperationType)
                {
                    case
                         QualityLockEnum.InstantLock:
                        await InstantLockMaterialAsync(new InstantLockDto
                        {
                            Sfcs = parm.Sfcs,
                        });
                        break;
                    case
                         QualityLockEnum.Unlock:
                        await UnLockMaterialAsync(new UnLockDto
                        {
                            Sfcs = parm.Sfcs,
                        });
                        break;
                    default:
                        break;
                }
            }
            else
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15332));
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


        #region 物料锁定
        /// <summary>
        /// 及时锁（物料）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private async Task InstantLockMaterialAsync(InstantLockDto parm)
        {
            var sfcs = parm.Sfcs.Distinct();
            // 库存 
            var siteId = _currentSite.SiteId ?? 0;
            var whMaterialInventorys = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery { SiteId = siteId, BarCodes = sfcs });
            if (whMaterialInventorys == null || !whMaterialInventorys.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15309));
            }
            var validationFailures = new List<ValidationFailure>();
            foreach (var materialBarCode in parm.Sfcs)
            {
                var materialBarCodeEntity = whMaterialInventorys.FirstOrDefault(x => x.MaterialBarCode == materialBarCode);
                //是否存在物料
                if (materialBarCodeEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", materialBarCode}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", materialBarCode);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15330);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                //是否被锁定
                if (materialBarCodeEntity.Status == WhMaterialInventoryStatusEnum.Locked)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", materialBarCode}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", materialBarCode);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15318);
                    validationFailures.Add(validationFailure);
                }

                //是否有库存
                if (materialBarCodeEntity.QuantityResidue <= 0)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", materialBarCode}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", materialBarCode);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15331);
                    validationFailures.Add(validationFailure);
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }

            #region 流转
            //流转
            var manuBarCodeRelations = await _manuBarCodeRelationRepository.GetEntitiesAsync(new ManuBarcodeRelationQuery { InputBarCodes = sfcs, IsDisassemble = TrueOrFalseEnum.No, SiteId = siteId });

            List<UpdateStatusByBarCodeCommand> updateStatusByBarCodeRelationList = new();
            List<WhMaterialStandingbookEntity> whMaterialStandingbookRelationList = new();
            List<ManuSfcUpdateStatusByIdCommand> manuSfcUpdateStatusByIdRelationList = new();
            List<ManuSfcStepEntity> sfcStepRelationList = new();
            List<ManuSfcProduceBusinessEntity> sfcProduceBusinessRelationList = new();
            List<MaunSfcStepBusinessEntity> sfcStepBusinessRelationList = new();
            List<string> lockedProcedureRelationSfcs = new();

            //有流转的
            if (manuBarCodeRelations != null && manuBarCodeRelations.Any())
            {
                //条码
                var relationOutputBarCodes = manuBarCodeRelations.Select(it => it.OutputBarCode);

                //物料
                var outputBarCodeMaterialId = manuBarCodeRelations.Select(it => it.OutputBarCodeMaterialId ?? 0);
                var outputBarCodeMaterials = await _procMaterialRepository.GetByIdsAsync(outputBarCodeMaterialId);

                //库存 
                var outputBarCodeInventorys = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery { SiteId = siteId, BarCodes = relationOutputBarCodes });

                //物料条码表
                var manuSfcRelations = await _manuSfcRepository.GetManuSfcInfoEntitiesAsync(new ManuSfcStatusQuery { SiteId = siteId, Sfcs = relationOutputBarCodes });

                //物料在制
                var manuSfcProduceRelations = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery
                {
                    Sfcs = relationOutputBarCodes,
                    SiteId = siteId
                });

                foreach (var item in manuBarCodeRelations)
                {
                    var procMaterial = outputBarCodeMaterials.FirstOrDefault(it => it.Id == item.OutputBarCodeMaterialId);
                    //有库存直接锁库存
                    if (outputBarCodeInventorys != null && outputBarCodeInventorys.Any())
                    {
                        var outputBarCodeInventory = outputBarCodeInventorys.FirstOrDefault(it => it.MaterialBarCode == item.OutputBarCode);
                        if (outputBarCodeInventory != null && outputBarCodeInventory.QuantityResidue > 0)
                        {
                            //库存
                            UpdateStatusByBarCodeCommand updateStatusByBarCode = new()
                            {
                                BarCode = item.OutputBarCode,
                                Status = WhMaterialInventoryStatusEnum.Locked,
                                SiteId = siteId,
                                UpdatedBy = _currentUser.UserName,
                                UpdatedOn = HymsonClock.Now()
                            };
                            updateStatusByBarCodeRelationList.Add(updateStatusByBarCode);

                            //台账
                            var whMaterialStandingbookEntity = new WhMaterialStandingbookEntity
                            {
                                MaterialCode = procMaterial?.MaterialCode ?? "",
                                MaterialName = procMaterial?.MaterialName ?? "",
                                MaterialVersion = procMaterial?.Version ?? "",
                                Unit = procMaterial?.Unit ?? "",

                                MaterialBarCode = item.OutputBarCode,
                                Type = WhMaterialInventoryTypeEnum.MaterialBarCodeLock,
                                Source = MaterialInventorySourceEnum.ManualEntry,
                                SiteId = _currentSite.SiteId ?? 0,

                                Batch = outputBarCodeInventory.Batch,
                                Quantity = outputBarCodeInventory.QuantityResidue,
                                SupplierId = outputBarCodeInventory.SupplierId,

                                Id = IdGenProvider.Instance.CreateId(),
                                CreatedBy = _currentUser.UserName,
                                UpdatedBy = _currentUser.UserName,
                                CreatedOn = HymsonClock.Now(),
                                UpdatedOn = HymsonClock.Now()
                            };
                            whMaterialStandingbookRelationList.Add(whMaterialStandingbookEntity);
                        }
                    }
                    //没有库存锁在制
                    else
                    {
                        //条码
                        var manuSfcRelation = manuSfcRelations.Where(it => it.SFC == item.OutputBarCode).FirstOrDefault();
                        if (manuSfcRelation != null)
                        {
                            ManuSfcUpdateStatusByIdCommand manuSfcUpdateStatusByIdCommand = new()
                            {
                                Id = manuSfcRelation.Id,
                                CurrentStatus = manuSfcRelation.Status,
                                Status = SfcStatusEnum.Locked,
                                UpdatedBy = _currentUser.UserName,
                                UpdatedOn = HymsonClock.Now(),
                            };
                            manuSfcUpdateStatusByIdRelationList.Add(manuSfcUpdateStatusByIdCommand);
                        }

                        //在制
                        var manuSfcProduceRelation = manuSfcProduceRelations.Where(it => it.SFC == item.OutputBarCode).FirstOrDefault();
                        if (manuSfcProduceRelation != null)
                        {
                            SfcProduceLockBo sfcProduceLockBo = new SfcProduceLockBo()
                            {
                                Lock = QualityLockEnum.InstantLock
                            };

                            //步骤
                            var stepEntity = new ManuSfcStepEntity
                            {
                                Id = IdGenProvider.Instance.CreateId(),
                                SiteId = _currentSite.SiteId ?? 0,
                                SFC = manuSfcProduceRelation.SFC,
                                ProductId = manuSfcProduceRelation.ProductId,
                                WorkOrderId = manuSfcProduceRelation.WorkOrderId,
                                WorkCenterId = manuSfcProduceRelation.WorkCenterId,
                                ProductBOMId = manuSfcProduceRelation.ProductBOMId,
                                Qty = manuSfcProduceRelation.Qty,
                                EquipmentId = manuSfcProduceRelation.EquipmentId,
                                ResourceId = manuSfcProduceRelation.ResourceId,
                                ProcedureId = manuSfcProduceRelation.ProcedureId,
                                Operatetype = ManuSfcStepTypeEnum.InstantLock,
                                CurrentStatus = manuSfcProduceRelation.Status,
                                CreatedBy = manuSfcProduceRelation.CreatedBy,
                                UpdatedBy = manuSfcProduceRelation.UpdatedBy
                            };
                            sfcStepRelationList.Add(stepEntity);

                            //步骤业务
                            sfcStepBusinessRelationList.Add(new MaunSfcStepBusinessEntity
                            {
                                Id = IdGenProvider.Instance.CreateId(),
                                SiteId = _currentSite.SiteId ?? 0,
                                SfcStepId = stepEntity.Id,
                                BusinessType = ManuSfcProduceBusinessType.Lock,
                                BusinessContent = JsonSerializer.Serialize(sfcProduceLockBo),
                                CreatedBy = manuSfcProduceRelation.CreatedBy,
                                UpdatedBy = manuSfcProduceRelation.UpdatedBy
                            });

                            //在制
                            lockedProcedureRelationSfcs.Add(item.OutputBarCode);

                            //在制业务
                            sfcProduceBusinessRelationList.Add(new ManuSfcProduceBusinessEntity
                            {
                                Id = IdGenProvider.Instance.CreateId(),
                                SiteId = _currentSite.SiteId ?? 0,
                                SfcProduceId = manuSfcProduceRelation.Id,
                                BusinessType = ManuSfcProduceBusinessType.Lock,
                                BusinessContent = JsonSerializer.Serialize(sfcProduceLockBo),
                                CreatedBy = manuSfcProduceRelation.CreatedBy,
                                UpdatedBy = manuSfcProduceRelation.UpdatedBy
                            });
                        }
                    }
                }

            }
            #endregion

            #region 物料库存
            //物料
            var procMaterials = await _procMaterialRepository.GetByIdsAsync(whMaterialInventorys.Select(it => it.MaterialId));
            List<UpdateStatusByBarCodeCommand> updateStatusByBarCodeList = new();
            List<WhMaterialStandingbookEntity> whMaterialStandingbookList = new();
            foreach (var item in whMaterialInventorys)
            {
                var procMaterial = procMaterials.FirstOrDefault(it => it.Id == item.MaterialId);

                //库存
                UpdateStatusByBarCodeCommand updateStatusByBarCode = new()
                {
                    BarCode = item.MaterialBarCode,
                    Status = WhMaterialInventoryStatusEnum.Locked,
                    SiteId = siteId,
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                };
                updateStatusByBarCodeList.Add(updateStatusByBarCode);

                //台账
                var whMaterialStandingbookEntity = new WhMaterialStandingbookEntity
                {
                    MaterialCode = procMaterial?.MaterialCode ?? "",
                    MaterialName = procMaterial?.MaterialName ?? "",
                    MaterialVersion = procMaterial?.Version ?? "",
                    Unit = procMaterial?.Unit ?? "",

                    MaterialBarCode = item.MaterialBarCode,
                    Type = WhMaterialInventoryTypeEnum.MaterialBarCodeLock,
                    Source = MaterialInventorySourceEnum.ManualEntry,
                    SiteId = _currentSite.SiteId ?? 0,

                    Batch = item.Batch,
                    Quantity = item.QuantityResidue,
                    SupplierId = item.SupplierId,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now()
                };
                whMaterialStandingbookList.Add(whMaterialStandingbookEntity);
            }

            #endregion

            #region 入库
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //更新库存状态
                await _whMaterialInventoryRepository.UpdateStatusByBarCodesAsync(updateStatusByBarCodeList);

                //添加台账
                await _whMaterialStandingbookRepository.InsertsAsync(whMaterialStandingbookList);

                //如果有使用
                if (manuBarCodeRelations != null && manuBarCodeRelations.Any())
                {
                    //物料库存状态
                    if (updateStatusByBarCodeRelationList.Any())
                    {
                        await _whMaterialInventoryRepository.UpdateStatusByBarCodesAsync(updateStatusByBarCodeRelationList);
                    }

                    //添加台账
                    if (whMaterialStandingbookRelationList.Any())
                    {
                        await _whMaterialStandingbookRepository.InsertsAsync(whMaterialStandingbookRelationList);
                    }

                    //条码
                    if (manuSfcUpdateStatusByIdRelationList.Any())
                    {
                        var row = await _manuSfcRepository.ManuSfcUpdateStatuByIdRangeAsync(manuSfcUpdateStatusByIdRelationList);
                        if (row != manuSfcUpdateStatusByIdRelationList.Count)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES15426));
                        }
                    }

                    //修改在制品表状态
                    if (lockedProcedureRelationSfcs.Any())
                    {
                        await _manuSfcProduceRepository.LockedSfcProcedureAsync(new LockedProcedureCommand
                        {
                            SiteId = _currentSite.SiteId ?? 0,
                            Sfcs = lockedProcedureRelationSfcs,
                            UserId = _currentUser.UserName,
                            UpdatedOn = HymsonClock.Now(),
                            Status = SfcStatusEnum.Locked
                        });
                    }

                    //在制业务
                    if (sfcProduceBusinessRelationList != null && sfcProduceBusinessRelationList.Any())
                    {
                        await _manuSfcProduceRepository.InsertOrUpdateSfcProduceBusinessRangeAsync(sfcProduceBusinessRelationList);
                    }

                    //步骤
                    if (sfcStepRelationList.Any())
                    {
                        await _manuSfcStepRepository.InsertRangeAsync(sfcStepRelationList);
                    }

                    //插入步骤业务表
                    if (sfcStepRelationList.Any())
                    {
                        await _manuSfcStepRepository.InsertSfcStepBusinessRangeAsync(sfcStepBusinessRelationList);
                    }
                }

                trans.Complete();
            }

            #endregion
        }



        /// <summary>
        /// 解除锁（物料）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private async Task UnLockMaterialAsync(UnLockDto parm)
        {
            var sfcs = parm.Sfcs.Distinct();
            // 库存 
            var siteId = _currentSite.SiteId ?? 0;
            var whMaterialInventorys = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery { SiteId = siteId, BarCodes = sfcs });
            if (whMaterialInventorys == null || !whMaterialInventorys.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15309));
            }
            var validationFailures = new List<ValidationFailure>();
            foreach (var materialBarCode in parm.Sfcs)
            {
                var materialBarCodeEntity = whMaterialInventorys.FirstOrDefault(x => x.MaterialBarCode == materialBarCode);
                //是否存在物料
                if (materialBarCodeEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", materialBarCode}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", materialBarCode);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15330);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                //是否被锁定
                if (materialBarCodeEntity.Status != WhMaterialInventoryStatusEnum.Locked)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", materialBarCode}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", materialBarCode);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15316);
                    validationFailures.Add(validationFailure);
                }

                //是否有库存
                if (materialBarCodeEntity.QuantityResidue <= 0)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", materialBarCode}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", materialBarCode);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15331);
                    validationFailures.Add(validationFailure);
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }

            #region 物料库存
            //物料
            var procMaterials = await _procMaterialRepository.GetByIdsAsync(whMaterialInventorys.Select(it => it.MaterialId));
            List<UpdateStatusByBarCodeCommand> updateStatusByBarCodeList = new();
            List<WhMaterialStandingbookEntity> whMaterialStandingbookList = new();
            foreach (var item in whMaterialInventorys)
            {
                var procMaterial = procMaterials.FirstOrDefault(it => it.Id == item.MaterialId);

                //库存
                UpdateStatusByBarCodeCommand updateStatusByBarCode = new()
                {
                    BarCode = item.MaterialBarCode,
                    Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                    SiteId = siteId,
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                };
                updateStatusByBarCodeList.Add(updateStatusByBarCode);

                //台账
                var whMaterialStandingbookEntity = new WhMaterialStandingbookEntity
                {
                    MaterialCode = procMaterial?.MaterialCode ?? "",
                    MaterialName = procMaterial?.MaterialName ?? "",
                    MaterialVersion = procMaterial?.Version ?? "",
                    Unit = procMaterial?.Unit ?? "",

                    MaterialBarCode = item.MaterialBarCode,
                    Type = WhMaterialInventoryTypeEnum.MaterialBarCodeUnLock,
                    Source = MaterialInventorySourceEnum.ManualEntry,
                    SiteId = _currentSite.SiteId ?? 0,

                    Batch = item.Batch,
                    Quantity = item.QuantityResidue,
                    SupplierId = item.SupplierId,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now()
                };
                whMaterialStandingbookList.Add(whMaterialStandingbookEntity);
            }

            #endregion

            #region 入库
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //更新库存状态
                await _whMaterialInventoryRepository.UpdateStatusByBarCodesAsync(updateStatusByBarCodeList);

                //添加台账
                await _whMaterialStandingbookRepository.InsertsAsync(whMaterialStandingbookList);

                trans.Complete();
            }

            #endregion
        }

        #endregion

    }
}
