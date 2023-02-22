using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    ///作业表验证
    /// @author admin
    /// @date 2023-02-21
    /// </summary>
    internal class InteJobCreateValidator : AbstractValidator<InteJobCreateDto>
    {
        public InteJobCreateValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12002));
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES12003));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12004));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES12005));
            RuleFor(x => x.ClassProgram).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12006));
            RuleFor(x => x.ClassProgram).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES12007));
            RuleFor(x => x.Remark).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES12008));
        }
    }

    /// <summary>
    /// 作业表修改验证
    /// @author admin
    /// @date 2023-02-21  
    /// </summary>
    internal class InteJobModifyValidator : AbstractValidator<InteJobModifyDto>
    {
        public InteJobModifyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12004));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES12005));
            RuleFor(x => x.ClassProgram).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12006));
            RuleFor(x => x.ClassProgram).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES12007));
            RuleFor(x => x.Remark).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES12008));
        }
    }
}
