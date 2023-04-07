using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuPackage
{
    /// <summary>
    /// 组装
    /// </summary>
    public class ManuPackageService
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
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        public ManuPackageService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonService manuCommonService,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcProduceRepository manuSfcProduceRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonService = manuCommonService;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
        }


        /// <summary>
        /// 组装
        /// </summary>
        /// <param name="procedureId"></param>
        /// <param name="resourceId"></param>
        /// <param name="productId"></param>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(long procedureId, long resourceId, long productId, string sfc)
        {
            // 获取生产条码信息（附带条码合法性校验）
            var sfcEntity = await _manuCommonService.GetProduceSPCWithCheckAsync(sfc);

            // 产品编码是否在当前工序活动
            if (sfcEntity.ProcedureId != productId)
            {
                // TODO SFC不在当前工序排队，请检查
                return;
            }

            if (sfcEntity.Status == SfcProduceStatusEnum.Activity)
            {
                // TODO SFC状态为***，请先置于活动
                return;
            }

            // TODO 获取条码对于的工序BOM
            

            // 


        }

    }
}
