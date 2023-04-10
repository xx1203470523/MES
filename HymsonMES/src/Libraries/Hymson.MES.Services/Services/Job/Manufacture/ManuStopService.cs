using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Services.Job.Common;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 中止
    /// </summary>
    public class ManuStopService
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
        /// 服务接口（作业通用）
        /// </summary>
        private readonly IJobCommonService _jobCommonService;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="jobCommonService"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuSfcProduceRepository"></param>
        public ManuStopService(ICurrentUser currentUser, ICurrentSite currentSite,
            IJobCommonService jobCommonService,
            IManuCommonService manuCommonService,
            IManuSfcProduceRepository manuSfcProduceRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _jobCommonService = jobCommonService;
            _manuCommonService = manuCommonService;
            _manuSfcProduceRepository = manuSfcProduceRepository;
        }


        /// <summary>
        /// 执行（中止）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(JobDto dto)
        {
            // 获取生产条码信息（附带条码合法性校验 + 工序活动状态校验）
            var sfcProduceEntity = await _manuCommonService.GetProduceSPCWithCheckAsync(dto.SFC, dto.ProcedureId, new SfcProduceStatusEnum[] { SfcProduceStatusEnum.Activity });

            // 读取挂载的作业并执行
            await _jobCommonService.ExecuteJobAsync(dto.FacePlateButtonId);

            // 更改状态，将条码由"活动"改为"排队"
            sfcProduceEntity.Status = SfcProduceStatusEnum.lineUp;
            sfcProduceEntity.UpdatedBy = _currentUser.UserName;
            sfcProduceEntity.UpdatedOn = HymsonClock.Now();
            await _manuSfcProduceRepository.UpdateAsync(sfcProduceEntity);
        }



    }
}
