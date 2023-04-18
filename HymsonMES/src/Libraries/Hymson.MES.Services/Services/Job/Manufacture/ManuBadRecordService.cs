using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Newtonsoft.Json;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 不良录入
    /// </summary>
    public class ManuBadRecordService : IManufactureJobService
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
        /// <param name="manuCommonService"></param>
        /// <param name="manuSfcProduceRepository"></param>
        public ManuBadRecordService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonService manuCommonService,
            IManuSfcProduceRepository manuSfcProduceRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
        }


        /// <summary>
        /// 执行（不良录入）
        /// </summary>
        /// <param name="extra"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(string? extra)
        {
            if (string.IsNullOrEmpty(extra) == true) return 0;

            var dto = JsonConvert.DeserializeObject<ManufactureBo>(extra);
            if (dto == null) return 0;

            // TODO
            return await Task.FromResult(0);
        }



    }
}
