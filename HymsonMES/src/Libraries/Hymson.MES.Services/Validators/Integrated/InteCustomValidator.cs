/*
 *creator: Karl
 *
 *describe: 客户维护    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-07-11 09:33:26
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 客户维护 更新 验证
    /// </summary>
    internal class InteCustomCreateValidator: AbstractValidator<InteCustomCreateDto>
    {
        public InteCustomCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 客户维护 修改 验证
    /// </summary>
    internal class InteCustomModifyValidator : AbstractValidator<InteCustomModifyDto>
    {
        public InteCustomModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
