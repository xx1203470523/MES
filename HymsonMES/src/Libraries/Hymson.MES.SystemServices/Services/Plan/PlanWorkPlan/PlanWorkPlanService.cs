using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
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


        /// <summary>
        /// 同步信息（生产计划）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        public async Task<int> SyncWorkPlanAsync(IEnumerable<SyncWorkPlanDto> requestDtos)
        {
            if (requestDtos == null || !requestDtos.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.ERPSite });
            if (configEntities == null || !configEntities.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10139));

            var resposeBo = await ConvertWorkPlanListAsync(configEntities.FirstOrDefault(), requestDtos)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

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
        /// 取消（生产计划）
        /// </summary>
        /// <param name="planCodes"></param>
        /// <returns></returns>
        public async Task<int> CancelWorkPlanAsync(IEnumerable<string> planCodes)
        {
            if (planCodes == null || !planCodes.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.ERPSite });
            if (configEntities == null || !configEntities.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10139));

            var configEntity = configEntities.FirstOrDefault()
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10139));

            // 初始化
            var siteId = configEntity.Value.ParseToLong();
            var updateUser = "ERP";
            var updateTime = HymsonClock.Now();

            // 读取生产计划列表
            var workPlanEntities = await _planWorkPlanRepository.GetEntitiesAsync(new PlanWorkPlanQuery
            {
                SiteId = siteId,
                Codes = planCodes
            });

            // 如果存在不是"未开始"的生产计划，不允许取消
            if (workPlanEntities.Any(a => a.Status != PlanWorkPlanStatusEnum.NotStarted))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10247)).WithData("Status", PlanWorkPlanStatusEnum.NotStarted.GetDescription());
            }

            List<PlanWorkPlanEntity> updates = new();
            foreach (var planEntity in workPlanEntities)
            {
                planEntity.Status = PlanWorkPlanStatusEnum.Canceled;
                planEntity.UpdatedBy = updateUser;
                planEntity.UpdatedOn = updateTime;
                updates.Add(planEntity);
            }

            // 更新数据
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _planWorkPlanRepository.UpdatesAsync(updates);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 转换信息集合（生产计划）
        /// </summary>
        /// <param name="configEntity"></param>
        /// <param name="lineDtoDict"></param>
        /// <returns></returns>
        private async Task<SyncWorkPlanSummaryBo?> ConvertWorkPlanListAsync(SysConfigEntity? configEntity, IEnumerable<SyncWorkPlanDto> lineDtoDict)
        {
            // 判断是否存在（配置）
            if (configEntity == null) throw new CustomerValidationException(nameof(ErrorCode.MES10139));

            // 初始化
            var siteId = configEntity.Value.ParseToLong();
            var updateUser = "ERP";
            var updateTime = HymsonClock.Now();

            var resposeBo = new SyncWorkPlanSummaryBo();

            // 判断是否有不存在的产品编码
            var productCodes = lineDtoDict.SelectMany(s => s.Products).Select(s => s.ProductCode).Distinct();
            var productEntities = await _procMaterialRepository.GetByCodesAsync(new ProcMaterialsByCodeQuery { SiteId = siteId, MaterialCodes = productCodes });
            if (productEntities == null || productEntities.Any())
            {
                // 这里应该提示产品不存在
                throw new CustomerValidationException(nameof(ErrorCode.MES10244)).WithData("Code", string.Join(',', productCodes));
            }

            // 判断BOM编码是否存在
            var bomCodes = lineDtoDict.SelectMany(s => s.Products).Select(s => s.BomCode).Distinct();
            var bomEntities = await _procBomRepository.GetByCodesAsync(new ProcBomsByCodeQuery { SiteId = siteId, Codes = bomCodes });
            if (bomEntities == null || bomEntities.Any())
            {
                // 这里应该提示BOM不存在
                throw new CustomerValidationException(nameof(ErrorCode.MES10233)).WithData("bomCode", string.Join(',', bomCodes));
            }

            // 读取已存在的生产计划记录
            var planCodes = lineDtoDict.Select(s => s.PlanCode).Distinct();
            var planEntities = await _planWorkPlanRepository.GetEntitiesAsync(new PlanWorkPlanQuery { SiteId = siteId, Codes = planCodes });

            // 遍历数据
            foreach (var planDto in lineDtoDict)
            {
                // 产品不能为空
                if (planDto.Products.Count == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10249)).WithData("Code", planDto.PlanCode);
                }

                // 不支持一个生产计划多个产品
                if (planDto.Products.Count > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10248)).WithData("Code", planDto.PlanCode);
                }

                // 获取产品对象
                var productDto = planDto.Products.FirstOrDefault();
                if (productDto == null) continue;

                var productEntity = productEntities.FirstOrDefault(f => f.MaterialCode == productDto.ProductCode)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES10245)).WithData("Code", productDto.ProductCode);

                var bomEntity = bomEntities.FirstOrDefault(f => f.BomCode == productDto.BomCode)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES10246)).WithData("Code", productDto.BomCode);

                var planEntity = planEntities.FirstOrDefault(f => f.PlanCode == planDto.PlanCode);

                // 不存在的新生产计划
                if (planEntity == null)
                {
                    planEntity = new PlanWorkPlanEntity
                    {
                        PlanCode = planDto.PlanCode,
                        ProductCode = productDto.ProductCode,
                        ProductVersion = productDto.ProductVersion,
                        BomCode = productDto.BomCode,
                        BomVersion = productDto.BomVersion,
                        Qty = planDto.PlanQty,
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
                        throw new CustomerValidationException(nameof(ErrorCode.MES10247)).WithData("Status", PlanWorkPlanStatusEnum.NotStarted.GetDescription());
                    }

                    // 除了数量/时间，好像什么都不能随便改
                    planEntity.Qty = planDto.PlanQty;
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
