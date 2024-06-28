using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 资质认证 验证
    /// </summary>
    internal class InteQualificationAuthenticationSaveValidator: AbstractValidator<InteQualificationAuthenticationSaveDto>
    {
        /// <summary>
        ///验证
        /// </summary>
        public InteQualificationAuthenticationSaveValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10113));
            RuleFor(x => x.Code).Must(x => !x.Any(x => char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES10114));
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10109));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10116));
            RuleFor(x => x.Name).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES10117));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES10121));
        }
    }

}
