/*
 *creator: Karl
 *
 *describe: 编码规则    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:26
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
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
            RuleFor(x => x.ProductId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12410));
            RuleFor(x => x.CodeType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12411));
            RuleFor(x => x.Base).NotEmpty().GreaterThan(0).WithErrorCode(nameof(ErrorCode.MES12412));
            RuleFor(x => x.Increment).GreaterThan(0).WithErrorCode(nameof(ErrorCode.MES12413));
            RuleFor(x => x.OrderLength).GreaterThanOrEqualTo(0).WithErrorCode(nameof(ErrorCode.MES12414));
            RuleFor(x => x.ResetType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12415));

            RuleFor(x => x.CodeRulesMakes).Must((x,CodeRulesMakes)=>x.CodeRulesMakes.Count!=0).WithErrorCode(nameof(ErrorCode.MES12416));

            //RuleFor(x => x.CodeRulesMakes).NotEmpty().Must((x, CodeRulesMakes) => x.CodeRulesMakes.Count == 0).WithMessage(ErrorCode.MES12416);

            RuleFor(x => x.IgnoreChar).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES16030));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES16030));

            //RuleFor(x => x).Must((x) =>
            //{
            //    foreach (var item in x.CodeRulesMakes)
            //    {
                    
            //    }
            //    return true;
            //}
            //    ).WithErrorCode(nameof(ErrorCode.MES12416));
        }
    }

    /// <summary>
    /// 编码规则 修改 验证
    /// </summary>
    internal class InteCodeRulesModifyValidator : AbstractValidator<InteCodeRulesModifyDto>
    {
        public InteCodeRulesModifyValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12419));

            RuleFor(x => x.ProductId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12410));
            RuleFor(x => x.CodeType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12411));
            RuleFor(x => x.Base).NotEmpty().GreaterThan(0).WithErrorCode(nameof(ErrorCode.MES12412));
            RuleFor(x => x.Increment).GreaterThan(0).WithErrorCode(nameof(ErrorCode.MES12413));
            RuleFor(x => x.OrderLength).GreaterThanOrEqualTo(0).WithErrorCode(nameof(ErrorCode.MES12414));
            RuleFor(x => x.ResetType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12415));

            RuleFor(x => x.CodeRulesMakes).NotEmpty().Must((x, CodeRulesMakes) => x.CodeRulesMakes.Count == 0).WithErrorCode(nameof(ErrorCode.MES12416));

            RuleFor(x => x.IgnoreChar).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES16030));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES16030));
        }
    }
}
