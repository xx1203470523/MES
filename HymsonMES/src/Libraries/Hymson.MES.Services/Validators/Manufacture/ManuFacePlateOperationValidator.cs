using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture;

internal class ManuFacePlateConfirmValidator : AbstractValidator<ManuFacePlateCommonDto>
{
    public ManuFacePlateConfirmValidator()
    {
        RuleFor(m => m.ProcedureCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
        RuleFor(m => m.ResourceCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
        RuleFor(m => m.Sfcs).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}

internal class ManuFacePlateConfirmByContainerCodeDtoValidator : AbstractValidator<ManuFacePlatePackDto>
{
    public ManuFacePlateConfirmByContainerCodeDtoValidator()
    {
        RuleFor(m => m.ProcedureCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
        RuleFor(m => m.ResourceCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
        RuleFor(m => m.Sfcs).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10130));
    }
}