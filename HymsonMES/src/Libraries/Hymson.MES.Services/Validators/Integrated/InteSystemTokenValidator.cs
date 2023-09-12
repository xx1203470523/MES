/*
 *creator: Karl
 *
 *describe: 系统Token    验证规则 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-06-15 02:09:57
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 系统Token 更新 验证
    /// </summary>
    internal class InteSystemTokenCreateValidator: AbstractValidator<InteSystemTokenCreateDto>
    {
        public InteSystemTokenCreateValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.SystemCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18301));
            RuleFor(x => x.SystemCode).Must(x => !x.Any(x => Char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES18302));
            RuleFor(x => x.SystemName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18303));
            RuleFor(x => x.SystemCode).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES18304));
            RuleFor(x => x.SystemName).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES18305));
        }
    }

    /// <summary>
    /// 系统Token 修改 验证
    /// </summary>
    internal class InteSystemTokenModifyValidator : AbstractValidator<InteSystemTokenModifyDto>
    {
        public InteSystemTokenModifyValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.SystemName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18303));
            RuleFor(x => x.SystemName).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES18305));
        }
    }
}
