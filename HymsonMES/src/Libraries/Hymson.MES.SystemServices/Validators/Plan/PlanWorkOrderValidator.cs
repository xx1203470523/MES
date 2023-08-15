using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.SystemServices.Dtos.Plan;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.SystemServices.Validators.Plan
{
    /// <summary>
    /// 工单同步校验
    /// </summary>
    public class PlanWorkOrderValidator : AbstractValidator<PlanWorkOrderDto>
    {
        private readonly ICurrentSystem _currentSystem;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        /// <summary>
        /// 同步工单校验
        /// </summary>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="currentSystem"></param>
        public PlanWorkOrderValidator(IPlanWorkOrderRepository planWorkOrderRepository, ICurrentSystem currentSystem)
        {
            _currentSystem = currentSystem;
            _planWorkOrderRepository = planWorkOrderRepository;
            //工单号不允许为空
            RuleFor(x => x.OrderCode).NotEmpty().Must(list => list.Any()).WithErrorCode(ErrorCode.MES19201);
            //校验工单是否已经存在
            RuleFor(x => x).MustAsync(async (planWorkOrderDto, cancellation) =>
            {
                var planWorkOrder = await _planWorkOrderRepository.GetByCodeAsync(new PlanWorkOrderQuery
                {
                    OrderCode = planWorkOrderDto.OrderCode,
                    SiteId = _currentSystem.SiteId
                });
                return planWorkOrder == null;
            }).WithErrorCode(nameof(ErrorCode.MES19202));
        }
    }
}
