using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Command;
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
        /// 仓储接口（生产计划产品）
        /// </summary>
        private readonly IPlanWorkPlanProductRepository _planWorkPlanProductRepository;

        /// <summary>
        /// 仓储接口（生产计划物料）
        /// </summary>
        private readonly IPlanWorkPlanMaterialRepository _planWorkPlanMaterialRepository;

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
        /// <param name="planWorkPlanProductRepository"></param>
        /// <param name="planWorkPlanMaterialRepository"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="procMaterialRepository"></param>
        public PlanWorkPlanService(ILogger<PlanWorkPlanService> logger,
            ISysConfigRepository sysConfigRepository,
            IPlanWorkPlanRepository planWorkPlanRepository,
            IPlanWorkPlanProductRepository planWorkPlanProductRepository,
            IPlanWorkPlanMaterialRepository planWorkPlanMaterialRepository,
            IProcBomRepository procBomRepository,
            IProcMaterialRepository procMaterialRepository)
        {
            _logger = logger;
            _sysConfigRepository = sysConfigRepository;
            _planWorkPlanRepository = planWorkPlanRepository;
            _planWorkPlanProductRepository = planWorkPlanProductRepository;
            _planWorkPlanMaterialRepository = planWorkPlanMaterialRepository;
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

            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10139));

            // 当前对象
            var currentBo = new BaseBo
            {
                User = "ERP",
                Time = HymsonClock.Now()
            };

            var resposeBo = await ConvertWorkPlanListAsync(configEntities.FirstOrDefault(), currentBo, requestDtos)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 添加到集合
            var resposeSummaryBo = new SyncWorkPlanSummaryBo();
            resposeSummaryBo.Adds.AddRange(resposeBo.Adds);
            resposeSummaryBo.Updates.AddRange(resposeBo.Updates);
            resposeSummaryBo.ProductAdds.AddRange(resposeBo.ProductAdds);
            resposeSummaryBo.MaterialAdds.AddRange(resposeBo.MaterialAdds);

            var workPlanIds = resposeBo.Adds.Select(s => s.Id);
            workPlanIds = workPlanIds.Concat(resposeBo.Updates.Select(s => s.Id));

            // 删除数据
            var command = new DeleteByParentIdsCommand
            {
                ParentIds = workPlanIds,
                UpdatedBy = currentBo.User,
                UpdatedOn = currentBo.Time
            };

            // 插入数据
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _planWorkPlanRepository.InsertsAsync(resposeSummaryBo.Adds);
            rows += await _planWorkPlanRepository.UpdatesAsync(resposeSummaryBo.Updates);

            rows += await _planWorkPlanProductRepository.DeleteByParentIdsAsync(command);
            rows += await _planWorkPlanMaterialRepository.DeleteByParentIdsAsync(command);

            rows += await _planWorkPlanProductRepository.InsertsAsync(resposeSummaryBo.ProductAdds);
            rows += await _planWorkPlanMaterialRepository.InsertsAsync(resposeSummaryBo.MaterialAdds);

            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 取消（生产计划）
        /// </summary>
        /// <param name="WorkPlanCodes"></param>
        /// <returns></returns>
        public async Task<int> CancelWorkPlanAsync(IEnumerable<string> WorkPlanCodes)
        {
            if (WorkPlanCodes == null || !WorkPlanCodes.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
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
                Codes = WorkPlanCodes
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
        /// <param name="currentBo"></param>
        /// <param name="workPlanDtos"></param>
        /// <returns></returns>
        private async Task<SyncWorkPlanSummaryBo?> ConvertWorkPlanListAsync(SysConfigEntity? configEntity, BaseBo currentBo, IEnumerable<SyncWorkPlanDto> workPlanDtos)
        {
            // 判断是否存在（配置）
            if (configEntity == null) throw new CustomerValidationException(nameof(ErrorCode.MES10139));

            // 初始化
            currentBo.SiteId = configEntity.Value.ParseToLong();

            var resposeBo = new SyncWorkPlanSummaryBo();

            // 判断是否有不存在的产品编码
            var productCodes = workPlanDtos.SelectMany(s => s.Products).Select(s => s.ProductCode).Distinct();
            productCodes = productCodes.Concat(workPlanDtos.SelectMany(s => s.Products).SelectMany(s => s.Materials).Select(s => s.MaterialCode).Distinct());
            var productEntities = await _procMaterialRepository.GetByCodesAsync(new ProcMaterialsByCodeQuery { SiteId = currentBo.SiteId, MaterialCodes = productCodes });

            /*
            if (productEntities == null || !productEntities.Any())
            {
                // 这里应该提示产品不存在
                throw new CustomerValidationException(nameof(ErrorCode.MES10251)).WithData("Code", string.Join(',', productCodes));
            }
            */

            // 读取已存在的BOM记录（因为U8会出现重复编码）
            /*
            var bomCodes = workPlanDtos.SelectMany(s => s.Products).Select(s => s.BomCode).Distinct();
            var bomEntities = await _procBomRepository.GetByCodesAsync(new ProcBomsByCodeQuery { SiteId = currentBo.SiteId, Codes = bomCodes });
            */
            var bomIds = workPlanDtos.SelectMany(s => s.Products).Select(s => s.BomId).Distinct();
            var bomEntities = await _procBomRepository.GetByIdsAsync(bomIds);
            if (bomEntities == null || !bomEntities.Any())
            {
                // 这里应该提示BOM不存在
                throw new CustomerValidationException(nameof(ErrorCode.MES10252));
            }

            // 读取已存在的生产计划记录
            var workPlanCodes = workPlanDtos.Select(s => s.PlanCode).Distinct();
            var planEntities = await _planWorkPlanRepository.GetEntitiesAsync(new PlanWorkPlanQuery { SiteId = currentBo.SiteId, Codes = workPlanCodes });

            // 遍历数据
            foreach (var planDto in workPlanDtos)
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
                var firstProductDto = planDto.Products.FirstOrDefault();
                if (firstProductDto == null) continue;

                /*
                var bomEntity = bomEntities.FirstOrDefault(f => f.BomCode == productDto.BomCode)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES10246)).WithData("Code", productDto.BomCode);
                */

                var planEntity = planEntities.FirstOrDefault(f => f.WorkPlanCode == planDto.PlanCode);

                // 不存在的新生产计划
                if (planEntity == null)
                {
                    planEntity = new PlanWorkPlanEntity
                    {
                        WorkPlanCode = planDto.PlanCode,
                        RequirementNumber = planDto.RequirementNumber,
                        PlanStartTime = firstProductDto.StartTime ?? SqlDateTime.MinValue.Value,
                        PlanEndTime = firstProductDto.EndTime ?? SqlDateTime.MaxValue.Value,

                        // TODO: 这里的字段需要确认
                        OverScale = 0,
                        Type = planDto.Type,
                        Status = PlanWorkPlanStatusEnum.NotStarted,

                        Remark = "",
                        Id = planDto.Id ?? IdGenProvider.Instance.CreateId(),
                        SiteId = currentBo.SiteId,
                        CreatedBy = currentBo.User,
                        CreatedOn = currentBo.Time,
                        UpdatedBy = currentBo.User,
                        UpdatedOn = currentBo.Time
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
                    planEntity.PlanStartTime = firstProductDto.StartTime ?? SqlDateTime.MinValue.Value;
                    planEntity.PlanEndTime = firstProductDto.EndTime ?? SqlDateTime.MaxValue.Value;

                    planEntity.Type = planDto.Type;
                    planEntity.UpdatedBy = currentBo.User;
                    planEntity.UpdatedOn = currentBo.Time;
                    resposeBo.Updates.Add(planEntity);
                }

                // 遍历产品列表
                foreach (var productDto in planDto.Products)
                {
                    // 读取产品实体
                    var productEntity = productEntities.FirstOrDefault(f => f.MaterialCode == productDto.ProductCode)
                        ?? throw new CustomerValidationException(nameof(ErrorCode.MES10245)).WithData("Code", productDto.ProductCode);

                    // 添加生产计划产品
                    var workPlanProductId = productDto.Id ?? IdGenProvider.Instance.CreateId();
                    resposeBo.ProductAdds.Add(new PlanWorkPlanProductEntity
                    {
                        WorkPlanId = planEntity.Id,
                        ProductId = productEntity.Id,
                        ProductCode = productDto.ProductCode,
                        ProductVersion = productEntity.Version ?? "",
                        BomId = productDto.BomId,
                        BomCode = productDto.BomCode,
                        BomVersion = productDto.BomVersion,
                        Qty = productDto.Qty,
                        OverScale = productDto.OverScale,

                        Remark = "",
                        Id = workPlanProductId,
                        SiteId = currentBo.SiteId,
                        CreatedBy = currentBo.User,
                        CreatedOn = currentBo.Time,
                        UpdatedBy = currentBo.User,
                        UpdatedOn = currentBo.Time
                    });

                    // 添加生产计划物料
                    foreach (var materialDto in productDto.Materials)
                    {
                        // 读取物料实体
                        var materialEntity = productEntities.FirstOrDefault(f => f.MaterialCode == materialDto.MaterialCode)
                            ?? throw new CustomerValidationException(nameof(ErrorCode.MES10251)).WithData("Code", materialDto.MaterialCode);

                        resposeBo.MaterialAdds.Add(new PlanWorkPlanMaterialEntity
                        {
                            WorkPlanId = planEntity.Id,
                            WorkPlanProductId = workPlanProductId,
                            MaterialId = materialEntity.Id,
                            MaterialCode = materialEntity.MaterialCode,
                            MaterialVersion = materialEntity.Version ?? "",
                            BomId = materialDto.BomId,
                            Usages = materialDto.MaterialDosage,
                            Loss = materialDto.MaterialLoss,

                            Remark = "",
                            Id = materialDto.Id ?? IdGenProvider.Instance.CreateId(),
                            SiteId = currentBo.SiteId,
                            CreatedBy = currentBo.User,
                            CreatedOn = currentBo.Time,
                            UpdatedBy = currentBo.User,
                            UpdatedOn = currentBo.Time
                        });
                    }
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
        /// <summary>
        /// 新增（工作计划产品）
        /// </summary>
        public List<PlanWorkPlanProductEntity> ProductAdds { get; set; } = new();
        /*
        /// <summary>
        /// 新增（工作计划产品）
        /// </summary>
        public List<PlanWorkPlanProductEntity> ProductUpdates { get; set; } = new();
        */
        /// <summary>
        /// 新增（工作计划物料）
        /// </summary>
        public List<PlanWorkPlanMaterialEntity> MaterialAdds { get; set; } = new();
        /*
        /// <summary>
        /// 新增（工作计划物料）
        /// </summary>
        public List<PlanWorkPlanMaterialEntity> MaterialUpdates { get; set; } = new();
        */

    }
}
