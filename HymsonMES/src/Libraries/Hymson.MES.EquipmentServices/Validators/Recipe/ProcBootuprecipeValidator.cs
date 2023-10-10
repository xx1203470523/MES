/*
 *creator: Karl
 *
 *describe: 开机配方表    验证规则 | 代码由框架生成  
 *builder:  wxk
 *build datetime: 2023-07-05 04:11:36
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 开机配方表 更新 验证
    /// </summary>
    internal class ProcBootuprecipeCreateValidator: AbstractValidator<ProcBootuprecipeCreateDto>
    {
        public ProcBootuprecipeCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 开机配方表 修改 验证
    /// </summary>
    internal class ProcBootuprecipeModifyValidator : AbstractValidator<ProcBootuprecipeModifyDto>
    {
        public ProcBootuprecipeModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
