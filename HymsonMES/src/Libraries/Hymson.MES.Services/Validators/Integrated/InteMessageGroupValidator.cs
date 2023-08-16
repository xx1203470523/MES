using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 消息组 验证
    /// </summary>
    internal class InteMessageGroupSaveValidator: AbstractValidator<InteMessageGroupSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public InteMessageGroupSaveValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10113));
            RuleFor(x => x.Code).Must(x => x.Any(x => char.IsWhiteSpace(x)) == false).WithErrorCode(nameof(ErrorCode.MES10114));
            RuleFor(x => x.Code).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES10115));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10116));
            RuleFor(x => x.Name).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES10117));
            RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(DisableOrEnableEnum), it)).WithErrorCode(nameof(ErrorCode.MES10120));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES10121));

            RuleFor(x => x.WorkShopId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10522));
        }
    }

}
