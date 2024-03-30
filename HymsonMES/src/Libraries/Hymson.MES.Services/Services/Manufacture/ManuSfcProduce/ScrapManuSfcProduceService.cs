using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcScrap.Command;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Manufacture.ManuSfcProduce
{
    /// <summary>
    /// 条码报废
    /// </summary>
    public partial class ManuSfcProduceService
    {
        /// <summary>
        /// 条码报废
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task QualityScrapAsync(ManuSfScrapDto parm)
        {
            #region 验证

            if (parm.Sfcs == null || parm.Sfcs.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15400));
            }
            //条码表
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SFCs = parm.Sfcs,
                SiteId = _currentSite.SiteId ?? 0,
                Type = SfcTypeEnum.Produce
            });
            //条码信息表
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(sfcEntities.Select(x => x.Id));
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = parm.Sfcs, SiteId = _currentSite.SiteId ?? 00 };
            //在制品信息
            var manuSfcProduces = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);
            List<ManuSfcScrapEntity> manuSfcScrapEntities = new();
            List<ManuSfcStepEntity> manuSfcStepEntities = new();
            List<ScrapManuSfcByIdCommand> scrapByIdCommands = new();
            var updateManuSfcProduceStatusByIdCommands = new List<UpdateManuSfcProduceStatusByIdCommand>();
            var validationFailures = new List<ValidationFailure>();
            foreach (var sfc in parm.Sfcs)
            {
                var sfcEntity = sfcEntities.FirstOrDefault(x => x.SFC == sfc);

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
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15415);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                if (sfcEntity.Status == SfcStatusEnum.Scrapping)
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
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15401);
                    validationFailures.Add(validationFailure);
                    continue;
                }


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
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15416);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                if (sfcEntity.Status == SfcStatusEnum.Complete)
                {
                    //TODO 完成品逻辑不清楚
                }

                var sfcInfoEntity = sfcInfoEntities.FirstOrDefault(x => x.SfcId == sfcEntity.Id);

                if (sfcInfoEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15401));
                }
                var manuSfcProduceInfoEntity = manuSfcProduces.FirstOrDefault(x => x.SFC == sfc);

                if (manuSfcProduceInfoEntity != null)
                {
                    updateManuSfcProduceStatusByIdCommands.Add(new UpdateManuSfcProduceStatusByIdCommand
                    {
                        Id = manuSfcProduceInfoEntity.Id,
                        Status = SfcStatusEnum.Scrapping,
                        CurrentStatus = manuSfcProduceInfoEntity.Status,
                        UpdatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentUser.UserName
                    });
                }
                var stepEntity = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfc,
                    ProductId = sfcInfoEntity.ProductId,
                    WorkOrderId = sfcInfoEntity.WorkOrderId ?? 0,
                    WorkCenterId = manuSfcProduceInfoEntity?.WorkCenterId,
                    ProductBOMId = manuSfcProduceInfoEntity?.ProductBOMId,
                    Qty = sfcEntity.Qty,
                    EquipmentId = manuSfcProduceInfoEntity?.EquipmentId,
                    ResourceId = manuSfcProduceInfoEntity?.ResourceId,
                    ProcedureId = manuSfcProduceInfoEntity?.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.Discard,
                    CurrentStatus = sfcEntity.Status,
                    Remark = parm.Remark ?? "",
                    SiteId = _currentSite.SiteId ?? 0,
                    CreatedOn = HymsonClock.Now(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName
                };
                manuSfcStepEntities.Add(stepEntity);

                var manuSfcScrapEntity = new ManuSfcScrapEntity()
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfc,
                    SfcinfoId = sfcInfoEntity?.Id ?? 0,
                    SfcStepId = stepEntity.Id,
                    ProcedureId = parm.ProcedureId ?? manuSfcProduceInfoEntity?.ProcedureId,
                    ScrapQty = sfcEntity.Qty,
                    IsCancel = false,
                    Remark = parm.Remark,
                    SiteId = _currentSite.SiteId ?? 0,
                    CreatedOn = HymsonClock.Now(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName
                };

                manuSfcScrapEntities.Add(manuSfcScrapEntity);

                scrapByIdCommands.Add(new ScrapManuSfcByIdCommand
                {
                    Id = sfcEntity.Id,
                    Status = SfcStatusEnum.Scrapping,
                    CurrentStatus = sfcEntity.Status,
                    SfcScrapId = manuSfcScrapEntity.Id,
                    UpdatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName
                });
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }

            #endregion

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {

                //1.条码信息表
                rows += await _manuSfcRepository.ManuSfcScrapByIdsAsync(scrapByIdCommands);

                if (rows != parm.Sfcs.Length)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15419));
                }

                //2.插入数据操作类型为报废
                rows += await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepEntities);

                //3.更新在制品状态
                if (updateManuSfcProduceStatusByIdCommands != null && updateManuSfcProduceStatusByIdCommands.Any())
                {
                    await _manuSfcProduceRepository.UpdateStatusByIdRangeAsync(updateManuSfcProduceStatusByIdCommands);
                }

                //4.插入报废表
                rows += await _manuSfcScrapRepository.InsertRangeAsync(manuSfcScrapEntities);

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
            if (parm.Sfcs == null || parm.Sfcs.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15400));
            }

            //条码表
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SFCs = parm.Sfcs,
                SiteId = _currentSite.SiteId ?? 0,
                Type = SfcTypeEnum.Produce
            });
            //条码信息表
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(sfcEntities.Select(x => x.Id));
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = parm.Sfcs, SiteId = _currentSite.SiteId ?? 00 };
            //在制品信息
            var manuSfcProduces = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);

            var orderIds = sfcInfoEntities.Select(x => x.WorkOrderId ?? 0).Distinct().ToArray();
            var activeOrders = await _planWorkOrderActivationRepository.GetByWorkOrderIdsAsync(orderIds);

            var validationFailures = new List<ValidationFailure>();
            List<ManuSfcScrapCancelCommand> scrapCancelCommands = new();
            List<ManuSfcStepEntity> manuSfcStepEntities = new();
            List<CancelScrapManuSfcByIdCommand> manuSfcCancelScrapByIdCommands = new();
            var updateManuSfcProduceStatusByIdCommands = new List<UpdateManuSfcProduceStatusByIdCommand>();
            foreach (var sfc in parm.Sfcs)
            {
                var sfcEntity = sfcEntities.FirstOrDefault(x => x.SFC == sfc);
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
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15415);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                //是否已经报废
                if (sfcEntity.Status != SfcStatusEnum.Scrapping)
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
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15417);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                //还原SFC表
                manuSfcCancelScrapByIdCommands.Add(new CancelScrapManuSfcByIdCommand
                {
                    Id = sfcEntity.Id,
                    UpdatedBy = _currentUser.UserName,
                    CurrentStatus = sfcEntity.Status,
                    UpdatedOn = HymsonClock.Now()
                });

                var sfcInfoEntity = sfcInfoEntities.FirstOrDefault(x => x.SfcId == sfcEntity.Id);

                if (sfcInfoEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15401));
                }

                var manuSfcProduceInfoEntity = manuSfcProduces.FirstOrDefault(x => x.SFC == sfc);
                //是否是在制品
                //在制品需要还原在制品表   在制品还原需要验证工单是否已经关闭 
                if (manuSfcProduceInfoEntity != null)
                {
                    if (!activeOrders.Any(x => x.WorkOrderId == manuSfcProduceInfoEntity.WorkOrderId))
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
                        validationFailure.ErrorCode = nameof(ErrorCode.MES15427);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    updateManuSfcProduceStatusByIdCommands.Add(new UpdateManuSfcProduceStatusByIdCommand
                    {
                        Id = manuSfcProduceInfoEntity.Id,
                        Status = sfcEntity.StatusBack,
                        CurrentStatus = manuSfcProduceInfoEntity.Status,
                        UpdatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentSite.Name
                    });
                }
                var stepEntity = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfc,
                    ProductId = sfcInfoEntity.ProductId,
                    WorkOrderId = sfcInfoEntity.WorkOrderId ?? 0,
                    WorkCenterId = manuSfcProduceInfoEntity?.WorkCenterId,
                    ProductBOMId = manuSfcProduceInfoEntity?.ProductBOMId,
                    Qty = sfcEntity.Qty,
                    EquipmentId = manuSfcProduceInfoEntity?.EquipmentId,
                    ResourceId = manuSfcProduceInfoEntity?.ResourceId,
                    ProcedureId = manuSfcProduceInfoEntity?.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.CancelDiscard,
                    CurrentStatus = sfcEntity.Status,
                    Remark = parm.Remark ?? "",
                    SiteId = _currentSite.SiteId ?? 0,
                    CreatedOn = HymsonClock.Now(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName
                };
                manuSfcStepEntities.Add(stepEntity);

                //还原报废表
                scrapCancelCommands.Add(new ManuSfcScrapCancelCommand
                {
                    Id = sfcEntity.SfcScrapId ?? 0,
                    IsCancel = true,
                    CancelSfcStepId = sfcEntity.Id,
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                }
                );
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }
            #endregion

            //入库
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //1.条码信息表状态更改
                rows += await _manuSfcRepository.ManuSfcCancellScrapByIdsAsync(manuSfcCancelScrapByIdCommands);

                if (rows != parm.Sfcs.Length)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15419));
                }

                //更新在制品状态
                if (updateManuSfcProduceStatusByIdCommands != null && updateManuSfcProduceStatusByIdCommands.Any())
                {
                    await _manuSfcProduceRepository.UpdateStatusByIdRangeAsync(updateManuSfcProduceStatusByIdCommands);
                }

                //3.插入数据操作类型为取消报废
                rows += await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepEntities);

                //4.修改报废表
                rows += await _manuSfcScrapRepository.ManuSfcScrapCancelAsync(scrapCancelCommands);

                trans.Complete();
            }
        }

    }
}
