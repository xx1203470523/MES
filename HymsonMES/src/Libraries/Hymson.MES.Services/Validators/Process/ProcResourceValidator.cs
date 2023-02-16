using FluentValidation;
using Hymson.MES.Core.Constants;
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
            RuleFor(x => x.ResCode).NotEmpty().WithErrorCode(ErrorCode.MES10301);
            RuleFor(x => x.ResName).NotEmpty().WithErrorCode(ErrorCode.MES10303);
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(ErrorCode.MES10305);
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    internal class ProcResourcelModifyValidator : AbstractValidator<ProcResourceModifyDto>
    {
        public ProcResourcelModifyValidator()
        {
            RuleFor(x => x.ResName).NotEmpty().WithErrorCode(ErrorCode.MES10303);
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
