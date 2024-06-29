using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 服务（生产计划）
    /// </summary>
    public class PlanWorkPlanService : IPlanWorkPlanService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

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
        /// 仓储接口（生产工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（BOM）
        /// </summary>
        private readonly IProcBomRepository _procBomRepository;

        /// <summary>
        /// 仓储接口（物料）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="planWorkPlanRepository"></param>
        /// <param name="planWorkPlanProductRepository"></param>
        /// <param name="planWorkPlanMaterialRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="procMaterialRepository"></param>
        public PlanWorkPlanService(ICurrentUser currentUser, ICurrentSite currentSite,
            IPlanWorkPlanRepository planWorkPlanRepository,
            IPlanWorkPlanProductRepository planWorkPlanProductRepository,
            IPlanWorkPlanMaterialRepository planWorkPlanMaterialRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcBomRepository procBomRepository,
            IProcMaterialRepository procMaterialRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _planWorkPlanRepository = planWorkPlanRepository;
            _planWorkPlanProductRepository = planWorkPlanProductRepository;
            _planWorkPlanMaterialRepository = planWorkPlanMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procBomRepository = procBomRepository;
            _procMaterialRepository = procMaterialRepository;
        }


        /// <summary>
        /// 生成子工单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> SaveAsync(PlanWorkPlanSaveDto dto)
        {
            // 检查生产计划是否存在
            var workPlanProductEntity = await _planWorkPlanProductRepository.GetByIdAsync(dto.WorkPlanProductId);
            if (workPlanProductEntity == null) throw new CustomerValidationException(nameof(ErrorCode.MES16018));

            // 检查生产计划是否存在
            var workPlanEntity = await _planWorkPlanRepository.GetByIdAsync(workPlanProductEntity.WorkPlanId);
            if (workPlanEntity == null) throw new CustomerValidationException(nameof(ErrorCode.MES16018));

            // 检查生产计划的状态
            if (workPlanEntity.Status != PlanWorkPlanStatusEnum.NotStarted)
            {
                // TODO: 生产计划状态不正确
                throw new CustomerValidationException(nameof(ErrorCode.MES16017));
            }

            if (dto.Details == null || !dto.Details.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES16019));

            // 检查子工单的工单号是否有重复
            var workOrderCodes = dto.Details.Select(s => s.WorkOrderCode);
            if (workOrderCodes.Count() != workOrderCodes.Distinct().Count())
            {
                // TODO: 工单号存在重复

                throw new CustomerValidationException(nameof(ErrorCode.MES16017));
            }

            // 查看数据库是否存在相同的工单号
            var workOrderEntities = await _planWorkOrderRepository.GetEntitiesAsync(new PlanWorkOrderNewQuery
            {
                SiteId = workPlanEntity.SiteId,
                Codes = workOrderCodes,
            });

            /*
            // 检查子工单的总数量是否等于计划数量
            var sumQuantity = dto.Details.Sum(s => s.Qty);
            if (sumQuantity != workPlanEntity.Qty)
            {
                // TODO: 子工单的总数量不等于计划数量
                throw new CustomerValidationException(nameof(ErrorCode.MES16017));
            }
            */

            // 检查子工单的计划时间是否超出生产计划的时间范围
            if (dto.Details.Any(a => a.PlanStartTime < workPlanEntity.PlanStartTime || a.PlanStartTime > workPlanEntity.PlanEndTime))
            {
                // TODO: 子工单的计划时间超出生产计划的时间范围
                throw new CustomerValidationException(nameof(ErrorCode.MES16017));
            }

            // 当前对象
            var currentBo = new BaseBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                User = "ERP",
                Time = HymsonClock.Now()
            };

            // 修改生产计划的状态
            workPlanEntity.Status = PlanWorkPlanStatusEnum.Distributed;
            workPlanEntity.UpdatedBy = currentBo.User;
            workPlanEntity.UpdatedOn = currentBo.Time;

            List<PlanWorkOrderEntity> workOrderEntites = new();
            workOrderEntites.AddRange(dto.Details.Select(s => new PlanWorkOrderEntity
            {
                OrderCode = s.WorkOrderCode,
                WorkCenterType = s.WorkCenterType,
                WorkCenterId = s.WorkCenterId,
                PlanStartTime = s.PlanStartTime,
                PlanEndTime = s.PlanEndTime,
                Qty = s.Qty,

                ProductId = workPlanProductEntity.ProductId,
                WorkPlanId = workPlanEntity.Id,
                ProcessRouteId = 0,
                ProductBOMId = 0,
                Type = workPlanEntity.Type,
                OverScale = workPlanProductEntity.OverScale,
                Status = PlanWorkOrderStatusEnum.NotStarted,

                Id = IdGenProvider.Instance.CreateId(),
                SiteId = currentBo.SiteId,
                CreatedBy = currentBo.User,
                CreatedOn = currentBo.Time,
                UpdatedBy = currentBo.User,
                UpdatedOn = currentBo.Time
            }));

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _planWorkPlanRepository.UpdateAsync(workPlanEntity);
            rows += await _planWorkOrderRepository.InsertsAsync(workOrderEntites);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            // 检查工单状态
            var workPlanEntities = await _planWorkPlanRepository.GetByIdsAsync(idsArr);
            if (workPlanEntities.Any(a => a.Status != PlanWorkPlanStatusEnum.NotStarted))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16017)).WithData("Status", PlanWorkPlanStatusEnum.NotStarted.GetDescription());
            }

            return await _planWorkPlanRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkPlanProductDto>> GetPageListAsync(PlanWorkPlanProductPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<PlanWorkPlanProductPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _planWorkPlanProductRepository.GetPagedInfoAsync(pagedQuery);

            // 读取生产计划
            var planEntities = await _planWorkPlanRepository.GetByIdsAsync(pagedInfo.Data.Select(s => s.WorkPlanId));

            List<PlanWorkPlanProductDto> dtos = new();
            foreach (var dataItem in pagedInfo.Data)
            {
                var dto = dataItem.ToModel<PlanWorkPlanProductDto>();
                if (dto == null) continue;

                // 填充生产计划
                var planEntity = planEntities.FirstOrDefault(f => f.Id == dto.BomId);
                if (planEntity != null)
                {
                    dto.WorkPlanCode = planEntity.WorkPlanCode;
                    dto.PlanStartTime = planEntity.PlanStartTime;
                    dto.PlanEndTime = planEntity.PlanEndTime;
                }

                dtos.Add(dto);
            }

            return new PagedInfo<PlanWorkPlanProductDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据planProductId查询
        /// </summary>
        /// <param name="planProductId"></param>
        /// <returns></returns>
        public async Task<PlanWorkPlanProductDto?> QueryByIdAsync(long planProductId)
        {
            var planProductEntity = await _planWorkPlanProductRepository.GetByIdAsync(planProductId);
            if (planProductEntity == null) return default;

            var dto = planProductEntity.ToModel<PlanWorkPlanProductDto>();

            // 填充生产计划
            var planEntity = await _planWorkPlanRepository.GetByIdAsync(planProductEntity.WorkPlanId);
            if (planEntity != null)
            {
                dto.WorkPlanCode = planEntity.WorkPlanCode;
                dto.PlanStartTime = planEntity.PlanStartTime;
                dto.PlanEndTime = planEntity.PlanEndTime;
            }

            return dto;
        }

        // TODO: 读取生产计划已经下发的子工单


        // TODO: 读取生产计划的物料
        /// <summary>
        /// 根据planProductId查询
        /// </summary>
        /// <param name="planProductId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkPlanMaterialDto>?> QueryMaterialsByMainIdAsync(long planProductId)
        {
            var planProductEntity = await _planWorkPlanProductRepository.GetByIdAsync(planProductId);
            if (planProductEntity == null) return default;

            // TODO: 读取生产计划的物料

            return default;
        }

    }
}
