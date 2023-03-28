using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 验证（容器维护）
    /// </summary>
    internal class InteContainerValidator : AbstractValidator<InteContainerSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public InteContainerValidator()
        {
            RuleFor(x => x.Maximum).GreaterThanOrEqualTo(x => x.Minimum).WithErrorCode(nameof(ErrorCode.MES12502));
            //RuleFor(x => x).Must(x => { return x.Minimum <= x.Maximum; }).WithErrorCode(nameof(ErrorCode.MES12502));
        }
    }

}
