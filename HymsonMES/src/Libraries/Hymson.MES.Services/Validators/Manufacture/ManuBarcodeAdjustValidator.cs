using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Manufacture
{
    internal class ManusBarcodeSplitAdjustValidator : AbstractValidator<ManuBarcodeSplitAdjustDto>
    {
        public ManusBarcodeSplitAdjustValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12800));
            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12801));
            RuleFor(x => x.Qty).Must(x=>x>0).WithErrorCode(nameof(ErrorCode.MES12828));
        }
    }
}
