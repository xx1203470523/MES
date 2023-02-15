/*
 *creator: Karl
 *
 *describe: QualUnqualifiedGroupEntity    验证规则 | 代码由框架生成  
 *builder:  wangkeming
 *build datetime: 2023-02-13 02:05:50
 */
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
    /// QualUnqualifiedGroupEntity 更新 验证
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
    /// QualUnqualifiedGroupEntity 修改 验证
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
