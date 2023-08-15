using FluentValidation;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.SystemServices.Dtos.Plan;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.SystemServices.Services.Plan
{
    /// <summary>
    /// 订单同步服务
    /// </summary>
    public class PlanWorkOrderService : IPlanWorkOrderService
    {
        private readonly ICurrentSystem _currentSystem;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly AbstractValidator<PlanWorkOrderDto> _validationPlanWorkOrderDtoRules;

        public PlanWorkOrderService(ICurrentSystem currentSystem,
            IPlanWorkOrderRepository planWorkOrderRepository,
            AbstractValidator<PlanWorkOrderDto> validationPlanWorkOrderDtoRules)
        {
            _planWorkOrderRepository = planWorkOrderRepository;
            _currentSystem = currentSystem;
            _validationPlanWorkOrderDtoRules = validationPlanWorkOrderDtoRules;
        }

        /// <summary>
        /// 添加工单 整厂MES创建工单
        /// 目前对方只要求一个工单号字段，不合理后续可能修改
        /// </summary>
        /// <param name="planWorkOrderDto"></param>
        /// <returns></returns>
        public async Task AddWorkOrderAsync(PlanWorkOrderDto planWorkOrderDto)
        {
            //同步工单时的校验
            await _validationPlanWorkOrderDtoRules.ValidateAndThrowAsync(planWorkOrderDto);
            // DTO转换实体
            var planWorkOrderEntity = planWorkOrderDto.ToEntity<PlanWorkOrderEntity>();
            planWorkOrderEntity.SiteId = _currentSystem.SiteId;
            planWorkOrderEntity.Type = PlanWorkOrderTypeEnum.Production;//生产订单
            planWorkOrderEntity.Status = PlanWorkOrderStatusEnum.NotStarted;//同步初始状态
            planWorkOrderEntity.ProductId = 0;//产品ID预留
            planWorkOrderEntity.Remark = $"{_currentSystem.Name}-同步工单";
            planWorkOrderEntity.Id = IdGenProvider.Instance.CreateId();
            planWorkOrderEntity.CreatedBy = _currentSystem.Name;
            planWorkOrderEntity.UpdatedBy = _currentSystem.Name;
            planWorkOrderEntity.CreatedOn = HymsonClock.Now();
            planWorkOrderEntity.UpdatedOn = HymsonClock.Now();
            //新增订单
            await _planWorkOrderRepository.InsertAsync(planWorkOrderEntity);
        }
    }
}
