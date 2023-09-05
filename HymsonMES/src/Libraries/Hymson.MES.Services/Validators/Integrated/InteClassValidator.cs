using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 验证
    /// </summary>
    internal class InteClassSaveValidator : AbstractValidator<InteClassSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public InteClassSaveValidator()
        {
            RuleFor(x => x.ClassType).NotEmpty().WithErrorCode(ErrorCode.MES10215);

        }
    }
}
