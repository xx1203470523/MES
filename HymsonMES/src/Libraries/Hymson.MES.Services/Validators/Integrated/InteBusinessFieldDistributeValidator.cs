using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 字段分配管理 验证
    /// </summary>
    internal class InteBusinessFieldDistributeSaveValidator: AbstractValidator<InteBusinessFieldDistributeSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public InteBusinessFieldDistributeSaveValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19428));
            RuleFor(x => x.Code).Matches("^[A-Z0-9_]+$").WithErrorCode(nameof(ErrorCode.MES19426));

            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10116));
            RuleFor(x => x.Name).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES10117));
            RuleFor(x => x.Type).Must(it => Enum.IsDefined(typeof(FieldAssignmentTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES10243));
        }
    }

}
