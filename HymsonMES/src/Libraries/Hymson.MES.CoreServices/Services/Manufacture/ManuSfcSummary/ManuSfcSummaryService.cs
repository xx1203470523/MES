using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcStep.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcSummary.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcSummary.Query;
using Hymson.Utils;
using Hymson.WaterMark;
using System.Collections.Generic;
using System.Security.Policy;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuSfcSummary
{
    public class ManuSfcSummaryService : IManuSfcSummaryService
    {
        public readonly IWaterMarkRepository _waterMarkRepository;
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
        public ManuSfcSummaryService(IWaterMarkRepository waterMarkRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcSummaryRepository manuSfcSummaryRepository,
            IManuProductBadRecordRepository manuProductBadRecordRepository)
        {
            _waterMarkRepository = waterMarkRepository;
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
            var waterMarkEntity = await _waterMarkRepository.GetByBusinessKeyAsync(BusinessKey.ManuSfcSummaryBusinessKey);
            long startwaterMarkId = 0;
            if (waterMarkEntity != null)
            {
                startwaterMarkId = waterMarkEntity.WaterLevel;
            }

            //获取步骤表数据
            var manuSfcStepList = await _manuSfcStepRepository.GeListtByStartwaterMarkIdAsync(new ManuSfcStepStatisticQuery
            {
                SiteId = siteId,
                StartwaterMarkId = startwaterMarkId,
                Rows = 1000
            });
            if (manuSfcStepList != null && manuSfcStepList.Any())
            {
                var manuSfcSummaryLastList = await _manuSfcSummaryRepository.GetyLastListBySfsAsync(new LastManuSfcSummaryBySfcsQuery
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

                List<ManuSfcSummaryEntity> manuSfcSummaryList = new List<ManuSfcSummaryEntity>();
                List<MultiUpdateSummaryOutStationCommand> multiUpdateSummaryOutStationCommands = new();
                List<MultiUpdateSummaryUnqualifiedCommand> multiUpdateSummaryUnqualifiedCommands = new();
                List<MultiUpdateSummaryReJudgmentUnqualifiedCommand> updateSummaryReJudgmentUnqualifiedCommands = new();
                List<MultiUpdateSummaryReJudgmentQualifiedCommand> updateSummaryReJudgmentQualifiedCommands = new();
                var groupManuSfcStepList = manuSfcStepList.GroupBy(x => x.SFC);
                foreach (var groupItem in groupManuSfcStepList)
                {
                    var lastSfcSummary = manuSfcSummaryLastList.FirstOrDefault(x => x.SFC == groupItem.Key);
                    foreach (var item in groupItem)
                    {
                        switch (item.Operatetype)
                        {
                            case ManuSfcStepTypeEnum.Create:
                                break;
                            case ManuSfcStepTypeEnum.Receive:
                                break;
                            case ManuSfcStepTypeEnum.InStock:
                                lastSfcSummary = new ManuSfcSummaryEntity
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
                                manuSfcSummaryList.Add(lastSfcSummary);
                                break;
                            case ManuSfcStepTypeEnum.OutStock:
                                if (lastSfcSummary != null)
                                {
                                    var multiUpdateSummaryOutStationCommand = new MultiUpdateSummaryOutStationCommand
                                    {
                                        Id = lastSfcSummary.Id,
                                        OutputQty = item.Qty,
                                        EndOn = item.CreatedOn,
                                        UpdatedBy = userId
                                    };
                                    var manusfcProductBadRecords = manuProductBadRecordList.Where(x => x.SfcStepId == item.Id);
                                    if (manusfcProductBadRecords != null && manusfcProductBadRecords.Any(x => x.DisposalResult == ProductBadDisposalResultEnum.WaitingJudge))
                                    {
                                        multiUpdateSummaryOutStationCommand.IsJudgment = true;
                                    }
                                    multiUpdateSummaryOutStationCommands.Add(multiUpdateSummaryOutStationCommand);
                                }
                                else
                                {
                                    //步骤控制处理的暂不处理
                                }
                                lastSfcSummary = null;
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
                                        long lastSfcSummaryId = 0;

                                        if (item.ProcedureId != manuProductBadRecord.FoundBadOperationId && item.CurrentStatus == SfcStatusEnum.Activity)
                                        {
                                            if (lastSfcSummary != null)
                                            {
                                                //完成当前工序
                                                multiUpdateSummaryOutStationCommands.Add(new MultiUpdateSummaryOutStationCommand
                                                {
                                                    Id = lastSfcSummary.Id,
                                                    OutputQty = item.Qty,
                                                    IsJudgment = true,
                                                    EndOn = item.CreatedOn,
                                                    UpdatedBy = userId
                                                });
                                            }
                                            else
                                            {
                                                //步骤控制处理的暂不处理
                                            }
                                        }

                                        if (item.ProcedureId == manuProductBadRecord.FoundBadOperationId && item.CurrentStatus == SfcStatusEnum.Activity)
                                        {
                                            if (lastSfcSummary != null)
                                            {
                                                lastSfcSummaryId = lastSfcSummary.Id;
                                            }
                                        }
                                        else
                                        {
                                            //找到报废工序记录数据
                                            var lastProcedureSfcSummary = await _manuSfcSummaryRepository.GetyLastListByProcedureIdsAndSfcsAsync(new LastManuSfcSummaryByProcedureIdAndSfcQuery
                                            {
                                                ProcedureId = manuProductBadRecord.FoundBadOperationId,
                                                Sfc = item.SFC,
                                                SiteId = item.SiteId
                                            });

                                            if (lastProcedureSfcSummary != null)
                                            {
                                                lastSfcSummaryId = lastProcedureSfcSummary.Id;
                                            }
                                        }

                                        multiUpdateSummaryUnqualifiedCommands.Add(new MultiUpdateSummaryUnqualifiedCommand
                                        {
                                            Id = lastSfcSummaryId,
                                            UnqualifiedQty = item.Qty,
                                            EndOn = item.CreatedOn,
                                            UpdatedBy = userId
                                        });

                                        lastSfcSummary = null;
                                    }
                                }
                                break;
                            case ManuSfcStepTypeEnum.Discard:
                                var manuSfcScrap = manuSfcScrapList.FirstOrDefault(x => x.SfcStepId == item.Id && x.SFC == item.SFC);
                                if (manuSfcScrap != null)
                                {
                                    long lastSfcSummaryId = 0;

                                    if (item.ProcedureId != manuSfcScrap.ProcedureId && item.CurrentStatus == SfcStatusEnum.Activity)
                                    {
                                        if (lastSfcSummary != null)
                                        {
                                            //完成当前工序
                                            multiUpdateSummaryOutStationCommands.Add(new MultiUpdateSummaryOutStationCommand
                                            {
                                                Id = lastSfcSummary.Id,
                                                OutputQty = item.Qty,
                                                IsJudgment = true,
                                                EndOn = item.CreatedOn,
                                                UpdatedBy = userId
                                            });
                                        }
                                        else
                                        {
                                            //步骤控制处理的暂不处理
                                        }
                                    }

                                    if (item.ProcedureId == manuSfcScrap.ProcedureId && item.CurrentStatus == SfcStatusEnum.Activity)
                                    {
                                        if (lastSfcSummary != null)
                                        {
                                            lastSfcSummaryId = lastSfcSummary.Id;
                                        }
                                    }
                                    else
                                    {
                                        //找到报废工序记录数据
                                        var lastProcedureSfcSummary = await _manuSfcSummaryRepository.GetyLastListByProcedureIdsAndSfcsAsync(new LastManuSfcSummaryByProcedureIdAndSfcQuery
                                        {
                                            ProcedureId = manuSfcScrap.ProcedureId ?? 0,
                                            Sfc = item.SFC,
                                            SiteId = item.SiteId
                                        });

                                        if (lastProcedureSfcSummary != null)
                                        {
                                            lastSfcSummaryId = lastProcedureSfcSummary.Id;
                                        }
                                    }

                                    multiUpdateSummaryUnqualifiedCommands.Add(new MultiUpdateSummaryUnqualifiedCommand
                                    {
                                        Id = lastSfcSummaryId,
                                        UnqualifiedQty = item.Qty,
                                        EndOn = item.CreatedOn,
                                        UpdatedBy = userId
                                    });
                                    lastSfcSummary = null;
                                }
                                break;
                            case ManuSfcStepTypeEnum.Stop:
                                //汇总异常处理
                                break;
                            case ManuSfcStepTypeEnum.BadRejudgment:
                                //复判暂定一个工序的异常
                                var reJudgmentSfcStepIdsProductBadRecord = reJudgmentSfcStepIdsProductBadRecordList.FirstOrDefault(x => x.ReJudgmentSfcStepId == item.Id &&
                                (x.ReJudgmentResult == ProductBadDisposalResultEnum.repair || x.ReJudgmentResult == ProductBadDisposalResultEnum.scrap));
                                if (reJudgmentSfcStepIdsProductBadRecord != null)
                                {
                                    //找到工序统计记录数据
                                    var lastProcedureSfcSummary = await _manuSfcSummaryRepository.GetyLastListByProcedureIdsAndSfcsAsync(new LastManuSfcSummaryByProcedureIdAndSfcQuery
                                    {
                                        ProcedureId = reJudgmentSfcStepIdsProductBadRecord.FoundBadOperationId,
                                        Sfc = item.SFC,
                                        SiteId = item.SiteId
                                    });
                                }
                                else
                                {

                                }
                                break;
                            case ManuSfcStepTypeEnum.Change:
                                if (lastSfcSummary != null)
                                {
                                    //这个怎么记录
                                    multiUpdateSummaryOutStationCommands.Add(new MultiUpdateSummaryOutStationCommand
                                    {
                                        Id = lastSfcSummary.Id,
                                        OutputQty = 0,
                                        EndOn = item.CreatedOn,
                                        UpdatedBy = userId
                                    });
                                }
                                break;
                            case ManuSfcStepTypeEnum.CancelDiscard:
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
            }
        }
    }
}
