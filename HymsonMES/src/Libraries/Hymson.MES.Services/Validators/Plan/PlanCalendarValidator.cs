
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Plan;

namespace Hymson.MES.Services.Validators;

/// <summary>
/// 生产日历 ; 创建校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class PlanCalendarCreateValidator : AbstractValidator<PlanCalendarDto>
{
    public PlanCalendarCreateValidator()
    {        
    }
}

/// <summary>
/// 生产日历 ; 更新校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class PlanCalendarUpdateValidator : AbstractValidator<PlanCalendarUpdateDto>
{
    public PlanCalendarUpdateValidator()
    {
        RuleFor(m => m.Id).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}

/// <summary>
/// 生产日历 ; 删除校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class PlanCalendarDeleteValidator : AbstractValidator<PlanCalendarDeleteDto>
{
    public PlanCalendarDeleteValidator()
    {
        RuleFor(m => m.Ids).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}