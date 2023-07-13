/*
 *creator: Karl
 *
 *describe: 载具类型维护    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-07-12 10:37:17
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 载具类型维护 更新 验证
    /// </summary>
    internal class InteVehicleTypeCreateValidator: AbstractValidator<InteVehicleTypeCreateDto>
    {
        public InteVehicleTypeCreateValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18503));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18504));
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES18505));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES18506));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES18507));
            RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(EnableEnum), it)).WithErrorCode(nameof(ErrorCode.MES18508));
            RuleFor(x => x.Row).Must(x => x>0 ).WithErrorCode(nameof(ErrorCode.MES18509));
            RuleFor(x => x.Arrange).Must(x => x>0 ).WithErrorCode(nameof(ErrorCode.MES18510));
            RuleFor(x => x.UnitNumber).Must(x => x>0 ).WithErrorCode(nameof(ErrorCode.MES18511));
        }
    }

    /// <summary>
    /// 载具类型维护 修改 验证
    /// </summary>
    internal class InteVehicleTypeModifyValidator : AbstractValidator<InteVehicleTypeModifyDto>
    {
        public InteVehicleTypeModifyValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Id).Must(x=>x>0).WithErrorCode(nameof(ErrorCode.MES18512));
            //RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18503));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18504));
            //RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES18505));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES18506));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES18507));
            RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(EnableEnum), it)).WithErrorCode(nameof(ErrorCode.MES18508));
            RuleFor(x => x.Row).Must(x => x > 0).WithErrorCode(nameof(ErrorCode.MES18509));
            RuleFor(x => x.Arrange).Must(x => x > 0).WithErrorCode(nameof(ErrorCode.MES18510));
            RuleFor(x => x.UnitNumber).Must(x => x > 0).WithErrorCode(nameof(ErrorCode.MES18511));
        }
    }
}
