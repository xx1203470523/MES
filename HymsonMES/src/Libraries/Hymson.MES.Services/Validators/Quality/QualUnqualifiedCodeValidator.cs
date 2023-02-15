using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// 不合格代码更新验证
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    internal class QualUnqualifiedCodeCreateValidator : AbstractValidator<QualUnqualifiedCodeCreateDto>
    {
        public QualUnqualifiedCodeCreateValidator()
        {
            RuleFor(x => x.UnqualifiedCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES21101));
            RuleFor(x => x.UnqualifiedCode).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES21102));
            RuleFor(x => x.UnqualifiedCodeName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES21103));
            RuleFor(x => x.UnqualifiedCodeName).MaximumLength(60).WithErrorCode(nameof(ErrorCode.MES21104));
            RuleFor(x => x.Type).NotEmpty().WithErrorCode(nameof(ErrorCode.MES21105));
            RuleFor(x => x.Degree).NotEmpty().WithErrorCode(nameof(ErrorCode.MES21106));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES21107));
        }
    }

    /// <summary>
    /// 不合格代码修改验证
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    internal class QualUnqualifiedCodeModifyValidator : AbstractValidator<QualUnqualifiedCodeModifyDto>
    {
        public QualUnqualifiedCodeModifyValidator()
        {
            RuleFor(x => x.UnqualifiedCodeName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES21103));
            RuleFor(x => x.UnqualifiedCodeName).MaximumLength(60).WithErrorCode(nameof(ErrorCode.MES21104));
            RuleFor(x => x.Type).NotEmpty().WithErrorCode(nameof(ErrorCode.MES21103));
            RuleFor(x => x.Degree).NotEmpty().WithErrorCode(nameof(ErrorCode.MES21103));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES21107));
        }
    }
}
