using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Execute;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;

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
        /// 仓储接口（载具注册）
        /// </summary>
        private readonly IInteVehicleRepository _inteVehicleRepository;

        /// <summary>
        /// 仓储接口（二维载具条码明细）
        /// </summary>
        private readonly IInteVehiceFreightStackRepository _inteVehiceFreightStackRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="executeJobService"></param>
        /// <param name="inteVehicleRepository"></param>
        /// <param name="inteVehiceFreightStackRepository"></param>
        public ManuPassStationService(IExecuteJobService<JobBaseBo> executeJobService,
            IInteVehicleRepository inteVehicleRepository,
            IInteVehiceFreightStackRepository inteVehiceFreightStackRepository)
        {
            _executeJobService = executeJobService;
            _inteVehicleRepository = inteVehicleRepository;
            _inteVehiceFreightStackRepository = inteVehiceFreightStackRepository;
        }

        #region 进站
        /// <summary>
        /// 批量进站（条码进站）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseBo>> InStationRangeBySFCAsync(SFCInStationBo bo)
        {
            // 作业请求参数
            var requestBo = new JobRequestBo
            {
                Type = ManuFacePlateBarcodeTypeEnum.Product,
                SiteId = bo.SiteId,
                UserName = bo.UserName,
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId
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
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseBo>> InStationRangeByVehicleAsync(VehicleInStationBo bo)
        {
            // 作业请求参数
            var requestBo = new JobRequestBo
            {
                Type = ManuFacePlateBarcodeTypeEnum.Vehicle,
                SiteId = bo.SiteId,
                UserName = bo.UserName,
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId
            };

            // 根据载具代码获取载具里面的条码
            var vehicleSFCs = await GetSFCsByVehicleCodesAsync(new VehicleSFCRequestBo { SiteId = bo.SiteId, VehicleCodes = bo.VehicleCodes });

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
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseBo>> OutStationRangeBySFCAsync(SFCOutStationBo bo)
        {
            // 作业请求参数
            var requestBo = new JobRequestBo
            {
                Type = ManuFacePlateBarcodeTypeEnum.Product,
                SiteId = bo.SiteId,
                UserName = bo.UserName,
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId
            };

            List<string> SFCs = new();
            List<OutStationRequestBo> outStationRequestBos = new();

            SFCs = bo.SFCs.ToList();
            outStationRequestBos.AddRange(SFCs.Select(s => new OutStationRequestBo { SFC = s }));

            requestBo.SFCs = SFCs;  // 这句后面要改
            requestBo.OutStationRequestBos = outStationRequestBos;

            var jobBos = new List<JobBo> { };
            jobBos.Add(new JobBo { Name = "OutStationJobService" });

            return await _executeJobService.ExecuteAsync(jobBos, requestBo);
        }

        /// <summary>
        /// 批量出站（托盘出站）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseBo>> OutStationRangeByVehicleAsync(VehicleOutStationBo bo)
        {
            // 作业请求参数
            var requestBo = new JobRequestBo
            {
                Type = ManuFacePlateBarcodeTypeEnum.Vehicle,
                SiteId = bo.SiteId,
                UserName = bo.UserName,
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId
            };

            // 根据载具代码获取载具里面的条码
            var vehicleSFCs = await GetSFCsByVehicleCodesAsync(new VehicleSFCRequestBo { SiteId = bo.SiteId, VehicleCodes = bo.VehicleCodes });

            requestBo.SFCs = vehicleSFCs.Select(s => s.SFC);
            requestBo.OutStationRequestBos = vehicleSFCs.Select(s => new OutStationRequestBo { SFC = s.SFC, VehicleCode = s.VehicleCode });


            var jobBos = new List<JobBo> { };
            jobBos.Add(new JobBo { Name = "OutStationJobService" });

            return await _executeJobService.ExecuteAsync(jobBos, requestBo);
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 获取载具里面的条码（带验证）
        /// </summary>
        /// <param name="requestBo"></param>
        /// <returns></returns>
        public async Task<IEnumerable<VehicleSFCResponseBo>> GetSFCsByVehicleCodesAsync(VehicleSFCRequestBo requestBo)
        {
            if (requestBo.VehicleCodes == null || !requestBo.VehicleCodes.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18623)).WithData("Code", "");
            }

            // 读取载具关联的条码
            var vehicleEntities = await _inteVehicleRepository.GetByCodesAsync(new EntityByCodesQuery
            {
                SiteId = requestBo.SiteId,
                Codes = requestBo.VehicleCodes
            });

            // 不在系统中的载具代码
            var notInSystem = requestBo.VehicleCodes.Except(vehicleEntities.Select(s => s.Code));
            if (notInSystem.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18624))
                    .WithData("Code", string.Join(',', notInSystem));
            }

            // 查询载具关联的条码明细
            var vehicleFreightStackEntities = await _inteVehiceFreightStackRepository.GetEntitiesAsync(new EntityByParentIdsQuery
            {
                SiteId = requestBo.SiteId,
                ParentIds = vehicleEntities.Select(s => s.Id)
            });

            List<VehicleSFCResponseBo> list = new();
            var vehicleFreightStackDic = vehicleFreightStackEntities.ToLookup(w => w.VehicleId).ToDictionary(d => d.Key, d => d);
            foreach (var item in vehicleFreightStackDic)
            {
                var vehicleEntity = vehicleEntities.FirstOrDefault(f => f.Id == item.Key);
                if (vehicleEntity == null) continue;

                list.AddRange(item.Value.Select(s => new VehicleSFCResponseBo { SFC = s.BarCode, VehicleCode = vehicleEntity.Code }));
            }

            return list;
        }
        #endregion

    }
}
