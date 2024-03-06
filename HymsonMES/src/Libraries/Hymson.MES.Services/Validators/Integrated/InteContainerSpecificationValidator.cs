
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Inte;

namespace Hymson.MES.Services.Validators;

/// <summary>
/// 容器规格尺寸表 ; 创建校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class InteContainerSpecificationCreateValidator : AbstractValidator<InteContainerSpecificationDto>
{
    public InteContainerSpecificationCreateValidator()
    {
        RuleFor(m => m.ContainerId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));        
    }
}

/// <summary>
/// 容器规格尺寸表 ; 更新校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class InteContainerSpecificationUpdateValidator : AbstractValidator<InteContainerSpecificationUpdateDto>
{
    public InteContainerSpecificationUpdateValidator()
    {
        RuleFor(m => m.Id).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}

/// <summary>
/// 容器规格尺寸表 ; 删除校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class InteContainerSpecificationDeleteValidator : AbstractValidator<InteContainerSpecificationDeleteDto>
{
    public InteContainerSpecificationDeleteValidator()
    {
        RuleFor(m => m.Ids).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}