/*
 *creator: Karl
 *
 *describe: 条码接收    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Snowflake;
using Hymson.Utils;
//using Hymson.Utils.Extensions;
using System.Transactions;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 条码接收 服务
    /// </summary>
    public class PlanSfcInfoService : IPlanSfcInfoService
    {
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 条码接收 仓储
        /// </summary>
        private readonly IPlanSfcInfoRepository _planSfcInfoRepository;
        private readonly AbstractValidator<PlanSfcInfoCreateDto> _validationCreateRules;
        private readonly AbstractValidator<PlanSfcInfoModifyDto> _validationModifyRules;

        public PlanSfcInfoService(ICurrentUser currentUser, IPlanSfcInfoRepository planSfcInfoRepository, AbstractValidator<PlanSfcInfoCreateDto> validationCreateRules, AbstractValidator<PlanSfcInfoModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _planSfcInfoRepository = planSfcInfoRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="planSfcInfoDto"></param>
        /// <returns></returns>
        public async Task CreatePlanSfcInfoAsync(PlanSfcInfoCreateDto planSfcInfoCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(planSfcInfoCreateDto);

            //DTO转换实体
            var planSfcInfoEntity = planSfcInfoCreateDto.ToEntity<PlanSfcInfoView>();
            planSfcInfoEntity.Id = IdGenProvider.Instance.CreateId();
            planSfcInfoEntity.CreatedBy = _currentUser.UserName;
            planSfcInfoEntity.UpdatedBy = _currentUser.UserName;
            planSfcInfoEntity.CreatedOn = HymsonClock.Now();
            planSfcInfoEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _planSfcInfoRepository.InsertAsync(planSfcInfoEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePlanSfcInfoAsync(long id)
        {
            await _planSfcInfoRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesPlanSfcInfoAsync(string ids)
        {
            var idsArr = StringExtension.SpitLongArrary(ids);
            return await _planSfcInfoRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="planSfcInfoPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanSfcInfoDto>> GetPageListAsync(PlanSfcInfoPagedQueryDto planSfcInfoPagedQueryDto)
        {
            var planSfcInfoPagedQuery = planSfcInfoPagedQueryDto.ToQuery<PlanSfcInfoPagedQuery>();
            var pagedInfo = await _planSfcInfoRepository.GetPagedInfoAsync(planSfcInfoPagedQuery);

            //实体到DTO转换 装载数据
            List<PlanSfcInfoDto> planSfcInfoDtos = PreparePlanSfcInfoDtos(pagedInfo);
            return new PagedInfo<PlanSfcInfoDto>(planSfcInfoDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<PlanSfcInfoDto> PreparePlanSfcInfoDtos(PagedInfo<PlanSfcInfoView> pagedInfo)
        {
            var planSfcInfoDtos = new List<PlanSfcInfoDto>();
            foreach (var planSfcInfoEntity in pagedInfo.Data)
            {
                var planSfcInfoDto = planSfcInfoEntity.ToModel<PlanSfcInfoDto>();
                planSfcInfoDtos.Add(planSfcInfoDto);
            }

            return planSfcInfoDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="planSfcInfoDto"></param>
        /// <returns></returns>
        public async Task ModifyPlanSfcInfoAsync(PlanSfcInfoModifyDto planSfcInfoModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(planSfcInfoModifyDto);

            //DTO转换实体
            var planSfcInfoEntity = planSfcInfoModifyDto.ToEntity<PlanSfcInfoView>();
            planSfcInfoEntity.UpdatedBy = _currentUser.UserName;
            planSfcInfoEntity.UpdatedOn = HymsonClock.Now();

            await _planSfcInfoRepository.UpdateAsync(planSfcInfoEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanSfcInfoDto> QueryPlanSfcInfoByIdAsync(long id)
        {
            var planSfcInfoEntity = await _planSfcInfoRepository.GetByIdAsync(id);
            if (planSfcInfoEntity != null)
            {
                return planSfcInfoEntity.ToModel<PlanSfcInfoDto>();
            }
            return null;
        }
    }
}
