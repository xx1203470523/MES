using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuPackage
{
    /// <summary>
    /// 组装
    /// </summary>
    public class ManuPackageService : IManuPackageService
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
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuCommonService"></param>
        public ManuPackageService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonService manuCommonService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonService = manuCommonService;
        }


        /// <summary>
        /// 执行（组装）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<int> PackageAsync(ManufactureBo bo)
        {
            var rows = 0;

            // 获取生产条码信息
            var (sfcProduceEntity, _) = await _manuCommonService.GetProduceSFCAsync(bo.SFC);

            // 合法性校验
            sfcProduceEntity.VerifySFCStatus(SfcProduceStatusEnum.Activity).VerifyProcedure(bo.ProcedureId);

            return rows;
        }

    }
}
