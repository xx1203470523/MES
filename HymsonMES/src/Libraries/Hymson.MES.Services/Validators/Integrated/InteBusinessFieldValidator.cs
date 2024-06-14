using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 字段定义 验证
    /// </summary>
    internal class InteBusinessFieldSaveValidator: AbstractValidator<InteBusinessFieldSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public InteBusinessFieldSaveValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15605));
            RuleFor(x => x.Code).Matches("^[A-Z0-9_]+$").WithErrorCode(nameof(ErrorCode.MES15611));
        }
    }

}
