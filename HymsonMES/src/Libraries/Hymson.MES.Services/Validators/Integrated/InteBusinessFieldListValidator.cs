using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 字段定义列表数据 验证
    /// </summary>
    internal class InteBusinessFieldListSaveValidator: AbstractValidator<InteBusinessFieldListCreateDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public InteBusinessFieldListSaveValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));

            RuleFor(x => x.BusinessFieldId).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES18515));

            RuleFor(x => x.FieldLabel).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19428));
            RuleFor(x => x.FieldLabel).Matches("^[A-Z0-9_]+$").WithErrorCode(nameof(ErrorCode.MES19434));
        }
    }

}
