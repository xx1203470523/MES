using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCommonDto;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon
{
    /// <summary>
    /// 生产通用
    /// </summary>
    public class ManuCommonService : IManuCommonService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        public ManuCommonService(ICurrentUser currentUser,
            ICurrentSite currentSite,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcedureRepository procProcedureRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcedureRepository = procProcedureRepository;
        }

        /// <summary>
        /// 获取生产工单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderEntity> GetProduceWorkOrderByIdAsync(long id)
        {
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(id);

            if (planWorkOrderEntity == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES16301));
            }

            if (planWorkOrderEntity.IsLocked == YesOrNoEnum.Yes)
            {
                throw new BusinessException(nameof(ErrorCode.MES16302)).WithData("ordercode", planWorkOrderEntity.OrderCode);
            }

            if (planWorkOrderEntity.Status != PlanWorkOrderStatusEnum.SendDown
             || planWorkOrderEntity.Status != PlanWorkOrderStatusEnum.InProduction
             || planWorkOrderEntity.Status != PlanWorkOrderStatusEnum.Finish)
            {
                throw new BusinessException(nameof(ErrorCode.MES16303)).WithData("ordercode", planWorkOrderEntity.OrderCode);
            }
            return planWorkOrderEntity;
        }

        /// <summary>
        /// 获取首工序
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcessRouteProcedureDto> GetFirstProcedureAsync(long id)
        {
            var procProcessRouteDetailNodeEntity = await _procProcessRouteDetailNodeRepository.GetFirstProcedureByProcessRouteIdAsync(id);
            if (procProcessRouteDetailNodeEntity == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES16304));
            }
            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(procProcessRouteDetailNodeEntity.ProcedureId);
           
            return new ProcessRouteProcedureDto
            {
                ProcessRouteId = id,
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
        /// 获取下工序
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<long> GetNextProcedureAsync(long id)
        {
            throw new NotFiniteNumberException();
        }
    }
}
