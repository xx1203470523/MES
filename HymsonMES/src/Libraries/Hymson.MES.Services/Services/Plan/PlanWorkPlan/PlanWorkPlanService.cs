using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Utils;
using Minio.DataModel;

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
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="procMaterialRepository"></param>
        public PlanWorkPlanService(ICurrentUser currentUser, ICurrentSite currentSite,
            IPlanWorkPlanRepository planWorkPlanRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcBomRepository procBomRepository,
            IProcMaterialRepository procMaterialRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _planWorkPlanRepository = planWorkPlanRepository;
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
            var workPlanEntity = await _planWorkPlanRepository.GetByIdAsync(dto.WorkPlanId);
            if (workPlanEntity == null) return 0;

            // 检查生产计划的状态
            if (workPlanEntity.Status != PlanWorkPlanStatusEnum.NotStarted)
            {
                // TODO: 生产计划状态不正确
            }

            if (dto.Details == null || !dto.Details.Any()) return 0;

            // 检查子工单的工单号是否有重复
            var workOrderCodes = dto.Details.Select(s => s.OrderCode);
            if (workOrderCodes.Count() != workOrderCodes.Distinct().Count())
            {
                // TODO: 工单号存在重复
            }

            // 查看数据库是否存在相同的工单号
            var workOrderEntities = await _planWorkOrderRepository.GetEntitiesAsync(new PlanWorkOrderNewQuery
            {
                SiteId = workPlanEntity.SiteId,
                Codes = workOrderCodes,
            });

            // 检查子工单的总数量是否等于计划数量
            var sumQuantity = dto.Details.Sum(s => s.Qty);
            if (sumQuantity != workPlanEntity.Qty)
            {
                // TODO: 子工单的总数量不等于计划数量
            }

            // 检查子工单的计划时间是否超出生产计划的时间范围
            if (dto.Details.Any(a => a.PlanStartTime < workPlanEntity.PlanStartTime || a.PlanStartTime > workPlanEntity.PlanEndTime))
            {
                // TODO: 子工单的计划时间超出生产计划的时间范围
            }

            await Task.CompletedTask;
            return 0;
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
                // TODO: 存在状态不为“未开始”的工单
                return 0;
            }

            return await _planWorkPlanRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkPlanDto>> GetPageListAsync(PlanWorkPlanPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<PlanWorkPlanPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _planWorkPlanRepository.GetPagedInfoAsync(pagedQuery);

            // 读取BOM
            var bomIds = pagedInfo.Data.Select(s => s.BomId).Distinct();
            var bomEntities = await _procBomRepository.GetEntitiesAsync(new ProcBomQuery
            {
                SiteId = pagedQuery.SiteId,
                BomIds = bomIds
            });

            // 读取物料
            var materialIds = pagedInfo.Data.Select(s => s.ProductId).Distinct();
            var materialEntities = await _procMaterialRepository.GetEntitiesAsync(new ProcMaterialQuery
            {
                SiteId = pagedQuery.SiteId,
                MaterialIds = materialIds
            });

            List<PlanWorkPlanDto> dtos = new();
            foreach (var dataItem in pagedInfo.Data)
            {
                var dto = dataItem.ToModel<PlanWorkPlanDto>();
                if (dto == null) continue;

                // 填充BOM
                var bomEntity = bomEntities.FirstOrDefault(f => f.Id == dto.BomId);
                if (bomEntity != null)
                {
                    dto.BomCode = bomEntity.BomCode;
                    dto.BomName = bomEntity.BomName;
                }

                // 填充物料
                var materialEntity = materialEntities.FirstOrDefault(f => f.Id == dto.ProductId);
                if (materialEntity != null)
                {
                    dto.ProductCode = materialEntity.MaterialCode;
                    dto.ProductName = materialEntity.MaterialName;
                }

                dtos.Add(dto);
            }

            return new PagedInfo<PlanWorkPlanDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanWorkPlanDto?> QueryByIdAsync(long id)
        {
            var entity = await _planWorkPlanRepository.GetByIdAsync(id);
            if (entity == null) return default;

            var dto = entity.ToModel<PlanWorkPlanDto>();

            // 读取BOM
            var bomEntity = await _procBomRepository.GetByIdAsync(entity.BomId);
            if (bomEntity != null)
            {
                dto.BomCode = bomEntity.BomCode;
                dto.BomName = bomEntity.BomName;
            }

            // 读取物料
            var materialEntity = await _procMaterialRepository.GetByIdAsync(entity.ProductId);
            if (materialEntity != null)
            {
                dto.ProductCode = materialEntity.MaterialCode;
                dto.ProductName = materialEntity.MaterialName;
            }

            return dto;
        }

    }
}
