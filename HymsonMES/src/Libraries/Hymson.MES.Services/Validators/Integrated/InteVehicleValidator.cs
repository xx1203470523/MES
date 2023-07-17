/*
 *creator: Karl
 *
 *describe: 载具注册表    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-07-14 10:03:53
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 载具注册表 更新 验证
    /// </summary>
    internal class InteVehicleCreateValidator: AbstractValidator<InteVehicleCreateDto>
    {
        public InteVehicleCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

    /// <summary>
    /// 载具注册表 修改 验证
    /// </summary>
    internal class InteVehicleModifyValidator : AbstractValidator<InteVehicleModifyDto>
    {
        public InteVehicleModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }
}
