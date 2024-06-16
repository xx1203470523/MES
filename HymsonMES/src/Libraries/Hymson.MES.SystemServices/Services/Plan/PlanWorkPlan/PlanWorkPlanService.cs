using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.SystemServices.Dtos;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Logging;
using System.Data.SqlTypes;

namespace Hymson.MES.SystemServices.Services.Plan
{
    /// <summary>
    /// 服务（生产计划）
    /// </summary>
    public class PlanWorkPlanService : IPlanWorkPlanService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<PlanWorkPlanService> _logger;

        /// <summary>
        /// 仓储接口（生产计划）
        /// </summary>
        private readonly IPlanWorkPlanRepository _planWorkPlanRepository;

        /// <summary>
        /// 仓储接口（工作中心）
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 仓储接口（BOM表）
        /// </summary>
        private readonly IProcBomRepository _procBomRepository;

        /// <summary>
        /// 仓储接口（物料）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="planWorkPlanRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="procMaterialRepository"></param>
        public PlanWorkPlanService(ILogger<PlanWorkPlanService> logger,
            IPlanWorkPlanRepository planWorkPlanRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IProcBomRepository procBomRepository,
            IProcMaterialRepository procMaterialRepository)
        {
            _logger = logger;
            _planWorkPlanRepository = planWorkPlanRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procBomRepository = procBomRepository;
            _procMaterialRepository = procMaterialRepository;
        }

        /// <summary>
        /// 同步信息（生产计划）
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

                var resposeBo = await ConvertWorkPlanListAsync(lineEntity, lineDict);
                if (resposeBo == null) continue;

                // 添加到集合
                resposeSummaryBo.PlanAdds.AddRange(resposeBo.PlanAdds);
                resposeSummaryBo.PlanUpdates.AddRange(resposeBo.PlanUpdates);
            }

            // 插入数据
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _planWorkPlanRepository.InsertsAsync(resposeSummaryBo.PlanAdds);
            rows += await _planWorkPlanRepository.UpdatesAsync(resposeSummaryBo.PlanUpdates);
            return rows;
        }

        /// <summary>
        /// 转换信息集合（生产计划）
        /// </summary>
        /// <param name="lineEntity"></param>
        /// <param name="lineDtoDict"></param>
        /// <returns></returns>
        private async Task<SyncWorkPlanSummaryBo> ConvertWorkPlanListAsync(InteWorkCenterEntity lineEntity, IEnumerable<WorkPlanDto> lineDtoDict)
        {
            var resposeBo = new SyncWorkPlanSummaryBo();

            // 判断产线是否存在
            if (lineEntity == null) return resposeBo;

            // 初始化
            var siteId = lineEntity.SiteId ?? 0;
            var updateUser = "ERP";
            var updateTime = HymsonClock.Now();

            // 判断是否有不存在的产品编码
            var productCodes = lineDtoDict.Select(s => s.ProductCode).Distinct();
            var productEntities = await _procMaterialRepository.GetByCodesAsync(new ProcMaterialsByCodeQuery { SiteId = lineEntity.SiteId, MaterialCodes = productCodes });
            if (productEntities == null || productEntities.Any())
            {
                // 这里应该提示产品不存在
                return resposeBo;
            }

            // 判断BOM编码是否存在
            var bomCodes = lineDtoDict.Select(s => s.BomCode).Distinct();
            var bomEntities = await _procBomRepository.GetByCodesAsync(new ProcBomsByCodeQuery { SiteId = lineEntity.SiteId, Codes = bomCodes });
            if (bomEntities == null || bomEntities.Any())
            {
                // 这里应该提示BOM不存在
                return resposeBo;
            }

            // 读取已存在的生产计划记录
            var planCodes = lineDtoDict.Select(s => s.PlanCode).Distinct();
            var planEntities = await _planWorkPlanRepository.GetEntitiesAsync(new PlanWorkPlanQuery { SiteId = lineEntity.SiteId, Codes = planCodes });

            // 遍历数据
            foreach (var planDto in lineDtoDict)
            {
                var planEntity = planEntities.FirstOrDefault(f => f.PlanCode == planDto.PlanCode);

                var productEntity = productEntities.FirstOrDefault(f => f.MaterialCode == planDto.ProductCode);
                if (productEntity == null)
                {
                    // 这里应该提示产品不存在
                    continue;
                }

                var bomEntity = bomEntities.FirstOrDefault(f => f.BomCode == planDto.BomCode);
                if (bomEntity == null)
                {
                    // 这里应该提示BOM不存在
                    continue;
                }

                // 不存在的新生产计划
                if (planEntity == null)
                {
                    planEntity = new PlanWorkPlanEntity
                    {
                        PlanCode = planDto.PlanCode,
                        ProductCode = planDto.ProductCode,
                        ProductVersion = planDto.ProductVersion,
                        BomCode = planDto.BomCode,
                        BomVersion = planDto.BomVersion,
                        Qty = planDto.Qty,
                        RequirementNumber = planDto.RequirementNumber,

                        ProductId = productEntity.Id,
                        BomId = bomEntity.Id,

                        StartTime = planDto.StartTime ?? SqlDateTime.MinValue.Value,
                        EndTime = planDto.EndTime ?? SqlDateTime.MinValue.Value,

                        // TODO: 这里的字段需要确认
                        OverScale = 0,
                        Type = 0,
                        Status = PlanWorkPlanStatusEnum.NotStarted,

                        Remark = "",
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = siteId,
                        CreatedBy = updateUser,
                        CreatedOn = updateTime,
                        UpdatedBy = updateUser,
                        UpdatedOn = updateTime
                    };

                    // 添加生产计划
                    resposeBo.PlanAdds.Add(planEntity);
                }
                // 之前已存在的生产计划
                else
                {
                    // 如果不是"未开始"的生产计划，不允许修改
                    if (planEntity.Status != PlanWorkPlanStatusEnum.NotStarted)
                    {
                        // 这里应该提示当前的生产计划状态不允许修改
                        continue;
                    }

                    // 除了数量/时间，好像什么都不能随便改
                    planEntity.Qty = planDto.Qty;
                    planEntity.StartTime = planDto.StartTime ?? SqlDateTime.MinValue.Value;
                    planEntity.EndTime = planDto.EndTime ?? SqlDateTime.MinValue.Value;

                    planEntity.UpdatedBy = updateUser;
                    planEntity.UpdatedOn = updateTime;
                    resposeBo.PlanUpdates.Add(planEntity);
                }
            }

            return resposeBo;
        }

    }

    /// <summary>
    /// 同步信息BO对象（生产计划）
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
