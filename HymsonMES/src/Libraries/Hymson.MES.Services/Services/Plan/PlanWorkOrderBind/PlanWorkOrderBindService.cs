/*
 *creator: Karl
 *
 *describe: 工单激活（物理删除）    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-12 11:14:23
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 工单激活（物理删除） 服务
    /// </summary>
    public class PlanWorkOrderBindService : IPlanWorkOrderBindService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 工单激活（物理删除） 仓储
        /// </summary>
        private readonly IPlanWorkOrderBindRepository _planWorkOrderBindRepository;
        private readonly AbstractValidator<PlanWorkOrderBindCreateDto> _validationCreateRules;
        private readonly AbstractValidator<PlanWorkOrderBindModifyDto> _validationModifyRules;

        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;
        private readonly IPlanWorkOrderBindRecordRepository _planWorkOrderBindRecordRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        public PlanWorkOrderBindService(ICurrentUser currentUser, ICurrentSite currentSite, IPlanWorkOrderBindRepository planWorkOrderBindRepository, AbstractValidator<PlanWorkOrderBindCreateDto> validationCreateRules, AbstractValidator<PlanWorkOrderBindModifyDto> validationModifyRules,
            IInteWorkCenterRepository inteWorkCenterRepository, IPlanWorkOrderActivationRepository planWorkOrderActivationRepository, IPlanWorkOrderBindRecordRepository planWorkOrderBindRecordRepository, IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _planWorkOrderBindRepository = planWorkOrderBindRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;

            _inteWorkCenterRepository = inteWorkCenterRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _planWorkOrderBindRecordRepository = planWorkOrderBindRecordRepository;

            _planWorkOrderRepository = planWorkOrderRepository;
        }
        #region 框架自带
        ///// <summary>
        ///// 创建
        ///// </summary>
        ///// <param name="planWorkOrderBindCreateDto"></param>
        ///// <returns></returns>
        //public async Task CreatePlanWorkOrderBindAsync(PlanWorkOrderBindCreateDto planWorkOrderBindCreateDto)
        //{
        //    // 判断是否有获取到站点码 
        //    if (_currentSite.SiteId == 0)
        //    {
        //        throw new ValidationException(nameof(ErrorCode.MES10101));
        //    }

        //    //验证DTO
        //    await _validationCreateRules.ValidateAndThrowAsync(planWorkOrderBindCreateDto);

        //    //DTO转换实体
        //    var planWorkOrderBindEntity = planWorkOrderBindCreateDto.ToEntity<PlanWorkOrderBindEntity>();
        //    planWorkOrderBindEntity.Id= IdGenProvider.Instance.CreateId();
        //    planWorkOrderBindEntity.CreatedBy = _currentUser.UserName;
        //    planWorkOrderBindEntity.UpdatedBy = _currentUser.UserName;
        //    planWorkOrderBindEntity.CreatedOn = HymsonClock.Now();
        //    planWorkOrderBindEntity.UpdatedOn = HymsonClock.Now();
        //    planWorkOrderBindEntity.SiteId = _currentSite.SiteId ?? 123456;

        //    //入库
        //    await _planWorkOrderBindRepository.InsertAsync(planWorkOrderBindEntity);
        //}

        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public async Task DeletePlanWorkOrderBindAsync(long id)
        //{
        //    await _planWorkOrderBindRepository.DeleteAsync(id);
        //}

        ///// <summary>
        ///// 批量删除
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //public async Task<int> DeletesPlanWorkOrderBindAsync(long[] ids)
        //{
        //    return await _planWorkOrderBindRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        //}

        ///// <summary>
        ///// 根据查询条件获取分页数据
        ///// </summary>
        ///// <param name="planWorkOrderBindPagedQueryDto"></param>
        ///// <returns></returns>
        //public async Task<PagedInfo<PlanWorkOrderBindDto>> GetPagedListAsync(PlanWorkOrderBindPagedQueryDto planWorkOrderBindPagedQueryDto)
        //{
        //    var planWorkOrderBindPagedQuery = planWorkOrderBindPagedQueryDto.ToQuery<PlanWorkOrderBindPagedQuery>();
        //    var pagedInfo = await _planWorkOrderBindRepository.GetPagedInfoAsync(planWorkOrderBindPagedQuery);

        //    //实体到DTO转换 装载数据
        //    List<PlanWorkOrderBindDto> planWorkOrderBindDtos = PreparePlanWorkOrderBindDtos(pagedInfo);
        //    return new PagedInfo<PlanWorkOrderBindDto>(planWorkOrderBindDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="pagedInfo"></param>
        ///// <returns></returns>
        //private static List<PlanWorkOrderBindDto> PreparePlanWorkOrderBindDtos(PagedInfo<PlanWorkOrderBindEntity>   pagedInfo)
        //{
        //    var planWorkOrderBindDtos = new List<PlanWorkOrderBindDto>();
        //    foreach (var planWorkOrderBindEntity in pagedInfo.Data)
        //    {
        //        var planWorkOrderBindDto = planWorkOrderBindEntity.ToModel<PlanWorkOrderBindDto>();
        //        planWorkOrderBindDtos.Add(planWorkOrderBindDto);
        //    }

        //    return planWorkOrderBindDtos;
        //}

        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <param name="planWorkOrderBindDto"></param>
        ///// <returns></returns>
        //public async Task ModifyPlanWorkOrderBindAsync(PlanWorkOrderBindModifyDto planWorkOrderBindModifyDto)
        //{
        //     // 判断是否有获取到站点码 
        //    if (_currentSite.SiteId == 0)
        //    {
        //        throw new ValidationException(nameof(ErrorCode.MES10101));
        //    }

        //     //验证DTO
        //    await _validationModifyRules.ValidateAndThrowAsync(planWorkOrderBindModifyDto);

        //    //DTO转换实体
        //    var planWorkOrderBindEntity = planWorkOrderBindModifyDto.ToEntity<PlanWorkOrderBindEntity>();
        //    planWorkOrderBindEntity.UpdatedBy = _currentUser.UserName;
        //    planWorkOrderBindEntity.UpdatedOn = HymsonClock.Now();

        //    await _planWorkOrderBindRepository.UpdateAsync(planWorkOrderBindEntity);
        //}

        ///// <summary>
        ///// 根据ID查询
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public async Task<PlanWorkOrderBindDto> QueryPlanWorkOrderBindByIdAsync(long id) 
        //{
        //   var planWorkOrderBindEntity = await _planWorkOrderBindRepository.GetByIdAsync(id);
        //   if (planWorkOrderBindEntity != null) 
        //   {
        //       return planWorkOrderBindEntity.ToModel<PlanWorkOrderBindDto>();
        //   }
        //    return null;
        //}
        #endregion

        /// <summary>
        /// 批量绑定激活的工单
        /// </summary>
        /// <param name="bindActivationWorkOrder"></param>
        /// <returns></returns>
        public async Task BindActivationWorkOrderAsync(BindActivationWorkOrderDto bindActivationWorkOrder)
        {
            //检查当前这些工单 是否属于当前资源对应的线体
            var workCenterEntity = await _inteWorkCenterRepository.GetByResourceIdAsync(bindActivationWorkOrder.ResourceId);
            if (workCenterEntity == null) 
            {
                throw new BusinessException(nameof(ErrorCode.MES16803));
            }

            if (workCenterEntity.Type != WorkCenterTypeEnum.Line) 
            {
                throw new BusinessException(nameof(ErrorCode.MES16801));
            }

            if (bindActivationWorkOrder.WorkOrderIds != null && bindActivationWorkOrder.WorkOrderIds.Any())
            {
                //检查当前工单是否重复
                if (bindActivationWorkOrder.WorkOrderIds.Distinct().Count() != bindActivationWorkOrder.WorkOrderIds.Count())
                {
                    throw new BusinessException(nameof(ErrorCode.MES16804));
                }

                //检查当前这些工单是否是激活
                var hasActivationWorkOrders = await _planWorkOrderActivationRepository.GetPlanWorkOrderActivationEntitiesAsync(new PlanWorkOrderActivationQuery()
                {
                    SiteId = _currentSite.SiteId ?? 123456,
                    LineId = workCenterEntity.Id,
                    WorkOrderIds = bindActivationWorkOrder.WorkOrderIds,
                });

                if (hasActivationWorkOrders.Count() != bindActivationWorkOrder.WorkOrderIds.Count)
                {
                    throw new BusinessException(nameof(ErrorCode.MES16802));
                }
            }
            else 
            {
                
            }

            //查询已经绑定在该资源上的工单
            var hasBindWorkOrders= await _planWorkOrderBindRepository.GetPlanWorkOrderBindEntitiesAsync(new PlanWorkOrderBindQuery() {
                SiteId = _currentSite.SiteId ?? 123456,
                ResourceId= bindActivationWorkOrder.ResourceId
            });
            var hasBindWorkOrderIds = hasBindWorkOrders.Select(x => x.WorkOrderId).ToList();

            List< PlanWorkOrderBindEntity > addPlanWorkOrderBindEntities = new List< PlanWorkOrderBindEntity >();
            List<PlanWorkOrderBindEntity> deletePlanWorkOrderBindEntities = new List<PlanWorkOrderBindEntity>();

            List<PlanWorkOrderBindRecordEntity> bindRecordEntities = new List<PlanWorkOrderBindRecordEntity>();

            ////找到不需要操作的
            //hasBindWorkOrderIds.Intersect(bindActivationWorkOrder.WorkOrderIds);
            //找到需要删除的
            var needDeletes= bindActivationWorkOrder.WorkOrderIds!=null&& bindActivationWorkOrder.WorkOrderIds.Any()?  hasBindWorkOrderIds.Except(bindActivationWorkOrder.WorkOrderIds): hasBindWorkOrderIds;
            //找到需要新增的
            var needAdds = bindActivationWorkOrder.WorkOrderIds != null && bindActivationWorkOrder.WorkOrderIds.Any() ? bindActivationWorkOrder.WorkOrderIds.Except(hasBindWorkOrderIds):new List<long>();

            foreach (var item in needAdds)
            {
                addPlanWorkOrderBindEntities.Add(new PlanWorkOrderBindEntity()
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    SiteId = _currentSite.SiteId ?? 123456,

                    ResourceId= bindActivationWorkOrder.ResourceId,
                    WorkOrderId = item
                });

                //绑定记录
                bindRecordEntities.Add(new PlanWorkOrderBindRecordEntity()
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    SiteId = _currentSite.SiteId ?? 123456,

                    ResourceId = bindActivationWorkOrder.ResourceId,
                    WorkOrderId = item,
                    ActivateType= PlanWorkOrderActivateTypeEnum.Activate
                });

            }

            foreach (var item in needDeletes)
            {
                //取消绑定记录
                bindRecordEntities.Add(new PlanWorkOrderBindRecordEntity()
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    SiteId = _currentSite.SiteId ?? 123456,

                    ResourceId = bindActivationWorkOrder.ResourceId,
                    WorkOrderId = item,
                    ActivateType = PlanWorkOrderActivateTypeEnum.CancelActivate
                });

            }

            using (TransactionScope ts = new TransactionScope())
            {
                //先删除对应资源和工单ids的工单
                if (needDeletes.Count() > 0) 
                {
                    await _planWorkOrderBindRepository.DeletesTrueByResourceIdAndWorkOrderIdsAsync(new DeleteplanWorkOrderBindCommand()
                    {
                        ResourceId = bindActivationWorkOrder.ResourceId,
                        WorkOrderIds = needDeletes
                    });
                }                

                if (addPlanWorkOrderBindEntities.Count > 0) 
                {
                    //绑定激活工单
                    await _planWorkOrderBindRepository.InsertsAsync(addPlanWorkOrderBindEntities);
                }

                if (bindRecordEntities.Count > 0) 
                {
                    //记录绑定激活工单日志
                    await _planWorkOrderBindRecordRepository.InsertsAsync(bindRecordEntities);
                }

                ts.Complete();
            }
        }

        /// <summary>
        /// 获取资源id上已经绑定的工单
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public async Task<List<HasBindWorkOrderInfoDto>> GetHasBindWorkOrderAsync(long resourceId) 
        {
            List<HasBindWorkOrderInfoDto> hasBindWorkOrderInfoDtos = new List<HasBindWorkOrderInfoDto>();

            //查询已经绑定在该资源上的工单
            var hasBindWorkOrders = await _planWorkOrderBindRepository.GetPlanWorkOrderBindEntitiesAsync(new PlanWorkOrderBindQuery()
            {
                SiteId = _currentSite.SiteId??0,
                ResourceId = resourceId
            });
            var hasBindWorkOrderIds = hasBindWorkOrders.Select(x => x.WorkOrderId).ToList();
            if (hasBindWorkOrderIds.Count > 0)
            {
                var workOrderEntirtys= await _planWorkOrderRepository.GetByIdsAboutMaterialInfoAsync(hasBindWorkOrderIds.ToArray());
                //转换
                foreach (var item in workOrderEntirtys)
                {
                    hasBindWorkOrderInfoDtos.Add(new HasBindWorkOrderInfoDto()
                    {
                        Id=item.Id,
                        OrderCode=item.OrderCode,
                        MaterialCode=item.MaterialCode,
                        MaterialName=item.MaterialName,
                        MaterialVersion=item.MaterialVersion
                    });
                }
            }
           
            return hasBindWorkOrderInfoDtos;
            
        }
    }
}
