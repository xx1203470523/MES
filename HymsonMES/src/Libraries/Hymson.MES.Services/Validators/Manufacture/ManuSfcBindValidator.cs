/*
 *creator: Karl
 *
 *describe: 条码绑定关系表    验证规则 | 代码由框架生成  
 *builder:  chenjianxiong
 *build datetime: 2023-05-17 10:09:11
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 条码绑定关系表 更新 验证
    /// </summary>
    internal class ManuSfcBindCreateValidator: AbstractValidator<ManuSfcBindCreateDto>
    {
        public ManuSfcBindCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 条码绑定关系表 修改 验证
    /// </summary>
    internal class ManuSfcBindModifyValidator : AbstractValidator<ManuSfcBindModifyDto>
    {
        public ManuSfcBindModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
