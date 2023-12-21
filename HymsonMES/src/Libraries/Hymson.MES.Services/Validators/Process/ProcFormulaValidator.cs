using FluentValidation;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 配方维护 验证
    /// </summary>
    internal class ProcFormulaSaveValidator: AbstractValidator<ProcFormulaSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ProcFormulaSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}
