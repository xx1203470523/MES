
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Qual;

namespace Hymson.MES.Services.Validators;

/// <summary>
/// IQC检验项目 ; 创建校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class QualIqcInspectionItemCreateValidator : AbstractValidator<QualIqcInspectionItemDto>
{
    public QualIqcInspectionItemCreateValidator()
    {
        RuleFor(m => m.MaterialId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11910));
    }
}

/// <summary>
/// IQC检验项目 ; 更新校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class QualIqcInspectionItemUpdateValidator : AbstractValidator<QualIqcInspectionItemUpdateDto>
{
    public QualIqcInspectionItemUpdateValidator()
    {
        RuleFor(m => m.Id).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}

/// <summary>
/// IQC检验项目 ; 删除校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class QualIqcInspectionItemDeleteValidator : AbstractValidator<QualIqcInspectionItemDeleteDto>
{
    public QualIqcInspectionItemDeleteValidator()
    {
        RuleFor(m => m.Ids).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}