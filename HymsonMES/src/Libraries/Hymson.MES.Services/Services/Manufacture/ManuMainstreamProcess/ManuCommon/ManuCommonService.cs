using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCommonDto;
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
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

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
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="planWorkOrderActivationRepository"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        /// <param name="procProcedureRepository"></param>
        public ManuCommonService(ICurrentUser currentUser, ICurrentSite currentSite,
            IMemoryCache memoryCache,
            IManuSfcInfoRepository manuSfcInfoRepository,
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
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
            _procProcedureRepository = procProcedureRepository;
        }


        /// <summary>
        /// 获取生产条码信息（附带条码合法性校验 + 工序活动状态校验）
        /// </summary>
        /// <param name="spc"></param>
        /// <param name="procedureId"></param>
        /// <param name="allowStatus"></param>
        /// <returns></returns>
        public async Task<ManuSfcProduceEntity> GetProduceSPCWithCheckAsync(string spc, long procedureId, SfcProduceStatusEnum[] allowStatus)
        {
            if (string.IsNullOrWhiteSpace(spc) == true
                || spc.Contains(' ') == true) throw new BusinessException(nameof(ErrorCode.MES16305));

            var sfcProduceEntity = await _manuSfcProduceRepository.GetBySPCAsync(spc);
            if (sfcProduceEntity == null) throw new BusinessException(nameof(ErrorCode.MES16306));

            // 当前工序是否是活动状态
            if (allowStatus.Length > 0 && allowStatus.Contains(sfcProduceEntity.Status) == false) throw new BusinessException(nameof(ErrorCode.MES16309));

            // 产品编码是否和工序对应
            if (sfcProduceEntity.ProcedureId != procedureId) throw new BusinessException(nameof(ErrorCode.MES16308));

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
            if (planWorkOrderEntity == null) throw new BusinessException(nameof(ErrorCode.MES16301));

            // 判断是否被锁定
            if (planWorkOrderEntity.IsLocked == YesOrNoEnum.Yes)
            {
                throw new BusinessException(nameof(ErrorCode.MES16302)).WithData("ordercode", planWorkOrderEntity.OrderCode);
            }

            // 判断是否是激活的工单
            var activatedWorkOrder = await _planWorkOrderActivationRepository.GetByIdAsync(planWorkOrderEntity.Id);
            if (activatedWorkOrder == null) throw new BusinessException(nameof(ErrorCode.MES16410));

            switch (planWorkOrderEntity.Status)
            {
                case PlanWorkOrderStatusEnum.SendDown:
                case PlanWorkOrderStatusEnum.InProduction:
                case PlanWorkOrderStatusEnum.Finish:
                    break;
                case PlanWorkOrderStatusEnum.NotStarted:
                case PlanWorkOrderStatusEnum.Closed:
                default:
                    throw new BusinessException(nameof(ErrorCode.MES16303)).WithData("ordercode", planWorkOrderEntity.OrderCode);
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
            if (procProcessRouteDetailNodeEntity == null) throw new BusinessException(nameof(ErrorCode.MES16304));

            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(procProcessRouteDetailNodeEntity.ProcedureId);
            if (procProcedureEntity == null) throw new BusinessException(nameof(ErrorCode.MES10406));

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
            var processRouteDetailLink = await _procProcessRouteDetailLinkRepository.GetProcessRouteDetailLinkAsync(new ProcProcessRouteDetailLinkQuery
            {
                ProcessRouteId = manuSfcProduce.ProcessRouteId,
                ProcedureId = manuSfcProduce.ProcedureId
            });
            if (processRouteDetailLink == null || processRouteDetailLink.Any() == false) throw new BusinessException(nameof(ErrorCode.MES10440));

            // 获取当前工序在工艺路线里面的扩展信息
            var procedureNodes = await _procProcessRouteDetailNodeRepository.GetByIdsAsync(processRouteDetailLink.Select(s => s.ProcessRouteDetailId).ToArray())
                ?? throw new BusinessException(nameof(ErrorCode.MES10440));

            // 检查是否有"空值"类型的工序
            var defaultNextProcedure = procedureNodes.FirstOrDefault(f => f.CheckType == ProcessRouteInspectTypeEnum.None)
                ?? throw new BusinessException(nameof(ErrorCode.MES10441));

            // 有多工序分叉的情况
            if (procedureNodes.Count() > 1)
            {
                var cacheKey = $"{manuSfcProduce.ProcessRouteId}-{manuSfcProduce.ProcedureId}-{manuSfcProduce.ResourceId}";
                if (_memoryCache.TryGetValue(cacheKey, out int count) == false) count = 0;

                // 读取工序抽检次数
                if (defaultNextProcedure.CheckRate == count)
                {
                    // 如果满足抽检次数，就取出一个非"空值"的随机工序作为下一工序
                    defaultNextProcedure = procedureNodes.FirstOrDefault(f => f.CheckType != ProcessRouteInspectTypeEnum.None);

                    // TODO 重置计数器
                }
                else
                {
                    // TODO 计数器+1
                }
            }

            // 获取下一工序
            if (defaultNextProcedure == null) throw new BusinessException(nameof(ErrorCode.MES10440));
            return await _procProcedureRepository.GetByIdAsync(defaultNextProcedure.ProcedureId);
        }

    }
}
