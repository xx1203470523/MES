/*
 *creator: Karl
 *
 *describe: 操作面板按钮    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-04-01 02:58:19
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 操作面板按钮 更新 验证
    /// </summary>
    internal class ManuFacePlateButtonCreateValidator: AbstractValidator<ManuFacePlateButtonCreateDto>
    {
        public ManuFacePlateButtonCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 操作面板按钮 修改 验证
    /// </summary>
    internal class ManuFacePlateButtonModifyValidator : AbstractValidator<ManuFacePlateButtonModifyDto>
    {
        public ManuFacePlateButtonModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
