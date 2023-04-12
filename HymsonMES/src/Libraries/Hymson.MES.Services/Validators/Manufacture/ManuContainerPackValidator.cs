/*
 *creator: Karl
 *
 *describe: 容器装载表（物理删除）    验证规则 | 代码由框架生成  
 *builder:  wxk
 *build datetime: 2023-04-12 02:33:13
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 容器装载表（物理删除） 更新 验证
    /// </summary>
    internal class ManuContainerPackCreateValidator: AbstractValidator<ManuContainerPackCreateDto>
    {
        public ManuContainerPackCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 容器装载表（物理删除） 修改 验证
    /// </summary>
    internal class ManuContainerPackModifyValidator : AbstractValidator<ManuContainerPackModifyDto>
    {
        public ManuContainerPackModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
