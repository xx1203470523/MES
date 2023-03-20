/*
 *creator: Karl
 *
 *describe: 工单信息表    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-20 10:07:17
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 工单信息表 服务
    /// </summary>
    public class PlanWorkOrderService : IPlanWorkOrderService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 工单信息表 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly AbstractValidator<PlanWorkOrderCreateDto> _validationCreateRules;
        private readonly AbstractValidator<PlanWorkOrderModifyDto> _validationModifyRules;

        public PlanWorkOrderService(ICurrentUser currentUser, ICurrentSite currentSite, IPlanWorkOrderRepository planWorkOrderRepository, AbstractValidator<PlanWorkOrderCreateDto> validationCreateRules, AbstractValidator<PlanWorkOrderModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _planWorkOrderRepository = planWorkOrderRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="planWorkOrderDto"></param>
        /// <returns></returns>
        public async Task CreatePlanWorkOrderAsync(PlanWorkOrderCreateDto planWorkOrderCreateDto)
        {
            //// 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(planWorkOrderCreateDto);

            //DTO转换实体
            var planWorkOrderEntity = planWorkOrderCreateDto.ToEntity<PlanWorkOrderEntity>();
            planWorkOrderEntity.Id= IdGenProvider.Instance.CreateId();
            planWorkOrderEntity.CreatedBy = _currentUser.UserName;
            planWorkOrderEntity.UpdatedBy = _currentUser.UserName;
            planWorkOrderEntity.CreatedOn = HymsonClock.Now();
            planWorkOrderEntity.UpdatedOn = HymsonClock.Now();
            planWorkOrderEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _planWorkOrderRepository.InsertAsync(planWorkOrderEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePlanWorkOrderAsync(long id)
        {
            await _planWorkOrderRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesPlanWorkOrderAsync(long[] idsArr)
        {
            return await _planWorkOrderRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="planWorkOrderPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkOrderDto>> GetPageListAsync(PlanWorkOrderPagedQueryDto planWorkOrderPagedQueryDto)
        {
            var planWorkOrderPagedQuery = planWorkOrderPagedQueryDto.ToQuery<PlanWorkOrderPagedQuery>();
            var pagedInfo = await _planWorkOrderRepository.GetPagedInfoAsync(planWorkOrderPagedQuery);

            //实体到DTO转换 装载数据
            List<PlanWorkOrderDto> planWorkOrderDtos = PreparePlanWorkOrderDtos(pagedInfo);
            return new PagedInfo<PlanWorkOrderDto>(planWorkOrderDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<PlanWorkOrderDto> PreparePlanWorkOrderDtos(PagedInfo<PlanWorkOrderEntity>   pagedInfo)
        {
            var planWorkOrderDtos = new List<PlanWorkOrderDto>();
            foreach (var planWorkOrderEntity in pagedInfo.Data)
            {
                var planWorkOrderDto = planWorkOrderEntity.ToModel<PlanWorkOrderDto>();
                planWorkOrderDtos.Add(planWorkOrderDto);
            }

            return planWorkOrderDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="planWorkOrderDto"></param>
        /// <returns></returns>
        public async Task ModifyPlanWorkOrderAsync(PlanWorkOrderModifyDto planWorkOrderModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(planWorkOrderModifyDto);

            //DTO转换实体
            var planWorkOrderEntity = planWorkOrderModifyDto.ToEntity<PlanWorkOrderEntity>();
            planWorkOrderEntity.UpdatedBy = _currentUser.UserName;
            planWorkOrderEntity.UpdatedOn = HymsonClock.Now();

            await _planWorkOrderRepository.UpdateAsync(planWorkOrderEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderDto> QueryPlanWorkOrderByIdAsync(long id) 
        {
           var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(id);
           if (planWorkOrderEntity != null) 
           {
               return planWorkOrderEntity.ToModel<PlanWorkOrderDto>();
           }
            return null;
        }
    }
}
