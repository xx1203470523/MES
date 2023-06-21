using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 验证（掩码维护）
    /// </summary>
    internal class ProcMaskCodeValidator : AbstractValidator<ProcMaskCodeSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ProcMaskCodeValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10801));
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10811));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10810));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10812));

        }
    }
}
