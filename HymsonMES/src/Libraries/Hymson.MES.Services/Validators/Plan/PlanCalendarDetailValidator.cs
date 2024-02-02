
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Plan;

namespace Hymson.MES.Services.Validators;

/// <summary>
/// 生产日历详情 ; 创建校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class PlanCalendarDetailCreateValidator : AbstractValidator<PlanCalendarDetailDto>
{
    public PlanCalendarDetailCreateValidator()
    {
        RuleFor(m => m.SiteId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
        RuleFor(m => m.Day).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}

/// <summary>
/// 生产日历详情 ; 更新校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class PlanCalendarDetailUpdateValidator : AbstractValidator<PlanCalendarDetailUpdateDto>
{
    public PlanCalendarDetailUpdateValidator()
    {
        RuleFor(m => m.Id).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}

/// <summary>
/// 生产日历详情 ; 删除校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class PlanCalendarDetailDeleteValidator : AbstractValidator<PlanCalendarDetailDeleteDto>
{
    public PlanCalendarDetailDeleteValidator()
    {
        RuleFor(m => m.Ids).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}