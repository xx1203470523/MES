/*
 *creator: Karl
 *
 *describe: 编码规则    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:26
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Sequences.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 编码规则 更新 验证
    /// </summary>
    internal class InteCodeRulesCreateValidator : AbstractValidator<InteCodeRulesCreateDto>
    {
        public InteCodeRulesCreateValidator()
        {
            //RuleFor(x => x.ProductId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12410));
            RuleFor(x => x.CodeMode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12442));
            RuleFor(x => x.CodeType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12411));
            RuleFor(x => x.Base).NotEmpty().GreaterThan(0).WithErrorCode(nameof(ErrorCode.MES12412));
            RuleFor(x => x.Increment).GreaterThan(0).WithErrorCode(nameof(ErrorCode.MES12413));
            RuleFor(x => x.OrderLength).GreaterThanOrEqualTo(0).WithErrorCode(nameof(ErrorCode.MES12414));
            RuleFor(x => x.ResetType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12415));
            
            RuleFor(x => x.CodeRulesMakes).Must((x, CodeRulesMakes) => x.CodeRulesMakes != null && x.CodeRulesMakes.Any()).WithErrorCode(nameof(ErrorCode.MES12416));


            RuleFor(x => x.IgnoreChar).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES16030));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES16030));
            RuleFor(x => x.CodeMode).Must(it => Enum.IsDefined(typeof(CodeRuleCodeModeEnum), it)).WithErrorCode(nameof(ErrorCode.MES12443));
            RuleFor(x => x.CodeType).Must(it => Enum.IsDefined(typeof(CodeRuleCodeTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES16038));
            RuleFor(x => x.ResetType).Must(it => Enum.IsDefined(typeof(SerialNumberTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES12439));
            RuleFor(x => x.StartNumber).Must(it => it>=1 && it % 1== 0 ).WithErrorCode(nameof(ErrorCode.MES12440));
            RuleFor(x => x.Base).Must(it => it == 10 || it == 16 || it == 32 ).WithErrorCode(nameof(ErrorCode.MES12441));

            //RuleFor(x => x).Must(y=>y.CodeType!= CodeRuleCodeTypeEnum.PackagingSeqCode|| (y.PackType.HasValue && Enum.IsDefined(typeof(CodeRulePackTypeEnum), y.PackType)) ).WithErrorCode(nameof(ErrorCode.MES12435));
            RuleFor(x => x.IgnoreChar).Must(y => string.IsNullOrEmpty(y) || new Regex(@"^[A-Z](;[A-Z])*$").IsMatch(y) ).WithErrorCode(nameof(ErrorCode.MES12436));
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

            //RuleFor(x => x.ProductId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12410));
            RuleFor(x => x.CodeMode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12442));
            RuleFor(x => x.CodeType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12411));
            RuleFor(x => x.Base).NotEmpty().GreaterThan(0).WithErrorCode(nameof(ErrorCode.MES12412));
            RuleFor(x => x.Increment).GreaterThan(0).WithErrorCode(nameof(ErrorCode.MES12413));
            RuleFor(x => x.OrderLength).GreaterThanOrEqualTo(0).WithErrorCode(nameof(ErrorCode.MES12414));
            RuleFor(x => x.ResetType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12415));

            RuleFor(x => x.CodeRulesMakes).Must((x, CodeRulesMakes) =>
                 x.CodeRulesMakes != null && x.CodeRulesMakes.Count != 0).WithErrorCode(nameof(ErrorCode.MES12416));

            RuleFor(x => x.IgnoreChar).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES16030));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES16030));
            RuleFor(x => x.CodeMode).Must(it => Enum.IsDefined(typeof(CodeRuleCodeModeEnum), it)).WithErrorCode(nameof(ErrorCode.MES12443));
            RuleFor(x => x.CodeType).Must(it => Enum.IsDefined(typeof(CodeRuleCodeTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES16038));
            RuleFor(x => x.ResetType).Must(it => Enum.IsDefined(typeof(SerialNumberTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES12439));
            RuleFor(x => x.StartNumber).Must(it => it >= 1 && it % 1 == 0).WithErrorCode(nameof(ErrorCode.MES12440));
            RuleFor(x => x.Base).Must(it => it == 10 || it == 16 || it == 32).WithErrorCode(nameof(ErrorCode.MES12441));

            //RuleFor(x => x).Must(y => y.CodeType != CodeRuleCodeTypeEnum.PackagingSeqCode || (y.PackType.HasValue && Enum.IsDefined(typeof(CodeRulePackTypeEnum), y.PackType))).WithErrorCode(nameof(ErrorCode.MES12435));
            RuleFor(x => x.IgnoreChar).Must(y => string.IsNullOrEmpty(y) || new Regex(@"^[A-Z](;[A-Z])*$").IsMatch(y)).WithErrorCode(nameof(ErrorCode.MES12436));
        }
    }
}
