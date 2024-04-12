using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Execute;

namespace Hymson.MES.CoreServices.Services.Manufacture
{
    /// <summary>
    /// 过站
    /// </summary>
    public class ManuPassStationService : IManuPassStationService
    {
        /// <summary>
        /// 仓储接口（作业）
        /// </summary>
        private readonly IExecuteJobService<JobBaseBo> _executeJobService;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="executeJobService"></param>
        /// <param name="manuCommonService"></param>
        public ManuPassStationService(IExecuteJobService<JobBaseBo> executeJobService,
            IManuCommonService manuCommonService)
        {
            _executeJobService = executeJobService;
            _manuCommonService = manuCommonService;
        }


        #region 进站
        /// <summary>
        /// 批量进站（条码进站）
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseBo>> InStationRangeBySFCAsync(SFCInStationBo bo, RequestSourceEnum source = RequestSourceEnum.EquipmentApi)
        {
            // 作业请求参数
            var requestBo = new JobRequestBo
            {
                Source = source,
                Type = ManuFacePlateBarcodeTypeEnum.Product,
                SiteId = bo.SiteId,
                UserName = bo.UserName,
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
                EquipmentId = bo.EquipmentId
            };

            List<string> SFCs = new();
            List<InStationRequestBo> inStationRequestBos = new();

            SFCs = bo.SFCs.ToList();
            inStationRequestBos.AddRange(SFCs.Select(s => new InStationRequestBo { SFC = s }));

            requestBo.SFCs = SFCs;  // 这句后面要改
            requestBo.InStationRequestBos = inStationRequestBos;

            var jobBos = new List<JobBo> { };
            jobBos.Add(new JobBo { Name = "InStationJobService" });

            return await _executeJobService.ExecuteAsync(jobBos, requestBo);
        }

        /// <summary>
        /// 批量进站（托盘进站）
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseBo>> InStationRangeByVehicleAsync(VehicleInStationBo bo, RequestSourceEnum source = RequestSourceEnum.EquipmentApi)
        {
            // 作业请求参数
            var requestBo = new JobRequestBo
            {
                Source = source,
                Type = ManuFacePlateBarcodeTypeEnum.Vehicle,
                SiteId = bo.SiteId,
                UserName = bo.UserName,
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
                EquipmentId = bo.EquipmentId
            };

            // 根据载具代码获取载具里面的条码
            var vehicleSFCs = await _manuCommonService.GetSFCsByVehicleCodesAsync(new VehicleSFCRequestBo { SiteId = bo.SiteId, VehicleCodes = bo.VehicleCodes });

            requestBo.SFCs = vehicleSFCs.Select(s => s.SFC);
            requestBo.InStationRequestBos = vehicleSFCs.Select(s => new InStationRequestBo { SFC = s.SFC, VehicleCode = s.VehicleCode });

            var jobBos = new List<JobBo> { };
            jobBos.Add(new JobBo { Name = "InStationJobService" });

            return await _executeJobService.ExecuteAsync(jobBos, requestBo);
        }
        #endregion

        #region 出站
        /// <summary>
        /// 批量出站（条码出站）
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseBo>> OutStationRangeBySFCAsync(SFCOutStationBo bo, RequestSourceEnum source = RequestSourceEnum.EquipmentApi)
        {
            // 作业请求参数
            var requestBo = new JobRequestBo
            {
                Source = source,
                Type = ManuFacePlateBarcodeTypeEnum.Product,
                SiteId = bo.SiteId,
                UserName = bo.UserName,
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
                EquipmentId = bo.EquipmentId
            };

            requestBo.SFCs = bo.OutStationRequestBos.Select(s => s.SFC);
            requestBo.OutStationRequestBos = bo.OutStationRequestBos;

            var jobBos = new List<JobBo> { };
            jobBos.Add(new JobBo { Name = "OutStationJobService" });

            return await _executeJobService.ExecuteAsync(jobBos, requestBo);
        }

        /// <summary>
        /// 批量出站（托盘出站）
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseBo>> OutStationRangeByVehicleAsync(VehicleOutStationBo bo, RequestSourceEnum source = RequestSourceEnum.EquipmentApi)
        {
            // 作业请求参数
            var requestBo = new JobRequestBo
            {
                Source = source,
                Type = ManuFacePlateBarcodeTypeEnum.Vehicle,
                SiteId = bo.SiteId,
                UserName = bo.UserName,
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
                EquipmentId = bo.EquipmentId
            };

            // 根据载具代码获取载具里面的条码
            var vehicleCodes = bo.OutStationRequestBos.Select(s => s.VehicleCode ?? "").Where(w => !string.IsNullOrWhiteSpace(w));
            if (vehicleCodes == null) return new Dictionary<string, JobResponseBo>();
            var vehicleSFCs = await _manuCommonService.GetSFCsByVehicleCodesAsync(new VehicleSFCRequestBo { SiteId = bo.SiteId, VehicleCodes = vehicleCodes });

            List<OutStationRequestBo> outStationRequestBos = new();
            foreach (var item in vehicleSFCs)
            {
                var outStationRequestBo = bo.OutStationRequestBos.FirstOrDefault(f => f.VehicleCode == item.VehicleCode);
                if (outStationRequestBo == null) continue;

                outStationRequestBos.Add(new OutStationRequestBo
                {
                    SFC = item.SFC,
                    VehicleCode = item.VehicleCode,
                    IsQualified = outStationRequestBo.IsQualified,
                    ConsumeList = outStationRequestBo.ConsumeList,
                    OutStationUnqualifiedList = outStationRequestBo.OutStationUnqualifiedList
                });
            }

            requestBo.SFCs = vehicleSFCs.Select(s => s.SFC);
            requestBo.OutStationRequestBos = outStationRequestBos;

            var jobBos = new List<JobBo> { };
            jobBos.Add(new JobBo { Name = "OutStationJobService" });

            return await _executeJobService.ExecuteAsync(jobBos, requestBo);
        }
        #endregion

        #region 中止
        /// <summary>
        /// 批量中止
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseBo>> StopStationRangeBySFCAsync(SFCStopStationBo bo, RequestSourceEnum source = RequestSourceEnum.EquipmentApi)
        {
            // 作业请求参数（TODO 后面需要把这个改为使用JobRequestBo对象）
            var requestBo = new StopRequestBo
            {
                SiteId = bo.SiteId,
                UserName = bo.UserName,
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
                EquipmentId = bo.EquipmentId,
                SFCs = bo.SFCs
            };

            var jobBos = new List<JobBo> { new() { Name = "StopJobService" } };
            return await _executeJobService.ExecuteAsync(jobBos, requestBo);
        }
        #endregion

    }
}
