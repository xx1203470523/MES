using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
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
            RuleFor(x => x.Minimum).GreaterThan(0).WithErrorCode(nameof(ErrorCode.MES12506));
            RuleFor(x => x.Maximum).GreaterThan(0).WithErrorCode(nameof(ErrorCode.MES12507));
            RuleFor(x => x.Maximum).GreaterThan(x => x.Minimum).WithErrorCode(nameof(ErrorCode.MES12508));
            RuleFor(x => x.Level).NotEmpty().Must(l => Enum.IsDefined(typeof(LevelEnum), l)).WithErrorCode(nameof(ErrorCode.MES12512));
        }
    }

}
