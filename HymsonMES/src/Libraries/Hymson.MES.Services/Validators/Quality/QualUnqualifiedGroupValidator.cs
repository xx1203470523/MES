using FluentValidation;
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
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
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
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
