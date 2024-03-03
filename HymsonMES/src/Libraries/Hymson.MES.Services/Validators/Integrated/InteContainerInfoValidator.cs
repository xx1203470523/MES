
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Inte;

namespace Hymson.MES.Services.Validators;

/// <summary>
/// 容器维护 ; 创建校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class InteContainerInfoCreateValidator : AbstractValidator<InteContainerInfoDto>
{
    public InteContainerInfoCreateValidator()
    {
        RuleFor(m => m.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
        RuleFor(m => m.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}

/// <summary>
/// 容器维护 ; 更新校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class InteContainerInfoUpdateValidator : AbstractValidator<InteContainerInfoUpdateDto>
{
    public InteContainerInfoUpdateValidator()
    {
        RuleFor(m => m.Id).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}

/// <summary>
/// 容器维护 ; 删除校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class InteContainerInfoDeleteValidator : AbstractValidator<InteContainerInfoDeleteDto>
{
    public InteContainerInfoDeleteValidator()
    {
        RuleFor(m => m.Ids).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}