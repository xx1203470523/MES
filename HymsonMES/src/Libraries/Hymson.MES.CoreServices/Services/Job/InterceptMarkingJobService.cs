using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Manufacture.ManuSfcMarking;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// Marking拦截作业
    /// </summary>
    [Job("Marking拦截", JobTypeEnum.Standard)]
    public class InterceptMarkingJobService : IJobService
    {
        private readonly IManuSfcMarkingCoreService _manuSfcMarkingCoreService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public InterceptMarkingJobService(IManuSfcMarkingCoreService manuSfcMarkingCoreService)
        {
            _manuSfcMarkingCoreService = manuSfcMarkingCoreService;
        }

        /// <summary>
        /// 执行前节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return await Task.FromResult<IEnumerable<JobBo>?>(default);
        }

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return await Task.FromResult<IEnumerable<JobBo>?>(default);
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            return await Task.FromResult(new EmptyRequestBo { });
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<JobResponseBo?> ExecuteAsync(object obj)
        {
            return await Task.FromResult<JobResponseBo?>(default);
        }

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return;
            if (commonBo == null) return;
            if (commonBo.InStationRequestBos == null || !commonBo.InStationRequestBos.Any()) return;

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.InStationRequestBos.Select(s => s.SFC) };

            await _manuSfcMarkingCoreService.MarkingInterceptAsync(new Bos.Manufacture.MarkingInterceptBo
            {
                SiteId = commonBo.SiteId,
                UserName = commonBo.UserName,
                ProcedureId = commonBo.ProcedureId,
                EquipmentId = commonBo.EquipmentId ?? 0,
                ResourceId = commonBo.ResourceId,
                SFCs = multiSFCBo.SFCs
            });
        }
    }
}
