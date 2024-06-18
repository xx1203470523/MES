using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.Query;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Utils;

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
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="planWorkPlanRepository"></param>
        public PlanWorkPlanService(ICurrentUser currentUser, ICurrentSite currentSite,
            IPlanWorkPlanRepository planWorkPlanRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _planWorkPlanRepository = planWorkPlanRepository;
        }


        /// <summary>
        /// 生成子工单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<long> SaveAsync(PlanWorkPlanSaveDto dto)
        {
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
            var workPlans = await _planWorkPlanRepository.GetByIdsAsync(idsArr);
            //foreach (var item in workPlans)
            //{
            //    if (item.Status != PlanWorkPlanStatusEnum.)
            //    {
            //        throw new CustomerValidationException(nameof(ErrorCode.MES16013));
            //    }
            //}

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
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _planWorkPlanRepository.GetPagedInfoAsync(pagedQuery);

            // TODO 这个应该在这里组装，不应该的DB查询全部数据，再直接转（看到了，但没时间改 -。-）

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<PlanWorkPlanDto>());
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



            await Task.CompletedTask;
            return default;
        }

    }
}
