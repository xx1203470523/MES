using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 不良录入
    /// </summary>
    public class JobManuBadRecordService : IJobManufactureService
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
        /// 服务接口（不良录入）
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuProductBadRecordRepository"></param>
        public JobManuBadRecordService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuProductBadRecordRepository manuProductBadRecordRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
        }


        /// <summary>
        /// 验证参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync(Dictionary<string, string>? param)
        {
            if (param == null ||
                param.ContainsKey("SFC") == false
                || param.ContainsKey("ProcedureId") == false
                || param.ContainsKey("ResourceId") == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16312));
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 执行（不良录入）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<JobResponseDto> ExecuteAsync(Dictionary<string, string>? param)
        {
            var defaultDto = new JobResponseDto { };
            defaultDto.Content?.Add("PackageCom", "False");

            var bo = new ManufactureBo
            {
                SFC = param["SFC"],
                ProcedureId = param["ProcedureId"].ParseToLong(),
                ResourceId = param["ResourceId"].ParseToLong()
            };

            var manuProductBadRecordViews = await _manuProductBadRecordRepository.GetBadRecordsBySfcAsync(new ManuProductBadRecordQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = bo.SFC,
            });
            var isShow = manuProductBadRecordViews != null && manuProductBadRecordViews.Any() == true;

            defaultDto.Content?.Add("BadEntryCom", $"{isShow}".ToString());
            defaultDto.Message = $"条码{bo.SFC}" + (isShow ? "开始录入" : "已经完成录入，无需重复录入！");

            return defaultDto;
        }


    }
}
