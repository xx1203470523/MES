/*
 *creator: Karl
 *
 *describe: 系统Token    验证规则 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-06-15 02:09:57
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 系统Token 更新 验证
    /// </summary>
    internal class InteSystemTokenCreateValidator: AbstractValidator<InteSystemTokenCreateDto>
    {
        public InteSystemTokenCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 系统Token 修改 验证
    /// </summary>
    internal class InteSystemTokenModifyValidator : AbstractValidator<InteSystemTokenModifyDto>
    {
        public InteSystemTokenModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
