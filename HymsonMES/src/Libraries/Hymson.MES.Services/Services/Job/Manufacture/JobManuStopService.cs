using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query;
using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 中止
    /// </summary>
    public class JobManuStopService : IJobManufactureService
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
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuSfcProduceRepository"></param>
        public JobManuStopService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonService manuCommonService,
            IManuSfcProduceRepository manuSfcProduceRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonService = manuCommonService;
            _manuSfcProduceRepository = manuSfcProduceRepository;
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
        /// 执行（中止）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<JobResponseDto> ExecuteAsync(Dictionary<string, string>? param)
        {
            var defaultDto = new JobResponseDto { };

            var bo = new ManufactureBo
            {
                SFC = param["SFC"],
                ProcedureId = param["ProcedureId"].ParseToLong(),
                ResourceId = param["ResourceId"].ParseToLong()
            };

            // 获取生产条码信息
            var (sfcProduceEntity, _) = await _manuCommonService.GetProduceSFCAsync(bo.SFC);

            // 合法性校验
            sfcProduceEntity.VerifySFCStatus(SfcProduceStatusEnum.Activity)
                            .VerifyProcedure(bo.ProcedureId)
                            .VerifyResource(bo.ResourceId);

            // 更改状态，将条码由"活动"改为"排队"
            sfcProduceEntity.Status = SfcProduceStatusEnum.lineUp;
            sfcProduceEntity.UpdatedBy = _currentUser.UserName;
            sfcProduceEntity.UpdatedOn = defaultDto.Time;

            _ = await _manuSfcProduceRepository.UpdateAsync(sfcProduceEntity);

            defaultDto.Content?.Add("PackageCom", "False");
            defaultDto.Content?.Add("BadEntryCom", "False");
            if (param.ContainsKey("IsClear")) defaultDto.Content?.Add("IsClear", param["IsClear"]);

            defaultDto.Message = $"条码{param["SFC"]}已中止！";
            return defaultDto;
        }
    }
}
