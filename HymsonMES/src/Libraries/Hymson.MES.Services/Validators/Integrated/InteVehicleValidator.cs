/*
 *creator: Karl
 *
 *describe: 载具注册表    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-07-14 10:03:53
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
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
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18604));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18605));
            RuleFor(x => x.Code).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18606));
            RuleFor(x => x.Name).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18607));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES18608));
            RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(EnableEnum), it)).WithErrorCode(nameof(ErrorCode.MES18609));
            RuleFor(x => x.Position).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18610));
            RuleFor(x => x.VehicleTypeId).Must(it => it>0).WithErrorCode(nameof(ErrorCode.MES18611));
        }
    }

    /// <summary>
    /// 载具注册表 修改 验证
    /// </summary>
    internal class InteVehicleModifyValidator : AbstractValidator<InteVehicleModifyDto>
    {
        public InteVehicleModifyValidator()
        {
            RuleFor(x => x.Id).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES18612));

            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            //RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18604));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18605));
            //RuleFor(x => x.Code).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18606));
            RuleFor(x => x.Name).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18607));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES18608));
            RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(EnableEnum), it)).WithErrorCode(nameof(ErrorCode.MES18609));
            RuleFor(x => x.Position).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18610));
            RuleFor(x => x.VehicleTypeId).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES18611));

        }
    }
}
