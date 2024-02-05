using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.CoreServices.Bos.Job;

namespace Hymson.MES.CoreServices.Validators
{
    /// <summary>
    /// Esop获取参数校验
    /// </summary>
    internal class EsopOutJobValidator: AbstractValidator<EsopOutRequestBo>
    {
        public EsopOutJobValidator() {
            RuleFor(a => a.ProcedureId).NotEmpty().WithErrorCode(ErrorCode.MES16335);
            RuleFor(a => a.ResourceId).NotEmpty().WithErrorCode(ErrorCode.MES16334);
        }
    }
}
