using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuPackage;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 开始（维修）
    /// </summary>
    public class JobManuRepairStartService : IJobManufactureService
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
        /// 服务接口（维修）
        /// </summary>
        private readonly IManuRepairService _manuRepairService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuRepairService"></param>
        public JobManuRepairStartService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonService manuCommonService,
            IManuRepairService manuRepairService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonService = manuCommonService;
            _manuRepairService = manuRepairService;
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
        /// 执行（开始）
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

            // 如果工序对应不上
            if (sfcProduceEntity.ProcedureId != bo.ProcedureId)
            {
                // 判断上一个工序是否是随机工序
                var IsRandomPreProcedure = await _manuCommonService.IsRandomPreProcedureAsync(sfcProduceEntity.ProcessRouteId, bo.ProcedureId);
                if (IsRandomPreProcedure == false) throw new CustomerValidationException(nameof(ErrorCode.MES16308));

                // 将SFC对应的工序改为当前工序
                sfcProduceEntity.ProcessRouteId = bo.ProcedureId;
            }

            // 校验工序和资源是否对应
            var resourceIds = await _manuCommonService.GetProcResourceIdByProcedureIdAsync(bo.ProcedureId);
            if (resourceIds.Any(a => a == bo.ResourceId) == false) throw new CustomerValidationException(nameof(ErrorCode.MES16317));

            // 当前工序是否是排队状态
            if (sfcProduceEntity.Status == SfcProduceStatusEnum.Activity)
            {
                // 如果状态已经为活动中，就直接返回成功
                return defaultDto;
            }

            // 开始维修
            sfcProduceEntity.ResourceId = bo.ResourceId;
            var rows = await _manuRepairService.StartAsync(sfcProduceEntity);

            defaultDto.Content?.Add("TableCom", (rows > 0).ToString());
            if (param.ContainsKey("IsClear")) defaultDto.Content?.Add("IsClear", param["IsClear"]);

            return defaultDto;
        }

    }
}
