using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuOutStation;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuOutStation
{
    /// <summary>
    /// 出站
    /// </summary>
    public class ManuOutStationService : IManuOutStationService
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
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        public ManuOutStationService(ICurrentUser currentUser, ICurrentSite currentSite)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="param"></param>
        public async Task ExecuteAsync(SFCOutStationDto param)
        {
            var userName = _currentUser.UserName;
            // TODO

            await Task.CompletedTask;
        }

        // 条码合法性校验

        // 工单状态校验

        // 当前工序对应的线体是否支持混工单

        // 检查是否测试工序

        // 检查前工序是否可选工序

        // 检验该节点是否有挂在其他作业

        // 状态更改

    }
}
