using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Data.Repositories.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture.ManuSfc
{
    /// <summary>
    /// 条码服务
    /// </summary>
    public class ManuSfcService : IManuSfcService
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
        /// 仓储（条码）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuSfcRepository"></param>
        public ManuSfcService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuSfcRepository manuSfcRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcRepository = manuSfcRepository;
        }


    }
}
