/*
 *creator: Karl
 *
 *describe: BOM明细表    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-02-14 10:38:06
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// BOM明细表 更新 验证
    /// </summary>
    internal class ProcBomDetailCreateValidator: AbstractValidator<ProcBomDetailCreateDto>
    {
        public ProcBomDetailCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    /// <summary>
    /// BOM明细表 修改 验证
    /// </summary>
    internal class ProcBomDetailModifyValidator : AbstractValidator<ProcBomDetailModifyDto>
    {
        public ProcBomDetailModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
