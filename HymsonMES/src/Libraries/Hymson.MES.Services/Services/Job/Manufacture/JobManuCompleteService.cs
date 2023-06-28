using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Execute;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Job.Manufacture
{
    /// <summary>
    /// 完成
    /// </summary>
    public class JobManuCompleteService : IJobManufactureService
    {
        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonOldService _manuCommonOldService;

        /// <summary>
        /// 服务接口（出站）
        /// </summary>
        private readonly IManuOutStationService _manuOutStationService;

        private readonly IExecuteJobService<InStationRequestBo> _ExecuteJobService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="manuCommonOldService"></param>
        /// <param name="manuOutStationService"></param>
        public JobManuCompleteService(
            IManuCommonService manuCommonService,
            IManuCommonOldService manuCommonOldService,
            IManuOutStationService manuOutStationService, IExecuteJobService<InStationRequestBo> ExecuteJobService)
        {
            _manuCommonService = manuCommonService;
            _manuCommonOldService = manuCommonOldService;
            _manuOutStationService = manuOutStationService;
            _ExecuteJobService = ExecuteJobService;
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
        /// 执行（完成）
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
            var (sfcProduceEntity, _) = await _manuCommonOldService.GetProduceSFCAsync(bo.SFC);

            /*
            var processRouteId = 15968606561873920;
            var result = await _jobContextProxy.GetValueAsync(_manuCommonOldService.GetProcessRouteAsync, processRouteId);
            */

        
            var jobBos = new List<JobBo> { };
            jobBos.Add(new JobBo { Name = "InStationJobService" });

            await _ExecuteJobService.ExecuteAsync(jobBos, new InStationRequestBo { });
       
            // 合法性校验
            sfcProduceEntity.VerifySFCStatus(SfcProduceStatusEnum.Activity)
                            .VerifyProcedure(bo.ProcedureId)
                            .VerifyResource(bo.ResourceId);

            // 验证BOM主物料数量
            await _manuCommonService.VerifyBomQtyAsync(new ManuProcedureBomBo
            {
                SiteId = sfcProduceEntity.SiteId,
                SFCs = new string[] { bo.SFC },
                ProcedureId = bo.ProcedureId,
                BomId = sfcProduceEntity.ProductBOMId
            });

            // 出站
            _ = await _manuOutStationService.OutStationAsync(sfcProduceEntity);

            defaultDto.Content?.Add("PackageCom", "False");
            defaultDto.Content?.Add("BadEntryCom", "False");
            defaultDto.Content?.Add("Qty", "1");
            if (param.ContainsKey("IsClear")) defaultDto.Content?.Add("IsClear", param["IsClear"]);

            defaultDto.Message = $"条码{param["SFC"]}完成，已于NF排队！";
            return defaultDto;
        }

    }
}
