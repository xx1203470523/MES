/*
 *creator: Karl
 *
 *describe: BOM表    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-02-14 10:04:25
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
    /// BOM表 更新 验证
    /// </summary>
    internal class ProcBomCreateValidator: AbstractValidator<ProcBomCreateDto>
    {
        public ProcBomCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    /// <summary>
    /// BOM表 修改 验证
    /// </summary>
    internal class ProcBomModifyValidator : AbstractValidator<ProcBomModifyDto>
    {
        public ProcBomModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
