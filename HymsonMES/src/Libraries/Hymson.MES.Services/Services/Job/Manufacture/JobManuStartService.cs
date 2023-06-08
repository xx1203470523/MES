using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuInStation;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 开始
    /// </summary>
    public class JobManuStartService : IJobManufactureService
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
        private readonly IManuCommonOldService _manuCommonOldService;

        /// <summary>
        /// 服务接口（进站）
        /// </summary>
        private readonly IManuInStationService _manuInStationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuCommonOldService"></param>
        /// <param name="manuInStationService"></param>
        public JobManuStartService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonOldService manuCommonOldService,
            IManuInStationService manuInStationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonOldService = manuCommonOldService;
            _manuInStationService = manuInStationService;
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

            // 校验工序和资源是否对应
            var resourceIds = await _manuCommonOldService.GetProcResourceIdByProcedureIdAsync(bo.ProcedureId);
            if (resourceIds.Any(a => a == bo.ResourceId) == false) throw new CustomerValidationException(nameof(ErrorCode.MES16317));

            // 获取生产条码信息
            var (sfcProduceEntity, sfcProduceBusinessEntity) = await _manuCommonOldService.GetProduceSFCAsync(bo.SFC);

            // 合法性校验
            sfcProduceEntity.VerifySFCStatus(SfcProduceStatusEnum.lineUp);
            sfcProduceBusinessEntity.VerifyProcedureLock(bo.SFC, bo.ProcedureId);

            // 验证条码是否被容器包装
            await _manuCommonOldService.VerifyContainerAsync(new string[] { bo.SFC });

            // 如果工序对应不上
            if (sfcProduceEntity.ProcedureId != bo.ProcedureId)
            {
                // 判断上一个工序是否是随机工序
                var IsRandomPreProcedure = await _manuCommonOldService.IsRandomPreProcedureAsync(sfcProduceEntity.ProcessRouteId, bo.ProcedureId);
                if (IsRandomPreProcedure == false) throw new CustomerValidationException(nameof(ErrorCode.MES16308));

                // 将SFC对应的工序改为当前工序
                sfcProduceEntity.ProcedureId = bo.ProcedureId;
            }

            // 进站
            sfcProduceEntity.ResourceId = bo.ResourceId;
            _ = await _manuInStationService.InStationAsync(sfcProduceEntity);

            defaultDto.Content?.Add("PackageCom", "False");
            defaultDto.Content?.Add("BadEntryCom", "False");
            if (param.ContainsKey("IsClear")) defaultDto.Content?.Add("IsClear", param["IsClear"]);

            defaultDto.Message = $"条码{param["SFC"]}设置为活动状态成功！";
            return defaultDto;
        }

    }
}
