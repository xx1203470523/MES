/*
 *creator: Karl
 *
 *describe: 托盘条码关系    验证规则 | 代码由框架生成  
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:11:13
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 托盘条码关系 更新 验证
    /// </summary>
    internal class ManuTraySfcRelationCreateValidator: AbstractValidator<ManuTraySfcRelationCreateDto>
    {
        public ManuTraySfcRelationCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 托盘条码关系 修改 验证
    /// </summary>
    internal class ManuTraySfcRelationModifyValidator : AbstractValidator<ManuTraySfcRelationModifyDto>
    {
        public ManuTraySfcRelationModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
