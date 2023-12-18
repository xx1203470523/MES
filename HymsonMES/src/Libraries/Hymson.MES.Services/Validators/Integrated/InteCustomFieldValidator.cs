using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 自定义字段 验证
    /// </summary>
    internal class InteCustomFieldSaveValidator: AbstractValidator<InteCustomFieldSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public InteCustomFieldSaveValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15605));
            RuleFor(x => x.Name).Matches("^[a-zA-Z0-9]+$").WithErrorCode(nameof(ErrorCode.MES15611));

            RuleFor(x => x.Remark).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES15608));
            RuleFor(x => x.Name).Must(x => !x.Any(x => char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES15606));
            RuleFor(x => x.BusinessType).Must(it => Enum.IsDefined(typeof(InteCustomFieldBusinessTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES15604));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES18407));

            //When(x => x != null &&x.Languages!=null && x.Languages.Any(), () =>
            //{
            //    RuleForEach(x => x.Languages).ChildRules(c =>
            //    {
            //        c.RuleFor(y => y.LanguageType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15609));
            //        c.RuleFor(y => y.LanguageType).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES15610));

            //        c.RuleFor(y => y.TranslationValue).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15609));
            //        c.RuleFor(y => y.TranslationValue).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES15610));
            //        c.RuleFor(x => x.TranslationValue).Matches("^[a-zA-Z0-9]+$").WithErrorCode(nameof(ErrorCode.MES15611));
            //        c.RuleFor(x => x.TranslationValue).Must(x => !x.Any(x => char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES15613));
            //    });
            //});

        }
    }

    /// <summary>
    /// 字段国际化 验证
    /// </summary>
    internal class InteCustomFieldInternationalizationValidator : AbstractValidator<InteCustomFieldInternationalizationDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public InteCustomFieldInternationalizationValidator()
        {
            RuleFor(y => y.LanguageType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15609));
            RuleFor(y => y.LanguageType).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES15610));

            RuleFor(y => y.TranslationValue).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15609));
            RuleFor(y => y.TranslationValue).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES15610));
            RuleFor(x => x.TranslationValue).Matches("^[^`~!@#$%^&*()+<>?:\\\"\\'{},.\\/;\\[\\]\\\\]+$").WithErrorCode(nameof(ErrorCode.MES15612));
            RuleFor(x => x.TranslationValue).Must(x => !x.Any(x => char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES15613));
        }
    }

}
