using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;
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
            RuleFor(x => x.UnqualifiedCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11101));
            RuleFor(x => x.UnqualifiedCode).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES11102));
            RuleFor(x => x.UnqualifiedCodeName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11103));
            RuleFor(x => x.UnqualifiedCodeName).MaximumLength(60).WithErrorCode(nameof(ErrorCode.MES11104));
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11109));
            RuleFor(x => x.Type).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11105));
            RuleFor(x => x.Degree).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11106));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES11107));
            RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(SysDataStatusEnum), it)).WithErrorCode(nameof(ErrorCode.MES11112));
            RuleFor(x => x.Type).Must(it => Enum.IsDefined(typeof(QualUnqualifiedCodeTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES11113));
            RuleFor(x => x.Degree).Must(it => Enum.IsDefined(typeof(QualUnqualifiedCodeDegreeEnum), it)).WithErrorCode(nameof(ErrorCode.MES11114));
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
            RuleFor(x => x.UnqualifiedCodeName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11103));
            RuleFor(x => x.UnqualifiedCodeName).MaximumLength(60).WithErrorCode(nameof(ErrorCode.MES11104));
            RuleFor(x => x.Type).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11103));
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11109));
            RuleFor(x => x.Degree).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11103));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES11107));
        }
    }
}
