
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Inte;

namespace Hymson.MES.Services.Validators;

/// <summary>
/// 容器装载维护 ; 创建校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class InteContainerFreightCreateValidator : AbstractValidator<InteContainerFreightDto>
{
    public InteContainerFreightCreateValidator()
    {
        RuleFor(m => m.ContainerId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
        RuleFor(m => m.Type).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));        
    }
}

/// <summary>
/// 容器装载维护 ; 更新校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class InteContainerFreightUpdateValidator : AbstractValidator<InteContainerFreightUpdateDto>
{
    public InteContainerFreightUpdateValidator()
    {
        RuleFor(m => m.Id).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}

/// <summary>
/// 容器装载维护 ; 删除校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class InteContainerFreightDeleteValidator : AbstractValidator<InteContainerFreightDeleteDto>
{
    public InteContainerFreightDeleteValidator()
    {
        RuleFor(m => m.Ids).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}