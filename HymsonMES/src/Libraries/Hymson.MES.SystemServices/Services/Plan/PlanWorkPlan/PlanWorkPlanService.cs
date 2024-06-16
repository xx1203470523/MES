using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.SystemServices.Dtos;
using Hymson.MES.SystemServices.Services.Process;
using Hymson.Utils.Tools;

namespace Hymson.MES.SystemServices.Services.Plan
{
    /// <summary>
    /// 服务（生产计划）
    /// </summary>
    public class PlanWorkPlanService : IPlanWorkPlanService
    {
        /// <summary>
        /// 仓储接口（生产计划）
        /// </summary>
        private readonly IPlanWorkPlanRepository _planWorkPlanRepository;

        /// <summary>
        /// 仓储接口（工作中心）
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="planWorkPlanRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        public PlanWorkPlanService(IPlanWorkPlanRepository planWorkPlanRepository,
            IInteWorkCenterRepository inteWorkCenterRepository)
        {
            _planWorkPlanRepository = planWorkPlanRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
        }

        /// <summary>
        /// 生产计划（同步）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        public async Task<int> SyncWorkPlanAsync(IEnumerable<WorkPlanDto> requestDtos)
        {
            if (requestDtos == null || !requestDtos.Any()) return 0;

            var resposeSummaryBo = new SyncWorkPlanSummaryBo();

            // 判断产线是否存在
            var lineCodes = requestDtos.Select(s => s.LineCode).Distinct();
            var lineEntities = await _inteWorkCenterRepository.GetAllSiteEntitiesAsync(new InteWorkCenterQuery { Codes = lineCodes });

            // 通过产线分组数据（支持一次传多个站点的数据，但是不建议这么传）
            var requestDict = requestDtos.GroupBy(g => g.LineCode);
            foreach (var lineDict in requestDict)
            {
                var lineEntity = lineEntities.FirstOrDefault(f => f.Code == lineDict.Key);
                if (lineEntity == null)
                {
                    // 这里应该提示产线不存在
                    continue;
                }

            }

            // 插入数据
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _planWorkPlanRepository.InsertsAsync(resposeSummaryBo.PlanAdds);
            rows += await _planWorkPlanRepository.UpdatesAsync(resposeSummaryBo.PlanUpdates);
            return rows;
        }

    }

    /// <summary>
    /// 同步计划信息BO对象
    /// </summary>
    public class SyncWorkPlanSummaryBo
    {
        /// <summary>
        /// 新增（工作计划）
        /// </summary>
        public List<PlanWorkPlanEntity> PlanAdds { get; set; } = new();
        /// <summary>
        /// 更新（工作计划）
        /// </summary>
        public List<PlanWorkPlanEntity> PlanUpdates { get; set; } = new();
    }
}
