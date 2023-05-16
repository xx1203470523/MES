/*
 *creator: Karl
 *
 *describe: 托盘信息    验证规则 | 代码由框架生成  
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 10:57:03
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 托盘信息 更新 验证
    /// </summary>
    internal class InteTrayCreateValidator: AbstractValidator<InteTrayCreateDto>
    {
        public InteTrayCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 托盘信息 修改 验证
    /// </summary>
    internal class InteTrayModifyValidator : AbstractValidator<InteTrayModifyDto>
    {
        public InteTrayModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
