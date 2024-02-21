
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Qual;

namespace Hymson.MES.Services.Validators;

/// <summary>
/// IQC检验项目详情 ; 创建校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class QualIqcInspectionItemDetailCreateValidator : AbstractValidator<QualIqcInspectionItemDetailDto>
{
    public QualIqcInspectionItemDetailCreateValidator()
    {
        RuleFor(m => m.QualIqcInspectionItemId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
        RuleFor(m => m.ParameterId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}

/// <summary>
/// IQC检验项目详情 ; 更新校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class QualIqcInspectionItemDetailUpdateValidator : AbstractValidator<QualIqcInspectionItemDetailUpdateDto>
{
    public QualIqcInspectionItemDetailUpdateValidator()
    {
        RuleFor(m => m.Id).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}

/// <summary>
/// IQC检验项目详情 ; 删除校验器
/// 描述：
/// </summary>
/// <returns></returns>
internal class QualIqcInspectionItemDetailDeleteValidator : AbstractValidator<QualIqcInspectionItemDetailDeleteDto>
{
    public QualIqcInspectionItemDetailDeleteValidator()
    {
        RuleFor(m => m.Ids).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}