using Hymson.Authentication.JwtBearer.Security;
using Hymson.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ManuSfcService(ICurrentUser currentUser, ICurrentSite currentSite)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
        }
    }
}
