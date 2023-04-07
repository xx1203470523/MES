using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuOutStation
{
    /// <summary>
    /// 开始
    /// </summary>
    public class ManuStartupService : IManuStartupService
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
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

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
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        public ManuStartupService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonService manuCommonService,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonService = manuCommonService;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _procProcedureRepository = procProcedureRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
        }


        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(SFCWorkDto dto)
        {
            // 获取生产条码信息（附带条码合法性校验）
            var sfcProduceEntity = await _manuCommonService.GetProduceSPCWithCheckAsync(dto.SFC);

            // 获取生产工单（附带工单状态校验）
            var workOrderEntity = await _manuCommonService.GetProduceWorkOrderByIdAsync(sfcProduceEntity.WorkOrderId);

            // 获取当前工序信息
            var procedureEntity = await _procProcedureRepository.GetByIdAsync(sfcProduceEntity.ProcedureId);

            // 检查是否测试工序
            if (procedureEntity.Type == ProcedureTypeEnum.Test)
            {
                // 是否，超过复投次数，标识为NG
                // TODO
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
                // 是否在当前工序排队
                // TODO
            }
            
            // TODO 检验该节点是否有挂在其他作业

            // 更改状态，将条码由"排队"改为"活动"
            sfcProduceEntity.Status = SfcProduceStatusEnum.Activity;
            sfcProduceEntity.UpdatedBy = _currentUser.UserName;
            sfcProduceEntity.UpdatedOn = HymsonClock.Now();
            await _manuSfcProduceRepository.UpdateAsync(sfcProduceEntity);
        }


        // 质检

        // 自动过站逻辑


    }
}
