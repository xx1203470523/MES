using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// Marking录入校验器
    /// </summary>
    internal class ManuMarkingValidator: AbstractValidator<ManuMarkingCheckDto>
    {
        public ManuMarkingValidator()
        {
            RuleFor(x => x.FoundBadOperationId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19701));
            RuleFor(x => x.UnqualifiedId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19702));
            RuleFor(x => x.Sfc).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19703));
        }
    }
}
