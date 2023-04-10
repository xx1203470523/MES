using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 开始
    /// </summary>
    public class ManuStartService
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
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（工艺路线节点）
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        public ManuStartService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonService manuCommonService,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonService = manuCommonService;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _procProcedureRepository = procProcedureRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
        }


        /// <summary>
        /// 执行（开始）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(JobDto dto)
        {
            // 获取生产条码信息（附带条码合法性校验）
            var sfcProduceEntity = await _manuCommonService.GetProduceSPCWithCheckAsync(dto.SFC, dto.ProcedureId, new SfcProduceStatusEnum[] { SfcProduceStatusEnum.lineUp });

            // 获取生产工单（附带工单状态校验）
            var workOrderEntity = await _manuCommonService.GetProduceWorkOrderByIdAsync(sfcProduceEntity.WorkOrderId);

            // 获取当前工序信息
            var procedureEntity = await _procProcedureRepository.GetByIdAsync(sfcProduceEntity.ProcedureId);

            // 检查是否测试工序
            if (procedureEntity.Type == ProcedureTypeEnum.Test)
            {
                // 超过复投次数，标识为NG
                if (sfcProduceEntity.RepeatedCount > procedureEntity.Cycle) throw new BusinessException(nameof(ErrorCode.MES16036));
                sfcProduceEntity.RepeatedCount++;
            }

            // 获取工艺路线里面的工序扩展信息
            var procedureExtend = await _procProcessRouteDetailNodeRepository.GetByProcessRouteIdAsync(new ProcProcessRouteDetailNodeQuery
            {
                ProcessRouteId = workOrderEntity.ProcessRouteId,
                ProcedureId = sfcProduceEntity.ProcedureId
            });

            // 检查前工序是否可选工序
            if (procedureExtend.CheckType == ProcessRouteInspectTypeEnum.RandomInspection)
            {
                // TODO
            }

            // TODO 检验该节点是否有挂在其他作业

            // 更改状态，将条码由"排队"改为"活动"
            sfcProduceEntity.Status = SfcProduceStatusEnum.Activity;
            sfcProduceEntity.UpdatedBy = _currentUser.UserName;
            sfcProduceEntity.UpdatedOn = HymsonClock.Now();
            await _manuSfcProduceRepository.UpdateAsync(sfcProduceEntity);
        }

    }
}
