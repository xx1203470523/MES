using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuOutStation
{
    /// <summary>
    /// 中止
    /// </summary>
    public class ManuStopService : IManuStopService
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
        public ManuStopService(ICurrentUser currentUser, ICurrentSite currentSite,
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
        /// 中止
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(SFCWorkDto dto)
        {
            // 获取生产条码信息（附带条码合法性校验）
            var sfcProduceEntity = await _manuCommonService.GetProduceSPCWithCheckAsync(dto.SFC);

            // 产品编码是否和工序对应
            if (sfcProduceEntity.ProcedureId != dto.ProcedureId)
            {
                // TODO SFC不在当前工序排队，请检查
                return;
            }
            // 产品编码是否在当前活动工序
            if (sfcProduceEntity.Status == SfcProduceStatusEnum.Activity)
            {
                // TODO SFC状态为***，请先置于活动
                return;
            }

            // 更改状态，将条码由"活动"改为"排队"
            sfcProduceEntity.Status = SfcProduceStatusEnum.lineUp;
            sfcProduceEntity.UpdatedBy = _currentUser.UserName;
            sfcProduceEntity.UpdatedOn = HymsonClock.Now();
            await _manuSfcProduceRepository.UpdateAsync(sfcProduceEntity);
        }



    }
}
