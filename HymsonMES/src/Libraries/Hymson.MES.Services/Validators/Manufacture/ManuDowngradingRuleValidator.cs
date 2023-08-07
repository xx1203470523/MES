/*
 *creator: Karl
 *
 *describe: 降级规则    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-08-07 02:00:57
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 降级规则 更新 验证
    /// </summary>
    internal class ManuDowngradingRuleCreateValidator: AbstractValidator<ManuDowngradingRuleCreateDto>
    {
        public ManuDowngradingRuleCreateValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES21103));
            RuleFor(x => x.Code).Must(x => !x.Any(x => Char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES21108));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES21104));
            RuleFor(x => x.Code).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES21105));
            RuleFor(x => x.Name).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES21106));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES21107));
        }
    }

    /// <summary>
    /// 降级规则 修改 验证
    /// </summary>
    internal class ManuDowngradingRuleModifyValidator : AbstractValidator<ManuDowngradingRuleModifyDto>
    {
        public ManuDowngradingRuleModifyValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Id).Must(x => x > 0).WithErrorCode(nameof(ErrorCode.MES21109));
            //RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES21103));
            //RuleFor(x => x.Code).Must(x => !x.Any(x => Char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES21108));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES21104));
            //RuleFor(x => x.Code).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES21105));
            RuleFor(x => x.Name).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES21106));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES21107));
        }
    }
}
