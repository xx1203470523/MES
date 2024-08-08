using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.WaterMark;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuSfcSummary
{
    /// <summary>
    /// 生产统计
    /// </summary>
    public class ManuSfcSummaryService : IManuSfcSummaryService
    {
        /// <summary>
        /// 日志服务接口
        /// </summary>
        private readonly ILogger<ManuSfcSummaryService> _logger;

        public readonly IWaterMarkService _waterMarkService;
        public readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 集合仓储
        /// </summary>
        public readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;

        /// <summary>
        /// 不合格记录仓储
        /// </summary>
        public readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 报废 仓储
        /// </summary>
        public readonly IManuSfcScrapRepository _manuSfcScrapRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="waterMarkService"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcSummaryRepository"></param>
        /// <param name="manuSfcScrapRepository"></param>
        /// <param name="manuProductBadRecordRepository"></param>
        public ManuSfcSummaryService(ILogger<ManuSfcSummaryService> logger,
            IWaterMarkService waterMarkService,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcSummaryRepository manuSfcSummaryRepository,
            IManuSfcScrapRepository manuSfcScrapRepository,
            IManuProductBadRecordRepository manuProductBadRecordRepository)
        {
            _logger = logger;
            _waterMarkService = waterMarkService;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcSummaryRepository = manuSfcSummaryRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _manuSfcScrapRepository = manuSfcScrapRepository;
        }

        /// <summary>
        /// 执行生产统计
        /// </summary>
        /// <returns></returns>
        public async Task ExecutStatisticAsync(string userId)
        {
            var startWaterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.ManuSfcSummary);

            //获取步骤表数据
            var manuSfcStepList = await _manuSfcStepRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            {
                StartWaterMarkId = startWaterMarkId,
                Rows = 500
            });

            if (manuSfcStepList == null || !manuSfcStepList.Any())
            {
                _logger.LogDebug($"生产统计 -> 没有新产生的的步骤数据！waterMarkId:{startWaterMarkId}");
                return;
            }

            if (manuSfcStepList != null && manuSfcStepList.Any())
            {
                //条码工序统计最后一条数据
                var manuSfcSummaryProcedureLastList = await _manuSfcSummaryRepository.GetyLastListByProcedureIdsAndSfcsAsync(new LastManuSfcSummaryByProcedureIdAndSfcsQuery
                {
                    Sfcs = manuSfcStepList.Select(x => x.SFC)
                });

                //录入不良列表
                var manuProductBadRecordList = await _manuProductBadRecordRepository.GetBySfcStepIdsAsync(manuSfcStepList.Select(x => x.Id));
                //复判列表
                var reJudgmentSfcStepIdsProductBadRecordList = await _manuProductBadRecordRepository.GetByReJudgmentSfcStepIdsAsync(manuSfcStepList.Select(x => x.Id));
                //报废列表
                var manuSfcScrapList = await _manuSfcScrapRepository.GetByStepIdsAsync(manuSfcStepList.Select(x => x.Id));
                //获取取消报废的列表
                var cancelManuSfcScrapList = await _manuSfcScrapRepository.GetByCancelSfcStepIdsAsync(manuSfcStepList.Select(x => x.Id));

                List<ManuSfcSummaryEntity> manuSfcSummaryList = new List<ManuSfcSummaryEntity>();
                var groupManuSfcStepList = manuSfcStepList.GroupBy(x => x.SFC);
                foreach (var groupItem in groupManuSfcStepList)
                {
                    foreach (var item in groupItem)
                    {
                        switch (item.Operatetype)
                        {
                            case ManuSfcStepTypeEnum.Create:
                                break;
                            case ManuSfcStepTypeEnum.Receive:
                                break;
                            case ManuSfcStepTypeEnum.InStock:
                                var sfcSummary = new ManuSfcSummaryEntity
                                {
                                    Id = IdGenProvider.Instance.CreateId(),
                                    SiteId = item.SiteId,
                                    SFC = item.SFC,
                                    WorkOrderId = item.WorkOrderId,
                                    ProductId = item.ProductId,
                                    ProcedureId = item.ProcedureId,
                                    StartOn = item.CreatedOn,
                                    InvestQty = item.Qty,
                                    RepeatedCount = item.RepeatedCount,
                                    CreatedOn = HymsonClock.Now(),
                                    CreatedBy = userId,
                                    UpdatedBy = userId,
                                    UpdatedOn = HymsonClock.Now()
                                };
                                //进站新建统计表
                                manuSfcSummaryList.Add(sfcSummary);
                                break;
                            case ManuSfcStepTypeEnum.OutStock:
                                var lastmanuSfcSummary = GetLastManuSfcSummary(manuSfcSummaryProcedureLastList, manuSfcSummaryList, groupItem.Key, item.ProcedureId ?? 0);
                                if (lastmanuSfcSummary != null)
                                {
                                    lastmanuSfcSummary.UpdatedBy = userId;
                                    lastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                    lastmanuSfcSummary.OutputQty = item.Qty;
                                    lastmanuSfcSummary.EndOn = item.CreatedOn;
                                    var manusfcProductBadRecords = manuProductBadRecordList.Where(x => x.SfcStepId == item.Id);
                                    if (manusfcProductBadRecords != null && manusfcProductBadRecords.Any(x => x.DisposalResult == ProductBadDisposalResultEnum.WaitingJudge))
                                    {
                                        lastmanuSfcSummary.IsJudgment = true;
                                    }
                                }
                                else
                                {
                                    //步骤控制处理的暂不处理
                                }
                                break;
                            case ManuSfcStepTypeEnum.FutureLock:
                                break;
                            case ManuSfcStepTypeEnum.InstantLock:
                                break;
                            case ManuSfcStepTypeEnum.Unlock:
                                break;
                            case ManuSfcStepTypeEnum.BadEntry:
                                //录入不良
                                var manuProductBadRecords = manuProductBadRecordList.Where(x => x.SfcStepId == item.Id);
                                if (manuProductBadRecords.Any(x => x.DisposalResult == ProductBadDisposalResultEnum.Repair || x.DisposalResult == ProductBadDisposalResultEnum.Scrap))
                                {
                                    var manuProductBadRecord = manuProductBadRecords.FirstOrDefault(x => x.DisposalResult == ProductBadDisposalResultEnum.Repair ||
                                                                                                         x.DisposalResult == ProductBadDisposalResultEnum.Scrap);
                                    if (manuProductBadRecord != null)
                                    {
                                        if (item.ProcedureId == manuProductBadRecord.FoundBadOperationId && item.CurrentStatus == SfcStatusEnum.Activity)
                                        {
                                            var badEntrylastManuSfcSummary = GetLastManuSfcSummary(manuSfcSummaryProcedureLastList, manuSfcSummaryList, groupItem.Key, item.ProcedureId ?? 0);
                                            if (badEntrylastManuSfcSummary != null)
                                            {
                                                badEntrylastManuSfcSummary.UpdatedBy = userId;
                                                badEntrylastManuSfcSummary.UpdatedOn = HymsonClock.Now();
                                                badEntrylastManuSfcSummary.OutputQty = item.Qty;
                                                badEntrylastManuSfcSummary.UnqualifiedQty = item.Qty;
                                                badEntrylastManuSfcSummary.EndOn = item.CreatedOn;
                                            }
                                            else
                                            {
                                                //步骤控制处理的暂不处理
                                            }
                                        }
                                        else
                                        {
                                            //条码状态为活动中，记录不良存在报废或者返修，即判定当前条码是不良的，当时报废工序和当前工序不一样， 即需要更新当前产出
                                            if (item.ProcedureId != manuProductBadRecord.FoundBadOperationId && item.CurrentStatus == SfcStatusEnum.Activity)
                                            {
                                                var badEntrylastManuSfcSummary = GetLastManuSfcSummary(manuSfcSummaryProcedureLastList, manuSfcSummaryList, groupItem.Key, item.ProcedureId ?? 0);
                                                if (badEntrylastManuSfcSummary != null)
                                                {
                                                    badEntrylastManuSfcSummary.UpdatedBy = userId;
                                                    badEntrylastManuSfcSummary.UpdatedOn = HymsonClock.Now();
                                                    badEntrylastManuSfcSummary.OutputQty = item.Qty;
                                                    badEntrylastManuSfcSummary.EndOn = item.CreatedOn;
                                                }
                                                else
                                                {
                                                    //步骤控制处理的暂不处理
                                                }
                                            }
                                            var badEntryLastmanuSfcSummary = GetLastManuSfcSummary(manuSfcSummaryProcedureLastList, manuSfcSummaryList, groupItem.Key, manuProductBadRecord.FoundBadOperationId);
                                            if (badEntryLastmanuSfcSummary != null)
                                            {
                                                badEntryLastmanuSfcSummary.UpdatedBy = userId;
                                                badEntryLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                                badEntryLastmanuSfcSummary.UnqualifiedQty = item.Qty;
                                                badEntryLastmanuSfcSummary.LastUpdatedOn = item.CreatedOn;
                                            }
                                        }
                                    }
                                }
                                break;
                            case ManuSfcStepTypeEnum.Discard:
                                var manuSfcScrap = manuSfcScrapList.FirstOrDefault(x => x.SfcStepId == item.Id && x.SFC == item.SFC);
                                if (manuSfcScrap != null)
                                {
                                    if (item.ProcedureId == manuSfcScrap.ProcedureId && item.CurrentStatus == SfcStatusEnum.Activity)
                                    {
                                        var discardLastmanuSfcSummary = GetLastManuSfcSummary(manuSfcSummaryProcedureLastList, manuSfcSummaryList, groupItem.Key, item.ProcedureId ?? 0);
                                        if (discardLastmanuSfcSummary != null)
                                        {
                                            discardLastmanuSfcSummary.UpdatedBy = userId;
                                            discardLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                            discardLastmanuSfcSummary.OutputQty = item.Qty;
                                            discardLastmanuSfcSummary.UnqualifiedQty = item.Qty;
                                        }
                                        else
                                        {
                                            //步骤控制处理的暂不处理
                                        }
                                    }
                                    else
                                    {
                                        //条码状态为活动中，记录不良存在报废或者返修，即判定当前条码是不良的，当时报废工序和当前工序不一样， 即需要更新当前产出
                                        if (item.ProcedureId != manuSfcScrap.ProcedureId && item.CurrentStatus == SfcStatusEnum.Activity)
                                        {
                                            var discardbeforeLastmanuSfcSummary = GetLastManuSfcSummary(manuSfcSummaryProcedureLastList, manuSfcSummaryList, groupItem.Key, item.ProcedureId ?? 0);
                                            if (discardbeforeLastmanuSfcSummary != null)
                                            {
                                                discardbeforeLastmanuSfcSummary.UpdatedBy = userId;
                                                discardbeforeLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                                discardbeforeLastmanuSfcSummary.OutputQty = item.Qty;
                                                discardbeforeLastmanuSfcSummary.EndOn = item.CreatedOn;
                                            }
                                            else
                                            {
                                                //步骤控制处理的暂不处理
                                            }
                                        }
                                        var discardLastmanuSfcSummary = GetLastManuSfcSummary(manuSfcSummaryProcedureLastList, manuSfcSummaryList, groupItem.Key, manuSfcScrap.ProcedureId ?? 0);
                                        if (discardLastmanuSfcSummary != null)
                                        {
                                            discardLastmanuSfcSummary.LastUpdatedOn = item.CreatedOn;
                                            discardLastmanuSfcSummary.UpdatedBy = userId;
                                            discardLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                            discardLastmanuSfcSummary.UnqualifiedQty = item.Qty;
                                        }
                                    }
                                }
                                break;
                            case ManuSfcStepTypeEnum.Stop:
                                var stopLastmanuSfcSummary = GetLastManuSfcSummary(manuSfcSummaryProcedureLastList, manuSfcSummaryList, groupItem.Key, item.ProcedureId ?? 0);
                                if (stopLastmanuSfcSummary != null)
                                {
                                    stopLastmanuSfcSummary.InvestQty = 0;
                                    stopLastmanuSfcSummary.UnqualifiedQty = 0;
                                    stopLastmanuSfcSummary.OutputQty = 0;
                                    stopLastmanuSfcSummary.EndOn = item.CreatedOn;
                                    stopLastmanuSfcSummary.UpdatedBy = userId;
                                    stopLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                }
                                break;
                            case ManuSfcStepTypeEnum.BadRejudgment:
                                //复判暂定一个工序的异常
                                var reJudgmentSfcStepIdsProductBadRecord = reJudgmentSfcStepIdsProductBadRecordList.FirstOrDefault(x => x.ReJudgmentSfcStepId == item.Id &&
                                (x.ReJudgmentResult == ProductBadDisposalResultEnum.Repair || x.ReJudgmentResult == ProductBadDisposalResultEnum.Scrap));
                                if (reJudgmentSfcStepIdsProductBadRecord != null)
                                {
                                    var badRejudgmentLastmanuSfcSummary = GetLastManuSfcSummary(manuSfcSummaryProcedureLastList, manuSfcSummaryList, groupItem.Key, reJudgmentSfcStepIdsProductBadRecord.FoundBadOperationId);
                                    if (badRejudgmentLastmanuSfcSummary != null)
                                    {
                                        badRejudgmentLastmanuSfcSummary.UnqualifiedQty = item.Qty;
                                        badRejudgmentLastmanuSfcSummary.JudgmentOn = item.CreatedOn;
                                        badRejudgmentLastmanuSfcSummary.UpdatedBy = userId;
                                        badRejudgmentLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                        badRejudgmentLastmanuSfcSummary.LastUpdatedOn = item.CreatedOn;
                                    }

                                }
                                else
                                {
                                    reJudgmentSfcStepIdsProductBadRecord = reJudgmentSfcStepIdsProductBadRecordList.FirstOrDefault();
                                    if (reJudgmentSfcStepIdsProductBadRecord != null)
                                    {
                                        var badRejudgmentLastmanuSfcSummary = GetLastManuSfcSummary(manuSfcSummaryProcedureLastList, manuSfcSummaryList, groupItem.Key, reJudgmentSfcStepIdsProductBadRecord.FoundBadOperationId);
                                        if (badRejudgmentLastmanuSfcSummary != null)
                                        {
                                            badRejudgmentLastmanuSfcSummary.JudgmentOn = item.CreatedOn;
                                            badRejudgmentLastmanuSfcSummary.UpdatedBy = userId;
                                            badRejudgmentLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                            badRejudgmentLastmanuSfcSummary.LastUpdatedOn = item.CreatedOn;
                                        }
                                    }
                                }
                                break;
                            case ManuSfcStepTypeEnum.Change:

                                var changeLastmanuSfcSummary = GetLastManuSfcSummary(manuSfcSummaryProcedureLastList, manuSfcSummaryList, groupItem.Key, item.ProcedureId ?? 0);
                                if (changeLastmanuSfcSummary != null)
                                {
                                    changeLastmanuSfcSummary.OutputQty = 0;
                                    changeLastmanuSfcSummary.EndOn = item.CreatedOn;
                                    changeLastmanuSfcSummary.UpdatedBy = userId;
                                    changeLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                }
                                break;
                            case ManuSfcStepTypeEnum.CancelDiscard:
                                var cancelManuSfcScrap = cancelManuSfcScrapList.FirstOrDefault(x => x.CancelSfcStepId == item.Id);
                                if (cancelManuSfcScrap != null && cancelManuSfcScrap.ProcedureId.HasValue)
                                {
                                    var cancelDiscardLastmanuSfcSummary = GetLastManuSfcSummary(manuSfcSummaryProcedureLastList, manuSfcSummaryList, groupItem.Key, cancelManuSfcScrap.ProcedureId ?? 0);
                                    if (cancelDiscardLastmanuSfcSummary != null)
                                    {
                                        var stepEntity = await _manuSfcStepRepository.GetByIdAsync(cancelManuSfcScrap.SfcStepId);
                                        if (stepEntity.CurrentStatus == SfcStatusEnum.Activity)
                                        {
                                            cancelDiscardLastmanuSfcSummary.UnqualifiedQty = 0;
                                            cancelDiscardLastmanuSfcSummary.EndOn = null;
                                            cancelDiscardLastmanuSfcSummary.OutputQty = 0;
                                        }
                                        else
                                        {
                                            cancelDiscardLastmanuSfcSummary.UnqualifiedQty = 0;
                                        }
                                        cancelDiscardLastmanuSfcSummary.UpdatedBy = userId;
                                        cancelDiscardLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                    }
                                }
                                break;
                            case ManuSfcStepTypeEnum.CloseIdentification:
                                break;
                            case ManuSfcStepTypeEnum.CloseDefect:
                                break;
                            case ManuSfcStepTypeEnum.Delete:
                                break;
                            case ManuSfcStepTypeEnum.Repair:
                                break;
                            case ManuSfcStepTypeEnum.StepControl:
                                break;
                            case ManuSfcStepTypeEnum.ManuUpdate:
                                break;
                            case ManuSfcStepTypeEnum.RepairReturn:
                                break;
                            case ManuSfcStepTypeEnum.Disassembly:
                                break;
                            case ManuSfcStepTypeEnum.Add:
                                break;
                            case ManuSfcStepTypeEnum.Assemble:
                                break;
                            case ManuSfcStepTypeEnum.Replace:
                                break;
                            case ManuSfcStepTypeEnum.Package:
                                break;
                            case ManuSfcStepTypeEnum.Unpack:
                                break;
                            case ManuSfcStepTypeEnum.EnterDowngrading:
                                break;
                            case ManuSfcStepTypeEnum.RemoveDowngrading:
                                break;
                        }
                    }
                }
                using var trans = TransactionHelper.GetTransactionScope();
                if (manuSfcSummaryList != null && manuSfcSummaryList.Any())
                {
                    await _manuSfcSummaryRepository.MergeRangeAsync(manuSfcSummaryList);
                }
                await _waterMarkService.RecordWaterMarkAsync(BusinessKey.ManuSfcSummary, manuSfcStepList.Max(x => x.Id));
                trans.Complete();
            }
        }

        /// <summary>
        /// 执行生产统计（新）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task ExecuteStatisticAsync(string userId, int limitCount = 500)
        {
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.ManuSfcSummary);

            // 获取步骤表数据
            var manuSfcStepList = await _manuSfcStepRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            {
                StartWaterMarkId = waterMarkId,
                Rows = limitCount
            });

            if (manuSfcStepList == null || !manuSfcStepList.Any())
            {
                _logger.LogDebug($"生产统计 -> 没有新产生的的步骤数据！waterMarkId:{waterMarkId}");
                return;
            }

            // 条码工序统计最后一条数据
            var manuSfcSummaryProcedureLastList = await _manuSfcSummaryRepository.GetyLastListByProcedureIdsAndSfcsAsync(new LastManuSfcSummaryByProcedureIdAndSfcsQuery
            {
                Sfcs = manuSfcStepList.Select(x => x.SFC)
            });

            // 步骤ID集合
            var manuStepIds = manuSfcStepList.Select(x => x.Id);

            // 录入不良列表
            var manuProductBadRecordList = await _manuProductBadRecordRepository.GetBySfcStepIdsAsync(manuStepIds);
            // 复判列表
            var reJudgmentSfcStepIdsProductBadRecordList = await _manuProductBadRecordRepository.GetByReJudgmentSfcStepIdsAsync(manuStepIds);
            // 报废列表
            var manuSfcScrapList = await _manuSfcScrapRepository.GetByStepIdsAsync(manuStepIds);
            // 获取取消报废的列表
            var cancelManuSfcScrapList = await _manuSfcScrapRepository.GetByCancelSfcStepIdsAsync(manuStepIds);

            List<ManuSfcSummaryEntity> manuSfcSummaryList = new();
            var groupManuSfcStepList = manuSfcStepList.GroupBy(x => x.SFC);
            foreach (var groupItem in groupManuSfcStepList)
            {
                foreach (var item in groupItem)
                {
                    switch (item.Operatetype)
                    {
                        case ManuSfcStepTypeEnum.Create:
                            break;
                        case ManuSfcStepTypeEnum.Receive:
                            break;
                        case ManuSfcStepTypeEnum.InStock:
                            // 进站新建统计表
                            manuSfcSummaryList.Add(new ManuSfcSummaryEntity
                            {
                                Id = IdGenProvider.Instance.CreateId(),
                                SiteId = item.SiteId,
                                SFC = item.SFC,
                                WorkOrderId = item.WorkOrderId,
                                ProductId = item.ProductId,
                                ProcedureId = item.ProcedureId,
                                StartOn = item.CreatedOn,
                                InvestQty = item.Qty,
                                RepeatedCount = item.RepeatedCount,
                                CreatedOn = HymsonClock.Now(),
                                CreatedBy = userId,
                                UpdatedBy = userId,
                                UpdatedOn = HymsonClock.Now()
                            });
                            break;
                        case ManuSfcStepTypeEnum.OutStock:
                            var lastmanuSfcSummary = GetLastManuSfcSummary(ref manuSfcSummaryList, manuSfcSummaryProcedureLastList, groupItem.Key, item.ProcedureId ?? 0);

                            // 步骤控制处理的暂不处理
                            if (lastmanuSfcSummary == null) continue;

                            lastmanuSfcSummary.UpdatedBy = userId;
                            lastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                            lastmanuSfcSummary.OutputQty = item.Qty;
                            lastmanuSfcSummary.EndOn = item.CreatedOn;
                            var manusfcProductBadRecords = manuProductBadRecordList.Where(x => x.SfcStepId == item.Id);
                            if (manusfcProductBadRecords != null && manusfcProductBadRecords.Any(x => x.DisposalResult == ProductBadDisposalResultEnum.WaitingJudge))
                            {
                                lastmanuSfcSummary.IsJudgment = true;
                            }
                            break;
                        case ManuSfcStepTypeEnum.FutureLock:
                            break;
                        case ManuSfcStepTypeEnum.InstantLock:
                            break;
                        case ManuSfcStepTypeEnum.Unlock:
                            break;
                        case ManuSfcStepTypeEnum.BadEntry:
                            // 录入不良
                            var manuProductBadRecords = manuProductBadRecordList.Where(x => x.SfcStepId == item.Id);
                            if (manuProductBadRecords.Any(x => x.DisposalResult == ProductBadDisposalResultEnum.Repair || x.DisposalResult == ProductBadDisposalResultEnum.Scrap))
                            {
                                var manuProductBadRecord = manuProductBadRecords.FirstOrDefault(x => x.DisposalResult == ProductBadDisposalResultEnum.Repair ||
                                                                                                     x.DisposalResult == ProductBadDisposalResultEnum.Scrap);

                                if (manuProductBadRecord == null) continue;

                                if (item.ProcedureId == manuProductBadRecord.FoundBadOperationId && item.CurrentStatus == SfcStatusEnum.Activity)
                                {
                                    var badEntrylastManuSfcSummary = GetLastManuSfcSummary(ref manuSfcSummaryList, manuSfcSummaryProcedureLastList, groupItem.Key, item.ProcedureId ?? 0);

                                    // 步骤控制处理的暂不处理
                                    if (badEntrylastManuSfcSummary == null) continue;

                                    badEntrylastManuSfcSummary.UpdatedBy = userId;
                                    badEntrylastManuSfcSummary.UpdatedOn = HymsonClock.Now();
                                    badEntrylastManuSfcSummary.OutputQty = item.Qty;
                                    badEntrylastManuSfcSummary.UnqualifiedQty = item.Qty;
                                    badEntrylastManuSfcSummary.EndOn = item.CreatedOn;
                                }
                                else
                                {
                                    // 条码状态为活动中，记录不良存在报废或者返修，即判定当前条码是不良的，当时报废工序和当前工序不一样， 即需要更新当前产出
                                    if (item.ProcedureId != manuProductBadRecord.FoundBadOperationId && item.CurrentStatus == SfcStatusEnum.Activity)
                                    {
                                        var badEntrylastManuSfcSummary = GetLastManuSfcSummary(ref manuSfcSummaryList, manuSfcSummaryProcedureLastList, groupItem.Key, item.ProcedureId ?? 0);

                                        // 步骤控制处理的暂不处理
                                        if (badEntrylastManuSfcSummary == null) continue;

                                        badEntrylastManuSfcSummary.UpdatedBy = userId;
                                        badEntrylastManuSfcSummary.UpdatedOn = HymsonClock.Now();
                                        badEntrylastManuSfcSummary.OutputQty = item.Qty;
                                        badEntrylastManuSfcSummary.EndOn = item.CreatedOn;
                                    }

                                    var badEntryLastmanuSfcSummary = GetLastManuSfcSummary(ref manuSfcSummaryList, manuSfcSummaryProcedureLastList, groupItem.Key, manuProductBadRecord.FoundBadOperationId);

                                    // 步骤控制处理的暂不处理
                                    if (badEntryLastmanuSfcSummary == null) continue;

                                    badEntryLastmanuSfcSummary.UpdatedBy = userId;
                                    badEntryLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                    badEntryLastmanuSfcSummary.UnqualifiedQty = item.Qty;
                                    badEntryLastmanuSfcSummary.LastUpdatedOn = item.CreatedOn;
                                }
                            }
                            break;
                        case ManuSfcStepTypeEnum.Discard:
                            var manuSfcScrap = manuSfcScrapList.FirstOrDefault(x => x.SfcStepId == item.Id && x.SFC == item.SFC);
                            if (manuSfcScrap == null) continue;

                            if (item.ProcedureId == manuSfcScrap.ProcedureId && item.CurrentStatus == SfcStatusEnum.Activity)
                            {
                                var discardLastmanuSfcSummary = GetLastManuSfcSummary(ref manuSfcSummaryList, manuSfcSummaryProcedureLastList, groupItem.Key, item.ProcedureId ?? 0);

                                // 步骤控制处理的暂不处理
                                if (discardLastmanuSfcSummary == null) continue;

                                discardLastmanuSfcSummary.UpdatedBy = userId;
                                discardLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                discardLastmanuSfcSummary.OutputQty = item.Qty;
                                discardLastmanuSfcSummary.UnqualifiedQty = item.Qty;
                            }
                            else
                            {
                                // 条码状态为活动中，记录不良存在报废或者返修，即判定当前条码是不良的，当时报废工序和当前工序不一样， 即需要更新当前产出
                                if (item.ProcedureId != manuSfcScrap.ProcedureId && item.CurrentStatus == SfcStatusEnum.Activity)
                                {
                                    var discardbeforeLastmanuSfcSummary = GetLastManuSfcSummary(ref manuSfcSummaryList, manuSfcSummaryProcedureLastList, groupItem.Key, item.ProcedureId ?? 0);

                                    // 步骤控制处理的暂不处理
                                    if (discardbeforeLastmanuSfcSummary == null) continue;

                                    discardbeforeLastmanuSfcSummary.UpdatedBy = userId;
                                    discardbeforeLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                    discardbeforeLastmanuSfcSummary.OutputQty = item.Qty;
                                    discardbeforeLastmanuSfcSummary.EndOn = item.CreatedOn;
                                }

                                var discardLastmanuSfcSummary = GetLastManuSfcSummary(ref manuSfcSummaryList, manuSfcSummaryProcedureLastList, groupItem.Key, manuSfcScrap.ProcedureId ?? 0);
                                if (discardLastmanuSfcSummary == null) continue;

                                discardLastmanuSfcSummary.LastUpdatedOn = item.CreatedOn;
                                discardLastmanuSfcSummary.UpdatedBy = userId;
                                discardLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                discardLastmanuSfcSummary.UnqualifiedQty = item.Qty;
                            }
                            break;
                        case ManuSfcStepTypeEnum.Stop:
                            var stopLastmanuSfcSummary = GetLastManuSfcSummary(ref manuSfcSummaryList, manuSfcSummaryProcedureLastList, groupItem.Key, item.ProcedureId ?? 0);
                            if (stopLastmanuSfcSummary == null) continue;

                            stopLastmanuSfcSummary.InvestQty = 0;
                            stopLastmanuSfcSummary.UnqualifiedQty = 0;
                            stopLastmanuSfcSummary.OutputQty = 0;
                            stopLastmanuSfcSummary.EndOn = item.CreatedOn;
                            stopLastmanuSfcSummary.UpdatedBy = userId;
                            stopLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                            break;
                        case ManuSfcStepTypeEnum.BadRejudgment:
                            // 复判暂定一个工序的异常
                            var reJudgmentSfcStepIdsProductBadRecord = reJudgmentSfcStepIdsProductBadRecordList.FirstOrDefault(x => x.ReJudgmentSfcStepId == item.Id &&
                            (x.ReJudgmentResult == ProductBadDisposalResultEnum.Repair || x.ReJudgmentResult == ProductBadDisposalResultEnum.Scrap));
                            if (reJudgmentSfcStepIdsProductBadRecord != null)
                            {
                                var badRejudgmentLastmanuSfcSummary = GetLastManuSfcSummary(ref manuSfcSummaryList, manuSfcSummaryProcedureLastList, groupItem.Key, reJudgmentSfcStepIdsProductBadRecord.FoundBadOperationId);
                                if (badRejudgmentLastmanuSfcSummary == null) continue;

                                badRejudgmentLastmanuSfcSummary.UnqualifiedQty = item.Qty;
                                badRejudgmentLastmanuSfcSummary.JudgmentOn = item.CreatedOn;
                                badRejudgmentLastmanuSfcSummary.UpdatedBy = userId;
                                badRejudgmentLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                badRejudgmentLastmanuSfcSummary.LastUpdatedOn = item.CreatedOn;
                            }
                            else
                            {
                                reJudgmentSfcStepIdsProductBadRecord = reJudgmentSfcStepIdsProductBadRecordList.FirstOrDefault();
                                if (reJudgmentSfcStepIdsProductBadRecord == null) continue;

                                var badRejudgmentLastmanuSfcSummary = GetLastManuSfcSummary(ref manuSfcSummaryList, manuSfcSummaryProcedureLastList, groupItem.Key, reJudgmentSfcStepIdsProductBadRecord.FoundBadOperationId);
                                if (badRejudgmentLastmanuSfcSummary == null) continue;

                                badRejudgmentLastmanuSfcSummary.JudgmentOn = item.CreatedOn;
                                badRejudgmentLastmanuSfcSummary.UpdatedBy = userId;
                                badRejudgmentLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                badRejudgmentLastmanuSfcSummary.LastUpdatedOn = item.CreatedOn;
                            }
                            break;
                        case ManuSfcStepTypeEnum.Change:
                            var changeLastmanuSfcSummary = GetLastManuSfcSummary(ref manuSfcSummaryList, manuSfcSummaryProcedureLastList, groupItem.Key, item.ProcedureId ?? 0);
                            if (changeLastmanuSfcSummary == null) continue;

                            changeLastmanuSfcSummary.OutputQty = 0;
                            changeLastmanuSfcSummary.EndOn = item.CreatedOn;
                            changeLastmanuSfcSummary.UpdatedBy = userId;
                            changeLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                            break;
                        case ManuSfcStepTypeEnum.CancelDiscard:
                            var cancelManuSfcScrap = cancelManuSfcScrapList.FirstOrDefault(x => x.CancelSfcStepId == item.Id);
                            if (cancelManuSfcScrap == null || !cancelManuSfcScrap.ProcedureId.HasValue) continue;

                            var cancelDiscardLastmanuSfcSummary = GetLastManuSfcSummary(ref manuSfcSummaryList, manuSfcSummaryProcedureLastList, groupItem.Key, cancelManuSfcScrap.ProcedureId ?? 0);
                            if (cancelDiscardLastmanuSfcSummary == null) continue;

                            cancelDiscardLastmanuSfcSummary.UnqualifiedQty = 0;
                            cancelDiscardLastmanuSfcSummary.UpdatedBy = userId;
                            cancelDiscardLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();

                            var stepEntity = await _manuSfcStepRepository.GetByIdAsync(cancelManuSfcScrap.SfcStepId);
                            if (stepEntity.CurrentStatus == SfcStatusEnum.Activity)
                            {
                                cancelDiscardLastmanuSfcSummary.EndOn = null;
                                cancelDiscardLastmanuSfcSummary.OutputQty = 0;
                            }
                            break;
                        case ManuSfcStepTypeEnum.CloseIdentification:
                            break;
                        case ManuSfcStepTypeEnum.CloseDefect:
                            break;
                        case ManuSfcStepTypeEnum.Delete:
                            break;
                        case ManuSfcStepTypeEnum.Repair:
                            break;
                        case ManuSfcStepTypeEnum.StepControl:
                            break;
                        case ManuSfcStepTypeEnum.ManuUpdate:
                            break;
                        case ManuSfcStepTypeEnum.RepairReturn:
                            break;
                        case ManuSfcStepTypeEnum.Disassembly:
                            break;
                        case ManuSfcStepTypeEnum.Add:
                            break;
                        case ManuSfcStepTypeEnum.Assemble:
                            break;
                        case ManuSfcStepTypeEnum.Replace:
                            break;
                        case ManuSfcStepTypeEnum.Package:
                            break;
                        case ManuSfcStepTypeEnum.Unpack:
                            break;
                        case ManuSfcStepTypeEnum.EnterDowngrading:
                            break;
                        case ManuSfcStepTypeEnum.RemoveDowngrading:
                            break;
                    }
                }
            }

            using var trans = TransactionHelper.GetTransactionScope();
            await _manuSfcSummaryRepository.MergeRangeAsync(manuSfcSummaryList);
            await _waterMarkService.RecordWaterMarkAsync(BusinessKey.ManuSfcSummary, manuSfcStepList.Max(x => x.Id));
            trans.Complete();
        }

        /// <summary>
        /// 获取统计最新数据
        /// </summary>
        /// <param name="manuSfcStepList"></param>
        /// <param name="changeManuSfcStepList"></param>
        /// <param name="sfc"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        private ManuSfcSummaryEntity? GetLastManuSfcSummary(IEnumerable<ManuSfcSummaryEntity>? manuSfcStepList, List<ManuSfcSummaryEntity> changeManuSfcStepList, string sfc, long procedureId)
        {
            var lastmanuSfcSummary = manuSfcStepList?.OrderByDescending(x => x.StartOn).FirstOrDefault(x => x.SFC == sfc && x.ProcedureId == procedureId);
            if (lastmanuSfcSummary == null)
            {
                lastmanuSfcSummary = changeManuSfcStepList?.OrderByDescending(x => x.StartOn).FirstOrDefault(x => x.SFC == sfc && x.ProcedureId == procedureId);
            }
            else
            {
                lastmanuSfcSummary = new ManuSfcSummaryEntity
                {
                    Id = lastmanuSfcSummary.Id,
                    SiteId = lastmanuSfcSummary.SiteId,
                    SFC = lastmanuSfcSummary.SFC,
                    WorkOrderId = lastmanuSfcSummary.WorkOrderId,
                    ProductId = lastmanuSfcSummary.ProductId,
                    ProcedureId = lastmanuSfcSummary.ProcedureId,
                    StartOn = lastmanuSfcSummary.StartOn,
                    EndOn = lastmanuSfcSummary.EndOn,
                    InvestQty = lastmanuSfcSummary.InvestQty,
                    OutputQty = lastmanuSfcSummary.OutputQty,
                    UnqualifiedQty = lastmanuSfcSummary.UnqualifiedQty,
                    RepeatedCount = lastmanuSfcSummary.RepeatedCount,
                    IsJudgment = lastmanuSfcSummary.IsJudgment,
                    JudgmentOn = lastmanuSfcSummary.JudgmentOn,
                    Remark = lastmanuSfcSummary.Remark,
                    LastUpdatedOn = lastmanuSfcSummary.LastUpdatedOn,
                    CreatedBy = lastmanuSfcSummary.CreatedBy,
                    CreatedOn = lastmanuSfcSummary.CreatedOn,
                    UpdatedBy = lastmanuSfcSummary.UpdatedBy,
                    UpdatedOn = lastmanuSfcSummary.UpdatedOn,
                    IsDeleted = lastmanuSfcSummary.IsDeleted
                };
                changeManuSfcStepList.Add(lastmanuSfcSummary);
            }
            return lastmanuSfcSummary;
        }

        /// <summary>
        /// 获取统计最新数据
        /// </summary>
        /// <param name="changeManuSfcStepList"></param>
        /// <param name="manuSfcStepList"></param>
        /// <param name="sfc"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        private static ManuSfcSummaryEntity? GetLastManuSfcSummary(ref List<ManuSfcSummaryEntity> changeManuSfcStepList, IEnumerable<ManuSfcSummaryEntity>? manuSfcStepList, string sfc, long procedureId)
        {
            var lastmanuSfcSummary = manuSfcStepList?.OrderByDescending(x => x.StartOn).FirstOrDefault(x => x.SFC == sfc && x.ProcedureId == procedureId);
            if (lastmanuSfcSummary == null)
            {
                lastmanuSfcSummary = changeManuSfcStepList?.OrderByDescending(x => x.StartOn).FirstOrDefault(x => x.SFC == sfc && x.ProcedureId == procedureId);
            }
            else
            {
                lastmanuSfcSummary = new ManuSfcSummaryEntity
                {
                    Id = lastmanuSfcSummary.Id,
                    SiteId = lastmanuSfcSummary.SiteId,
                    SFC = lastmanuSfcSummary.SFC,
                    WorkOrderId = lastmanuSfcSummary.WorkOrderId,
                    ProductId = lastmanuSfcSummary.ProductId,
                    ProcedureId = lastmanuSfcSummary.ProcedureId,
                    StartOn = lastmanuSfcSummary.StartOn,
                    EndOn = lastmanuSfcSummary.EndOn,
                    InvestQty = lastmanuSfcSummary.InvestQty,
                    OutputQty = lastmanuSfcSummary.OutputQty,
                    UnqualifiedQty = lastmanuSfcSummary.UnqualifiedQty,
                    RepeatedCount = lastmanuSfcSummary.RepeatedCount,
                    IsJudgment = lastmanuSfcSummary.IsJudgment,
                    JudgmentOn = lastmanuSfcSummary.JudgmentOn,
                    Remark = lastmanuSfcSummary.Remark,
                    LastUpdatedOn = lastmanuSfcSummary.LastUpdatedOn,
                    CreatedBy = lastmanuSfcSummary.CreatedBy,
                    CreatedOn = lastmanuSfcSummary.CreatedOn,
                    UpdatedBy = lastmanuSfcSummary.UpdatedBy,
                    UpdatedOn = lastmanuSfcSummary.UpdatedOn,
                    IsDeleted = lastmanuSfcSummary.IsDeleted
                };
                changeManuSfcStepList.Add(lastmanuSfcSummary);
            }
            return lastmanuSfcSummary;
        }

    }
}
