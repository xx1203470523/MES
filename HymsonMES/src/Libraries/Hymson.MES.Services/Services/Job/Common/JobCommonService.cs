using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Data.Repositories.Manufacture;

namespace Hymson.MES.Services.Services.Job.Common
{
    /// <summary>
    /// 生产通用
    /// </summary>
    public class JobCommonService : IJobCommonService
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
        /// 仓储接口（面板按钮作业关系）
        /// </summary>
        private readonly IManuFacePlateButtonJobRelationRepository _manuFacePlateButtonJobRelationRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuFacePlateButtonJobRelationRepository"></param>
        public JobCommonService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuFacePlateButtonJobRelationRepository manuFacePlateButtonJobRelationRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuFacePlateButtonJobRelationRepository = manuFacePlateButtonJobRelationRepository;
        }


        /// <summary>
        /// 读取挂载的作业并执行
        /// </summary>
        /// <param name="facePlateButtonId"></param>
        /// <returns></returns>
        public async Task ExecuteJobAsync(long facePlateButtonId)
        {
            // 根据面板ID和按钮ID找出绑定的作业job
            var buttonJobs = await _manuFacePlateButtonJobRelationRepository.GetByFacePlateButtonIdAsync(facePlateButtonId);
            if (buttonJobs.Any() == false) return;


            // TODO

            await Task.CompletedTask;
        }

    }
}
