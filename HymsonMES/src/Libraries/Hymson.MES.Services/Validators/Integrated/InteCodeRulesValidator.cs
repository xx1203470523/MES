/*
 *creator: Karl
 *
 *describe: 编码规则    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:26
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 编码规则 更新 验证
    /// </summary>
    internal class InteCodeRulesCreateValidator: AbstractValidator<InteCodeRulesCreateDto>
    {
        public InteCodeRulesCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    /// <summary>
    /// 编码规则 修改 验证
    /// </summary>
    internal class InteCodeRulesModifyValidator : AbstractValidator<InteCodeRulesModifyDto>
    {
        public InteCodeRulesModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
