using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Quality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Process
{
    internal class ProcResourceTypeCreateValidator : AbstractValidator<ProcResourceTypeAddDto>
    {
        public ProcResourceTypeCreateValidator()
        {
            RuleFor(x => x.ResType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10351));
            RuleFor(x => x.ResType).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10352));
            RuleFor(x => x.ResTypeName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10353));
            RuleFor(x => x.ResTypeName).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10354));
        }
    }

    internal class ProcResourceTypeModifyValidator : AbstractValidator<ProcResourceTypeUpdateDto>
    {
        public ProcResourceTypeModifyValidator()
        {
            RuleFor(x => x.ResTypeName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10353));
            RuleFor(x => x.ResTypeName).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10354));
        }
    }
}
