using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Integrated;
namespace Hymson.MES.Services.Validators.Integrated
{
    internal class InteSFCBoxValidator : AbstractValidator<InteSFCBoxImportDto>
    {
        public InteSFCBoxValidator()
        {
            RuleFor(x => x.Grade).NotEmpty().WithErrorCode(ErrorCode.MES19301);
            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(ErrorCode.MES19302);
            RuleFor(x => x.BoxCode).NotEmpty().WithErrorCode(ErrorCode.MES19303);
        }
   
    }
}
