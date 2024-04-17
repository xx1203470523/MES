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
using Hymson.Utils.Tools;
using System.Linq;
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

        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;
        private readonly IPlanWorkOrderBindRecordRepository _planWorkOrderBindRecordRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        public PlanWorkOrderBindService(ICurrentUser currentUser, ICurrentSite currentSite, IPlanWorkOrderBindRepository planWorkOrderBindRepository,
            IInteWorkCenterRepository inteWorkCenterRepository, IPlanWorkOrderActivationRepository planWorkOrderActivationRepository, IPlanWorkOrderBindRecordRepository planWorkOrderBindRecordRepository, IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _planWorkOrderBindRepository = planWorkOrderBindRepository;

            _inteWorkCenterRepository = inteWorkCenterRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _planWorkOrderBindRecordRepository = planWorkOrderBindRecordRepository;

            _planWorkOrderRepository = planWorkOrderRepository;
        }

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
                throw new CustomerValidationException(nameof(ErrorCode.MES16803));
            }

            if (workCenterEntity.Type != WorkCenterTypeEnum.Line)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16801));
            }


            if (bindActivationWorkOrder.WorkOrderIds != null && bindActivationWorkOrder.WorkOrderIds.Any())
            {
                //检查当前工单是否重复
                if (bindActivationWorkOrder.WorkOrderIds.Distinct().Count() != bindActivationWorkOrder.WorkOrderIds.Count)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16804));
                }

                //检查当前这些工单是否是激活
                var hasActivationWorkOrders = await _planWorkOrderActivationRepository.GetPlanWorkOrderActivationEntitiesAsync(new PlanWorkOrderActivationQuery()
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    LineId = workCenterEntity.Id,
                    WorkOrderIds = bindActivationWorkOrder.WorkOrderIds,
                });

                if (hasActivationWorkOrders.Count() != bindActivationWorkOrder.WorkOrderIds.Count)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16802));
                }
                //检查工单站点
                var planWorkOrderEntities = await _planWorkOrderRepository.GetByIdsAsync(bindActivationWorkOrder.WorkOrderIds);
                var firstEntity = planWorkOrderEntities.First();
                if (firstEntity.SiteId != _currentSite.SiteId)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16805));
                }

            }

            //查询已经绑定在该资源上的工单
            var hasBindWorkOrders= await _planWorkOrderBindRepository.GetPlanWorkOrderBindEntitiesAsync(new PlanWorkOrderBindQuery() {
                SiteId = _currentSite.SiteId ?? 0,
                ResourceId= bindActivationWorkOrder.ResourceId
            });
            var hasBindWorkOrderIds = hasBindWorkOrders.Select(x => x.WorkOrderId).ToList();

            List< PlanWorkOrderBindEntity > addPlanWorkOrderBindEntities = new();

            List<PlanWorkOrderBindRecordEntity> bindRecordEntities = new();

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
                    SiteId = _currentSite.SiteId ?? 0,

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
                    SiteId = _currentSite.SiteId ?? 0,

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
                    SiteId = _currentSite.SiteId ?? 0,

                    ResourceId = bindActivationWorkOrder.ResourceId,
                    WorkOrderId = item,
                    ActivateType = PlanWorkOrderActivateTypeEnum.CancelActivate
                });

            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //先删除对应资源和工单ids的工单
                if (needDeletes.Any()) 
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
