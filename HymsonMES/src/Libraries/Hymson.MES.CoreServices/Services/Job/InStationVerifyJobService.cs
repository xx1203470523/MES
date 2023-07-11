using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Process;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 进站验证
    /// </summary>
    [Job("进站验证", JobTypeEnum.Standard)]
    public class InStationVerifyJobService : IJobService
    {
        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 仓储接口（工艺路线工序节点）
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;

        /// <summary>
        /// 仓储接口（工艺路线工序连线）
        /// </summary>
        private readonly IProcProcessRouteDetailLinkRepository _procProcessRouteDetailLinkRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="masterDataService"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        /// <param name="localizationService"></param>
        public InStationVerifyJobService(IManuCommonService manuCommonService,
            IMasterDataService masterDataService,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository,
            ILocalizationService localizationService)
        {
            _manuCommonService = manuCommonService;
            _masterDataService = masterDataService;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<InStationRequestBo>();
            if (bo == null) return;

            // 校验工序和资源是否对应
            var resourceIds = await bo.Proxy.GetValueAsync(_masterDataService.GetProcResourceIdByProcedureIdAsync, bo.ProcedureId);
            if (resourceIds == null || resourceIds.Any(a => a == bo.ResourceId) == false) throw new CustomerValidationException(nameof(ErrorCode.MES16317));

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, bo);
            if (sfcProduceEntities == null || sfcProduceEntities.Any() == false) return;

            var sfcProduceBusinessEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceBusinessEntitiesBySFCsAsync, bo);

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcProduceStatusEnum.lineUp, _localizationService.GetResource($"{typeof(SfcProduceStatusEnum).FullName}.{nameof(SfcProduceStatusEnum.lineUp)}"));
            sfcProduceBusinessEntities?.VerifyProcedureLock(bo.SFCs, bo.ProcedureId);

            // 验证条码是否被容器包装
            await _manuCommonService.VerifyContainerAsync(bo);

            //（前提：这些条码都是同一工单同一工序）
            var firstProduceEntity = sfcProduceEntities.FirstOrDefault();
            if (firstProduceEntity == null) return;

            // 获取生产工单（附带工单状态校验）
            var planWorkOrderEntity = await bo.Proxy.GetValueAsync(async parameters =>
            {
                long workOrderId = (long)parameters[0];
                bool isVerifyActivation = parameters.Length <= 1 || (bool)parameters[1];
                return await _masterDataService.GetProduceWorkOrderByIdAsync(workOrderId, isVerifyActivation);
            }, new object[] { firstProduceEntity.WorkOrderId, true });

            // 当工单已激活且完工状态，且条码处于工艺路线的首工序，进站时，提示“工单状态为完工，不允许再对工单投入”
            if (planWorkOrderEntity?.Status == PlanWorkOrderStatusEnum.Finish)
            {
                // 检查是否首工序
                var isFirstProcedure = await bo.Proxy.GetValueAsync(async parameters =>
                {
                    var processRouteId = (long)parameters[0];
                    var procedureId = (long)parameters[1];
                    return await _masterDataService.IsFirstProcedureAsync(processRouteId, procedureId);
                }, new object[] { firstProduceEntity.ProcessRouteId, firstProduceEntity.ProcedureId });

                // 因为获取工单方法已经对激活状态做了校验，这里不需要再校验
                if (isFirstProcedure) throw new CustomerValidationException(nameof(ErrorCode.MES16350));
            }

            // 如果工序对应不上
            if (firstProduceEntity.ProcedureId != bo.ProcedureId)
            {
                var processRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetProcessRouteDetailLinksByProcessRouteIdAsync(firstProduceEntity.ProcessRouteId)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES18213));

                var processRouteDetailNodes = await _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(firstProduceEntity.ProcessRouteId)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES18208));

                // 判断上一个工序是否是随机工序
                var IsRandomPreProcedure = await bo.Proxy.GetValueAsync(async parameters =>
                 {
                     var processRouteDetailLinks = (IEnumerable<ProcProcessRouteDetailLinkEntity>)parameters[0];
                     var processRouteDetailNodes = (IEnumerable<ProcProcessRouteDetailNodeEntity>)parameters[1];
                     var processRouteId = (long)parameters[2];
                     var procedureId = (long)parameters[3];
                     return await _masterDataService.IsRandomPreProcedureAsync(processRouteDetailLinks, processRouteDetailNodes, processRouteId, procedureId);
                 }, new object[] { processRouteDetailLinks, processRouteDetailNodes, firstProduceEntity.ProcessRouteId, bo.ProcedureId });

                if (IsRandomPreProcedure == false) throw new CustomerValidationException(nameof(ErrorCode.MES16308));
            }

        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            return await Task.FromResult(new JobResponseBo { });
        }

    }
}
