using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Quality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    ///不合格代码组验证
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    internal class QualUnqualifiedGroupCreateValidator: AbstractValidator<QualUnqualifiedGroupCreateDto>
    {
        public QualUnqualifiedGroupCreateValidator()
        {
            RuleFor(x => x.UnqualifiedGroup).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11201));
            RuleFor(x => x.UnqualifiedGroup).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES11202));
            RuleFor(x => x.UnqualifiedGroupName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11203));
            RuleFor(x => x.UnqualifiedGroupName).MaximumLength(60).WithErrorCode(nameof(ErrorCode.MES11204));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES11205));
        }
    }

    /// <summary>
    /// 不合格代码组修改验证
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25   
    /// </summary>
    internal class QualUnqualifiedGroupModifyValidator : AbstractValidator<QualUnqualifiedGroupModifyDto>
    {
        public QualUnqualifiedGroupModifyValidator()
        {
            RuleFor(x => x.UnqualifiedGroupName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11203));
            RuleFor(x => x.UnqualifiedGroupName).MaximumLength(60).WithErrorCode(nameof(ErrorCode.MES11204));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES11205));
        }
    }
}
