using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Process
{
    internal class ProcResourceCreateValidator : AbstractValidator<ProcResourceCreateDto>
    {
        public ProcResourceCreateValidator()
        {
            RuleFor(x => x.ResCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10301));
            RuleFor(x => x.ResCode).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10302));
            RuleFor(x => x.ResName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10303));
            RuleFor(x => x.ResName).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10304));
            //RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(SysDataStatusEnum), it)).WithErrorCode(ErrorCode.MES10380);

            //RuleFor(x => x.ResTypeId).NotEmpty().WithErrorCode(ErrorCode.MES10320);
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    internal class ProcResourcelModifyValidator : AbstractValidator<ProcResourceModifyDto>
    {
        public ProcResourcelModifyValidator()
        {
            RuleFor(x => x.ResName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10303));
            RuleFor(x => x.ResName).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10304));
            // RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(SysDataStatusEnum), it)).WithErrorCode(ErrorCode.MES10380);

            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
