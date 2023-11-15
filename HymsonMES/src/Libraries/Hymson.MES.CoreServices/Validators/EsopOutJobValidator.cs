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
            RuleFor(a => a.ProcedureId).NotEmpty().WithErrorCode(ErrorCode.MES10214);
            RuleFor(a => a.MaterialId).NotEmpty().WithErrorCode(ErrorCode.MES16335);
        }
    }
}
