/*
 *creator: Karl
 *
 *describe: 工单激活    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-29 10:23:51
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 工单激活 服务
    /// </summary>
    public class PlanWorkOrderActivationService : IPlanWorkOrderActivationService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 工单激活 仓储
        /// </summary>
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;
        private readonly AbstractValidator<PlanWorkOrderActivationCreateDto> _validationCreateRules;
        private readonly AbstractValidator<PlanWorkOrderActivationModifyDto> _validationModifyRules;

        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        public PlanWorkOrderActivationService(ICurrentUser currentUser, ICurrentSite currentSite, IPlanWorkOrderActivationRepository planWorkOrderActivationRepository, AbstractValidator<PlanWorkOrderActivationCreateDto> validationCreateRules, AbstractValidator<PlanWorkOrderActivationModifyDto> validationModifyRules, IInteWorkCenterRepository inteWorkCenterRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;

            _inteWorkCenterRepository = inteWorkCenterRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="planWorkOrderActivationDto"></param>
        /// <returns></returns>
        public async Task CreatePlanWorkOrderActivationAsync(PlanWorkOrderActivationCreateDto planWorkOrderActivationCreateDto)
        {
            //// 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(planWorkOrderActivationCreateDto);

            //DTO转换实体
            var planWorkOrderActivationEntity = planWorkOrderActivationCreateDto.ToEntity<PlanWorkOrderActivationEntity>();
            planWorkOrderActivationEntity.Id= IdGenProvider.Instance.CreateId();
            planWorkOrderActivationEntity.CreatedBy = _currentUser.UserName;
            planWorkOrderActivationEntity.UpdatedBy = _currentUser.UserName;
            planWorkOrderActivationEntity.CreatedOn = HymsonClock.Now();
            planWorkOrderActivationEntity.UpdatedOn = HymsonClock.Now();
            planWorkOrderActivationEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _planWorkOrderActivationRepository.InsertAsync(planWorkOrderActivationEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePlanWorkOrderActivationAsync(long id)
        {
            await _planWorkOrderActivationRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesPlanWorkOrderActivationAsync(long[] idsArr)
        {
            return await _planWorkOrderActivationRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="planWorkOrderActivationPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkOrderActivationListDetailViewDto>> GetPageListAsync(PlanWorkOrderActivationPagedQueryDto planWorkOrderActivationPagedQueryDto)
        {
            if (!planWorkOrderActivationPagedQueryDto.LineId.HasValue) 
            {
                throw new BusinessException(nameof(ErrorCode.MES16401));
            }

            //查询当前线体
           var line = await _inteWorkCenterRepository.GetByIdAsync(planWorkOrderActivationPagedQueryDto.LineId.Value);
            if (line == null ) 
            {
                throw new BusinessException(nameof(ErrorCode.MES16402));
            }
            if (line.Type!=WorkCenterTypeEnum.Line)
            {
                throw new BusinessException(nameof(ErrorCode.MES16403));
            }

            //查询线体上级车间
            var workCenter = await _inteWorkCenterRepository.GetHigherInteWorkCenterAsync(planWorkOrderActivationPagedQueryDto.LineId??0);

            var planWorkOrderActivationPagedQuery = planWorkOrderActivationPagedQueryDto.ToQuery<PlanWorkOrderActivationPagedQuery>();

            //将对应的工作中心ID放置查询条件中
            planWorkOrderActivationPagedQuery.WorkCenterIds.Add(planWorkOrderActivationPagedQueryDto.LineId??0);
            if (workCenter != null && workCenter.Id>0) 
            {
                planWorkOrderActivationPagedQuery.WorkCenterIds.Add(workCenter.Id);
            }            

            var pagedInfo = await _planWorkOrderActivationRepository.GetPagedInfoAsync(planWorkOrderActivationPagedQuery);

            //实体到DTO转换 装载数据
            List<PlanWorkOrderActivationListDetailViewDto> planWorkOrderActivationDtos = PreparePlanWorkOrderActivationDtos(pagedInfo);
            return new PagedInfo<PlanWorkOrderActivationListDetailViewDto>(planWorkOrderActivationDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<PlanWorkOrderActivationListDetailViewDto> PreparePlanWorkOrderActivationDtos(PagedInfo<PlanWorkOrderActivationListDetailView>   pagedInfo)
        {
            var planWorkOrderActivationDtos = new List<PlanWorkOrderActivationListDetailViewDto>();
            foreach (var planWorkOrderActivation in pagedInfo.Data)
            {
                var planWorkOrderActivationDto = planWorkOrderActivation.ToModel<PlanWorkOrderActivationListDetailViewDto>();
                planWorkOrderActivationDtos.Add(planWorkOrderActivationDto);
            }

            return planWorkOrderActivationDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="planWorkOrderActivationDto"></param>
        /// <returns></returns>
        public async Task ModifyPlanWorkOrderActivationAsync(PlanWorkOrderActivationModifyDto planWorkOrderActivationModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(planWorkOrderActivationModifyDto);

            //DTO转换实体
            var planWorkOrderActivationEntity = planWorkOrderActivationModifyDto.ToEntity<PlanWorkOrderActivationEntity>();
            planWorkOrderActivationEntity.UpdatedBy = _currentUser.UserName;
            planWorkOrderActivationEntity.UpdatedOn = HymsonClock.Now();

            await _planWorkOrderActivationRepository.UpdateAsync(planWorkOrderActivationEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderActivationDto> QueryPlanWorkOrderActivationByIdAsync(long id) 
        {
           var planWorkOrderActivationEntity = await _planWorkOrderActivationRepository.GetByIdAsync(id);
           if (planWorkOrderActivationEntity != null) 
           {
               return planWorkOrderActivationEntity.ToModel<PlanWorkOrderActivationDto>();
           }
            return null;
        }
    }
}
