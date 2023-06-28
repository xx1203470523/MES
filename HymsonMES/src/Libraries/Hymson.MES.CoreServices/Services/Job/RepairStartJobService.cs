using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility;
using Hymson.MES.Data.Repositories.Process;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 维修开始
    /// </summary>
    [Job("维修开始", JobTypeEnum.Standard)]
    public class RepairStartJobService : IJobService
    {

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

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
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        public RepairStartJobService(IManuCommonService manuCommonService,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository)
        {
            _manuCommonService = manuCommonService;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <param name="proxy"></param> 
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            if ((param is RepairStartRequestBo bo) == false) return;
            // 获取生产条码信息
            var sfcProduceEntitys = await param.Proxy.GetValueAsync(_manuCommonService.GetProduceEntitiesBySFCsAsync, new MultiSfcBo { SFCs = bo.SFCs, SiteId = bo.SiteId });
            if (sfcProduceEntitys?.GroupBy(it => it.ProcedureId).Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16330));
            };
            if (sfcProduceEntitys?.GroupBy(it => it.ProcedureId).Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16331));
            };
            var sfcProduceEntity = sfcProduceEntitys?.FirstOrDefault();
            // 如果工序对应不上
            if (sfcProduceEntity?.ProcedureId != bo.ProcedureId)
            {
                // 判断上一个工序是否是随机工序
                var IsRandomPreProcedure = await param.Proxy.GetValueAsync(_manuCommonService.IsRandomPreProcedureAsync, new IsRandomPreProcedureBo { ProcessRouteId = sfcProduceEntity.ProcessRouteId, ProcedureId = bo.ProcedureId });
                if (IsRandomPreProcedure == false) throw new CustomerValidationException(nameof(ErrorCode.MES16308));

                // 将SFC对应的工序改为当前工序
                sfcProduceEntity.ProcessRouteId = bo.ProcedureId;
            }

            // 校验工序和资源是否对应
            var resourceIds = await param.Proxy.GetValueAsync(_manuCommonService.GetProcResourceIdByProcedureIdAsync, bo.ProcedureId);
            if (resourceIds == null || resourceIds.Any(a => a == bo.ResourceId) == false) throw new CustomerValidationException(nameof(ErrorCode.MES16317));

            // 当前工序是否是排队状态
            if (sfcProduceEntity.Status == SfcProduceStatusEnum.Activity)
            {
                // 如果状态已经为活动中，就直接返回成功
                //return defaultDto;
                //TODO这个怎么返回？
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public async Task<TResult> DataAssemblingAsync<T, TResult>(T param) where T : JobBaseBo where TResult : JobBaseBo, new()
        {
            await Task.CompletedTask;
            return new TResult();
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public async Task ExecuteAsync()
        {
            await Task.CompletedTask;
        }

    }
}
