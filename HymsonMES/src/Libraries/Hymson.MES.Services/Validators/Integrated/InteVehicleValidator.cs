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
            RuleFor(x => x.Code).Must(x => !x.Any(x => Char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES18618));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18605));
            RuleFor(x => x.Code).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18606));
            RuleFor(x => x.Name).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18607));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES18608));
            RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(DisableOrEnableEnum), it)).WithErrorCode(nameof(ErrorCode.MES18609));
            RuleFor(x => x.Position).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18610));
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
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18605));
            RuleFor(x => x.Name).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18607));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES18608));
            RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(DisableOrEnableEnum), it)).WithErrorCode(nameof(ErrorCode.MES18609));
            RuleFor(x => x.Position).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18610));

        }
    }


    internal class InteVehicletImportValidator : AbstractValidator<InteVehicleFreightImportDto>
    {
        public InteVehicletImportValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.VehicleTypeCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18604));
            RuleFor(x => x.VehicleTypeCode).Must(x => !x.Any(x => Char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES18618));
            RuleFor(x => x.VehicleTypeName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18605));
            RuleFor(x => x.VehicleTypeCode).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18606));
            RuleFor(x => x.VehicleTypeName).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18607));
            RuleFor(x => x.Describe).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES18408));
            RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(DisableOrEnableEnum), it)).WithErrorCode(nameof(ErrorCode.MES18609));
        }
    }
    /// <summary>
    /// 解盘校验
    /// </summary>
    internal class InteVehicleUnBindoptValidator : AbstractValidator<InteVehicleUnbindOperationDto>
    {
        public InteVehicleUnBindoptValidator()
        {
            RuleFor(x => x.LocationId).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES18620));

            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.PalletNo).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18604));
            RuleFor(x => x.StackIds).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18622));
            

        }
    }
    /// <summary>
    /// 绑盘校验
    /// </summary>
    internal class InteVehicleBindoptValidator : AbstractValidator<InteVehicleBindOperationDto>
    {
        public InteVehicleBindoptValidator()
        {
            RuleFor(x => x.LocationId).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES18620));

            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.PalletNo).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18604));
            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18621));


        }
    }
}
