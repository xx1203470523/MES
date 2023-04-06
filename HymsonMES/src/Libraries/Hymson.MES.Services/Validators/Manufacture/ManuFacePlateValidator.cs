/*
 *creator: Karl
 *
 *describe: 操作面板    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-04-01 02:05:24
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 操作面板 更新 验证
    /// </summary>
    internal class ManuFacePlateCreateValidator: AbstractValidator<ManuFacePlateCreateDto>
    {
        public ManuFacePlateCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 操作面板 修改 验证
    /// </summary>
    internal class ManuFacePlateModifyValidator : AbstractValidator<ManuFacePlateModifyDto>
    {
        public ManuFacePlateModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
