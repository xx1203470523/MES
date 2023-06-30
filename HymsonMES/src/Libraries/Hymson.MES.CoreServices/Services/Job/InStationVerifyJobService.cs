using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
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
        /// 构造函数
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="masterDataService"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        public InStationVerifyJobService(IManuCommonService manuCommonService, IMasterDataService masterDataService,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository)
        {
            _manuCommonService = manuCommonService;
            _masterDataService = masterDataService;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            if ((param is InStationRequestBo bo) == false) return;

            // 校验工序和资源是否对应
            var resourceIds = await param.Proxy.GetValueAsync(_masterDataService.GetProcResourceIdByProcedureIdAsync, bo.ProcedureId);
            if (resourceIds == null || resourceIds.Any(a => a == bo.ResourceId) == false) throw new CustomerValidationException(nameof(ErrorCode.MES16317));

            // 获取生产条码信息
            var sfcProduceEntities = await param.Proxy.GetValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, bo);
            if (sfcProduceEntities == null || sfcProduceEntities.Any() == false) return;

            var sfcProduceBusinessEntities = await param.Proxy.GetValueAsync(_masterDataService.GetProduceBusinessEntitiesBySFCsAsync, bo);

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcProduceStatusEnum.lineUp);
            sfcProduceBusinessEntities?.VerifyProcedureLock(bo.SFCs, bo.ProcedureId);

            // 验证条码是否被容器包装
            await _manuCommonService.VerifyContainerAsync(bo);

            //（前提：这些条码都是同一工单同一工序）
            var firstProduceEntity = sfcProduceEntities.FirstOrDefault();
            if (firstProduceEntity == null) return;

            // 获取生产工单（附带工单状态校验）
            _ = await _masterDataService.GetProduceWorkOrderByIdAsync(firstProduceEntity.WorkOrderId);

            // 如果工序对应不上
            if (firstProduceEntity.ProcedureId != bo.ProcedureId)
            {
                var processRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetProcessRouteDetailLinksByProcessRouteIdAsync(firstProduceEntity.ProcessRouteId)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES18213));

                var processRouteDetailNodes = await _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(firstProduceEntity.ProcessRouteId)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES18208));

                // 判断上一个工序是否是随机工序
                var IsRandomPreProcedure = await _masterDataService.IsRandomPreProcedureAsync(processRouteDetailLinks, processRouteDetailNodes, firstProduceEntity.ProcessRouteId, bo.ProcedureId);
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
        public async Task<int> ExecuteAsync(object obj)
        {
            await Task.CompletedTask;
            return 0;
        }

    }
}
