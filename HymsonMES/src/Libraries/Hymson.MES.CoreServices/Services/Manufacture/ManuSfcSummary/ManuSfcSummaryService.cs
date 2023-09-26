using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcStep.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcSummary.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcSummary.Query;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.WaterMark;
using IdGen;
using Microsoft.VisualBasic;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuSfcSummary
{
    public class ManuSfcSummaryService : IManuSfcSummaryService
    {
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
        /// 生产统计表
        /// </summary>
        /// <param name="waterMarkRepository"></param>
        public ManuSfcSummaryService(IWaterMarkService waterMarkService,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcSummaryRepository manuSfcSummaryRepository,
            IManuProductBadRecordRepository manuProductBadRecordRepository)
        {
            _waterMarkService = waterMarkService;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcSummaryRepository = manuSfcSummaryRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
        }

        /// <summary>
        /// 执行生产统计
        /// </summary>
        /// <returns></returns>
        public async Task ExecutStatisticAsync(string userId, long siteId)
        {
            var startwaterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.ManuSfcSummaryBusinessKey);

            //获取步骤表数据
            var manuSfcStepList = await _manuSfcStepRepository.GeListtByStartwaterMarkIdAsync(new ManuSfcStepStatisticQuery
            {
                SiteId = siteId,
                StartwaterMarkId = startwaterMarkId,
                Rows = 500
            });

            if (manuSfcStepList != null && manuSfcStepList.Any())
            {
                //条码工序统计最后一条数据
                var manuSfcSummaryProcedureLastList = await _manuSfcSummaryRepository.GetyLastListByProcedureIdsAndSfcsAsync(new LastManuSfcSummaryByProcedureIdAndSfcsQuery
                {
                    Sfcs = manuSfcStepList.Select(x => x.SFC),
                    SiteId = siteId
                });

                //录入不良列表
                var manuProductBadRecordList = await _manuProductBadRecordRepository.GetBySfcStepIdsAsync(manuSfcStepList.Select(x => x.Id));
                //复判列表
                var reJudgmentSfcStepIdsProductBadRecordList = await _manuProductBadRecordRepository.GetByReJudgmentSfcStepIdsAsync(manuSfcStepList.Select(x => x.Id));
                //报废列表
                var manuSfcScrapList = await _manuSfcScrapRepository.GetByStepIdsAsync(manuSfcStepList.Select(x => x.Id));
                //获取取消报废的列表
                var cancelManuSfcScrapList = await _manuSfcScrapRepository.GetByCancelSfcStepIdsAsync(manuSfcStepList.Select(x => x.Id));

                List<ManuSfcSummaryEntity> addManuSfcSummaryList = new List<ManuSfcSummaryEntity>();
                List<ManuSfcSummaryEntity> updateManuSfcSummaryList = new List<ManuSfcSummaryEntity>();
                var groupManuSfcStepList = manuSfcStepList.GroupBy(x => x.SFC);
                foreach (var groupItem in groupManuSfcStepList)
                {
                    foreach (var item in groupItem)
                    {
                        var lastmanuSfcSummary = addManuSfcSummaryList.OrderBy(x => x.StartOn).FirstOrDefault(x => x.SFC == groupItem.Key && x.ProcedureId == item.ProcedureId);
                        if (lastmanuSfcSummary != null)
                        {
                            lastmanuSfcSummary = manuSfcSummaryProcedureLastList.FirstOrDefault(x => x.SFC == groupItem.Key && x.ProcedureId == item.ProcedureId);
                        }
                        switch (item.Operatetype)
                        {
                            case ManuSfcStepTypeEnum.Create:
                                break;
                            case ManuSfcStepTypeEnum.Receive:
                                break;
                            case ManuSfcStepTypeEnum.InStock:
                                var sfcSummary = new ManuSfcSummaryEntity
                                {
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
                                addManuSfcSummaryList.Add(sfcSummary);
                                break;
                            case ManuSfcStepTypeEnum.OutStock:
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
                                    updateManuSfcSummaryList.Add(lastmanuSfcSummary);
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
                                if (manuProductBadRecords.Any(x => x.DisposalResult == ProductBadDisposalResultEnum.repair || x.DisposalResult == ProductBadDisposalResultEnum.scrap))
                                {
                                    var manuProductBadRecord = manuProductBadRecords.FirstOrDefault(x => x.DisposalResult == ProductBadDisposalResultEnum.repair ||
                                                                                                         x.DisposalResult == ProductBadDisposalResultEnum.scrap);
                                    if (manuProductBadRecord != null)
                                    {
                                        if (item.ProcedureId == manuProductBadRecord.FoundBadOperationId && item.CurrentStatus == SfcStatusEnum.Activity)
                                        {
                                            if (lastmanuSfcSummary != null)
                                            {
                                                lastmanuSfcSummary.UpdatedBy = userId;
                                                lastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                                lastmanuSfcSummary.OutputQty = item.Qty;
                                                lastmanuSfcSummary.UnqualifiedQty = item.Qty;
                                                lastmanuSfcSummary.EndOn = item.CreatedOn;
                                                updateManuSfcSummaryList.Add(lastmanuSfcSummary);
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
                                                if (lastmanuSfcSummary != null)
                                                {
                                                    lastmanuSfcSummary.UpdatedBy = userId;
                                                    lastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                                    lastmanuSfcSummary.OutputQty = item.Qty;
                                                    lastmanuSfcSummary.EndOn = item.CreatedOn;
                                                    updateManuSfcSummaryList.Add(lastmanuSfcSummary);
                                                }
                                                else
                                                {
                                                    //步骤控制处理的暂不处理
                                                }
                                            }
                                            var badEntryLastmanuSfcSummary = addManuSfcSummaryList.OrderBy(x => x.StartOn).FirstOrDefault(x => x.SFC == groupItem.Key && x.ProcedureId == manuProductBadRecord.FoundBadOperationId);
                                            if (badEntryLastmanuSfcSummary != null)
                                            {
                                                badEntryLastmanuSfcSummary = manuSfcSummaryProcedureLastList.FirstOrDefault(x => x.SFC == groupItem.Key && x.ProcedureId == manuProductBadRecord.FoundBadOperationId);
                                            }
                                            if (badEntryLastmanuSfcSummary != null)
                                            {
                                                badEntryLastmanuSfcSummary.UpdatedBy = userId;
                                                badEntryLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                                badEntryLastmanuSfcSummary.UnqualifiedQty = item.Qty;
                                                updateManuSfcSummaryList.Add(badEntryLastmanuSfcSummary);
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
                                        if (lastmanuSfcSummary != null)
                                        {
                                            lastmanuSfcSummary.UpdatedBy = userId;
                                            lastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                            lastmanuSfcSummary.OutputQty = item.Qty;
                                            lastmanuSfcSummary.UnqualifiedQty = item.Qty;
                                            updateManuSfcSummaryList.Add(lastmanuSfcSummary);
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
                                            if (lastmanuSfcSummary != null)
                                            {
                                                lastmanuSfcSummary.UpdatedBy = userId;
                                                lastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                                lastmanuSfcSummary.OutputQty = item.Qty;
                                                lastmanuSfcSummary.EndOn = HymsonClock.Now();
                                                updateManuSfcSummaryList.Add(lastmanuSfcSummary);
                                            }
                                            else
                                            {
                                                //步骤控制处理的暂不处理
                                            }
                                        }
                                        var badEntryLastmanuSfcSummary = addManuSfcSummaryList.OrderBy(x => x.StartOn).FirstOrDefault(x => x.SFC == groupItem.Key && x.ProcedureId == manuSfcScrap.ProcedureId);
                                        if (badEntryLastmanuSfcSummary != null)
                                        {
                                            badEntryLastmanuSfcSummary = manuSfcSummaryProcedureLastList.FirstOrDefault(x => x.SFC == groupItem.Key && x.ProcedureId == manuSfcScrap.ProcedureId);
                                        }
                                        if (badEntryLastmanuSfcSummary != null)
                                        {
                                            badEntryLastmanuSfcSummary.UpdatedBy = userId;
                                            badEntryLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                            badEntryLastmanuSfcSummary.UnqualifiedQty = item.Qty;
                                            updateManuSfcSummaryList.Add(badEntryLastmanuSfcSummary);
                                        }
                                    }
                                }
                                break;
                            case ManuSfcStepTypeEnum.Stop:
                                if (lastmanuSfcSummary != null)
                                {
                                    lastmanuSfcSummary.InvestQty = 0;
                                    lastmanuSfcSummary.UnqualifiedQty = 0;
                                    lastmanuSfcSummary.OutputQty = 0;
                                    lastmanuSfcSummary.EndOn = item.CreatedOn;
                                    lastmanuSfcSummary.UpdatedBy = userId;
                                    lastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                    updateManuSfcSummaryList.Add(lastmanuSfcSummary);
                                }
                                break;
                            case ManuSfcStepTypeEnum.BadRejudgment:
                                //复判暂定一个工序的异常
                                var reJudgmentSfcStepIdsProductBadRecord = reJudgmentSfcStepIdsProductBadRecordList.FirstOrDefault(x => x.ReJudgmentSfcStepId == item.Id &&
                                (x.ReJudgmentResult == ProductBadDisposalResultEnum.repair || x.ReJudgmentResult == ProductBadDisposalResultEnum.scrap));
                                if (reJudgmentSfcStepIdsProductBadRecord != null)
                                {
                                    var badRejudgmentLastmanuSfcSummary = addManuSfcSummaryList.OrderBy(x => x.StartOn).FirstOrDefault(x => x.SFC == groupItem.Key && x.ProcedureId == reJudgmentSfcStepIdsProductBadRecord.FoundBadOperationId);
                                    if (badRejudgmentLastmanuSfcSummary != null)
                                    {
                                        badRejudgmentLastmanuSfcSummary = manuSfcSummaryProcedureLastList.FirstOrDefault(x => x.SFC == groupItem.Key && x.ProcedureId == reJudgmentSfcStepIdsProductBadRecord.FoundBadOperationId);
                                    }
                                    if (badRejudgmentLastmanuSfcSummary != null)
                                    {
                                        badRejudgmentLastmanuSfcSummary.UnqualifiedQty = item.Qty;
                                        badRejudgmentLastmanuSfcSummary.JudgmentOn = item.CreatedOn;
                                        badRejudgmentLastmanuSfcSummary.UpdatedBy = userId;
                                        badRejudgmentLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                        updateManuSfcSummaryList.Add(badRejudgmentLastmanuSfcSummary);
                                    }

                                }
                                else
                                {
                                    reJudgmentSfcStepIdsProductBadRecord = reJudgmentSfcStepIdsProductBadRecordList.FirstOrDefault();
                                    if (reJudgmentSfcStepIdsProductBadRecord != null)
                                    {
                                        var badRejudgmentLastmanuSfcSummary = addManuSfcSummaryList.OrderBy(x => x.StartOn).FirstOrDefault(x => x.SFC == groupItem.Key && x.ProcedureId == reJudgmentSfcStepIdsProductBadRecord.FoundBadOperationId);
                                        if (badRejudgmentLastmanuSfcSummary != null)
                                        {
                                            badRejudgmentLastmanuSfcSummary = manuSfcSummaryProcedureLastList.FirstOrDefault(x => x.SFC == groupItem.Key && x.ProcedureId == reJudgmentSfcStepIdsProductBadRecord.FoundBadOperationId);
                                        }
                                        if (badRejudgmentLastmanuSfcSummary != null)
                                        {
                                            badRejudgmentLastmanuSfcSummary.JudgmentOn = item.CreatedOn;
                                            badRejudgmentLastmanuSfcSummary.UpdatedBy = userId;
                                            badRejudgmentLastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                            updateManuSfcSummaryList.Add(badRejudgmentLastmanuSfcSummary);
                                        }
                                    }
                                }
                                break;
                            case ManuSfcStepTypeEnum.Change:
                                if (lastmanuSfcSummary != null)
                                {
                                    lastmanuSfcSummary.OutputQty = 0;
                                    lastmanuSfcSummary.EndOn = item.CreatedOn;
                                    lastmanuSfcSummary.UpdatedBy = userId;
                                    lastmanuSfcSummary.UpdatedOn = HymsonClock.Now();
                                    updateManuSfcSummaryList.Add(lastmanuSfcSummary);
                                }
                                break;
                            case ManuSfcStepTypeEnum.CancelDiscard:
                                var cancelManuSfcScrap = cancelManuSfcScrapList.FirstOrDefault(x => x.CancelSfcStepId == item.Id);
                                if (cancelManuSfcScrap != null)
                                {
                                    if (cancelManuSfcScrap.ProcedureId.HasValue)
                                    {
                                        var cancelDiscardLastmanuSfcSummary = addManuSfcSummaryList.OrderBy(x => x.StartOn).FirstOrDefault(x => x.SFC == groupItem.Key && x.ProcedureId == cancelManuSfcScrap.ProcedureId);
                                        if (cancelDiscardLastmanuSfcSummary != null)
                                        {
                                            cancelDiscardLastmanuSfcSummary = manuSfcSummaryProcedureLastList.FirstOrDefault(x => x.SFC == groupItem.Key && x.ProcedureId == cancelManuSfcScrap.ProcedureId);
                                        }
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
                                            updateManuSfcSummaryList.Add(cancelDiscardLastmanuSfcSummary);
                                        }
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
                await _manuSfcSummaryRepository.InsertRangeAsync(addManuSfcSummaryList);
                await _manuSfcSummaryRepository.UpdateRangeAsync(updateManuSfcSummaryList);
                await _waterMarkService.RecordWaterMarkAsync(BusinessKey.ManuSfcSummaryBusinessKey, manuSfcStepList.Max(x => x.Id));
                trans.Complete();
            }
        }
    }
}
