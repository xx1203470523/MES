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
        /// 容器新建校验
        /// </summary>
        public InteContainerValidator()
        {
            RuleFor(x => x.Minimum).GreaterThan(0).WithErrorCode(nameof(ErrorCode.MES12506));
            RuleFor(x => x.Maximum).GreaterThan(0).WithErrorCode(nameof(ErrorCode.MES12507));
            RuleFor(x => x.Maximum).GreaterThan(x => x.Minimum).WithErrorCode(nameof(ErrorCode.MES12508));
            RuleFor(x => x.Level).NotEmpty().Must(l => Enum.IsDefined(typeof(LevelEnum), l)).WithErrorCode(nameof(ErrorCode.MES12512));
            //允许为空但必须大于0
            RuleFor(x => x.Height).Must(Height => Height == null || Height > 0).WithErrorCode(nameof(ErrorCode.MES12521)).WithMessage("高度");
            RuleFor(x => x.Length).Must(Length => Length == null || Length > 0).WithErrorCode(nameof(ErrorCode.MES12522));
            RuleFor(x => x.Width).Must(Width => Width == null || Width > 0).WithErrorCode(nameof(ErrorCode.MES12523));
            RuleFor(x => x.Weight).Must(Weight => Weight == null || Weight > 0).WithErrorCode(nameof(ErrorCode.MES12524));
            RuleFor(x => x.MaxFillWeight).Must(MaxFillWeight => MaxFillWeight == null || MaxFillWeight > 0).WithErrorCode(nameof(ErrorCode.MES12525));
        }
    }

}
