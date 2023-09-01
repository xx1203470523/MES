/*
 *creator: Karl
 *
 *describe: 编码规则组成    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:19
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Services.Dtos.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 编码规则组成 更新 验证
    /// </summary>
    internal class InteCodeRulesMakeCreateValidator: AbstractValidator<InteCodeRulesMakeCreateDto>
    {
        public InteCodeRulesMakeCreateValidator()
        {
            RuleFor(x => x.Seq).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12430));
            RuleFor(x => x.ValueTakingType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12431));
            RuleFor(x => x.SegmentedValue).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12432));

            RuleFor(x => x.SegmentedValue).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES12433));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES12434));

            RuleFor(x => x.ValueTakingType).Must(it => Enum.IsDefined(typeof(CodeValueTakingTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES12437));
        }
    }

    /// <summary>
    /// 编码规则组成 修改 验证
    /// </summary>
    internal class InteCodeRulesMakeModifyValidator : AbstractValidator<InteCodeRulesMakeModifyDto>
    {
        public InteCodeRulesMakeModifyValidator()
        {
            

        }
    }

}
