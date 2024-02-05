/*
 *creator: Karl
 *
 *describe: 客户维护    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-07-11 09:33:26
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 客户维护 更新 验证
    /// </summary>
    internal class InteCustomCreateValidator: AbstractValidator<InteCustomCreateDto>
    {
        public InteCustomCreateValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18403));
            RuleFor(x => x.Code).Must(x => !x.Any(x => Char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES18410));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18404));
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES18405));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES18406));
            RuleFor(x => x.Address).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES18407));
            RuleFor(x => x.Describe).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES18408));
            RuleFor(x => x.Telephone).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES18409));
        }
    }


    internal class InteCustomImportValidator : AbstractValidator<InteCustomImportDto>
    {
        public InteCustomImportValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18403));
            RuleFor(x => x.Code).Must(x => !x.Any(x => Char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES18410));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18404));
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES18405));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES18406));
            RuleFor(x => x.Address).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES18407));
            RuleFor(x => x.Describe).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES18408));
            RuleFor(x => x.Telephone).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES18409));
        }
    }


    /// <summary>
    /// 客户维护 修改 验证
    /// </summary>
    internal class InteCustomModifyValidator : AbstractValidator<InteCustomModifyDto>
    {
        public InteCustomModifyValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18404));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES18406));
            RuleFor(x => x.Address).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES18407));
            RuleFor(x => x.Describe).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES18408));
            RuleFor(x => x.Telephone).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES18409));
        }
    }
}
