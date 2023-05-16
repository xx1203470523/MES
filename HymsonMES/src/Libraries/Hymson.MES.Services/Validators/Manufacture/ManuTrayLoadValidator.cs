/*
 *creator: Karl
 *
 *describe: 托盘装载信息表    验证规则 | 代码由框架生成  
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:10:43
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 托盘装载信息表 更新 验证
    /// </summary>
    internal class ManuTrayLoadCreateValidator: AbstractValidator<ManuTrayLoadCreateDto>
    {
        public ManuTrayLoadCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 托盘装载信息表 修改 验证
    /// </summary>
    internal class ManuTrayLoadModifyValidator : AbstractValidator<ManuTrayLoadModifyDto>
    {
        public ManuTrayLoadModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
