using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
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
        /// 仓储接口（系统配置）
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 仓储接口（生产计划）
        /// </summary>
        private readonly IPlanWorkPlanRepository _planWorkPlanRepository;

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
        /// <param name="sysConfigRepository"></param>
        /// <param name="planWorkPlanRepository"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="procMaterialRepository"></param>
        public PlanWorkPlanService(ILogger<PlanWorkPlanService> logger,
            ISysConfigRepository sysConfigRepository,
            IPlanWorkPlanRepository planWorkPlanRepository,
            IProcBomRepository procBomRepository,
            IProcMaterialRepository procMaterialRepository)
        {
            _logger = logger;
            _sysConfigRepository = sysConfigRepository;
            _planWorkPlanRepository = planWorkPlanRepository;
            _procBomRepository = procBomRepository;
            _procMaterialRepository = procMaterialRepository;
        }

        public async Task<IEnumerable<RotorWorkOrder>> SyncWorkOrderAsync(long WorkCenterId)
        {
            //获取产品编码

            return null;
        }

        /// <summary>
        /// 同步信息（生产计划）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        public async Task<int> SyncWorkPlanAsync(IEnumerable<WorkPlanDto> requestDtos)
        {
            if (requestDtos == null || !requestDtos.Any()) return 0;

            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.ERPSite });
            if (configEntities == null || !configEntities.Any()) return 0;

            var resposeBo = await ConvertWorkPlanListAsync(configEntities.FirstOrDefault(), requestDtos);
            if (resposeBo == null) return 0;

            // 添加到集合
            var resposeSummaryBo = new SyncWorkPlanSummaryBo();
            resposeSummaryBo.Adds.AddRange(resposeBo.Adds);
            resposeSummaryBo.Updates.AddRange(resposeBo.Updates);

            // 插入数据
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _planWorkPlanRepository.InsertsAsync(resposeSummaryBo.Adds);
            rows += await _planWorkPlanRepository.UpdatesAsync(resposeSummaryBo.Updates);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 转换信息集合（生产计划）
        /// </summary>
        /// <param name="configEntity"></param>
        /// <param name="lineDtoDict"></param>
        /// <returns></returns>
        private async Task<SyncWorkPlanSummaryBo?> ConvertWorkPlanListAsync(SysConfigEntity? configEntity, IEnumerable<WorkPlanDto> lineDtoDict)
        {
            // 判断是否存在（配置）
            if (configEntity == null) return default;

            // 初始化
            var siteId = configEntity.Value.ParseToLong();
            var updateUser = "ERP";
            var updateTime = HymsonClock.Now();

            var resposeBo = new SyncWorkPlanSummaryBo();

            // 判断是否有不存在的产品编码
            var productCodes = lineDtoDict.Select(s => s.ProductCode).Distinct();
            var productEntities = await _procMaterialRepository.GetByCodesAsync(new ProcMaterialsByCodeQuery { SiteId = siteId, MaterialCodes = productCodes });
            if (productEntities == null || productEntities.Any())
            {
                // 这里应该提示产品不存在
                return resposeBo;
            }

            // 判断BOM编码是否存在
            var bomCodes = lineDtoDict.Select(s => s.BomCode).Distinct();
            var bomEntities = await _procBomRepository.GetByCodesAsync(new ProcBomsByCodeQuery { SiteId = siteId, Codes = bomCodes });
            if (bomEntities == null || bomEntities.Any())
            {
                // 这里应该提示BOM不存在
                return resposeBo;
            }

            // 读取已存在的生产计划记录
            var planCodes = lineDtoDict.Select(s => s.PlanCode).Distinct();
            var planEntities = await _planWorkPlanRepository.GetEntitiesAsync(new PlanWorkPlanQuery { SiteId = siteId, Codes = planCodes });

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

                        PlanStartTime = planDto.PlanStartTime ?? SqlDateTime.MinValue.Value,
                        PlanEndTime = planDto.PlanEndTime ?? SqlDateTime.MinValue.Value,

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
                    resposeBo.Adds.Add(planEntity);
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
                    planEntity.PlanStartTime = planDto.PlanStartTime ?? SqlDateTime.MinValue.Value;
                    planEntity.PlanEndTime = planDto.PlanEndTime ?? SqlDateTime.MinValue.Value;

                    planEntity.UpdatedBy = updateUser;
                    planEntity.UpdatedOn = updateTime;
                    resposeBo.Updates.Add(planEntity);
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
        public List<PlanWorkPlanEntity> Adds { get; set; } = new();
        /// <summary>
        /// 更新（工作计划）
        /// </summary>
        public List<PlanWorkPlanEntity> Updates { get; set; } = new();
    }
}
