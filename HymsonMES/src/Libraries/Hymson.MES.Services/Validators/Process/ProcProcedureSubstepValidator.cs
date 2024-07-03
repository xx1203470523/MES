using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 子步骤 验证
    /// </summary>
    internal class ProcProcedureSubstepSaveValidator: AbstractValidator<ProcProcedureSubstepSaveDto>
    {
        /// <summary>
        /// 验证
        /// </summary>
        public ProcProcedureSubstepSaveValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10113));
            RuleFor(x => x.Code).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES10115));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10116));
            RuleFor(x => x.Name).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES10117));
        }
    }

}
