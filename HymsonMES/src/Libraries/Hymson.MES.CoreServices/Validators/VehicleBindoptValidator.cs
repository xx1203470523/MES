using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.CoreServices.Bos.Job;

namespace Hymson.MES.CoreServices.Validators
{
    public class VehicleBindoptValidator : AbstractValidator<VehicleBo>
    {
        public VehicleBindoptValidator()
        {
            // RuleFor(x => x.LocationId).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES18620));

            // RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.PalletNo).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18604));
            // RuleFor(x => x.SFC).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18621));
            //RuleFor(x => x.Code).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18606));
            //RuleFor(x => x.VehicleTypeId).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES18611));

        }
    }
}
