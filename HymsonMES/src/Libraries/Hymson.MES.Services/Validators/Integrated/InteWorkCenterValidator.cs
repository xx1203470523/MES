using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    ///工作中心表验证
    /// @author admin
    /// @date 2023-02-22
    /// </summary>
    internal class InteWorkCenterCreateValidator : AbstractValidator<InteWorkCenterCreateDto>
    {
        public InteWorkCenterCreateValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12102));
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES12103));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12104));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES12105));
            RuleFor(x => x.Type).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12106));
            RuleFor(x => x.Source).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12107));
            RuleFor(x => x.IsMixLine).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12108));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES12109));
        }
    }

    /// <summary>
    /// 工作中心表修改验证
    /// @author admin
    /// @date 2023-02-22  
    /// </summary>
    internal class InteWorkCenterModifyValidator : AbstractValidator<InteWorkCenterModifyDto>
    {
        public InteWorkCenterModifyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12104));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES12105));
            RuleFor(x => x.Type).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12106));
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12110));
            RuleFor(x => x.Source).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12107));
            RuleFor(x => x.IsMixLine).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12108));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES12109));
        }
    }
}
