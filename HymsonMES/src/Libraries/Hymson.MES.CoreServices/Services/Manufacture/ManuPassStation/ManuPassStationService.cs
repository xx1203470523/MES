using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Execute;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.CoreServices.Services.Manufacture
{
    /// <summary>
    /// 过站
    /// </summary>
    public class ManuPassStationService : IManuPassStationService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<ManuPassStationService> _logger;

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
        /// <param name="logger"></param>
        /// <param name="executeJobService"></param>
        /// <param name="inteVehicleRepository"></param>
        /// <param name="inteVehiceFreightStackRepository"></param>
        public ManuPassStationService(ILogger<ManuPassStationService> logger,
            IExecuteJobService<JobBaseBo> executeJobService,
            IInteVehicleRepository inteVehicleRepository,
            IInteVehiceFreightStackRepository inteVehiceFreightStackRepository)
        {
            _logger = logger;
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
        public async Task<Dictionary<string, JobResponseBo>> InStationRangeBySFC(SFCInStationBo bo)
        {
            // 作业请求参数
            var requestBo = new JobRequestBo
            {
                Type = CodeTypeEnum.SFC,
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
        public async Task<Dictionary<string, JobResponseBo>> InStationRangeByVehicle(VehicleInStationBo bo)
        {
            // 作业请求参数
            var requestBo = new JobRequestBo
            {
                Type = CodeTypeEnum.Vehicle,
                SiteId = bo.SiteId,
                UserName = bo.UserName,
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId
            };

            List<string> SFCs = new();
            List<InStationRequestBo> inStationRequestBos = new();
            if (bo.VehicleCodes == null || !bo.VehicleCodes.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18623)).WithData("Code", "");
            }

            // 读取载具关联的条码
            var vehicleEntities = await _inteVehicleRepository.GetByCodesAsync(new EntityByCodesQuery
            {
                SiteId = requestBo.SiteId,
                Codes = bo.VehicleCodes
            });

            // 不在系统中的载具代码
            var notInSystem = bo.VehicleCodes.Except(vehicleEntities.Select(s => s.Code));
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
            var vehicleFreightStackDic = vehicleFreightStackEntities.ToLookup(w => w.VehicleId).ToDictionary(d => d.Key, d => d);

            SFCs = vehicleFreightStackEntities.Select(s => s.BarCode).ToList();
            foreach (var item in vehicleFreightStackDic)
            {
                var vehicleEntity = vehicleEntities.FirstOrDefault(f => f.Id == item.Key);
                if (vehicleEntity == null) continue;

                inStationRequestBos.AddRange(item.Value.Select(s => new InStationRequestBo { SFC = s.BarCode, VehicleCode = vehicleEntity.Code }));
            }

            requestBo.SFCs = SFCs;  // 这句后面要改
            requestBo.InStationRequestBos = inStationRequestBos;

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
        public async Task<Dictionary<string, JobResponseBo>> OutStationRangeBySFC(SFCOutStationBo bo)
        {
            // 作业请求参数
            var requestBo = new JobRequestBo
            {
                Type = CodeTypeEnum.SFC,
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
        public async Task<Dictionary<string, JobResponseBo>> OutStationRangeByVehicle(VehicleOutStationBo bo)
        {
            // 作业请求参数
            var requestBo = new JobRequestBo
            {
                Type = CodeTypeEnum.Vehicle,
                SiteId = bo.SiteId,
                UserName = bo.UserName,
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId
            };

            List<string> SFCs = new();
            List<OutStationRequestBo> outStationRequestBos = new();
            if (bo.VehicleCodes == null || !bo.VehicleCodes.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18623)).WithData("Code", "");
            }

            // 读取载具关联的条码
            var vehicleEntities = await _inteVehicleRepository.GetByCodesAsync(new EntityByCodesQuery
            {
                SiteId = requestBo.SiteId,
                Codes = bo.VehicleCodes
            });

            // 不在系统中的载具代码
            var notInSystem = bo.VehicleCodes.Except(vehicleEntities.Select(s => s.Code));
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
            var vehicleFreightStackDic = vehicleFreightStackEntities.ToLookup(w => w.VehicleId).ToDictionary(d => d.Key, d => d);

            SFCs = vehicleFreightStackEntities.Select(s => s.BarCode).ToList();
            foreach (var item in vehicleFreightStackDic)
            {
                var vehicleEntity = vehicleEntities.FirstOrDefault(f => f.Id == item.Key);
                if (vehicleEntity == null) continue;

                outStationRequestBos.AddRange(item.Value.Select(s => new OutStationRequestBo { SFC = s.BarCode, VehicleCode = vehicleEntity.Code }));
            }

            requestBo.SFCs = SFCs;  // 这句后面要改
            requestBo.OutStationRequestBos = outStationRequestBos;

            var jobBos = new List<JobBo> { };
            jobBos.Add(new JobBo { Name = "OutStationJobService" });

            return await _executeJobService.ExecuteAsync(jobBos, requestBo);
        }
        #endregion

        #region 产品参数收集
        /// <summary>
        /// 产品参数收集（出站）
        /// </summary>
        /// <param name="bos"></param>
        /// <returns></returns>
        public async Task ParameterForOutStation(IEnumerable<OutStationParameterBo> bos)
        {
            // TODO
            await Task.CompletedTask;
        }
        #endregion
    }
}
