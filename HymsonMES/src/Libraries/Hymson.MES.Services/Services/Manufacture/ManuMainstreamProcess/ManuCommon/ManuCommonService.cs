using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Process;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCommonDto;
using Hymson.Sequences;
using Hymson.Snowflake;
using Hymson.Utils;
using Microsoft.Extensions.Caching.Memory;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon
{
    /// <summary>
    /// 生产通用
    /// </summary>
    public class ManuCommonService : IManuCommonService
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
        /// 缓存
        /// </summary>
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 序列号服务
        /// </summary>
        private readonly ISequenceService _sequenceService;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（工单信息）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（工单激活信息）
        /// </summary>
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;

        /// <summary>
        /// 仓储接口（工艺路线工序节点）
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;

        /// <summary>
        /// 仓储接口（工艺路线工序连线）
        /// </summary>
        private readonly IProcProcessRouteDetailLinkRepository _procProcessRouteDetailLinkRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="memoryCache"></param>
        /// <param name="sequenceService"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="planWorkOrderActivationRepository"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        /// <param name="procProcedureRepository"></param>
        public ManuCommonService(ICurrentUser currentUser, ICurrentSite currentSite,
            IMemoryCache memoryCache, ISequenceService sequenceService,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IPlanWorkOrderActivationRepository planWorkOrderActivationRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository,
            IProcProcedureRepository procProcedureRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _memoryCache = memoryCache;
            _sequenceService = sequenceService;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
            _procProcedureRepository = procProcedureRepository;
        }


        /// <summary>
        /// 获取生产条码信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<ManuSfcProduceEntity> GetProduceSFCAsync(string sfc)
        {
            if (string.IsNullOrWhiteSpace(sfc) == true
                || sfc.Contains(' ') == true) throw new CustomerValidationException(nameof(ErrorCode.MES16305));

            var sfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(sfc);
            if (sfcProduceEntity == null) throw new CustomerValidationException(nameof(ErrorCode.MES16306));

            return sfcProduceEntity;
        }

        /// <summary>
        /// 获取生产工单
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderEntity> GetProduceWorkOrderByIdAsync(long workOrderId)
        {
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(workOrderId);
            if (planWorkOrderEntity == null) throw new CustomerValidationException(nameof(ErrorCode.MES16301));

            // 判断是否被锁定
            if (planWorkOrderEntity.IsLocked == YesOrNoEnum.Yes)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16302)).WithData("ordercode", planWorkOrderEntity.OrderCode);
            }

            // 判断是否是激活的工单
            var activatedWorkOrder = await _planWorkOrderActivationRepository.GetByIdAsync(planWorkOrderEntity.Id);
            if (activatedWorkOrder == null) throw new CustomerValidationException(nameof(ErrorCode.MES16410));

            switch (planWorkOrderEntity.Status)
            {
                case PlanWorkOrderStatusEnum.SendDown:
                case PlanWorkOrderStatusEnum.InProduction:
                case PlanWorkOrderStatusEnum.Finish:
                    break;
                case PlanWorkOrderStatusEnum.NotStarted:
                case PlanWorkOrderStatusEnum.Closed:
                default:
                    throw new CustomerValidationException(nameof(ErrorCode.MES16303)).WithData("ordercode", planWorkOrderEntity.OrderCode);
            }

            return planWorkOrderEntity;
        }

        /// <summary>
        /// 获取首工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        public async Task<ProcessRouteProcedureDto> GetFirstProcedureAsync(long processRouteId)
        {
            var procProcessRouteDetailNodeEntity = await _procProcessRouteDetailNodeRepository.GetFirstProcedureByProcessRouteIdAsync(processRouteId);
            if (procProcessRouteDetailNodeEntity == null) throw new CustomerValidationException(nameof(ErrorCode.MES16304));

            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(procProcessRouteDetailNodeEntity.ProcedureId);
            if (procProcedureEntity == null) throw new CustomerValidationException(nameof(ErrorCode.MES10406));

            return new ProcessRouteProcedureDto
            {
                ProcessRouteId = processRouteId,
                SerialNo = procProcessRouteDetailNodeEntity.SerialNo,
                ProcedureId = procProcessRouteDetailNodeEntity.ProcedureId,
                CheckType = procProcessRouteDetailNodeEntity.CheckType,
                CheckRate = procProcessRouteDetailNodeEntity.CheckRate,
                IsWorkReport = procProcessRouteDetailNodeEntity.IsWorkReport,
                ProcedureCode = procProcedureEntity.Code,
                ProcedureName = procProcedureEntity.Name,
                Type = procProcedureEntity.Type,
                PackingLevel = procProcedureEntity.PackingLevel,
                ResourceTypeId = procProcedureEntity.ResourceTypeId,
                Cycle = procProcedureEntity.Cycle,
                IsRepairReturn = procProcedureEntity.IsRepairReturn
            };
        }

        /// <summary>
        /// 获当前工序对应的下一工序
        /// </summary>
        /// <param name="manuSfcProduce"></param>
        /// <returns></returns>
        public async Task<ProcProcedureEntity?> GetNextProcedureAsync(ManuSfcProduceEntity manuSfcProduce)
        {
            // 因为可能有分叉，所以返回的下一步工序是集合
            var netxtProcessRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetNextProcessRouteDetailLinkAsync(new ProcProcessRouteDetailLinkQuery
            {
                ProcessRouteId = manuSfcProduce.ProcessRouteId,
                ProcedureId = manuSfcProduce.ProcedureId
            });
            if (netxtProcessRouteDetailLinks == null || netxtProcessRouteDetailLinks.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES10440));

            // 获取当前工序在工艺路线里面的扩展信息
            var procedureNodes = await _procProcessRouteDetailNodeRepository.GetByIdsAsync(netxtProcessRouteDetailLinks.Select(s => s.ProcessRouteDetailId).ToArray())
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10440));

            // 检查是否有"空值"类型的工序
            var defaultNextProcedure = procedureNodes.FirstOrDefault(f => f.CheckType == ProcessRouteInspectTypeEnum.None)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10441));

            // 有多工序分叉的情况
            if (procedureNodes.Count() > 1)
            {
                // 这个Key太长了
                //var cacheKey = $"{manuSfcProduce.ProcessRouteId}-{manuSfcProduce.ProcedureId}-{manuSfcProduce.ResourceId}-{manuSfcProduce.WorkOrderId}";

                var cacheKey = $"{manuSfcProduce.ProcedureId}-{manuSfcProduce.WorkOrderId}";
                var count = await _sequenceService.GetSerialNumberAsync(Sequences.Enums.SerialNumberTypeEnum.None, cacheKey);

                // 读取工序抽检次数
                if (count > 0 && count % defaultNextProcedure.CheckRate == 0)
                {
                    // 如果满足抽检次数，就取出一个非"空值"的随机工序作为下一工序
                    defaultNextProcedure = procedureNodes.FirstOrDefault(f => f.CheckType != ProcessRouteInspectTypeEnum.None);
                }
            }

            // 获取下一工序
            if (defaultNextProcedure == null) throw new CustomerValidationException(nameof(ErrorCode.MES10440));
            return await _procProcedureRepository.GetByIdAsync(defaultNextProcedure.ProcedureId);
        }

        /// <summary>
        /// 判断上一工序是否随机工序
        /// </summary>
        /// <param name="manuSfcProduce"></param>
        /// <returns></returns>
        public async Task<bool> IsRandomPreProcedure(ManuSfcProduceEntity manuSfcProduce)
        {
            // 因为可能有分叉，所以返回的上一步工序是集合
            var preProcessRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetPreProcessRouteDetailLinkAsync(new ProcProcessRouteDetailLinkQuery
            {
                ProcessRouteId = manuSfcProduce.ProcessRouteId,
                ProcedureId = manuSfcProduce.ProcedureId
            });
            if (preProcessRouteDetailLinks == null || preProcessRouteDetailLinks.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES10442));

            // 获取当前工序在工艺路线里面的扩展信息
            var procedureNodes = await _procProcessRouteDetailNodeRepository
                .GetByIdsAsync(preProcessRouteDetailLinks
                .Where(w => w.PreProcessRouteDetailId.HasValue)
                .Select(s => s.PreProcessRouteDetailId.Value).ToArray())
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10442));

            // 有多工序分叉的情况（取第一个当默认值）
            ProcProcessRouteDetailNodeEntity defaultPreProcedure = procedureNodes.FirstOrDefault();
            if (preProcessRouteDetailLinks.Count() > 1)
            {
                // 下工序找上工序，执照正常流程的工序
                defaultPreProcedure = procedureNodes.FirstOrDefault(f => f.CheckType == ProcessRouteInspectTypeEnum.None)
                   ?? throw new CustomerValidationException(nameof(ErrorCode.MES10441));
            }

            // 获取上一工序
            if (defaultPreProcedure == null) throw new CustomerValidationException(nameof(ErrorCode.MES10442));
            return defaultPreProcedure.CheckType == ProcessRouteInspectTypeEnum.RandomInspection;
        }

        /// <summary>
        /// 验证开始工序是否在结束工序之前
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="startProcedureId"></param>
        /// <param name="endProcedureId"></param>
        /// <returns></returns>
        public async Task<bool> IsProcessStartBeforeEnd(long processRouteId, long startProcedureId, long endProcedureId)
        {
            var processRouteDetailList = await GetProcessRoute(processRouteId);
            var processRouteDetails = processRouteDetailList.Where(x => x.ProcedureIds.Contains(startProcedureId) && x.ProcedureIds.Contains(endProcedureId));
            if (processRouteDetails != null && processRouteDetails.Any())
            {
                foreach (var processRouteDetail in processRouteDetails)
                {
                    var startIndex = processRouteDetail.ProcedureIds.ToList().IndexOf(startProcedureId);
                    var endIndex = processRouteDetail.ProcedureIds.ToList().IndexOf(startProcedureId);
                    if (startIndex < endIndex)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 获取工艺路线
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcessRouteDetailDto>> GetProcessRoute(long processRouteId)
        {
            var processRouteDetailLinkListTask = _procProcessRouteDetailLinkRepository.GetListAsync(new ProcProcessRouteDetailLinkQuery { ProcessRouteId = processRouteId });
            var processRouteDetailNodeListTask = _procProcessRouteDetailNodeRepository.GetProcProcessRouteDetailNodesByProcessRouteId(processRouteId);
            var processRouteDetailLinkList = await processRouteDetailLinkListTask;
            var processRouteDetailNodeList = await processRouteDetailNodeListTask;

            IList<ProcessRouteDetailDto> list = new List<ProcessRouteDetailDto>();
            if (processRouteDetailLinkList != null && processRouteDetailLinkList.Any()
                && processRouteDetailNodeList != null && processRouteDetailNodeList.Any())
            {
                var firstProcedure = processRouteDetailNodeList.FirstOrDefault(x => x.IsFirstProcess == 1);
                if (firstProcedure != null)
                {
                    CombinationProcessRoute(ref list, firstProcedure.ProcedureId, processRouteDetailLinkList);
                }
            }
            return list;
        }

        /// <summary>
        /// 组装工艺路线
        /// </summary>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <param name="procedureId"></param>
        /// <param name="procProcessRouteDetailLinkEntities"></param>
        private void CombinationProcessRoute(ref IList<ProcessRouteDetailDto> list, long procedureId, IEnumerable<ProcProcessRouteDetailLinkEntity> procProcessRouteDetailLinkEntities, long key = 0)
        {
            if (list == null || !list.Any())
            {
                key = IdGenProvider.Instance.CreateId();
                list = new List<ProcessRouteDetailDto>
                {
                    new ProcessRouteDetailDto
                    {
                        key = key,
                        ProcedureIds = new List<long> { procedureId }
                    }
                };
            }

            var procProcessRouteDetailLinkByprocedureIdList = procProcessRouteDetailLinkEntities.Where(x => x.PreProcessRouteDetailId == procedureId);
            if (procProcessRouteDetailLinkByprocedureIdList != null && procProcessRouteDetailLinkByprocedureIdList.Any())
            {
                var processRouteDetail = list.FirstOrDefault(x => x.key == key);
                if (processRouteDetail != null)
                {
                    var procedureIds = processRouteDetail.ProcedureIds.ToList();
                    int index = 1;
                    foreach (var item in procProcessRouteDetailLinkByprocedureIdList)
                    {
                        if (item.ProcessRouteDetailId != ProcessRoute.LastProcedureId)
                        {
                            if (index == 1)
                            {
                                processRouteDetail.ProcedureIds.Add(item.ProcessRouteDetailId);
                                CombinationProcessRoute(ref list, item.ProcessRouteDetailId, procProcessRouteDetailLinkEntities, key);
                            }
                            else
                            {
                                var processRouteDetailDto = new ProcessRouteDetailDto()
                                {
                                    key = IdGenProvider.Instance.CreateId(),
                                    ProcedureIds = procedureIds,
                                };
                                processRouteDetailDto.ProcedureIds.Add(item.ProcessRouteDetailId);
                                list.Add(processRouteDetailDto);
                                CombinationProcessRoute(ref list, item.ProcessRouteDetailId, procProcessRouteDetailLinkEntities, processRouteDetailDto.key);
                            }
                        }
                        index++;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ManuSfcProduceExtensions
    {
        /// <summary>
        /// 条码合法性校验
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <param name="produceStatus"></param>
        public static ManuSfcProduceEntity VerifySFCStatus(this ManuSfcProduceEntity sfcProduceEntity, SfcProduceStatusEnum produceStatus)
        {
            // 当前工序是否是指定状态
            if (sfcProduceEntity.Status != produceStatus) throw new CustomerValidationException(nameof(ErrorCode.MES16313)).WithData("Status", produceStatus.GetDescription());

            return sfcProduceEntity;
        }

        /// <summary>
        /// 工序活动状态校验
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public static ManuSfcProduceEntity VerifyProcedure(this ManuSfcProduceEntity sfcProduceEntity, long procedureId)
        {
            // 产品编码是否和工序对应
            if (sfcProduceEntity.ProcedureId != procedureId) throw new CustomerValidationException(nameof(ErrorCode.MES16308));

            // 是否被锁定
            if (sfcProduceEntity.Lock.HasValue == true)
            {
                if (sfcProduceEntity.Lock == QualityLockEnum.InstantLock)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16314)).WithData("SFC", sfcProduceEntity.SFC);
                }

                if (sfcProduceEntity.Lock == QualityLockEnum.FutureLock && sfcProduceEntity.ProcedureId == procedureId)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16314)).WithData("SFC", sfcProduceEntity.SFC);
                }
            }

            return sfcProduceEntity;
        }

    }
}
