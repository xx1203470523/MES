/*
 *creator: Karl
 *
 *describe: 分选规则    验证规则 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:24:54
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 分选规则 更新 验证
    /// </summary>
    internal class ProcSortingRuleCreateValidator: AbstractValidator<ProcSortingRuleCreateDto>
    {
        public ProcSortingRuleCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 分选规则 修改 验证
    /// </summary>
    internal class ProcSortingRuleModifyValidator : AbstractValidator<ProcSortingRuleModifyDto>
    {
        public ProcSortingRuleModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
